namespace Language.AST.Statements
{
    using System.Windows.Forms;

    public class OperStm : StatementBase
    {
        public string Operation { get; set; }

        public override TreeNode GetNodes()
        {
            return new TreeNode(Operation);
        }
    }
}
