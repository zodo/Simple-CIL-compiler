namespace Language.Semantic
{
    using System;

    using TinyPG;

    public class ParseException : Exception
    {
        public ParseError Error { get; set; }
    }
}
