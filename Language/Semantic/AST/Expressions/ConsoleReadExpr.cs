namespace Language.Semantic.AST.Expressions
{
    using ASTVisitor;
    using Data;
    using Language.AST;

    public class ConsoleReadExpr : ExpressionBase
    {
        public SymbolType Type { get; set; }

        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}