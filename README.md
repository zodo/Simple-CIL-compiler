# Simple-CIL-compiler
*Simple compiler. TinyPG + RunSharp*

## Links
[TinyPG](http://www.codeproject.com/Articles/28294/a-Tiny-Parser-Generator-v)
[RunSharp](https://github.com/AqlaSolutions/runsharp)

## Grammar
```
// Terminals.
[IgnoreCase]GLOBAL -> @"global";
[IgnoreCase]END  	-> @"end";
[IgnoreCase]RETURN	-> @"return";
ARROW  -> @"=>";

[IgnoreCase]IF	-> @"if";
[IgnoreCase]ELSE	-> @"else";
[IgnoreCase]FOR	-> @"for";
[IgnoreCase]TO 	-> @"to";
[IgnoreCase]INCBY	-> @"incby";
[IgnoreCase]WHILE	-> @"while";
[IgnoreCase]DO	-> @"do";

[IgnoreCase]OR	-> @"or";
[IgnoreCase]AND	-> @"and";
[IgnoreCase]NOT	-> @"not"; 
[IgnoreCase]OPER	-> @"write|call";

PLUSMINUS-> @"\+|-";
MULTDIV	-> @"\*|/|\%\%|\%/";
COMP	-> @"=|\!=|\<\=|\<|\>=|\>";
POW	-> @"\^";
UNARYOP -> @"\+\+|--|\+|-";

COLON	-> @":";
QUESTION -> @"\?";
COMMA	-> @",";
ASSIGN	-> @"\=";

BROPEN		-> @"\(";
BRCLOSE	-> @"\)";
SQOPEN		-> @"\[";
SQCLOSE	-> @"\]";

STRING		-> @"@?\""(\""\""|[^\""])*\""";.
INTEGER	-> @"[0-9]+";
DOUBLE		-> @"[0-9]*\.[0-9]+";
[IgnoreCase]BOOL-> @"true|false";
[IgnoreCase]READFUNC-> @"readnum|readstr|call";

[IgnoreCase]IDENTIFIER-> @"[a-zA-Z_][a-zA-Z0-9_]*(?<!(^)(end|else|do|while|for|true|false|return|to|incby|global|or|and|not|write|readnum|readstr|call))(?!\w)";

NEWLINE	-> @"\s+";
EOF		-> @"^$";

[Skip]
WHITESPACE 	-> @"\s+";
[Skip]
[Style("comment")]
COMMENT	-> @"//[^\n]*\n?";
 
// Base non-terms.
Start		-> Program? EOF;
Program	-> Member (NEWLINE Member?)*;
Member		-> Globalvar | Function;
Globalvar	-> GLOBAL IDENTIFIER (ASSIGN Literal)?;
Function	-> IDENTIFIER (BROPEN Parameters BRCLOSE)? ((ARROW Expr) | Statements);
Parameters	-> IDENTIFIER (COMMA IDENTIFIER)*;
Statements	-> (Statement (NEWLINE Statement?)*)? END;
 
// Statements.
Statement	-> IfStm | WhileStm | DoStm | ForStm | ReturnStm | CallOrAssign | OperStm;
IfStm 		-> IF Expr Statements (ELSE Statements)?;
WhileStm 	-> WHILE Expr? Statements;
DoStm 		-> DO Statements WHILE Expr;
ForStm 	-> FOR CallOrAssign TO Expr (INCBY Expr)? Statements;
ReturnStm	-> RETURN Expr;
OperStm	-> OPER Call?;

// Calls or assigns.
CallOrAssign  -> Variable Assign?;
Assign		-> ASSIGN Expr;
Variable 	-> IDENTIFIER (Array | Call)?;
Array		-> SQOPEN Expr SQCLOSE;
Call		-> BROPEN Arguments? BRCLOSE;
Arguments	-> Expr (COMMA Expr)*;
Literal	-> INTEGER | DOUBLE | STRING | BOOL | READFUNC Call?;

 
// Expressions.
Expr		-> OrExpr (QUESTION Expr COLON Expr)?;
OrExpr 	-> AndExpr (OR AndExpr)*;
AndExpr	-> NotExpr (AND NotExpr)*;
NotExpr 	-> NOT? CompExpr;
CompExpr 	-> AddExpr (COMP AddExpr)?;
AddExpr 	-> MultExpr ((PLUSMINUS) MultExpr)*; 
MultExpr 	-> PowExpr ((MULTDIV) PowExpr)*;
PowExpr  	-> UnaryExpr (POW UnaryExpr)*;
UnaryExpr 	-> UNARYOP? Atom;
Atom		-> Literal | Variable | BROPEN Expr BRCLOSE;
```

## Sample program

```
isprime(x) 
    count = 0
    for n = 1 to x
        if x %% n = 0
        count = count + 1
        end
    end
    return count <= 2
end

main
    write("Input max number")
    max = readnum
    count = 0
    for i = 1 to max
        if isprime(i)
            write(i)
            count = count + 1
        end
    end
    write("{0} primes number between {1} and {2}", count, 0, max)
    
end
```
