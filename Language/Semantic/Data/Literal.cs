namespace Language.Semantic.Data
{
    using System;

    public class Literal
    {
        public Literal(LiteralType type, string value, string name)
        {
            Type = type;
            Value = value;
            Name = name;
        }

        public LiteralType Type { get; }

        private string Value { get; set; }

        public T GetValue<T>()
        {
            return (T)Convert.ChangeType(Value, typeof(T));
        }

        public string Name { get; set; }
    }
}
