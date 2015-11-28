namespace Language.Semantic.AST.LeftExprSide
{
    using ASTVisitor;
    using Data;

    public class LeftSideExprVariable : LeftSideExprBase
    {
        public SymbolType VariableType { get; set; }
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
