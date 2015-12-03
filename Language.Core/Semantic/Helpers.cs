namespace Language
{
    using System.Collections.Generic;
    using System.Linq;

    using TinyPG;

    public static class Helpers
    {
        public static IEnumerable<ParseNode> OfTokenType(this List<ParseNode> list, TokenType type)
        {
            return list.Where(n => n.Token.Type == type);
        } 
    }
}
