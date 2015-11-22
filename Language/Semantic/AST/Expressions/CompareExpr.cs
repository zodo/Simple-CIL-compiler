namespace Language.AST.Expressions
{
    using System.Linq;
    using System.Windows.Forms;

    using Semantic;
    using Semantic.Data;

    public class CompareExpr : ExpressionBase
    {
        public override SymbolType GetExprType()
        {
            var intdouble = new[] { SymbolType.Double, SymbolType.Integer };
            if (intdouble.Contains(FirstType) && intdouble.Contains(SecondType))
            {
                return SymbolType.Bool;
            }
            if (FirstType == SymbolType.String && SecondType == SymbolType.String)
            {
                return SymbolType.Bool;
            }
            if (FirstType == SymbolType.Bool && SecondType == SymbolType.Bool)
            {
                return SymbolType.Bool;
            }
            throw new ParseException($"Невозможно сравнить типы {FirstType} и {SecondType}", Node);
        }

        public override TreeNode GetNodes()
        {
            var node = base.GetNodes();
            node.Text = Type.ToString();
            return node;
        }

        public CompareType Type
        {
            get
            {
                switch (OperationText)
                {
                    case "=": return CompareType.Eq;
                    case "!=": return CompareType.NotEq;
                    case ">": return CompareType.More;
                    case ">=": return CompareType.MoreEq;
                    case "<":return CompareType.Less;
                    case "<=":return CompareType.LeesEq;
                    default: throw new ParseException("Неизвестное сравнение", Node);
                }
            }
        }

    }

    //COMP	-> @"=|\!=|\<\=|\<|\>=|\>";
    public enum CompareType
    {
        Eq,
        NotEq,
        More,
        Less,
        MoreEq,
        LeesEq
    }
}
