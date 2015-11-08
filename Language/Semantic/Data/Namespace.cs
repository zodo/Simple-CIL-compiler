namespace Language.Semantic.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TinyPG;

    public class Namespace
    {
        private readonly List<Literal> _literals = new List<Literal>();

        private Namespace _owner;

        public Namespace(string name)
        {
            Name = name;
            Guid = Guid.NewGuid();
        }

        public Guid Guid { get; }

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

        //public IReadOnlyCollection<Literal> Literals => Owner?.Literals.Concat(_literals).ToList() ?? _literals;
        public IReadOnlyCollection<Literal> Literals => _literals;


        public void AddLiteral(Literal literal, ParseNode node)
        {
            var existedLiterals = Literals.Where(l => l.Name == literal.Name);
            if (existedLiterals.Any())
            {
                ParseTree.Instance.Errors.Add(new ParseError($"Переменная {literal.Name} уже используется", 0x007, node));
            }
            _literals.Add(literal);
        }
    }
}
