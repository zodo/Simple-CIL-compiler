using System.Linq;

namespace Language.Semantic
{
    using System;
    using System.Collections.Generic;

    using AST;
    using AST.Expressions;

    using Data;

    using TinyPG;

    class AstGenerationNode : ParseNode
    {
        public AstGenerationNode(Token token, string text)
            : base(token, text)
        {
        }

        public override ParseNode CreateNode(Token token, string text)
        {
            ParseNode node = new AstGenerationNode(token, text);
            node.Parent = this;
            return node;
        }

        private ParseNode GetNode(TokenType type)
        {
            return nodes.OfTokenType(type).FirstOrDefault();
        }

        /// <summary>
        /// Rule: Start -> (Program)? EOF ;
        /// </summary>
        protected override object EvalStart(ParseTree tree, params object[] paramlist)
        {
            Namespaces.Reset();
            AstCreator.Reset();

            var program = GetNode(TokenType.Program);
            program?.Eval(tree);

            return null;
        }

        /// <summary>
        /// Rule: Program -> Member (NEWLINE (Member)? )* ;
        /// </summary>
        protected override object EvalProgram(ParseTree tree, params object[] paramlist)
        {
            foreach (var parseNode in nodes.OfTokenType(TokenType.Member))
            {
                Namespaces.Current = Namespaces.Root;
                parseNode.Eval(tree);
            }
            return null;
        }

        /// <summary>
        /// Rule: Member -> (Globalvar | Function);
        /// </summary>
        protected override object EvalMember(ParseTree tree, params object[] paramlist)
        {
            nodes.First().Eval(tree);

            return null;
        }

        /// <summary>
        /// Rule: Globalvar -> GLOBAL IDENTIFIER (ASSIGN Literal )? ;
        /// </summary>
        protected override object EvalGlobalvar(ParseTree tree, params object[] paramlist)
        {
            var identNode = GetNode(TokenType.IDENTIFIER);
            
            var literalNode = GetNode(TokenType.Literal);

            var symbolType = ((LiteralExpr)literalNode?.Eval(tree))?.SymbolType ?? SymbolType.Unknown;

            var symbol = new Symbol(symbolType, "", identNode.Token.Text);

            AstCreator.GlobalVariables.Add(new GlobalVariable
            {
                Namespace = Namespaces.Current,
                Value = symbol
            });
            Namespaces.Current.AddSymbol(symbol, identNode);

            return null;
        }

        /// <summary>
        /// Rule: Function -> IDENTIFIER (BROPEN Parameters BRCLOSE )? (ARROW Expr  | Statements) ;
        /// </summary>
        protected override object EvalFunction(ParseTree tree, params object[] paramlist)
        {
            var funcName = GetNode(TokenType.IDENTIFIER).Token.Text;
            var arguments = (List<ParseNode>)GetNode(TokenType.Parameters)?.Eval(tree) ?? new List<ParseNode>();

            if (funcName == "main")
            {
                if (arguments.Count > 0)
                {
                    throw new ParseException {Error = new ParseError("Main не может иметь аргументов", 0, GetNode(TokenType.IDENTIFIER)) };
                }
                if (AstCreator.FuncImplementation.Count == 0)
                {
                    AstCreator.FuncImplementation.Push(new FuncImplementation());
                    Namespaces.LevelDown(new Namespace(funcName));

                    GetNode(TokenType.Statements)?.Eval(tree);

                    Namespaces.LevelUp();
                    AstCreator.FuncImplementations.Add(AstCreator.FuncImplementation.Pop());
                }
                else
                {
                    throw new ParseException { Error = new ParseError("нельзя вызвать main", 0, GetNode(TokenType.IDENTIFIER))};
                }
            }
            else if (AstCreator.FuncImplementation.Count > 0)
            {
                var exprNode = GetNode(TokenType.Expr);
                if (exprNode != null)
                {
                    var expr = (Expression)exprNode.Eval(tree);
                    AstCreator.FuncImplementation.Peek().ReturnType = expr.GetExprType();
                }
                else
                {
                    GetNode(TokenType.Statements)?.Eval(tree);
                }
            }
            else
            {
                // Если функция не объявлялась.
                if (!AstCreator.FuncDeclarations.Any(f => f.Name == funcName && f.ArgumentsAmount == arguments.Count))
                {
                    AstCreator.FuncDeclarations.Add(
                        new FuncDeclaration
                        {
                            Name = funcName,
                            FunctionNode = this,
                            ParseTree = tree,
                            Arguments = arguments.Select(x => x.Token.Text).ToList()
                        });
                    Namespaces.Current.AddSymbol(new Symbol(SymbolType.Function, "", funcName), GetNode(TokenType.IDENTIFIER));
                }
                else
                {
                    throw new ParseException
                    {
                        Error = new ParseError(
                            $"Повтороное объявление функции {funcName}", 0, GetNode(TokenType.IDENTIFIER))
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Rule: Parameters -> IDENTIFIER (COMMA IDENTIFIER )* ;
        /// </summary>
        protected override object EvalParameters(ParseTree tree, params object[] paramlist)
        {
            return nodes.OfTokenType(TokenType.IDENTIFIER).ToList();
        }

        /// <summary>
        /// Rule: Statements -> (Statement (NEWLINE (Statement)? )* )? END ;
        /// </summary>
        protected override object EvalStatements(ParseTree tree, params object[] paramlist)
        {
            Namespaces.LevelDown(new Namespace());
            foreach (var parseNode in nodes.OfTokenType(TokenType.Statement))
            {
                parseNode.Eval(tree);
            }
            Namespaces.LevelUp();
            if (!Namespaces.Current.Children.Last().SymbolsSameLevel.Any())
            {
                Namespaces.Current.Children.RemoveAt(Namespaces.Current.Children.Count - 1);
            }
            return null;
        }

        /// <summary>
        /// Rule: Statement -> (IfStm | WhileStm | DoStm | ForStm | ReturnStm | CallOrAssign | OperStm);
        /// </summary>
        protected override object EvalStatement(ParseTree tree, params object[] paramlist)
        {
            nodes.FirstOrDefault()?.Eval(tree);
            return null;
        }

        /// <summary>
        /// Rule: IfStm -> IF Expr Statements (ELSE Statements )? ;
        /// </summary>
        protected override object EvalIfStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalIfStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: WhileStm -> WHILE (Expr)? Statements ;
        /// </summary>
        protected override object EvalWhileStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalWhileStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: DoStm -> DO Statements WHILE Expr ;
        /// </summary>
        protected override object EvalDoStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalDoStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: ForStm -> FOR Variable Assign TO Expr (INCBY Expr )? Statements ;
        /// </summary>
        protected override object EvalForStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalForStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: ReturnStm -> RETURN (Expr)? ;
        /// </summary>
        protected override object EvalReturnStm(ParseTree tree, params object[] paramlist)
        {
            var exprNode = GetNode(TokenType.Expr);
            var returnType = SymbolType.Void;
            var funcImpl = AstCreator.FuncImplementation.Peek();
            if (exprNode != null)
            {
                var expr = (Expression)exprNode.Eval(tree);
                returnType = expr.GetExprType();
            }
            if (funcImpl.ReturnType != SymbolType.Unknown && funcImpl.ReturnType != returnType)
            {
                throw new ParseException {Error = new ParseError("Возвращаемые значения должны быть одинакового типа", 0, GetNode(TokenType.RETURN))};
            }
            AstCreator.FuncImplementation.Peek().ReturnType = returnType;
            
            return null;
        }

        /// <summary>
        /// Rule: OperStm -> OPER (Call)? ;
        /// </summary>
        protected override object EvalOperStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalOperStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: CallOrAssign -> Variable (Assign)? ;
        /// </summary>
        protected override object EvalCallOrAssign(ParseTree tree, params object[] paramlist)
        {
            var assignNode = GetNode(TokenType.Assign);
            var variable = (Variable)GetNode(TokenType.Variable).Eval(tree, true);
            if (assignNode != null)
            {
                if (variable.Type == VariableType.Call)
                {
                    throw new ParseException
                    {
                        Error = new ParseError("Попытка присвоить значение вызову функции", 0x009, assignNode)
                    };
                }
                else
                {
                    var symbol = Namespaces.Current.Symbols.SingleOrDefault(x => x.Name == variable.Name);
                    var expr = (Expression)assignNode.Eval(tree);
                    if (symbol != null)
                    {

                        if (symbol.Type == SymbolType.Unknown)
                        {
                            symbol.Type = expr.GetExprType();
                        }
                        else
                        {
                            if (symbol.Type != expr.GetExprType())
                            {
                                throw new ParseException
                                {
                                    Error = new ParseError("Невозможно изменить тип переменной", 0x009, assignNode)
                                };
                            }
                        }
                    }
                    else
                    {
                        Namespaces.Current.AddSymbol(
                            new Symbol(expr.GetExprType(), "", variable.Name),
                            GetNode(TokenType.Variable));
                    }
                }
            }
            else
            {
                if (variable.Type == VariableType.Simple || variable.Type == VariableType.Array)
                {
                    throw new ParseException {Error = new ParseError("Отсутствует правая часть выражения", 0, GetNode(TokenType.Variable))};
                }
            }

            return null;
        }

        /// <summary>
        /// Rule: Assign -> ASSIGN Expr ;
        /// </summary>
        protected override object EvalAssign(ParseTree tree, params object[] paramlist)
        {
            return (Expression)GetNode(TokenType.Expr).Eval(tree);
        }

        /// <summary>
        /// Rule: Variable -> IDENTIFIER ((Array | Call))? ;
        /// </summary>
        protected override object EvalVariable(ParseTree tree, params object[] paramlist)
        {
            var identNode = GetNode(TokenType.IDENTIFIER);
            var identName = identNode.Token.Text;
            if (GetNode(TokenType.Array) == null && GetNode(TokenType.Call) == null)
            {
                if (paramlist != null && paramlist.Any() && (bool)paramlist.First())
                {
                    return new Variable { Name = identName, Namespace = Namespaces.Current, Type = VariableType.Simple };
                }
                var existingSymbol = Namespaces.Current.Symbols.SingleOrDefault(x => x.Name == identName);
                if (existingSymbol != null)
                {
                    return new LiteralExpr {SymbolType = existingSymbol.Type, Namespace = Namespaces.Current};
                }
                throw new ParseException { Error = new ParseError($"Использование непроинициализированной переменной {identName}", 0, identNode)};
            }

            if (GetNode(TokenType.Call) != null)
            {
                var arguments = (List<SymbolType>)GetNode(TokenType.Call).Eval(tree);
                var declaration =
                    AstCreator.FuncDeclarations.SingleOrDefault(
                        x => x.Name == identName && x.ArgumentsAmount == arguments.Count);
                if (declaration == null)
                {
                    throw new ParseException { Error = new ParseError($"Попытка использовать необъявленную функцию {identName}", 0, identNode)};
                }
                else
                {
                    if (AstCreator.FuncImplementation.Count > 0)
                    {
                        if (AstCreator.FuncImplementation.Any(x => x.Name == identName))
                        {
                            throw new ParseException {Error = new ParseError("Рекурсия не поддерживается", 0, identNode)};
                        }
                    }
                    AstCreator.FuncImplementation.Push(new FuncImplementation {Name = identName});
                    var node = declaration.FunctionNode;
                    var currentNamespace = Namespaces.Current;
                    Namespaces.Current = Namespaces.Root;
                    Namespaces.LevelDown(new Namespace($"{identName}_{string.Join("_", arguments.Select(x => x.ToString()))}"));
                    for (int index = 0; index < arguments.Count; index++)
                    {
                        var symbolType = arguments[index];
                        var symbol = new Symbol(symbolType, "", declaration.Arguments[index]);
                        Namespaces.Current.AddSymbol(symbol, identNode);
                        AstCreator.FuncImplementation.Peek().Arguments.Add(symbol);
                        
                    }
                    node.Eval(tree);
                    Namespaces.Current = currentNamespace;
                    AstCreator.FuncImplementations.Add(AstCreator.FuncImplementation.Pop());

                    if (paramlist != null && paramlist.Any() && (bool)paramlist.First())
                    {
                        return new Variable {Name = identName, Type = VariableType.Call};
                    }

                    return new LiteralExpr { SymbolType = AstCreator.FuncImplementations.Last().ReturnType};
                }



            }
            return null;

        }

        /// <summary>
        /// Rule: Array -> SQOPEN Expr SQCLOSE ;
        /// </summary>
        protected override object EvalArray(ParseTree tree, params object[] paramlist)
        {
            return base.EvalArray(tree, paramlist);
        }

        /// <summary>
        /// Rule: Call -> BROPEN (Arguments)? BRCLOSE ;
        /// </summary>
        protected override object EvalCall(ParseTree tree, params object[] paramlist)
        {
            return GetNode(TokenType.Arguments) == null ? new List<SymbolType>() : GetNode(TokenType.Arguments).Eval(tree);
        }

        /// <summary>
        /// Rule: Arguments -> Expr (COMMA Expr )* ;
        /// </summary>
        protected override object EvalArguments(ParseTree tree, params object[] paramlist)
        {
            return nodes.OfTokenType(TokenType.Expr).Select(x => x.Eval(tree)).Cast<Expression>().Select(x => x.GetExprType()).ToList();
        }

        /// <summary>
        /// Rule: Literal -> (INTEGER | DOUBLE | STRING | BOOL | READFUNC);
        /// </summary>
        protected override object EvalLiteral(ParseTree tree, params object[] paramlist)
        {
            // Определить тип, вернуть экземпляр Symbol
            var nodei = GetNode(TokenType.INTEGER);
            if (nodei != null)
            {
                return new LiteralExpr {SymbolType = SymbolType.Integer, Namespace = Namespaces.Current};
            }

            var noded = GetNode(TokenType.DOUBLE);
            if (noded != null)
            {
                return new LiteralExpr {SymbolType = SymbolType.Double, Namespace = Namespaces.Current};
            }

            var nodestr = GetNode(TokenType.STRING);
            if (nodestr != null)
            {
                return new LiteralExpr {SymbolType = SymbolType.String, Namespace = Namespaces.Current};
            }

            var nodeb = GetNode(TokenType.BOOL);
            if (nodeb != null)
            {
                return new LiteralExpr {SymbolType = SymbolType.Bool, Namespace = Namespaces.Current};
            }

            var nodefunc = GetNode(TokenType.READFUNC);
            if (nodefunc != null)
            {
                var functext = nodefunc.Token.Text;
                switch (functext)
                {
                    case "readnum":
                        return new LiteralExpr {SymbolType = SymbolType.Integer, Namespace = Namespaces.Current};
                    case "readstr":
                        return new LiteralExpr {SymbolType = SymbolType.String, Namespace = Namespaces.Current};
                }
            }
            return null;
        }

        /// <summary>
        /// Rule: Expr -> OrExpr (QUESTION Expr COLON Expr )? ;
        /// </summary>
        protected override object EvalExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.QUESTION) == null)
            {
                return (Expression)GetNode(TokenType.OrExpr).Eval(tree);
            }
            var ternaryExpr = new TernaryExpr
            {
                First = (Expression)GetNode(TokenType.OrExpr).Eval(tree),
                Second = (Expression)nodes.OfTokenType(TokenType.Expr).First().Eval(tree),
                Third = (Expression)nodes.OfTokenType(TokenType.Expr).Last().Eval(tree),
                Namespace = Namespaces.Current
            };
            return ternaryExpr;
        }

        /// <summary>
        /// Rule: OrExpr -> AndExpr (OR AndExpr )* ;
        /// </summary>
        protected override object EvalOrExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.OR) == null)
            {
                return (Expression)GetNode(TokenType.AndExpr).Eval(tree);
            }

            var expressions = new Stack<Expression>(nodes.OfTokenType(TokenType.AndExpr).Select(n => n.Eval(tree)).Cast<Expression>());

            while (expressions.Count > 1)
            {
                var rightExpr = expressions.Pop();
                var leftExpr = expressions.Pop();
                var newExpr = new BoolExpr
                {
                    First = leftExpr,
                    Second = rightExpr,
                    Namespace = Namespaces.Current
                };
                expressions.Push(newExpr);
            }

            return expressions.First();
        }

        /// <summary>
        /// Rule: AndExpr -> NotExpr (AND NotExpr )* ;
        /// </summary>
        protected override object EvalAndExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.AND) == null)
            {
                return (Expression)GetNode(TokenType.NotExpr).Eval(tree);
            }

            var expressions = new Stack<Expression>(nodes.OfTokenType(TokenType.NotExpr).Select(n => n.Eval(tree)).Cast<Expression>());

            while (expressions.Count > 1)
            {
                var rightExpr = expressions.Pop();
                var leftExpr = expressions.Pop();
                var newExpr = new BoolExpr
                {
                    First = leftExpr,
                    Second = rightExpr,
                    Namespace = Namespaces.Current
                };
                expressions.Push(newExpr);
            }

            return expressions.First();
        }

        /// <summary>
        /// Rule: NotExpr -> (NOT)? CompExpr ;
        /// </summary>
        protected override object EvalNotExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.NOT) == null)
            {
                return (Expression)GetNode(TokenType.CompExpr).Eval(tree);
            }
            var unaryExpr = new MultPowUnaryExpr { First = (Expression)GetNode(TokenType.CompExpr).Eval(tree), Namespace = Namespaces.Current};

            return unaryExpr;
        }

        /// <summary>
        /// Rule: CompExpr -> AddExpr (COMP AddExpr )? ;
        /// </summary>
        protected override object EvalCompExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.COMP) == null)
            {
                return (Expression)GetNode(TokenType.AddExpr).Eval(tree);
            }

            var boolExpr = new BoolExpr
            {
                First = (Expression)nodes.OfTokenType(TokenType.AddExpr).First().Eval(tree),
                Second = (Expression)nodes.OfTokenType(TokenType.AddExpr).Last().Eval(tree),
                Namespace = Namespaces.Current
            };

            return boolExpr;
        }

        /// <summary>
        /// Rule: AddExpr -> MultExpr ((PLUSMINUS) MultExpr )* ;
        /// </summary>
        protected override object EvalAddExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.PLUSMINUS) == null)
            {
                return (Expression)GetNode(TokenType.MultExpr).Eval(tree);
            }

            var expressions = new Stack<Expression>(nodes.OfTokenType(TokenType.MultExpr).Select(n => n.Eval(tree)).Cast<Expression>());

            while (expressions.Count > 1)
            {
                var rightExpr = expressions.Pop();
                var leftExpr = expressions.Pop();
                var newExpr = new AddExpr
                {
                    First = leftExpr,
                    Second = rightExpr,
                    Namespace = Namespaces.Current
                };
                expressions.Push(newExpr);
            }

            return expressions.First();
        }

        /// <summary>
        /// Rule: MultExpr -> PowExpr ((MULTDIV) PowExpr )* ;
        /// </summary>
        protected override object EvalMultExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.MULTDIV) == null)
            {
                return (Expression)GetNode(TokenType.PowExpr).Eval(tree);
            }

            var expressions = new Stack<Expression>(nodes.OfTokenType(TokenType.PowExpr).Select(n => n.Eval(tree)).Cast<Expression>());

            while (expressions.Count > 1)
            {
                var rightExpr = expressions.Pop();
                var leftExpr = expressions.Pop();
                var newExpr = new MultPowUnaryExpr
                {
                    First = leftExpr,
                    Second = rightExpr,
                    Namespace = Namespaces.Current
                };
                expressions.Push(newExpr);
            }

            return expressions.First();
        }

       

        /// <summary>
        /// Rule: PowExpr -> UnaryExpr (POW UnaryExpr )* ;
        /// </summary>
        protected override object EvalPowExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.POW) == null)
            {
                return (Expression)GetNode(TokenType.UnaryExpr).Eval(tree);
            }

            var expressions = new Stack<Expression>(nodes.OfTokenType(TokenType.UnaryExpr).Select(n => n.Eval(tree)).Cast<Expression>());

            while (expressions.Count > 1)
            {
                var rightExpr = expressions.Pop();
                var leftExpr = expressions.Pop();
                var newExpr = new MultPowUnaryExpr
                {
                    First = leftExpr,
                    Second = rightExpr,
                    Namespace = Namespaces.Current
                };
                expressions.Push(newExpr);
            }

            return expressions.First();
        }

        /// <summary>
        /// Rule: UnaryExpr -> ((UNARYOP))? Atom ;
        /// </summary>
        protected override object EvalUnaryExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.UNARYOP) == null)
            {
                return (Expression)GetNode(TokenType.Atom).Eval(tree);
            }

            var unaryExpr = new MultPowUnaryExpr
            {
                First = (Expression)GetNode(TokenType.Atom).Eval(tree),
                Namespace = Namespaces.Current
            };
            
            return unaryExpr;
        }

        /// <summary>
        /// Rule: Atom -> (Literal | Variable | BROPEN Expr BRCLOSE );
        /// </summary>
        protected override object EvalAtom(ParseTree tree, params object[] paramlist)
        {
            var literalNode = GetNode(TokenType.Literal);
            if (literalNode != null)
            {
                return ((Expression)literalNode.Eval(tree));
            }

            var variableNode = GetNode(TokenType.Variable);
            if (variableNode != null)
            {
                return (Expression)variableNode.Eval(tree);
            }

            var exprNode = GetNode(TokenType.Expr);
            return ((Expression)exprNode.Eval(tree));
            
        }

        private static object GetExprType(List<SymbolType> types)
        {
            if (types.Contains(SymbolType.Unknown))
            {
                return SymbolType.Unknown;
            }

            if (types.Contains(SymbolType.String))
            {
                return SymbolType.String;
            }

            if (types.Contains(SymbolType.Double))
            {
                return SymbolType.Double;
            }

            if (types.Contains(SymbolType.Integer))
            {
                return SymbolType.Integer;
            }

            return SymbolType.Bool;
        }
    }
}
