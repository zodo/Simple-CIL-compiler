namespace Language.AST
{
    using System.Collections.Generic;
    using Semantic.ASTVisitor;

    public class Program : AstBase
    {
        public List<GlobalVariable> GlobalVariables { get; set; } 

        public List<FuncImplementation> FuncImplementations { get; set; }

        
       public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
