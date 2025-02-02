namespace neta
{
    partial class Form6
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
            label1 = new System.Windows.Forms.Label();
            now = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            left = new System.Windows.Forms.ComboBox();
            label4 = new System.Windows.Forms.Label();
            elapsed = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            span = new System.Windows.Forms.ComboBox();
            start = new System.Windows.Forms.ComboBox();
            end = new System.Windows.Forms.ComboBox();
            comboBox1 = new System.Windows.Forms.ComboBox();
            label8 = new System.Windows.Forms.Label();
            comboBox2 = new System.Windows.Forms.ComboBox();
            comboBox3 = new System.Windows.Forms.ComboBox();
            label9 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(22, 40);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(69, 20);
            label1.TabIndex = 0;
            label1.Text = "現在時刻";
            // 
            // now
            // 
            now.FormattingEnabled = true;
            now.Items.AddRange(new object[] { "現在時刻:", "現在:", "current:", "NOW:" });
            now.Location = new System.Drawing.Point(112, 37);
            now.Name = "now";
            now.Size = new System.Drawing.Size(105, 28);
            now.TabIndex = 1;
            now.Text = "現在時刻:";
            now.SelectedIndexChanged += now_SelectedIndexChanged;
            now.TextChanged += now_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(22, 74);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(0, 20);
            label2.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(22, 105);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(64, 20);
            label3.TabIndex = 3;
            label3.Text = "残り時間";
            // 
            // left
            // 
            left.FormattingEnabled = true;
            left.Items.AddRange(new object[] { "残り時間:", "残り:", "LEFT TIME:", "LEFT:" });
            left.Location = new System.Drawing.Point(112, 102);
            left.Name = "left";
            left.Size = new System.Drawing.Size(105, 28);
            left.TabIndex = 4;
            left.Text = "残り時間:";
            left.SelectedIndexChanged += left_SelectedIndexChanged;
            left.TextChanged += left_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(22, 71);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(69, 20);
            label4.TabIndex = 5;
            label4.Text = "経過時間";
            // 
            // elapsed
            // 
            elapsed.FormattingEnabled = true;
            elapsed.Items.AddRange(new object[] { "経過時間:", "経過:", "ELAPSED TIME:", "ELAPSED:" });
            elapsed.Location = new System.Drawing.Point(112, 71);
            elapsed.Name = "elapsed";
            elapsed.Size = new System.Drawing.Size(105, 28);
            elapsed.TabIndex = 6;
            elapsed.Text = "経過時間";
            elapsed.SelectedIndexChanged += elapsted_SelectedIndexChanged;
            elapsed.TextChanged += elapsted_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(24, 139);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(62, 20);
            label5.TabIndex = 7;
            label5.Text = "イベ期間";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(22, 173);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(69, 20);
            label6.TabIndex = 8;
            label6.Text = "開始時間";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(22, 207);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(69, 20);
            label7.TabIndex = 9;
            label7.Text = "終了時間";
            // 
            // span
            // 
            span.FormattingEnabled = true;
            span.Items.AddRange(new object[] { "イベ期間:", "期間:", "DURATION:", "SPAN:" });
            span.Location = new System.Drawing.Point(112, 136);
            span.Name = "span";
            span.Size = new System.Drawing.Size(105, 28);
            span.TabIndex = 10;
            span.Text = "イベ期間:";
            span.SelectedIndexChanged += span_SelectedIndexChanged;
            span.TextChanged += span_SelectedIndexChanged;
            // 
            // start
            // 
            start.FormattingEnabled = true;
            start.Items.AddRange(new object[] { "開始時間:", "開始:", "START TIME:", "START:" });
            start.Location = new System.Drawing.Point(112, 170);
            start.Name = "start";
            start.Size = new System.Drawing.Size(105, 28);
            start.TabIndex = 11;
            start.Text = "開始時間:";
            start.SelectedIndexChanged += start_SelectedIndexChanged;
            start.TextChanged += start_SelectedIndexChanged;
            // 
            // end
            // 
            end.FormattingEnabled = true;
            end.Items.AddRange(new object[] { "終了時間:", "終了:", "END TIME:", "END:" });
            end.Location = new System.Drawing.Point(112, 204);
            end.Name = "end";
            end.Size = new System.Drawing.Size(105, 28);
            end.TabIndex = 12;
            end.Text = "終了時間:";
            end.SelectedIndexChanged += end_SelectedIndexChanged;
            end.TextChanged += end_SelectedIndexChanged;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "があいまいな時間の範囲です", "あいまいな時間:タイムゾーン情報付きで時刻が入力されてない場合ライブラリ別に処理が異なるため不正確になります", " is ambigous", " Ambiguous times: If the time is not entered with time zone information,it will be inaccurate because the processing differs depending on the library." });
            comboBox1.Location = new System.Drawing.Point(114, 238);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(105, 28);
            comboBox1.TabIndex = 13;
            comboBox1.Text = "があいまいな時間の範囲です";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(24, 242);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(56, 20);
            label8.TabIndex = 14;
            label8.Text = "あいまい";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "イベントはすでに終了しています", " has finished" });
            comboBox2.Location = new System.Drawing.Point(112, 272);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(105, 28);
            comboBox2.TabIndex = 15;
            comboBox2.Text = "イベントはすでに終了しています";
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "イベントがまだ開始されてません", " not yet starts" });
            comboBox3.Location = new System.Drawing.Point(112, 306);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new System.Drawing.Size(105, 28);
            comboBox3.TabIndex = 16;
            comboBox3.Text = "イベントがまだ開始されてません";
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(24, 275);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(54, 20);
            label9.TabIndex = 17;
            label9.Text = "終了時";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(24, 306);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(54, 20);
            label10.TabIndex = 18;
            label10.Text = "開始前";
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new System.Drawing.Point(255, 37);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new System.Drawing.Size(150, 158);
            checkedListBox1.TabIndex = 20;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(255, 207);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(94, 29);
            button1.TabIndex = 21;
            button1.Text = "↑";
            button1.UseVisualStyleBackColor = true;
            button1.Click += buttonUp_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(355, 207);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(94, 29);
            button2.TabIndex = 22;
            button2.Text = "↓";
            button2.UseVisualStyleBackColor = true;
            button2.Click += buttonDown_Click;
            // 
            // Form6
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(474, 387);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(checkedListBox1);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(comboBox3);
            Controls.Add(comboBox2);
            Controls.Add(label8);
            Controls.Add(comboBox1);
            Controls.Add(end);
            Controls.Add(start);
            Controls.Add(span);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(elapsed);
            Controls.Add(label4);
            Controls.Add(left);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(now);
            Controls.Add(label1);
            Name = "Form6";
            Text = "custom_string";
            FormClosing += Form6_FormClosing;
            Load += Form6_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox now;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox left;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox elapsed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox span;
        private System.Windows.Forms.ComboBox start;
        private System.Windows.Forms.ComboBox end;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}