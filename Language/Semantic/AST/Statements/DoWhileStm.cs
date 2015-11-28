namespace Language.AST.Statements
{
    using Semantic.ASTVisitor;

    public class DoWhileStm : StatementBase
    {
        public CodeBlock Statements { get; set; }

        public ExpressionBase Condition { get; set; }

        public LoopType Type { get; set; }

       
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public enum LoopType
    {
        While,
        DoWhile
    }
}

