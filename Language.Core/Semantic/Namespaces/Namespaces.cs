namespace Language.Semantic.Data
{
    public static class Namespaces
    {
        public static Namespace Root { get; private set; } = new Namespace("root");

        public static Namespace Current { get; set; }

        public static void LevelDown(Namespace newnamespace)
        {
            newnamespace.Owner = Current;
            Current = newnamespace;
        }

        public static void LevelUp()
        {
            Current = Current.Owner;
        }

        public static void Reset()
        {
            Root = new Namespace("root");
            Current = Root;
        }
    }
}
