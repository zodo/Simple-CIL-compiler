namespace Language.AST.Expressions
{
    using System;

    using Semantic.Data;

    class TernaryExpr : Expression
    {
        public override SymbolType GetExprType()
        {
            return Second.GetExprType();
        }
    }
}
