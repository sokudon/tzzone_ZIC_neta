namespace neta
{
    partial class ZIC
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
            tzutc = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(22, 12);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox1.Size = new System.Drawing.Size(751, 273);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(249, 321);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(216, 74);
            button1.TabIndex = 1;
            button1.Text = "tzdataを開く";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tzutc
            // 
            tzutc.AutoSize = true;
            tzutc.Checked = true;
            tzutc.CheckState = System.Windows.Forms.CheckState.Checked;
            tzutc.Location = new System.Drawing.Point(472, 347);
            tzutc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tzutc.Name = "tzutc";
            tzutc.Size = new System.Drawing.Size(323, 24);
            tzutc.TabIndex = 26;
            tzutc.Text = "unix秒を現地時間に変更,オフセットを3600で割る";
            tzutc.UseVisualStyleBackColor = true;
            // 
            // ZIC
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(tzutc);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Name = "ZIC";
            Text = "ZICバイナリビューアー　";
            Load += Form5_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox tzutc;
    }
}