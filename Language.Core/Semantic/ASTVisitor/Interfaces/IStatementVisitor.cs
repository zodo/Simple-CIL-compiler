namespace Language.Semantic.ASTVisitor.Interfaces
{
    using Language.AST;
    using Language.AST.Statements;

    public interface IStatementVisitor
    {
        dynamic Visit(CallOrAssign stm);

        dynamic Visit(CodeBlock stm);

        dynamic Visit(DoWhileStm stm);

        dynamic Visit(ForStm stm);

        dynamic Visit(IfStm stm);

        dynamic Visit(OperStm stm);

        dynamic Visit(ReturnStm stm);

        dynamic Visit(StatementBase stm);

    }
}
