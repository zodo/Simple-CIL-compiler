namespace Language.AST.Expressions
{
    using System.Collections;
    using System.Windows.Forms;

    using Semantic;
    using Semantic.Data;

    public class UnaryExpr : ExpressionBase
    {
        public override SymbolType GetExprType()
        {
            if (FirstType == SymbolType.Bool || FirstType == SymbolType.String)
            {
                throw new ParseException(
                    $"Невозможно произвести операцию между типами {FirstType} и {SecondType}",
                    Node);
            }
            if (FirstType == SymbolType.Double)
            {
                return SymbolType.Double;
            }
            return SymbolType.Integer;
        }

        public override TreeNode GetNodes()
        {
            var node = base.GetNodes();
            node.Text = Type.ToString();
            return node;
        }

        public UnaryType Type
        {
            get
            {
                switch (OperationText)
                {
                    case "-":return UnaryType.Minus;
                    case "--": return UnaryType.MinusMinus;
                    case "+":return UnaryType.Plus;
                    case "++":return UnaryType.PlusPlus;
                    default: throw new ParseException("Неправильный тип унарной операции", Node);
                }
            }
        }

    }

    //UNARYOP -> @"\+\+|--|\+|-";
    public enum UnaryType
    {
        Plus,
        PlusPlus,
        Minus,
        MinusMinus
    }
}
