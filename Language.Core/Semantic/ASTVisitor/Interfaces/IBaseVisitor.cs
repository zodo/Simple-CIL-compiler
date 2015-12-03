namespace Language.Semantic.ASTVisitor.Interfaces
{
    using AST;

    using Language.AST;

    public interface IBaseVisitor
    {
        dynamic Visit(Program program);

        dynamic Visit(GlobalVariable variable);

        dynamic Visit(FuncImplementation impl);

        dynamic Visit(FuncDeclaration decl);

        dynamic Visit(Arguments arg);
    }
}
