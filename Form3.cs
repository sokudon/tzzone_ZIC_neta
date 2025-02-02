using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using System.Collections.ObjectModel;
using System.Buffers.Binary;
using System.Text.Json;
using TZPASER;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.WebRequestMethods;
using NodaTime.Text;
using NodaTime;
using System.Security.Cryptography;
using System.Globalization;
using System.Windows.Controls;
using System.Threading;


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
            ReadOnlyCollection<TimeZoneInfo> zoneinfo = TimeZoneInfo.GetSystemTimeZones();

            foreach (TimeZoneInfo z in zoneinfo)
            {
                if (z.DisplayName.IndexOf("廃止") < 0)
                {
                    ms_timezone_items.Items.Add(z.DisplayName + " - " + z.Id);
                }
            }

            LoadCultures();


            elapst_left.Text = Properties.Settings.Default.lefttimeformat;
            normal_dateformat.Text = Properties.Settings.Default.datetimeformat;
            parse_target.Text = Properties.Settings.Default.parse;
            tzbinary_dir.Text = Properties.Settings.Default.lasttzdatapath_base_utc;
            parse_test.Text = Properties.Settings.Default.datetester;
            y_start.Text = Properties.Settings.Default.stfilter;
            y_end.Text = Properties.Settings.Default.enfilter;

            ms_utcoffset.Checked = Properties.Settings.Default.useutc;
            ms_timezone.Checked = Properties.Settings.Default.usems;
            tzbinary_timezone.Checked = Properties.Settings.Default.usetz;
            checkBox4.Checked = Properties.Settings.Default.usefiler;
            custom_local.Checked = Properties.Settings.Default.local_chager;
            ms_utcoffset_items.Text = Properties.Settings.Default.useutczone;
            ms_timezone_items.Text = Properties.Settings.Default.msstring;
            bar_length.Text = Properties.Settings.Default.barlen.ToString();


            tzbinary_tzst.Text = Properties.Settings.Default.usetzdatabin;
            custom_url_path.Text = Properties.Settings.Default.api;



            noda_timezone.Checked = Properties.Settings.Default.usenoda;
            noda_timezone_items.Text = Properties.Settings.Default.noddatz;

            noda_dateformat.Text = Properties.Settings.Default.nodaformat;


            invaid_ambigous.Checked = Properties.Settings.Default.noda_strict;

            panel6.Location = new System.Drawing.Point(5, panel6.Location.Y);


            change_baseurl.Checked = Properties.Settings.Default.change_baseurl;
            localeBox.Text = Properties.Settings.Default.locale;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void dtformat_FormClosed(object sender, FormClosedEventArgs e)
        {
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.useutc = ms_utcoffset.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.useutczone = ms_utcoffset_items.Text;
            Properties.Settings.Default.useutcint = gettimeoffset(ms_utcoffset_items.Text);
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
            Properties.Settings.Default.usems = ms_timezone.Checked;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            var m = Regex.Match(ms_timezone_items.Text, " \\-.+");
            if (m.Success)
            {
                string tm = m.Value.Replace(" -", "").Trim();
                try
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(tm);

                    Properties.Settings.Default.mstime = tm;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("廃止されたタイムゾーンのため、東京に差し替えます。\r\n" + ex.ToString());
                    Properties.Settings.Default.mstime = "Tokyo Standard Time";
                }
            }
            else
            {
                Properties.Settings.Default.mstime = "Tokyo Standard Time";
            }

            Properties.Settings.Default.msstring = ms_timezone_items.Text;
        }
        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime st = DateTime.Now;
            try
            {
                string format = normal_dateformat.Text;
                bool tz = tzbinary_timezone.Checked;
                string posix = Properties.Settings.Default.footerstring;

                if (!tz)
                {
                    string pattern = @"(%TZ|%z|%Z)";
                    format = Regex.Replace(format, pattern, match => "");
                    string patternn = @"(?<!\\)[!""#$'&%]"; // 「\K \z」を無視し、「Kz」のみマッチ !"#$'&%はダメ文字
                    format = Regex.Replace(format, patternn, match => "");
                    format = Regex.Replace(format, "%PO", match => "");
                }
                else
                {

                    format = Regex.Replace(format, "%PO", match => posix);

                }

                st.ToString(format);

                Properties.Settings.Default.datetimeformat = normal_dateformat.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            var m = Regex.Match(bar_length.Text, "^\\d+");
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

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {

            var m = Regex.Match(bar_length.Text, "^\\d+");
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
            var op = parse_target.Text.Split(',');
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
                    Properties.Settings.Default.parse = parse_target.Text;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("Newtonsoft.Json.dll"))
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Properties.Settings.Default.lasttzdatapath_base_utc;

                openFileDialog.Title = "unix usr/share/tzinfoやpython pytz dateutilのtzdatabaseフォルダのUTCがあるところを選択してください";
                openFileDialog.Filter = "すべてのファイル (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 選択したファイルのパスを取得
                    string filePath = Path.GetDirectoryName(openFileDialog.FileName);

                    Properties.Settings.Default.lasttzdatapath_base_utc = filePath;
                    tzbinary_dir.Text = filePath;
                }
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.usetzdatabin = tzbinary_tzst.Text;
            checkBox3_CheckedChanged(sender, e);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.usetz = tzbinary_timezone.Checked;
            if (tzbinary_timezone.Checked == true)
            {
                if (this.Width < 500)
                {
                    this.Width = this.Width + 450;
                }

                string tzdata = Path.Combine(Properties.Settings.Default.lasttzdatapath_base_utc, Properties.Settings.Default.usetzdatabin);
                if (System.IO.File.Exists(tzdata))
                {
                    System.IO.FileStream fs = new FileStream(tzdata, FileMode.Open, FileAccess.Read);
                    byte[] bs = new byte[fs.Length];
                    fs.Read(bs, 0, bs.Length);
                    fs.Close();
                    string tztxt = TZif_ParseRaw(bs);

                    textBox3.Text = tztxt;
                    return;
                }
            }
            else
            {
                if (this.Width >= 960)
                {
                    this.Width = this.Width - 450;
                }
            }
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // サイズ変更を禁止
            this.MaximizeBox = false; // 最大化ボタンを無効化
        }


        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            var url = "https?://[ -_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$";

            var m = Regex.Match(custom_url_path.Text, url);
            if (m.Success)
            {
                Properties.Settings.Default.api = m.Value;
            }

        }

        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            var url = "https?://[ -_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$";

            var m = Regex.Match(custom_url_path.Text, url);
            if (m.Success)
            {
                Properties.Settings.Default.apipath_raw = noda_timezone.Text;
                Properties.Settings.Default.api = m.Value;
            }
            var path = Properties.Settings.Default.api;
            string tzfile = get_normalize_path(custom_url_path.Text);
            if (tzfile != "")
            {
                Properties.Settings.Default.api = tzfile;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();

            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.lastfile);
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "jsonファイル(*.json)|*.json|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに選択されるものを指定する
            //2番目の「すべてのファイル」が選択されているようにする
            ofd.FilterIndex = 2;
            //タイトルを設定する
            ofd.Title = "開くjsonファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.api = ofd.FileName;
                Properties.Settings.Default.lastfile = Path.GetFileName(Path.GetDirectoryName(ofd.FileName));
                custom_url_path.Text = ofd.FileName;
            }
        }

        private static string get_normalize_dir(string relativePath)
        {
            try
            {
                string fullPath = "";
                //先頭が. ..のパス
                Regex rp = new Regex(@"\.\.?/");
                if (rp.IsMatch(relativePath))
                {

                    // 実行ディレクトリを取得
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    // 絶対パスを取得 (./, ../ を解決)
                    fullPath = Path.GetFullPath(Path.Combine(baseDirectory, relativePath));
                }
                else
                {

                    fullPath = relativePath;
                    Properties.Settings.Default.tzDirectory_raw = "";
                }


                // ファイルの存在を確認
                if (Directory.Exists(fullPath))
                {
                    return fullPath;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                // エラー処理
                Console.WriteLine("エラーが発生しました:");
                Console.WriteLine(ex.Message);
            }

            return "";
        }

        private static string get_normalize_path(string relativePath)
        {
            try
            {
                string fullPath = "";
                //先頭が. ..のパス
                Regex rp = new Regex(@"\.\.?/");
                if (rp.IsMatch(relativePath))
                {

                    // 実行ディレクトリを取得
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    // 絶対パスを取得 (./, ../ を解決)
                    fullPath = Path.GetFullPath(Path.Combine(baseDirectory, relativePath));
                }
                else
                {

                    fullPath = relativePath;
                    Properties.Settings.Default.apipath_raw = "";
                }


                // ファイルの存在を確認
                if (System.IO.File.Exists(fullPath))
                {
                    return fullPath;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                // エラー処理
                Console.WriteLine("エラーが発生しました:");
                Console.WriteLine(ex.Message);
            }

            return "";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string tzdir = get_normalize_dir(tzbinary_dir.Text);
            if (tzdir != "")
            {
                Properties.Settings.Default.lasttzdatapath_base_utc = tzdir;

            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.usefiler = checkBox4.Checked;
            checkBox3_CheckedChanged(sender, e);
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.enfilter = y_end.Text;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.stfilter = y_start.Text;
            int st = Convert.ToInt32(y_start.Text);
            int en = Convert.ToInt32(y_end.Text);

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string tester = parse_test.Text;
            DateTime testDateTime;
            if (TZPASER.FastDateTimeParsing.TryParseFastDateTime(tester, out testDateTime) || TZPASER.RFC2822DateTimeParser.TryParseRFC2822DateTime(tester, out testDateTime))
            {

                Properties.Settings.Default.datetester = parse_test.Text;
            }
        }

        private void custom_local_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.local_chager = custom_local.Checked;
            checkBox3_CheckedChanged(sender, e);

        }

        //ヘッダ処理だけMS仕様
        private static string TZif_ParseRaw(byte[] data)
        {

            // read in the 44-byte TZ header containing the count/length fields
            //
            int index = 0;
            TZifHead t = new TZifHead(data, index);
            index += TZifHead.Length;

            int timeValuesLength = 4; // the first version uses 4-bytes to specify times
            if (t.Version != TZVersion.V1)
            {
                // move index past the V1 information to read the V2 information
                index += (int)((timeValuesLength * t.TimeCount) + t.TimeCount + (6 * t.TypeCount) + ((timeValuesLength + 4) * t.LeapCount) + t.IsStdCount + t.IsGmtCount + t.CharCount);

                // read the V2 header
                t = new TZifHead(data, index);
                index += TZifHead.Length;
                timeValuesLength = 8; // the second version uses 8-bytes
            }

            index -= TZifHead.Length;

            string tzdata = Path.Combine(Properties.Settings.Default.lasttzdatapath_base_utc, Properties.Settings.Default.usetzdatabin);
            string tzst = (Properties.Settings.Default.usetzdatabin);
            byte[] bs = new byte[data.Length];
            Array.ConstrainedCopy(data, 0, bs, 0, data.Length);

            byte[] TZifnext = new byte[4];
            int pos = index;
            Array.ConstrainedCopy(bs, pos, TZifnext, 0, 4);
            Encoding encoding = Encoding.GetEncoding(0);
            string header2 = encoding.GetString(TZifnext).Substring(0, 4);
            int nexttzpos = pos;
            int finalpos = -1;
            string finaltz = "";
            string footer = "";
            StringBuilder sb = new StringBuilder();
            StringBuilder js = new StringBuilder();
            js.Append("{");

            string tr = "";
            string ab = "";
            string of = "";

            long Y_filter_st = Convert.ToInt64(Properties.Settings.Default.stfilter);
            long Y_filter_en = Convert.ToInt64(Properties.Settings.Default.enfilter);

            int tzh_ttisgmtcnt_next = 0;
            int tzh_ttisstdcnt_next = 0;
            int tzh_leapcnt_next = 0;
            int tzh_timecnt_next = 0;
            int tzh_typecnt_next = 0;
            int tzh_charcnt_next = 0;

            if (header2.Contains("TZif") == false)
            {
                sb.AppendLine("2番目のZone infomation compiler で作成されたtzdataバイナリTZifはありません");
                sb.AppendLine();
            }
            else
            {
                byte[] v = new byte[] { bs[pos + 4] };
                string versionn = encoding.GetString(v);

                if (bs[pos + 4] == 0)
                {
                    sb.AppendLine("2番目のZone infomation compiler で作成されたtzdataバイナリTZif version NULL(1) はMUST BE IGNOREです");
                    sb.AppendLine();
                }
                else
                {

                    pos += 20;
                    tzh_ttisgmtcnt_next = BitConverter.ToInt32(Bigval32(bs, pos), 0);
                    tzh_ttisstdcnt_next = BitConverter.ToInt32(Bigval32(bs, pos + 4), 0);
                    tzh_leapcnt_next = BitConverter.ToInt32(Bigval32(bs, pos + 8), 0);
                    tzh_timecnt_next = BitConverter.ToInt32(Bigval32(bs, pos + 12), 0);
                    tzh_typecnt_next = BitConverter.ToInt32(Bigval32(bs, pos + 16), 0);
                    tzh_charcnt_next = BitConverter.ToInt32(Bigval32(bs, pos + 20), 0);


                    //C#  transition_timeが64bitのため
                    long[] transition_timesn = new long[tzh_timecnt_next];
                    int[] transition_typesn = new int[tzh_timecnt_next];

                    pos += 24;
                    if (tzh_timecnt_next != 0)
                    {
                        for (int i = 0; i < tzh_timecnt_next; i++)
                        {
                            transition_timesn[i] = BitConverter.ToInt64(Bigval64(bs, pos + 8 * i), 0);
                            transition_typesn[i] = bs[pos + tzh_timecnt_next * 8 + i];
                        }
                    }
                    else
                    {
                        transition_timesn = [];
                        transition_typesn = [];
                    }

                    pos += tzh_timecnt_next * 9;
                    int[] local_time_types_gmtn = new int[tzh_typecnt_next];
                    int[] local_time_types_isdstn = new int[tzh_typecnt_next];
                    int[] local_time_types_abbrn = new int[tzh_typecnt_next];

                    for (int i = 0; i < tzh_typecnt_next; i++)
                    {
                        local_time_types_gmtn[i] = BitConverter.ToInt32(Bigval32(bs, pos + i * 6), 0);
                        local_time_types_isdstn[i] = bs[pos + 4 + i * 6];
                        local_time_types_abbrn[i] = bs[pos + 5 + i * 6];
                    }

                    pos = pos + 6 * tzh_typecnt_next;
                    byte[] abbr2 = new byte[tzh_charcnt_next + 10];
                    Array.ConstrainedCopy(bs, pos, abbr2, 0, tzh_charcnt_next);


                    sb.AppendLine("2ndTZif transitions,gmtoffset,isdst,abbr");
                    pos = pos + tzh_charcnt_next;
                    if (tzh_leapcnt_next > 0)
                    {
                        pos = pos + tzh_leapcnt_next * 8;
                    }


                    if (tzh_ttisstdcnt_next > 0)
                    {
                        //isstd = struct.unpack(">%db" % ttisstdcnt fileobj.read(ttisstdcnt))
                        byte[] isstd = new byte[tzh_ttisstdcnt_next];
                        Array.ConstrainedCopy(bs, pos, isstd, 0, tzh_ttisstdcnt_next);
                        pos += tzh_ttisstdcnt_next;
                    }

                    if (tzh_ttisgmtcnt_next > 0)
                    {
                        //isgmt = struct.unpack(">%db" % ttisgmtcnt, fileobj.read(ttisgmtcnt))
                        byte[] isgmt = new byte[tzh_ttisgmtcnt_next];
                        Array.ConstrainedCopy(bs, pos, isgmt, 0, tzh_ttisgmtcnt_next);
                        pos += tzh_ttisgmtcnt_next;

                    }


                    js.Append(@"""Zone"":" + @"""TZST"",");
                    js.Append(@"""TransList"":" + @"[0],");
                    js.Append(@"""Offsets"":" + @"[1],");
                    js.Append(@"""Abbrs"":" + @"[3]");

                    bool filter = Properties.Settings.Default.usefiler;
                    int max = tzh_timecnt_next - 1;
                    string[][] transitions_next = new string[tzh_timecnt_next][];
                    if (tzh_timecnt_next == 0)
                    {
                        int type = 0;
                        string[][] transitions_next_zero = new string[1][];
                        transitions_next_zero[0] = new string[4];
                        transitions_next_zero[0][0] = "";
                        transitions_next_zero[0][1] = Convert.ToString(local_time_types_gmtn[type]);
                        transitions_next_zero[0][2] = Convert.ToString(local_time_types_isdstn[type]);

                        byte[] tmp2 = new byte[20];
                        Array.ConstrainedCopy(abbr2, local_time_types_abbrn[type], tmp2, 0, 10);
                        char[] charArray = ByteArrayToCharArray(tmp2, Encoding.UTF8);
                        transitions_next_zero[0][3] = TerminateAtNull(charArray);

                        tr = tr + transitions_next_zero[0][0] + @",";
                        of = of + Convert.ToString(Convert.ToDouble(local_time_types_gmtn[type]) / 3600) + @",";
                        ab = ab + @"""" + transitions_next_zero[0][3] + @""",";


                        sb.Append("null");
                        sb.Append(",");
                        sb.Append(transitions_next_zero[0][1]);
                        sb.Append(",");
                        sb.Append(Convert.ToString(Convert.ToDouble(local_time_types_gmtn[type]) / 3600));
                        sb.Append(",");
                        sb.AppendLine(transitions_next_zero[0][3]);
                    }

                    for (int i = 0; i < tzh_timecnt_next; i++)
                    {
                        int type = transition_typesn[i];
                        transitions_next[i] = new string[4];
                        transitions_next[i][0] = transition_timesn[i].ToString();
                        transitions_next[i][1] = Convert.ToString(local_time_types_gmtn[type]);
                        transitions_next[i][2] = Convert.ToString(local_time_types_isdstn[type]);

                        byte[] tmp2 = new byte[20];
                        Array.ConstrainedCopy(abbr2, local_time_types_abbrn[type], tmp2, 0, 10);
                        char[] charArray = ByteArrayToCharArray(tmp2, Encoding.UTF8);
                        transitions_next[i][3] = TerminateAtNull(charArray);

                        if (filter == false)
                        {
                            tr = tr + transitions_next[i][0] + @",";
                            of = of + Convert.ToString(Convert.ToDouble(local_time_types_gmtn[type]) / 3600) + @",";
                            ab = ab + @"""" + transitions_next[i][3] + @""",";
                        }
                        else
                        {
                            // DateTimeOffsetに変換
                            DateTimeOffset dateTimeWithOffset = DateTimeOffset.FromUnixTimeSeconds(transition_timesn[i]);

                            // オフセット付きのフォーマットに変換
                            string y = dateTimeWithOffset.ToString("yyyy");
                            int YY = Convert.ToInt32(y);
                            if (YY >= Y_filter_st && YY <= Y_filter_en)
                            {
                                tr = tr + transitions_next[i][0] + @",";
                                of = of + Convert.ToString(Convert.ToDouble(local_time_types_gmtn[type]) / 3600) + @",";
                                ab = ab + @"""" + transitions_next[i][3] + @""",";
                            }
                        }

                        if (true)
                        {
                            transitions_next[i][1] = Convert.ToString(Convert.ToDouble(local_time_types_gmtn[type]) / 3600);
                            double localTimeOffseth = Convert.ToDouble(local_time_types_gmtn[type]) / 3600;

                            long unixTimestamp = transition_timesn[i];


                            TimeSpan utcOffset = TimeSpan.FromHours(localTimeOffseth);
                            // TotalHours で符号を判定
                            double totalHours = utcOffset.TotalHours; // 全体の時間（小数部分を含む）

                            string sign = totalHours >= 0 ? "+" : "-";
                            // HH と MM を取得
                            int hours = (int)Math.Abs(utcOffset.Hours); // 絶対値を取った整数部の時間
                            int minutes = (int)Math.Abs(utcOffset.Minutes); // 絶対値を取った整数部の時間
                            int seconds = (int)Math.Abs(utcOffset.Seconds); // 絶対値を取った整数部の時間

                            // フォーマット
                            //string formattedOffset = $"{sign}{hours:00}:{totalMinutes:00.00}";
                            string formattedOffset = $"{sign}{hours:00}:{minutes:00}:{seconds:00}";

                            try
                            {
                                // HH:MM の形式に変換可能かどうかを判定,アフリカ初期のGMT HH:MM:SS込は .TOffsetが例外
                                if (utcOffset.TotalSeconds % 60 == 0)
                                {
                                    // HH:MM 形式が可能
                                    DateTimeOffset dateTimeWithOffset = DateTimeOffset
                                        .FromUnixTimeSeconds(unixTimestamp)
                                        .ToOffset(utcOffset);

                                    // 標準フォーマットで変換
                                    string formattedDateh = dateTimeWithOffset.ToString("yyyy-MM-ddTHH:mm:sszzz");
                                    transitions_next[i][0] = formattedDateh;
                                }
                                else
                                {
                                    // カスタムフォーマット
                                    DateTimeOffset originalTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).AddHours(localTimeOffseth);
                                    string formattedDate = $"{originalTime:yyyy-MM-ddTHH:mm:ss.fff} {formattedOffset}";
                                    transitions_next[i][0] = formattedDate;
                                }
                            }
                            catch (Exception ex)
                            {
                                sb.AppendLine(ex.ToString());
                                break;
                            }

                        }
                        if (filter == true)
                        {
                            // DateTimeOffsetに変換
                            DateTimeOffset dateTimeWithOffset = DateTimeOffset.FromUnixTimeSeconds(transition_timesn[i]);

                            // オフセット付きのフォーマットに変換
                            string yy = dateTimeWithOffset.ToString("yyyy");
                            int YYy = Convert.ToInt32(yy);
                            if (YYy >= Y_filter_st && YYy <= Y_filter_en)
                            {
                                sb.Append(transitions_next[i][0]);
                                sb.Append(",");
                                sb.Append(transitions_next[i][1]);
                                sb.Append(",");
                                sb.Append(transitions_next[i][2]);
                                sb.Append(",");
                                sb.AppendLine(transitions_next[i][3]);
                            }
                        }
                        else
                        {
                            sb.Append(transitions_next[i][0]);
                            sb.Append(",");
                            sb.Append(transitions_next[i][1]);
                            sb.Append(",");
                            sb.Append(transitions_next[i][2]);
                            sb.Append(",");
                            sb.AppendLine(transitions_next[i][3]);
                        }

                    }
                    if (tr == "")
                    {
                        if (tzh_timecnt_next > 0)
                        {
                            sb.Append(transitions_next[max][0]);
                            sb.Append(",");
                            sb.Append(transitions_next[max][1]);
                            sb.Append(",");
                            sb.Append(transitions_next[max][2]);
                            sb.Append(",");
                            sb.AppendLine(transitions_next[max][3]);
                            tr = tr + transition_timesn[max].ToString() + @",";
                            of = of + transitions_next[max][1] + @",";
                            ab = ab + @"""" + transitions_next[max][3] + @""",";
                            finaltz = Y_filter_st + "～" + Y_filter_en + "年期間内にはゾーン情報存在しませんが、から配列回避のため最後のゾーン情報を追加しています(から配列＝UTCになるため)";
                        }
                    }
                    if (filter)
                    {
                        finaltz = "カッティングしたゾーン情報になっています" + Y_filter_st + "～" + Y_filter_en + "年期間内のみ";

                    }

                    finalpos = pos;
                    int finalstring = bs.Length - pos;
                    byte[] foolerbyte = new byte[finalstring];
                    if (finalstring > 0)
                    {
                        Array.ConstrainedCopy(bs, pos, foolerbyte, 0, finalstring);
                        footer = encoding.GetString(foolerbyte);
                    }

                    sb.AppendLine();
                }
            }


            js.Append("}");

            string mkjson = js.ToString();
            mkjson = mkjson.Replace("TZST", tzst);
            mkjson = mkjson.Replace("[0]", "[" + tr + "]");
            mkjson = mkjson.Replace("[1]", "[" + of + "]");
            mkjson = mkjson.Replace("[3]", "[" + ab + "]");
            mkjson = mkjson.Replace(",]", "]");
            mkjson = mkjson.Replace(",]", "]");
            mkjson = mkjson.Replace(",]", "]");

            sb.Append("tzdata name:");
            sb.Append(tzdata);
            sb.AppendLine();
            sb.Append("filesize:");
            sb.Append(bs.Length.ToString());
            sb.AppendLine("byte ");
            sb.Append("footer position:");
            sb.Append(finalpos.ToString());
            sb.AppendLine();
            sb.Append("foorter string:");
            sb.Append(footer);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(mkjson);
            sb.AppendLine();
            Properties.Settings.Default.footerstring = footer.Replace("\n", "");

            try
            {
                // JSONパース
                TimeZoneData tzData = JsonSerializer.Deserialize<TimeZoneData>(mkjson);

                // TimeZoneTransitionsインスタンスを作成
                TimeZoneTransitions tzTransitions = new TimeZoneTransitions(
                    tzData.TransList,
                    tzData.Offsets,
                    tzData.Abbrs
                );
                string tester = Properties.Settings.Default.datetester;
                DateTime testDateTime;

                Properties.Settings.Default.TZJSON = mkjson;


                if (TZPASER.FastDateTimeParsing.TryParseFastDateTime(tester, out testDateTime) || TZPASER.RFC2822DateTimeParser.TryParseRFC2822DateTime(tester, out testDateTime))
                {
                    DateTimeOffset ddt = DateTime.SpecifyKind(testDateTime, testDateTime.Kind);
                    DateTimeOffset ddt_l = ddt.ToLocalTime();
                    DateTimeOffset ddt_u = ddt.ToUniversalTime();
                    string ddt_local_offset = ddt_l.Offset.ToString();
                    string localt = ddt_l.ToString();
                    string utct = ddt_u.ToString();

                    int lastTransitionIdx = tzTransitions.FindLastTransition(testDateTime);
                    int lastTransitionIdx_w = tzTransitions.FindLastTransition_w(testDateTime);

                    string rp = Properties.Settings.Default.useutch + ":" + Properties.Settings.Default.useutcm;
                    string format = Properties.Settings.Default.datetimeformat;//"yyyy/MM/dd HH:mm:ss'(GMT'zzz')'";

                    // タイムゾーンを取得（ローカルタイムゾーン）
                    TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.mstime);

                    string pattern = @"(%TZ|%z|%Z|%PO)";
                    string format_ms = Regex.Replace(format, pattern, match => "");

                    var o1 = tzi.GetUtcOffset(ddt_l);
                    string st = o1.ToString();

                    string rrp = Regex.Replace("+" + st, ":\\d\\d$", "");
                    string rp2 = Regex.Replace("+" + st, ":\\d\\d:\\d\\d$", "");
                    rrp = Regex.Replace(rrp, "\\+\\-", "-");
                    rp2 = Regex.Replace(rp2, "\\+\\-", "-");

                    string tmp = tzi.StandardName;
                    string tmp_l = localTimeZone.StandardName;


                    if (tzi.IsDaylightSavingTime((ddt_l)))
                    {
                        tmp = tzi.DaylightName;
                    }
                    if (localTimeZone.IsDaylightSavingTime(ddt_l))
                    {
                        tmp_l = localTimeZone.DaylightName;
                    }

                    string format_mstz = format_ms.Replace("zzz", rrp).Replace("zz", rp2).Replace("z", rp2).Replace("K", tmp);
                    format_ms = format_ms.Replace("K", rp).Replace("zzz", rp).Replace("zz", Properties.Settings.Default.useutch).Replace("z", Properties.Settings.Default.useutch);

                    string ms_utc = testDateTime.ToUniversalTime().AddHours(Properties.Settings.Default.useutcint).ToString(format_ms);
                    string ms_tz = TimeZoneInfo.ConvertTime(ddt_u, tzi).ToString(format_mstz);

                    string tznd = Properties.Settings.Default.noddatz;
                    string timeStrings = "-----";
                    if (TZPASER.nodaparser.CheckTimeZoneExists(tznd))
                    {
                        try
                        {
                            // 2. UTC時刻を指定したタイムゾーンの現地時間に変換
                            ZonedDateTime convertedTimes = TZPASER.nodaparser.ConvertToTimeZone(testDateTime, tznd);

                            var zonebcl = DateTimeZoneProviders.Bcl;
                            var zonetzdb = DateTimeZoneProviders.Tzdb;

                            var pattern_nd = Properties.Settings.Default.nodaformat;
                            DateTimeZone zone = DateTimeZoneProviders.Tzdb[tznd]; // Specify your timezone
                            ZonedDateTimePattern formatter = ZonedDateTimePattern.CreateWithInvariantCulture(pattern_nd, zonetzdb);

                            timeStrings = formatter.Format(convertedTimes);

                        }
                        catch (Exception ex)
                        {
                            timeStrings += "nodatimeエラー" + ex.ToString();
                        }
                    }
                    else
                    {

                        timeStrings += "nodatimeで未対応のタイムゾーンです";
                    }


                    bool use_zoneparse = Properties.Settings.Default.local_chager;

                    if ((!use_zoneparse && lastTransitionIdx >= 0) || (use_zoneparse && lastTransitionIdx_w >= 0))
                    {
                        if (use_zoneparse)
                        {
                            lastTransitionIdx = lastTransitionIdx_w;
                        }

                        string tran = tzData.TransList[lastTransitionIdx].ToString();
                        double uo = tzData.Offsets[lastTransitionIdx];
                        string uoff = TZPASER.TimeZoneOffsetParser.ToCustomFormat(uo, true).ToString();
                        string abb = tzData.Abbrs[lastTransitionIdx];
                        string iso8601tz = testDateTime.ToUniversalTime().AddHours(uo).ToString("yyyy-MM-dd'T'HH:mm:ss" + uoff + " " + abb);

                        sb.AppendLine(finaltz);
                        sb.AppendLine();
                        sb.AppendLine("日付パースのてすと");
                        sb.Append("入力: ");
                        sb.AppendLine(tester);
                        sb.Append("ローカルとUTCの時差(入力変換時): ");
                        sb.AppendLine(ddt_local_offset + tmp_l);
                        sb.Append("M$ local:"); //local utc tzdate
                        sb.AppendLine(localt);
                        sb.Append("M$ UTC master:"); //local utc tzdate
                        sb.AppendLine(ms_utc);
                        sb.Append("M$ timezone:"); //local utc tzdate
                        sb.AppendLine(ms_tz);
                        sb.Append("utc:"); // utc tzdate
                        sb.AppendLine(utct);
                        sb.Append("nodatime:"); // noda tzdate
                        sb.AppendLine(timeStrings);
                        sb.Append("tzdata iso8601+zone:"); // utc tzdate
                        sb.AppendLine(iso8601tz);
                        sb.AppendLine();
                        sb.AppendLine($"//tzdata_info\r\nLast transition index: {lastTransitionIdx}");
                        sb.AppendLine($"Timestamp: " + tran);
                        sb.AppendLine($"Offset: " + uoff);
                        sb.AppendLine($"Abbreviation: " + abb);
                    }
                    //みつからなかったらUTCのバイナリとみなす
                    else if (tzData.Offsets.Count >= 1 && tzData.Abbrs[0] != "")
                    {
                        // マッチさせたい文字群を指定
                        string patterntz = @"[dfFgGhtHKkmMsTyz:/]";
                        tzst = Regex.Replace(tzst, patterntz, match => "\\" + match.Value);

                        double uo = tzData.Offsets[0];
                        string uoff = TZPASER.TimeZoneOffsetParser.ToCustomFormat(uo, true).ToString();
                        string abb = tzData.Abbrs[0];
                        //string uoff = ToCustomFormat(uo, true).ToString();
                        abb = Regex.Replace(abb, patterntz, match => "\\" + match.Value);
                        string iso8601tz = testDateTime.ToUniversalTime().AddHours(uo).ToString("yyyy-MM-dd'T'HH:mm:ss" + uoff + " " + abb);

                        sb.AppendLine(finaltz);
                        sb.AppendLine();
                        sb.AppendLine("日付パースのてすと");
                        sb.Append("入力: ");
                        sb.AppendLine(tester);
                        sb.Append("ローカルとUTCの時差(入力変換時): ");
                        sb.AppendLine(ddt_local_offset + tmp_l);
                        sb.Append("M$ local:"); //local utc tzdate
                        sb.AppendLine(localt);
                        sb.Append("M$ UTC master:"); //local utc tzdate
                        sb.AppendLine(ms_utc);
                        sb.Append("M$ timezone:"); //local utc tzdate
                        sb.AppendLine(ms_tz);
                        sb.Append("utc:"); // utc tzdate
                        sb.AppendLine(utct);
                        sb.Append("nodatime:"); // noda tzdate
                        sb.AppendLine(timeStrings);
                        sb.Append("tzdata iso8601+zone:"); // utc tzdate
                        sb.AppendLine(iso8601tz);
                        sb.AppendLine();
                        sb.AppendLine($"//tzdata_info\r\nLast transition index: {lastTransitionIdx}");
                        sb.AppendLine($"Timestamp: null");
                        sb.AppendLine($"Offset: {tzData.Offsets[0]}");
                        sb.AppendLine($"Abbreviation: {tzData.Abbrs[0]}");
                    }
                    else
                    {
                        string iso8601tz = testDateTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ssUTC");

                        sb.AppendLine("期間内に偏移ファイルがみつかりませんでした、暫定でUTCになります");
                        sb.AppendLine("日付パースのてすと");
                        sb.Append("入力: ");
                        sb.AppendLine(tester);
                        sb.Append("ローカルとUTCの時差(入力変換時): ");
                        sb.AppendLine(ddt_local_offset + tmp_l);
                        sb.Append("M$ local:"); //local utc tzdate
                        sb.AppendLine(localt);
                        sb.Append("M$ UTC master:"); //local utc tzdate
                        sb.AppendLine(ms_utc);
                        sb.Append("M$ timezone:"); //local utc tzdate
                        sb.AppendLine(ms_tz);
                        sb.Append("utc:"); // utc tzdate
                        sb.AppendLine(utct);
                        sb.Append("nodatime:"); // noda tzdate
                        sb.AppendLine(timeStrings);
                        sb.Append("tzdata iso8601+zone:"); // utc tzdate
                        sb.AppendLine(iso8601tz);
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.AppendLine("日付てすとのパースに失敗しました、正しい日付を入力してください");
                    sb.AppendLine("入力例:");
                    sb.AppendLine("日付てすとのパースに失敗しました、正しい日付を入力してください");
                    sb.AppendLine("日付てすとのパースに失敗しました、正しい日付を入力してください");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.ToString());
            }

            return sb.ToString();

        }




        static string TerminateAtNull(char[] charArray)
        {
            int length = 0;
            while (length < charArray.Length && charArray[length] != '\0')
            {
                length++;
            }

            return new string(charArray, 0, length);
        }

        static char[] ByteArrayToCharArray(byte[] byteArray, Encoding encoding)
        {
            // byte配列を文字列に変換
            string str = encoding.GetString(byteArray);

            // 文字列をchar配列に変換
            return str.ToCharArray();
        }

        //エンディアン配列変換
        public static byte[] Bigval64(byte[] bs, int pos)
        {
            byte[] swapbin = new byte[8];
            Array.ConstrainedCopy(bs, pos, swapbin, 0, 8);
            Array.Reverse(swapbin);
            return swapbin;
        }

        public static byte[] Bigval32(byte[] bs, int pos)
        {
            byte[] swapbin = new byte[4];
            Array.ConstrainedCopy(bs, pos, swapbin, 0, 4);
            Array.Reverse(swapbin);
            return swapbin;
        }

        //https://chatgpt.com/share/6766ace0-fda8-800f-b9fe-783554d4c119 BinaryPrimitives代わり
        public static long ReverseEndianness64bit(long value)
        {
            ulong unsignedValue = (ulong)value;

            // エンディアンを反転
            unsignedValue = ((unsignedValue & 0x00000000000000FF) << 56) |
                            ((unsignedValue & 0x000000000000FF00) << 40) |
                            ((unsignedValue & 0x0000000000FF0000) << 24) |
                            ((unsignedValue & 0x00000000FF000000) << 8) |
                            ((unsignedValue & 0x000000FF00000000) >> 8) |
                            ((unsignedValue & 0x0000FF0000000000) >> 24) |
                            ((unsignedValue & 0x00FF000000000000) >> 40) |
                            ((unsignedValue & 0xFF00000000000000) >> 56);

            return (long)unsignedValue;
        }

        public static int ReverseEndianness32bit(int value)
        {

            uint unsignedValue = (uint)value;

            unsignedValue = ((unsignedValue & 0x000000FF) << 24) |
                   ((unsignedValue & 0x0000FF00) << 8) |
                   ((unsignedValue & 0x00FF0000) >> 8) |
                   ((unsignedValue & 0xFF000000) >> 24);

            return (int)unsignedValue;
        }




        //https://github.com/dotnet/corert/blob/master/src/System.Private.CoreLib/shared/System/TimeZoneInfo.Unix.cs m$財団のソース
        private readonly struct TZifHead
        {
            public const int Length = 44;

            public readonly uint Magic; // TZ_MAGIC "TZif"
            public readonly TZVersion Version; // 1 byte for a \0 or 2 or 3
            // public byte[15] Reserved; // reserved for future use
            public readonly uint IsGmtCount; // number of transition time flags
            public readonly uint IsStdCount; // number of transition time flags
            public readonly uint LeapCount; // number of leap seconds
            public readonly uint TimeCount; // number of transition times
            public readonly uint TypeCount; // number of local time types
            public readonly uint CharCount; // number of abbreviated characters

            public TZifHead(byte[] data, int index)
            {
                if (data == null || data.Length < Length)
                {
                    throw new ArgumentException("bad data", nameof(data));
                }

                Magic = (uint)TZif_ToInt32(data, index + 00);

                if (Magic != 0x545A6966)
                {
                    // 0x545A6966 = {0x54, 0x5A, 0x69, 0x66} = "TZif"
                    //throw new ArgumentException(SR.Argument_TimeZoneInfoBadTZif, nameof(data));
                }

                byte version = data[index + 04];
                Version =
                    version == '2' ? TZVersion.V2 :
                    version == '3' ? TZVersion.V3 :
                    TZVersion.V1;  // default/fallback to V1 to guard against future, unsupported version numbers

                // skip the 15 byte reserved field

                // don't use the BitConverter class which parses data
                // based on the Endianness of the machine architecture.
                // this data is expected to always be in "standard byte order",
                // regardless of the machine it is being processed on.

                IsGmtCount = (uint)TZif_ToInt32(data, index + 20);
                IsStdCount = (uint)TZif_ToInt32(data, index + 24);
                LeapCount = (uint)TZif_ToInt32(data, index + 28);
                TimeCount = (uint)TZif_ToInt32(data, index + 32);
                TypeCount = (uint)TZif_ToInt32(data, index + 36);
                CharCount = (uint)TZif_ToInt32(data, index + 40);
            }
        }

        private enum TZVersion : byte
        {
            V1 = 0,
            V2,
            V3,
            // when adding more versions, ensure all the logic using TZVersion is still correct
        }



        private readonly struct TZifType
        {
            public const int Length = 6;

            public readonly TimeSpan UtcOffset;
            public readonly bool IsDst;
            public readonly byte AbbreviationIndex;

            public TZifType(byte[] data, int index)
            {
                if (data == null || data.Length < index + Length)
                {
                    //throw new ArgumentException(SR.Argument_TimeZoneInfoInvalidTZif, nameof(data));
                }
                UtcOffset = new TimeSpan(0, 0, TZif_ToInt32(data, index + 00));
                IsDst = (data[index + 4] != 0);
                AbbreviationIndex = data[index + 5];
            }
        }

        // Converts an array of bytes into an int - always using standard byte order (Big Endian)
        // per TZif file standard
        private static int TZif_ToInt32(byte[] value, int startIndex)
            => BinaryPrimitives.ReadInt32BigEndian(value.AsSpan(startIndex));

        // Converts a span of bytes into an int - always using standard byte order (Big Endian)
        // per TZif file standard
        private static int TZif_ToInt32(ReadOnlySpan<byte> value)
            => BinaryPrimitives.ReadInt32BigEndian(value);

        // Converts an array of bytes into a long - always using standard byte order (Big Endian)
        // per TZif file standard
        private static long TZif_ToInt64(byte[] value, int startIndex)
            => BinaryPrimitives.ReadInt64BigEndian(value.AsSpan(startIndex));

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.usenoda = noda_timezone.Checked;
            if (noda_timezone.Checked)
            {
                panel6.Visible = true;
            }
            else
            {

                panel6.Visible = false;
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TZPASER.nodaparser.CheckTimeZoneExists(noda_timezone_items.Text))
            {
                Properties.Settings.Default.noddatz = noda_timezone_items.Text;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.nodaformat = noda_dateformat.Text;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://learn.microsoft.com/ja-jp/dotnet/standard/base-types/custom-date-and-time-format-strings";

            try
            {
                // デフォルトブラウザでURLを開く
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // 必須：デフォルトブラウザを使用する
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("エラーが発生しました: " + ex.Message);
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.nodaformat = noda_dateformat.Text;
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.lefttimeformat = elapst_left.Text;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://nodatime.org/1.3.x/userguide/text";

            try
            {
                // デフォルトブラウザでURLを開く
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // 必須：デフォルトブラウザを使用する
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("エラーが発生しました: " + ex.Message);
            }
        }

        private void checkBox6_CheckedChanged_1(object sender, EventArgs e)
        {

            //Properties.Settings.Default.shift_ambigous = checkBox6.Checked;
        }

        private void change_baseurl_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.change_baseurl = change_baseurl.Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var url = "https?://[ -_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$";

            var m = Regex.Match(baseurl_txt.Text, url);
            if (m.Success)
            {
                Properties.Settings.Default.alt_baseurl = m.Value;
            }
        }

        private void baseurl_keyval_TextChanged(object sender, EventArgs e)
        {
            string[] keys = baseurl_keyval.Text.Split(",");
            if (keys.Length == 10)
            {
                Properties.Settings.Default.alt_basekey = baseurl_keyval.Text;
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {


            OpenFileDialog ofd = new OpenFileDialog();

            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.lastfile);
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "jsonファイル(*.json)|*.json|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに選択されるものを指定する
            //2番目の「すべてのファイル」が選択されているようにする
            ofd.FilterIndex = 2;
            //タイトルを設定する
            ofd.Title = "開くjsonファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.alt_baseurl = ofd.FileName;
                Properties.Settings.Default.base_lastfile = Path.GetFileName(Path.GetDirectoryName(ofd.FileName));
                custom_url_path.Text = ofd.FileName;
            }
        }

        private void invaid_ambigous_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.noda_strict = invaid_ambigous.Checked;
        }

        private void LoadCultures()
        {
            // すべてのカルチャーを取得
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

            // コンボボックスにカルチャー名を追加
            foreach (CultureInfo culture in cultures)
            {
                if (culture.Name != "")
                {
                    localeBox.Items.Add(culture.Name + ";" + culture.DisplayName);
                }
                else
                {
                    localeBox.Items.Add("InvariantCulture");
                }
            }

        }

       
        private void localeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Regex rg = new Regex("^.*?:");
            Match m = rg.Match(localeBox.Text);
            if (m.Success)
            {
                Properties.Settings.Default.locale = m.Value.Replace(":","");
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Properties.Settings.Default.locale);
            }
            else
            {
                Properties.Settings.Default.locale = "InvariantCulture";
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            }
        }
    }
}
