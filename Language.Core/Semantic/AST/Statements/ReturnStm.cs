namespace Language.AST.Statements
{
    using Semantic.ASTVisitor;

    public class ReturnStm : StatementBase
    {
        public ExpressionBase ReturnExpression { get; set; }
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
