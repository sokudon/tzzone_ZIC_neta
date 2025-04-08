using Codeplex.Data;
using Microsoft.Win32;
using neta.Properties;
using Newtonsoft.Json.Linq;
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using TZPASER;

namespace neta
{
    public partial class NETA_TIMER : Form
    {
        private System.Windows.Forms.Timer timer;
        private string[] imageFiles;
        private int currentImageIndex = 0;
        private bool isSlideshowRunning = false;
        private string folderPath = @"C:\Users\imasp\OneDrive\Desktop\aimiku\4-8"; // 初期値

        public NETA_TIMER()
        {
            InitializeComponent();
            // ダブルバッファリングを有効化
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();

            // タイマーの設定
            timer = new System.Windows.Forms.Timer
            {
                Interval = Properties.Settings.Default.slidershow_interval //3000
            };
            timer.Tick += Timer_Tick;


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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

        //https://claude.site/artifacts/b26eae91-aff9-456f-a13d-013a1cae2eb5
        public async Task UpdateComboBox(string[] new_games, string new_url)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // 現在のコンボボックスの11番目と12番目の項目を保存
                    var item11 = comboBox1.Items[game_maxlen];
                    var item12 = comboBox1.Items[game_maxlen + 1];

                    if (new_games.Length != game_maxlen)
                    {
                        return;
                    }

                    // APIから全データを一度に取得
                    var response = await client.GetAsync(new_url);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var json = JObject.Parse(jsonString);

                        // 最初の10項目を更新
                        for (int i = 0; i < game_maxlen; i++)
                        {
                            // ネストされたパスからnameを取得
                            var name = json.SelectToken($"data.{new_games[i]}.name")?.ToString();
                            if (!string.IsNullOrEmpty(name))
                            {
                                // 既存の項目を更新
                                if (i < comboBox1.Items.Count)
                                {
                                    comboBox1.Items[i] = name;
                                }

                            }
                        }

                        // 11番目と12番目の項目を元に戻す
                        if (comboBox1.Items.Count > game_maxlen)
                        {
                            comboBox1.Items[game_maxlen] = item11;
                            comboBox1.Items[game_maxlen + 1] = item12;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"APIエラー: {response.StatusCode}", "エラー",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"エラーが発生しました: {ex.Message}", "エラー",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        string url = "https://script.google.com/macros/s/AKfycbxH2PF9yxHCZCp-e-n4LrGHRSi-Ag-E32trEdw_MhLrMf-cnkb8qwy27KwD7Deut1Mj2Q/exec";
        string[] game = { "gakumasu", "deresute", "mirsita", "shanimasu", "syanison", "yumesute", "proseka", "proseka_kr", "proseka_los", "proseka_hk" };
        string[] orig_games = { "学ます", "でれすて", "みりした", "シャニマス", "シャニソン", "ユメステ", "プロセカ", "プロセカグローバル", "プロセカ韓国", "プロセカ繁体" };
        int game_maxlen = 10;

        private void button2_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();

            wc.Encoding = Encoding.UTF8;
            var selecter = comboBox1.SelectedIndex;
            bool change_url = Properties.Settings.Default.change_baseurl;

            if (comboBox1.Text == "かすたむJS")
            {
                button3_Click(sender, e);
                return;
            }
            if (comboBox1.Text == "フリー入力")
            {
                return;
            }

            var url2 = url;
            string[] new_games = game;

            if (change_url)
            {
                new_games = Properties.Settings.Default.alt_basekey.Split(",");
                UpdateComboBox(new_games, Properties.Settings.Default.alt_baseurl);
                url2 = Properties.Settings.Default.alt_baseurl;
            }
            else
            {
                for (int i = 0; i < game_maxlen; i++)
                {
                    comboBox1.Items[i] = orig_games[i];
                }
            }

            if (selecter > game_maxlen)
            {
                return;
            }

            try
            {
                string text = "";

                text = wc.DownloadString(url2);
                if (text == null || text == "")
                {
                    endbox.Text = "baseurlの接続に失敗しました";
                    return;
                }
                var obj = Codeplex.Data.DynamicJson.Parse(text);
                string path = "/data/" + new_games[selecter] + "/name," +
                    "/data/" + new_games[selecter] + "/start," +
                    "/data/" + new_games[selecter] + "/end";

                get_json_parse(url2, text, path, false);



                Properties.Settings.Default.json = text;

            }
            catch (WebException exc)
            {
                endbox.Text = exc.Message;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //https://chatgpt.com/share/677e10d5-018c-800f-9356-ac6a02a537e2 begin updateみたいな描写制御        
            this_begin_update();

            if (Properties.Settings.Default.locale == "InvariantCulture")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Properties.Settings.Default.locale);
            }


            this.comboBox1.Text = Properties.Settings.Default.goog;
            this.startbox.Text = Properties.Settings.Default.st;
            this.endbox.Text = Properties.Settings.Default.en;
            this.ibemei.Text = Properties.Settings.Default.ibe;
            this.parcent.Left = Properties.Settings.Default.parcent;

            this.progressBar1.Width = Properties.Settings.Default.barlen;

            this.Font = Properties.Settings.Default.uifont;
            this.ForeColor = Properties.Settings.Default.uicolor;
            this.BackColor = Properties.Settings.Default.bgcolor;
            this.panel1.BackColor = Properties.Settings.Default.bgcolor;

            this.eventname.Font = Properties.Settings.Default.uifont;
            this.current.Font = Properties.Settings.Default.uifont;
            this.elapsed.Font = Properties.Settings.Default.uifont;

            this.eventname.ForeColor = Properties.Settings.Default.uicolor;
            this.current.ForeColor = Properties.Settings.Default.uicolor;
            panel1.ForeColor = Properties.Settings.Default.uicolor;

            this.panel2.Visible = Properties.Settings.Default.uihide;
            ぱねる１似合わせるToolStripMenuItem.Checked = Properties.Settings.Default.image_Stretch;
            hide_under_panel.Checked = Properties.Settings.Default.uihide;

            this.正月ミクさん.Checked = Properties.Settings.Default.syougautmiku;
            this.お花見みくさん.Checked = Properties.Settings.Default.ohanami_miku;
            this.星屑ハンターの双子.Checked = Properties.Settings.Default.hosikuzuhunter;


            Properties.Settings.Default.system_tz = TimeZoneInfo.Local.Id;

            LoadIni();

            if (comboBox1.SelectedItem != null)
            {
                string key = comboBox1.SelectedItem.ToString();
                if (HasImagePath(key))
                {
                    read_picture(imagePaths[key].ToString());
                    Properties.Settings.Default.lastimagefile = imagePaths[key];
                }
            }



            if (Properties.Settings.Default.font_margn)
            {
                menu_align(0, true);
            }
            if (Properties.Settings.Default.barvisible == false)
            {
                バーの表示ToolStripMenuItem_Click(sender, e);
            }


            if (Properties.Settings.Default.use_upui_chroma)
            {
                うえのいろToolStripMenuItem_Click(sender, e);
            }


            if (Properties.Settings.Default.change_baseurl)
            {

                button2_Click(sender, e);
            }


            isSlideshowRunning = Properties.Settings.Default.using_slideshow;
            if (isSlideshowRunning)
            {
                isSlideshowRunning = false;
                ToggleButton_Click(sender, e);
            }

            this_end_update();

            this.TransparencyKey = Properties.Settings.Default.colorkey;

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
            bool noda = Properties.Settings.Default.usenoda;
            string posix = Properties.Settings.Default.footerstring;
            string format = Properties.Settings.Default.datetimeformat;//"yyyy/MM/dd HH:mm:ss'(GMT'zzz')'";
            string mode = "";
            DateTime dt = DateTime.Now;
            error.Text = "";


            string current_tmp = "";
            string start_tmp = "";
            string end_tmp = "";
            string elapsed_tmp = "";
            string left_tmp = "";
            string duration_tmp = "";

            string current_c = Properties.Settings.Default.custom_curr;
            string elapsed_c = Properties.Settings.Default.custom_elapsed;
            string left_c = Properties.Settings.Default.custom_left;
            string duration_c = Properties.Settings.Default.custom_span;
            string start_c = Properties.Settings.Default.custom_start;
            string end_c = Properties.Settings.Default.custom_end;
            string ambigous_c = Properties.Settings.Default.custom_ambigous;
            string has_end_c = Properties.Settings.Default.custom_finished;
            string not_start_c = Properties.Settings.Default.custom_not_start;
            string noda_strict_error = Properties.Settings.Default.noda_strict_error;




            // インデックス0の項目のチェック状態を取得
            bool d_curr = Properties.Settings.Default.display_curr;
            bool d_el = Properties.Settings.Default.display_elapsed;
            bool d_lf = Properties.Settings.Default.display_left;
            bool d_sp = Properties.Settings.Default.display_span;
            bool d_st = Properties.Settings.Default.display_start;
            bool d_en = Properties.Settings.Default.display_end;

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

                error.Text += "invalid date(ex: supoort format";
                error.Text += "NOMARL: 2020/12/18 21:00  <-this convert Localtime OS denpending";
                error.Text += "ISO8601: 2020-12-18T21:00:00+09:00";
                error.Text += "RFC2822: Sun, 10 Mar 2024 03:00:00 PDT)";
                if (noda_strict_error != "")
                {

                    error.Text += noda_strict_error;
                }

                return;
            }
            if (TryParseDateTimeCutom(endbox.Text, out en))
            {

            }
            else
            {

                error.Text += "invalid date(ex: supoort format";
                error.Text += "NOMARL: 2020/12/18 21:00  <-this convert Localtime OS denpending";
                error.Text += "ISO8601: 2020-12-18T21:00:00+09:00";
                error.Text += "RFC2822: Sun, 10 Mar 2024 03:00:00 PDT)";
                if (noda_strict_error != "")
                {

                    error.Text += noda_strict_error;
                }

                return;
            }
            if (utc)
            {
                dt = dt.ToUniversalTime();
                string rp = Properties.Settings.Default.useutch + ":" + Properties.Settings.Default.useutcm;
                format = format.Replace("K", rp).Replace("zzz", rp).Replace("zz", Properties.Settings.Default.useutch).Replace("z", Properties.Settings.Default.useutch);
                current_tmp = current_c + dt.AddHours(Properties.Settings.Default.useutcint).ToString(format);
                start_tmp = start_c + st.AddHours(Properties.Settings.Default.useutcint).ToString(format);
                end_tmp = end_c + en.AddHours(Properties.Settings.Default.useutcint).ToString(format);

                mode = "UTC Master;" + rp;
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


                current_tmp = current_c + TimeZoneInfo.ConvertTime(ddt, tzi).ToString(formatd);
                start_tmp = start_c + TimeZoneInfo.ConvertTime(sst, tzi).ToString(formats);
                end_tmp = end_c + TimeZoneInfo.ConvertTime(een, tzi).ToString(formate);

                bool isAmbiguouss = tzi.IsAmbiguousTime(TimeZoneInfo.ConvertTime(sst, tzi));
                bool isAmbiguouse = tzi.IsAmbiguousTime(TimeZoneInfo.ConvertTime(een, tzi));
                mode = "Microsoft Timezone:" + Properties.Settings.Default.mstime;
                if (isAmbiguouss) { mode += "start" + ambigous_c; }
                if (isAmbiguouse) { mode += "end" + ambigous_c; }

            }
            else if (noda)
            {
                string tznd = Properties.Settings.Default.noddatz;
                if (TZPASER.nodaparser.CheckTimeZoneExists(tznd))
                {
                    try
                    {
                        // 2. UTC時刻を指定したタイムゾーンの現地時間に変換
                        ZonedDateTime convertedTime = TZPASER.nodaparser.ConvertToTimeZone(dt.ToUniversalTime(), tznd);
                        ZonedDateTime convertedTime_st = TZPASER.nodaparser.ConvertToTimeZone(st, tznd);
                        ZonedDateTime convertedTime_en = TZPASER.nodaparser.ConvertToTimeZone(en, tznd);

                        //var zonebcl = DateTimeZoneProviders.Bcl;　どっちからしい
                        var zonetzdb = DateTimeZoneProviders.Tzdb;

                        var pattern = Properties.Settings.Default.nodaformat;
                        DateTimeZone zone = DateTimeZoneProviders.Tzdb[tznd]; // Specify your timezone
                        //ZonedDateTimePattern formatter = ZonedDateTimePattern.CreateWithInvariantCulture(pattern, zonetzdb);
                        ZonedDateTimePattern formatter = ZonedDateTimePattern.CreateWithCurrentCulture(pattern, zonetzdb);


                        // Format
                        string timeString = formatter.Format(SystemClock.Instance.GetCurrentInstant().InZone(zone));
                        string timeStrings = formatter.Format(convertedTime_st);
                        string timeStringe = formatter.Format(convertedTime_en);


                        current_tmp = current_c + timeString;
                        start_tmp = start_c + timeStrings;
                        end_tmp = end_c + timeStringe;


                        // タイムゾーンでの ZonedDateTime の取得
                        var mappings = zone.MapLocal(convertedTime_st.LocalDateTime);
                        var mappings_e = zone.MapLocal(convertedTime_en.LocalDateTime);


                        //https://chatgpt.com/share/678e6fdb-f598-800f-8faa-2c5521c95496 invaidtime とambigoustime
                        mode = "Nodatime Timezone:" + zone;
                        // 時間があいまいかどうかを確認
                        if (mappings.Count > 1)
                        {
                            mode += "start" + ambigous_c;
                        }
                        if (mappings_e.Count > 1)
                        {
                            mode += "end" + ambigous_c;
                        }
                        if (noda_strict_error != "")
                        {

                            mode += noda_strict_error;
                        }

                    }
                    catch (Exception ex)
                    {
                        error.Text += "フォーマットでふせいです" + ex.ToString();
                        error.Text += "error;";
                        error.Text += "unsuppored timezone nodatime";

                    }
                }
                else
                {
                    error.Text += "のだたいむが対応してないタイムゾーンです";
                    error.Text += "error;";
                    error.Text += "unsuppored timezone nodatime";
                }

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

                        dt = dt.ToUniversalTime();

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

                            current_tmp = current_c + dt.AddHours(uo).ToString(formatc);
                            start_tmp = start_c + st.AddHours(uoc).ToString(formats);
                            end_tmp = end_c + en.AddHours(uoe).ToString(formate);

                            mode = "TZif binay BisectR:" + Properties.Settings.Default.usetzdatabin;

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
                                current_tmp = current_c + dt.ToUniversalTime().AddHours(uo).ToString(format);
                                start_tmp = start_c + st.ToUniversalTime().AddHours(uo).ToString(format);
                                end_tmp = end_c + en.ToUniversalTime().AddHours(uo).ToString(format);


                                mode = "TZif binay BisectR:" + Properties.Settings.Default.usetzdatabin;
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


                                current_tmp = current_c + TimeZoneInfo.ConvertTime(ddt, tzi).ToString(formatd);
                                start_tmp = start_c + TimeZoneInfo.ConvertTime(sst, tzi).ToString(formats);
                                end_tmp = end_c + TimeZoneInfo.ConvertTime(een, tzi).ToString(formate);


                                mode = "TZif binay BisectR:parse error,UTC mode";
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        error.Text += "例外発生 ";
                        error.Text += "えらー:tzdbを変換したJSONが空か異常があります";
                        error.Text += ex.ToString();
                    }

                }
            }
            else
            {
                current_tmp = current_c + dt.ToLocalTime().ToString(format);
                start_tmp = start_c + st.ToLocalTime().ToString(format);
                end_tmp = end_c + en.ToLocalTime().ToString(format);

                mode = "Local Timezone";
            }

            string L_format = Properties.Settings.Default.lefttimeformat;
            //st enはUTCになっているのでutcに必ずする
            dt = dt.ToUniversalTime();

            if (st < dt)
            {
                TimeSpan elapsedSpan = dt - st;
                elapsed_tmp = elapsed_c + TZPASER.TimeZoneOffsetParser.getleft(elapsedSpan, L_format);

                if (en > dt)
                {
                    TimeSpan leftSpan = en - dt;
                    left_tmp = left_c + TZPASER.TimeZoneOffsetParser.getleft(leftSpan, L_format);
                }
                else
                {
                    left_tmp = left_c + has_end_c;
                }

            }
            else
            {

                elapsed_tmp = elapsed_c + not_start_c;
                left_tmp = left_c + not_start_c;
            }


            TimeSpan drationSpan = en - st;

            duration_tmp = duration_c + TZPASER.TimeZoneOffsetParser.getleft(drationSpan, L_format);


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

            if (d_curr == false) { current.Text = ""; }
            if (d_st == false) { start.Text = ""; }
            if (d_en == false) { end.Text = ""; }
            if (d_el == false) { elapsed.Text = ""; }
            if (d_lf == false) { left.Text = ""; }
            if (d_sp == false) { duration.Text = ""; }


            if (縺化けUTF8SJIS.Checked)
            {

                eventname.Text = wrong_encoder(ibemei.Text);

                if (d_curr) { current.Text = wrong_encoder(current_tmp); }
                if (d_st) { start.Text = wrong_encoder(start_tmp); }
                if (d_en) { end.Text = wrong_encoder(end_tmp); }
                if (d_el) { elapsed.Text = wrong_encoder(elapsed_tmp); }
                if (d_lf) { left.Text = wrong_encoder(left_tmp); }
                if (d_sp) { duration.Text = wrong_encoder(duration_tmp); }

                label1.Text = wrong_encoder(mode);

            }
            else if (縺化け戻し.Checked)
            {
                eventname.Text = wrong_encoder_restore(ibemei.Text);
                if (d_curr) { current.Text = wrong_encoder_restore(current_tmp); }
                if (d_st) { start.Text = wrong_encoder_restore(start_tmp); }
                if (d_en) { end.Text = wrong_encoder_restore(end_tmp); }
                if (d_el) { elapsed.Text = wrong_encoder_restore(elapsed_tmp); }
                if (d_lf) { left.Text = wrong_encoder_restore(left_tmp); }
                if (d_sp) { duration.Text = wrong_encoder_restore(duration_tmp); }
                label1.Text = wrong_encoder_restore(mode);

            }
            else if (コードページ指定.Checked)
            {
                try
                {
                    Encoding encoding_in = Encoding.GetEncoding(Properties.Settings.Default.wrong_encoder_in);
                    Encoding encoding_out = Encoding.GetEncoding(Properties.Settings.Default.wrong_encoder_out);
                    eventname.Text = wrong_encoder_codepage(ibemei.Text, encoding_in, encoding_out);

                    if (d_curr) { current.Text = wrong_encoder_codepage(current_tmp, encoding_in, encoding_out); }
                    if (d_st) { start.Text = wrong_encoder_codepage(start_tmp, encoding_in, encoding_out); }
                    if (d_en) { end.Text = wrong_encoder_codepage(end_tmp, encoding_in, encoding_out); }
                    if (d_el) { elapsed.Text = wrong_encoder_codepage(elapsed_tmp, encoding_in, encoding_out); }
                    if (d_lf) { left.Text = wrong_encoder_codepage(left_tmp, encoding_in, encoding_out); }
                    if (d_sp) { duration.Text = wrong_encoder_codepage(duration_tmp, encoding_in, encoding_out); }

                    label1.Text = wrong_encoder_codepage(mode, encoding_in, encoding_out);

                }
                catch (Exception ex)
                {
                    error.Text += "例外発生 ";
                    error.Text += "エンコーディングエラー";
                    error.Text += ex.ToString();
                }
            }
            else
            {
                eventname.Text = ibemei.Text;

                if (d_curr) { current.Text = (current_tmp); }
                if (d_st) { start.Text = (start_tmp); }
                if (d_en) { end.Text = (end_tmp); }
                if (d_el) { elapsed.Text = (elapsed_tmp); }
                if (d_lf) { left.Text = (left_tmp); }
                if (d_sp) { duration.Text = (duration_tmp); }


                label1.Text = mode;
            }
        }

        private static string wrong_encoder(string input)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(input);
            string wrong_sjis = Encoding.GetEncoding("Shift_JIS").GetString(utf8);

            return wrong_sjis;
        }

        private static string wrong_encoder_restore(string input)
        {
            byte[] st_bytes = Encoding.GetEncoding(932).GetBytes(input);
            string wrong_sjis = Encoding.GetEncoding(65001).GetString(st_bytes);

            return wrong_sjis;
        }


        private static string wrong_encoder_codepage(string input, Encoding incp, Encoding outcp)
        {
            byte[] st_bytes = incp.GetBytes(input);
            string wrong_st = outcp.GetString(st_bytes);

            return wrong_st;
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

        // 現在選択されているコンボボックス項目の画像パスが存在するか確認
        public bool HasCurrentImagePath()
        {
            if (comboBox1.SelectedItem == null) return false;
            return HasImagePath(comboBox1.SelectedItem.ToString());
        }
        // 画像パスが存在するか確認
        public bool HasImagePath(string key)
        {
            return imagePaths.ContainsKey(key) && !string.IsNullOrEmpty(imagePaths[key]);
        }

        // 画像パスを取得（存在しない場合はnullを返す）
        public string GetImagePath(string key)
        {
            return HasImagePath(key) ? imagePaths[key] : null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedItem != null)
            {
                string key = comboBox1.SelectedItem.ToString();
                if (HasImagePath(key))
                {
                    read_picture(imagePaths[key].ToString());
                    Properties.Settings.Default.lastimagefile = imagePaths[key];
                }
                else
                {
                    panel1.BackgroundImage = null;          // 背景画像を削除
                    panel1.BackColor = this.BackColor;
                    panel1.Invalidate();                    // 再描画をリクエスト
                }
            }

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
                var url2 = url;
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
                if (selecter > game_maxlen) { return; }

                var obj = Codeplex.Data.DynamicJson.Parse(text);
                string path = "/data/" + game[selecter] + "/name," +
                    "/data/" + game[selecter] + "/start," +
                    "/data/" + game[selecter] + "/end";

                get_json_parse(url2, text, path, false);

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

        public static string GetDefaultBrowser()
        {
            string browser = string.Empty;
            RegistryKey key = null;
            try
            {
                key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                if (key != null)
                {
                    // デフォルトブラウザのパスを取得
                    browser = key.GetValue(null).ToString().ToLower();

                    // コマンドライン引数を削除
                    if (!string.IsNullOrEmpty(browser))
                    {
                        browser = browser.Split('"')[1];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: {ex.Message}");
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
            }
            return browser;
        }

        private void wEBたいまーToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime st;
            DateTime en;
            string sst = ",";
            string sen = ",";
            string format = "yyyy-MM-dd'T'HH:mm:ssZ";
            if (TryParseDateTimeCutom(startbox.Text, out st))
            {
                sst = st.ToUniversalTime().ToString(format);
            }
            if (TryParseDateTimeCutom(endbox.Text, out en))
            {
                sen = en.ToUniversalTime().ToString(format);
                sen = Uri.EscapeDataString(sen) + ",";
            }
            string gamename = Uri.EscapeDataString(ibemei.Text) + ",";

            string url = "https://ss1.xrea.com/sokudon.s17.xrea.com/neta/imm.html#";// NAME,START,END,OS,";
            url = url + gamename + sst + sen + "OS,";
            System.Diagnostics.Process.Start(GetDefaultBrowser(), url);

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
            if (TryParseDateTimeCutom(startbox.Text, out st))
            {
                sst = st.ToUniversalTime().ToString(format);
            }
            if (TryParseDateTimeCutom(endbox.Text, out en))
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


        private void wEBせかいどけいToolStripMenuItem_Click(object sender, EventArgs e)
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

            string url = "https://ss1.xrea.com/sokudon.s17.xrea.com/sekai_miku.html#";
            url = url + gamename + sst + sen;
            string ms = Properties.Settings.Default.useutczone;
            Match m = Regex.Match(ms, "M\\$.+$");
            if (m.Success)
            {
                url += m.Value;
            }

            System.Diagnostics.Process.Start(GetDefaultBrowser(), url);

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
                .Replace("%T%n経過時間%K%n残り時間%L%nイベント時間%I%n現地時間%N%n日本時間%JST%n達成率%P%nS %S%nE %E%n%nSJ %SJ%nEJ %EJ%n%nSU %SU%nEU %EU", "日本時間%JST\\n経過時間%K\\n残り時間%L\\nイベント時間%I%\\n%T%P％\\n%Q")
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
            Properties.Settings.Default.uihide = !panel2.Visible;
            hide_under_panel.Checked = !panel2.Visible;
            panel2.Visible = !panel2.Visible;
        }


        private void クロマキー設定_Click(object sender, EventArgs e)
        {
            ChromaKeyColorPicker picker = new ChromaKeyColorPicker(this);
            picker.ColorApplied += (color) =>
            {
                panel1.BackColor = color;
                this.BackColor = color;
                Properties.Settings.Default.bgcolor = color;

                if (Properties.Settings.Default.use_upui_chroma)
                {
                    うえのいろToolStripMenuItem_Click(sender, e);
                }
            };
            picker.Show();  // モーダレス表示
        }

        public class CustomProgressBar : ProgressBar
        {
            public Color BarColor { get; set; } = Color.Blue;

            public CustomProgressBar()
            {
                // プログレスバーのダブルバッファリングを有効化
                SetStyle(ControlStyles.UserPaint, true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                // 背景を描画
                System.Drawing.Rectangle rect = this.ClientRectangle;
                e.Graphics.FillRectangle(Brushes.White, rect);

                // プログレスバーの塗りつぶし部分を描画
                rect.Width = (int)(rect.Width * ((double)Value / Maximum));
                e.Graphics.FillRectangle(new SolidBrush(BarColor), rect);

                // 枠線を描画
                e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
            }
        }


        public class ChromaKeyColorPicker : Form
        {
            private readonly Form parentForm;  // 親フォームの参照を保持

            // コンストラクタで親フォームを受け取る
            public ChromaKeyColorPicker(Form parent)
            {
                this.parentForm = parent;
                InitializeComponents();
                SetupEvents();
            }

            private Color selectedColor;
            private TrackBar alphaTrackBar;
            private ComboBox presetCombo;
            private PictureBox colorPalette;
            private Panel colorPreview;
            private TrackBar[] rgbTrackBars;
            private NumericUpDown[] rgbNumerics;
            private PictureBox targetImagePreview;
            private TextBox obsSettingsText;

            // プリセット色の定義
            private readonly Dictionary<string, Color> presetColors = new Dictionary<string, Color>
    {
            {"元の背景色", Properties.Settings.Default.bgcolor},
        {"グリーン", Color.FromArgb(255, 0, 255, 0)},
        {"ブルー", Color.FromArgb(255, 0, 0, 255)},
        {"レッド", Color.FromArgb(255, 255, 0, 0)},
        {"シアン", Color.FromArgb(255, 0, 255, 255)},
        {"マゼンタ", Color.FromArgb(255, 255, 0, 255)},
        {"ブライトパープル", Color.FromArgb(255, 180, 0, 255)}
    };
            // カラー変更イベントの定義
            public event Action<Color> ColorApplied;

            private void InitializeComponents()
            {
                this.Size = new System.Drawing.Size(800, 600);
                this.Text = "クロマキー カラーピッカー";

                // プリセットコンボボックス
                presetCombo = new ComboBox
                {
                    Location = new System.Drawing.Point(20, 20),
                    Size = new System.Drawing.Size(150, 25),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                presetCombo.Items.AddRange(presetColors.Keys.ToArray());
                this.Controls.Add(presetCombo);

                // カラープレビュー
                colorPreview = new Panel
                {
                    Location = new System.Drawing.Point(20, 60),
                    Size = new System.Drawing.Size(150, 150),
                    BorderStyle = BorderStyle.FixedSingle
                };
                this.Controls.Add(colorPreview);



                // RGB・アルファスライダーの設定
                rgbTrackBars = new TrackBar[3];
                rgbNumerics = new NumericUpDown[3];
                string[] rgbLabels = { "R:", "G:", "B:" };

                for (int i = 0; i < 3; i++)
                {
                    CreateColorControl(rgbLabels[i], 20, 230 + (i * 60), out rgbTrackBars[i], out rgbNumerics[i]);
                }



                // 画像分析セクション
                GroupBox analysisGroup = new GroupBox
                {
                    Text = "クロマキー分析",
                    Location = new System.Drawing.Point(400, 20),
                    Size = new System.Drawing.Size(360, 500)
                };
                this.Controls.Add(analysisGroup);

                Button applyButton = new Button
                {
                    Text = "カラー設定を適用",
                    Location = new System.Drawing.Point(10, 30),
                    Size = new System.Drawing.Size(100, 30),
                    Parent = analysisGroup
                };
                applyButton.Click += (s, e) =>
                {
                    ColorApplied?.Invoke(selectedColor);
                };

                targetImagePreview = new PictureBox
                {
                    Location = new System.Drawing.Point(10, 70),
                    Size = new System.Drawing.Size(340, 200),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    Parent = analysisGroup
                };

                obsSettingsText = new TextBox
                {
                    Location = new System.Drawing.Point(10, 280),
                    Size = new System.Drawing.Size(340, 200),
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    ReadOnly = true,
                    Parent = analysisGroup
                };




            }

            private void SetupEvents()
            {
                presetCombo.SelectedIndexChanged += (s, e) =>
                {
                    if (presetCombo.SelectedItem != null)
                    {
                        var colorName = presetCombo.SelectedItem.ToString();
                        if (presetColors.ContainsKey(colorName))
                        {
                            UpdateColor(presetColors[colorName]);
                        }
                        else
                        {
                            MessageBox.Show($"指定の色 '{colorName}' がプリセットに見つかりません。");
                        }
                    }
                };

                presetCombo.SelectedItem = "元の背景色";
                if (presetColors.ContainsKey("元の背景色"))
                {
                    UpdateColor(presetColors["元の背景色"]);
                }


                //https://claude.ai/chat/fd23135f-2e05-4ea2-843b-6f0998363871
                // 画像分析イベント
                targetImagePreview.Click += async (s, e) =>
                {
                    try
                    {
                        string img = Properties.Settings.Default.lastimagefile;
                        if (File.Exists(img))
                        {
                            await LoadAndAnalyzeImage(img);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"エラー: {ex.Message}");
                    }
                };


            }

            private async Task LoadAndAnalyzeImage(string imagePath)
            {
                try
                {
                    // 既存の画像を適切に破棄
                    if (targetImagePreview.Image != null)
                    {
                        var oldImage = targetImagePreview.Image;
                        targetImagePreview.Image = null;
                        oldImage.Dispose();
                    }

                    // 画像をメモリにコピーして読み込み
                    using (var stream = new MemoryStream(File.ReadAllBytes(imagePath)))
                    {
                        // 新しい画像のインスタンスを作成
                        targetImagePreview.Image = new Bitmap(System.Drawing.Image.FromStream(stream));
                    }

                    await AnalyzeImage();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"画像の読み込みエラー: {ex.Message}");
                }
            }

            // 2つの色の距離を計算
            static double ColorDistance(Color color1, Color color2)
            {
                double bDiff = color1.B - color2.B;
                double gDiff = color1.G - color2.G;
                double rDiff = color1.R - color2.R;

                return Math.Sqrt(bDiff * bDiff + gDiff * gDiff + rDiff * rDiff);
            }


            // 分析処理の修正
            private async Task AnalyzeImage()
            {
                if (targetImagePreview.Image == null) return;

                await Task.Run(() =>
                {
                    try
                    {
                        using (Bitmap bmp = new Bitmap(targetImagePreview.Image))
                        {
                            var (averageColor, colorRatios) = AnalyzeImageColors(bmp);

                            StringBuilder analysisResult = new StringBuilder();
                            analysisResult.AppendLine("【分析結果】");
                            analysisResult.AppendLine($"画像サイズ: {bmp.Width}x{bmp.Height}");

                            // 平均色の表示
                            analysisResult.AppendLine($"\n平均色:");
                            analysisResult.AppendLine($"R:{averageColor.R} G:{averageColor.G} B:{averageColor.B}");

                            // 色の分布
                            analysisResult.AppendLine("\n色の分布:");
                            foreach (var ratio in colorRatios.OrderByDescending(r => r.Value))
                            {
                                if (ratio.Value > 1.0) // 1%以上の色のみ表示
                                {
                                    analysisResult.AppendLine($"{ratio.Key}: {ratio.Value:F1}%");
                                }
                            }

                            // 距離をリストに格納
                            List<(string Name, Color Color, double Distance)> distances = new List<(string, Color, double)>();
                            foreach (var chromaColor in presetColors)
                            {
                                double distance = ColorDistance(averageColor, chromaColor.Value);
                                distances.Add((chromaColor.Key + chromaColor.Value.Name, chromaColor.Value, distance));
                            }

                            // 距離を昇順にソート
                            distances.Sort((a, b) => a.Distance.CompareTo(b.Distance));

                            // 結果を表示
                            analysisResult.AppendLine("クロマキー候補色とのゆーくりっど距離 (昇順):");
                            foreach (var entry in distances)
                            {
                                analysisResult.AppendLine($"- 色: {entry.Name}, 距離: {entry.Distance:F2}, RGB: ({entry.Color.R}, {entry.Color.G}, {entry.Color.B})");
                            }

                            this.Invoke((MethodInvoker)delegate
                            {
                                obsSettingsText.Text = analysisResult.ToString();
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            obsSettingsText.Text = $"分析エラー: {ex.Message}";
                        });
                    }
                });
            }

            // 画像の色分析関数
            private (Color averageColor, Dictionary<string, double> colorRatios) AnalyzeImageColors(Bitmap bmp)
            {
                long totalR = 0, totalG = 0, totalB = 0;
                int totalPixels = bmp.Width * bmp.Height;
                Dictionary<string, int> colorGroups = new Dictionary<string, int>
    {
        {"Red", 0},
        {"Green", 0},
        {"Blue", 0},
        {"Magenta", 0},
        {"Cyan", 0},
        {"Yellow", 0},
        {"White", 0},
        {"Black", 0}
    };

                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        Color pixel = bmp.GetPixel(x, y);
                        totalR += pixel.R;
                        totalG += pixel.G;
                        totalB += pixel.B;

                        // 色相による分類
                        if (Math.Max(Math.Max(pixel.R, pixel.G), pixel.B) - Math.Min(Math.Min(pixel.R, pixel.G), pixel.B) < 30)
                        {
                            if (pixel.R > 200) colorGroups["White"]++;
                            else if (pixel.R < 50) colorGroups["Black"]++;
                        }
                        else if (pixel.R > pixel.G && pixel.R > pixel.B) colorGroups["Red"]++;
                        else if (pixel.G > pixel.R && pixel.G > pixel.B) colorGroups["Green"]++;
                        else if (pixel.B > pixel.R && pixel.B > pixel.G) colorGroups["Blue"]++;
                        else if (pixel.R > 200 && pixel.B > 200) colorGroups["Magenta"]++;
                        else if (pixel.G > 200 && pixel.B > 200) colorGroups["Cyan"]++;
                        else if (pixel.R > 200 && pixel.G > 200) colorGroups["Yellow"]++;
                    }
                }

                // 平均色の計算
                Color averageColor = Color.FromArgb(
                    (int)(totalR / totalPixels),
                    (int)(totalG / totalPixels),
                    (int)(totalB / totalPixels)
                );

                // 色の割合を計算
                Dictionary<string, double> colorRatios = colorGroups.ToDictionary(
                    kv => kv.Key,
                    kv => (double)kv.Value / totalPixels * 100
                );

                return (averageColor, colorRatios);
            }


            // UpdateColorメソッドの修正
            private void UpdateColor(Color? newColor = null)
            {
                if (newColor.HasValue)
                {
                    selectedColor = newColor.Value;
                    // RGB値を正しく設定
                    rgbTrackBars[0].Value = newColor.Value.R;
                    rgbTrackBars[1].Value = newColor.Value.G;
                    rgbTrackBars[2].Value = newColor.Value.B;
                    rgbNumerics[0].Value = newColor.Value.R;
                    rgbNumerics[1].Value = newColor.Value.G;
                    rgbNumerics[2].Value = newColor.Value.B;

                }
                else
                {
                    selectedColor = Color.FromArgb(
                   (int)rgbNumerics[0].Value,
                    (int)rgbNumerics[1].Value,
                    (int)rgbNumerics[2].Value
                 );
                }
                targetImagePreview.BackColor = selectedColor;
                colorPreview.BackColor = selectedColor;
            }

            // ColorControl_ValueChangedの修正
            private void ColorControl_ValueChanged(object sender, EventArgs e)
            {
                if (sender is TrackBar trackBar)
                {
                    int index = Array.IndexOf(rgbTrackBars, trackBar);
                    if (index >= 0)
                    {
                        rgbNumerics[index].Value = trackBar.Value;
                    }
                }
                else if (sender is NumericUpDown numeric)
                {
                    int index = Array.IndexOf(rgbNumerics, numeric);
                    if (index >= 0)
                    {
                        rgbTrackBars[index].Value = (int)numeric.Value;
                    }
                }
                UpdateColor();
            }

            private TrackBar redTrackBar, greenTrackBar, blueTrackBar;
            private NumericUpDown redNumeric, greenNumeric, blueNumeric;


            private void CreateColorControl(string label, int x, int y,
         out TrackBar trackBar, out NumericUpDown numeric)
            {
                Label lbl = new Label
                {
                    Text = label,
                    Location = new System.Drawing.Point(x, y),
                    Size = new System.Drawing.Size(30, 20)
                };
                this.Controls.Add(lbl);

                trackBar = new TrackBar
                {
                    Location = new System.Drawing.Point(x + 30, y),
                    Size = new System.Drawing.Size(150, 45),
                    Minimum = 0,
                    Maximum = 255
                };
                trackBar.ValueChanged += ColorControl_ValueChanged;
                this.Controls.Add(trackBar);

                numeric = new NumericUpDown
                {
                    Location = new System.Drawing.Point(x + 190, y),
                    Size = new System.Drawing.Size(60, 20),
                    Minimum = 0,
                    Maximum = 255
                };
                numeric.ValueChanged += ColorControl_ValueChanged;
                this.Controls.Add(numeric);
            }


        }

        private void めにゅーの色に戻すToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.BackColor = this.menuStrip1.BackColor;
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
            Properties.Settings.Default.colorkey = this.TransparencyKey;
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
            fd.MaxSize = 20;
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

        private void read_picture(string s)
        {
            try
            {
                if (s == "null")
                {
                    if (正月ミクさん.Checked) { panel1.BackgroundImage = Resources.syougtu_kurinuki; }
                    if (星屑ハンターの双子.Checked) { panel1.BackgroundImage = Resources.hosikuzu; }
                    panel1.BackgroundImageLayout = ImageLayout.Stretch; // 必要に応じて調整
                    panel1.BackColor = this.BackColor;
                    panel1.Invalidate();                    // 再描画をリクエスト
                    return;
                }
                if (s == "none")
                {
                    panel1.BackgroundImage = null;          // 背景画像を削除
                    panel1.BackColor = this.BackColor;
                    panel1.Invalidate();                    // 再描画をリクエスト
                    return;
                }
                if (File.Exists(s))
                {
                    // 画像を直接パネルの背景として設定
                    panel1.BackgroundImage = System.Drawing.Image.FromFile(s);
                    if (ぱねる１似合わせるToolStripMenuItem.Checked)
                    {
                        panel1.BackgroundImageLayout = ImageLayout.Stretch; // 必要に応じて調整
                    }
                    else
                    {
                        panel1.BackgroundImageLayout = ImageLayout.Tile;

                    }

                    Properties.Settings.Default.lastimagefile = s;

                    LoadImages(Path.GetDirectoryName(s));

                    SaveIni();
                }
            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.ToString());
            }
        }

        private void 画像ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();

            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Properties.Settings.Default.lastimagefile);

            ofd.Filter = "すべての対応画像|*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.tiff;*.ico|" +
                     "BMP 画像 (*.bmp)|*.bmp|" +
                     "JPEG 画像 (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                     "GIF 画像 (*.gif)|*.gif|" +
                     "PNG 画像 (*.png)|*.png|" +
                     "TIFF 画像 (*.tiff)|*.tiff|" +
                     "ICO アイコン (*.ico)|*.ico";
            //"WebP 画像 (*.webp)|*.webp";

            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;


            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {


                panel1.BackgroundImage = null;
                panel2.BackgroundImage = null;


                string s = ofd.FileName;
                string key = comboBox1.SelectedItem.ToString();
                imagePaths[key] = ofd.FileName;
                read_picture(s);
            }

        }

        private Dictionary<string, string> imagePaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
        };
        private const string INI_FILE = "imagePaths.ini";

        // INI形式で保存
        private void SaveIni()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(INI_FILE))
                {
                    writer.WriteLine("[ImagePaths]");
                    foreach (var pair in imagePaths)
                    {
                        writer.WriteLine($"{pair.Key}={pair.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"INIの保存に失敗しました: {ex.Message}");
            }
        }

        private void UpdatePictureBox()
        {
            if (comboBox1.SelectedItem != null)
            {
                string key = comboBox1.SelectedItem.ToString();
                if (imagePaths.ContainsKey(key))
                {
                    read_picture(imagePaths[key]);
                }
            }
        }

        // INI形式で読み込み
        private void LoadIni()
        {
            if (!File.Exists(INI_FILE)) return;

            try
            {
                imagePaths.Clear();
                string[] lines = File.ReadAllLines(INI_FILE);
                bool inImagePathsSection = false;

                foreach (string line in lines)
                {
                    if (line.Trim().StartsWith("["))
                    {
                        inImagePathsSection = line.Trim() == "[ImagePaths]";
                        continue;
                    }

                    if (inImagePathsSection && line.Contains("="))
                    {
                        string[] parts = line.Split(new[] { '=' }, 2);
                        if (parts.Length == 2)
                        {
                            imagePaths[parts[0].Trim()] = parts[1].Trim();
                        }
                    }
                }
                UpdatePictureBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"INIの読み込みに失敗しました: {ex.Message}");
            }
        }

        private Image convert_pictute(byte[] imageBytes)
        {

            if (imageBytes != null && imageBytes.Length > 0)
            {
                try
                {
                    // バイト配列からMemoryStreamを作成
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        // MemoryStreamからImageオブジェクトを作成
                        // FromStreamはストリームを閉じないオプション(leaveOpen=true)が推奨されることもありますが、
                        // 多くの場合、Bitmapが内部でデータをコピーするため、usingを抜けても大丈夫です。
                        Image img = Image.FromStream(ms);

                        return img;
                    }
                }
                catch (ArgumentException ex)
                {
                    // バイト配列が有効な画像データでない場合など
                    MessageBox.Show("画像データの読み込みに失敗しました: " + ex.Message);
                    return null;
                }
            }
            else
            {
                // リソースが見つからない、または空の場合
                MessageBox.Show("画像リソースが見つからないか、空です。");
                return null;
            }
            return null;
        }

        private void でふぉるとにもどす正月みくToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (正月ミクさん.Checked) { panel1.BackgroundImage = Resources.syougtu_kurinuki; }
            if (お花見みくさん.Checked) { panel1.BackgroundImage = convert_pictute(Resources.ohanami_mikusan); }
            if (星屑ハンターの双子.Checked) { panel1.BackgroundImage = Resources.hosikuzu; }

            panel1.BackgroundImageLayout = ImageLayout.Stretch; // 必要に応じて調整
            panel1.BackColor = this.BackColor;
            panel1.Invalidate();                    // 再描画をリクエスト
            Properties.Settings.Default.lastimagefile = "null";

            string key = comboBox1.SelectedItem.ToString();
            imagePaths[key] = "null";
            SaveIni();
        }

        private void 画像なしToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = null;          // 背景画像を削除
            panel1.BackColor = this.BackColor;
            panel1.Invalidate();                    // 再描画をリクエスト
            Properties.Settings.Default.lastimagefile = "";

            string key = comboBox1.SelectedItem.ToString();
            imagePaths[key] = "none";
            SaveIni();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void クリップへコピーToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string texter = "";
            texter = eventname.Text + "\r\n"
                 + current.Text + "\r\n"
                 + elapsed.Text + "\r\n"
                 + left.Text + "\r\n"
                 + duration.Text + "\r\n"
                 + start.Text + "\r\n"
                 + end.Text + "\r\n";
            Clipboard.SetText(texter);
        }

        private void tanspaciy_rate()
        {
            // 方法1: BackColorにアルファ値を設定する
            eventname.BackColor = Color.FromArgb(128, 255, 255, 255);  // 128が透明度（0-255）
                                                                       // 255は完全不透明、0は完全透明

            // 方法2: 前景色（文字色）を半透明にする
            eventname.ForeColor = Color.FromArgb(128, 0, 0, 0);  // 黒い文字を半透明に

            // 方法3: 背景と文字の両方を半透明にする例
            eventname.BackColor = Color.FromArgb(128, 255, 255, 255);  // 背景を半透明の白に
            eventname.ForeColor = Color.FromArgb(200, 0, 0, 0);        // 文字をやや透明な黒に
        }

        public void menu_align(int y, bool fontsize)
        {



            // インデックス0の項目のチェック状態を取得
            bool d_curr = Properties.Settings.Default.display_curr;
            bool d_el = Properties.Settings.Default.display_elapsed;
            bool d_lf = Properties.Settings.Default.display_left;
            bool d_sp = Properties.Settings.Default.display_span;
            bool d_st = Properties.Settings.Default.display_start;
            bool d_en = Properties.Settings.Default.display_end;


            int height = y;
            int base_x = 4;
            int base_y = 26;
            // フォントサイズに基づいて高さを調整
            if (fontsize)
            {
                height = TextRenderer.MeasureText("A", this.Font).Height;
            }

            eventname.Location = new Point(base_x, base_y);
            int tmp_counter = 1;

            string[] item = Properties.Settings.Default.display_order.Split(",");
            int ITEMMAX = 6;

            for (var i = 0; i < ITEMMAX; i++)
            {
                string s = item[i].ToString();

                if (s == "現在時刻")
                {
                    current.Location = new Point(base_x, base_y + height * tmp_counter);
                    if (d_curr) { tmp_counter++; }
                }
                if (s == "経過時間")
                {
                    elapsed.Location = new Point(base_x, base_y + height * tmp_counter);
                    if (d_el) { tmp_counter++; }
                }
                if (s == "残り時間")
                {
                    left.Location = new Point(base_x, base_y + height * tmp_counter);
                    if (d_lf) { tmp_counter++; }
                }
                if (s == "イベ期間")
                {
                    duration.Location = new Point(base_x, base_y + height * tmp_counter);
                    if (d_sp) { tmp_counter++; }
                }
                if (s == "開始時間")
                {
                    start.Location = new Point(base_x, base_y + height * tmp_counter);
                    if (d_st) { tmp_counter++; }
                }
                if (s == "終了時間")
                {
                    end.Location = new Point(base_x, base_y + height * tmp_counter);
                    if (d_en) { tmp_counter++; }
                }

            }
            height = TextRenderer.MeasureText("A", this.Font).Height;
            progressBar1.Location = new Point(base_x, end.Location.Y + height);
            parcent.Location = new Point(parcent.Location.X, end.Location.Y + height);

        }

        private void メニューを詰めるToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void バーの表示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            バーの表示ToolStripMenuItem.Checked = !バーの表示ToolStripMenuItem.Checked;
            progressBar1.Visible = !progressBar1.Visible;
            parcent.Visible = !parcent.Visible;
            Properties.Settings.Default.barvisible = バーの表示ToolStripMenuItem.Checked;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            menu_align(0, true);
            Properties.Settings.Default.font_margn = true;
        }


        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

            menu_align(50, false);
            Properties.Settings.Default.font_margn = false;
        }

        private void this_begin_update()
        {
            this.SuspendLayout(); // レイアウト更新を一時停止
            panel1.SuspendLayout();
            panel2.SuspendLayout();


        }
        private void this_end_update()
        {
            this.ResumeLayout(false); // レイアウト更新を再開
            panel1.ResumeLayout(false); // レイアウト更新を再開
            panel2.ResumeLayout(false); // レイアウト更新を再開

            panel1.Invalidate(); // 必要な部分だけを再描画
            panel2.Invalidate(); // 必要な部分だけを再描画

        }

        private void うえのいろToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this_begin_update();

            // 背景色と文字色の設定
            ToolStripMenuItem[] menuItems = {
              色の設定ToolStripMenuItem,
             バージョンToolStripMenuItem,
             netaToolStripMenuItem,
             外部つーるへエクスポートToolStripMenuItem,
                時刻設定ToolStripMenuItem
            };

            foreach (var item in menuItems)
            {
                item.BackColor = this.BackColor;
                item.ForeColor = this.BackColor;
            }

            menuStrip1.BackColor = this.BackColor;

            // フォーム設定の変更
            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            Properties.Settings.Default.use_upui_chroma = true;

            this_end_update();


        }

        private void もとに戻すToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this_begin_update();

            ToolStripMenuItem[] menuItems = {
        色の設定ToolStripMenuItem,
        バージョンToolStripMenuItem,
        netaToolStripMenuItem,
        外部つーるへエクスポートToolStripMenuItem,
        時刻設定ToolStripMenuItem
    };

            foreach (var item in menuItems)
            {
                item.BackColor = this.contextMenuStrip1.BackColor;
                item.ForeColor = Color.Black;
            }

            menuStrip1.BackColor = this.contextMenuStrip1.BackColor;

            // フォームのプロパティ変更
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.AllowTransparency = false;
            Properties.Settings.Default.use_upui_chroma = false;

            this_end_update();

        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ぱねる１似合わせるToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ぱねる１似合わせるToolStripMenuItem.Checked = !ぱねる１似合わせるToolStripMenuItem.Checked;
            Properties.Settings.Default.image_Stretch = ぱねる１似合わせるToolStripMenuItem.Checked;

            string ss = Properties.Settings.Default.lastimagefile;
            if (ss == "null" || ss == "none")
            {
                return;
            }

            try
            {
                // 画像を直接パネルの背景として設定
                panel1.BackgroundImage = System.Drawing.Image.FromFile(Properties.Settings.Default.lastimagefile);
                if (ぱねる１似合わせるToolStripMenuItem.Checked)
                {
                    panel1.BackgroundImageLayout = ImageLayout.Stretch; // 必要に応じて調整
                }
                else
                {
                    panel1.BackgroundImageLayout = ImageLayout.Tile;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 画像ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void wrong_encode_Click(object sender, EventArgs e)
        {

        }

        private void 縺化け_Click(object sender, EventArgs e)
        {
            縺化けUTF8SJIS.Checked = !縺化けUTF8SJIS.Checked;
        }

        private void コードページ指定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            コードページ指定.Checked = !コードページ指定.Checked;
            EncodingSelectForm cp = new EncodingSelectForm();
            cp.Show();

        }

        private void かすたむToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var form6 = new Form6(this);
            form6.ShowDialog();
            form6.Dispose();
        }

        private void 縺化け戻しSJISUTF8CP932CP65001ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            縺化け戻し.Checked = !縺化け戻し.Checked;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }





        public partial class EncodingSelectForm : Form
        {
            private ComboBox inputEncodingComboBox;
            private ComboBox outputEncodingComboBox;
            private Label inputLabel;
            private Label outputLabel;
            private Label selectedInputEncoding;
            private Label selectedOutputEncoding;

            public EncodingSelectForm()
            {
                InitializeComponent();
                InitializeEncodingSelectors();
            }

            private void InitializeComponent()
            {
                this.inputEncodingComboBox = new ComboBox();
                this.outputEncodingComboBox = new ComboBox();
                this.inputLabel = new Label();
                this.outputLabel = new Label();
                this.selectedInputEncoding = new Label();
                this.selectedOutputEncoding = new Label();

                // Input encoding controls
                this.inputLabel.Text = "入力エンコーディング:";
                this.inputLabel.Location = new System.Drawing.Point(10, 20);
                this.inputLabel.AutoSize = true;

                this.inputEncodingComboBox.Location = new System.Drawing.Point(10, 40);
                this.inputEncodingComboBox.Width = 200;
                this.inputEncodingComboBox.SelectedIndexChanged += new EventHandler(InputEncoding_SelectedIndexChanged);

                this.selectedInputEncoding.Location = new System.Drawing.Point(220, 43);
                this.selectedInputEncoding.AutoSize = true;

                // Output encoding controls
                this.outputLabel.Text = "出力エンコーディング:";
                this.outputLabel.Location = new System.Drawing.Point(10, 80);
                this.outputLabel.AutoSize = true;

                this.outputEncodingComboBox.Location = new System.Drawing.Point(10, 100);
                this.outputEncodingComboBox.Width = 200;
                this.outputEncodingComboBox.SelectedIndexChanged += new EventHandler(OutputEncoding_SelectedIndexChanged);

                this.selectedOutputEncoding.Location = new System.Drawing.Point(220, 103);
                this.selectedOutputEncoding.AutoSize = true;

                // Form settings
                this.Controls.AddRange(new Control[] {
            this.inputLabel,
            this.inputEncodingComboBox,
            this.selectedInputEncoding,
            this.outputLabel,
            this.outputEncodingComboBox,
            this.selectedOutputEncoding
        });
                this.ClientSize = new System.Drawing.Size(400, 150);
                this.Text = "エンコーディング選択";
            }

            private void InitializeEncodingSelectors()
            {
                // よく使用されるエンコーディングのリストを作成
                var encodings = new List<EncodingInfo>
        {
new EncodingInfo { DisplayName = "cp65001 utf-8	Unicode (UTF-8)", CodePage = 65001 },
new EncodingInfo { DisplayName = "cp57011 x-iscii-pa	ISCII パンジャブ語", CodePage = 57011 },
new EncodingInfo { DisplayName = "cp57010 x-iscii-gu	ISCII グジャラート語", CodePage = 57010 },
new EncodingInfo { DisplayName = "cp57009 x-iscii-ma	ISCII マラヤーラム語", CodePage = 57009 },
new EncodingInfo { DisplayName = "cp57008 x-iscii-ka	ISCII カンナダ語", CodePage = 57008 },
new EncodingInfo { DisplayName = "cp57007 x-iscii-または	ISCII Odia", CodePage = 57007 },
new EncodingInfo { DisplayName = "cp57006 x-iscii-as	ISCII アッサム語", CodePage = 57006 },
new EncodingInfo { DisplayName = "cp57005 x-iscii-te	ISCII テルグ語", CodePage = 57005 },
new EncodingInfo { DisplayName = "cp57004 x-iscii-ta	ISCII タミール語", CodePage = 57004 },
new EncodingInfo { DisplayName = "cp57003 x-iscii-be	ISCII Bangla", CodePage = 57003 },
new EncodingInfo { DisplayName = "cp57002 x-iscii-de	ISCII デバナガリ文字", CodePage = 57002 },
new EncodingInfo { DisplayName = "cp54936 GB18030	Windows XP 以降: GB18030 簡体字中国語 (4 バイト);簡体字中国語 (GB18030)", CodePage = 54936 },
new EncodingInfo { DisplayName = "cp52936 hz-gb-2312	HZ-GB2312 簡体字中国語;簡体字中国語 (HZ)", CodePage = 52936 },
new EncodingInfo { DisplayName = "cp51949 euc-韓国	EUC 韓国語", CodePage = 51949 },
new EncodingInfo { DisplayName = "cp51936 EUC-CN	EUC 簡体字中国語;簡体字中国語 (EUC)", CodePage = 51936 },
new EncodingInfo { DisplayName = "cp51932 euc-jp	EUC 日本語", CodePage = 51932 },
//new EncodingInfo { DisplayName = "cp65000 utf-7	Unicode (UTF-7)", CodePage = 65000 },
//new EncodingInfo { DisplayName = "cp51950 EUC 繁体字中国語	", CodePage = 51950 },
//new EncodingInfo { DisplayName = "cp50939 EBCDIC 日本語 (ラテン) 拡張および日本語	", CodePage = 50939 },
//new EncodingInfo { DisplayName = "cp50937 EBCDIC US-Canadaと繁体字中国語	", CodePage = 50937 },
//new EncodingInfo { DisplayName = "cp50936 EBCDIC 簡体字中国語	", CodePage = 50936 },
//new EncodingInfo { DisplayName = "cp50935 EBCDIC 簡体中国語 拡張中国語と簡体字中国語	", CodePage = 50935 },
//new EncodingInfo { DisplayName = "cp50933 EBCDIC 韓国語拡張および韓国語	", CodePage = 50933 },
//new EncodingInfo { DisplayName = "cp50931 EBCDIC US-Canadaと日本語	", CodePage = 50931 },
//new EncodingInfo { DisplayName = "cp50930 EBCDIC 日本語 (カタカナ) 拡張	", CodePage = 50930 },
//new EncodingInfo { DisplayName = "cp50229 ISO 2022 繁体字中国語	", CodePage = 50229 },
//new EncodingInfo { DisplayName = "cp720 DOS-720	アラビア語 (Transparent ASMO);アラビア語 (DOS)", CodePage = 720 },
//new EncodingInfo { DisplayName = "cp710 アラビア語 - 透明アラビア語	", CodePage = 710 },
//new EncodingInfo { DisplayName = "cp709 アラビア語 (ASMO-449 以降、BCON V4)	", CodePage = 709 },
//new EncodingInfo { DisplayName = "cp21027 (非推奨)	", CodePage = 21027 },
new EncodingInfo { DisplayName = "cp50227 x-cp50227	ISO 2022 簡体字中国語;簡体字中国語 (ISO 2022)", CodePage = 50227 },
new EncodingInfo { DisplayName = "cp50225 iso-2022-韓国	ISO 2022 韓国語", CodePage = 50225 },
new EncodingInfo { DisplayName = "cp50222 iso-2022-jp	ISO 2022 日本語 JIS X 0201-1989;日本語 (JIS-Allow 1 バイトかな - SO/SI)", CodePage = 50222 },
new EncodingInfo { DisplayName = "cp50221 csISO2022JP	ISO 2022 日本語(半角カタカナ)日本語 (JIS-Allow 1 バイトかな)", CodePage = 50221 },
new EncodingInfo { DisplayName = "cp50220 iso-2022-jp	半角カタカナを持たないISO 2022日本語;日本語 (JIS)", CodePage = 50220 },
new EncodingInfo { DisplayName = "cp38598 iso-8859-8-i	ISO 8859-8 ヘブライ語;ヘブライ語 (ISO-Logical)", CodePage = 38598 },
new EncodingInfo { DisplayName = "cp29001 x-Europa	ヨーロッパ 3", CodePage = 29001 },
new EncodingInfo { DisplayName = "cp28605 iso-8859-15	ISO 8859-15 Latin 9", CodePage = 28605 },
new EncodingInfo { DisplayName = "cp28603 iso-8859-13	ISO 8859-13 エストニア語", CodePage = 28603 },
new EncodingInfo { DisplayName = "cp28599 iso-8859-9	ISO 8859-3 トルコ語", CodePage = 28599 },
new EncodingInfo { DisplayName = "cp28598 iso-8859-8	ISO 8859-8 ヘブライ語;ヘブライ語 (ISO-Visual)", CodePage = 28598 },
new EncodingInfo { DisplayName = "cp28597 iso-8859-7	ISO 8859-7 ギリシャ語", CodePage = 28597 },
new EncodingInfo { DisplayName = "cp28596 iso-8859-6	ISO 8859-6 アラビア語", CodePage = 28596 },
new EncodingInfo { DisplayName = "cp28595 iso-8859-5	ISO 8859-5 キリル語", CodePage = 28595 },
new EncodingInfo { DisplayName = "cp28594 iso-8859-4	ISO 8859-4 バルティック", CodePage = 28594 },
new EncodingInfo { DisplayName = "cp28593 iso-8859-3	ISO 8859-3 ラテン 3", CodePage = 28593 },
new EncodingInfo { DisplayName = "cp28592 iso-8859-2	ISO 8859-2 中央ヨーロッパ;中央ヨーロッパ (ISO)", CodePage = 28592 },
new EncodingInfo { DisplayName = "cp28591 iso-8859-1	ISO 8859-1 ラテン 1;西ヨーロッパ語 (ISO)", CodePage = 28591 },
new EncodingInfo { DisplayName = "cp21866 koi8-u	ウクライナ語 (KOI8-U);キリル語 (KOI8-U)", CodePage = 21866 },
new EncodingInfo { DisplayName = "cp21025 cp1025	IBM EBCDIC キリル文字Serbian-Bulgarian", CodePage = 21025 },
new EncodingInfo { DisplayName = "cp20949 x-cp20949	韓国 Korean-wansung-unicode", CodePage = 20949 },
new EncodingInfo { DisplayName = "cp20936 x-cp20936	簡体字中国語 (GB2312);簡体字中国語 (GB2312-80)", CodePage = 20936 },
new EncodingInfo { DisplayName = "cp20932 EUC-JP	日本語 (JIS 0208-1990 および 0212-1990)", CodePage = 20932 },
new EncodingInfo { DisplayName = "cp20924 IBM00924	IBM EBCDIC Latin 1/Open System (1047 + ユーロ記号)", CodePage = 20924 },
new EncodingInfo { DisplayName = "cp20905 IBM905	IBM EBCDIC トルコ語", CodePage = 20905 },
new EncodingInfo { DisplayName = "cp20880 IBM880	IBM EBCDIC キリルロシア語", CodePage = 20880 },
new EncodingInfo { DisplayName = "cp20871 IBM871	IBM EBCDIC アイスランド語", CodePage = 20871 },
new EncodingInfo { DisplayName = "cp20866 koi8-r	ロシア語 (KOI8-R);キリル語 (KOI8-R)", CodePage = 20866 },
new EncodingInfo { DisplayName = "cp20838 IBM-タイ語	IBM EBCDIC タイ語", CodePage = 20838 },
new EncodingInfo { DisplayName = "cp20833 x-EBCDIC-KoreanExtended	IBM EBCDIC 韓国語拡張", CodePage = 20833 },
new EncodingInfo { DisplayName = "cp20424 IBM424	IBM EBCDIC ヘブライ語", CodePage = 20424 },
new EncodingInfo { DisplayName = "cp20423 IBM423	IBM EBCDIC ギリシャ語", CodePage = 20423 },
new EncodingInfo { DisplayName = "cp20420 IBM420	IBM EBCDIC アラビア語", CodePage = 20420 },
new EncodingInfo { DisplayName = "cp20297 IBM297	IBM EBCDIC France", CodePage = 20297 },
new EncodingInfo { DisplayName = "cp20290 IBM290	IBM EBCDIC 日本語カタカナ拡張", CodePage = 20290 },
new EncodingInfo { DisplayName = "cp20285 IBM285	IBM EBCDIC イギリス", CodePage = 20285 },
new EncodingInfo { DisplayName = "cp20284 IBM284	IBM EBCDIC Latin America-Spain", CodePage = 20284 },
new EncodingInfo { DisplayName = "cp20280 IBM280	IBM EBCDIC イタリア", CodePage = 20280 },
new EncodingInfo { DisplayName = "cp20278 IBM278	IBM EBCDIC Finland-Sweden", CodePage = 20278 },
new EncodingInfo { DisplayName = "cp20277 IBM277	IBM EBCDIC Denmark-Norway", CodePage = 20277 },
new EncodingInfo { DisplayName = "cp20273 IBM273	IBM EBCDIC Germany", CodePage = 20273 },
new EncodingInfo { DisplayName = "cp20269 x-cp20269	ISO 6937 スペースなしアクサン", CodePage = 20269 },
new EncodingInfo { DisplayName = "cp20261 x-cp20261	T. 61", CodePage = 20261 },
new EncodingInfo { DisplayName = "cp20127 us-ascii	US-ASCII (7 ビット)", CodePage = 20127 },
new EncodingInfo { DisplayName = "cp20108 x-IA5-ノルウェー語	IA5 ノルウェー語 (7 ビット)", CodePage = 20108 },
new EncodingInfo { DisplayName = "cp20107 x-IA5-スウェーデン語	IA5 スウェーデン語 (7 ビット)", CodePage = 20107 },
new EncodingInfo { DisplayName = "cp20106 x-IA5-ドイツ語	IA5 ドイツ語 (7 ビット)", CodePage = 20106 },
new EncodingInfo { DisplayName = "cp20105 x-IA5	IA5 (IRV International Alphabet No. 5, 7-bit);西ヨーロッパ (IA5)", CodePage = 20105 },
new EncodingInfo { DisplayName = "cp20005 x-cp20005	Wang 台湾", CodePage = 20005 },
new EncodingInfo { DisplayName = "cp20004 x-cp20004	文字放送 (台湾)", CodePage = 20004 },
new EncodingInfo { DisplayName = "cp20003 x-cp20003	IBM5550 台湾", CodePage = 20003 },
new EncodingInfo { DisplayName = "cp20002 x_Chinese-Eten	Eten Taiwan;繁体字中国語 (Eten)", CodePage = 20002 },
new EncodingInfo { DisplayName = "cp20001 x-cp20001	TCA 台湾", CodePage = 20001 },
new EncodingInfo { DisplayName = "cp20000 x-Chinese_CNS	CNS 台湾;繁体字中国語 (CNS)", CodePage = 20000 },
new EncodingInfo { DisplayName = "cp12001 utf-32	Unicode UTF-32、ビッグ エンディアン バイト順。マネージド アプリケーションでのみ使用できる", CodePage = 12001 },
new EncodingInfo { DisplayName = "cp12000 utf-8-32	Unicode UTF-32、リトル エンディアン バイト順。マネージド アプリケーションでのみ使用できる", CodePage = 12000 },
new EncodingInfo { DisplayName = "cp10082 x-mac-クロアチア語	クロアチア語 (Mac)", CodePage = 10082 },
new EncodingInfo { DisplayName = "cp10081 x-mac-トルコ語	トルコ語 (Mac)", CodePage = 10081 },
new EncodingInfo { DisplayName = "cp10079 x-mac-アイスランド語	アイスランド語 (Mac)", CodePage = 10079 },
new EncodingInfo { DisplayName = "cp10029 x-mac-ce	MAC ラテン 2;中央ヨーロッパ (Mac)", CodePage = 10029 },
new EncodingInfo { DisplayName = "cp10021 x-mac-タイ語	タイ語 (Mac)", CodePage = 10021 },
new EncodingInfo { DisplayName = "cp10017 x-mac-ウクライナ語	ウクライナ語 (Mac)", CodePage = 10017 },
new EncodingInfo { DisplayName = "cp10010 x-mac-ルーマニア語	ルーマニア語 (Mac)", CodePage = 10010 },
new EncodingInfo { DisplayName = "cp10008 chinesesimp	MAC 簡体字中国語 (GB 2312);簡体字中国語 (Mac)", CodePage = 10008 },
new EncodingInfo { DisplayName = "cp10007 x-mac-キリル文字	キリル語 (Mac)", CodePage = 10007 },
new EncodingInfo { DisplayName = "cp10006 x-mac-ギリシャ語	ギリシャ語 (Mac)", CodePage = 10006 },
new EncodingInfo { DisplayName = "cp10005 x-mac-ヘブライ語	ヘブライ語 (Mac)", CodePage = 10005 },
new EncodingInfo { DisplayName = "cp10004 x-mac-アラビア語	アラビア語 (Mac)", CodePage = 10004 },
new EncodingInfo { DisplayName = "cp10003 x-mac-韓国語	韓国語 (Mac)", CodePage = 10003 },
new EncodingInfo { DisplayName = "cp10002 chinesetrad	MAC 繁体字中国語 (Big5);繁体字中国語 (Mac)", CodePage = 10002 },
new EncodingInfo { DisplayName = "cp10001 x-mac-日本語	日本語 (Mac)", CodePage = 10001 },
new EncodingInfo { DisplayName = "cp10000 版	MAC Roman;西ヨーロッパ語 (Mac)", CodePage = 10000 },
new EncodingInfo { DisplayName = "cp1361 Johab	韓国語 (Johab)", CodePage = 1361 },
new EncodingInfo { DisplayName = "cp1258 windows-1258	ANSI/OEM ベトナム語;ベトナム語 (Windows)", CodePage = 1258 },
new EncodingInfo { DisplayName = "cp1257 windows-1257	ANSI バルト語;バルト語 (Windows)", CodePage = 1257 },
new EncodingInfo { DisplayName = "cp1256 windows-1256	ANSI アラビア語;アラビア語 (Windows)", CodePage = 1256 },
new EncodingInfo { DisplayName = "cp1255 windows-1255	ANSI ヘブライ語;ヘブライ語 (Windows)", CodePage = 1255 },
new EncodingInfo { DisplayName = "cp1254 windows-1254	ANSI トルコ語;トルコ語 (Windows)", CodePage = 1254 },
new EncodingInfo { DisplayName = "cp1253 windows-1253	ANSI ギリシャ語;ギリシャ語 (Windows)", CodePage = 1253 },
new EncodingInfo { DisplayName = "cp1252 windows-1252	ANSI ラテン 1;西ヨーロッパ (Windows)", CodePage = 1252 },
new EncodingInfo { DisplayName = "cp1251 windows-1251	ANSI キリル文字;キリル文字 (Windows)", CodePage = 1251 },
new EncodingInfo { DisplayName = "cp1250 windows-1250	ANSI 中央ヨーロッパ;中央ヨーロッパ (Windows)", CodePage = 1250 },
new EncodingInfo { DisplayName = "cp1201 unicodeFFFE	Unicode UTF-16、ビッグ エンディアン バイト順。マネージド アプリケーションでのみ使用できる", CodePage = 1201 },
new EncodingInfo { DisplayName = "cp1200 utf-16	Unicode UTF-16、リトル エンディアン バイト順 (ISO 10646 の BMP)。マネージド アプリケーションでのみ使用できる", CodePage = 1200 },
new EncodingInfo { DisplayName = "cp1149 IBM01149	IBM EBCDIC アイスランド語 (20871 + ユーロ記号);IBM EBCDIC (アイスランド語-ユーロ)", CodePage = 1149 },
new EncodingInfo { DisplayName = "cp1148 IBM01148	IBM EBCDIC International (500 + ユーロ記号);IBM EBCDIC (国際ユーロ)", CodePage = 1148 },
new EncodingInfo { DisplayName = "cp1147 IBM01147	IBM EBCDIC フランス (20297 + ユーロ記号);IBM EBCDIC (フランス-ユーロ)", CodePage = 1147 },
new EncodingInfo { DisplayName = "cp1146 IBM01146	IBM EBCDIC 英国 (20285 + ユーロ記号);IBM EBCDIC (UK-Euro)", CodePage = 1146 },
new EncodingInfo { DisplayName = "cp1145 IBM01145	IBM EBCDIC Latin America-Spain (20284 + ユーロ記号);IBM EBCDIC (スペイン-ユーロ)", CodePage = 1145 },
new EncodingInfo { DisplayName = "cp1144 IBM01144	IBM EBCDIC イタリア (20280 + ユーロ記号);IBM EBCDIC (イタリア-ユーロ)", CodePage = 1144 },
new EncodingInfo { DisplayName = "cp1143 IBM01143	IBM EBCDIC Finland-Sweden (20278 + ユーロ記号);IBM EBCDIC (フィンランド-スウェーデン-ユーロ)", CodePage = 1143 },
new EncodingInfo { DisplayName = "cp1142 IBM01142	IBM EBCDIC Denmark-Norway (20277 + ユーロ記号);IBM EBCDIC (デンマーク-ノルウェー-ユーロ)", CodePage = 1142 },
new EncodingInfo { DisplayName = "cp1141 IBM01141	IBM EBCDIC Germany (20273 + ユーロ記号);IBM EBCDIC (ドイツ-ユーロ)", CodePage = 1141 },
new EncodingInfo { DisplayName = "cp1140 IBM01140	IBM EBCDIC US-Canada (037 + ユーロ記号);IBM EBCDIC (US-Canada-Euro)", CodePage = 1140 },
new EncodingInfo { DisplayName = "cp1047 IBM01047	IBM EBCDIC Latin 1/Open システム", CodePage = 1047 },
new EncodingInfo { DisplayName = "cp1026 IBM1026	IBM EBCDIC トルコ語 (ラテン 5)", CodePage = 1026 },
new EncodingInfo { DisplayName = "cp950 big5	ANSI/OEM 繁体字中国語 (台湾;香港特別行政区(PRC);繁体字中国語 (Big5)", CodePage = 950 },
new EncodingInfo { DisplayName = "cp949 ks_c_5601-1987	ANSI/OEM 韓国語 (統合ハングル コード)", CodePage = 949 },
new EncodingInfo { DisplayName = "cp936 gb2312	ANSI/OEM 簡体字中国語 (PRC、シンガポール);簡体字中国語 (GB2312)", CodePage = 936 },
new EncodingInfo { DisplayName = "cp932 shift_jis	ANSI/OEM 日本語;日本語 (Shift-JIS)", CodePage = 932 },
new EncodingInfo { DisplayName = "cp875 cp875	IBM EBCDIC ギリシャ語モダン", CodePage = 875 },
new EncodingInfo { DisplayName = "cp874 windows-874	タイ語 (Windows)", CodePage = 874 },
new EncodingInfo { DisplayName = "cp870 IBM870	IBM EBCDIC 多言語/ROECE (ラテン 2);IBM EBCDIC 多言語ラテン語 2", CodePage = 870 },
new EncodingInfo { DisplayName = "cp869 ibm869	OEM モダン ギリシャ語;ギリシャ語、モダン (DOS)", CodePage = 869 },
new EncodingInfo { DisplayName = "cp866 cp866	OEM ロシア語;キリル語 (DOS)", CodePage = 866 },
new EncodingInfo { DisplayName = "cp865 IBM865	OEM ノルディック;ノルディック語 (DOS)", CodePage = 865 },
new EncodingInfo { DisplayName = "cp864 IBM864	OEM アラビア語;アラビア語 (864)", CodePage = 864 },
new EncodingInfo { DisplayName = "cp863 IBM863	OEM フランス語 (カナダ)フランス語 (カナダ) (DOS)", CodePage = 863 },
new EncodingInfo { DisplayName = "cp862 DOS-862	OEM ヘブライ語;ヘブライ語 (DOS)", CodePage = 862 },
new EncodingInfo { DisplayName = "cp861 ibm861	OEM アイスランド語;アイスランド語 (DOS)", CodePage = 861 },
new EncodingInfo { DisplayName = "cp860 IBM860	OEM ポルトガル語;ポルトガル語 (DOS)", CodePage = 860 },
new EncodingInfo { DisplayName = "cp858 IBM00858	OEM 多言語ラテン 1 + ユーロ記号", CodePage = 858 },
new EncodingInfo { DisplayName = "cp857 ibm857	OEM トルコ語;トルコ語 (DOS)", CodePage = 857 },
new EncodingInfo { DisplayName = "cp855 IBM855	OEM キリル語 (主にロシア語)", CodePage = 855 },
new EncodingInfo { DisplayName = "cp852 ibm852	OEM ラテン 2;中央ヨーロッパ (DOS)", CodePage = 852 },
new EncodingInfo { DisplayName = "cp850 ibm850	OEM 多言語ラテン 1;西ヨーロッパ語 (DOS)", CodePage = 850 },
new EncodingInfo { DisplayName = "cp775 ibm775	OEM バルト語;バルト語 (DOS)", CodePage = 775 },
new EncodingInfo { DisplayName = "cp737 ibm737	OEM ギリシャ語 (旧称 437G);ギリシャ語 (DOS)", CodePage = 737 },
new EncodingInfo { DisplayName = "cp708 ASMO-708	アラビア語 (ASMO 708)", CodePage = 708 },
new EncodingInfo { DisplayName = "cp500 IBM500	IBM EBCDIC International", CodePage = 500 },
new EncodingInfo { DisplayName = "cp437 IBM437	OEM 米国", CodePage = 437 },
new EncodingInfo { DisplayName = "cp37 IBM037	IBM EBCDIC US-Canada", CodePage = 37 },
new EncodingInfo { DisplayName = "cp0 OS default	", CodePage = 0 }        };

                // コンボボックスにエンコーディングを追加
                inputEncodingComboBox.DataSource = new List<EncodingInfo>(encodings);
                outputEncodingComboBox.DataSource = new List<EncodingInfo>(encodings);

                inputEncodingComboBox.DisplayMember = "DisplayName";
                outputEncodingComboBox.DisplayMember = "DisplayName";

                // デフォルトでUTF-8を選択
                inputEncodingComboBox.SelectedIndex = neta.Properties.Settings.Default.w_in_idx;
                outputEncodingComboBox.SelectedIndex = neta.Properties.Settings.Default.w_out_idx;
            }

            private void InputEncoding_SelectedIndexChanged(object sender, EventArgs e)
            {
                var selectedEncoding = (EncodingInfo)inputEncodingComboBox.SelectedItem;
                selectedInputEncoding.Text = $"CodePage: {selectedEncoding.CodePage}";
                neta.Properties.Settings.Default.wrong_encoder_in = selectedEncoding.CodePage;
                neta.Properties.Settings.Default.w_in_idx = inputEncodingComboBox.SelectedIndex;
            }

            private void OutputEncoding_SelectedIndexChanged(object sender, EventArgs e)
            {
                var selectedEncoding = (EncodingInfo)outputEncodingComboBox.SelectedItem;
                selectedOutputEncoding.Text = $"CodePage: {selectedEncoding.CodePage}";
                neta.Properties.Settings.Default.wrong_encoder_out = selectedEncoding.CodePage;
                neta.Properties.Settings.Default.w_out_idx = outputEncodingComboBox.SelectedIndex;
            }

            private class EncodingInfo
            {
                public string DisplayName { get; set; }
                public int CodePage { get; set; }
            }
        }

        class EncodingList
        {
            public static void ListAllEncodings()
            {
                // すべての利用可能なエンコーディングを取得
                EncodingInfo[] encodings = Encoding.GetEncodings();

                // エンコーディング情報を整理して表示
                Console.WriteLine("利用可能なエンコーディング一覧:");
                Console.WriteLine("CodePage\tName\tDisplayName");
                Console.WriteLine("----------------------------------------");

                foreach (EncodingInfo ei in encodings.OrderBy(e => e.CodePage))
                {
                    Encoding e = ei.GetEncoding();
                    Console.WriteLine($"{ei.CodePage}\t{ei.Name}\t{ei.DisplayName}");
                }

                Console.WriteLine($"\n合計: {encodings.Length} エンコーディング");
            }

            public static List<EncodingInfo> GetAllEncodingsAsList()
            {
                return Encoding.GetEncodings().OrderBy(e => e.CodePage).ToList();
            }
        }

        private void gemini20ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            imgtobase64s(Properties.Resources.gemini, "gemini_base64.use.css");
        }

        private void claude35ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imgtobase64s(Properties.Resources.claude, "claude_base64.use.css");
        }

        private void imgtobase64_Click(object sender, EventArgs e)
        {

            imgtobase64s(Properties.Resources.base64jpg, "image_base64.txt");
        }

        private static void imgtobase64s(string mode, string outpath)
        {

            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();

            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Properties.Settings.Default.lastimagefile);

            ofd.Filter = "すべての対応画像|*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.tiff;*.ico|" +
                     "BMP 画像 (*.bmp)|*.bmp|" +
                     "JPEG 画像 (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                     "GIF 画像 (*.gif)|*.gif|" +
                     "PNG 画像 (*.png)|*.png|" +
                     "TIFF 画像 (*.tiff)|*.tiff|" +
                     "ICO アイコン (*.ico)|*.ico";
            //"WebP 画像 (*.webp)|*.webp";

            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;


            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string s = ofd.FileName;

                    Image raw = System.Drawing.Image.FromFile(s);

                    ConvertToJpegWithQuality(s, "tmp", 90);
                    string ss = mode +
                            ConvertImageToBase64("tmp");
                    if (mode != Properties.Resources.base64jpg)
                    {
                        ss += ");\r\n}}";
                    }
                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter(outpath);
                    //Write a line of text
                    sw.WriteLine(ss);
                    //Close the file
                    sw.Close();
                }
                catch
                {

                }
            }
        }
        static string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

        static void ConvertToJpegWithQuality(string inputPath, string outputPath, int quality)
        {
            using (Image raw = Image.FromFile(inputPath))
            {
                // JPEGエンコーダーを取得
                ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders()
                    .FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                if (jpegCodec == null)
                {
                    throw new Exception("JPEGエンコーダーが見つかりません");
                }

                // エンコーダーのパラメータを設定（品質を指定）
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                // JPEGとして保存
                raw.Save(outputPath, jpegCodec, encoderParams);
            }
        }

        private void luascripttzselectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.TZJSON == "")
            {
                MessageBox.Show("tzdatebaseバイナリ" +
                    "が読まれてません");
                return;
            }


            string url = "https://raw.githubusercontent.com/sokudon/miku/refs/heads/master/neta/obsduration_timer_39usatz.lua";
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string text = wc.DownloadString(url);
            DateTime st;
            DateTime en;
            string sst = ",";
            string sen = ",";
            string format = "yyyy-MM-dd'T'HH:mm:ssZ";

            if (TryParseDateTimeCutom(startbox.Text, out st))
            {
                sst = st.ToUniversalTime().ToString(format);
            }
            if (TryParseDateTimeCutom(endbox.Text, out en))
            {
                sen = en.ToUniversalTime().ToString(format);
            }

            text = text.Replace("でれすて", ibemei.Text)
               .Replace("2020-04-30T12:00:00+09:00", sst)
               .Replace("2020-05-07T21:00:00+09:00", sen)
               .Replace("%H:%m:%s", "%d %hh:%mm:%ss")
               .Replace("%Y/%m/%d %H:%M:%S", "%Y-%m-%d(%a)%H:%M:%S(GMT%z)")
               .Replace("%T%n経過時間%K%n残り時間%L%nイベント時間%I%n現地時間%N%n日本時間%JST%n達成率%P%nS %S%nE %E%n%nSJ %SJ%nEJ %EJ%n%nSU %SU%nEU %EU", "NOW:%TZ\\nEND:%EE\\nLEFT:%L\\nSPAN:%I%\\n%T%P％\\n%Q")
               .Replace("bar\", 1", "bar\",2");

            string[] rp = [];

            text = ConvertTimeZoneData(text, rp);

            if (text == "")
            {


            }

            string tzst = Properties.Settings.Default.usetzdatabin.Replace("/", "_").Replace("/", "_");

            string path = $"obs_neta_timer_tz_{tzst}.lua";

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



        public static string ConvertTimeZoneData(string data, string[] rp)
        {
            try
            {
                // JSONパース
                TimeZoneData tzData = System.Text.Json.JsonSerializer.Deserialize<TimeZoneData>(Properties.Settings.Default.TZJSON);

                // TimeZoneTransitionsインスタンスを作成
                TimeZoneTransitions tzTransitions = new TimeZoneTransitions(
                    tzData.TransList,
                    tzData.Offsets,
                    tzData.Abbrs
                );




                string tzst = Properties.Settings.Default.usetzdatabin;
                // Replace timezone name
                data = data.Replace("America/Los_Angeles", tzst);
                data = data.Replace("ロサンゼルス", tzst);
                data = data.Replace("--//! github.com/moment/moment-timezone", "--converted tzif binary timezezone:");

                // Extract country/city name
                string cut_country = Regex.Replace(tzst, @"^.*?/", "").Replace("_", "");


                // Replace town name
                data = data.Replace("town_name = \"LA\"", $"town_name = \"{cut_country}\"");

                // Update report placeholders
                if (rp != null && rp.Length > 1)
                {
                    rp[1] = rp[1].Replace("%%K", "");
                    rp[1] = rp[1].Replace("%UTC", "%TZ");
                    rp[1] = rp[1].Replace("%EU", "%EE");
                }

                // 正規表現で --//! で始まる行を削除
                data = Regex.Replace(data, @"^--//!.*$\n?", "", RegexOptions.Multiline);

                // 新しいリストとして保存する場合
                //https://claude.ai/chat/4b396f51-3b68-46c0-b142-5be690ae83be
                string abbrsString = string.Join(",", tzTransitions.abbrs.Select(x => $"\"{x}\""));
                List<double> convertedOffsets = tzTransitions.offsets.Select(x => x * -60).ToList();
                List<long> convertedTrans = tzTransitions.transList.Select(x => x * 1000).ToList();
                string UTC = "\"UTC\",";
                string ut = "0,";
                string comma = ",";
                string y_st = Properties.Settings.Default.stfilter;
                string y_en = Properties.Settings.Default.enfilter;
                bool isUseFilter = Properties.Settings.Default.usefiler;
                if (isUseFilter == false)
                {
                    y_st = "";
                    y_en = "";
                }
                if (convertedTrans.Count < 2)
                {
                    UTC = "";
                    ut = "";
                    comma = "";
                    y_st = "";
                    y_en = "";
                }

                // Replace timezone abbreviations
                data = data.Replace("{PST PDT}",
                   "{" + $"{UTC}" + abbrsString.Trim('[', ']') + "}");

                // Replace until values
                data = data.Replace("{1520762400000 1541322000000}",
                    "{" + string.Join(",", convertedTrans).Trim('[', ']') + $"{comma}math.huge" + "}");

                // Replace offset values
                data = data.Replace("{480,420}",
                    "{" + $"{ut}" + string.Join(",", convertedOffsets) + "}");

                // Replace empty objects and null values
                data = data.Replace("{}", "{\"\"}")
                    .Replace("null}", "math_huge}")
                    .Replace("null", "nil");

                // Replace timezone information
                data = Regex.Replace(data, "%TIMEZONE",
                    $"%TZ %SS %EE TZbinary {y_st} {y_en} {tzst} のみ移植されてます(実験的)");

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }
        }

        private void 正月ミクさん_Click(object sender, EventArgs e)
        {
            正月ミクさん.Checked = true;
            お花見みくさん.Checked = false;
            星屑ハンターの双子.Checked = false;
            Properties.Settings.Default.syougautmiku = true;
            Properties.Settings.Default.ohanami_miku = false;
            Properties.Settings.Default.hosikuzuhunter = false;
            でふぉるとにもどす正月みくToolStripMenuItem_Click(sender, e);
        }

        private void 花見みくさん_Click(object sender, EventArgs e)
        {
            正月ミクさん.Checked = false;
            お花見みくさん.Checked = true;
            星屑ハンターの双子.Checked = false;
            Properties.Settings.Default.syougautmiku = false;
            Properties.Settings.Default.ohanami_miku = true;            
            Properties.Settings.Default.hosikuzuhunter = false;
            でふぉるとにもどす正月みくToolStripMenuItem_Click(sender, e);
        }

        private void 星屑ハンターの双子_Click(object sender, EventArgs e)
        {

            星屑ハンターの双子.Checked = true;
            お花見みくさん.Checked = false;
            正月ミクさん.Checked = false;
            Properties.Settings.Default.syougautmiku = false;
            Properties.Settings.Default.ohanami_miku = false;
            Properties.Settings.Default.hosikuzuhunter = true;
            でふぉるとにもどす正月みくToolStripMenuItem_Click(sender, e);
        }

        private void obssocket送信のみToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form7 = new Form7();
            form7.ShowDialog(this);
            form7.Dispose();
        }

        private void SetTimerInterval(int interval)
        {
            timer.Interval = interval; // タイマーの間隔を変更
            if (isSlideshowRunning)
            {
                timer.Stop();
                timer.Start(); // 間隔変更を即時反映
            }
        }


        private void LoadImages(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    MessageBox.Show("指定されたフォルダが存在しません: " + folderPath);
                    timer.Stop();
                    return;
                }

                imageFiles = Directory.GetFiles(folderPath, "*.jpg")
                    .Concat(Directory.GetFiles(folderPath, "*.png"))
                    .ToArray();

                if (imageFiles.Length == 0)
                {
                    MessageBox.Show("指定フォルダに画像が見つかりません");
                    timer.Stop();
                    return;
                }

                currentImageIndex = 0; // インデックスをリセット
                ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"画像の読み込み中にエラーが発生しました: {ex.Message}");
                timer.Stop();
            }
        }

        private void ToggleButton_Click(object sender, EventArgs e)
        {
            if (imageFiles == null || imageFiles.Length == 0) return;

            isSlideshowRunning = !isSlideshowRunning;
            スライドショー.Checked = isSlideshowRunning;
            Properties.Settings.Default.using_slideshow = isSlideshowRunning;

            if (isSlideshowRunning)
            {
                timer.Start();
                string folderPath = Path.GetDirectoryName(Properties.Settings.Default.lastimagefile);
                LoadImages(folderPath); // 新しいフォルダの画像を読み込む
            }
            else
            {
                timer.Stop();
            }
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            currentImageIndex++;
            if (currentImageIndex >= imageFiles.Length)
                currentImageIndex = 0;

            ShowImage();
        }

        private void ShowImage()
        {
            if (Properties.Settings.Default.lastimagefile == "null" || Properties.Settings.Default.lastimagefile == "none")
            {
                return;
            }

            try
            {
                if (imageFiles != null && imageFiles.Length > 0)
                {
                    panel1.BackgroundImage?.Dispose();
                    panel1.BackgroundImage = Image.FromFile(imageFiles[currentImageIndex]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"画像の表示に失敗しました: {ex.Message}");
            }
        }

        private void 表示間隔ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var IntervalForm = new IntervalForm(timer.Interval);
            IntervalForm.ShowDialog(this);
            IntervalForm.Dispose();
            timer.Interval = Properties.Settings.Default.slidershow_interval;
            SetTimerInterval(timer.Interval);
        }

    }


    public partial class IntervalForm : Form
    {
        private ComboBox intervalComboBox;
        private Button okButton;
        private Button cancelButton;

        public int SelectedInterval { get; private set; } // 選択された間隔

        public IntervalForm(int currentInterval)
        {
            InitializeComponent();
            SetupControls(currentInterval);
        }

        private void SetupControls(int currentInterval)
        {
            this.Text = "タイマー間隔の設定";
            this.Width = 300;
            this.Height = 150;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // ラベル
            Label label = new Label
            {
                Text = "表示間隔を選択してください（ミリ秒）:",
                Location = new Point(10, 20),
                AutoSize = true
            };

            // コンボボックス
            intervalComboBox = new ComboBox
            {
                Location = new Point(10, 50),
                Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            intervalComboBox.Items.AddRange(new object[] { "1000 (1秒)", "3000 (3秒)", "5000 (5秒)", "10000 (10秒)" });
            intervalComboBox.SelectedIndex = GetIndexFromInterval(currentInterval);

            // OKボタン
            okButton = new Button
            {
                Text = "OK",
                Location = new Point(110, 80),
                DialogResult = DialogResult.OK
            };
            okButton.Click += OkButton_Click;

            // キャンセルボタン
            cancelButton = new Button
            {
                Text = "キャンセル",
                Location = new Point(190, 80),
                DialogResult = DialogResult.Cancel
            };

            // コントロールを追加
            this.Controls.Add(label);
            this.Controls.Add(intervalComboBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        private int GetIndexFromInterval(int interval)
        {
            switch (interval)
            {
                case 1000: return 0;
                case 3000: return 1;
                case 5000: return 2;
                case 10000: return 3;
                default: return 1; // デフォルトは3秒
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            string selectedText = intervalComboBox.SelectedItem.ToString();
            SelectedInterval = int.Parse(selectedText.Split(' ')[0]); // "1000 (1秒)" から "1000" を抽出
            Properties.Settings.Default.slidershow_interval = SelectedInterval;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(284, 111);
            this.Name = "IntervalForm";
            this.ResumeLayout(false);
        }
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


