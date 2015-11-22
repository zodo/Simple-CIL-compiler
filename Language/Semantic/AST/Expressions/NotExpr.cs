namespace Language.AST.Expressions
{
    using System.Windows.Forms;

    using Semantic;
    using Semantic.Data;

    public class NotExpr : ExpressionBase
    {
        public override SymbolType GetExprType()
        {
            if (FirstType != SymbolType.Bool)
            {
                throw new ParseException("Отрицать можно только тип Bool", Node);
            }
            return SymbolType.Bool;
        }

        public override TreeNode GetNodes()
        {
            var node = base.GetNodes();
            node.Text = "NOT";
            return node;
        }
    }
}
