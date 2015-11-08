using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language.Semantic
{
    using TinyPG;

    class ParseNodeWithSemantic : ParseNode
    {
        public ParseNodeWithSemantic(Token token, string text)
            : base(token, text)
        {
        }

        public override ParseNode CreateNode(Token token, string text)
        {
            ParseNode node = new ParseNodeWithSemantic(token, text);
            node.Parent = this;
            return node;
        }

        /// <summary>
        /// Rule: Start -> (Program)? EOF ;
        /// </summary>
        protected override object EvalStart(ParseTree tree, params object[] paramlist)
        {
            var program = nodes.FirstOrDefault(n => n.Token.Type == TokenType.Program);
            if (program != null)
            {
                return program.Eval(tree);
            }
            return "Nothing found";
        }

        /// <summary>
        /// Rule: Program -> Member (NEWLINE (Member)? )* ;
        /// </summary>
        protected override object EvalProgram(ParseTree tree, params object[] paramlist)
        {
            return nodes.Where(n => n.Token.Type == TokenType.Member).Aggregate("", (current, member) => current + (member.Eval(tree).ToString() + "\r\n"));
        }

        /// <summary>
        /// Rule: Member -> (Globalvar | Function);
        /// </summary>
        protected override object EvalMember(ParseTree tree, params object[] paramlist)
        {
            return nodes.First().Eval(tree);
        }

        /// <summary>
        /// Rule: Globalvar -> GLOBAL IDENTIFIER (ASSIGN Literal )? ;
        /// </summary>
        protected override object EvalGlobalvar(ParseTree tree, params object[] paramlist)
        {
            return base.EvalGlobalvar(tree, paramlist);
        }

        /// <summary>
        /// Rule: Function -> IDENTIFIER (BROPEN Parameters BRCLOSE )? (ARROW Expr  | Statements) ;
        /// </summary>
        protected override object EvalFunction(ParseTree tree, params object[] paramlist)
        {
            var expr = nodes.SingleOrDefault(n => n.Token.Type == TokenType.Expr);
            if (expr != null)
            {
                return expr.Eval(tree);
            }
            else return "not supported";
        }

        /// <summary>
        /// Rule: Parameters -> IDENTIFIER (COMMA IDENTIFIER )* ;
        /// </summary>
        protected override object EvalParameters(ParseTree tree, params object[] paramlist)
        {
            return base.EvalParameters(tree, paramlist);
        }

        /// <summary>
        /// Rule: Statements -> (Statement (NEWLINE (Statement)? )* )? END ;
        /// </summary>
        protected override object EvalStatements(ParseTree tree, params object[] paramlist)
        {
            return base.EvalStatements(tree, paramlist);
        }

        /// <summary>
        /// Rule: Statement -> (IfStm | WhileStm | DoStm | ForStm | ReturnStm | CallOrAssign | OperStm);
        /// </summary>
        protected override object EvalStatement(ParseTree tree, params object[] paramlist)
        {
            return base.EvalStatement(tree, paramlist);
        }

        /// <summary>
        /// Rule: IfStm -> IF Expr Statements (ELSE Statements )? ;
        /// </summary>
        protected override object EvalIfStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalIfStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: WhileStm -> WHILE (Expr)? Statements ;
        /// </summary>
        protected override object EvalWhileStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalWhileStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: DoStm -> DO Statements WHILE Expr ;
        /// </summary>
        protected override object EvalDoStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalDoStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: ForStm -> FOR Variable Assign TO Expr (INCBY Expr )? Statements ;
        /// </summary>
        protected override object EvalForStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalForStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: ReturnStm -> RETURN (Expr)? ;
        /// </summary>
        protected override object EvalReturnStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalReturnStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: OperStm -> OPER (Call)? ;
        /// </summary>
        protected override object EvalOperStm(ParseTree tree, params object[] paramlist)
        {
            return base.EvalOperStm(tree, paramlist);
        }

        /// <summary>
        /// Rule: CallOrAssign -> Variable (Assign)? ;
        /// </summary>
        protected override object EvalCallOrAssign(ParseTree tree, params object[] paramlist)
        {
            return base.EvalCallOrAssign(tree, paramlist);
        }

        /// <summary>
        /// Rule: Assign -> ASSIGN Expr ;
        /// </summary>
        protected override object EvalAssign(ParseTree tree, params object[] paramlist)
        {
            return base.EvalAssign(tree, paramlist);
        }

        /// <summary>
        /// Rule: Variable -> IDENTIFIER ((Array | Call))? ;
        /// </summary>
        protected override object EvalVariable(ParseTree tree, params object[] paramlist)
        {
            return base.EvalVariable(tree, paramlist);
        }

        /// <summary>
        /// Rule: Array -> SQOPEN Expr SQCLOSE ;
        /// </summary>
        protected override object EvalArray(ParseTree tree, params object[] paramlist)
        {
            return base.EvalArray(tree, paramlist);
        }

        /// <summary>
        /// Rule: Call -> BROPEN (Arguments)? BRCLOSE ;
        /// </summary>
        protected override object EvalCall(ParseTree tree, params object[] paramlist)
        {
            return base.EvalCall(tree, paramlist);
        }

        /// <summary>
        /// Rule: Arguments -> Expr (COMMA Expr )* ;
        /// </summary>
        protected override object EvalArguments(ParseTree tree, params object[] paramlist)
        {
            return base.EvalArguments(tree, paramlist);
        }

        /// <summary>
        /// Rule: Literal -> (INTEGER | DOUBLE | STRING | BOOL);
        /// </summary>
        protected override object EvalLiteral(ParseTree tree, params object[] paramlist)
        {
            return Convert.ToInt32(nodes[0].Token.Text);
        }

        /// <summary>
        /// Rule: Expr -> OrExpr (QUESTION Expr COLON Expr )? ;
        /// </summary>
        protected override object EvalExpr(ParseTree tree, params object[] paramlist)
        {
            return nodes.First().Eval(tree);
        }

        /// <summary>
        /// Rule: OrExpr -> AndExpr (OR AndExpr )* ;
        /// </summary>
        protected override object EvalOrExpr(ParseTree tree, params object[] paramlist)
        {
            return nodes.First().Eval(tree);
        }

        /// <summary>
        /// Rule: AndExpr -> NotExpr (AND NotExpr )* ;
        /// </summary>
        protected override object EvalAndExpr(ParseTree tree, params object[] paramlist)
        {
            return nodes.First().Eval(tree);
        }

        /// <summary>
        /// Rule: NotExpr -> (NOT)? CompExpr ;
        /// </summary>
        protected override object EvalNotExpr(ParseTree tree, params object[] paramlist)
        {
            return nodes.First().Eval(tree);
        }

        /// <summary>
        /// Rule: CompExpr -> AddExpr (COMP AddExpr )? ;
        /// </summary>
        protected override object EvalCompExpr(ParseTree tree, params object[] paramlist)
        {
            return nodes.First().Eval(tree);
        }

        /// <summary>
        /// Rule: AddExpr -> MultExpr ((PLUSMINUS) MultExpr )* ;
        /// </summary>
        protected override object EvalAddExpr(ParseTree tree, params object[] paramlist)
        {
            var result = Convert.ToInt32(nodes[0].Eval(tree));
            for (var i = 1; i < nodes.Count; i += 2)
            {
                if (nodes[i].Token.Text == "+")
                {
                    result += Convert.ToInt32(nodes[i + 1].Eval(tree));
                }
                else
                {
                    result -= Convert.ToInt32(nodes[i + 1].Eval(tree));
                }
            }
            return result;
        }

        /// <summary>
        /// Rule: MultExpr -> PowExpr ((MULTDIV) PowExpr )* ;
        /// </summary>
        protected override object EvalMultExpr(ParseTree tree, params object[] paramlist)
        {
            var result = Convert.ToInt32(nodes[0].Eval(tree));
            for (var i = 1; i < nodes.Count; i += 2)
            {
                if (nodes[i].Token.Text == "*")
                {
                    result *= Convert.ToInt32(nodes[i + 1].Eval(tree));
                }
                else
                {
                    result /= Convert.ToInt32(nodes[i + 1].Eval(tree));
                }
            }
            return result;
        }

        /// <summary>
        /// Rule: PowExpr -> UnaryExpr (POW UnaryExpr )* ;
        /// </summary>
        protected override object EvalPowExpr(ParseTree tree, params object[] paramlist)
        {
            var result = Convert.ToInt32(nodes[0].Eval(tree));
            for (var i = 1; i < nodes.Count; i += 2)
            {
                result = (int)Math.Pow(result, Convert.ToInt32(nodes[i + 1].Eval(tree)));
            }
            return result;
        }

        /// <summary>
        /// Rule: UnaryExpr -> ((UNARYOP))? Atom ;
        /// </summary>
        protected override object EvalUnaryExpr(ParseTree tree, params object[] paramlist)
        {
            var str = nodes.Single(n => n.Token.Type == TokenType.Atom).Eval(tree);
            var result = Convert.ToInt32(str);
            var unaryop = nodes.FirstOrDefault(n => n.Token.Type == TokenType.UNARYOP);
            if (unaryop == null)
            {
                return result;
            }
            if (unaryop.Token.Text == "-")
            {
                result = -result;
            }
            return result;
        }

        /// <summary>
        /// Rule: Atom -> (Literal | Variable | BROPEN Expr BRCLOSE );
        /// </summary>
        protected override object EvalAtom(ParseTree tree, params object[] paramlist)
        {
            return nodes.Count == 1 ?nodes[0].Eval(tree): nodes[1].Eval(tree);
        }
    }
}
