namespace Language.Semantic.ASTVisitor.Interfaces
{
    using AST.LeftExprSide;

    public interface ILeftExprVisitor
    {
        dynamic Visit(LeftSideExprArray left);

        dynamic Visit(LeftSideExprBase left);

        dynamic Visit(LeftSideExprCall left);

        dynamic Visit(LeftSideExprVariable left);
    }
}
