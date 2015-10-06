using System;
using System.Windows.Forms;

namespace Language
{
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
            textBoxStatus.Text = $"{handler.Status}{Environment.NewLine}{string.Join("; ", handler.Tokens)}";
        }
    }
}
