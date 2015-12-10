namespace Language.AST.Expressions
{
    using Semantic.ASTVisitor;
    using Semantic.Data;

    public class LiteralExpr : ExpressionBase
    {
        public SymbolType SymbolType { get; set; }

        public string StrValue => Value?.ToString() ?? "";

        public dynamic Value { get; set; }

        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

       
    }
}