namespace BookReader
{
    partial class DocumentForm
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
            this.Picture = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LblWidth = new System.Windows.Forms.Label();
            this.LblHeight = new System.Windows.Forms.Label();
            this.CmbRenderMode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LblAddress = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LblDrawTime = new System.Windows.Forms.Label();
            this.LblRenderDesktopTime = new System.Windows.Forms.Label();
            this.LblRenderEInkTime = new System.Windows.Forms.Label();
            this.BtnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Picture
            // 
            this.Picture.Location = new System.Drawing.Point(0, 100);
            this.Picture.Name = "Picture";
            this.Picture.Size = new System.Drawing.Size(936, 702);
            this.Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Picture.TabIndex = 1;
            this.Picture.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.BtnClear);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.LblAddress);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.CmbRenderMode);
            this.panel1.Controls.Add(this.LblHeight);
            this.panel1.Controls.Add(this.LblWidth);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(936, 100);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Width:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Height:";
            // 
            // LblWidth
            // 
            this.LblWidth.AutoSize = true;
            this.LblWidth.Location = new System.Drawing.Point(80, 14);
            this.LblWidth.Name = "LblWidth";
            this.LblWidth.Size = new System.Drawing.Size(11, 12);
            this.LblWidth.TabIndex = 2;
            this.LblWidth.Text = "0";
            // 
            // LblHeight
            // 
            this.LblHeight.AutoSize = true;
            this.LblHeight.Location = new System.Drawing.Point(80, 42);
            this.LblHeight.Name = "LblHeight";
            this.LblHeight.Size = new System.Drawing.Size(11, 12);
            this.LblHeight.TabIndex = 3;
            this.LblHeight.Text = "0";
            // 
            // CmbRenderMode
            // 
            this.CmbRenderMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbRenderMode.Enabled = false;
            this.CmbRenderMode.FormattingEnabled = true;
            this.CmbRenderMode.Location = new System.Drawing.Point(808, 74);
            this.CmbRenderMode.Name = "CmbRenderMode";
            this.CmbRenderMode.Size = new System.Drawing.Size(121, 20);
            this.CmbRenderMode.TabIndex = 4;
            this.CmbRenderMode.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(150, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Address:";
            // 
            // LblAddress
            // 
            this.LblAddress.AutoSize = true;
            this.LblAddress.Location = new System.Drawing.Point(209, 14);
            this.LblAddress.Name = "LblAddress";
            this.LblAddress.Size = new System.Drawing.Size(53, 12);
            this.LblAddress.TabIndex = 6;
            this.LblAddress.Text = "FFFFFFFF";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Draw:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LblRenderEInkTime);
            this.groupBox1.Controls.Add(this.LblRenderDesktopTime);
            this.groupBox1.Controls.Add(this.LblDrawTime);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(385, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 91);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cost";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "Desktop:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "EInk:";
            // 
            // LblDrawTime
            // 
            this.LblDrawTime.AutoSize = true;
            this.LblDrawTime.Location = new System.Drawing.Point(100, 22);
            this.LblDrawTime.Name = "LblDrawTime";
            this.LblDrawTime.Size = new System.Drawing.Size(11, 12);
            this.LblDrawTime.TabIndex = 10;
            this.LblDrawTime.Text = "0";
            // 
            // LblRenderDesktopTime
            // 
            this.LblRenderDesktopTime.AutoSize = true;
            this.LblRenderDesktopTime.Location = new System.Drawing.Point(100, 47);
            this.LblRenderDesktopTime.Name = "LblRenderDesktopTime";
            this.LblRenderDesktopTime.Size = new System.Drawing.Size(11, 12);
            this.LblRenderDesktopTime.TabIndex = 11;
            this.LblRenderDesktopTime.Text = "0";
            // 
            // LblRenderEInkTime
            // 
            this.LblRenderEInkTime.AutoSize = true;
            this.LblRenderEInkTime.Location = new System.Drawing.Point(100, 71);
            this.LblRenderEInkTime.Name = "LblRenderEInkTime";
            this.LblRenderEInkTime.Size = new System.Drawing.Size(11, 12);
            this.LblRenderEInkTime.TabIndex = 12;
            this.LblRenderEInkTime.Text = "0";
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(854, 45);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 23);
            this.BtnClear.TabIndex = 9;
            this.BtnClear.TabStop = false;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // DocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(936, 803);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Picture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DocumentForm";
            this.Text = "LongTextForm";
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Picture;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblHeight;
        private System.Windows.Forms.Label LblWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CmbRenderMode;
        private System.Windows.Forms.Label LblAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LblRenderEInkTime;
        private System.Windows.Forms.Label LblRenderDesktopTime;
        private System.Windows.Forms.Label LblDrawTime;
        private System.Windows.Forms.Button BtnClear;
    }
}