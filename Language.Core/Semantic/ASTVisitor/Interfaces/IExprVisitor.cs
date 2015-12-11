namespace Language.Semantic.ASTVisitor
{
    using AST.Expressions;

    using Language.AST;
    using Language.AST.Expressions;

    public interface IExprVisitor
    {
        dynamic Visit(AddExpr expr);

        dynamic Visit(AndOrExpr expr);

        dynamic Visit(CallFuncExpr expr);

        dynamic Visit(CompareExpr expr);

        dynamic Visit(CallCustomExpr expr);

        dynamic Visit(ExpressionBase expr);

        dynamic Visit(GetArrayExpr expr);

        dynamic Visit(GetVariableExpr expr);

        dynamic Visit(LiteralExpr expr);

        dynamic Visit(MultPowExpr expr);

        dynamic Visit(NotExpr expr);

        dynamic Visit(TernaryExpr expr);

        dynamic Visit(UnaryExpr expr);

    }
}
