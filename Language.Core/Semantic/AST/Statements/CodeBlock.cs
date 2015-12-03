using System.Collections.Generic;

namespace Language.AST.Statements
{
    using System.Linq;

    using Semantic.ASTVisitor;

    public class CodeBlock : StatementBase
    {
        public List<StatementBase> Statements { get; set; }

        public void Split(StatementBase stm)
        {
            var left = new CodeBlock { Namespace = Namespace, Node = Node, Statements = Statements.TakeWhile(x => x != stm).ToList() };
            var right = new CodeBlock { Namespace = Namespace, Node = Node, Statements = Statements.ToArray().Reverse().TakeWhile(x => x != stm).Reverse().ToList() };
            Statements = new List<StatementBase> { left, right };
            Children.AddRange(new[] {left, right});
        }
        
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
        

    }
}
