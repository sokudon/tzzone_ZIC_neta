namespace neta
{
    partial class Form8
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
            textBox1 = new System.Windows.Forms.TextBox();
            button1 = new System.Windows.Forms.Button();
            comboBox1 = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            textBox2 = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            comboBox2 = new System.Windows.Forms.ComboBox();
            button2 = new System.Windows.Forms.Button();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            開くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            開くToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            文字コードToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            SELCP = new System.Windows.Forms.ToolStripMenuItem();
            SJIS = new System.Windows.Forms.ToolStripMenuItem();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            unihex = new System.Windows.Forms.Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(20, 56);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox1.Size = new System.Drawing.Size(419, 206);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(137, 302);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(168, 29);
            button1.TabIndex = 1;
            button1.Text = "変換";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(55, 268);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(157, 28);
            comboBox1.TabIndex = 2;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(10, 271);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 20);
            label1.TabIndex = 3;
            label1.Text = "入力";
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(20, 337);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox2.Size = new System.Drawing.Size(406, 216);
            textBox2.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(230, 271);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(39, 20);
            label2.TabIndex = 5;
            label2.Text = "出力";
            // 
            // comboBox2
            // 
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new System.Drawing.Point(275, 268);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(151, 28);
            comboBox2.TabIndex = 6;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(358, 302);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(60, 29);
            button2.TabIndex = 7;
            button2.Text = "Copy";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 開くToolStripMenuItem, 文字コードToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(448, 28);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // 開くToolStripMenuItem
            // 
            開くToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { 開くToolStripMenuItem1, 保存ToolStripMenuItem });
            開くToolStripMenuItem.Name = "開くToolStripMenuItem";
            開くToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            開くToolStripMenuItem.Text = "ファイル";
            // 
            // 開くToolStripMenuItem1
            // 
            開くToolStripMenuItem1.Name = "開くToolStripMenuItem1";
            開くToolStripMenuItem1.Size = new System.Drawing.Size(224, 26);
            開くToolStripMenuItem1.Text = "[入力]開く";
            開くToolStripMenuItem1.Click += OPENFILE_Click;
            // 
            // 保存ToolStripMenuItem
            // 
            保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            保存ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            保存ToolStripMenuItem.Text = "[入力]保存";
            保存ToolStripMenuItem.Click += SAVEAS_Click;
            // 
            // 文字コードToolStripMenuItem
            // 
            文字コードToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SELCP, SJIS });
            文字コードToolStripMenuItem.Name = "文字コードToolStripMenuItem";
            文字コードToolStripMenuItem.Size = new System.Drawing.Size(84, 24);
            文字コードToolStripMenuItem.Text = "文字コード";
            // 
            // SELCP
            // 
            SELCP.Name = "SELCP";
            SELCP.Size = new System.Drawing.Size(207, 26);
            SELCP.Text = "MSCP(入力と同じ)";
            SELCP.Click += mscodeToolStripMenuItem_Click;
            // 
            // SJIS
            // 
            SJIS.Name = "SJIS";
            SJIS.Size = new System.Drawing.Size(207, 26);
            SJIS.Text = "Shift_JIS_2004";
            SJIS.Click += shiftJIS2004ToolStripMenuItem_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(20, 33);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(39, 20);
            label3.TabIndex = 9;
            label3.Text = "入力";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(19, 311);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(39, 20);
            label4.TabIndex = 10;
            label4.Text = "出力";
            // 
            // unihex
            // 
            unihex.AutoSize = true;
            unihex.Location = new System.Drawing.Point(318, 29);
            unihex.Name = "unihex";
            unihex.Size = new System.Drawing.Size(0, 20);
            unihex.TabIndex = 11;
            // 
            // Form8
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(448, 565);
            Controls.Add(unihex);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button2);
            Controls.Add(comboBox2);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(comboBox1);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form8";
            Text = "文字化け作成";
            Load += Form8_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 開くToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文字コードToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SELCP;
        private System.Windows.Forms.ToolStripMenuItem SJIS;
        private System.Windows.Forms.ToolStripMenuItem 開くToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label unihex;
    }
}