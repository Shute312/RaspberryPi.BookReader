namespace BookReader
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontToolStripMenuItem,
            this.graphicToolStripMenuItem,
            this.singleFrameToolStripMenuItem,
            this.documentToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1064, 25);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "Font";
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(45, 21);
            this.fontToolStripMenuItem.Text = "Font";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.OnClickFont);
            // 
            // graphicToolStripMenuItem
            // 
            this.graphicToolStripMenuItem.Name = "graphicToolStripMenuItem";
            this.graphicToolStripMenuItem.Size = new System.Drawing.Size(65, 21);
            this.graphicToolStripMenuItem.Text = "Graphic";
            this.graphicToolStripMenuItem.Click += new System.EventHandler(this.OnClickGraphic);
            // 
            // singleFrameToolStripMenuItem
            // 
            this.singleFrameToolStripMenuItem.Name = "singleFrameToolStripMenuItem";
            this.singleFrameToolStripMenuItem.Size = new System.Drawing.Size(91, 21);
            this.singleFrameToolStripMenuItem.Text = "SingleFrame";
            this.singleFrameToolStripMenuItem.Click += new System.EventHandler(this.OnClickSingleFrame);
            // 
            // documentToolStripMenuItem
            // 
            this.documentToolStripMenuItem.Name = "documentToolStripMenuItem";
            this.documentToolStripMenuItem.Size = new System.Drawing.Size(79, 21);
            this.documentToolStripMenuItem.Text = "Document";
            this.documentToolStripMenuItem.Click += new System.EventHandler(this.OnClickDocument);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 781);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleFrameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentToolStripMenuItem;
    }
}