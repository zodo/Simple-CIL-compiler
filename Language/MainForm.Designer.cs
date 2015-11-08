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
            this.textBoxProgram = new System.Windows.Forms.RichTextBox();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.parseTreeView = new System.Windows.Forms.TreeView();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rUNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mISCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.identifierTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
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
            this.splitContainerMain.Panel2.Controls.Add(this.parseTreeView);
            this.splitContainerMain.Size = new System.Drawing.Size(702, 465);
            this.splitContainerMain.SplitterDistance = 375;
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
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.textBoxProgram);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.textBoxStatus);
            this.splitContainerLeft.Size = new System.Drawing.Size(375, 465);
            this.splitContainerLeft.SplitterDistance = 357;
            this.splitContainerLeft.TabIndex = 0;
            // 
            // textBoxProgram
            // 
            this.textBoxProgram.AcceptsTab = true;
            this.textBoxProgram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProgram.HideSelection = false;
            this.textBoxProgram.Location = new System.Drawing.Point(0, 0);
            this.textBoxProgram.Name = "textBoxProgram";
            this.textBoxProgram.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxProgram.Size = new System.Drawing.Size(373, 355);
            this.textBoxProgram.TabIndex = 0;
            this.textBoxProgram.Text = "";
            this.textBoxProgram.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBoxProgram_PreviewKeyDown);
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxStatus.HideSelection = false;
            this.textBoxStatus.Location = new System.Drawing.Point(0, 0);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatus.Size = new System.Drawing.Size(373, 102);
            this.textBoxStatus.TabIndex = 0;
            // 
            // parseTreeView
            // 
            this.parseTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parseTreeView.HideSelection = false;
            this.parseTreeView.Location = new System.Drawing.Point(0, 0);
            this.parseTreeView.Name = "parseTreeView";
            this.parseTreeView.Size = new System.Drawing.Size(321, 463);
            this.parseTreeView.TabIndex = 0;
            this.parseTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.parseTreeView_AfterSelect);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fILEToolStripMenuItem,
            this.rUNToolStripMenuItem,
            this.mISCToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(702, 24);
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
            this.openToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // rUNToolStripMenuItem
            // 
            this.rUNToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem1});
            this.rUNToolStripMenuItem.Name = "rUNToolStripMenuItem";
            this.rUNToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.rUNToolStripMenuItem.Text = "RUN";
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            this.runToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.runToolStripMenuItem1.Text = "Run";
            this.runToolStripMenuItem1.Click += new System.EventHandler(this.runToolStripMenuItem1_Click);
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
            this.identifierTableToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.identifierTableToolStripMenuItem.Text = "Identifier table";
            this.identifierTableToolStripMenuItem.Click += new System.EventHandler(this.identifierTableToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 489);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.menuStrip);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            this.splitContainerLeft.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
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
        private System.Windows.Forms.RichTextBox textBoxProgram;
        private System.Windows.Forms.ToolStripMenuItem mISCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem identifierTableToolStripMenuItem;
    }
}

