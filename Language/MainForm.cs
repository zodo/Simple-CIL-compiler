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

        private Form _identForm;

        public MainForm()
        {
            InitializeComponent();
            textBoxProgram.SelectionTabs = new int[] { 20, 40, 60, 80 };
            _identForm = new Form() { Width = 400, Height = 700, Text = @"Symbol table" };
            var t = new TreeView() { Dock = DockStyle.Fill, Name = "IdentifierTreeView" };
            _identForm.Controls.Add(t);
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            handler = new SourceCodeHandler(textBoxProgram);
            parseTreeView.Nodes.Clear();
            parseTreeView.Nodes.Add(handler.ParseTree.FirstNode);
            textBoxStatus.Text = $"{handler.Status}{Environment.NewLine}{string.Join(Environment.NewLine, handler.Tokens)}";

            var tree = (TreeView)_identForm.Controls.Find("IdentifierTreeView", false).First();
            tree.Nodes.Clear();
            tree.Nodes.Add(handler.IdentifierTree.FirstNode);
            tree.ExpandAll();
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
            _identForm.Show();
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
