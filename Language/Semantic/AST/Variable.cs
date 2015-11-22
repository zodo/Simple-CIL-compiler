namespace Language.AST
{
    using System.Windows.Forms;

    using Expressions;

    public class Variable :AstBase
    {
        public VariableType Type { get; set; }

        public string Name { get; set; }
        

        public override TreeNode GetNodes()
        {
            return new TreeNode($"{Type}:{Name}");
        }
    }

    public enum VariableType
    {
        Array,
        Call,
        Simple
    }
}
