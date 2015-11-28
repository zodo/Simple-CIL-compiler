namespace Language.AST.Expressions
{
    using Semantic;
    using Semantic.ASTVisitor;

    public class AddExpr : ExpressionBase
    {
        public AddType Type
        {
            get
            {
                switch (OperationText)
                {
                    case "+":
                        return AddType.Plus;
                    case "-":
                        return AddType.Minus;
                    default:
                        throw new ParseException("Неправильный тип сложения", Node);
                }
            }
        }
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

   }

    public enum AddType
    {
        Plus,

        Minus
    }
}