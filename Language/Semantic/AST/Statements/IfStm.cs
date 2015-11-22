namespace Language.AST.Statements
{
    using System.Windows.Forms;

    using Semantic;
    using Semantic.Data;

    using TinyPG;

    public class IfStm : StatementBase
    {
        private ExpressionBase _condition;

        public ExpressionBase Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                var exprType = value.GetExprType();
                if (exprType != SymbolType.Bool)
                {
                    throw new ParseException("Условие должно иметь тип Boolean", Node);
                }
                _condition = value;
            }
        }

        public CodeBlock IfTrue { get; set; }

        public CodeBlock IfFalse { get; set; }

        public override TreeNode GetNodes()
        {
            var node = new TreeNode("IfStatement");
            node.Nodes.Add(new TreeNode("Condition") { Nodes = { Condition.GetNodes() } });
            node.Nodes.Add(new TreeNode("IfTrueStatements") { Nodes = { IfTrue.GetNodes() } });
            if (IfFalse != null)
            {
                node.Nodes.Add(new TreeNode("IfFalseStatements") { Nodes = { IfFalse.GetNodes() } });
            }
            return node;
        }
    }
}
