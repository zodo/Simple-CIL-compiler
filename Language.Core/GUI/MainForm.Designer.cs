namespace Language
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ASTtreeView = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.IdentifierTreeView = new System.Windows.Forms.TreeView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.parseTreeView = new System.Windows.Forms.TreeView();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rUNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mISCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.identifierTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oPTIMIZATIONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expressionSimplifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unreacheableCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loopExpansionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.generateAssemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 24);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tabControl1);
            this.splitContainerMain.Size = new System.Drawing.Size(1154, 654);
            this.splitContainerMain.SplitterDistance = 616;
            this.splitContainerMain.TabIndex = 0;
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.textBoxStatus);
            this.splitContainerLeft.Size = new System.Drawing.Size(616, 654);
            this.splitContainerLeft.SplitterDistance = 503;
            this.splitContainerLeft.TabIndex = 0;
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxStatus.HideSelection = false;
            this.textBoxStatus.Location = new System.Drawing.Point(0, 0);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatus.Size = new System.Drawing.Size(614, 145);
            this.textBoxStatus.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(532, 652);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ASTtreeView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(524, 626);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "AST";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ASTtreeView
            // 
            this.ASTtreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ASTtreeView.Location = new System.Drawing.Point(3, 3);
            this.ASTtreeView.Name = "ASTtreeView";
            this.ASTtreeView.Size = new System.Drawing.Size(518, 620);
            this.ASTtreeView.TabIndex = 0;
            this.ASTtreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ASTtreeView_AfterSelect);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.IdentifierTreeView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(524, 626);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Symbols";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // IdentifierTreeView
            // 
            this.IdentifierTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IdentifierTreeView.Location = new System.Drawing.Point(3, 3);
            this.IdentifierTreeView.Name = "IdentifierTreeView";
            this.IdentifierTreeView.Size = new System.Drawing.Size(518, 620);
            this.IdentifierTreeView.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.parseTreeView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(524, 626);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "CST";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // parseTreeView
            // 
            this.parseTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parseTreeView.HideSelection = false;
            this.parseTreeView.Location = new System.Drawing.Point(3, 3);
            this.parseTreeView.Name = "parseTreeView";
            this.parseTreeView.Size = new System.Drawing.Size(518, 620);
            this.parseTreeView.TabIndex = 0;
            this.parseTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.parseTreeView_AfterSelect);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fILEToolStripMenuItem,
            this.rUNToolStripMenuItem,
            this.mISCToolStripMenuItem,
            this.oPTIMIZATIONToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1154, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fILEToolStripMenuItem
            // 
            this.fILEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fILEToolStripMenuItem.Name = "fILEToolStripMenuItem";
            this.fILEToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.fILEToolStripMenuItem.Text = "FILE";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // rUNToolStripMenuItem
            // 
            this.rUNToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem1,
            this.runToolStripMenuItem2,
            this.generateAssemblyToolStripMenuItem});
            this.rUNToolStripMenuItem.Name = "rUNToolStripMenuItem";
            this.rUNToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.rUNToolStripMenuItem.Text = "RUN";
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            this.runToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runToolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
            this.runToolStripMenuItem1.Text = "Compile";
            this.runToolStripMenuItem1.Click += new System.EventHandler(this.runToolStripMenuItem1_Click);
            // 
            // runToolStripMenuItem2
            // 
            this.runToolStripMenuItem2.Name = "runToolStripMenuItem2";
            this.runToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.runToolStripMenuItem2.Size = new System.Drawing.Size(173, 22);
            this.runToolStripMenuItem2.Text = "Run";
            this.runToolStripMenuItem2.Click += new System.EventHandler(this.runToolStripMenuItem2_Click);
            // 
            // mISCToolStripMenuItem
            // 
            this.mISCToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.identifierTableToolStripMenuItem});
            this.mISCToolStripMenuItem.Name = "mISCToolStripMenuItem";
            this.mISCToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.mISCToolStripMenuItem.Text = "MISC";
            // 
            // identifierTableToolStripMenuItem
            // 
            this.identifierTableToolStripMenuItem.Name = "identifierTableToolStripMenuItem";
            this.identifierTableToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.identifierTableToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.identifierTableToolStripMenuItem.Text = "Identifier table";
            this.identifierTableToolStripMenuItem.Click += new System.EventHandler(this.identifierTableToolStripMenuItem_Click);
            // 
            // oPTIMIZATIONToolStripMenuItem
            // 
            this.oPTIMIZATIONToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expressionSimplifyToolStripMenuItem,
            this.unreacheableCodeToolStripMenuItem,
            this.variablesToolStripMenuItem,
            this.loopExpansionToolStripMenuItem});
            this.oPTIMIZATIONToolStripMenuItem.Name = "oPTIMIZATIONToolStripMenuItem";
            this.oPTIMIZATIONToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.oPTIMIZATIONToolStripMenuItem.Text = "OPTIMIZATION";
            // 
            // expressionSimplifyToolStripMenuItem
            // 
            this.expressionSimplifyToolStripMenuItem.Checked = true;
            this.expressionSimplifyToolStripMenuItem.CheckOnClick = true;
            this.expressionSimplifyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.expressionSimplifyToolStripMenuItem.Name = "expressionSimplifyToolStripMenuItem";
            this.expressionSimplifyToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.expressionSimplifyToolStripMenuItem.Text = "Expression simplify";
            this.expressionSimplifyToolStripMenuItem.CheckedChanged += new System.EventHandler(this.expressionSimplifyToolStripMenuItem_CheckedChanged);
            // 
            // unreacheableCodeToolStripMenuItem
            // 
            this.unreacheableCodeToolStripMenuItem.Checked = true;
            this.unreacheableCodeToolStripMenuItem.CheckOnClick = true;
            this.unreacheableCodeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.unreacheableCodeToolStripMenuItem.Name = "unreacheableCodeToolStripMenuItem";
            this.unreacheableCodeToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.unreacheableCodeToolStripMenuItem.Text = "Unreacheable code";
            this.unreacheableCodeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.unreacheableCodeToolStripMenuItem_CheckedChanged);
            // 
            // variablesToolStripMenuItem
            // 
            this.variablesToolStripMenuItem.Checked = true;
            this.variablesToolStripMenuItem.CheckOnClick = true;
            this.variablesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.variablesToolStripMenuItem.Name = "variablesToolStripMenuItem";
            this.variablesToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.variablesToolStripMenuItem.Text = "Variables";
            this.variablesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.variablesToolStripMenuItem_CheckedChanged);
            // 
            // loopExpansionToolStripMenuItem
            // 
            this.loopExpansionToolStripMenuItem.Checked = true;
            this.loopExpansionToolStripMenuItem.CheckOnClick = true;
            this.loopExpansionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loopExpansionToolStripMenuItem.Name = "loopExpansionToolStripMenuItem";
            this.loopExpansionToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.loopExpansionToolStripMenuItem.Text = "Loop expansion";
            this.loopExpansionToolStripMenuItem.CheckedChanged += new System.EventHandler(this.loopExpansionToolStripMenuItem_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // generateAssemblyToolStripMenuItem
            // 
            this.generateAssemblyToolStripMenuItem.Checked = true;
            this.generateAssemblyToolStripMenuItem.CheckOnClick = true;
            this.generateAssemblyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.generateAssemblyToolStripMenuItem.Name = "generateAssemblyToolStripMenuItem";
            this.generateAssemblyToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.generateAssemblyToolStripMenuItem.Text = "Generate assembly";
            this.generateAssemblyToolStripMenuItem.CheckedChanged += new System.EventHandler(this.generateAssemblyToolStripMenuItem_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 678);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.menuStrip);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            this.splitContainerLeft.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.TreeView parseTreeView;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rUNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mISCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem identifierTableToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TreeView IdentifierTreeView;
        private System.Windows.Forms.TreeView ASTtreeView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStripMenuItem oPTIMIZATIONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expressionSimplifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unreacheableCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem variablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loopExpansionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem generateAssemblyToolStripMenuItem;
    }
}

