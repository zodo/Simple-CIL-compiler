namespace Language.AST
{
    using System.Collections.Generic;
    using System.Windows.Forms;

    using Semantic.Data;

    public abstract class ExpressionBase : AstBase
    {
       public ExpressionBase First { get; set; }

        public ExpressionBase Second { get; set; }

        public ExpressionBase Third { get; set; }

        public virtual SymbolType GetExprType()
        {
            return FirstType;
        }

        protected SymbolType FirstType => First?.GetExprType() ?? SymbolType.Unknown;

        protected SymbolType SecondType => Second?.GetExprType() ?? SymbolType.Unknown;

        protected ICollection<SymbolType> Types => new[] { FirstType, SecondType };

        public string OperationText { get; set; }

        public override TreeNode GetNodes()
        {
            var node = new TreeNode("Expression");
            if (First != null)
            {
                node.Nodes.Add(First.GetNodes());
            }
            if (Second != null)
            {
                node.Nodes.Add(Second.GetNodes());
            }
            if (Third != null)
            {
                node.Nodes.Add(Third.GetNodes());
            }
            return node;
        }
    }

    
    public enum Operations
    {
        Plus,
        Minus,
        Mult,
        Div,
        UnaryMinus
    }

}
