namespace Language.Semantic.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TinyPG;

    public class Namespace
    {
        private readonly List<Symbol> _symbols = new List<Symbol>();

        private Namespace _owner;

        public Namespace(string name)
        {
            Name = $"{name}";
        }
        
        public string Name { get; }

        public Namespace Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner?.Children.Remove(this);
                _owner = value;
                _owner?.Children.Add(this);
            }
        }

        public List<Namespace> Children { get; } = new List<Namespace>();

        public IReadOnlyCollection<Symbol> Symbols => Owner?.Symbols.Concat(_symbols).ToList() ?? _symbols;

        public IReadOnlyCollection<Symbol> SymbolsSameLevel => _symbols; 


        public void AddSymbol(Symbol symbol, ParseNode node)
        {
            var existedSymbols = Symbols.Where(l => l.Name == symbol.Name);
            if (existedSymbols.Any())
            {
                ParseTree.Instance.Errors.Add(new ParseError($"Переменная {symbol.Name} уже используется", 0x007, node));
            }
            _symbols.Add(symbol);
        }
    }
}
