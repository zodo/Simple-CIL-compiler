namespace Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Semantic.Data;

    using TinyPG;

    public static class Helpers
    {
        public static IEnumerable<ParseNode> OfTokenType(this List<ParseNode> list, TokenType type)
        {
            return list.Where(n => n.Token.Type == type);
        }

        public static Type CodeGenType(this SymbolType type)
        {
            switch (type)
            {
                case SymbolType.Integer:
                    return typeof(int);
                case SymbolType.Double:
                    return typeof(double);
                case SymbolType.String:
                    return typeof(string);
                case SymbolType.Bool:
                    return typeof(bool);
                case SymbolType.Unknown:
                case SymbolType.Void:
                    return typeof(void);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
