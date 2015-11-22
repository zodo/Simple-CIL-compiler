namespace Language.AST
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public class Program : AstBase
    {
        public List<GlobalVariable> GlobalVariables { get; set; } 

        public List<FuncImplementation> FuncImplementations { get; set; }

        public override TreeNode GetNodes()
        {
            var node = new TreeNode("Program");
            var globvarnode = new TreeNode("GlobalVariables");
            globvarnode.Nodes.AddRange(GlobalVariables.Select(x => x.GetNodes()).ToArray());
            node.Nodes.Add(globvarnode);
            var funcimpl = new TreeNode("FuncImpls");
            funcimpl.Nodes.AddRange(FuncImplementations.Select(x => x.GetNodes()).ToArray());
            node.Nodes.Add(funcimpl);
            return node;
        }
    }
}
