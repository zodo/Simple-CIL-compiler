namespace Language.Semantic.ASTVisitor
{
    using Interfaces;

    public interface IAstVisitor : IExprVisitor, IStatementVisitor, IBaseVisitor, ILeftExprVisitor
    {
       
    }
}
