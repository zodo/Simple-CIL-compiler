namespace Language.Semantic.AST
{
    using System.Collections.Generic;
    using ASTVisitor;
    using Language.AST;

    public class Arguments : AstBase
    {
        public List<ExpressionBase> Values { get; set; } = new List<ExpressionBase>();
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
