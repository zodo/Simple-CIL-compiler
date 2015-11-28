namespace Language.Semantic.ASTVisitor
{
    using AST;
    using AST.Expressions;
    using AST.LeftExprSide;

    using Language.AST;
    using Language.AST.Expressions;
    using Language.AST.Statements;

    public interface IAstVisitor : IExprVisitor
    {
        dynamic Visit(Program program);

        dynamic Visit(GlobalVariable variable);

        dynamic Visit(FuncImplementation impl);

        dynamic Visit(FuncDeclaration decl);

        dynamic Visit(Arguments arg);

        dynamic Visit(CallOrAssign stm);

        dynamic Visit(CodeBlock code);

        dynamic Visit(DoWhileStm stm);

        dynamic Visit(ForStm stm);

        dynamic Visit(IfStm stm);

        dynamic Visit(OperStm stm);

        dynamic Visit(ReturnStm stm);

        dynamic Visit(StatementBase statement);

        dynamic Visit(LeftSideExprArray left);

        dynamic Visit(LeftSideExprBase left);

        dynamic Visit(LeftSideExprCall left);

        dynamic Visit(LeftSideExprVariable left);

        
    }
}
