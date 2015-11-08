namespace Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using Semantic;
    using Semantic.Data;

    using TinyPG;

    public class SourceCodeHandler
    {
        private readonly string _source;

        private readonly TokenType[] _tokenWithTexts = {
            TokenType.STRING, TokenType.BOOL, TokenType.COMMENT, TokenType.COMP, TokenType.DOUBLE, TokenType.INTEGER,
            TokenType.IDENTIFIER, TokenType.OPER
        };

        public SourceCodeHandler(RichTextBox source)
        {
            _source = source.Text;
            var scanner = new Scanner();
            var parser = new Parser(scanner);

            //var highlighter = new TextHighlighter(source, scanner, parser);

            var tree = parser.Parse(source.Text, "", new ParseTreeWithSemantic());

            if (!tree.Errors.Any())
            {
                var res = tree.Eval()?.ToString();
                var r = IdentifierTable.RootNamespace;

                
            }
            

            var node = (ParseNode)tree;
            //ToAST(ref node);
            AddToTree(tree.Nodes.First(), ParseTree);

            AddToIdentifierTree(IdentifierTable.RootNamespace, IdentifierTree);

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

        public TreeNode IdentifierTree { get; set; } = new TreeNode();

        private void AddToIdentifierTree(Namespace namespc, TreeNode node)
        {
            var nodeText = namespc.Name;
            var singleNode = new TreeNode(nodeText) { Tag = namespc };

            foreach (var literal in namespc.Literals.Where(l => namespc.Children.All(c => c.Name != l.Name)))
            {
                var literalNode = new TreeNode(literal.Name);
                singleNode.Nodes.Add(literalNode);
            }

            
            node.Nodes.Add(singleNode);

            foreach (var child in namespc.Children)
            {
                AddToIdentifierTree(child, singleNode);
            }
            
        }

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

        private void ToAST(ref ParseNode tree)
        {
            if (tree.Nodes.Count > 1)
            {
                for (var i = 0; i < tree.Nodes.Count; i++)
                {
                    var parseNode = tree.Nodes[i];
                    ToAST(ref parseNode);
                }
            }
            else if (tree.Nodes.Count == 1)
            {
                tree.Nodes = tree.Nodes.First().Nodes;
                ToAST(ref tree);
            }
        }
    }
}
