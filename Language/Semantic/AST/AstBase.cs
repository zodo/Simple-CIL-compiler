namespace Language.AST
{
    using System.Windows.Forms;

    using Semantic.ASTVisitor;
    using Semantic.Data;

    using TinyPG;

    public abstract class AstBase
    {
        public Namespace Namespace { get; set; }

        public ParseNode Node { get; set; }

        protected AstBase()
        {
            Namespace = Namespaces.Current;
        }
        

        public AstBase Optimize()
        {
            return new OptimizeVisitor().Visit((dynamic)this);
        }

        public abstract dynamic Accept(IAstVisitor visitor);

        public TreeNode GetNodes()
        {
            return new MakeTreeVisitor().Visit((dynamic)this);
        }

    }
}
