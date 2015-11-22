namespace Language.AST.Statements
{
    using System.Windows.Forms;

    public class DoWhileStm : StatementBase
    {
        public CodeBlock Statements { get; set; }

        public ExpressionBase Condition { get; set; }

        public LoopType Type { get; set; }

        public override TreeNode GetNodes()
        {
            var node = new TreeNode(Type.ToString());
            node.Nodes.Add(new TreeNode("Condition") { Nodes = { Condition.GetNodes() } });
            node.Nodes.Add(new TreeNode("Statements") {Nodes = { Statements.GetNodes() } });
            return node;
        }
    }

    public enum LoopType
    {
        While,
        DoWhile
    }
}

