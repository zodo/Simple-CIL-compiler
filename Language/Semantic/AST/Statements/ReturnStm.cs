namespace Language.AST.Statements
{
    using System.Windows.Forms;

    public class ReturnStm : StatementBase
    {
        public ExpressionBase ReturnExpression { get; set; }

        public override TreeNode GetNodes()
        {
            var node = new TreeNode("ReturnStm");
            if (ReturnExpression != null)
            {
                node.Nodes.Add(new TreeNode("ReturnExpr") { Nodes = { ReturnExpression.GetNodes() } });
            }
            return node;
        }
    }
}
