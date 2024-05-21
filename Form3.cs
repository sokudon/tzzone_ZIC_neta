using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace neta
{
    public partial class dtformat : Form
    {
        Form f1;

        public dtformat(Form f)
        {
            f1 = f; // メイン・フォームへの参照を保存
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo> zoneinfo
        = TimeZoneInfo.GetSystemTimeZones();

            foreach (TimeZoneInfo z in zoneinfo)
            {
                if (z.DisplayName.IndexOf("廃止") < 0) {
                    comboBox2.Items.Add(z.DisplayName + " - " + z.Id);
                } }

            textBox1.Text = Properties.Settings.Default.lefttimeformat;
            textBox2.Text = Properties.Settings.Default.datetimeformat;
            checkBox1.Checked = Properties.Settings.Default.useutc;
            checkBox2.Checked = Properties.Settings.Default.usems;
            comboBox1.Text = Properties.Settings.Default.useutczone;
            comboBox2.Text = Properties.Settings.Default.msstring;
            comboBox3.Text = Properties.Settings.Default.barlen.ToString();
            textBox3.Text = Properties.Settings.Default.api;
            textBox4.Text = Properties.Settings.Default.parse;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.lefttimeformat = textBox1.Text;
        }

        private void dtformat_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.useutc = checkBox1.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.useutczone = comboBox1.Text;
            Properties.Settings.Default.useutcint = gettimeoffset(comboBox1.Text);
        }

        private double gettimeoffset(string dt)
        {
            double i = 9;
            var m = Regex.Match(dt, "\\+?\\-?\\d\\d");
            if (m.Success)
            {
                i = Convert.ToDouble(m.Value);
                Properties.Settings.Default.useutch = m.Value;
                m = m.NextMatch();
                i = i + Convert.ToDouble(m.Value) / 60;
                Properties.Settings.Default.useutcm = m.Value;
            }


            return i;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.usems = checkBox2.Checked;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            var m = Regex.Match(comboBox2.Text, " \\-.+");
            if (m.Success)
            {
                string tm = m.Value.Replace(" -", "").Trim();
                try
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(tm);

                    Properties.Settings.Default.mstime = tm;
                }
                catch (Exception ex) {
                    MessageBox.Show("廃止されたタイムゾーンのため、東京に差し替えます。\r\n" + ex.ToString());
                    Properties.Settings.Default.mstime = "Tokyo Standard Time";
                }
            }
            else
            {
                Properties.Settings.Default.mstime = "Tokyo Standard Time";
            }

            Properties.Settings.Default.msstring = comboBox2.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            DateTime st = DateTime.Now;
            try
            {
                st.ToString(textBox2.Text);

                Properties.Settings.Default.datetimeformat = textBox2.Text;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            var m = Regex.Match(comboBox3.Text, "^\\d+");
            if (m.Success)
            {
                var len = Convert.ToInt32(m.Value);
                if (len > 390) {
                    len = 390;
                }
                Properties.Settings.Default.barlen = len;
                Properties.Settings.Default.parcent = len + 5;
                ((NETA_TIMER)this.Owner).progressBar1.Width = len;
                ((NETA_TIMER)this.Owner).parcent.Left = len + 5;
            }
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {

            var m = Regex.Match(comboBox3.Text, "^\\d+");
            if (m.Success)
            {
                var len = Convert.ToInt32(m.Value);
                if (len > 390)
                {
                    len = 390;
                }
                Properties.Settings.Default.barlen = len;
                Properties.Settings.Default.parcent = len + 5;
                ((NETA_TIMER)this.Owner).progressBar1.Width = len;
                ((NETA_TIMER)this.Owner).parcent.Left = len + 5;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            var url = "^s?https?://[ -_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$";
        
                var m = Regex.Match(textBox3.Text, url);
                if (m.Success)
                {
                    Properties.Settings.Default.api = textBox3.Text;
                }
           
        }


        //mobaapi
        //　https://pink-check.school/api/v2/events/?time=TODAY()
        //　/content/shortName,/content/detail/beginDateTime,/content/detail/endDateTime

        //miriapi
        //　https://api.matsurihi.me/mltd/v1/events/?at=TODAY() 15:00";
        //　/name,/schedule/beginDate,/schedule/endDate

        //samplegoog
        //　https://script.google.com/macros/s/AKfycbw__S8TqcNhP4XbwRfb0UR0KfiT0rhg7KmtOCchftmR_AsYmDJJNe8Z5g/exec
        //　/data/name,/data/start,/data/end


        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            var op = textBox4.Text.Split(',');
            if (op.Length != 3)
            {
                MessageBox.Show("/(パス名 イベ名),/(パス名 開始),/(パス名　終了) ,カンマ区切りの対象パスが３つ必要です");
            }
            else
            {

                var m = Regex.Match(op[0], "^\\/.+$");
                var m1 = Regex.Match(op[1], "^\\/.+$");
                var m2 = Regex.Match(op[2], "^\\/.+$");

               
                    if (m.Success && m1.Success && m2.Success)
                    {
                        Properties.Settings.Default.parse = textBox4.Text;
                    }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            DateTime dt = DateTime.Now;
            System.Diagnostics.Process.Start(textBox3.Text.ToString().Replace("TODAY()", dt.ToString("yyyy-MM-dd")));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists("Newtonsoft.Json.dll"))
            {

            }
            else
            {
                string message = "JSON解析に必要なdllライブラリが不足しています。ダウンロードしますか？";

                string caption = "DLLライブラリ不足";

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                DialogResult result = MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)

                {
                    string urldll = "https://github.com/sokudon/netataimaC-/raw/master/bin/Release/Newtonsoft.Json.dll";
                    WebClient dlldl = new WebClient();
                    dlldl.DownloadFile(urldll, "Newtonsoft.Json.dll");

                }
                else
                {
                    return;
                }

            }
            var form4 = new Form4();
            form4.ShowDialog();
            form4.Dispose();


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }


    }
}
