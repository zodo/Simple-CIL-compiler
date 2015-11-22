namespace Language.AST.Expressions
{
    using Semantic.Data;

    public class LiteralExpr : Expression
    {
        public SymbolType SymbolType { get; set; }

        

        public override SymbolType GetExprType()
        {
            return SymbolType;
        }
    }
}
