using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language.AST
{
    public static class AstCreator
    {
        public static List<GlobalVariable> GlobalVariables { get; private set; } = new List<GlobalVariable>();

        public static List<FuncDeclaration> FuncDeclarations { get; set; } = new List<FuncDeclaration> {new FuncDeclaration {Name = "main"} }; 

        public static List<FuncImplementation> FuncImplementations { get; set; } = new List<FuncImplementation>(); 

        public static Stack<FuncImplementation> FuncImplementation { get; set; } = new Stack<FuncImplementation>();

        public static Program GetProgram()
        {
            return new Program
            {
                FuncImplementations = new List<FuncImplementation>(FuncImplementations),
                GlobalVariables = new List<GlobalVariable>(GlobalVariables)
            };
        }


        public static void Reset()
        {
            GlobalVariables = new List<GlobalVariable>();
            FuncDeclarations = new List<FuncDeclaration> { new FuncDeclaration { Name = "main" } };
            FuncImplementations = new List<FuncImplementation>();
            FuncImplementation = new Stack<FuncImplementation>();
        } 
    }
}
