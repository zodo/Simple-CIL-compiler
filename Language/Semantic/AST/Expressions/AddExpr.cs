namespace Language.AST.Expressions
{
    using System.Windows.Forms;

    using Semantic;
    using Semantic.Data;

    class AddExpr : ExpressionBase
    {
        public override SymbolType GetExprType()
        {
            if (Types.Contains(SymbolType.Bool))
            {
                throw new ParseException("Невозможно применить эту операцию к Bool", Node);
            }
            if (Types.Contains(SymbolType.String))
            {
                return SymbolType.String;
            }
            if (Types.Contains(SymbolType.Double))
            {
                return SymbolType.Double;
            }
            return SymbolType.Integer;
           
        }


        public AddType Type
        {
            get
            {
                switch (OperationText)
                {
                    case "+":return AddType.Plus;
                    case "-":return AddType.Minus;
                    default: throw new ParseException("Неправильный тип сложения", Node);
                }
            }
        }

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
            node.Text = Type.ToString();
            return node;
        }
    }

    public enum AddType
    {
        Plus,
        Minus
    }
}
