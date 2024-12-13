namespace neta
{
    partial class NETA_TIMER
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            current = new System.Windows.Forms.Label();
            startbox = new System.Windows.Forms.TextBox();
            endbox = new System.Windows.Forms.TextBox();
            elapsed = new System.Windows.Forms.Label();
            left = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            parcent = new System.Windows.Forms.Label();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            eventname = new System.Windows.Forms.Label();
            end = new System.Windows.Forms.Label();
            start = new System.Windows.Forms.Label();
            duration = new System.Windows.Forms.Label();
            button2 = new System.Windows.Forms.Button();
            ibemei = new System.Windows.Forms.TextBox();
            panel2 = new System.Windows.Forms.Panel();
            button3 = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            comboBox1 = new System.Windows.Forms.ComboBox();
            timer1 = new System.Windows.Forms.Timer(components);
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            時刻設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            外部つーるへエクスポートToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            oBSタイマーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            luascriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pythonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ぱいそんたいまーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            wEBたいまーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            wEBせかいどけいToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            バージョンToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            netaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            色の設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            下パネルを隠すToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            下パネルを表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            クロマキー青 = new System.Windows.Forms.ToolStripMenuItem();
            クロマキー赤 = new System.Windows.Forms.ToolStripMenuItem();
            クロマキー緑 = new System.Windows.Forms.ToolStripMenuItem();
            カラーキー今のメニューToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            カラーキーなしToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            文字白ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            文字黒ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            フォントToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // current
            // 
            current.AutoSize = true;
            current.BackColor = System.Drawing.SystemColors.Menu;
            current.ForeColor = System.Drawing.Color.Black;
            current.Location = new System.Drawing.Point(4, 64);
            current.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            current.Name = "current";
            current.Size = new System.Drawing.Size(72, 20);
            current.TabIndex = 0;
            current.Text = "現在時間:";
            // 
            // startbox
            // 
            startbox.BackColor = System.Drawing.Color.White;
            startbox.ForeColor = System.Drawing.Color.Black;
            startbox.Location = new System.Drawing.Point(16, 61);
            startbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            startbox.Name = "startbox";
            startbox.Size = new System.Drawing.Size(281, 27);
            startbox.TabIndex = 1;
            startbox.Text = "2020-12-16T06:00:00Z";
            // 
            // endbox
            // 
            endbox.BackColor = System.Drawing.Color.White;
            endbox.ForeColor = System.Drawing.Color.Black;
            endbox.Location = new System.Drawing.Point(16, 104);
            endbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            endbox.Name = "endbox";
            endbox.Size = new System.Drawing.Size(283, 27);
            endbox.TabIndex = 2;
            endbox.Text = "2020/12/17 21:00";
            // 
            // elapsed
            // 
            elapsed.AutoSize = true;
            elapsed.Location = new System.Drawing.Point(5, 100);
            elapsed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            elapsed.Name = "elapsed";
            elapsed.Size = new System.Drawing.Size(72, 20);
            elapsed.TabIndex = 4;
            elapsed.Text = "経過時間:";
            // 
            // left
            // 
            left.AutoSize = true;
            left.Location = new System.Drawing.Point(5, 136);
            left.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            left.Name = "left";
            left.Size = new System.Drawing.Size(67, 20);
            left.TabIndex = 5;
            left.Text = "残り時間:";
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.SystemColors.Menu;
            panel1.Controls.Add(parcent);
            panel1.Controls.Add(progressBar1);
            panel1.Controls.Add(eventname);
            panel1.Controls.Add(end);
            panel1.Controls.Add(start);
            panel1.Controls.Add(duration);
            panel1.Controls.Add(current);
            panel1.Controls.Add(left);
            panel1.Controls.Add(elapsed);
            panel1.ForeColor = System.Drawing.Color.Black;
            panel1.Location = new System.Drawing.Point(36, 48);
            panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(576, 317);
            panel1.TabIndex = 6;
            // 
            // parcent
            // 
            parcent.AutoSize = true;
            parcent.Location = new System.Drawing.Point(535, 292);
            parcent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            parcent.Name = "parcent";
            parcent.Size = new System.Drawing.Size(29, 20);
            parcent.TabIndex = 11;
            parcent.Text = "0%";
            parcent.Click += parcent_Click;
            // 
            // progressBar1
            // 
            progressBar1.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            progressBar1.ForeColor = System.Drawing.Color.FromArgb(192, 255, 192);
            progressBar1.Location = new System.Drawing.Point(7, 277);
            progressBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(520, 35);
            progressBar1.TabIndex = 10;
            progressBar1.Value = 90;
            // 
            // eventname
            // 
            eventname.AutoSize = true;
            eventname.BackColor = System.Drawing.SystemColors.Menu;
            eventname.ForeColor = System.Drawing.Color.Black;
            eventname.Location = new System.Drawing.Point(5, 27);
            eventname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            eventname.Name = "eventname";
            eventname.Size = new System.Drawing.Size(71, 20);
            eventname.TabIndex = 9;
            eventname.Text = "イベント名:";
            // 
            // end
            // 
            end.AutoSize = true;
            end.Location = new System.Drawing.Point(5, 252);
            end.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            end.Name = "end";
            end.Size = new System.Drawing.Size(72, 20);
            end.TabIndex = 8;
            end.Text = "終了時間:";
            // 
            // start
            // 
            start.AutoSize = true;
            start.Location = new System.Drawing.Point(5, 212);
            start.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            start.Name = "start";
            start.Size = new System.Drawing.Size(69, 20);
            start.TabIndex = 7;
            start.Text = "開始時間";
            // 
            // duration
            // 
            duration.AutoSize = true;
            duration.Location = new System.Drawing.Point(5, 173);
            duration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            duration.Name = "duration";
            duration.Size = new System.Drawing.Size(65, 20);
            duration.TabIndex = 6;
            duration.Text = "イベ期間:";
            // 
            // button2
            // 
            button2.ForeColor = System.Drawing.Color.Black;
            button2.Location = new System.Drawing.Point(325, 88);
            button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(120, 47);
            button2.TabIndex = 7;
            button2.Text = "ぐぐるから取得";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // ibemei
            // 
            ibemei.BackColor = System.Drawing.Color.White;
            ibemei.ForeColor = System.Drawing.Color.Black;
            ibemei.Location = new System.Drawing.Point(15, 24);
            ibemei.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ibemei.Name = "ibemei";
            ibemei.Size = new System.Drawing.Size(283, 27);
            ibemei.TabIndex = 8;
            ibemei.Text = "ibemei";
            // 
            // panel2
            // 
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(comboBox1);
            panel2.Controls.Add(ibemei);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(startbox);
            panel2.Controls.Add(endbox);
            panel2.Location = new System.Drawing.Point(36, 389);
            panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(576, 167);
            panel2.TabIndex = 9;
            // 
            // button3
            // 
            button3.ForeColor = System.Drawing.Color.Black;
            button3.Location = new System.Drawing.Point(459, 24);
            button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(100, 39);
            button3.TabIndex = 11;
            button3.Text = "カスタムJS";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button1
            // 
            button1.ForeColor = System.Drawing.Color.Black;
            button1.Location = new System.Drawing.Point(453, 88);
            button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(105, 47);
            button1.TabIndex = 10;
            button1.Text = "カレンダ-作成";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // comboBox1
            // 
            comboBox1.BackColor = System.Drawing.Color.White;
            comboBox1.ForeColor = System.Drawing.Color.Black;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "シャニマス", "でれすて", "みりした", "プロセカ", "かすたむJS" });
            comboBox1.Location = new System.Drawing.Point(324, 23);
            comboBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(121, 28);
            comboBox1.TabIndex = 9;
            comboBox1.Text = "シャニマス";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 時刻設定ToolStripMenuItem, 外部つーるへエクスポートToolStripMenuItem, バージョンToolStripMenuItem, netaToolStripMenuItem, 色の設定ToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            menuStrip1.Size = new System.Drawing.Size(638, 30);
            menuStrip1.TabIndex = 10;
            menuStrip1.Text = "menuStrip1";
            // 
            // 時刻設定ToolStripMenuItem
            // 
            時刻設定ToolStripMenuItem.Name = "時刻設定ToolStripMenuItem";
            時刻設定ToolStripMenuItem.Size = new System.Drawing.Size(83, 24);
            時刻設定ToolStripMenuItem.Text = "時刻設定";
            時刻設定ToolStripMenuItem.Click += 時刻設定ToolStripMenuItem_Click;
            // 
            // 外部つーるへエクスポートToolStripMenuItem
            // 
            外部つーるへエクスポートToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { oBSタイマーToolStripMenuItem, ぱいそんたいまーToolStripMenuItem, wEBたいまーToolStripMenuItem, wEBせかいどけいToolStripMenuItem });
            外部つーるへエクスポートToolStripMenuItem.Name = "外部つーるへエクスポートToolStripMenuItem";
            外部つーるへエクスポートToolStripMenuItem.Size = new System.Drawing.Size(90, 24);
            外部つーるへエクスポートToolStripMenuItem.Text = "えくすぽーと";
            // 
            // oBSタイマーToolStripMenuItem
            // 
            oBSタイマーToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { luascriptToolStripMenuItem, pythonToolStripMenuItem });
            oBSタイマーToolStripMenuItem.Name = "oBSタイマーToolStripMenuItem";
            oBSタイマーToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            oBSタイマーToolStripMenuItem.Text = "OBSたいまーぷらぐいん";
            oBSタイマーToolStripMenuItem.Click += oBSタイマーToolStripMenuItem_Click;
            // 
            // luascriptToolStripMenuItem
            // 
            luascriptToolStripMenuItem.Name = "luascriptToolStripMenuItem";
            luascriptToolStripMenuItem.Size = new System.Drawing.Size(148, 26);
            luascriptToolStripMenuItem.Text = "luascript";
            luascriptToolStripMenuItem.Click += luascriptToolStripMenuItem_Click;
            // 
            // pythonToolStripMenuItem
            // 
            pythonToolStripMenuItem.Name = "pythonToolStripMenuItem";
            pythonToolStripMenuItem.Size = new System.Drawing.Size(148, 26);
            pythonToolStripMenuItem.Text = "python";
            pythonToolStripMenuItem.Click += pythonToolStripMenuItem_Click;
            // 
            // ぱいそんたいまーToolStripMenuItem
            // 
            ぱいそんたいまーToolStripMenuItem.Name = "ぱいそんたいまーToolStripMenuItem";
            ぱいそんたいまーToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            ぱいそんたいまーToolStripMenuItem.Text = "ぱいそんたいまー";
            ぱいそんたいまーToolStripMenuItem.Click += ぱいそんたいまーToolStripMenuItem_Click;
            // 
            // wEBたいまーToolStripMenuItem
            // 
            wEBたいまーToolStripMenuItem.Name = "wEBたいまーToolStripMenuItem";
            wEBたいまーToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            wEBたいまーToolStripMenuItem.Text = "WEBたいまー";
            wEBたいまーToolStripMenuItem.Click += wEBたいまーToolStripMenuItem_Click;
            // 
            // wEBせかいどけいToolStripMenuItem
            // 
            wEBせかいどけいToolStripMenuItem.Name = "wEBせかいどけいToolStripMenuItem";
            wEBせかいどけいToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            wEBせかいどけいToolStripMenuItem.Text = "WEBせかいどけい";
            wEBせかいどけいToolStripMenuItem.Click += wEBせかいどけいToolStripMenuItem_Click;
            // 
            // バージョンToolStripMenuItem
            // 
            バージョンToolStripMenuItem.Name = "バージョンToolStripMenuItem";
            バージョンToolStripMenuItem.Size = new System.Drawing.Size(77, 24);
            バージョンToolStripMenuItem.Text = "バージョン";
            バージョンToolStripMenuItem.Click += バージョンToolStripMenuItem_Click;
            // 
            // netaToolStripMenuItem
            // 
            netaToolStripMenuItem.Name = "netaToolStripMenuItem";
            netaToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            netaToolStripMenuItem.Text = "tzdata_neta zic binary view";
            netaToolStripMenuItem.Click += netaToolStripMenuItem_Click;
            // 
            // 色の設定ToolStripMenuItem
            // 
            色の設定ToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            色の設定ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { 下パネルを隠すToolStripMenuItem, 下パネルを表示ToolStripMenuItem, クロマキー青, クロマキー赤, クロマキー緑, カラーキー今のメニューToolStripMenuItem, カラーキーなしToolStripMenuItem, 文字白ToolStripMenuItem, 文字黒ToolStripMenuItem, フォントToolStripMenuItem });
            色の設定ToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            色の設定ToolStripMenuItem.Name = "色の設定ToolStripMenuItem";
            色の設定ToolStripMenuItem.Size = new System.Drawing.Size(80, 24);
            色の設定ToolStripMenuItem.Text = "色の設定";
            // 
            // 下パネルを隠すToolStripMenuItem
            // 
            下パネルを隠すToolStripMenuItem.Name = "下パネルを隠すToolStripMenuItem";
            下パネルを隠すToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            下パネルを隠すToolStripMenuItem.Text = "下パネルを隠す";
            下パネルを隠すToolStripMenuItem.Click += 下パネルを隠すToolStripMenuItem_Click;
            // 
            // 下パネルを表示ToolStripMenuItem
            // 
            下パネルを表示ToolStripMenuItem.Name = "下パネルを表示ToolStripMenuItem";
            下パネルを表示ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            下パネルを表示ToolStripMenuItem.Text = "下パネルを表示";
            下パネルを表示ToolStripMenuItem.Click += 下パネルを表示ToolStripMenuItem_Click;
            // 
            // クロマキー青
            // 
            クロマキー青.Name = "クロマキー青";
            クロマキー青.Size = new System.Drawing.Size(224, 26);
            クロマキー青.Text = "クロマキー青";
            クロマキー青.Click += クロマキー青_Click;
            // 
            // クロマキー赤
            // 
            クロマキー赤.Name = "クロマキー赤";
            クロマキー赤.Size = new System.Drawing.Size(224, 26);
            クロマキー赤.Text = "クロマキー赤";
            クロマキー赤.Click += クロマキー赤_Click;
            // 
            // クロマキー緑
            // 
            クロマキー緑.Name = "クロマキー緑";
            クロマキー緑.Size = new System.Drawing.Size(224, 26);
            クロマキー緑.Text = "クロマキー緑";
            クロマキー緑.Click += クロマキー緑_Click;
            // 
            // カラーキー今のメニューToolStripMenuItem
            // 
            カラーキー今のメニューToolStripMenuItem.Name = "カラーキー今のメニューToolStripMenuItem";
            カラーキー今のメニューToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            カラーキー今のメニューToolStripMenuItem.Text = "カラーキー今のメニュー";
            カラーキー今のメニューToolStripMenuItem.Click += カラーキー今のメニューToolStripMenuItem_Click;
            // 
            // カラーキーなしToolStripMenuItem
            // 
            カラーキーなしToolStripMenuItem.Name = "カラーキーなしToolStripMenuItem";
            カラーキーなしToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            カラーキーなしToolStripMenuItem.Text = "カラーキーなし";
            カラーキーなしToolStripMenuItem.Click += カラーキーなしToolStripMenuItem_Click;
            // 
            // 文字白ToolStripMenuItem
            // 
            文字白ToolStripMenuItem.Name = "文字白ToolStripMenuItem";
            文字白ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            文字白ToolStripMenuItem.Text = "文字　白";
            文字白ToolStripMenuItem.Click += 文字白ToolStripMenuItem_Click;
            // 
            // 文字黒ToolStripMenuItem
            // 
            文字黒ToolStripMenuItem.Name = "文字黒ToolStripMenuItem";
            文字黒ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            文字黒ToolStripMenuItem.Text = "文字　黒";
            文字黒ToolStripMenuItem.Click += 文字黒ToolStripMenuItem_Click;
            // 
            // フォントToolStripMenuItem
            // 
            フォントToolStripMenuItem.Name = "フォントToolStripMenuItem";
            フォントToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            フォントToolStripMenuItem.Text = "フォント";
            フォントToolStripMenuItem.Click += フォントToolStripMenuItem_Click;
            // 
            // NETA_TIMER
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(638, 578);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            DoubleBuffered = true;
            ForeColor = System.Drawing.Color.Black;
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "NETA_TIMER";
            Text = "NETA_TIMER";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label current;
        private System.Windows.Forms.TextBox startbox;
        private System.Windows.Forms.TextBox endbox;
        private System.Windows.Forms.Label elapsed;
        private System.Windows.Forms.Label left;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label start;
        private System.Windows.Forms.Label duration;
        private System.Windows.Forms.Label end;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox ibemei;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label eventname;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 時刻設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem バージョンToolStripMenuItem;
        public System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.Label parcent;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem 外部つーるへエクスポートToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wEBたいまーToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ぱいそんたいまーToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oBSタイマーToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wEBせかいどけいToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem netaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem luascriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pythonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 色の設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下パネルを隠すToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下パネルを表示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem クロマキー青;
        private System.Windows.Forms.ToolStripMenuItem クロマキー赤;
        private System.Windows.Forms.ToolStripMenuItem クロマキー緑;
        private System.Windows.Forms.ToolStripMenuItem カラーキー今のメニューToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem カラーキーなしToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文字白ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文字黒ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem フォントToolStripMenuItem;
    }
}

