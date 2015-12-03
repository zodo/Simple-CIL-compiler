using System;
using System.Windows.Forms;

namespace Language
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    using FastColoredTextBoxNS;

    using GUI;

    using Semantic;

    using TinyPG;

    public partial class MainForm : Form
    {
        private SourceCodeHandler handler;

        private string _filePath;

        private FastColoredTextBox TextBox;

        private List<Token> _tokens;

        private ParseErrors _errors;

        public MainForm()
        {
            InitializeComponent();
            TextBox = new FastColoredTextBox() {Dock = DockStyle.Fill, AutoIndent = true};
            TextBox.TextChanged+=TextBoxOnTextChanged;
            TextBox.TextChangedDelayed+=TextBoxOnTextChangedDelayed;
            TextBox.Zoom = 150;
            TextBox.CommentPrefix = "//";
            TextBox.HotkeysMapping[Keys.Control | Keys.OemQuestion] = FCTBAction.CommentSelected;
            TextBox.ToolTipNeeded += TextBoxOnToolTipNeeded;
            splitContainerLeft.Panel1.Controls.Add(TextBox);
            //textBoxProgram.SelectionTabs = new int[] { 20, 40, 60, 80 };
        }

        private void TextBoxOnToolTipNeeded(object sender, ToolTipNeededEventArgs e)
        {
            if (_tokens == null)
            {
                return;
            }
            var place = e.Place;
            int n = place.iLine;
            var absolutePosition = TextBox.Text.TakeWhile(c => (n -= (c == '\n' ? 1 : 0)) > 0).Count() + place.iChar + 1;
            var token = _tokens.Where(t => t.Type != TokenType.WHITESPACE).FirstOrDefault(x => x.StartPos <= absolutePosition && x.EndPos >= absolutePosition);
            if (token != null)
            {
                e.ToolTipIcon = ToolTipIcon.Info;
                e.ToolTipTitle = token.Type.ToString();
                e.ToolTipText = token.Text;
            }
        }

        private void TextBoxOnTextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            ColorScheme.Colors.Values.ToList().ForEach(x => TextBox.Range.ClearStyle(x));
            var scanner = new Scanner();
            var parser = new Parser(scanner);
            var tree = (SyntaxHiglightParseTree)parser.Parse(TextBox.Text, "", new SyntaxHiglightParseTree());
            tree.Eval();
            if (!tree.Errors.Any())
            {
                TextBox.Range.ClearFoldingMarkers();
                foreach (var codeRegion in tree.CodeRegions)
                {
                    if (codeRegion.Start != codeRegion.End)
                    {
                        var startLine = TextBox.Text.Take(codeRegion.Start).Count(s => s == '\n');
                        var endLine = TextBox.Text.Take(codeRegion.End).Count(s => s == '\n');
                        TextBox[startLine].FoldingStartMarker = codeRegion.Name;
                        TextBox[endLine].FoldingEndMarker = codeRegion.Name;
                        TextBox.Invalidate();
                    }
                }
            }

            _tokens = scanner.RecognizedTokens.Concat(scanner.SkippedGlobal).ToList();
            var rangesWithColors = _tokens
                .Where(t => Scanner.Styles.ContainsKey(t.Type))
                    .Select(t => new { Start = t.StartPos, End = t.EndPos, Style = Scanner.Styles[t.Type] })
                    .Where(t => ColorScheme.Colors.ContainsKey(t.Style))
                    .Select(t => new { t.Start, t.End, S = ColorScheme.Colors[t.Style] })
                    .ToList();
            foreach (var rangeColor in rangesWithColors)
            {
                var startLine = TextBox.Text.Take(rangeColor.Start).Count(s => s == '\n');
                var endLine = TextBox.Text.Take(rangeColor.End).Count(s => s == '\n');
                var startChar = rangeColor.Start - TextBox.Text.LastIndexOf("\n", rangeColor.Start, StringComparison.Ordinal) - 1;
                var endChar = rangeColor.End - TextBox.Text.LastIndexOf("\n", rangeColor.End, StringComparison.Ordinal) - 1;
                var range = new Range(TextBox, startChar, startLine, endChar, endLine);
                range.SetStyle(rangeColor.S);
            }
            DrawErrors();


        }

        private void TextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            handler = new SourceCodeHandler(TextBox.Text);
            parseTreeView.Nodes.Clear();
            parseTreeView.Nodes.Add(handler.ParseTree.FirstNode);
            parseTreeView.ExpandAll();
            textBoxStatus.Text = $"{handler.Status}{Environment.NewLine}{string.Join(Environment.NewLine, handler.Tokens)}";
            if (handler.ASTTree != null)
            {
                ASTtreeView.Nodes.Clear();
                ASTtreeView.Nodes.Add(handler.ASTTree);
                ASTtreeView.ExpandAll();
            }
            if (handler.IdentifierTree?.FirstNode != null)
            {
                IdentifierTreeView.Nodes.Clear();
                IdentifierTreeView.Nodes.Add(handler.IdentifierTree.FirstNode);
                IdentifierTreeView.ExpandAll();
            }

            _errors = handler.Errors;
            DrawErrors();
        }

        private void DrawErrors()
        {
            TextBox.Range.ClearStyle(ColorScheme.Colors["error"]);
            if (_errors != null && _errors.Any())
            {
                foreach (var errorRange in _errors.Select(x => new { x.Line, x.Column, x.Length }))
                {
                    var startLine = errorRange.Line - 1;
                    var startChar = errorRange.Column == 0? errorRange.Column: errorRange.Column - 1;
                    var endChar = errorRange.Column + errorRange.Length == 0? 1 : errorRange.Length;
                    var range = new Range(TextBox, startChar, startLine, endChar, startLine);
                    ColorScheme.Colors.Values.ToList().ForEach(x => range.ClearStyle(x));
                    range.SetStyle(ColorScheme.Colors["error"]);
                }
            }
        }

        private void parseTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = e.Node.Tag as ParseNode;
            if (node != null)
            {
                var token = node.Token;
                TextBox.SelectionStart = token.StartPos;
                TextBox.SelectionLength = token.Length;
                TextBox.DoSelectionVisible();
            }

        }

        private void identifierTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string marker = "myMarker";

            var currentSelection = TextBox.Selection.Clone();
            currentSelection.Normalize();

            if (currentSelection.Start.iLine != currentSelection.End.iLine)
            {
                TextBox[currentSelection.Start.iLine].FoldingStartMarker = marker;
                TextBox[currentSelection.End.iLine].FoldingEndMarker = marker;
                TextBox.Invalidate();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _filePath = openFileDialog1.FileName;
                TextBox.Text = File.ReadAllText(_filePath);

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_filePath != null)
            {
                File.WriteAllText(_filePath, TextBox.Text);
            }
            else
            {
                saveAsToolStripMenuItem_Click(this, null);
            }
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _filePath = saveFileDialog1.FileName;
                File.WriteAllText(_filePath, TextBox.Text);
            }
        }

        private void ASTtreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = e.Node.Tag as ParseNode;
            if (node != null)
            {
                var token = node.Token;
                TextBox.SelectionStart = token.StartPos;
                TextBox.SelectionLength = token.Length;
                TextBox.DoSelectionVisible();
            }
        }

        private void expressionSimplifyToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            OptimizeMode.ExpressionSimplify = expressionSimplifyToolStripMenuItem.Checked;
        }

        private void unreacheableCodeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            OptimizeMode.UnreacheableCode = unreacheableCodeToolStripMenuItem.Checked;
        }

        private void variablesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            OptimizeMode.Variables = variablesToolStripMenuItem.Checked;
        }
    }
}
