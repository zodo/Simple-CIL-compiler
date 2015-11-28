namespace Language.AST.Expressions
{
    using Semantic.ASTVisitor;
    using Semantic.Data;

    public class LiteralExpr : ExpressionBase
    {
        public SymbolType SymbolType { get; set; }

        public string Value => RawValue.ToString();

        public dynamic RawValue { get; set; }

        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

       
    }
}