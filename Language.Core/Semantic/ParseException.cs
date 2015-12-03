namespace Language.Semantic
{
    using System;

    using TinyPG;

    public class ParseException : Exception
    {
        public ParseException(string message, ParseNode node)
        {
            Error = new ParseError(message, 0, node);
        }

        public ParseError Error { get; set; }


    }
}
