namespace Language.AST
{
    using System.Windows.Forms;

    using Semantic.Data;

    public class GlobalVariable : AstBase
    {
        public Symbol Value { get; set; }

        public override TreeNode GetNodes()
        {
            return new TreeNode($"{Value.Type}:{Value.Name}");
        }
    }
}
