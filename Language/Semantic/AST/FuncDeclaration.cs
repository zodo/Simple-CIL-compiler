namespace Language.AST
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using TinyPG;

    public class FuncDeclaration : AstBase
    {
        public int ArgumentsAmount => Arguments.Count;
        public string Name { get; set; }
        
        public List<string> Arguments { get; set; }

        public override TreeNode GetNodes()
        {
            var node = new TreeNode($"{Name} FuncDeclaration");
            node.Nodes.Add(new TreeNode("Arguments") { Nodes = { new TreeNode(string.Join(", ", Arguments)) } });
            return node;
        }
    }
}
