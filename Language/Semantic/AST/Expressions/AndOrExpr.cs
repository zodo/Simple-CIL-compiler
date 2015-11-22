namespace Language.AST.Expressions
{
    using System.Windows.Forms;

    using Semantic;
    using Semantic.Data;

    public class AndOrExpr : ExpressionBase
    {
        public override SymbolType GetExprType()
        {
            if (FirstType != SymbolType.Bool || SecondType != SymbolType.Bool)
            {
                throw new ParseException("Логические операции можно производить только над типом Bool", Node);

            }
            return SymbolType.Bool;
        }

        public AndOrOperation Type { get; set; }

        public override TreeNode GetNodes()
        {
            var node = base.GetNodes();
            node.Text = Type.ToString();
            return node;
        }


    }
    public enum AndOrOperation
    {
        Or,
        And
    }
}
