namespace Language.AST
{
    using Semantic.Data;

    public class GlobalVariable : AstBase
    {
        public Symbol Value { get; set; }
    }
}
