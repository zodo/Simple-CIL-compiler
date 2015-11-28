namespace Language.Semantic.ASTVisitor
{
    using System.Linq;
    using System.Windows.Forms;

    using AST;
    using AST.Expressions;
    using AST.LeftExprSide;

    using Language.AST;
    using Language.AST.Expressions;
    using Language.AST.Statements;

    public class MakeTreeVisitor : IAstVisitor
    {
        public dynamic Visit(Program program)
        {
            var node = new TreeNode("Program");
            var globvarnode = new TreeNode("GlobalVariables");
            globvarnode.Nodes.AddRange(program.GlobalVariables.Select(x => x.Accept(this)).Cast<TreeNode>().ToArray());
            node.Nodes.Add(globvarnode);
            var funcimpl = new TreeNode("FuncImpls");
            funcimpl.Nodes.AddRange(program.FuncImplementations.Select(x => x.Accept(this)).Cast<TreeNode>().ToArray());
            node.Nodes.Add(funcimpl);
            return node;
        }

        public dynamic Visit(GlobalVariable variable)
        {
            return new TreeNode($"{variable.Value.Type}:{variable.Value.Name}") { Tag = variable.Node };
        }

        public dynamic Visit(FuncImplementation impl)
        {
            var node = new TreeNode($"{impl.Name} Implementation") { Tag = impl.Node };
            var argNode = new TreeNode("Arguments");
            argNode.Nodes.AddRange(impl.ArgumentsTypes.Select(x => new TreeNode(x.ToString())).ToArray());
            node.Nodes.Add(argNode);
            if (impl.ReturnExpression != null)
            {
                node.Nodes.Add(new TreeNode("ReturnExpr") { Nodes = { Visit(impl.ReturnExpression) } });
            }
            if (impl.Code != null)
            {
                node.Nodes.Add(new TreeNode("Code") { Nodes = { Visit(impl.Code) } });
            }
            node.Nodes.Add(new TreeNode("ReturnType") { Nodes = { new TreeNode(impl.ReturnType.ToString()) } });
            return node;
        }

        public dynamic Visit(FuncDeclaration decl)
        {
            var node = new TreeNode($"{decl.Name} Declaration") { Tag = decl.Node };
            node.Nodes.Add(new TreeNode("ArgumentsNames") { Nodes = { new TreeNode(string.Join(", ", decl.Arguments)) } });
            return node;
        }

        public dynamic Visit(Arguments arg)
        {
            var node = new TreeNode("Arguments") { Tag = arg.Node };
            if (arg.Values != null && arg.Values.Any())
            {
                node.Nodes.AddRange(arg.Values.Select(x => Visit((dynamic)x)).Cast<TreeNode>().ToArray());
            }
            return node;
        }

        public dynamic Visit(CallOrAssign stm)
        {
            var node = new TreeNode("CallOrAssign") { Tag = stm.Node };
            node.Nodes.Add(new TreeNode("LeftSide") { Nodes = { Visit((dynamic)stm.LeftSideExpr) } });
            if (stm.AssignExpression != null)
            {
                node.Nodes.Add(new TreeNode("AssignExpr") { Nodes = { Visit((dynamic)stm.AssignExpression) } });
            }
            return node;
        }

        public dynamic Visit(CodeBlock code)
        {
            var node = new TreeNode("CodeBlock") { Tag = code.Node };
            node.Nodes.AddRange(code.Statements.Cast<dynamic>().Select(x => Visit(x)).Cast<TreeNode>().ToArray());
            return node;
        }

        public dynamic Visit(DoWhileStm stm)
        {
            var node = new TreeNode(stm.Type.ToString()) { Tag = stm.Node };
            node.Nodes.Add(new TreeNode("Condition") { Nodes = { Visit((dynamic)stm.Condition) } });
            node.Nodes.Add(new TreeNode("Statements") { Nodes = { Visit(stm.Statements) } });
            return node;
        }

        public dynamic Visit(ForStm stm)
        {
            var node = new TreeNode("ForLoop") { Tag = stm.Node };
            node.Nodes.Add(new TreeNode("Variable") { Nodes = { Visit(stm.Variable) } });
            node.Nodes.Add(new TreeNode("AssignExpression") { Nodes = { Visit((dynamic)stm.AssignExpression) } });
            node.Nodes.Add(new TreeNode("ToExpression") { Nodes = { Visit((dynamic)stm.ToExpression) } });
            if (stm.IncByExpression != null)
            {
                node.Nodes.Add(new TreeNode("IncByExpression") { Nodes = { Visit((dynamic)stm.IncByExpression) } });
            }
            node.Nodes.Add(new TreeNode("Statements") { Nodes = { Visit(stm.Statements) } });
            return node;
        }

        public dynamic Visit(IfStm stm)
        {
            var node = new TreeNode("IfStatement") { Tag = stm.Node };
            node.Nodes.Add(new TreeNode("Condition") { Nodes = { Visit((dynamic)stm.Condition) } });
            if (stm.IfTrue != null)
            {
                node.Nodes.Add(new TreeNode("IfTrueStatements") { Nodes = { Visit(stm.IfTrue) } });
            }
            if (stm.IfFalse != null)
            {
                node.Nodes.Add(new TreeNode("IfFalseStatements") { Nodes = { Visit(stm.IfFalse) } });
            }
            return node;
        }

        public dynamic Visit(OperStm stm)
        {
            var node = new TreeNode(stm.Operation) { Tag = stm.Node };
            if (stm.Arguments != null)
            {
                node.Nodes.Add(Visit(stm.Arguments));
            }
            return node;
        }

        public dynamic Visit(ReturnStm stm)
        {
            var node = new TreeNode("ReturnStm") { Tag = stm.Node };
            if (stm.ReturnExpression != null)
            {
                node.Nodes.Add(new TreeNode("ReturnExpr") { Nodes = { Visit((dynamic)stm.ReturnExpression) } });
            }
            return node;
        }

        public dynamic Visit(StatementBase statement)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(LeftSideExprArray left)
        {
            var node = Visit((LeftSideExprBase)left);
            node.Nodes.Add(new TreeNode($"Array keys type : {left.ArrayKeyType}"));
            node.Nodes.Add(new TreeNode("Index") { Nodes = { Visit((dynamic)left.Index) } });
            return node;
        }

        public dynamic Visit(LeftSideExprBase left)
        {
            return new TreeNode($"{left.Type}:{left.Name}") { Tag = left.Node };
        }

        public dynamic Visit(LeftSideExprCall left)
        {
            var node = Visit((LeftSideExprBase)left);
            node.Nodes.Add(Visit(left.CallFunc));
            return node;
        }

        public dynamic Visit(LeftSideExprVariable left)
        {
            var node = Visit((LeftSideExprBase)left);
            node.Nodes.Add(new TreeNode($"Variable type : {left.VariableType}"));
            return node;
        }

        public dynamic Visit(AddExpr expr)
        {
            var node = new TreeNode("Expression") { Tag = expr.Node };
            if (expr.First != null)
            {
                node.Nodes.Add(Visit((dynamic)expr.First));
            }
            if (expr.Second != null)
            {
                node.Nodes.Add(Visit((dynamic)expr.Second));
            }
            if (expr.Third != null)
            {
                node.Nodes.Add(Visit((dynamic)expr.Third));
            }
            node.Text = expr.Type.ToString();
            return node;
        }

        public dynamic Visit(AndOrExpr expr)
        {
            var node = Visit((ExpressionBase)expr);
            node.Text = expr.Type.ToString();
            return node;
        }

        public dynamic Visit(CallFuncExpr expr)
        {
            var node = new TreeNode("Function Call") { Tag = expr.Node };
            if (expr.Arguments != null)
            {
                node.Nodes.Add(Visit(expr.Arguments));
            }
            return node;
        }

        public dynamic Visit(CompareExpr expr)
        {
            var node = Visit((ExpressionBase)expr);
            node.Text = expr.Type.ToString();
            return node;
        }

        public dynamic Visit(ConsoleReadExpr expr)
        {
            return new TreeNode($"Console read {expr.Type} function");
        }

        public dynamic Visit(ExpressionBase expr)
        {
            var node = new TreeNode("Expression") { Tag = expr.Node };
            if (expr.First != null)
            {
                node.Nodes.Add(Visit((dynamic)expr.First));
            }
            if (expr.Second != null)
            {
                node.Nodes.Add(Visit((dynamic)expr.Second));
            }
            if (expr.Third != null)
            {
                node.Nodes.Add(Visit((dynamic)expr.Third));
            }
            return node;
        }

        public dynamic Visit(GetArrayExpr expr)
        {
            var node = new TreeNode("Get array value") { Tag = expr.Node };
            node.Nodes.Add(new TreeNode($"Name: {expr.Name}"));
            node.Nodes.Add(new TreeNode($"Keys {expr.KeysType}"));
            node.Nodes.Add(new TreeNode($"Values {expr.ValuesType}"));
            node.Nodes.Add(new TreeNode("Index") { Nodes = { Visit(expr.Index) } });
            return node;
        }

        public dynamic Visit(GetVariableExpr expr)
        {
            var node = new TreeNode("Get variable value") { Tag = expr.Node };
            node.Nodes.Add(new TreeNode($"Name: {expr.Name}"));
            node.Nodes.Add(new TreeNode($"Type: {expr.Type}"));
            return node;
        }

        public dynamic Visit(LiteralExpr expr)
        {
            var node = Visit((ExpressionBase)expr);
            node.Text = expr.Value;
            return node;
        }

        public dynamic Visit(MultPowExpr expr)
        {
            var node = Visit((ExpressionBase)expr);
            node.Text = expr.Type.ToString();
            return node;
        }

        public dynamic Visit(NotExpr expr)
        {
            var node = Visit((ExpressionBase)expr);
            node.Text = "NOT";
            return node;
        }

        public dynamic Visit(TernaryExpr expr)
        {
            var node = Visit((ExpressionBase)expr);
            node.Text = "Ternary";
            return node;
        }

        public dynamic Visit(UnaryExpr expr)
        {
            var node = Visit((ExpressionBase)expr);
            node.Text = expr.Type.ToString();
            return node;
        }
    }
}
