namespace Language.Semantic.AST.Expressions
{
    using ASTVisitor;
    using Data;
    using Language.AST;

    public class GetArrayExpr : ExpressionBase
    {
        public ExpressionBase Index { get; set; }

        public SymbolType ValuesType { get; set; }

        public SymbolType KeysType { get; set; }

        public string Name { get; set; }

        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}