using System;
using System.Linq;

namespace Language.Semantic
{
    using System.Collections.Generic;
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
            Namespaces.Reset();

            var program = GetNode(TokenType.Program);
            program?.Eval(tree);

            return null;
        }

        /// <summary>
        /// Rule: Program -> Member (NEWLINE (Member)? )* ;
        /// </summary>
        protected override object EvalProgram(ParseTree tree, params object[] paramlist)
        {
            foreach (var parseNode in nodes.OfTokenType(TokenType.Member))
            {
                Namespaces.Current = Namespaces.Root;
                parseNode.Eval(tree);
            }
            return null;
        }

        /// <summary>
        /// Rule: Member -> (Globalvar | Function);
        /// </summary>
        protected override object EvalMember(ParseTree tree, params object[] paramlist)
        {
            nodes.First().Eval(tree);

            return null;
        }

        /// <summary>
        /// Rule: Globalvar -> GLOBAL IDENTIFIER (ASSIGN Symbol )? ;
        /// </summary>
        protected override object EvalGlobalvar(ParseTree tree, params object[] paramlist)
        {
            var identNode = GetNode(TokenType.IDENTIFIER);
            var varName = identNode.Token.Text;
           
            var literalNode = GetNode(TokenType.Literal);
            if (literalNode != null)
            {
                var literal = (Symbol)literalNode.Eval(tree);
                literal.Name = varName;
                Namespaces.Current.AddSymbol(literal, identNode);
            }
            else
            {
                Namespaces.Current.AddSymbol(new Symbol(SymbolType.Unknown, "", varName), identNode);
            }
            return null;
        }

        /// <summary>
        /// Rule: Function -> IDENTIFIER (BROPEN Parameters BRCLOSE )? (ARROW Expr  | Statements) ;
        /// </summary>
        protected override object EvalFunction(ParseTree tree, params object[] paramlist)
        {
            var funcName = GetNode(TokenType.IDENTIFIER).Token.Text;
            
            Namespaces.Current.AddSymbol(new Symbol(SymbolType.Function, "", funcName), GetNode(TokenType.IDENTIFIER));

            Namespaces.LevelDown(new Namespace(funcName));

            GetNode(TokenType.Parameters)?.Eval(tree);

            GetNode(TokenType.Expr)?.Eval(tree);
            
            GetNode(TokenType.Statements)?.Eval(tree);

            Namespaces.LevelUp();

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
                Namespaces.Current.AddSymbol(new Symbol(SymbolType.Unknown, "", identName), identNode);
            }
            return null;
        }

        /// <summary>
        /// Rule: Statements -> (Statement (NEWLINE (Statement)? )* )? END ;
        /// </summary>
        protected override object EvalStatements(ParseTree tree, params object[] paramlist)
        {
            Namespaces.LevelDown(new Namespace());
            foreach (var parseNode in nodes.OfTokenType(TokenType.Statement))
            {
                parseNode.Eval(tree);
            }
            Namespaces.LevelUp();
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
            var assignNode = GetNode(TokenType.Assign);
            var stype = assignNode?.Eval(tree) ?? SymbolType.Unknown;

            var isFunc = ((int)GetNode(TokenType.Variable).Eval(tree, stype)) == 1;

            

            if (isFunc && assignNode != null)
            {
                ParseTree.Instance.Errors.Add(new ParseError("Попытка присвоить значение вызову функции", 0x009, assignNode));
            }
            

            return null;
        }

        /// <summary>
        /// Rule: Assign -> ASSIGN Expr ;
        /// </summary>
        protected override object EvalAssign(ParseTree tree, params object[] paramlist)
        {
            return GetNode(TokenType.Expr).Eval(tree);
        }

        /// <summary>
        /// Rule: Variable -> IDENTIFIER ((Array | Call))? ;
        /// </summary>
        protected override object EvalVariable(ParseTree tree, params object[] paramlist)
        {
            var identNode = GetNode(TokenType.IDENTIFIER);
            if (GetNode(TokenType.Call) != null)
            {
                if (Namespaces.Current.Symbols.Any(s => s.Type == SymbolType.Function && s.Name == identNode.Token.Text))
                {
                    return 1;
                }
                ParseTree.Instance.Errors.Add(new ParseError($"Попытка вызова {identNode.Token.Text}, функция не объявлена", 0x008, identNode));
                return 1;

            }
            else
            {
                Namespaces.Current.AddSymbol(new Symbol((SymbolType)paramlist[0], "", identNode.Token.Text), identNode);
                return 0;
            }
            
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
            // Определить тип, вернуть экземпляр Symbol
            var nodei = GetNode(TokenType.INTEGER);
            if (nodei != null)
            {
                return new Symbol(SymbolType.Integer, nodei.Token.Text, "");
            }

            var noded = GetNode(TokenType.DOUBLE);
            if (noded != null)
            {
                return new Symbol(SymbolType.Double, noded.Token.Text, "");
            }

            var nodestr = GetNode(TokenType.STRING);
            if (nodestr != null)
            {
                return new Symbol(SymbolType.String, nodestr.Token.Text, "");
            }

            var nodeb = GetNode(TokenType.BOOL);
            if (nodeb != null)
            {
                return new Symbol(SymbolType.Bool, nodeb.Token.Text, "");
            }
            return null;
        }

        /// <summary>
        /// Rule: Expr -> OrExpr (QUESTION Expr COLON Expr )? ;
        /// </summary>
        protected override object EvalExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.QUESTION) == null)
            {
                return (SymbolType)GetNode(TokenType.OrExpr).Eval(tree);
            }
            return SymbolType.Unknown;
        }

        /// <summary>
        /// Rule: OrExpr -> AndExpr (OR AndExpr )* ;
        /// </summary>
        protected override object EvalOrExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.OR) == null)
            {
                return (SymbolType)GetNode(TokenType.AndExpr).Eval(tree);
            }

            foreach (var parseNode in nodes.OfTokenType(TokenType.AndExpr))
            {
                var type = ((SymbolType)parseNode.Eval(tree));
                if (type != SymbolType.Bool && type != SymbolType.Unknown)
                {
                    ParseTree.Instance.Errors.Add(new ParseError("Попытка илить неслагаемое", 0, parseNode));
                }
            }

            return SymbolType.Bool;
        }

        /// <summary>
        /// Rule: AndExpr -> NotExpr (AND NotExpr )* ;
        /// </summary>
        protected override object EvalAndExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.AND) == null)
            {
                return (SymbolType)GetNode(TokenType.NotExpr).Eval(tree);
            }

            var types = new List<SymbolType>();
            foreach (var parseNode in nodes.OfTokenType(TokenType.NotExpr))
            {
                var type = ((SymbolType)parseNode.Eval(tree));
                if (type != SymbolType.Bool && type != SymbolType.Unknown)
                {
                    ParseTree.Instance.Errors.Add(new ParseError("Попытка слагать неслагаемое", 0, parseNode));
                }
            }

            return GetExprType(types);
        }

        /// <summary>
        /// Rule: NotExpr -> (NOT)? CompExpr ;
        /// </summary>
        protected override object EvalNotExpr(ParseTree tree, params object[] paramlist)
        {
            var evalNotExpr = (SymbolType)GetNode(TokenType.CompExpr).Eval(tree);
            if (GetNode(TokenType.NOT) == null)
            {
                return evalNotExpr;
            }
            if (evalNotExpr != SymbolType.Bool && evalNotExpr != SymbolType.Unknown)
            {
                ParseTree.Instance.Errors.Add(new ParseError("Попытка отрицать неотрицаемое", 0, GetNode(TokenType.NOT)));
            }
            return SymbolType.Bool;
        }

        /// <summary>
        /// Rule: CompExpr -> AddExpr (COMP AddExpr )? ;
        /// </summary>
        protected override object EvalCompExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.COMP) == null)
            {
                return (SymbolType)GetNode(TokenType.AddExpr).Eval(tree);
            }

            return SymbolType.Bool;
        }

        /// <summary>
        /// Rule: AddExpr -> MultExpr ((PLUSMINUS) MultExpr )* ;
        /// </summary>
        protected override object EvalAddExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.PLUSMINUS) == null)
            {
                return (SymbolType)GetNode(TokenType.MultExpr).Eval(tree);
            }

            var types = nodes.OfTokenType(TokenType.MultExpr).Select(parseNode => (SymbolType)parseNode.Eval(tree)).ToList();

            return GetExprType(types);
        }

        /// <summary>
        /// Rule: MultExpr -> PowExpr ((MULTDIV) PowExpr )* ;
        /// </summary>
        protected override object EvalMultExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.MULTDIV) == null)
            {
                return (SymbolType)GetNode(TokenType.PowExpr).Eval(tree);
            }
            
            var types = new List<SymbolType>();
            foreach (var parseNode in nodes.OfTokenType(TokenType.PowExpr))
            {
                var powType = (SymbolType)parseNode.Eval(tree);

                types.Add(powType);

                if (powType == SymbolType.String && parseNode != nodes.OfTokenType(TokenType.PowExpr).First())
                {
                    ParseTree.Instance.Errors.Add(new ParseError("Невозможно умножить или поделить на строку", 0, parseNode));
                }
            }

            return GetExprType(types);
        }

       

        /// <summary>
        /// Rule: PowExpr -> UnaryExpr (POW UnaryExpr )* ;
        /// </summary>
        protected override object EvalPowExpr(ParseTree tree, params object[] paramlist)
        {
            if (GetNode(TokenType.POW) == null)
            {
                return (SymbolType)GetNode(TokenType.UnaryExpr).Eval(tree);
            }
            var types = new List<SymbolType>();
            foreach (var parseNode in nodes.OfTokenType(TokenType.UnaryExpr))
            {
                var unaryType = (SymbolType)parseNode.Eval(tree);

                types.Add(unaryType);

                if (unaryType == SymbolType.String)
                {
                    ParseTree.Instance.Errors.Add(new ParseError("Невозможно применить операцию возведения в степень к строке", 0, parseNode));
                }
            }

            return GetExprType(types);
        }

        /// <summary>
        /// Rule: UnaryExpr -> ((UNARYOP))? Atom ;
        /// </summary>
        protected override object EvalUnaryExpr(ParseTree tree, params object[] paramlist)
        {
            var atomType = (SymbolType)GetNode(TokenType.Atom).Eval(tree);

            var unaryNode = GetNode(TokenType.UNARYOP);

            if (atomType == SymbolType.String && unaryNode != null)
            {
                ParseTree.Instance.Errors.Add(new ParseError($"Невозможно применить унарную операцию к строке", 0, unaryNode));
            }

            return atomType;
        }

        /// <summary>
        /// Rule: Atom -> (Literal | Variable | BROPEN Expr BRCLOSE );
        /// </summary>
        protected override object EvalAtom(ParseTree tree, params object[] paramlist)
        {
            var literalNode = GetNode(TokenType.Literal);
            if (literalNode != null)
            {
                return ((Symbol)literalNode.Eval(tree)).Type;
            }

            var variableNode = GetNode(TokenType.Variable);
            if (variableNode != null)
            {
                return SymbolType.Unknown;
            }

            var exprNode = GetNode(TokenType.Expr);
            if (exprNode != null)
            {
                return ((Symbol)exprNode.Eval(tree)).Type;
            }
            return SymbolType.Unknown;
        }

        private static object GetExprType(List<SymbolType> types)
        {
            if (types.Contains(SymbolType.Unknown))
            {
                return SymbolType.Unknown;
            }

            if (types.Contains(SymbolType.String))
            {
                return SymbolType.String;
            }

            if (types.Contains(SymbolType.Double))
            {
                return SymbolType.Double;
            }

            if (types.Contains(SymbolType.Integer))
            {
                return SymbolType.Integer;
            }

            return SymbolType.Bool;
        }
    }
}
