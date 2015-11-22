namespace Language.AST
{
    using System.Windows.Forms;

    using Semantic.Data;

    using TinyPG;

    public abstract class AstBase
    {
        protected AstBase()
        {
            Namespace = Namespaces.Current;
        }
        public Namespace Namespace { get; }

        public ParseNode Node { get; set; }

        public abstract TreeNode GetNodes();
    }
}
