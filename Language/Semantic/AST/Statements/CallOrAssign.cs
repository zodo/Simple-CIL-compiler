namespace Language.AST.Statements
{
    using System.Windows.Forms;

    public class CallOrAssign : StatementBase
    {
        public Variable Variable { get; set; }

        public ExpressionBase AssignExpression { get; set; }

        public override TreeNode GetNodes()
        {
            var node = new TreeNode("CallOrAssign");
            node.Nodes.Add(new TreeNode("Variable") {Nodes = { Variable.GetNodes() }});
            if (AssignExpression != null)
            {
                node.Nodes.Add(new TreeNode("AssignExpr") { Nodes = { AssignExpression.GetNodes()}});
            }
            return node;
        }
    }
}
