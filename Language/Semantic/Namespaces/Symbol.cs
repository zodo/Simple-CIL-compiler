namespace Language.Semantic.Data
{
    using System;

    public class Symbol
    {
        public Symbol(SymbolType type, string name)
        {
            Type = type;
            Name = name;
        }

        public dynamic Value { get; set; }

        public SymbolType Type { get; set; }

        public SymbolType ArrayKeyType { get; set; }

        public SymbolType ArrayValueType { get; set; }

        public string Name { get; set; }

        
    }
}
