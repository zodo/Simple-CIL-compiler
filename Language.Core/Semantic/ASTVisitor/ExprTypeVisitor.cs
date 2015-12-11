namespace Language.Semantic.ASTVisitor
{
    using System.Linq;

    using AST.Expressions;

    using Data;

    using Language.AST;
    using Language.AST.Expressions;

    public class ExprTypeVisitor : IExprVisitor
    {
        public dynamic Visit(AddExpr expr)
        {
            if (expr.Types.Contains(SymbolType.Bool))
            {
                throw new ParseException("Невозможно применить эту операцию к Bool", expr.Node);
            }
            if (expr.Types.Contains(SymbolType.String))
            {
                if (expr.Type == AddType.Minus)
                {
                    throw new ParseException("Вычитание строк не поддерживается", expr.Node);
                }
                return SymbolType.String;
            }
            if (expr.Types.Contains(SymbolType.Double))
            {
                return SymbolType.Double;
            }
            return SymbolType.Integer;
        }

        public dynamic Visit(AndOrExpr expr)
        {
            if (expr.FirstType != SymbolType.Bool || expr.SecondType != SymbolType.Bool)
            {
                throw new ParseException("Логические операции можно производить только над типом Bool", expr.Node);
            }
            return SymbolType.Bool;
        }

        public dynamic Visit(CallFuncExpr expr)
        {
            return expr.Function.ReturnType;
        }

        public dynamic Visit(CompareExpr expr)
        {
            var intdouble = new[] { SymbolType.Double, SymbolType.Integer };
            if (intdouble.Contains(expr.FirstType) && intdouble.Contains(expr.SecondType))
            {
                return SymbolType.Bool;
            }
            if (expr.FirstType == expr.SecondType && (expr.Type == CompareType.Eq || expr.Type == CompareType.NotEq))
            {
                return SymbolType.Bool;
            }

            throw new ParseException($"Невозможно сравнить типы {expr.FirstType} и {expr.SecondType}", expr.Node);
        }

        public dynamic Visit(CallCustomExpr expr)
        {
            return expr.Type;
        }

        public dynamic Visit(ExpressionBase expr)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(GetArrayExpr expr)
        {
            return expr.ValuesType;
        }

        public dynamic Visit(GetVariableExpr expr)
        {
            return expr.Type;
        }

        public dynamic Visit(LiteralExpr expr)
        {
            return expr.SymbolType;
        }

        public dynamic Visit(MultPowExpr expr)
        {
            if (expr.Types.Contains(SymbolType.Bool) || expr.Types.Contains(SymbolType.String))
            {
                throw new ParseException(
                    $"Невозможно произвести операцию между типами {expr.FirstType} и {expr.SecondType}",
                    expr.Node);
            }
            if (expr.Type == MultPowDivType.Div)
            {
                return SymbolType.Double;
            }
            if (expr.Type == MultPowDivType.IntDiv)
            {
                return SymbolType.Integer;
            }
            if (expr.Types.Contains(SymbolType.Double))
            {
                return SymbolType.Double;
            }
            return SymbolType.Integer;
        }

        public dynamic Visit(NotExpr expr)
        {
            if (expr.FirstType != SymbolType.Bool)
            {
                throw new ParseException("Отрицать можно только тип Bool", expr.Node);
            }
            return SymbolType.Bool;
        }

        public dynamic Visit(TernaryExpr expr)
        {
            if (expr.FirstType != SymbolType.Bool)
            {
                throw new ParseException("Тип условия должен быть bool", expr.Node);
            }
            if (expr.Third == null)
            {
                return expr.SecondType;
            }
            if (expr.SecondType != expr.Third.GetExprType())
            {
                throw new ParseException("Типы левой и правой части должны быть одинаковы", expr.Node);
            }
            return expr.SecondType;
        }

        public dynamic Visit(UnaryExpr expr)
        {
            if (expr.FirstType != SymbolType.Double && expr.FirstType != SymbolType.Integer)
            {
                throw new ParseException(
                    $"Невозможно произвести операцию между типами {expr.FirstType} и {expr.SecondType}",
                    expr.Node);
            }
            return expr.FirstType == SymbolType.Double ? SymbolType.Double : SymbolType.Integer;
        }
    }
}
