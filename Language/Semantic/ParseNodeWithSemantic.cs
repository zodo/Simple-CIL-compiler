using System;
using System.Linq;

namespace Language.Semantic
{
    using System.Linq.Expressions;

    using Data;

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

        private ParseNode GetNode(TokenType type)
        {
            return nodes.OfTokenType(type).FirstOrDefault();
        }

        /// <summary>
        /// Rule: Start -> (Program)? EOF ;
        /// </summary>
        protected override object EvalStart(ParseTree tree, params object[] paramlist)
        {
            IdentifierTable.Reset();
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
            foreach (var parseNode in nodes.OfTokenType(TokenType.Member))
            {
                IdentifierTable.CurrentNamespace = IdentifierTable.RootNamespace;
                parseNode.Eval(tree);
            }
            return null;
        }

        /// <summary>
        /// Rule: Member -> (Globalvar | Function);
        /// </summary>
        protected override object EvalMember(ParseTree tree, params object[] paramlist)
        {
            foreach (var parseNode in nodes)
            {
                parseNode.Eval(tree);
            }
            return null;
        }

        /// <summary>
        /// Rule: Globalvar -> GLOBAL IDENTIFIER (ASSIGN Literal )? ;
        /// </summary>
        protected override object EvalGlobalvar(ParseTree tree, params object[] paramlist)
        {
            var globVarName = GetNode(TokenType.IDENTIFIER).Token.Text;
           
            var literalNode = GetNode(TokenType.Literal);
            if (literalNode != null)
            {
                var literal = (Literal)literalNode.Eval(tree);
                literal.Name = globVarName;
                IdentifierTable.CurrentNamespace.AddLiteral(literal, this);
            }
            else
            {
                IdentifierTable.CurrentNamespace.AddLiteral(new Literal(LiteralType.Unknown, "", globVarName), this);
            }
            return null;
        }

        /// <summary>
        /// Rule: Function -> IDENTIFIER (BROPEN Parameters BRCLOSE )? (ARROW Expr  | Statements) ;
        /// </summary>
        protected override object EvalFunction(ParseTree tree, params object[] paramlist)
        {
            var funcName = GetNode(TokenType.IDENTIFIER).Token.Text;
            
            IdentifierTable.CurrentNamespace.AddLiteral(new Literal(LiteralType.Function, "", funcName), this);

            IdentifierTable.Down(new Namespace(funcName));

            GetNode(TokenType.Parameters)?.Eval(tree);

            var exprNode = GetNode(TokenType.Expr);
            exprNode?.Eval(tree);

            var stateNode = GetNode(TokenType.Statements);
            stateNode?.Eval(tree);

            IdentifierTable.Up();

            return null;
        }

        /// <summary>
        /// Rule: Parameters -> IDENTIFIER (COMMA IDENTIFIER )* ;
        /// </summary>
        protected override object EvalParameters(ParseTree tree, params object[] paramlist)
        {
            foreach (var identNode in nodes.OfTokenType(TokenType.IDENTIFIER))
            {
                var identName = identNode.Token.Text;
                IdentifierTable.CurrentNamespace.AddLiteral(new Literal(LiteralType.Unknown, "", identName), this);
            }
            return null;
        }

        /// <summary>
        /// Rule: Statements -> (Statement (NEWLINE (Statement)? )* )? END ;
        /// </summary>
        protected override object EvalStatements(ParseTree tree, params object[] paramlist)
        {
            IdentifierTable.Down(new Namespace(Guid.NewGuid().ToString()));
            foreach (var parseNode in nodes.OfTokenType(TokenType.Statement))
            {
                parseNode.Eval(tree);
            }
            IdentifierTable.Up();
            return null;
        }

        /// <summary>
        /// Rule: Statement -> (IfStm | WhileStm | DoStm | ForStm | ReturnStm | CallOrAssign | OperStm);
        /// </summary>
        protected override object EvalStatement(ParseTree tree, params object[] paramlist)
        {
            nodes.FirstOrDefault()?.Eval(tree);
            return null;
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
            GetNode(TokenType.Variable)?.Eval(tree);
            return null;
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
            IdentifierTable.CurrentNamespace.AddLiteral(new Literal(LiteralType.Unknown, "", GetNode(TokenType.IDENTIFIER).Token.Text), this);
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
            // Определить тип, вернуть экземпляр Literal
            var nodei = GetNode(TokenType.INTEGER);
            if (nodei != null)
            {
                return new Literal(LiteralType.Integer, nodei.Token.Text, "");
            }

            var noded = GetNode(TokenType.DOUBLE);
            if (noded != null)
            {
                return new Literal(LiteralType.Double, noded.Token.Text, "");
            }

            var nodestr = GetNode(TokenType.STRING);
            if (nodestr != null)
            {
                return new Literal(LiteralType.String, nodestr.Token.Text, "");
            }

            var nodeb = GetNode(TokenType.BOOL);
            if (nodeb != null)
            {
                return new Literal(LiteralType.Bool, nodeb.Token.Text, "");
            }
            return null;
        }

        /// <summary>
        /// Rule: Expr -> OrExpr (QUESTION Expr COLON Expr )? ;
        /// </summary>
        protected override object EvalExpr(ParseTree tree, params object[] paramlist)
        {
            return null;
        }

        /// <summary>
        /// Rule: OrExpr -> AndExpr (OR AndExpr )* ;
        /// </summary>
        protected override object EvalOrExpr(ParseTree tree, params object[] paramlist)
        {
            return null;
        }

        /// <summary>
        /// Rule: AndExpr -> NotExpr (AND NotExpr )* ;
        /// </summary>
        protected override object EvalAndExpr(ParseTree tree, params object[] paramlist)
        {
            return null;
        }

        /// <summary>
        /// Rule: NotExpr -> (NOT)? CompExpr ;
        /// </summary>
        protected override object EvalNotExpr(ParseTree tree, params object[] paramlist)
        {
            return null; 
        }

        /// <summary>
        /// Rule: CompExpr -> AddExpr (COMP AddExpr )? ;
        /// </summary>
        protected override object EvalCompExpr(ParseTree tree, params object[] paramlist)
        {
            return null;
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
