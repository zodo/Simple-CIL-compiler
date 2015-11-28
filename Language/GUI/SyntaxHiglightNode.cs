namespace Language.GUI
{
    using System;

    using TinyPG;

    public class SyntaxHiglightNode : ParseNode
    {
        public SyntaxHiglightNode(Token token, string text)
            : base(token, text)
        {
        }

        /// <summary>
        /// Rule: Start -> (Program)? EOF ;
        /// </summary>
        protected override object EvalStart(ParseTree tree, params object[] paramlist)
        {
            foreach (var node in Nodes)
                node.Eval(tree, paramlist);
            return null;
        }

        /// <summary>
        /// Rule: Statements -> (Statement (NEWLINE (Statement)? )* )? END ;
        /// </summary>
        protected override object EvalStatements(ParseTree tree, params object[] paramlist)
        {
            var region = new CodeRegion
            {
                Start = Token.StartPos,
                End = Token.EndPos,
                Name = Guid.NewGuid().ToString()
            };
            var tr = (SyntaxHiglightParseTree)tree;
            tr.CodeRegions.Add(region);
            return base.EvalStatements(tree, paramlist);
        }

        public override ParseNode CreateNode(Token token, string text)
        {
            ParseNode node = new SyntaxHiglightNode(token, text);
            node.Parent = this;
            return node;
        }
    }
}
