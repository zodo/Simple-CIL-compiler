namespace Language.Semantic.Data
{
    using System.Windows.Forms;

    public static class IdentifierTable
    {
        public static Namespace RootNamespace { get; private set; } = new Namespace("root");

        public static Namespace CurrentNamespace { get; set; }

        public static void Down(Namespace newnamespace)
        {
            newnamespace.Owner = CurrentNamespace;
            CurrentNamespace = newnamespace;
        }

        public static void Up()
        {
            CurrentNamespace = CurrentNamespace.Owner;
        }

        public static void Reset()
        {
            RootNamespace = new Namespace("root");
            CurrentNamespace = RootNamespace;
        }
    }
}
