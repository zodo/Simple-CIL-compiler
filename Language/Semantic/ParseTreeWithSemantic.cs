namespace Language.Semantic
{
    using TinyPG;

    public class ParseTreeWithSemantic : ParseTree
    {
        public override ParseNode CreateNode(Token token, string text)
        {
            ParseNode node = new ParseNodeWithSemantic(token, text);
            node.Parent = this;
            return node;
        }
    }
}
