namespace Language.AST.Expressions
{
    using Semantic;
    using Semantic.ASTVisitor;

    public class MultPowExpr : ExpressionBase
    {
        public MultPowDivType Type
        {
            get
            {
                switch (OperationText)
                {
                    case "/":
                        return MultPowDivType.Div;
                    case "%/":
                        return MultPowDivType.IntDiv;
                    case "%%":
                        return MultPowDivType.Mod;
                    case "*":
                        return MultPowDivType.Mult;
                    case "^":
                        return MultPowDivType.Pow;
                    default:
                        throw new ParseException("Неправильный тип операции", Node);
                }
            }
        }
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    // MULTDIV	-> @"\*|/|\%\%|\%/";
    public enum MultPowDivType
    {
        Mult,

        Div,

        IntDiv,

        Mod,

        Pow
    }
}