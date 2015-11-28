namespace Language.AST.Statements
{
    using Semantic.AST;
    using Semantic.ASTVisitor;

    public class OperStm : StatementBase
    {
        public string Operation { get; set; }

        public Arguments Arguments { get; set; }
       
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
