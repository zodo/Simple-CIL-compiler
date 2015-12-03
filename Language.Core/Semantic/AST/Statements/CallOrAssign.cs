namespace Language.AST.Statements
{
    using Semantic.AST.LeftExprSide;
    using Semantic.ASTVisitor;

    public class CallOrAssign : StatementBase
    {
        public LeftSideExprBase LeftSideExpr { get; set; }

        public ExpressionBase AssignExpression { get; set; }
       
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
