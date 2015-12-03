namespace Language.AST
{
    using System.Collections.Generic;

    using Semantic.ASTVisitor;
    using Semantic.Data;

    public abstract class ExpressionBase : AstBase
    {
        public ExpressionBase First { get; set; }

        public ExpressionBase Second { get; set; }

        public ExpressionBase Third { get; set; }

        public SymbolType FirstType => First?.GetExprType() ?? SymbolType.Unknown;

        public SymbolType SecondType => Second?.GetExprType() ?? SymbolType.Unknown;

        public ICollection<SymbolType> Types => new[] { FirstType, SecondType };

        public string OperationText { get; set; }

        public SymbolType GetExprType()
        {
            return new ExprTypeVisitor().Visit((dynamic)this);
        }
        
    }

    
}