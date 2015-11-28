namespace Language.Semantic.AST.LeftExprSide
{
    using ASTVisitor;

    using Expressions;

    public class LeftSideExprCall : LeftSideExprBase
    {
        public CallFuncExpr CallFunc { get; set; }
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
