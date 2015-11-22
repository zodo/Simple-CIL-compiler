namespace Language.AST
{
    using Expressions;

    public class Variable :AstBase
    {
        public VariableType Type { get; set; }

        public string Name { get; set; }

        public LiteralExpr Expression { get; set; }
    }

    public enum VariableType
    {
        Array,
        Call,
        Simple
    }
}
