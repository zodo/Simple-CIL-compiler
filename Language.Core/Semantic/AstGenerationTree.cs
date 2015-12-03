namespace Language.Semantic
{
    using TinyPG;

    public class AstGenerationTree : ParseTree
    {
        public override ParseNode CreateNode(Token token, string txt)
        {
            ParseNode node = new AstGenerationNode(token, txt);
            node.Parent = this;
            return node;
        }
    }
}
