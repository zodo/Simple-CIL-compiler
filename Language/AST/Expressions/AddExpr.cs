namespace Language.AST.Expressions
{
    using System;
    using System.Linq;

    using Semantic.Data;

    class AddExpr : Expression
    {
        public override SymbolType GetExprType()
        {
            if (Types.Contains(SymbolType.String))
            {
                return SymbolType.String;
            }
            if (Types.Contains(SymbolType.Double))
            {
                return SymbolType.Double;
            }
            if (Types.Contains(SymbolType.Integer))
            {
                return SymbolType.Integer;
            }
            throw new Exception();
        }
    }
}
