using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language.AST
{
    using System.Windows.Forms;

    using Semantic.Data;

    using Statements;

    public class FuncImplementation : AstBase
    {
        public List<Symbol> Arguments { get; set; } = new List<Symbol>(); 

        public CodeBlock Code { get; set; }

        public ExpressionBase ReturnExpression { get; set; }

        public SymbolType ReturnType { get; set; }

        public string Name { get; set; }

        public override TreeNode GetNodes()
        {
            var node = new TreeNode($"{Name} FuncImpl");
            var argNode = new TreeNode("Arguments");
            argNode.Nodes.AddRange(Arguments.Select(x => new TreeNode($"{x.Name}:{x.Type.ToString()}")).ToArray());
            node.Nodes.Add(argNode);
            if (ReturnExpression != null)
            {
                node.Nodes.Add(new TreeNode("ReturnExpr") { Nodes = { ReturnExpression.GetNodes() } });
            }
            if (Code != null)
            {
                node.Nodes.Add(new TreeNode("Code") { Nodes = { Code.GetNodes() } });
            }
            node.Nodes.Add(new TreeNode("ReturnType") { Nodes = { new TreeNode(ReturnType.ToString()) } });
            return node;
        }
    }
}
