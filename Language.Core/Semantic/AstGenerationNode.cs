namespace Language.Semantic
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using AST;
    using AST.Expressions;
    using AST.LeftExprSide;

    using Data;

    using Language.AST;
    using Language.AST.Expressions;
    using Language.AST.Statements;

    using TinyPG;


    class AstGenerationNode : ParseNode
    {
        public AstGenerationNode(Token token, string text)
            : base(token, text)
        {
        }

        public override ParseNode CreateNode(Token token, string txt)
        {
            ParseNode node = new AstGenerationNode(token, txt);
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

            var symbol = new Symbol(symbolType, identNode.Token.Text);

            AstCreator.GlobalVariables.Add(new GlobalVariable
            {
                Value = symbol,
                Node = this
            });
            Namespaces.Current.AddSymbol(symbol, identNode);

            return null;
        }

        /// <summary>
        /// Rule: Function -> IDENTIFIER (BROPEN Parameters BRCLOSE )? (ARROW Expr  | Statements) ;
        /// </summary>
        protected override object EvalFunction(ParseTree tree, params object[] paramlist)
        {
            var functionName = GetNode(TokenType.IDENTIFIER).Token.Text;
            var parameters = (List<string>)GetNode(TokenType.Parameters)?.Eval(tree) ?? new List<string>();
            bool isCalled = false;
            ParseNode callPlace = null;

            if (paramlist != null && paramlist.Any())
            {
                isCalled = (bool)paramlist[0];
                callPlace = (ParseNode)paramlist[1];
            }
            if (functionName == "main")
            {
                if (isCalled)
                {
                    throw new ParseException("Нельзя вызвать main", callPlace??GetNode(TokenType.IDENTIFIER));
                }
                if (parameters.Count > 0)
                {
                    throw new ParseException("Main не может иметь аргументов", GetNode(TokenType.IDENTIFIER));
                }
                if (AstCreator.FuncImplementations.Any(x => x.Name == "main"))
                {
                    throw new ParseException("Повторное обявление main", GetNode(TokenType.IDENTIFIER));
                }
                var implementation = new FuncImplementation { Node = this, Name = "main" };
                AstCreator.FuncImplementation.Push(implementation);
                Namespaces.LevelDown(new Namespace(functionName));

                var codeblockNode = GetNode(TokenType.Statements);
                if (codeblockNode == null)
                {
                    throw new ParseException("Main должен иметь тело", GetNode(TokenType.IDENTIFIER));
                }
                var stms = (CodeBlock)codeblockNode.Eval(tree);
                implementation.Code = stms;

                Namespaces.LevelUp();
                AstCreator.FuncImplementations.Add(AstCreator.FuncImplementation.Pop());

            }
            else
            {
                if (isCalled)
                {
                    var exprNode = GetNode(TokenType.Expr);
                    var funcImpl = AstCreator.FuncImplementation.Peek();
                    funcImpl.Node = this;
                    if (exprNode != null)
                    {
                        var expr = (ExpressionBase)exprNode.Eval(tree);
                        funcImpl.ReturnExpression = expr;
                        funcImpl.ReturnType = expr.GetExprType();
                    }
                    else
                    {
                        var stms = (CodeBlock)GetNode(TokenType.Statements).Eval(tree);
                        funcImpl.Code = stms;
                    }
                }
                else
                {
                    if (AstCreator.FuncDeclarations.Any(f => f.Name == functionName && f.Arguments.Count == parameters.Count))
                    {
                        throw new ParseException($"Повтороное объявление функции {functionName}", GetNode(TokenType.IDENTIFIER));
                    }
                    AstCreator.FuncDeclarations.Add(
                        new FuncDeclaration { Name = functionName, Node = this, Arguments = parameters });

                    Namespaces.Current.AddSymbol(
                        new Symbol(SymbolType.Function, functionName),
                        GetNode(TokenType.IDENTIFIER));
                }
            }
            return null;
        }

        /// <summary>
        /// Rule: Parameters -> IDENTIFIER (COMMA IDENTIFIER )* ;
        /// </summary>
        protected override object EvalParameters(ParseTree tree, params object[] paramlist)
        {
            return nodes.OfTokenType(TokenType.IDENTIFIER).Select(x => x.Token.Text).ToList();
        }

        /// <summary>
        /// Rule: Statements -> (Statement (NEWLINE (Statement)? )* )? END ;
        /// </summary>
        protected override object EvalStatements(ParseTree tree, params object[] paramlist)
        {
            Namespaces.LevelDown(new Namespace());
            var statements = new CodeBlock
            {
                Statements = nodes.OfTokenType(TokenType.Statement).Select(x => x.Eval(tree)).Cast<StatementBase>().ToList(),
                Node = this
            };
            Namespaces.LevelUp();
            if (!Namespaces.Current.Children.Last().SymbolsSameLevel.Any() && !Namespaces.Current.Children.Last().Children.Any())
            {
                Namespaces.Current.Children.RemoveAt(Namespaces.Current.Children.Count - 1);
            }
            return statements;
        }

        /// <summary>
        /// Rule: Statement -> (IfStm | WhileStm | DoStm | ForStm | ReturnStm | CallOrAssign | OperStm);
        /// </summary>
        protected override object EvalStatement(ParseTree tree, params object[] paramlist)
        {
            return nodes.First().Eval(tree);
        }

        /// <summary>
        /// Rule: IfStm -> IF Expr Statements (ELSE Statements )? ;
        /// </summary>
        protected override object EvalIfStm(ParseTree tree, params object[] paramlist)
        {
            var expr = (ExpressionBase)GetNode(TokenType.Expr).Eval(tree);
            var iftrue = (CodeBlock)nodes.OfTokenType(TokenType.Statements).First().Eval(tree);
            var iffalse = nodes.OfTokenType(TokenType.Statements).Count() > 1 ? (CodeBlock)nodes.OfTokenType(TokenType.Statements).Last().Eval(tree):null;
            var ifstm = new IfStm
            {
                Node = this,
                Condition = expr,
                IfFalse = iffalse,
                IfTrue = iftrue
                
            };
            return ifstm;
        }

        /// <summary>
        /// Rule: WhileStm -> WHILE (Expr)? Statements ;
        /// </summary>
        protected override object EvalWhileStm(ParseTree tree, params object[] paramlist)
        {
            var whilestm = new DoWhileStm
            {
                Condition = (ExpressionBase)GetNode(TokenType.Expr)?.Eval(tree),
                Statements = (CodeBlock)GetNode(TokenType.Statements).Eval(tree),
                Type = LoopType.While,
                Node = this
            };
            return whilestm;
        }

        /// <summary>
        /// Rule: DoStm -> DO Statements WHILE Expr ;
        /// </summary>
        protected override object EvalDoStm(ParseTree tree, params object[] paramlist)
        {
            var whileDoStm = new DoWhileStm
            {
                Condition = (ExpressionBase)GetNode(TokenType.Expr)?.Eval(tree),
                Statements = (CodeBlock)GetNode(TokenType.Statements).Eval(tree),
                Type = LoopType.DoWhile,
                Node = this
            };
            return whileDoStm;
        }

        /// <summary>
        /// Rule: ForStm -> FOR Variable Assign TO Expr (INCBY Expr )? Statements ;
        /// </summary>
        protected override object EvalForStm(ParseTree tree, params object[] paramlist)
        {
            var forStm = new ForStm
            {
                Variable = (LeftSideExprVariable)GetNode(TokenType.Variable).Eval(tree, true),
                AssignExpression = (ExpressionBase)GetNode(TokenType.Assign).Eval(tree),
                ToExpression = (ExpressionBase)nodes.OfTokenType(TokenType.Expr).First().Eval(tree),
                IncByExpression = nodes.OfTokenType(TokenType.Expr).Count() > 1 ? (ExpressionBase)nodes.OfTokenType(TokenType.Expr).Last().Eval(tree) : null,
                Statements = (CodeBlock)GetNode(TokenType.Statements).Eval(tree),
                Node = this
            };
            return forStm;
        }

        /// <summary>
        /// Rule: ReturnStm -> RETURN (Expr)? ;
        /// </summary>
        protected override object EvalReturnStm(ParseTree tree, params object[] paramlist)
        {
            var returnStm = new ReturnStm {Node = this};
            var exprNode = GetNode(TokenType.Expr);
            var returnType = SymbolType.Void;
            var funcImpl = AstCreator.FuncImplementation.Peek();
            if (exprNode != null)
            {
                var expr = (ExpressionBase)exprNode.Eval(tree);
                returnType = expr.GetExprType();
                returnStm.ReturnExpression = expr;

            }
            if (funcImpl.ReturnType != SymbolType.Unknown && funcImpl.ReturnType != returnType)
            {
                throw new ParseException("Возвращаемые значения должны быть одинакового типа", GetNode(TokenType.RETURN));
            }
            AstCreator.FuncImplementation.Peek().ReturnType = returnType;
            
            return returnStm;
        }

        /// <summary>
        /// Rule: OperStm -> OPER (Call)? ;
        /// </summary>
        protected override object EvalOperStm(ParseTree tree, params object[] paramlist)
        {
            return new OperStm {Operation = GetNode(TokenType.OPER).Token.Text, Node = this, Arguments = (Arguments)GetNode(TokenType.Call)?.Eval(tree)};
        }

        /// <summary>
        /// Rule: CallOrAssign -> Variable (Assign)? ;
        /// </summary>
        protected override object EvalCallOrAssign(ParseTree tree, params object[] paramlist)
        {
            var callOrAssignStm = new CallOrAssign {Node = this};
            var assignNode = GetNode(TokenType.Assign);
            var leftSide = (LeftSideExprBase)GetNode(TokenType.Variable).Eval(tree, true);
            callOrAssignStm.LeftSideExpr = leftSide;
            if (assignNode != null)
            {
                if (leftSide.Type == LeftSideExprType.Call)
                {
                    throw new ParseException("Попытка присвоить значение вызову функции", assignNode);
                }
                var symbol = Namespaces.Current.Symbols.SingleOrDefault(x => x.Name == leftSide.Name);
                var expr = (ExpressionBase)assignNode.Eval(tree);
                callOrAssignStm.AssignExpression = expr;
                var exprType = expr.GetExprType();
                if (leftSide.Type == LeftSideExprType.Variable)
                {
                    ((LeftSideExprVariable)leftSide).VariableType = exprType;
                    if (symbol != null)
                    {

                        if (symbol.Type == SymbolType.Unknown)
                        {
                            symbol.Type = exprType;
                        }
                        else
                        {
                            if (symbol.Type != exprType)
                            {
                                throw new ParseException("Невозможно изменить тип переменной", assignNode);
                            }
                        }
                    }
                    else
                    {
                        Namespaces.Current.AddSymbol(
                            new Symbol(exprType, leftSide.Name),
                            GetNode(TokenType.Variable));
                    }
                }
                if (leftSide.Type == LeftSideExprType.Array)
                {
                    var leftsidearray = (LeftSideExprArray)leftSide;
                    if (symbol != null)
                    {
                        if (symbol.Type == SymbolType.Array)
                        {
                            if (symbol.ArrayKeyType != leftsidearray.ArrayKeyType)
                            {
                                throw new ParseException("Невозможно изменить тип ключа у массива", assignNode);
                            }
                            if (symbol.ArrayValueType != exprType)
                            {
                                throw new ParseException("Невозможно изменить тип значений массива", assignNode);
                            }
                        }
                        else
                        {
                            throw new ParseException("Нельзя использовать скалярную переменную как массив!", assignNode);
                        }
                    }
                    else
                    {
                        Namespaces.Current.AddSymbol(
                            new Symbol(SymbolType.Array, leftSide.Name) {ArrayKeyType = leftsidearray.ArrayKeyType, ArrayValueType = exprType},
                            GetNode(TokenType.Variable));
                    }
                }
            }

            if (assignNode == null)
            {
                if (leftSide.Type == LeftSideExprType.Variable || leftSide.Type == LeftSideExprType.Array)
                {
                    throw new ParseException("Отсутствует правая часть выражения", GetNode(TokenType.Variable));
                }
            }

            return callOrAssignStm;
        }

        /// <summary>
        /// Rule: Assign -> ASSIGN Expr ;
        /// </summary>
        protected override object EvalAssign(ParseTree tree, params object[] paramlist)
        {
            return GetNode(TokenType.Expr).Eval(tree);
        }

        /// <summary>
        /// Rule: Variable -> IDENTIFIER ((Array | Call))? ;
        /// </summary>
        protected override object EvalVariable(ParseTree tree, params object[] paramlist)
        {
            var identNode = GetNode(TokenType.IDENTIFIER);
            var name = identNode.Token.Text;
            var isLeftSide = paramlist != null && paramlist.Any() && (bool)paramlist.First();

            if (GetNode(TokenType.Array) == null && GetNode(TokenType.Call) == null)
            {
                if (isLeftSide)
                {
                    return new LeftSideExprVariable{ Name = name,  Type = LeftSideExprType.Variable , Node = this};
                }
                var existingSymbol = Namespaces.Current.Symbols.SingleOrDefault(x => x.Name == name);
                if (existingSymbol != null && existingSymbol.Type == SymbolType.Array)
                {
                    throw new ParseException($"Использование массива {name} без индексатора", identNode);
                }
                if (existingSymbol != null && existingSymbol.Type == SymbolType.Function)
                {
                    throw new ParseException($"Использование функции {name} без скобок", identNode);
                }
                if (existingSymbol != null && existingSymbol.Type != SymbolType.Unknown)
                {
                    existingSymbol.UsagesCount++;
                    return new GetVariableExpr {Type = existingSymbol.Type, Node = this, Name = name};
                }
                
                throw new ParseException($"Использование непроинициализированной переменной {name}", identNode);
            }

            if (GetNode(TokenType.Array) != null)
            {
                var expr = (ExpressionBase)GetNode(TokenType.Array).Eval(tree);
                var exprType = expr.GetExprType();
                if (isLeftSide)
                {
                    return new LeftSideExprArray
                    {
                        ArrayKeyType = exprType,
                        Index = expr,
                        Node = this,
                        Type = LeftSideExprType.Array,
                        Name = name
                    };
                }
                var existingSymbol = Namespaces.Current.Symbols.SingleOrDefault(x => x.Name == name && x.Type == SymbolType.Array && x.ArrayKeyType == exprType);
                if (existingSymbol != null)
                {
                    existingSymbol.UsagesCount++;
                    return new GetArrayExpr { ValuesType = existingSymbol.ArrayValueType, Node = this, Name = name, Index = expr, KeysType = existingSymbol.ArrayKeyType};
                }
                if(Namespaces.Current.Symbols.Any(x => x.Name == name && x.Type == SymbolType.Array))
                {
                    throw new ParseException("Невозможно изменить тип ключа у массива", identNode);
                }
                throw new ParseException($"Использование непроинициализированного массива {name}", identNode);
            }

            if (GetNode(TokenType.Call) != null)
            {
                var arguments = ((Arguments)GetNode(TokenType.Call).Eval(tree));
                var funcDeclaration = AstCreator.FuncDeclarations.SingleOrDefault(x => x.Name == name && x.Arguments.Count == arguments.Values.Count);
                if (funcDeclaration == null)
                {
                    throw new ParseException($"Попытка использовать необъявленную функцию {name}", identNode);
                }

                var argumentsTypes = arguments.Values.Select(av => av.GetExprType()).ToList();
                if (AstCreator.FuncImplementation.Count > 0)
                {
                    if (AstCreator.FuncImplementation.Any(x => x.Name == name && x.ArgumentsTypes.SequenceEqual(argumentsTypes)))
                    {
                        throw new ParseException("Рекурсия не поддерживается", identNode);
                    }
                }
                var funcImplementation = AstCreator.FuncImplementations.SingleOrDefault(x => x.Name == name && x.ArgumentsTypes.SequenceEqual(argumentsTypes));
                if (funcImplementation == null)
                {
                    funcImplementation = new FuncImplementation { Name = name, Node = this, ArgumentsTypes = argumentsTypes};
                    AstCreator.FuncImplementation.Push(funcImplementation);

                    var currentNamespace = Namespaces.Current;
                    Namespaces.Current = Namespaces.Root;
                    Namespaces.LevelDown(new Namespace($"{name} {string.Join(", ", argumentsTypes)}"));

                    for (var index = 0; index < arguments.Values.Count; index++)
                    {
                        var symbolType = argumentsTypes[index];
                        var symbol = new Symbol(symbolType, funcDeclaration.Arguments[index]);
                        Namespaces.Current.AddSymbol(symbol, identNode);
                    }

                    funcDeclaration.Node.Eval(tree, true, this);

                    Namespaces.Current = currentNamespace;
                    AstCreator.FuncImplementations.Add(AstCreator.FuncImplementation.Pop());
                }

                var callFuncExpr = new CallFuncExpr { Node = this, Function = funcImplementation, Arguments = arguments};
                if (isLeftSide)
                {
                    return new LeftSideExprCall {Name = name, Type = LeftSideExprType.Call, Node = this, CallFunc = callFuncExpr};
                
                }
                if (funcImplementation.ReturnType == SymbolType.Void || funcImplementation.ReturnType == SymbolType.Unknown)
                {
                    throw new ParseException("Невозможно использовать функции, возвращающие void в выражениях", this);
                }
                return callFuncExpr;
            }
            return null;

        }

        /// <summary>
        /// Rule: Array -> SQOPEN Expr SQCLOSE ;
        /// </summary>
        protected override object EvalArray(ParseTree tree, params object[] paramlist)
        {
            return GetNode(TokenType.Expr).Eval(tree);
        }

        /// <summary>
        /// Rule: Call -> BROPEN (Arguments)? BRCLOSE ;
        /// </summary>
        protected override object EvalCall(ParseTree tree, params object[] paramlist)
        {
            return GetNode(TokenType.Arguments)?.Eval(tree) ?? new Arguments {Node = this};
        }

        /// <summary>
        /// Rule: Arguments -> Expr (COMMA Expr )* ;
        /// </summary>
        protected override object EvalArguments(ParseTree tree, params object[] paramlist)
        {
            return new Arguments
            {
                Values = nodes.OfTokenType(TokenType.Expr).Select(x => x.Eval(tree)).Cast<ExpressionBase>().ToList(),
                Node = this
            };
        }

        /// <summary>
        /// Rule: Literal -> (INTEGER | DOUBLE | STRING | BOOL | READFUNC);
        /// </summary>
        protected override object EvalLiteral(ParseTree tree, params object[] paramlist)
        {
            var value = nodes.First().Token.Text;
            if (GetNode(TokenType.INTEGER) != null)
            {
                return new LiteralExpr {SymbolType = SymbolType.Integer, Node = this, Value = Convert.ToInt32(value)};
            }

            if (GetNode(TokenType.DOUBLE) != null)
            {
                return new LiteralExpr {SymbolType = SymbolType.Double, Node = this,  Value = Convert.ToDouble(value, CultureInfo.InvariantCulture) };
            }

            if (GetNode(TokenType.STRING) != null)
            {
                return new LiteralExpr {SymbolType = SymbolType.String, Node = this,  Value = value.Substring(1, value.Length - 2)};
            }

            if (GetNode(TokenType.BOOL) != null)
            {
                return new LiteralExpr {SymbolType = SymbolType.Bool, Node = this,  Value = Convert.ToBoolean(value)};
            }

            if (GetNode(TokenType.READFUNC) != null)
            {
                switch (value)
                {
                    case "readnum":
                        return new ConsoleReadExpr {Type = SymbolType.Integer, Node = this};
                    case "readstr":
                        return new ConsoleReadExpr {Type = SymbolType.String, Node = this};
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
                return (ExpressionBase)GetNode(TokenType.OrExpr).Eval(tree);
            }
            var ternaryExpr = new TernaryExpr
            {
                First = (ExpressionBase)GetNode(TokenType.OrExpr).Eval(tree),
                Second = (ExpressionBase)nodes.OfTokenType(TokenType.Expr).First().Eval(tree),
                Third = (ExpressionBase)nodes.OfTokenType(TokenType.Expr).Last().Eval(tree),
                Node = this

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
                return (ExpressionBase)GetNode(TokenType.AndExpr).Eval(tree);
            }

            var expressions = new Stack<ExpressionBase>(nodes.OfTokenType(TokenType.AndExpr).Select(n => n.Eval(tree)).Cast<ExpressionBase>().Reverse());

            while (expressions.Count > 1)
            {
                var leftExpr = expressions.Pop();
                var rightExpr = expressions.Pop();
                var newExpr = new AndOrExpr
                {
                    First = leftExpr,
                    Second = rightExpr,
                    Node = this,
                    Type = AndOrOperation.Or
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
                return (ExpressionBase)GetNode(TokenType.NotExpr).Eval(tree);
            }

            var expressions = new Stack<ExpressionBase>(nodes.OfTokenType(TokenType.NotExpr).Select(n => n.Eval(tree)).Cast<ExpressionBase>().Reverse());

            while (expressions.Count > 1)
            {
                var leftExpr = expressions.Pop();
                var rightExpr = expressions.Pop();
                var newExpr = new AndOrExpr
                {
                    First = leftExpr,
                    Second = rightExpr,
                    Node = this,
                    Type = AndOrOperation.And
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
                return (ExpressionBase)GetNode(TokenType.CompExpr).Eval(tree);
            }
            var unaryExpr = new NotExpr { First = (ExpressionBase)GetNode(TokenType.CompExpr).Eval(tree), Node = this };

            return unaryExpr;
        }

        /// <summary>
        /// Rule: CompExpr -> AddExpr (COMP AddExpr )? ;
        /// </summary>
        protected override object EvalCompExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.COMP) == null)
            {
                return (ExpressionBase)GetNode(TokenType.AddExpr).Eval(tree);
            }

            var boolExpr = new CompareExpr
            {
                Node = this,
                First = (ExpressionBase)nodes.OfTokenType(TokenType.AddExpr).First().Eval(tree),
                Second = (ExpressionBase)nodes.OfTokenType(TokenType.AddExpr).Last().Eval(tree),
                OperationText = GetNode(TokenType.COMP).Token.Text
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
                return (ExpressionBase)GetNode(TokenType.MultExpr).Eval(tree);
            }

            var expressions = new Stack<ExpressionBase>(nodes.OfTokenType(TokenType.MultExpr).Select(n => n.Eval(tree)).Cast<ExpressionBase>().Reverse());
            var operations = new Stack<string>(nodes.OfTokenType(TokenType.PLUSMINUS).Select(x => x.Token.Text).Reverse());

            while (expressions.Count > 1)
            {
                var leftExpr = expressions.Pop();
                var rightExpr = expressions.Pop();
                var newExpr = new AddExpr
                {
                    First = leftExpr,
                    Second = rightExpr,
                    Node = this,
                    OperationText = operations.Pop()
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
                return (ExpressionBase)GetNode(TokenType.PowExpr).Eval(tree);
            }

            var expressions = new Stack<ExpressionBase>(nodes.OfTokenType(TokenType.PowExpr).Select(n => n.Eval(tree)).Cast<ExpressionBase>().Reverse());
            var operations = new Stack<string>(nodes.OfTokenType(TokenType.MULTDIV).Select(x => x.Token.Text).Reverse());

            while (expressions.Count > 1)
            {
                var leftExpr = expressions.Pop();
                var rightExpr = expressions.Pop();
                var newExpr = new MultPowExpr
                {
                    First = leftExpr,
                    Second = rightExpr,
                    Node = this,
                    OperationText = operations.Pop()
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
                return (ExpressionBase)GetNode(TokenType.UnaryExpr).Eval(tree);
            }

            var expressions = new Stack<ExpressionBase>(nodes.OfTokenType(TokenType.UnaryExpr).Select(n => n.Eval(tree)).Cast<ExpressionBase>().Reverse());
            var operations = new Stack<string>(nodes.OfTokenType(TokenType.POW).Select(x => x.Token.Text).Reverse());


            while (expressions.Count > 1)
            {
                var leftExpr = expressions.Pop();
                var rightExpr = expressions.Pop();
                var newExpr = new MultPowExpr
                {
                    First = leftExpr,
                    Second = rightExpr,
                    Node = this,
                    OperationText = operations.Pop()
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
                return (ExpressionBase)GetNode(TokenType.Atom).Eval(tree);
            }

            var unaryExpr = new UnaryExpr
            {
                First = (ExpressionBase)GetNode(TokenType.Atom).Eval(tree),
                Node = this,
                OperationText = GetNode(TokenType.UNARYOP).Token.Text
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
                return ((ExpressionBase)literalNode.Eval(tree));
            }

            var variableNode = GetNode(TokenType.Variable);
            if (variableNode != null)
            {
                return (ExpressionBase)variableNode.Eval(tree);
            }

            var exprNode = GetNode(TokenType.Expr);
            return ((ExpressionBase)exprNode.Eval(tree));
            
        }
    }
}
