namespace SkypeHistoryEnc
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.splitleft = new System.Windows.Forms.SplitContainer();
            this.btnglobal = new System.Windows.Forms.Button();
            this.txtglobal = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ckbdelete = new System.Windows.Forms.CheckBox();
            this.ctxUnenc = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.encryptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitright = new System.Windows.Forms.SplitContainer();
            this.ctxEnc = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ckbfiltermsg = new System.Windows.Forms.CheckBox();
            this.btnfindprev = new System.Windows.Forms.Button();
            this.btnfindnext = new System.Windows.Forms.Button();
            this.txtfilter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtmsgs = new System.Windows.Forms.RichTextBox();
            this.lstUnenc = new SkypeHistoryEnc.DoubleBufferDataGrid();
            this.lstEnc = new SkypeHistoryEnc.DoubleBufferDataGrid();
            this.label3 = new System.Windows.Forms.Label();
            this.splitleft.Panel1.SuspendLayout();
            this.splitleft.Panel2.SuspendLayout();
            this.splitleft.SuspendLayout();
            this.ctxUnenc.SuspendLayout();
            this.splitright.Panel1.SuspendLayout();
            this.splitright.Panel2.SuspendLayout();
            this.splitright.SuspendLayout();
            this.ctxEnc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstUnenc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstEnc)).BeginInit();
            this.SuspendLayout();
            // 
            // splitleft
            // 
            this.splitleft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitleft.Location = new System.Drawing.Point(0, 0);
            this.splitleft.Name = "splitleft";
            // 
            // splitleft.Panel1
            // 
            this.splitleft.Panel1.Controls.Add(this.btnglobal);
            this.splitleft.Panel1.Controls.Add(this.txtglobal);
            this.splitleft.Panel1.Controls.Add(this.label2);
            this.splitleft.Panel1.Controls.Add(this.ckbdelete);
            this.splitleft.Panel1.Controls.Add(this.lstUnenc);
            // 
            // splitleft.Panel2
            // 
            this.splitleft.Panel2.Controls.Add(this.splitright);
            this.splitleft.Size = new System.Drawing.Size(1250, 782);
            this.splitleft.SplitterDistance = 363;
            this.splitleft.TabIndex = 0;
            // 
            // btnglobal
            // 
            this.btnglobal.Location = new System.Drawing.Point(289, 0);
            this.btnglobal.Name = "btnglobal";
            this.btnglobal.Size = new System.Drawing.Size(75, 23);
            this.btnglobal.TabIndex = 9;
            this.btnglobal.Text = "Filter Chats";
            this.btnglobal.UseVisualStyleBackColor = true;
            this.btnglobal.Click += new System.EventHandler(this.btnglobal_Click);
            // 
            // txtglobal
            // 
            this.txtglobal.Location = new System.Drawing.Point(205, 2);
            this.txtglobal.Name = "txtglobal";
            this.txtglobal.Size = new System.Drawing.Size(79, 20);
            this.txtglobal.TabIndex = 8;
            this.txtglobal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtglobal_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Filter:";
            // 
            // ckbdelete
            // 
            this.ckbdelete.AutoSize = true;
            this.ckbdelete.Location = new System.Drawing.Point(3, 3);
            this.ckbdelete.Name = "ckbdelete";
            this.ckbdelete.Size = new System.Drawing.Size(138, 17);
            this.ckbdelete.TabIndex = 1;
            this.ckbdelete.Text = "Delete when encrypting";
            this.ckbdelete.UseVisualStyleBackColor = true;
            // 
            // ctxUnenc
            // 
            this.ctxUnenc.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.encryptToolStripMenuItem});
            this.ctxUnenc.Name = "ctxUnenc";
            this.ctxUnenc.Size = new System.Drawing.Size(115, 26);
            this.ctxUnenc.Opening += new System.ComponentModel.CancelEventHandler(this.ctxUnenc_Opening);
            // 
            // encryptToolStripMenuItem
            // 
            this.encryptToolStripMenuItem.Name = "encryptToolStripMenuItem";
            this.encryptToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.encryptToolStripMenuItem.Text = "Encrypt";
            this.encryptToolStripMenuItem.Click += new System.EventHandler(this.encryptToolStripMenuItem_Click);
            // 
            // splitright
            // 
            this.splitright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitright.Location = new System.Drawing.Point(0, 0);
            this.splitright.Name = "splitright";
            // 
            // splitright.Panel1
            // 
            this.splitright.Panel1.Controls.Add(this.label3);
            this.splitright.Panel1.Controls.Add(this.lstEnc);
            // 
            // splitright.Panel2
            // 
            this.splitright.Panel2.Controls.Add(this.ckbfiltermsg);
            this.splitright.Panel2.Controls.Add(this.btnfindprev);
            this.splitright.Panel2.Controls.Add(this.btnfindnext);
            this.splitright.Panel2.Controls.Add(this.txtfilter);
            this.splitright.Panel2.Controls.Add(this.label1);
            this.splitright.Panel2.Controls.Add(this.txtmsgs);
            this.splitright.Size = new System.Drawing.Size(883, 782);
            this.splitright.SplitterDistance = 258;
            this.splitright.TabIndex = 0;
            // 
            // ctxEnc
            // 
            this.ctxEnc.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.ctxEnc.Name = "ctxEnc";
            this.ctxEnc.Size = new System.Drawing.Size(108, 26);
            this.ctxEnc.Opening += new System.ComponentModel.CancelEventHandler(this.ctxEnc_Opening);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // ckbfiltermsg
            // 
            this.ckbfiltermsg.AutoSize = true;
            this.ckbfiltermsg.Location = new System.Drawing.Point(385, 4);
            this.ckbfiltermsg.Name = "ckbfiltermsg";
            this.ckbfiltermsg.Size = new System.Drawing.Size(99, 17);
            this.ckbfiltermsg.TabIndex = 6;
            this.ckbfiltermsg.Text = "Filter Messages";
            this.ckbfiltermsg.UseVisualStyleBackColor = true;
            this.ckbfiltermsg.CheckedChanged += new System.EventHandler(this.ckbfiltermsg_CheckedChanged);
            // 
            // btnfindprev
            // 
            this.btnfindprev.Location = new System.Drawing.Point(299, 0);
            this.btnfindprev.Name = "btnfindprev";
            this.btnfindprev.Size = new System.Drawing.Size(80, 23);
            this.btnfindprev.TabIndex = 5;
            this.btnfindprev.Text = "Find Previous";
            this.btnfindprev.UseVisualStyleBackColor = true;
            this.btnfindprev.Click += new System.EventHandler(this.btnfindprev_Click);
            // 
            // btnfindnext
            // 
            this.btnfindnext.Location = new System.Drawing.Point(218, 0);
            this.btnfindnext.Name = "btnfindnext";
            this.btnfindnext.Size = new System.Drawing.Size(75, 23);
            this.btnfindnext.TabIndex = 4;
            this.btnfindnext.Text = "Find Next";
            this.btnfindnext.UseVisualStyleBackColor = true;
            this.btnfindnext.Click += new System.EventHandler(this.btnfindnext_Click);
            // 
            // txtfilter
            // 
            this.txtfilter.Location = new System.Drawing.Point(39, 2);
            this.txtfilter.Name = "txtfilter";
            this.txtfilter.Size = new System.Drawing.Size(173, 20);
            this.txtfilter.TabIndex = 2;
            this.txtfilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtfilter_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Find:";
            // 
            // txtmsgs
            // 
            this.txtmsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtmsgs.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtmsgs.Location = new System.Drawing.Point(0, 23);
            this.txtmsgs.Name = "txtmsgs";
            this.txtmsgs.ReadOnly = true;
            this.txtmsgs.Size = new System.Drawing.Size(621, 759);
            this.txtmsgs.TabIndex = 0;
            this.txtmsgs.Text = "";
            // 
            // lstUnenc
            // 
            this.lstUnenc.AllowUserToAddRows = false;
            this.lstUnenc.AllowUserToDeleteRows = false;
            this.lstUnenc.AllowUserToOrderColumns = true;
            this.lstUnenc.AllowUserToResizeRows = false;
            this.lstUnenc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstUnenc.ContextMenuStrip = this.ctxUnenc;
            this.lstUnenc.Location = new System.Drawing.Point(0, 23);
            this.lstUnenc.MultiSelect = false;
            this.lstUnenc.Name = "lstUnenc";
            this.lstUnenc.ReadOnly = true;
            this.lstUnenc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lstUnenc.Size = new System.Drawing.Size(363, 759);
            this.lstUnenc.TabIndex = 0;
            this.lstUnenc.SelectionChanged += new System.EventHandler(this.lstUnenc_SelectionChanged);
            // 
            // lstEnc
            // 
            this.lstEnc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstEnc.ContextMenuStrip = this.ctxEnc;
            this.lstEnc.Location = new System.Drawing.Point(0, 23);
            this.lstEnc.MultiSelect = false;
            this.lstEnc.Name = "lstEnc";
            this.lstEnc.ReadOnly = true;
            this.lstEnc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lstEnc.Size = new System.Drawing.Size(258, 782);
            this.lstEnc.TabIndex = 1;
            this.lstEnc.SelectionChanged += new System.EventHandler(this.lstEnc_SelectionChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(88, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Encrypted Chats:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 782);
            this.Controls.Add(this.splitleft);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HistEncrypter";
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.splitleft.Panel1.ResumeLayout(false);
            this.splitleft.Panel1.PerformLayout();
            this.splitleft.Panel2.ResumeLayout(false);
            this.splitleft.ResumeLayout(false);
            this.ctxUnenc.ResumeLayout(false);
            this.splitright.Panel1.ResumeLayout(false);
            this.splitright.Panel1.PerformLayout();
            this.splitright.Panel2.ResumeLayout(false);
            this.splitright.Panel2.PerformLayout();
            this.splitright.ResumeLayout(false);
            this.ctxEnc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstUnenc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstEnc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitleft;
        private System.Windows.Forms.SplitContainer splitright;
        private SkypeHistoryEnc.DoubleBufferDataGrid lstUnenc;
        private SkypeHistoryEnc.DoubleBufferDataGrid lstEnc;
        private System.Windows.Forms.ContextMenuStrip ctxEnc;
        private System.Windows.Forms.ContextMenuStrip ctxUnenc;
        private System.Windows.Forms.ToolStripMenuItem encryptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.RichTextBox txtmsgs;
        private System.Windows.Forms.CheckBox ckbdelete;
        private System.Windows.Forms.TextBox txtfilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnfindprev;
        private System.Windows.Forms.Button btnfindnext;
        private System.Windows.Forms.CheckBox ckbfiltermsg;
        private System.Windows.Forms.Button btnglobal;
        private System.Windows.Forms.TextBox txtglobal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

    }
}

