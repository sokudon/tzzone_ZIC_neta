namespace neta
{
    partial class Form7
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
            script = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            scene = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            source = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            title = new System.Windows.Forms.TextBox();
            start = new System.Windows.Forms.TextBox();
            end = new System.Windows.Forms.TextBox();
            button1 = new System.Windows.Forms.Button();
            ip = new System.Windows.Forms.TextBox();
            port = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            label10 = new System.Windows.Forms.Label();
            req = new System.Windows.Forms.ComboBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // script
            // 
            script.FormattingEnabled = true;
            script.Items.AddRange(new object[] { "obs_sample.lua", "obs_sample.py" });
            script.Location = new System.Drawing.Point(104, 70);
            script.Name = "script";
            script.Size = new System.Drawing.Size(177, 28);
            script.TabIndex = 0;
            script.Text = "obsduration_timer.lua";
            script.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(10, 70);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(88, 20);
            label1.TabIndex = 1;
            label1.Text = "script_name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(10, 7);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(89, 20);
            label2.TabIndex = 2;
            label2.Text = "scene_name";
            // 
            // scene
            // 
            scene.Location = new System.Drawing.Point(102, 4);
            scene.Name = "scene";
            scene.Size = new System.Drawing.Size(177, 27);
            scene.TabIndex = 3;
            scene.Text = "scene2";
            scene.TextChanged += textBox1_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(9, 37);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(95, 20);
            label3.TabIndex = 4;
            label3.Text = "source_name";
            // 
            // source
            // 
            source.Location = new System.Drawing.Point(104, 37);
            source.Name = "source";
            source.Size = new System.Drawing.Size(177, 27);
            source.TabIndex = 5;
            source.Text = "yume";
            source.TextChanged += textBox2_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(13, 107);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(35, 20);
            label4.TabIndex = 6;
            label4.Text = "title";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(13, 137);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(38, 20);
            label5.TabIndex = 7;
            label5.Text = "start";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(13, 173);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(34, 20);
            label6.TabIndex = 8;
            label6.Text = "end";
            // 
            // title
            // 
            title.Location = new System.Drawing.Point(103, 104);
            title.Name = "title";
            title.Size = new System.Drawing.Size(178, 27);
            title.TabIndex = 9;
            // 
            // start
            // 
            start.Location = new System.Drawing.Point(101, 137);
            start.Name = "start";
            start.Size = new System.Drawing.Size(178, 27);
            start.TabIndex = 10;
            // 
            // end
            // 
            end.Location = new System.Drawing.Point(103, 170);
            end.Name = "end";
            end.Size = new System.Drawing.Size(178, 27);
            end.TabIndex = 11;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(214, 281);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(79, 29);
            button1.TabIndex = 12;
            button1.Text = "send";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // ip
            // 
            ip.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            ip.Location = new System.Drawing.Point(49, 248);
            ip.Name = "ip";
            ip.Size = new System.Drawing.Size(114, 27);
            ip.TabIndex = 13;
            ip.Text = "192.168.1.100";
            ip.TextChanged += ip_TextChanged;
            // 
            // port
            // 
            port.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            port.Location = new System.Drawing.Point(212, 248);
            port.Name = "port";
            port.Size = new System.Drawing.Size(79, 27);
            port.TabIndex = 14;
            port.Text = "4455";
            port.TextChanged += port_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(22, 251);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(21, 20);
            label7.TabIndex = 15;
            label7.Text = "IP";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(169, 251);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(37, 20);
            label8.TabIndex = 16;
            label8.Text = "port";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(22, 285);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(58, 20);
            label9.TabIndex = 17;
            label9.Text = "request";
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(scene);
            panel1.Controls.Add(source);
            panel1.Controls.Add(script);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(end);
            panel1.Controls.Add(title);
            panel1.Controls.Add(start);
            panel1.Location = new System.Drawing.Point(12, 23);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(291, 208);
            panel1.TabIndex = 19;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(30, 10);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(78, 20);
            label10.TabIndex = 20;
            label10.Text = " {setting:?}";
            // 
            // req
            // 
            req.FormattingEnabled = true;
            req.Items.AddRange(new object[] { "", "GetVersion", "StartStream", "StopStream", "StartRecord", "StopRecord" });
            req.Location = new System.Drawing.Point(86, 282);
            req.Name = "req";
            req.Size = new System.Drawing.Size(120, 28);
            req.TabIndex = 21;
            req.SelectedIndexChanged += req_SelectedIndexChanged;
            req.TextChanged += req_SelectedIndexChanged;
            // 
            // Form7
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(315, 322);
            Controls.Add(req);
            Controls.Add(label10);
            Controls.Add(panel1);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(port);
            Controls.Add(ip);
            Controls.Add(button1);
            Name = "Form7";
            Text = "OBSWEBSOCKET";
            Load += Form7_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox script;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox scene;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox source;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox title;
        private System.Windows.Forms.TextBox start;
        private System.Windows.Forms.TextBox end;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox ip;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox request;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox req;
    }
}