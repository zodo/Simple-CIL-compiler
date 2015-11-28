namespace Language.Semantic.ASTVisitor
{
    using System;
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
                            result = left.RawValue + right.RawValue;
                            break;
                        case AddType.Minus:
                            result = left.RawValue - right.RawValue;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    if (literal.SymbolType == SymbolType.Integer)
                    {
                        result = Convert.ToInt32(result);
                    }
                    literal.RawValue = result;
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
                            result = left.RawValue || right.RawValue;
                            break;
                        case AndOrOperation.And:
                            result = left.RawValue && right.RawValue;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    literal.RawValue = result;
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
                            result = left.RawValue == right.RawValue;
                            break;
                        case CompareType.NotEq:
                            result = left.RawValue != right.RawValue;
                            break;
                        case CompareType.More:
                            result = left.RawValue > right.RawValue;
                            break;
                        case CompareType.Less:
                            result = left.RawValue < right.RawValue;
                            break;
                        case CompareType.MoreEq:
                            result = left.RawValue >= right.RawValue;
                            break;
                        case CompareType.LeesEq:
                            result = left.RawValue <= right.RawValue;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    literal.RawValue = result;
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
                        RawValue = symbol.Value,
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
                            result = left.RawValue * right.RawValue;
                            break;
                        case MultPowDivType.Div:
                            result = (double)left.RawValue / right.RawValue;
                            break;
                        case MultPowDivType.IntDiv:
                            result = left.RawValue / right.RawValue;
                            break;
                        case MultPowDivType.Mod:
                            result = left.RawValue % right.RawValue;
                            break;
                        case MultPowDivType.Pow:
                            result = Math.Pow(left.RawValue, right.RawValue);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    if (literal.SymbolType == SymbolType.Integer)
                    {
                        result = Convert.ToInt32(result);
                    }
                    literal.RawValue = result;
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
                    left.RawValue = !left.RawValue;
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
                    return left.RawValue ? expr.Second : expr.Third;
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
                    literal.RawValue = -literal.RawValue;
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
                            symbol.Value = right.RawValue;
                        }
                    }
                }
            }
            return stm;
        }

        public dynamic Visit(CodeBlock code)
        {
            code.Statements.ForEach(x => x.Optimize());
            if (OptimizeMode.UnreacheableCode)
            {
                var retPosition = code.Statements.FindIndex(x => x is ReturnStm);
                if (retPosition != -1)
                {
                    code.Statements = code.Statements.Take(retPosition + 1).ToList();
                }
            }
            return code;
        }

        public dynamic Visit(DoWhileStm stm)
        {
            return stm;
        }

        public dynamic Visit(ForStm stm)
        {
            return stm;
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
            if (!condition.RawValue)
            {
                stm.IfTrue = null;
            }
            else
            {
                stm.IfFalse = null;
            }
            return stm;
        }

        public dynamic Visit(OperStm stm)
        {
            return stm;
        }

        public dynamic Visit(ReturnStm stm)
        {
            return stm;
        }

        public dynamic Visit(StatementBase statement)
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
