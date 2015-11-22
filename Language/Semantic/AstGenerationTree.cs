namespace Language.Semantic
{
    using TinyPG;

    public class AstGenerationTree : ParseTree
    {
        public override ParseNode CreateNode(Token token, string text)
        {
            ParseNode node = new AstGenerationNode(token, text);
            node.Parent = this;
            return node;
        }
    }
}
