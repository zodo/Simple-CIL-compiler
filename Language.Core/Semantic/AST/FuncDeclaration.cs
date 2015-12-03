namespace Language.AST
{
    using System.Collections.Generic;
    using Semantic.ASTVisitor;

    public class FuncDeclaration : AstBase
    {
        public string Name { get; set; }
        
        public List<string> Arguments { get; set; }
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
