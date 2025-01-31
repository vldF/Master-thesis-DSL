grammar JSADSL;

file : packageDecl? topLevelDecl*;

topLevelDecl
   :   funcDecl
   |   intrinsicFuncDecl
   |   objectDecl
   |   importDecl
   ;

funcDecl : FUNC_KW name=ID L_PAREN (functionArgs) R_PAREN (COLON resultType=ID)? L_BRACE statementsBlock R_BRACE;

intrinsicFuncDecl : INTRINSIC_KW FUNC_KW name=ID L_PAREN (functionArgs) R_PAREN (COLON resultType=ID)?;

functionArgs : functionArg? (COMMA functionArg)* COMMA?;

functionArg : name=ID (COLON type=ID)?;

statementsBlock : (statement)*;

statement
   :   ifStatement
   |   varAssignmentStatement SEMI_COLON
   |   varDeclarationStatement SEMI_COLON
   |   returnStatement SEMI_COLON
   |   functionCall SEMI_COLON
   ;

varAssignmentStatement : varName=ID EQ expression;

// todo: make the type optional
varDeclarationStatement : VAR_KW varName=ID (COLON type=ID) (EQ initValue=expression)?;

ifStatement : IF_KW L_PAREN cond=expression R_PAREN L_BRACE mainBlock=statementsBlock R_BRACE ((ELSE_KW elseIfStatement=ifStatement)|(ELSE_KW L_BRACE elseBlock=statementsBlock R_BRACE))?;

returnStatement : RETURN_KW expression?;

expressionList : expression? (COMMA expression)* COMMA?;

variableExpression : name=ID;

newExpression : NEW_KW name=ID L_PAREN expressionList R_PAREN;

expression
   :   L_PAREN expr_in_paren=expression R_PAREN
   |   left=expression op=(ASTERISK | SLASH) right=expression
   |   left=expression op=PERCENT right=expression
   |   left=expression op=(PLUS | MINUS) right=expression
   |   op=MINUS unary=expression
   |   op=EXCLAMATION unary=expression
   |   left=expression op=(EQEQ | NOT_EQ | LESS_EQ | LESS | GREAT_EQ | GREAT) right=expression
   |   left=expression op=(AND | OR | XOR) right=expression
   |   atomic=expressionAtomic
   ;

expressionAtomic
   :   primitiveLiteral
   |   newExpression
   |   variableExpression
   |   functionCall
   ;

primitiveLiteral
   :   integerNumberLiteral
   |   floatNumberLiteral
   |   DoubleQuotedString
   |   boolLiteral
   ;

integerNumberLiteral
   :   MINUS? Digit+
   |   Digit
   ;

floatNumberLiteral
   :   MINUS? Digit+ DOT Digit+
   ;

boolLiteral
   :   bool=(TRUE_KW | FALSE_KW)
   ;

functionCall
   :   name=ID L_PAREN args=expressionList R_PAREN
   ;

objectDecl : OBJECT_KW name=ID L_BRACE objectBody R_BRACE;

objectBody : funcDecl*;

importDecl : IMPORT_KW package=DoubleQuotedString SEMI_COLON;
packageDecl : PACKAGE_KW package=DoubleQuotedString SEMI_COLON;

INTRINSIC_KW : 'intrinsic';
FUNC_KW : 'func';
OBJECT_KW: 'object';
IF_KW : 'if';
ELSE_KW : 'else';
VAR_KW : 'var';
RETURN_KW : 'return';
NEW_KW : 'new';
TRUE_KW : 'true';
FALSE_KW : 'false';
PACKAGE_KW : 'package';
IMPORT_KW : 'import';

L_PAREN : '(';
R_PAREN : ')';
L_BRACE : '{';
R_BRACE : '}';
COLON   : ':';
COMMA   : ',';
SEMI_COLON   : ';';

ASTERISK : '*';
SLASH : '/';
PERCENT : '%';
PLUS : '+';
MINUS : '-';
EXCLAMATION : '!';
EQ : '=';
EQEQ : '==';
NOT_EQ : '!=';
LESS_EQ : '<=';
LESS : '<';
GREAT_EQ : '>=';
GREAT : '>';
AND : '&&';
OR : '||';
XOR : '^';
DOT : '.';


ID : VALID_ID_START VALID_ID_CHAR*;
fragment VALID_ID_START : ('a' .. 'z') | ('A' .. 'Z') | '_' ;
fragment VALID_ID_CHAR : VALID_ID_START | ('0' .. '9') ;
Digit : ('0'..'9');

WS : [ \r\n\t] + -> channel(HIDDEN) ;

COMMENT
   :   '/*' .*? '*/' -> channel(HIDDEN)
   ;

LINE_COMMENT
   :   (' //' ~[\r\n]* | '// ' ~[\r\n]*) -> channel(HIDDEN)
   ;

DoubleQuotedString
   :   '"' .*? '"'
   ;
