namespace Language.AST.Expressions
{
    using System;

    using Semantic;
    using Semantic.Data;

    using TinyPG;

    public class MultPowUnaryExpr : Expression
    {
        public override SymbolType GetExprType()
        {
            if (Types.Contains(SymbolType.Double))
            {
                return SymbolType.Double;
            }
            if (Types.Contains(SymbolType.Integer))
            {
                return SymbolType.Integer;
            }
            throw new ParseException {Error = new ParseError("Невозможно произвести операцию", 0)};
        }
    }
}
