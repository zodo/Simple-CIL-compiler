using System;
using System.Windows.Forms;

namespace Language
{
    using System.Linq;

    using TinyPG;

    public partial class MainForm : Form
    {
        private SourceCodeHandler handler;
        public MainForm()
        {
            InitializeComponent();
            textBoxProgram.SelectionTabs = new int[] { 20, 40, 60, 80 };
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            handler = new SourceCodeHandler(textBoxProgram);
            parseTreeView.Nodes.Clear();
            parseTreeView.Nodes.Add(handler.ParseTree.FirstNode);
            textBoxStatus.Text = $"{handler.Status}{Environment.NewLine}{string.Join(Environment.NewLine, handler.Tokens)}";
        }

        private void parseTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = e.Node.Tag as ParseNode;
            var token = node.Token;
            textBoxProgram.Select(token.StartPos, token.Length);
            textBoxProgram.ScrollToCaret();
        }

        private void textBoxProgram_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var currentLine = textBoxProgram.Lines.Length;
                if (textBoxProgram.Lines[currentLine - 1].StartsWith("\t"))
                {
                    textBoxProgram.AppendText(@"	");
                }

            }
        }

        private void identifierTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var identForm = new Form() {Width = 400, Height = 700, Text = @"Symbol table"};
            var tree = new TreeView() { Dock = DockStyle.Fill };
            tree.Nodes.Clear();
            if (handler != null)
            {
                tree.Nodes.Add(handler.IdentifierTree.FirstNode);
                identForm.Controls.Add(tree);
                identForm.Show();
            }
        }
    }
}
