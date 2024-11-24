grammar JSADSL;

file : topLevelDecl*;

topLevelDecl : funcDecl | objectDecl;

funcDecl : FUNC_KW name=ID L_PAREN (functionArgs) R_PAREN (COLON resultType=ID)? L_BRACE statementsBlock R_BRACE;

functionArgs : functionArg? (COMMA functionArg)* COMMA?;

functionArg : name=ID (COLON type=ID)?;

statementsBlock : (statement SEMI_COLON)*;

statement :
    ifStatement
    | varAssignmentStatement
    | varDeclarationStatement
    | returnStatement
    ;

varAssignmentStatement : varName=ID EQ expression;

// todo: make the type optional
varDeclarationStatement : VAR_KW varName=ID (COLON type=ID) (EQ initValue=expression)?;

ifStatement : IF_KW L_PAREN cond=expression R_PAREN mainBlock=statementsBlock L_BRACE ((ELSE_KW else_if=ifStatement) | (ELSE_KW else=statementsBlock));

returnStatement : RETURN_KW expression?;

expression :
    variableExpression
    | newExpression
    ;

expressionList : expression? (COMMA expression)* COMMA?;

variableExpression : name=ID;

newExpression : NEW_KW name=ID L_PAREN expressionList R_PAREN;

objectDecl : OBJECT_KW name=ID L_BRACE objectBody R_BRACE;

objectBody : funcDecl*;

FUNC_KW : 'func';
OBJECT_KW: 'object';
IF_KW : 'if';
ELSE_KW : 'else';
VAR_KW : 'var';
RETURN_KW : 'return';
NEW_KW : 'new';

L_PAREN : '(';
R_PAREN : ')';
L_BRACE : '{';
R_BRACE : '}';
COLON   : ':';
COMMA   : ',';
SEMI_COLON   : ';';
EQ : '=';
ID : VALID_ID_START VALID_ID_CHAR*;
fragment VALID_ID_START : ('a' .. 'z') | ('A' .. 'Z') | '_' ;
fragment VALID_ID_CHAR : VALID_ID_START | ('0' .. '9') ;

WS : [ \r\n\t] + -> channel(HIDDEN) ;
