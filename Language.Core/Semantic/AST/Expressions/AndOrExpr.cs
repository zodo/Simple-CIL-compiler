namespace Language.AST.Expressions
{
    using Semantic.ASTVisitor;

    public class AndOrExpr : ExpressionBase
    {
        public AndOrOperation Type { get; set; }
       
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public enum AndOrOperation
    {
        Or,

        And
    }
}