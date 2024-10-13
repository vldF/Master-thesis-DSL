grammar JSADSL;

file : topLevelDecl*;

topLevelDecl : funcDecl | objectDecl;

// func ID ([arg1: [type1]], ...) [: retType] { statement* }
funcDecl : FUNC_KW name=ID L_PAREN (args) R_PAREN (COLON ID)? L_BRACE funcBody R_BRACE;

args : arg? (COMMA arg)* COMMA?;

arg : ID (COLON ID)?;

funcBody : (statement SEMI_COLON)*;

statement : 'expr'; // todo

objectDecl : OBJECT_KW name=ID L_BRACE objectBody R_BRACE;

objectBody : funcDecl*;

FUNC_KW : 'func';
OBJECT_KW: 'object';
L_PAREN : '(';
R_PAREN : ')';
L_BRACE : '{';
R_BRACE : '}';
COLON   : ':';
COMMA   : ',';
SEMI_COLON   : ';';
ID : VALID_ID_START VALID_ID_CHAR*;
fragment VALID_ID_START : ('a' .. 'z') | ('A' .. 'Z') | '_' ;
fragment VALID_ID_CHAR : VALID_ID_START | ('0' .. '9') ;

WS : [ \r\n\t] + -> channel(HIDDEN) ;
