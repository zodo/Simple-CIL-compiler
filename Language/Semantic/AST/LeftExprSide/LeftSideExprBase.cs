namespace Language.Semantic.AST.LeftExprSide
{
    using Language.AST;

    public abstract class LeftSideExprBase : AstBase
    {
        public string Name { get; set; }

        public LeftSideExprType Type { get; set; }

    }

    public enum LeftSideExprType
    {
        Array,
        Call,
        Variable
    }
}
