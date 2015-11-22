namespace Language.AST.Expressions
{
    using System;
    using System.Windows.Forms;

    using Semantic;
    using Semantic.Data;

    class TernaryExpr : ExpressionBase
    {
        public override SymbolType GetExprType()
        {
            if (FirstType != SymbolType.Bool)
            {
                throw new ParseException("Тип условия должен быть bool", Node);
            }
            if (Third == null)
            {
                return SecondType;
            }
            if (SecondType != Third.GetExprType())
            {
                throw new ParseException("Типы левой и правой части должны быть одинаковы", Node);
            }
            return SecondType;
        }

        public override TreeNode GetNodes()
        {
            var node = base.GetNodes();
            node.Text = "Ternary";
            return node;
        }
    }
}
