namespace Language.AST.Expressions
{
    using Semantic.Data;

    public class BoolExpr : Expression
    {
        public override SymbolType GetExprType()
        {
            if (SecondType != SymbolType.Bool)
            {
                return FirstType;
            }
            return SymbolType.Bool;
        }
    }
}
