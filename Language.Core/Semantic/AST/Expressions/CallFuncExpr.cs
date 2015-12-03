namespace Language.Semantic.AST.Expressions
{
    using ASTVisitor;

    using Language.AST;

    public class CallFuncExpr : ExpressionBase
    {
        public FuncImplementation Function { get; set; }

        public Arguments Arguments { get; set; }
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}