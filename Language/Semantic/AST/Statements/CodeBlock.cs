using System.Collections.Generic;

namespace Language.AST.Statements
{
    using System.Linq;
    using System.Windows.Forms;

    public class CodeBlock : AstBase
    {
        public List<StatementBase> Statements { get; set; }

        public override TreeNode GetNodes()
        {
            var node = new TreeNode("CodeBlock");
            node.Nodes.AddRange(Statements.Select(x => x.GetNodes()).ToArray());
            return node;
        }
    }
}
