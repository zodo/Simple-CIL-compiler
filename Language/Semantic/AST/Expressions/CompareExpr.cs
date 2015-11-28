namespace Language.AST.Expressions
{
    using Semantic;
    using Semantic.ASTVisitor;

    public class CompareExpr : ExpressionBase
    {
        public CompareType Type
        {
            get
            {
                switch (OperationText)
                {
                    case "=":
                        return CompareType.Eq;
                    case "!=":
                        return CompareType.NotEq;
                    case ">":
                        return CompareType.More;
                    case ">=":
                        return CompareType.MoreEq;
                    case "<":
                        return CompareType.Less;
                    case "<=":
                        return CompareType.LeesEq;
                    default:
                        throw new ParseException("Неизвестное сравнение", Node);
                }
            }
        }

        
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        
    }

    //COMP	-> @"=|\!=|\<\=|\<|\>=|\>";
    public enum CompareType
    {
        Eq,

        NotEq,

        More,

        Less,

        MoreEq,

        LeesEq
    }
}