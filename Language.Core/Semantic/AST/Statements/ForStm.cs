namespace Language.AST.Statements
{
    using Semantic.AST.LeftExprSide;
    using Semantic.ASTVisitor;

    public class ForStm : StatementBase
    {
        public LeftSideExprVariable Variable { get; set; }

        public ExpressionBase AssignExpression { get; set; }

        public ExpressionBase ToExpression { get; set; }

        public ExpressionBase IncByExpression { get; set; }

        public CodeBlock Statements { get; set; }

        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
