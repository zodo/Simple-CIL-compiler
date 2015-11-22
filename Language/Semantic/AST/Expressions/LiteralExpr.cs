namespace Language.AST.Expressions
{
    using System.Windows.Forms;

    using Semantic.Data;

    public class LiteralExpr : ExpressionBase
    {
        public SymbolType SymbolType { get; set; }

        public string Value { get; set; }

        public override SymbolType GetExprType()
        {
            return SymbolType;
        }

        public override TreeNode GetNodes()
        {
            var node = base.GetNodes();
            node.Text = Value;
            return node;
        }
    }
}
