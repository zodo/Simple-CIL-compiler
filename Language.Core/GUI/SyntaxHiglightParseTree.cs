namespace Language.GUI
{
    using System.Collections.Generic;

    using TinyPG;

    public class SyntaxHiglightParseTree : ParseTree
    {
        public override ParseNode CreateNode(Token token, string txt)
        {
            ParseNode node = new SyntaxHiglightNode(token, txt);
            node.Parent = this;
            return node;
        }

        public List<CodeRegion> CodeRegions { get; set; } = new List<CodeRegion>(); 
    }

    public struct CodeRegion
    {
        public string Name { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}
