namespace Language.AST.Expressions
{
    using System;
    using System.Windows.Forms;

    using Semantic;
    using Semantic.Data;

    using TinyPG;

    public class MultPowExpr : ExpressionBase
    {
        public override SymbolType GetExprType()
        {
            if (Types.Contains(SymbolType.Bool) || Types.Contains(SymbolType.String))
            {
                throw new ParseException($"Невозможно произвести операцию между типами {FirstType} и {SecondType}", Node);
            }
            if (Types.Contains(SymbolType.Double))
            {
                return SymbolType.Double;
            }
            return SymbolType.Integer;
           
        }

        public override TreeNode GetNodes()
        {
            var node = base.GetNodes();
            node.Text = Type.ToString();
            return node;
        }

        public MultPowDivType Type
        {
            get
            {
                switch (OperationText)
                {
                    case "/":return MultPowDivType.Div;
                    case "//": return MultPowDivType.IntDiv;
                    case "%": return MultPowDivType.Mod;
                    case "*": return MultPowDivType.Mult;
                    case "^": return MultPowDivType.Pow;
                    default: throw new ParseException("Неправильный тип операции", Node);
                }
            }
        }
    }

    //MULTDIV	-> @"\*|/|\%|//";
    public enum MultPowDivType
    {
        Mult,
        Div,
        IntDiv,
        Mod,
        Pow
    }
}
