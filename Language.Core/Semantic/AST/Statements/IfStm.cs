namespace Language.AST.Statements
{
    using Semantic;
    using Semantic.ASTVisitor;
    using Semantic.Data;

    public class IfStm : StatementBase
    {
        private ExpressionBase _condition;

        public ExpressionBase Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                var exprType = value.GetExprType();
                if (exprType != SymbolType.Bool)
                {
                    throw new ParseException("Условие должно иметь тип Boolean", Node);
                }
                _condition = value;
            }
        }

        public CodeBlock IfTrue { get; set; }

        public CodeBlock IfFalse { get; set; }

        
       public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
