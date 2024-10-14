grammar JSADSL;

file : topLevelDecl*;

topLevelDecl : funcDecl | objectDecl;

funcDecl : FUNC_KW name=ID L_PAREN (args) R_PAREN (COLON ID)? L_BRACE statementsBlock R_BRACE;

args : arg? (COMMA arg)* COMMA?;

arg : ID (COLON ID)?;

statementsBlock : (statement SEMI_COLON)*;

statement : ifStatement; // todo

varAssignmentStatement : VAR_KW ID EQ expression;

ifStatement : IF_KW L_PAREN expression R_PAREN statementsBlock L_BRACE ((ELSE_KW else_if=ifStatement) | (ELSE_KW else=statementsBlock));

expression : ID;

objectDecl : OBJECT_KW name=ID L_BRACE objectBody R_BRACE;

objectBody : funcDecl*;

FUNC_KW : 'func';
OBJECT_KW: 'object';
IF_KW : 'if';
ELSE_KW : 'else';
VAR_KW : 'var';

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
