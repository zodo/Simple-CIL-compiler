namespace Language.AST
{
    using System.Collections.Generic;

    using TinyPG;

    public class FuncDeclaration : AstBase
    {
        public int ArgumentsAmount => Arguments.Count;
        public string Name { get; set; }

        public ParseNode FunctionNode { get; set; }

        public ParseTree ParseTree { get; set; }

        public List<string> Arguments { get; set; } 
    }
}
