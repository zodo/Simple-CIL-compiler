namespace Language.AST.Expressions
{
    using Semantic.ASTVisitor;

    public class TernaryExpr : ExpressionBase
    {
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }
}