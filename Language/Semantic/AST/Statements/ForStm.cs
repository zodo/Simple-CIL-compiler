namespace Language.AST.Statements
{
    using System.Windows.Forms;

    public class ForStm : StatementBase
    {
        public Variable Variable { get; set; }

        public ExpressionBase AssignExpression { get; set; }

        public ExpressionBase ToExpression { get; set; }

        public ExpressionBase IncByExpression { get; set; }

        public CodeBlock Statements { get; set; }

        public override TreeNode GetNodes()
        {
            var node = new TreeNode("ForLoop");
            node.Nodes.Add(new TreeNode("Variable") { Nodes = { Variable.GetNodes() } });
            node.Nodes.Add(new TreeNode("AssignExpression") { Nodes = { AssignExpression.GetNodes() } });
            node.Nodes.Add(new TreeNode("ToExpression") { Nodes = { ToExpression.GetNodes() } });
            if (IncByExpression != null)
            {
                node.Nodes.Add(new TreeNode("IncByExpression") { Nodes = { IncByExpression.GetNodes() } });
            }
            node.Nodes.Add(new TreeNode("Statements") { Nodes = { Statements.GetNodes() } });
            return node;
        }
    }
}
