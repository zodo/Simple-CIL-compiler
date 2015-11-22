namespace Language.AST
{
    using System.Collections.Generic;

    public class Program : AstBase
    {
        public List<GlobalVariable> GlobalVariables { get; set; } 

        public List<FuncImplementation> FuncImplementations { get; set; } 
    }
}
