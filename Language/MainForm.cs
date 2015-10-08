using System;
using System.Windows.Forms;

namespace Language
{
    using TinyPG;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var handler = new SourceCodeHandler(textBoxProgram.Text);
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
    }
}
