using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;
using Codeplex.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static neta.dtformat;
using System.Web;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;

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

        string url = "https://script.google.com/macros/s/AKfycbxiN0USvNN0hQyO5b3Ep_oJy_qQxCRAlT4NU954QXKYZ6GrGyzsBnhi8RgMHLZHct-QJg/exec";
        string[] game = { "shanimasu", "deresute", "mirsita", "proseka", "saisuta", "mirikr", "miricn", "sidem", "mobamasu" };

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


            try
            {
                var url2 = url + "?game=" + game[selecter];
                string text = wc.DownloadString(url2);
                var obj = Codeplex.Data.DynamicJson.Parse(text);



                ibemei.Text = obj.data.name;
                startbox.Text = obj.data.start;
                endbox.Text = obj.data.end;


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
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            Properties.Settings.Default.st = this.startbox.Text;
            Properties.Settings.Default.en = this.endbox.Text;
            Properties.Settings.Default.ibe = this.ibemei.Text;
            Properties.Settings.Default.goog = this.comboBox1.Text;

            Properties.Settings.Default.Save();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            bool utc = Properties.Settings.Default.useutc;
            bool ms = Properties.Settings.Default.usems;
            bool tz = Properties.Settings.Default.usetz;
            string format = Properties.Settings.Default.datetimeformat;//"yyyy/MM/dd HH:mm:ss'(GMT'zzz')'";
            eventname.Text = ibemei.Text;
            DateTime dt = DateTime.Now;


            DateTime st;//= DateTime.Parse(startbox.Text);
            DateTime en;//= DateTime.Parse(endbox.Text);
            if (!tz)
            {
                string pattern = @"(%TZ|%z|%Z)";
                format = Regex.Replace(format, pattern, match => "");
            }

            if (DateTime.TryParse(startbox.Text, out st))
            {

            }
            else
            {

                start.Text = "invalid date(ex: 2020/12/18 21:00 or 2020-12-18T21:00:00+09:00)";
                elapsed.Text = "--";
                left.Text = "--";
                duration.Text = "--";

                return;
            }
            if (DateTime.TryParse(endbox.Text, out en))
            {

            }
            else
            {

                end.Text = "invalid date(ex: 2020/12/18 21:00 or 2020-12-18T21:00:00+09:00)";
                elapsed.Text = "--";

                left.Text = "--";
                duration.Text = "--";

                return;
            }
            if (utc)
            {
                string rp = Properties.Settings.Default.useutch + ":" + Properties.Settings.Default.useutcm;
                format = format.Replace("K", rp).Replace("zzz", rp).Replace("zz", Properties.Settings.Default.useutch).Replace("z", Properties.Settings.Default.useutch);
                current.Text = "現在時間:" + dt.ToUniversalTime().AddHours(Properties.Settings.Default.useutcint).ToString(format);
                start.Text = "開始時間:" + st.ToUniversalTime().AddHours(Properties.Settings.Default.useutcint).ToString(format);
                end.Text = "終了時間:" + en.ToUniversalTime().AddHours(Properties.Settings.Default.useutcint).ToString(format);

            }
            else if (ms)
            {

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.mstime);

                DateTimeOffset ddt = DateTime.SpecifyKind(dt, DateTimeKind.Local);
                DateTimeOffset sst = DateTime.SpecifyKind(st, DateTimeKind.Local);
                DateTimeOffset een = DateTime.SpecifyKind(en, DateTimeKind.Local);

                string formatd = getoffset(ddt, format, tzi);
                string formats = getoffset(sst, format, tzi);
                string formate = getoffset(een, format, tzi);


                current.Text = "現在時間:" + TimeZoneInfo.ConvertTime(ddt, tzi).ToString(formatd);
                start.Text = "開始時間:" + TimeZoneInfo.ConvertTime(sst, tzi).ToString(formats);
                end.Text = "終了時間:" + TimeZoneInfo.ConvertTime(een, tzi).ToString(formate);


            }
            else if (tz)
           {

                string mkjson = Properties.Settings.Default.TZJSON;
                // JSONパース
                if (mkjson != null && mkjson !="")
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
                            string pattern = @"[dfFgGhtHKmMsTyz:/]";
                            tzst = Regex.Replace(tzst, pattern, match => "\\" + match.Value);


                            double uo = tzData.Offsets[lastTransitionIdx];
                            string abb = tzData.Abbrs[lastTransitionIdx];
                            string uoff = ToCustomFormat(uo, true).ToString();
                            abb = Regex.Replace(abb, pattern, match => "\\" + match.Value);

                            double uoc = tzData.Offsets[lastTransitionIdx_s];
                            string abbc = tzData.Abbrs[lastTransitionIdx_s];
                            string uoffc = ToCustomFormat(uoc, true).ToString();
                            abbc = Regex.Replace(abbc, pattern, match => "\\" + match.Value);

                            double uoe = tzData.Offsets[lastTransitionIdx_d];
                            string abbe = tzData.Abbrs[lastTransitionIdx_d];
                            string uoffe = ToCustomFormat(uoe, true).ToString();
                            abbe = Regex.Replace(abbe, pattern, match => "\\" + match.Value);


                            format = format.Replace("%TZ", tzst).Replace("%Z", abb).Replace("%z", uoff);
                            string formats = format.Replace("%TZ", tzst).Replace("%Z", abbc).Replace("%z", uoffc);
                            string formate = format.Replace("%TZ", tzst).Replace("%Z", abbe).Replace("%z", uoffe);
                            string patternn = @"[Kz]";
                            format = Regex.Replace(format, patternn, match => "");
                            formats = Regex.Replace(formats, patternn, match => "");
                            formate = Regex.Replace(formate, patternn, match => "");

                            current.Text = "現在時間:" + dt.ToUniversalTime().AddHours(uo).ToString(format);
                            start.Text = "開始時間:" + st.ToUniversalTime().AddHours(uoc).ToString(formats);
                            end.Text = "終了時間:" + en.ToUniversalTime().AddHours(uoe).ToString(formate);
                        }
                        else
                        {


                            string pattern = @"(%TZ|%z|%Z)";
                            format = Regex.Replace(format, pattern, match => "\\" + match.Value);

                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("UTC");

                            DateTimeOffset ddt = DateTime.SpecifyKind(dt, DateTimeKind.Local);
                            DateTimeOffset sst = DateTime.SpecifyKind(st, DateTimeKind.Local);
                            DateTimeOffset een = DateTime.SpecifyKind(en, DateTimeKind.Local);

                            string formatd = getoffset(ddt, format, tzi);
                            string formats = getoffset(sst, format, tzi);
                            string formate = getoffset(een, format, tzi);


                            current.Text = "現在時間:" + TimeZoneInfo.ConvertTime(ddt, tzi).ToString(formatd);
                            start.Text = "開始時間:" + TimeZoneInfo.ConvertTime(sst, tzi).ToString(formats);
                            end.Text = "終了時間:" + TimeZoneInfo.ConvertTime(een, tzi).ToString(formate);

                        }

                    }
                    catch (Exception ex)
                    {
                       MessageBox.Show(ex.ToString());
                    }

                }
            }
            else
            {
                current.Text = "現在時間:" + dt.ToString(format);
                start.Text = "開始時間:" + st.ToString(format);
                end.Text = "終了時間:" + en.ToString(format);
            }

            if (st < dt)
            {
                TimeSpan elapsedSpan = dt - st;
                elapsed.Text = "経過時間:" + getleft(elapsedSpan);
            }
            else
            {

                elapsed.Text = "イベントがまだ開始されてません";
            }


            if (en > dt)
            {
                TimeSpan leftSpan = en - dt;
                left.Text = "残り時間:" + getleft(leftSpan);
            }
            else
            {
                left.Text = "イベントはすでに終了しています";
            }

            TimeSpan drationSpan = en - st;

            duration.Text = "イベ期間:" + getleft(drationSpan);

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

        //https://chatgpt.com/c/675c1ac5-6f48-800f-b683-ae9745604c89
        static string ToCustomFormat(double value, bool useColon)
        {
            // 符号を取得
            string sign = value >= 0 ? "+" : "-";

            // 絶対値を取得
            double absValue = Math.Abs(value);

            // 整数部と小数部を分ける
            int hours = (int)Math.Floor(absValue);
            int minutes = (int)Math.Round((absValue - hours) * 60);

            // 時間が60分を超える場合の調整（丸め処理のため）
            if (minutes == 60)
            {
                hours += 1;
                minutes = 0;
            }

            // 書式を整える
            if (useColon)
            {
                return $"{sign}{hours:D2}:{minutes:D2}";
            }
            else
            {
                return $"{sign}{hours:D2}{minutes:D2}";
            }
        }

        private string getoffset(DateTimeOffset dt, string format, TimeZoneInfo tz)
        {
            var o1 = tz.GetUtcOffset(dt);
            string st = o1.ToString();

            string rp = Regex.Replace("+" + st, ":\\d\\d$", "");
            string rp2 = Regex.Replace("+" + st, ":\\d\\d:\\d\\d$", "");
            rp = Regex.Replace(rp, "\\+\\-", "-");
            rp2 = Regex.Replace(rp2, "\\+\\-", "-");

            string tmp = tz.StandardName;

            var DST = tz.IsDaylightSavingTime(dt);
            if (DST)
            {
                tmp = tz.DaylightName;
            }

            format = format.Replace("zzz", rp).Replace("zz", rp2).Replace("z", rp2).Replace("K", tmp);


            return format;
        }

        private string getleft(TimeSpan tspan)
        {
            string leftformat = Properties.Settings.Default.lefttimeformat;

            string dd = tspan.Days.ToString();
            string hh = tspan.Hours.ToString("00");
            string mm = tspan.Minutes.ToString("00");
            string ss = tspan.Seconds.ToString("00");

            string h = tspan.Hours.ToString("0");
            string m = tspan.Minutes.ToString("0");
            string s = tspan.Seconds.ToString("0");

            string ds = tspan.TotalDays.ToString("0.000");
            string hs = tspan.TotalHours.ToString("0.000");

            string MM = tspan.TotalDays.ToString("#");
            string HH = tspan.TotalHours.ToString("#");

            string[] rp = { HH, MM, ds, hs, dd, hh, mm, ss, h, m, s };
            string[] rpb = { "HH", "MM", "ds", "hs", "dd", "hh", "mm", "ss", "h", "m", "s" };

            string left = leftformat;
            for (var i = 0; i < rp.Length; i++)
            {
                left = left.Replace(rpb[i], rp[i]);
            }

            return left;
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


            if (comboBox1.Text == "かすたむJS")
            {
                button3_Click(sender, e);
                return;
            }

            try
            {


                var selecter = comboBox1.SelectedIndex;
                WebClient wc = new WebClient();

                wc.Encoding = Encoding.UTF8;
                var url2 = url + "?game=" + game[selecter];
                string text = wc.DownloadString(url2);
                var obj = Codeplex.Data.DynamicJson.Parse(text);



                ibemei.Text = obj.data.name;
                startbox.Text = obj.data.start;
                endbox.Text = obj.data.end;


                Properties.Settings.Default.json = text;
                wc.Dispose();

            }
            catch (WebException exc)
            {
                endbox.Text = exc.Message;
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

                // 取得した値を設定
                ibemei.Text = getValues[0] ?? "";
                startbox.Text = getValues[1] ?? "";
                endbox.Text = getValues[2] ?? "";
                comboBox1.Text = "かすたむJS";
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
        }

        private void 下パネルを表示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }



        private void クロマキー青_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.Blue;
            panel1.BackColor = Color.Blue;
        }

        private void クロマキー赤_Click(object sender, EventArgs e)
        {

            this.BackColor = Color.Red;
            panel1.BackColor = Color.Red;
        }

        private void クロマキー緑_Click(object sender, EventArgs e)
        {

            this.BackColor = Color.Green;
            panel1.BackColor = Color.Green;
        }

        private void カラーキー今のメニューToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TransparencyKey = this.BackColor;
        }

        private void カラーキーなしToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //int alpha = 128;
            //Color color = Color.FromArgb(alpha, 255, 0, 0);//R
            this.TransparencyKey = Color.Empty;
        }

        private void parcent_Click(object sender, EventArgs e)
        {

        }

        private void 文字白ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ForeColor = Color.White;
            panel1.ForeColor = Color.White;
        }

        private void 文字黒ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.ForeColor = Color.Black;
            panel1.ForeColor = Color.Black;
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
                this.ForeColor = fd.Color;
            }
        }
    }
}
    
