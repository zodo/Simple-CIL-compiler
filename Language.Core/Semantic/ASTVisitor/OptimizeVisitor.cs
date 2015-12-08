namespace Language.Semantic.ASTVisitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AST;
    using AST.Expressions;
    using AST.LeftExprSide;

    using Data;

    using Language.AST;
    using Language.AST.Expressions;
    using Language.AST.Statements;

    public class OptimizeVisitor : IAstVisitor
    {
        public dynamic Visit(AddExpr expr)
        {
            Visit((ExpressionBase)expr);
            if (OptimizeMode.ExpressionSimplify)
            {
                var left = expr.First as LiteralExpr;
                var right = expr.Second as LiteralExpr;
                if (left != null && right != null)
                {
                    var literal = new LiteralExpr { Namespace = expr.Namespace, Node = expr.Node, SymbolType = expr.GetExprType() };
                    dynamic result;
                    switch (expr.Type)
                    {
                        case AddType.Plus:
                            result = left.Value + right.Value;
                            break;
                        case AddType.Minus:
                            result = left.Value - right.Value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    if (literal.SymbolType == SymbolType.Integer)
                    {
                        result = Convert.ToInt32(result);
                    }
                    literal.Value = result;
                    return literal;
                }
            }
            return expr;
        }

        public dynamic Visit(AndOrExpr expr)
        {
            Visit((ExpressionBase)expr);
            if (OptimizeMode.ExpressionSimplify)
            {
                var left = expr.First as LiteralExpr;
                var right = expr.Second as LiteralExpr;
                if (left != null && right != null)
                {
                    var literal = new LiteralExpr { Namespace = expr.Namespace, Node = expr.Node, SymbolType = expr.GetExprType() };
                    bool result;
                    switch (expr.Type)
                    {
                        case AndOrOperation.Or:
                            result = left.Value || right.Value;
                            break;
                        case AndOrOperation.And:
                            result = left.Value && right.Value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    literal.Value = result;
                    return literal;
                }
            }
            return expr;
        }

        public dynamic Visit(CallFuncExpr expr)
        {
            return expr;
        }

        public dynamic Visit(CompareExpr expr)
        {
            Visit((ExpressionBase)expr);
            if (OptimizeMode.ExpressionSimplify)
            {
                var left = expr.First as LiteralExpr;
                var right = expr.Second as LiteralExpr;
                if (left != null && right != null)
                {
                    var literal = new LiteralExpr { Namespace = expr.Namespace, Node = expr.Node, SymbolType = expr.GetExprType() };
                    bool result;
                    switch (expr.Type)
                    {
                        case CompareType.Eq:
                            result = left.Value == right.Value;
                            break;
                        case CompareType.NotEq:
                            result = left.Value != right.Value;
                            break;
                        case CompareType.More:
                            result = left.Value > right.Value;
                            break;
                        case CompareType.Less:
                            result = left.Value < right.Value;
                            break;
                        case CompareType.MoreEq:
                            result = left.Value >= right.Value;
                            break;
                        case CompareType.LeesEq:
                            result = left.Value <= right.Value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    literal.Value = result;
                    return literal;
                }
            }
            return expr;
        }

        public dynamic Visit(ConsoleReadExpr expr)
        {
            return expr;
        }

        public dynamic Visit(ExpressionBase expr)
        {
            expr.First = (ExpressionBase)(expr.First?.Optimize() ?? expr.First);
            expr.Second = (ExpressionBase)(expr.Second?.Optimize() ?? expr.Second);
            expr.Third = (ExpressionBase)(expr.Third?.Optimize() ?? expr.Third);
            return expr;
        }

        public dynamic Visit(GetArrayExpr expr)
        {
            return expr;
        }

        public dynamic Visit(GetVariableExpr expr)
        {
            if (OptimizeMode.Variables)
            {
                var symbol = expr.Namespace.Symbols.FirstOrDefault(x => x.Name == expr.Name);
                if (symbol?.Value != null)
                {
                    var literal = new LiteralExpr
                    {
                        Namespace = expr.Namespace,
                        Value = symbol.Value,
                        SymbolType = expr.Type,
                        Node = expr.Node
                    };
                    return literal;
                }
            }
            return expr;
        }

        public dynamic Visit(LiteralExpr expr)
        {
            return expr;
        }

        public dynamic Visit(MultPowExpr expr)
        {
            Visit((ExpressionBase)expr);
            if (OptimizeMode.ExpressionSimplify)
            {
                var left = expr.First as LiteralExpr;
                var right = expr.Second as LiteralExpr;
                if (left != null && right != null)
                {
                    var literal = new LiteralExpr { Namespace = expr.Namespace, Node = expr.Node, SymbolType = expr.GetExprType() };
                    dynamic result;
                    switch (expr.Type)
                    {
                        case MultPowDivType.Mult:
                            result = left.Value * right.Value;
                            break;
                        case MultPowDivType.Div:
                            result = (double)left.Value / right.Value;
                            break;
                        case MultPowDivType.IntDiv:
                            result = left.Value / right.Value;
                            break;
                        case MultPowDivType.Mod:
                            result = left.Value % right.Value;
                            break;
                        case MultPowDivType.Pow:
                            result = Math.Pow(left.Value, right.Value);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    if (literal.SymbolType == SymbolType.Integer)
                    {
                        result = Convert.ToInt32(result);
                    }
                    literal.Value = result;
                    return literal;
                }
            }
            return expr;
        }

        public dynamic Visit(NotExpr expr)
        {
            Visit((ExpressionBase)expr);

            if (OptimizeMode.ExpressionSimplify)
            {
                var left = expr.First as LiteralExpr;
                if (left != null)
                {
                    left.Value = !left.Value;
                    return left;
                }
            }
            return expr;
        }

        public dynamic Visit(TernaryExpr expr)
        {
            Visit((ExpressionBase)expr);

            if (OptimizeMode.ExpressionSimplify)
            {
                var left = expr.First as LiteralExpr;
                if (left != null)
                {
                    return left.Value ? expr.Second : expr.Third;
                }
            }
            return expr;
        }

        public dynamic Visit(UnaryExpr expr)
        {
            if (OptimizeMode.ExpressionSimplify)
            {
                var literal = expr.First as LiteralExpr;
                if (literal != null)
                {
                    literal.Value = -literal.Value;
                    return literal;
                }
            }

            return expr;
        }

        public dynamic Visit(Program program)
        {
            program.FuncImplementations.ForEach(x => x.Optimize());
            return null;
        }

        public dynamic Visit(GlobalVariable variable)
        {
            return null;
        }

        public dynamic Visit(FuncImplementation impl)
        {
            impl.Code?.Optimize();
            impl.ReturnExpression?.Optimize();
            return null;
        }

        public dynamic Visit(FuncDeclaration decl)
        {
            return decl;
        }

        public dynamic Visit(Arguments arg)
        {
            return arg;
        }

        public dynamic Visit(CallOrAssign stm)
        {
            stm.AssignExpression = (ExpressionBase)(stm.AssignExpression?.Optimize() ?? stm.AssignExpression);
            if (OptimizeMode.Variables)
            {
                var left = stm.LeftSideExpr as LeftSideExprVariable;
                if (left != null)
                {
                    var symbol = left.Namespace.Symbols.FirstOrDefault(x => x.Name == left.Name);
                    if (symbol != null)
                    {
                        symbol.Value = null;
                    }
                    var right = stm.AssignExpression as LiteralExpr;
                    if (right != null)
                    {
                        if (symbol != null)
                        {
                            symbol.Value = right.Value;
                        }
                    }
                }
            }
            return stm;
        }

        public dynamic Visit(CodeBlock stm)
        {
            stm.Statements = stm.Statements.Select(x => x.Optimize()).Where(x => x != null).Cast<StatementBase>().ToList();
            if (OptimizeMode.UnreacheableCode)
            {
                var retPosition = stm.Statements.FindIndex(x => x is ReturnStm);
                if (retPosition != -1)
                {
                    stm.Statements = stm.Statements.Take(retPosition + 1).ToList();
                }
            }
            return stm;
        }

        public dynamic Visit(DoWhileStm stm)
        {
            stm.Condition = (ExpressionBase)stm.Condition.Optimize();
            stm.Statements = (CodeBlock)stm.Statements.Optimize();
            if (!OptimizeMode.UnreacheableCode)
            {
                return stm;
            }
            var condition = stm.Condition as LiteralExpr;
            if (condition == null)
            {
                return stm;
            }
            switch (stm.Type)
            {
                case LoopType.While:
                    return condition.Value ? stm : null;
                case LoopType.DoWhile:
                    return condition.Value ? (dynamic)stm : stm.Statements;
            }

            return stm;
        }

        public dynamic Visit(ForStm stm)
        {
            stm.AssignExpression = Visit(stm.AssignExpression);
            stm.Statements = Visit(stm.Statements);
            stm.ToExpression = Visit(stm.ToExpression);
            if (stm.IncByExpression != null)
            {
                stm.IncByExpression = Visit(stm.IncByExpression);
            }

            stm.Variable = Visit(stm.Variable);
            if (OptimizeMode.LoopExpansion)
            {
                var assignExpr = stm.AssignExpression as LiteralExpr;
                var toExpr = stm.ToExpression as LiteralExpr;
                if (toExpr != null && assignExpr != null && assignExpr.GetExprType() == SymbolType.Integer)
                {
                    int startValue = assignExpr.Value;
                    int toValue = toExpr.Value;
                    int incByValue = ((LiteralExpr)stm.IncByExpression)?.Value ?? 1;

                    var steps = (toValue - startValue) / incByValue;
                    if (steps >= 0 && steps < OptimizeMode.LoopExpansionRepeatLimit)
                    {
                        var block = new CodeBlock {Namespace = stm.Namespace, Node = stm.Node, Statements = new List<StatementBase>()};
                        for (var i = startValue; i <= toValue; i+=incByValue)
                        {
                            var assig = Visit(new CallOrAssign
                            {
                                Namespace = stm.AssignExpression.Namespace,
                                Node = stm.AssignExpression.Node,
                                AssignExpression = new LiteralExpr {Namespace = stm.Namespace, SymbolType = stm.Variable.VariableType, Value = i},
                                LeftSideExpr = new LeftSideExprVariable
                                {
                                    Name = stm.Variable.Name,
                                    Namespace = stm.Variable.Namespace,
                                    Type = LeftSideExprType.Variable,
                                    VariableType = stm.Variable.VariableType
                                }
                            });
                            block.Statements.Add(assig);
                            block.Statements.AddRange(Visit(stm.Statements).Statements);
                        }
                        return block;
                    }
                }
            }

            var assign = new CallOrAssign
            {
                Namespace = stm.AssignExpression.Namespace,
                Node = stm.AssignExpression.Node,
                AssignExpression = stm.AssignExpression,
                LeftSideExpr = new LeftSideExprVariable
                {
                    Name = stm.Variable.Name,
                    Namespace = stm.Variable.Namespace,
                    Type = LeftSideExprType.Variable,
                    VariableType = stm.Variable.VariableType
                }
            };

            var loopBody = new CodeBlock
            {
                Namespace = stm.Namespace,
                Node = stm.Node,
                Statements = new List<StatementBase>
                {
                    stm.Statements,
                    new CallOrAssign
                    {
                        Namespace = stm.AssignExpression.Namespace,
                        Node = stm.AssignExpression.Node,
                        AssignExpression = Visit(new AddExpr
                        {
                            First = new GetVariableExpr
                            {
                                Name = assign.LeftSideExpr.Name, Type = stm.Variable.VariableType
                            },
                            Second = stm.IncByExpression??new LiteralExpr
                            {
                                SymbolType = SymbolType.Integer,
                                Value = 1
                            },
                            OperationText = "+",
                            Namespace = stm.Namespace

                        }),
                        LeftSideExpr = new LeftSideExprVariable
                        {
                            Name = stm.Variable.Name,
                            Namespace = stm.Variable.Namespace,
                            Type = LeftSideExprType.Variable,
                            VariableType = stm.Variable.VariableType
                        }

                    }
                }
            };

            var loop = new DoWhileStm
            {
                Namespace = stm.Namespace,
                Condition = Visit(new CompareExpr
                {
                    First = Visit(new GetVariableExpr { Name = assign.LeftSideExpr.Name, Type = stm.Variable.VariableType}),
                    OperationText = "<=",
                    Second = stm.ToExpression
                }),
                Statements = loopBody,
                Type = LoopType.While
            };

            var outerBlock = new CodeBlock
            {
                Statements = new List<StatementBase>(),
                Namespace = stm.Namespace
            };

            outerBlock.Statements.Add(assign);
            outerBlock.Statements.Add(loop);
            
            return outerBlock;
        }

        public dynamic Visit(IfStm stm)
        {
            stm.Condition = (ExpressionBase)stm.Condition.Optimize();
            stm.IfTrue = (CodeBlock)stm.IfTrue.Optimize() ?? stm.IfTrue;
            stm.IfFalse = (CodeBlock)(stm.IfFalse?.Optimize() ?? stm.IfFalse);
            if (!OptimizeMode.UnreacheableCode)
            {
                return stm;
            }
            var condition = stm.Condition as LiteralExpr;
            if (condition == null)
            {
                return stm;
            }
            return condition.Value ? stm.IfTrue : stm.IfFalse;
        }

        public dynamic Visit(OperStm stm)
        {
            return stm;
        }

        public dynamic Visit(ReturnStm stm)
        {
            stm.ReturnExpression = (ExpressionBase)stm.ReturnExpression.Optimize();
            return stm;
        }

        public dynamic Visit(StatementBase stm)
        {
            throw new NotImplementedException();
        }

        public dynamic Visit(LeftSideExprArray left)
        {
            return left;
        }

        public dynamic Visit(LeftSideExprBase left)
        {
            return left;
        }

        public dynamic Visit(LeftSideExprCall left)
        {
            return left;
        }

        public dynamic Visit(LeftSideExprVariable left)
        {
            return left;
        }
    }
}
