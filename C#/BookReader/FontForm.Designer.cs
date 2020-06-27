namespace BookReader
{
    partial class FontForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontForm));
            this.Txt = new System.Windows.Forms.TextBox();
            this.BtnRender = new System.Windows.Forms.Button();
            this.BtnGeneration = new System.Windows.Forms.Button();
            this.Picture = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NudFontSize = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CmbBpp = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudFontSize)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Txt
            // 
            this.Txt.Location = new System.Drawing.Point(12, 93);
            this.Txt.Multiline = true;
            this.Txt.Name = "Txt";
            this.Txt.Size = new System.Drawing.Size(583, 85);
            this.Txt.TabIndex = 0;
            this.Txt.Text = resources.GetString("Txt.Text");
            // 
            // BtnRender
            // 
            this.BtnRender.Location = new System.Drawing.Point(601, 93);
            this.BtnRender.Name = "BtnRender";
            this.BtnRender.Size = new System.Drawing.Size(75, 85);
            this.BtnRender.TabIndex = 1;
            this.BtnRender.Text = "Render";
            this.BtnRender.UseVisualStyleBackColor = true;
            this.BtnRender.Click += new System.EventHandler(this.BtnRender_Click);
            // 
            // BtnGeneration
            // 
            this.BtnGeneration.Location = new System.Drawing.Point(271, 16);
            this.BtnGeneration.Name = "BtnGeneration";
            this.BtnGeneration.Size = new System.Drawing.Size(75, 23);
            this.BtnGeneration.TabIndex = 2;
            this.BtnGeneration.Text = "Generation";
            this.BtnGeneration.UseVisualStyleBackColor = true;
            this.BtnGeneration.Click += new System.EventHandler(this.BtnGeneration_Click);
            // 
            // Picture
            // 
            this.Picture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Picture.BackColor = System.Drawing.Color.White;
            this.Picture.Location = new System.Drawing.Point(8, 8);
            this.Picture.Name = "Picture";
            this.Picture.Size = new System.Drawing.Size(936, 702);
            this.Picture.TabIndex = 3;
            this.Picture.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(164, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "Bpp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "FontSize";
            // 
            // NudFontSize
            // 
            this.NudFontSize.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NudFontSize.Location = new System.Drawing.Point(78, 16);
            this.NudFontSize.Maximum = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.NudFontSize.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.NudFontSize.Name = "NudFontSize";
            this.NudFontSize.Size = new System.Drawing.Size(61, 21);
            this.NudFontSize.TabIndex = 9;
            this.NudFontSize.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.CmbBpp);
            this.groupBox1.Controls.Add(this.BtnRender);
            this.groupBox1.Controls.Add(this.NudFontSize);
            this.groupBox1.Controls.Add(this.BtnGeneration);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Txt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 711);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(952, 185);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // CmbBpp
            // 
            this.CmbBpp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBpp.FormattingEnabled = true;
            this.CmbBpp.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8"});
            this.CmbBpp.Location = new System.Drawing.Point(193, 17);
            this.CmbBpp.Name = "CmbBpp";
            this.CmbBpp.Size = new System.Drawing.Size(59, 20);
            this.CmbBpp.TabIndex = 10;
            // 
            // FontForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 896);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Picture);
            this.Name = "FontForm";
            this.Text = "渲染";
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudFontSize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox Txt;
        private System.Windows.Forms.Button BtnRender;
        private System.Windows.Forms.Button BtnGeneration;
        private System.Windows.Forms.PictureBox Picture;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NudFontSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox CmbBpp;
    }
}

