namespace Language.Semantic.AST.Expressions
{
    using ASTVisitor;
    using Data;
    using Language.AST;

    public class CallCustomExpr : ExpressionBase
    {
        public SymbolType Type { get; set; }

        public Arguments Arguments { get; set; }

        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}