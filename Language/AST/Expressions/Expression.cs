namespace Language.AST
{
    using System.Collections.Generic;

    using Semantic.Data;

    public class Expression : AstBase
    {
       public Expression First { get; set; }

        public Expression Second { get; set; }

        public Expression Third { get; set; }

        public virtual SymbolType GetExprType()
        {
            return FirstType;
        }

        protected SymbolType FirstType => First?.GetExprType() ?? SymbolType.Unknown;

        protected SymbolType SecondType => Second?.GetExprType() ?? SymbolType.Unknown;

        protected ICollection<SymbolType> Types => new[] { FirstType, SecondType };
    }

    
    public enum Operations
    {
        Plus,
        Minus,
        Mult,
        Div,
        UnaryMinus
    }

}
