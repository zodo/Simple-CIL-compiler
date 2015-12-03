namespace Language.AST.Expressions
{
    using Semantic;
    using Semantic.ASTVisitor;

    public class UnaryExpr : ExpressionBase
    {
        public UnaryType Type
        {
            get
            {
                switch (OperationText)
                {
                    case "-":
                        return UnaryType.Minus;
                    case "--":
                        return UnaryType.MinusMinus;
                    case "+":
                        return UnaryType.Plus;
                    case "++":
                        return UnaryType.PlusPlus;
                    default:
                        throw new ParseException("Неправильный тип унарной операции", Node);
                }
            }
        }
       
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    //UNARYOP -> @"\+\+|--|\+|-";
    public enum UnaryType
    {
        Plus,

        PlusPlus,

        Minus,

        MinusMinus
    }
}