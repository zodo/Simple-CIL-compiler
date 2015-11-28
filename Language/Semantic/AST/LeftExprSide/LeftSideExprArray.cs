namespace Language.Semantic.AST.LeftExprSide
{
    using ASTVisitor;
    using Data;
    using Language.AST;

    public class LeftSideExprArray : LeftSideExprBase
    {
        public SymbolType ArrayKeyType { get; set; }

        public ExpressionBase Index { get; set; }
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
