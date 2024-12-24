using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Codeplex.Data;
using static neta.dtformat;
using System.Drawing;
using static System.Windows.Forms.DataFormats;
using System.Runtime.Intrinsics.X86;
using Microsoft.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TZPASER;

namespace neta
{
    public partial class NETA_TIMER : Form
    {
        public NETA_TIMER()
        {
            InitializeComponent();

        }



        private void button1_Click(object sender, EventArgs e)
        {
        }

        void SetFontRecursive(Control parent, System.Drawing.Font font)
        {
            foreach (Control control in parent.Controls)
            {
                control.Font = font;
                if (control.HasChildren) // 子コントロールがある場合
                {
                    SetFontRecursive(control, font);
                }
            }
        }


        string url = "https://script.google.com/macros/s/AKfycbxiN0USvNN0hQyO5b3Ep_oJy_qQxCRAlT4NU954QXKYZ6GrGyzsBnhi8RgMHLZHct-QJg/exec";
        string[] game = { "mirikr", "deresute", "mirsita", "shanimasu", "saisuta", "miricn", "proseka", "mobamasu", "sidem" };

        private void button2_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();

            wc.Encoding = Encoding.UTF8;


            var selecter = comboBox1.SelectedIndex;

            if (comboBox1.Text == "かすたむJS")
            {
                button3_Click(sender, e);
                return;
            }
            if (comboBox1.Text == "フリー入力")
            {
                return;
            }


            try
            {
                var url2 = url + "?game=all"; //+ game[selecter];
                string text = wc.DownloadString(url2);
                var obj = Codeplex.Data.DynamicJson.Parse(text);
                string path = "/data/" + game[selecter] + "/name," +
                    "/data/" + game[selecter] + "/start," +
                    "/data/" + game[selecter] + "/end";

                get_json_parse(url2, text, path, false);

                //ibemei.Text = obj.data[game[selecter]].name;
                //startbox.Text = obj.data[game[selecter]].start;
                //endbox.Text = obj.data[game[selecter]].end;


                Properties.Settings.Default.json = text;

            }
            catch (WebException exc)
            {
                endbox.Text = exc.Message;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.startbox.Text = Properties.Settings.Default.st;
            this.endbox.Text = Properties.Settings.Default.en;
            this.ibemei.Text = Properties.Settings.Default.ibe;
            this.comboBox1.Text = Properties.Settings.Default.goog;
            this.progressBar1.Width = Properties.Settings.Default.barlen;
            this.parcent.Left = Properties.Settings.Default.parcent;


            this.Font = Properties.Settings.Default.uifont;
            this.ForeColor = Properties.Settings.Default.uicolor;
            this.BackColor = Properties.Settings.Default.bgcolor;
            this.TransparencyKey = Properties.Settings.Default.colorkey;

            this.eventname.BackColor = Properties.Settings.Default.bgcolor;
            this.current.BackColor = Properties.Settings.Default.bgcolor;
            this.panel1.BackColor = Properties.Settings.Default.bgcolor;

            this.eventname.Font = Properties.Settings.Default.uifont;
            this.current.Font = Properties.Settings.Default.uifont;
            this.elapsed.Font = Properties.Settings.Default.uifont;

            this.eventname.ForeColor = Properties.Settings.Default.uicolor;
            this.current.ForeColor = Properties.Settings.Default.uicolor;
            panel1.ForeColor = Properties.Settings.Default.uicolor;

            this.panel2.Visible = Properties.Settings.Default.uihide;

            Properties.Settings.Default.system_tz = TimeZoneInfo.Local.Id;

            if (File.Exists(Properties.Settings.Default.lastimagefile)) { 
            try
            {
                // 画像を直接パネルの背景として設定
                panel1.BackgroundImage = Image.FromFile(Properties.Settings.Default.lastimagefile);
                panel1.BackgroundImageLayout = ImageLayout.Stretch; // 必要に応じて調整
                current.BackColor = this.BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            } }

            // using Microsoft.Win32;
            // システム時間変更時のイベントハンドラを登録
            SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);
        }

        // システム時間変更イベントハンドラ
        private void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("OS のタイムゾーンが変更されました。");

            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            //OLD
            //MessageBox.Show("現在時間：" + DateTime.Now.ToString() + localTimeZone.Id.ToString());

            // キャッシュをクリアして、最新のタイムゾーンを取得
            TimeZoneInfo.ClearCachedData();
            localTimeZone = TimeZoneInfo.Local;

            //Console.WriteLine($"現在のタイムゾーン: {localTimeZone.DisplayName}");
            // 変更後のシステム時間をコンソールへ出力
            Properties.Settings.Default.system_tz = localTimeZone.Id;
            //NEW
            //MessageBox.Show("現在時間：" + DateTime.Now.ToString()+ localTimeZone.ToString());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            Properties.Settings.Default.st = this.startbox.Text;
            Properties.Settings.Default.en = this.endbox.Text;
            Properties.Settings.Default.ibe = this.ibemei.Text;
            Properties.Settings.Default.goog = this.comboBox1.Text;

            Properties.Settings.Default.Save();
            // using Microsoft.Win32;
            // イベントハンドラを削除（削除しないとメモリリークが発生する）
            SystemEvents.TimeChanged -= new EventHandler(SystemEvents_TimeChanged);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            bool utc = Properties.Settings.Default.useutc;
            bool ms = Properties.Settings.Default.usems;
            bool tz = Properties.Settings.Default.usetz;
            string posix = Properties.Settings.Default.footerstring;
            string format = Properties.Settings.Default.datetimeformat;//"yyyy/MM/dd HH:mm:ss'(GMT'zzz')'";
            eventname.Text = ibemei.Text;
            DateTime dt = DateTime.Now;


            DateTime st;//= DateTime.Parse(startbox.Text);
            DateTime en;//= DateTime.Parse(endbox.Text);
            if (!tz)
            {
                string pattern = @"(%TZ|%z|%Z|%PO)";
                format = Regex.Replace(format, pattern, match => "");
                string patternn = @"(?<!\\)[!""#$'&%]"; // 「\K \z」を無視し、「Kz」のみマッチ !"#$'&%はダメ文字
                format = Regex.Replace(format, patternn, match => "");
            }
            else
            {
                string pattern = @"[dfFgGhtHKkmMsTyz:/]";
                string po = Regex.Replace(posix, pattern, match => "\\" + match.Value);
                format = Regex.Replace(format, "%PO", match => po);
            }

            if (TryParseDateTimeCutom(startbox.Text, out st))
            {

            }
            else
            {

                current.Text = "invalid date(ex: supoort format";
                elapsed.Text = "NOMARL: 2020/12/18 21:00  <-this convert Localtime OS denpending";
                left.Text = "ISO8601: 2020-12-18T21:00:00+09:00";
                duration.Text = "RFC2822: Sun, 10 Mar 2024 03:00:00 PDT)";

                return;
            }
            if (TryParseDateTimeCutom(endbox.Text, out en))
            {

            }
            else
            {

                current.Text = "invalid date(ex: supoort format";
                elapsed.Text = "NOMARL: 2020/12/18 21:00  <-this convert Localtime OS denpending";
                left.Text = "ISO8601: 2020-12-18T21:00:00+09:00";
                duration.Text = "RFC2822: Sun, 10 Mar 2024 03:00:00 PDT)";

                return;
            }
            if (utc)
            {
                string rp = Properties.Settings.Default.useutch + ":" + Properties.Settings.Default.useutcm;
                format = format.Replace("K", rp).Replace("zzz", rp).Replace("zz", Properties.Settings.Default.useutch).Replace("z", Properties.Settings.Default.useutch);
                current.Text = "現在時間:" + dt.AddHours(Properties.Settings.Default.useutcint).ToString(format);
                start.Text = "開始時間:" + st.AddHours(Properties.Settings.Default.useutcint).ToString(format);
                end.Text = "終了時間:" + en.AddHours(Properties.Settings.Default.useutcint).ToString(format);

            }
            else if (ms)
            {

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.mstime);

                DateTimeOffset ddt = DateTime.SpecifyKind(dt, dt.Kind);
                DateTimeOffset sst = DateTime.SpecifyKind(st, st.Kind);
                DateTimeOffset een = DateTime.SpecifyKind(en, en.Kind);

                string formatd = TZPASER.TimeZoneOffsetParser.getoffset(ddt, format, tzi);
                string formats = TZPASER.TimeZoneOffsetParser.getoffset(sst, format, tzi);
                string formate = TZPASER.TimeZoneOffsetParser.getoffset(een, format, tzi);


                current.Text = "現在時間:" + TimeZoneInfo.ConvertTime(ddt, tzi).ToString(formatd);
                start.Text = "開始時間:" + TimeZoneInfo.ConvertTime(sst, tzi).ToString(formats);
                end.Text = "終了時間:" + TimeZoneInfo.ConvertTime(een, tzi).ToString(formate);


            }
            else if (tz)
            {

                string mkjson = Properties.Settings.Default.TZJSON;
                // JSONパース
                if (mkjson != null && mkjson != "")
                {
                    try
                    {
                        TimeZoneData tzData = System.Text.Json.JsonSerializer.Deserialize<TimeZoneData>(mkjson);

                        // TimeZoneTransitionsインスタンスを作成
                        TimeZoneTransitions tzTransitions = new TimeZoneTransitions(
                            tzData.TransList,
                            tzData.Offsets,
                            tzData.Abbrs
                        );

                        string tzst = (Properties.Settings.Default.usetzdatabin);
                        int lastTransitionIdx = tzTransitions.FindLastTransition(dt);
                        int lastTransitionIdx_s = tzTransitions.FindLastTransition(st);
                        int lastTransitionIdx_d = tzTransitions.FindLastTransition(en);

                        if (lastTransitionIdx >= 0 && lastTransitionIdx_s >= 0 && lastTransitionIdx_d >= 0)
                        {
                            // マッチさせたい文字群を指定
                            string pattern = @"[dfFgGhtHKkmMsTyz:/]";
                            tzst = Regex.Replace(tzst, pattern, match => "\\" + match.Value);


                            double uo = tzData.Offsets[lastTransitionIdx];
                            string abb = tzData.Abbrs[lastTransitionIdx];
                            string uoff = TZPASER.TimeZoneOffsetParser.ToCustomFormat(uo, true).ToString();
                            abb = Regex.Replace(abb, pattern, match => "\\" + match.Value);

                            double uoc = tzData.Offsets[lastTransitionIdx_s];
                            string abbc = tzData.Abbrs[lastTransitionIdx_s];
                            string uoffc = TZPASER.TimeZoneOffsetParser.ToCustomFormat(uoc, true).ToString();
                            abbc = Regex.Replace(abbc, pattern, match => "\\" + match.Value);

                            double uoe = tzData.Offsets[lastTransitionIdx_d];
                            string abbe = tzData.Abbrs[lastTransitionIdx_d];
                            string uoffe = TZPASER.TimeZoneOffsetParser.ToCustomFormat(uoe, true).ToString();
                            abbe = Regex.Replace(abbe, pattern, match => "\\" + match.Value);

                            string formatc = format.Replace("%TZ", tzst).Replace("%Z", abb).Replace("%z", uoff).Replace("zzz", uoff);
                            string formats = format.Replace("%TZ", tzst).Replace("%Z", abbc).Replace("%z", uoffc).Replace("zzz", uoffc);
                            string formate = format.Replace("%TZ", tzst).Replace("%Z", abbe).Replace("%z", uoffe).Replace("zzz", uoffe);
                            string patternn = @"(?<!\\)[Kz!""#$'&%]"; // 「\K \z」を無視し、「Kz」のみマッチ !"#$'&%はダメ文字
                            formatc = Regex.Replace(formatc, patternn, match => "");
                            formats = Regex.Replace(formats, patternn, match => "");
                            formate = Regex.Replace(formate, patternn, match => "");

                            current.Text = "現在時間:" + dt.AddHours(uo).ToString(formatc);
                            start.Text = "開始時間:" + st.AddHours(uoc).ToString(formats);
                            end.Text = "終了時間:" + en.AddHours(uoe).ToString(formate);
                        }
                        else
                        {
                            //みつからなかったらUTCのバイナリとみなす
                            if (tzData.Offsets.Count >= 1 && tzData.Abbrs[0] != "")
                            {
                                // マッチさせたい文字群を指定
                                string pattern = @"[dfFgGhtHKkmMsTyz:/]";
                                tzst = Regex.Replace(tzst, pattern, match => "\\" + match.Value);

                                double uo = tzData.Offsets[0];
                                string abb = tzData.Abbrs[0];
                                string uoff = TZPASER.TimeZoneOffsetParser.ToCustomFormat(uo, true).ToString();
                                abb = Regex.Replace(abb, pattern, match => "\\" + match.Value);

                                format = format.Replace("%TZ", tzst).Replace("%Z", abb).Replace("%z", uoff).Replace("zzz", uoff);
                                string patternn = @"(?<!\\)[Kz!""#$'&%]"; // 「\K \z」を無視し、「Kz」のみマッチ !"#$'&%はダメ文字
                                format = Regex.Replace(format, patternn, match => "");
                                format = format.Replace("%TZ", tzst).Replace("%Z", abb).Replace("%z", uoff).Replace("zzz", uoff);
                                current.Text = "現在時間:" + dt.ToUniversalTime().AddHours(uo).ToString(format);
                                start.Text = "開始時間:" + st.ToUniversalTime().AddHours(uo).ToString(format);
                                end.Text = "終了時間:" + en.ToUniversalTime().AddHours(uo).ToString(format);
                            }
                            else
                            {
                                //UTC処理
                                string pattern = @"(%TZ|%z|%Z)";
                                format = Regex.Replace(format, pattern, match => "");

                                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("UTC");

                                DateTimeOffset ddt = DateTime.SpecifyKind(dt.ToUniversalTime(), DateTimeKind.Utc);
                                DateTimeOffset sst = DateTime.SpecifyKind(st.ToUniversalTime(), DateTimeKind.Utc);
                                DateTimeOffset een = DateTime.SpecifyKind(en.ToUniversalTime(), DateTimeKind.Utc);

                                string formatd = TZPASER.TimeZoneOffsetParser.getoffset(ddt, format, tzi);
                                string formats = TZPASER.TimeZoneOffsetParser.getoffset(sst, format, tzi);
                                string formate = TZPASER.TimeZoneOffsetParser.getoffset(een, format, tzi);


                                current.Text = "現在時間:" + TimeZoneInfo.ConvertTime(ddt, tzi).ToString(formatd);
                                start.Text = "開始時間:" + TimeZoneInfo.ConvertTime(sst, tzi).ToString(formats);
                                end.Text = "終了時間:" + TimeZoneInfo.ConvertTime(een, tzi).ToString(formate);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        current.Text = "例外発生";
                        start.Text = "えらー:tzdbを変換したJSONが空か異常があります";
                        end.Text = ex.ToString();
                    }

                }
            }
            else
            {
                current.Text = "現在時間:" + dt.ToLocalTime().ToString(format);
                start.Text = "開始時間:" + st.ToLocalTime().ToString(format);
                end.Text = "終了時間:" + en.ToLocalTime().ToString(format);
            }

            string L_format = Properties.Settings.Default.lefttimeformat;
            //st enはUTCになっているのでutcに必ずする
            dt = dt.ToUniversalTime();

            if (st < dt)
            {
                TimeSpan elapsedSpan = dt - st;
                elapsed.Text = "経過時間:" + TZPASER.TimeZoneOffsetParser.getleft(elapsedSpan, L_format);

                if (en > dt)
                {
                    TimeSpan leftSpan = en - dt;
                    left.Text = "残り時間:" + TZPASER.TimeZoneOffsetParser.getleft(leftSpan, L_format);
                }
                else
                {
                    left.Text = "残り時間:イベントはすでに終了しています";
                }

            }
            else
            {

                elapsed.Text = "経過時間:イベントがまだ開始されてません";
                left.Text = "残り時間:イベントがまだ開始されてません";
            }


            TimeSpan drationSpan = en - st;

            duration.Text = "イベ期間:" + TZPASER.TimeZoneOffsetParser.getleft(drationSpan, L_format);


            //msbarだとst=en 零割例外が起きない
            double bar = (dt - st).TotalSeconds / (en - st).TotalSeconds * 100;
            bar = Math.Round(bar, 2, MidpointRounding.AwayFromZero);
            if (bar > 100)
            {
                bar = 100;
            }
            if (bar < 0)
            {
                bar = 0;
            }
            parcent.Text = bar + "%";
            bar = Math.Floor(bar);
            progressBar1.Value = Convert.ToInt32(bar.ToString());

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            string path = @"data.ics";

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    string tmp = Properties.Resources.ical;
                    string ibemie = ibemei.Text;

                    DateTime st;//= DateTime.Parse(startbox.Text);
                    DateTime en;//= DateTime.Parse(endbox.Text);
                    string format = "yyyyMMdd'T'HHmmssZ";

                    string sst = "";
                    string sen = "";
                    string pend = "";
                    if (DateTime.TryParse(startbox.Text, out st))
                    {
                        sst = st.ToUniversalTime().ToString(format);
                    }
                    if (DateTime.TryParse(endbox.Text, out en))
                    {
                        sen = en.ToUniversalTime().ToString(format);
                    }
                    if (sen == "")
                    {
                        sen = sst;
                        pend = "（※終了時間未定です）";
                    }

                    string game = "[" + comboBox1.Text + "]";
                    if (ibemie.IndexOf(game) >= 0)
                    {
                        game = "";
                    }
                    if (comboBox1.Text == "かすたむJS")
                    {

                        game = "";
                    }

                    //= Regex.Replace("{置換対象文字列}", "{正規表現パターン}", "{置換パターン}");
                    tmp = Regex.Replace(tmp, "SUMMARY:うづき", "SUMMARY:" + game + ibemie + pend);
                    tmp = Regex.Replace(tmp, "20200423T150000Z", sst);
                    tmp = Regex.Replace(tmp, "20200424T150000Z", sen);
                    tmp = Regex.Replace(tmp, "\\\\r\\\\n", "\r\n");

                    byte[] info = new UTF8Encoding(true).GetBytes(tmp);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }

                //カレンダー起動
                var proc = new System.Diagnostics.Process();

                proc.StartInfo.FileName = path;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void 時刻設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form2 = new dtformat(this);
            form2.ShowDialog(this);
            form2.Dispose();
        }

        private void バージョンToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form2 = new VER();
            form2.ShowDialog();
            form2.Dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "フリー入力")
            {
                ibemei.Text = Properties.Settings.Default.freeevent;
                startbox.Text = Properties.Settings.Default.freest;
                endbox.Text = Properties.Settings.Default.freeend;
                return;
            }

            if (comboBox1.Text == "かすたむJS")
            {
                button3_Click(sender, e);
                return;
            }

            try
            {
                var url2 = url + "?game=all"; //+ game[selecter];
                string text = Properties.Settings.Default.json;
                var selecter = comboBox1.SelectedIndex;
                if (text == "")
                {
                    WebClient wc = new WebClient();

                    wc.Encoding = Encoding.UTF8;
                    text = wc.DownloadString(url2);
                    wc.Dispose();
                    Properties.Settings.Default.json = text;
                }

                var obj = Codeplex.Data.DynamicJson.Parse(text);
                string path = "/data/" + game[selecter] + "/name," +
                    "/data/" + game[selecter] + "/start," +
                    "/data/" + game[selecter] + "/end";

                get_json_parse(url2, text, path, false);

                //string text = Properties.Settings.Default.json;
                //var selecter = comboBox1.SelectedIndex;
                //var obj = Codeplex.Data.DynamicJson.Parse(text);

                //if (text == "") { 
                //WebClient wc = new WebClient();

                //wc.Encoding = Encoding.UTF8;
                //    var url2 = url + "?game=all"; //+ game[selecter];
                //text = wc.DownloadString(url2);
                //wc.Dispose();
                //Properties.Settings.Default.json = text;
                //}



                //ibemei.Text = obj.data.name;
                //startbox.Text = obj.data.start;
                //endbox.Text = obj.data.end;



            }
            catch (WebException exc)
            {
                endbox.Text = exc.Message;
            }
        }

        private void get_json_parse(string url, string text, string parseop, bool useweb_local)
        {

            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            DateTime dt = DateTime.Now;
            var errorPath = "";
            var urlrg = "https?://[ -_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$";

            var m = Regex.Match(url, urlrg);
            if (useweb_local)
            {
                if (m.Success)
                {
                    try
                    {
                        text = wc.DownloadString(url);
                    }
                    catch (WebException exc)
                    {
                        MessageBox.Show(exc.Message);
                        return;
                    }
                }
                else
                {
                    var path = url;
                    if (File.Exists(path))
                    {
                        StreamReader sr = new StreamReader(path, Encoding.GetEncoding("UTF-8"));
                        text = sr.ReadToEnd();
                        sr.Close();
                    }
                    else
                    {
                        MessageBox.Show(path + "のファイルは存在しません");
                        return;
                    }

                }
            }

            try
            {
                var obj = Codeplex.Data.DynamicJson.Parse(text);
                var objOriginal = obj;
                string[] parsePaths = parseop.Split(',');
                string[] getValues = { "", "", "" };
                bool end = false;

                Regex regex = new Regex("\\[(\\d+)\\]");

                if (string.IsNullOrWhiteSpace(text) || text == "[]" || text == "{}")
                {
                    MessageBox.Show("JSONが空または不正です。APIのURLを確認してください。");
                    return;
                }

                for (var k = 0; k < parsePaths.Length; k++)
                //for (var k = 0; k < 1; k++)
                {
                    // 現在のオブジェクトを元のオブジェクトにリセット
                    obj = objOriginal;
                    string[] pathSegments = parsePaths[k].Trim('/').Split('/');
                    errorPath = parsePaths[k];
                    end = false;

                    // パスをたどるループ
                    for (var i = 0; i < pathSegments.Length; i++)
                    {
                        try
                        {
                            // 配列の処理
                            while (obj.IsArray)
                            {
                                // 配列の最初の要素がない場合は処理をスキップ
                                if (!obj.IsDefined(0))
                                {
                                    obj = null;
                                    break;
                                }
                                Match match = regex.Match(pathSegments[i]);
                                while (match.Success)
                                {
                                    string matchedNumber = match.Value;
                                    string p = matchedNumber.ToString().Replace("[", "").Replace("]", ""); // 2番目の文字を取り出す
                                    int n = Convert.ToInt32(p);
                                    obj = obj[n];
                                    match = match.NextMatch();

                                }
                                if (i == pathSegments.Length - 1)
                                {

                                    getValues[k] = obj.ToString();
                                    end = true;
                                    break;
                                }
                                obj = obj[0];
                            }
                            if (end)
                            {
                                break;
                            }

                            // オブジェクトの処理
                            if (obj != null && obj.IsObject)
                            {
                                Match matcho = regex.Match(pathSegments[i]);
                                if (matcho.Success == false)
                                {
                                    // 途中の要素の場合
                                    obj = obj[pathSegments[i].ToString()];
                                    // 最後の要素の場合
                                    if (i == pathSegments.Length - 1)
                                    {
                                        getValues[k] = obj.ToString();
                                        break;
                                    }

                                }

                                if (matcho.Success)
                                {
                                    string sepa_seg = regex.Replace(pathSegments[i], "");
                                    obj = obj[sepa_seg];
                                    string matchedNumber = matcho.Value;
                                    string po = matchedNumber.ToString().Replace("[", "").Replace("]", ""); // 2番目の文字を取り出す
                                    int no = Convert.ToInt32(po);
                                    obj = obj[no];
                                    i++;
                                    matcho = matcho.NextMatch();
                                    while (matcho.Success)
                                    {
                                        matchedNumber = matcho.Value;
                                        string p = matchedNumber.ToString().Replace("[", "").Replace("]", ""); // 2番目の文字を取り出す
                                        int n = Convert.ToInt32(p);
                                        obj = obj[n];
                                        matcho = matcho.NextMatch();

                                    }
                                    i--;
                                    if (obj is DynamicJson) // または productDetails.GetType() == typeof(DynamicJson)
                                    {
                                        //Console.WriteLine("details はオブジェクトです");
                                    }
                                    else
                                    {
                                        //Console.WriteLine("details はオブジェクトではありません"); 
                                        if (i == pathSegments.Length - 1)
                                        {
                                            getValues[k] = obj.ToString();
                                            end = true;
                                            break;
                                        }
                                    }
                                }
                                if (end)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                // オブジェクトが見つからない場合
                                MessageBox.Show("objがみつかりません");
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            // パスのどこかで例外が発生した場合
                            MessageBox.Show($"{ex.Message} エラー場所:'{errorPath}' {pathSegments[i]} {obj}");
                            break;
                        }
                    }
                }

                // 取得した値を設定
                ibemei.Text = getValues[0] ?? "";
                startbox.Text = getValues[1] ?? "";
                endbox.Text = getValues[2] ?? "";
                //comboBox1.Text = "かすたむJS";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} エラー場所:'{errorPath}'");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            DateTime dt = DateTime.Now;
            string url = Properties.Settings.Default.api.ToString();
            string parseop = Properties.Settings.Default.parse;
            string text = "";
            var errorPath = "";
            var urlrg = "https?://[ -_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$";

            var m = Regex.Match(url, urlrg);
            if (m.Success)
            {
                try
                {
                    text = wc.DownloadString(url);
                }
                catch (WebException exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }
            }
            else
            {
                var path = Properties.Settings.Default.api.ToString();
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path, Encoding.GetEncoding("UTF-8"));
                    text = sr.ReadToEnd();
                    sr.Close();
                }
                else
                {
                    MessageBox.Show(path + "のファイルは存在しません");
                    return;
                }

            }

            try
            {
                var obj = Codeplex.Data.DynamicJson.Parse(text);
                var objOriginal = obj;
                string[] parsePaths = parseop.Split(',');
                string[] getValues = { "", "", "" };
                bool end = false;

                Regex regex = new Regex("\\[(\\d+)\\]");

                if (string.IsNullOrWhiteSpace(text) || text == "[]" || text == "{}")
                {
                    MessageBox.Show("JSONが空または不正です。APIのURLを確認してください。");
                    return;
                }

                for (var k = 0; k < parsePaths.Length; k++)
                //for (var k = 0; k < 1; k++)
                {
                    // 現在のオブジェクトを元のオブジェクトにリセット
                    obj = objOriginal;
                    string[] pathSegments = parsePaths[k].Trim('/').Split('/');
                    errorPath = parsePaths[k];
                    end = false;

                    // パスをたどるループ
                    for (var i = 0; i < pathSegments.Length; i++)
                    {
                        try
                        {
                            // 配列の処理
                            while (obj.IsArray)
                            {
                                // 配列の最初の要素がない場合は処理をスキップ
                                if (!obj.IsDefined(0))
                                {
                                    obj = null;
                                    break;
                                }
                                Match match = regex.Match(pathSegments[i]);
                                while (match.Success)
                                {
                                    string matchedNumber = match.Value;
                                    string p = matchedNumber.ToString().Replace("[", "").Replace("]", ""); // 2番目の文字を取り出す
                                    int n = Convert.ToInt32(p);
                                    obj = obj[n];
                                    match = match.NextMatch();

                                }
                                if (i == pathSegments.Length - 1)
                                {

                                    getValues[k] = obj.ToString();
                                    end = true;
                                    break;
                                }
                                obj = obj[0];
                            }
                            if (end)
                            {
                                break;
                            }

                            // オブジェクトの処理
                            if (obj != null && obj.IsObject)
                            {
                                Match matcho = regex.Match(pathSegments[i]);
                                if (matcho.Success == false)
                                {
                                    // 途中の要素の場合
                                    obj = obj[pathSegments[i].ToString()];
                                    // 最後の要素の場合
                                    if (i == pathSegments.Length - 1)
                                    {
                                        getValues[k] = obj.ToString();
                                        break;
                                    }

                                }

                                if (matcho.Success)
                                {
                                    string sepa_seg = regex.Replace(pathSegments[i], "");
                                    obj = obj[sepa_seg];
                                    string matchedNumber = matcho.Value;
                                    string po = matchedNumber.ToString().Replace("[", "").Replace("]", ""); // 2番目の文字を取り出す
                                    int no = Convert.ToInt32(po);
                                    obj = obj[no];
                                    i++;
                                    matcho = matcho.NextMatch();
                                    while (matcho.Success)
                                    {
                                        matchedNumber = matcho.Value;
                                        string p = matchedNumber.ToString().Replace("[", "").Replace("]", ""); // 2番目の文字を取り出す
                                        int n = Convert.ToInt32(p);
                                        obj = obj[n];
                                        matcho = matcho.NextMatch();

                                    }
                                    i--;
                                    if (obj is DynamicJson) // または productDetails.GetType() == typeof(DynamicJson)
                                    {
                                        //Console.WriteLine("details はオブジェクトです");
                                    }
                                    else
                                    {
                                        //Console.WriteLine("details はオブジェクトではありません"); 
                                        if (i == pathSegments.Length - 1)
                                        {
                                            getValues[k] = obj.ToString();
                                            end = true;
                                            break;
                                        }
                                    }
                                }
                                if (end)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                // オブジェクトが見つからない場合
                                MessageBox.Show("objがみつかりません");
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            // パスのどこかで例外が発生した場合
                            MessageBox.Show($"{ex.Message} エラー場所:'{errorPath}' {pathSegments[i]} {obj}");
                            break;
                        }
                    }
                }

                comboBox1.Text = "かすたむJS";
                // 取得した値を設定
                ibemei.Text = getValues[0] ?? "";
                startbox.Text = getValues[1] ?? "";
                endbox.Text = getValues[2] ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} エラー場所:'{errorPath}'");
            }
        }

        private void wEBたいまーToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime st;
            DateTime en;
            string sst = ",";
            string sen = ",";
            string format = "yyyy-MM-dd'T'HH:mm:ssZ";
            if (DateTime.TryParse(startbox.Text, out st))
            {
                sst = st.ToUniversalTime().ToString(format);
                sst = Uri.EscapeDataString(sst) + ",";
            }
            if (DateTime.TryParse(endbox.Text, out en))
            {
                sen = en.ToUniversalTime().ToString(format);
                sen = Uri.EscapeDataString(sen) + ",";
            }
            string gamename = Uri.EscapeDataString(ibemei.Text) + ",";
            string url = "http://sokudon.s17.xrea.com/neta/imm.html#";// NAME,START,END,OS,";
            url = url + gamename + sst + sen + "OS,";
            System.Diagnostics.Process.Start(url);

        }

        private void ぱいそんたいまーToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://raw.githubusercontent.com/sokudon/deresute/master/%E3%81%B1%E3%81%84%E3%81%9D%E3%82%93%E3%81%AE%E3%81%9F%E3%81%84%E3%81%BE%E3%83%BC.py";
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string text = wc.DownloadString(url);
            //ibe = 'ぷろせか'
            //s = '2020-12-10T14:00:00+08:00'
            //ss = '2020-12-18T12:00:00Z'
            DateTime st;
            DateTime en;
            string sst = ",";
            string sen = ",";
            string format = "yyyy-MM-dd'T'HH:mm:ssZ";
            if (DateTime.TryParse(startbox.Text, out st))
            {
                sst = st.ToUniversalTime().ToString(format);
            }
            if (DateTime.TryParse(endbox.Text, out en))
            {
                sen = en.ToUniversalTime().ToString(format);
            }
            text = text.Replace("ぷろせか", ibemei.Text)
                .Replace("2020-12-10T14:00:00+08:00", sst)
                .Replace("2020-12-18T12:00:00Z", sen);

            string path = @"neta_timer.py";

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(text);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }

                var proc = new System.Diagnostics.Process();

                proc.StartInfo.FileName = path;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void oBSタイマーToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void wEBせかいどけいToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //http://sokudon.s17.xrea.com/sekai-dere.html#%5B%E3%83%9F%E3%83%AA%E3%82%B7%E3%82%BF%5D%EC%95%84%EC%9D%B4%EB%8F%8C%EB%A7%88%EC%8A%A4%ED%84%B0%20%EB%B0%80%EB%A6%AC%EC%96%B8%20%EB%9D%BC%EC%9D%B4%EB%B8%8C!%20%EC%8B%9C%EC%96%B4%ED%84%B0%20%EB%8D%B0%EC%9D%B4%EC%A6%88%EC%9B%94%EC%9A%94%EC%9D%BC%20%ED%81%AC%EB%A6%BC%20%EC%86%8C%EB%8B%A4,2021-02-19T15%3A00%3A00%2B09%3A00,2021-02-26T21%3A00%3A00%2B09%3A00,M$E6,KST308
            DateTime st;
            DateTime en;
            string sst = ",";
            string sen = ",";
            string format = "yyyy-MM-dd'T'HH:mm:ssZ";
            if (DateTime.TryParse(startbox.Text, out st))
            {
                sst = st.ToUniversalTime().ToString(format);
                sst = Uri.EscapeDataString(sst) + ",";
            }
            if (DateTime.TryParse(endbox.Text, out en))
            {
                sen = en.ToUniversalTime().ToString(format);
                sen = Uri.EscapeDataString(sen) + ",";
            }
            string gamename = Uri.EscapeDataString(ibemei.Text) + ",";
            string url = "http://sokudon.s17.xrea.com/sekai-dere.html#";
            url = url + gamename + sst + sen;
            string ms = Properties.Settings.Default.useutczone;
            Match m = Regex.Match(ms, "M\\$.+$");
            if (m.Success)
            {
                url += m.Value;
            }

            System.Diagnostics.Process.Start(url);

        }

        private void cmpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void netaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var form5 = new ZIC();
            form5.ShowDialog(this);
            form5.Dispose();
        }

        private void luascriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://raw.githubusercontent.com/sokudon/deresute/master/OBSdere_extend.lua";
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string text = wc.DownloadString(url);
            DateTime st;
            DateTime en;
            string sst = ",";
            string sen = ",";
            string format = "yyyy-MM-dd'T'HH:mm:ssZ";
            if (DateTime.TryParse(startbox.Text, out st))
            {
                sst = st.ToUniversalTime().ToString(format);
            }
            if (DateTime.TryParse(endbox.Text, out en))
            {
                sen = en.ToUniversalTime().ToString(format);
            }

            text = text.Replace("でれすて", ibemei.Text)
                .Replace("2020-04-30T12:00:00+09:00", sst)
                .Replace("2020-05-07T21:00:00+09:00", sen)
                .Replace("%H:%m:%s", "%d %hh:%mm:%ss")
                .Replace("%Y/%m/%d %H:%M:%S", "%Y-%m-%d(%a)%H:%M:%S(GMT%z)")
                .Replace("%T%n経過時間%K%n残り時間%L%nイベント時間%I%n現地時間%N%n日本時間%JST%n達成率%P%nS %S%nE %E%n%nSJ %SJ%nEJ %EJ%n%nSU %SU%nEU %EU", "日本時間%JST%n経過時間%K%n残り時間%L%nイベント時間%I%n%T%P％%n%Q")
                .Replace("bar\", 1", "bar\",2");

            string path = @"obs_neta_timer.lua";

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(text);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void pythonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String url = "https://raw.githubusercontent.com/sokudon/gakumasu/master/date-time_with_tzinfo_gaku.py";
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string text = wc.DownloadString(url);
            DateTime st;
            DateTime en;
            string sst = ",";
            string sen = ",";
            string format = "yyyy-MM-dd'T'HH:mm:ssZ";
            if (DateTime.TryParse(startbox.Text, out st))
            {
                sst = st.ToUniversalTime().ToString(format);
            }
            if (DateTime.TryParse(endbox.Text, out en))
            {
                sen = en.ToUniversalTime().ToString(format);
            }
            //ibe = '星雲の窓辺'
            //st = '2024-04-30T17:00:00+09:00'
            //en = '2024-05-08T22:00:00+09:00'
            text = text.Replace("星雲の窓辺", ibemei.Text)
                .Replace("2024-04-30T17:00:00+09:00", sst)
                .Replace("2024-05-08T22:00:00+09:00", sen);

            string path = @"obs_neta_timer.py";

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(text);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void 下パネルを隠すToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            Properties.Settings.Default.uihide = false;
        }

        private void 下パネルを表示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            Properties.Settings.Default.uihide = true;
        }

        private void クロマキー青_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.Blue;
            this.eventname.BackColor = Color.Blue;
            this.current.BackColor = Color.Blue;
            panel1.BackColor = Color.Blue;
            Properties.Settings.Default.bgcolor = Color.Blue;
        }

        private void クロマキー赤_Click(object sender, EventArgs e)
        {

            this.BackColor = Color.Red;
            this.eventname.BackColor = Color.Red;
            this.current.BackColor = Color.Red;
            panel1.BackColor = Color.Red;
            Properties.Settings.Default.bgcolor = Color.Red;
        }

        private void クロマキー緑_Click(object sender, EventArgs e)
        {

            this.BackColor = Color.Green;
            this.eventname.BackColor = Color.Green;
            this.current.BackColor = Color.Green;
            panel1.BackColor = Color.Green;
            Properties.Settings.Default.bgcolor = Color.Green;
        }


        private void めにゅーの色に戻すToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.BackColor = this.menuStrip1.BackColor;
            this.eventname.BackColor = this.menuStrip1.BackColor;
            this.current.BackColor = this.menuStrip1.BackColor;
            panel1.BackColor = this.menuStrip1.BackColor;
            Properties.Settings.Default.bgcolor = this.menuStrip1.BackColor;
        }

        private void カラーキー今のメニューToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TransparencyKey = this.BackColor;
            Properties.Settings.Default.colorkey = this.TransparencyKey;
        }

        private void カラーキーなしToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TransparencyKey = Color.Empty;
            Properties.Settings.Default.colorkey= this.TransparencyKey;
        }

        private void parcent_Click(object sender, EventArgs e)
        {

        }

        private void 文字白ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ForeColor = Color.White;
            this.eventname.ForeColor = Color.White;
            this.current.ForeColor = Color.White;
            panel1.ForeColor = Color.White;
            Properties.Settings.Default.uicolor = Color.White;
        }

        private void 文字黒ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.ForeColor = Color.Black;
            this.eventname.ForeColor = Color.Black;
            this.current.ForeColor = Color.Black;
            panel1.ForeColor = Color.Black;
            Properties.Settings.Default.uicolor = Color.Black;
        }

        private void フォントToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FontDialog fd = new FontDialog();

            //ユーザーが選択できるポイントサイズの最大値を設定する
            fd.MaxSize = 15;
            fd.MinSize = 10;
            //存在しないフォントやスタイルをユーザーが選択すると
            //エラーメッセージを表示する
            fd.FontMustExist = true;
            //横書きフォントだけを表示する
            fd.AllowVerticalFonts = false;
            //色を選択できるようにする
            fd.ShowColor = true;
            //取り消し線、下線、テキストの色などのオプションを指定可能にする
            //デフォルトがTrueのため必要はない
            fd.ShowEffects = true;
            //固定ピッチフォント以外も表示する
            //デフォルトがFalseのため必要はない
            fd.FixedPitchOnly = false;
            //ベクタ フォントを選択できるようにする
            //デフォルトがTrueのため必要はない
            fd.AllowVectorFonts = true;

            //ダイアログを表示する
            if (fd.ShowDialog() != DialogResult.Cancel)
            {
                //TextBox1のフォントと色を変える
                this.Font = fd.Font;
                this.eventname.Font = fd.Font;
                this.current.Font = fd.Font;
                this.elapsed.Font = fd.Font;
                this.ForeColor = fd.Color;

                this.eventname.ForeColor = fd.Color;
                this.current.ForeColor = fd.Color;
                panel1.ForeColor = fd.Color;


                Properties.Settings.Default.uifont = fd.Font;
                Properties.Settings.Default.uicolor = fd.Color;
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            progressBar1.BackColor = Color.White;
        }

        private void ibemei_TextChanged(object sender, EventArgs e)
        {
            string freeinput = comboBox1.Text;
            if (freeinput == "フリー入力")
            {
                Properties.Settings.Default.freeevent = ibemei.Text;
            }
        }

        private void startbox_TextChanged(object sender, EventArgs e)
        {
            string freeinput = comboBox1.Text;
            if (freeinput == "フリー入力")
            {
                String input = startbox.Text;
                if (TryParseDateTimeCutom(input, out DateTime dt))
                {
                    Properties.Settings.Default.freest = startbox.Text;
                }
            }
        }

        private void endbox_TextChanged(object sender, EventArgs e)
        {
            string freeinput = comboBox1.Text;
            if (freeinput == "フリー入力")
            {
                String input = endbox.Text;
                if (TryParseDateTimeCutom(input, out DateTime dt))
                {

                    Properties.Settings.Default.freeend = endbox.Text;
                }
            }
        }

        public static bool TryParseDateTimeCutom(string input, out DateTime dt)
        {
            dt = DateTime.MinValue;
            if (TZPASER.FastDateTimeParsing.TryParseFastDateTime(input, out dt)
                || TZPASER.RFC2822DateTimeParser.TryParseRFC2822DateTime(input, out dt)
                || TZPASER.RFC2822DateTimeParser.YMDHMZ_to_ISO(input, out dt))
            {
                return true;
            }
            return false;
        }

        private void 画像ToolStripMenuItem_Click(object sender, EventArgs e)
        {



            OpenFileDialog ofd = new OpenFileDialog();

            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = Properties.Settings.Default.lastfile;
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "pngファイル(*.png)|*.png|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに選択されるものを指定する
            //2番目の「すべてのファイル」が選択されているようにする
            ofd.FilterIndex = 2;
            //タイトルを設定する
            ofd.Title = "開くpngファイルを選択してください";
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


                panel1.BackgroundImage = null;
                panel2.BackgroundImage = null;


                string s = ofd.FileName;
                try
                {
                    // 画像を直接パネルの背景として設定
                    panel1.BackgroundImage = Image.FromFile(s);
                    panel1.BackgroundImageLayout = ImageLayout.Stretch; // 必要に応じて調整
                    current.BackColor = this.BackColor;

                    Properties.Settings.Default.lastimagefile = s;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }


        }

        private void 画像なしToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = null;          // 背景画像を削除
            panel1.BackColor = this.BackColor;
            panel1.Invalidate();                    // 再描画をリクエスト
            Properties.Settings.Default.lastimagefile = "";
        }
    }



    //public class CustomTransparentPanel : Form
    //{
    //    private Panel panel1;
    //    private MenuStrip mainMenu;

    //    public CustomTransparentPanel()
    //    {
    //        InitializeComponent();
    //    }

    //    private void InitializeComponent()
    //    {
    //        // フォームの設定
    //        this.Size = new Size(800, 600);
    //        this.Text = "透過パネルの例";

    //        // メインメニューの作成
    //        mainMenu = new MenuStrip();
    //        mainMenu.Text = "メインメニュー";

    //        // パネル1の作成（透過）
    //        panel1 = new Panel();
    //        panel1.Location = new Point(50, 100);
    //        panel1.Size = new Size(300, 200);
    //        panel1.BackColor = Color.FromArgb(100, Color.Green); // 透過設定

    //        // メニューの作成（透過なし）
    //        MenuStrip menuStrip = new MenuStrip();
    //        menuStrip.Items.Add(new ToolStripMenuItem("ファイル"));
    //        menuStrip.Items.Add(new ToolStripMenuItem("編集"));

    //        // コントロールの追加
    //        this.Controls.Add(panel1);
    //        this.Controls.Add(menuStrip);
    //        this.MainMenuStrip = menuStrip;
    //    }

    //    // パネル1の透過メソッド
    //    private void SetPanelTransparency()
    //    {
    //        // アルファ値を調整して透過度を設定
    //        panel1.BackColor = Color.FromArgb(100, Color.Blue); // 透過率40%
    //    }

    //    // 透過度を動的に変更するメソッド
    //    public void AdjustTransparency(int alphaValue)
    //    {
    //        // alphaValue: 0(完全透過)から255(不透明)の間の値
    //        panel1.BackColor = Color.FromArgb(alphaValue, Color.Green);
    //    }
    //}

}

