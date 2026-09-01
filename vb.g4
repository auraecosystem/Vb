grammar Vb;

// Parser Rules

compilationUnit
    : optionStatement* importStatement* namespaceMemberDeclaration* EOF
    ;

optionStatement
    : 'Option' ( 'Explicit' | 'Strict' | 'Infer' ) ( 'On' | 'Off' ) NEWLINE
    ;

importStatement
    : 'Imports' qualifiedName NEWLINE
    ;

namespaceMemberDeclaration
    : moduleDeclaration
    | classDeclaration
    ;

moduleDeclaration
    : 'Module' IDENTIFIER NEWLINE memberDeclaration* 'End' 'Module' NEWLINE
    ;

classDeclaration
    : 'Class' IDENTIFIER NEWLINE memberDeclaration* 'End' 'Class' NEWLINE
    ;

memberDeclaration
    : subDeclaration
    | functionDeclaration
    | fieldDeclaration
    ;

subDeclaration
    : 'Sub' IDENTIFIER '(' parameterList? ')' NEWLINE statement* 'End' 'Sub' NEWLINE
    ;

functionDeclaration
    : 'Function' IDENTIFIER '(' parameterList? ')' 'As' typeRef NEWLINE statement* 'End' 'Function' NEWLINE
    ;

fieldDeclaration
    : 'Dim' IDENTIFIER 'As' typeRef NEWLINE
    ;

parameterList
    : parameter ( ',' parameter )*
    ;

parameter
    : IDENTIFIER 'As' typeRef
    ;

statement
    : assignmentStatement
    | expressionStatement
    | returnStatement
    ;

assignmentStatement
    : IDENTIFIER '=' expression NEWLINE
    ;

expressionStatement
    : expression NEWLINE
    ;

returnStatement
    : 'Return' expression? NEWLINE
    ;

expression
    : literal
    | IDENTIFIER
    | methodCall
    ;

methodCall
    : IDENTIFIER '(' expressionList? ')'
    ;

expressionList
    : expression ( ',' expression )*
    ;

typeRef
    : IDENTIFIER
    ;

qualifiedName
    : IDENTIFIER ( '.' IDENTIFIER )*
    ;

// Lexer Rules

IDENTIFIER
    : [a-zA-Z_] [a-zA-Z0-9_]*
    ;

INTEGER_LITERAL
    : [0-9]+
    ;

STRING_LITERAL
    : '"' (~[ "\r\n])* '"'
    ;

literal
    : INTEGER_LITERAL
    | STRING_LITERAL
    ;

NEWLINE
    : '\r'? '\n'
    | '_' [ \t]* '\r'? '\n' // Line continuation
    ;

WS
    : [ \t]+ -> skip
    ;

COMMENT
    : '\'' ~[\r\n]* -> skip
    ;
