using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language.AST
{
    using System.Windows.Forms;

    using Semantic.ASTVisitor;
    using Semantic.Data;

    using Statements;

    public class FuncImplementation : AstBase
    {
        public List<SymbolType> ArgumentsTypes { get; set; } = new List<SymbolType>(); 

        public CodeBlock Code { get; set; }

        public ExpressionBase ReturnExpression { get; set; }

        public SymbolType ReturnType { get; set; }

        public string Name { get; set; }
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
