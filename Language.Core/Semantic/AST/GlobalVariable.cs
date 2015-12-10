namespace Language.AST
{
    using Semantic.ASTVisitor;
    using Semantic.Data;

    using TriAxis.RunSharp;

    public class GlobalVariable : AstBase
    {
       public Symbol Value { get; set; }

        public FieldGen CodeGenField { get; set; }

        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
