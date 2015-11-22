using System;
using System.Windows.Forms;

namespace Language
{
    using System.IO;
    using System.Linq;

    using TinyPG;

    public partial class MainForm : Form
    {
        private SourceCodeHandler handler;

        private string _filePath;

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

            if (handler.IdentifierTree?.FirstNode != null)
            {
                IdentifierTreeView.Nodes.Clear();
                IdentifierTreeView.Nodes.Add(handler.IdentifierTree.FirstNode);
                IdentifierTreeView.ExpandAll();
            }
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
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _filePath = openFileDialog1.FileName;
                textBoxProgram.Text = File.ReadAllText(_filePath);

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_filePath != null)
            {
                File.WriteAllText(_filePath, textBoxProgram.Text);
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
                File.WriteAllText(_filePath, textBoxProgram.Text);
            }
        }
    }
}
