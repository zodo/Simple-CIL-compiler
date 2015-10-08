namespace Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using TinyPG;

    public class SourceCodeHandler
    {
        private readonly string _source;

        private readonly TokenType[] _tokenWithTexts = {
            TokenType.STRING, TokenType.BOOL, TokenType.COMMENT, TokenType.COMP, TokenType.DOUBLE, TokenType.INTEGER,
            TokenType.IDENTIFIER, TokenType.OPER
        };

        public SourceCodeHandler(string source)
        {
            _source = source;
            var scanner = new Scanner();
            var parser = new Parser(scanner);

            var tree = parser.Parse(source);
            AddToTree(tree.Nodes.First(), ParseTree);

            Tokens.AddRange(
                scanner.RecognizedTokens.Select(
                    t =>
                        {
                            var result = $"{t.Type.ToString()} ({t.Line}:{t.Column})";
                            if (_tokenWithTexts.Contains(t.Type))
                            {
                                result += $" : {t.Text}";
                            }
                            return result;
                        }));

            Status = $"Errors: {string.Join(Environment.NewLine, tree.Errors.Select(e => e.ToString()))}{Environment.NewLine}";
        }

        public List<string> Tokens { get; private set; } = new List<string>();

        public TreeNode ParseTree { get; private set; } = new TreeNode();

        public string Status { get; private set; }

        private void AddToTree(ParseNode node, TreeNode treeNode)
        {
            var nodeText = node.Text;
            if (node.Token.Line != 0 && node.Token.Column != 0)
            {
                nodeText += $"({node.Token.Line}:{node.Token.Column})";
            }
            var singleNode = new TreeNode(nodeText) { Tag = node };
            treeNode.Nodes.Add(singleNode);
            foreach (var parseNode in node.Nodes)
            {
                AddToTree(parseNode, singleNode);
            }
        }
    }
}
