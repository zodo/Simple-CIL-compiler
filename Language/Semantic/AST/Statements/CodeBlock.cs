using System.Collections.Generic;

namespace Language.AST.Statements
{
    using Semantic.ASTVisitor;

    public class CodeBlock : StatementBase
    {
        public List<StatementBase> Statements { get; set; }

        
        public List<CodeBlock> Children { get; set; } = new List<CodeBlock>(); 

        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
