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
using System.Collections.ObjectModel;
using System.Globalization;
using neta.Properties;
using System.Buffers.Binary;
using System.Security.Cryptography;
using static System.TimeZoneInfo;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using static System.Windows.Forms.DataFormats;
using Codeplex.Data;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;


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
                    comboBox2.Items.Add(z.DisplayName + " - " + z.Id);
                }
            }


            textBox1.Text = Properties.Settings.Default.lefttimeformat;
            textBox2.Text = Properties.Settings.Default.datetimeformat;
            checkBox1.Checked = Properties.Settings.Default.useutc;
            checkBox2.Checked = Properties.Settings.Default.usems;
            comboBox1.Text = Properties.Settings.Default.useutczone;
            comboBox2.Text = Properties.Settings.Default.msstring;
            comboBox3.Text = Properties.Settings.Default.barlen.ToString();
            comboBox5.Text = Properties.Settings.Default.api;
            textBox4.Text = Properties.Settings.Default.parse;
            comboBox4.Text = Properties.Settings.Default.usetzdatabin;
            textBox5.Text = Properties.Settings.Default.lasttzdatapath;
            checkBox3.Checked = Properties.Settings.Default.usetz;
            checkBox4.Checked = Properties.Settings.Default.usefiler;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Properties.Settings.Default.lasttzdatapath;
                openFileDialog.Filter = "すべてのファイル (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 選択したファイルのパスを取得
                    string filePath = Path.GetDirectoryName(openFileDialog.FileName);

                    Properties.Settings.Default.lasttzdatapath = filePath;
                    textBox5.Text = filePath;
                }
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.usetzdatabin = comboBox4.Text;
            checkBox3_CheckedChanged(sender, e);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.usetz = checkBox3.Checked;
            if (checkBox3.Checked == true)
            {
                if (this.Width < 500)
                {
                    this.Width = this.Width + 450;
                }
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {

                    string tzdata = Path.Combine(Properties.Settings.Default.lasttzdatapath, Properties.Settings.Default.usetzdatabin);
                    if (File.Exists(tzdata))
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

            string tzdata = Path.Combine(Properties.Settings.Default.lasttzdatapath, Properties.Settings.Default.usetzdatabin);
            string tzst = (Properties.Settings.Default.usetzdatabin);
            byte[] bs = new byte[data.Length];
            Array.ConstrainedCopy(data, 0, bs, 0, data.Length);

            byte[] TZifnext = new byte[4];
            int pos = index;
            Array.ConstrainedCopy(bs, pos, TZifnext, 0, 4);
            var encoding = Encoding.GetEncoding(0);
            var header2 = encoding.GetString(TZifnext).Substring(0, 4);
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

            int tzh_ttisgmtcntn = 0;
            int tzh_ttisstdcntn = 0;
            int tzh_leapcntn = 0;
            int tzh_timecntn = 0;
            int tzh_typecntn = 0;
            int tzh_charcntn = 0;

            if (header2.Contains("TZif") == false)
            {
                sb.AppendLine("2番目のZICバイナリ,TZifはありません");
                sb.AppendLine();
            }
            else
            {
                byte[] v = new byte[] { bs[pos + 4] };
                string versionn = encoding.GetString(v);

                if (bs[pos + 4] == 0)
                {
                    sb.AppendLine("2番目のZICバイナリ,TZif version NULL(1) はMUST BE IGNOREです");
                    sb.AppendLine();
                }
                else
                {

                    pos += 20;
                    tzh_ttisgmtcntn = BitConverter.ToInt32(Bigval(bs, pos), 0);

                    tzh_ttisstdcntn = BitConverter.ToInt32(Bigval(bs, pos + 4), 0);

                    tzh_leapcntn = BitConverter.ToInt32(Bigval(bs, pos + 8), 0);

                    tzh_timecntn = BitConverter.ToInt32(Bigval(bs, pos + 12), 0);

                    tzh_typecntn = BitConverter.ToInt32(Bigval(bs, pos + 16), 0);

                    tzh_charcntn = BitConverter.ToInt32(Bigval(bs, pos + 20), 0);


                    //C#  transition_timeが64bitのため
                    long[] transition_timesn = new long[tzh_timecntn];
                    int[] transition_typesn = new int[tzh_timecntn];

                    pos += 24;
                    if (tzh_timecntn != 0)
                    {
                        for (int i = 0; i < tzh_timecntn; i++)
                        {
                            transition_timesn[i] = BitConverter.ToInt64(Bigval64(bs, pos + 8 * i), 0);
                            transition_typesn[i] = bs[pos + tzh_timecntn * 8 + i];
                        }
                    }
                    else
                    {
                        transition_timesn = [];
                        transition_typesn = [];
                    }

                    pos += tzh_timecntn * 9;
                    int[] local_time_types_gmtn = new int[tzh_typecntn];
                    int[] local_time_types_isdstn = new int[tzh_typecntn];
                    int[] local_time_types_abbrn = new int[tzh_typecntn];

                    for (int i = 0; i < tzh_typecntn; i++)
                    {
                        local_time_types_gmtn[i] = BitConverter.ToInt32(Bigval(bs, pos + i * 6), 0);
                        local_time_types_isdstn[i] = bs[pos + 4 + i * 6];
                        local_time_types_abbrn[i] = bs[pos + 5 + i * 6];
                    }

                    pos = pos + 6 * tzh_typecntn;
                    byte[] abbr2 = new byte[tzh_charcntn + 10];
                    Array.ConstrainedCopy(bs, pos, abbr2, 0, tzh_charcntn);


                    sb.AppendLine("2ndTZif transitions,gmtoffset,isdst,abbr");
                    pos = pos + tzh_charcntn;
                    if (tzh_leapcntn > 0)
                    {
                        pos = pos + tzh_leapcntn * 8;
                    }


                    if (tzh_ttisstdcntn > 0)
                    {
                        //isstd = struct.unpack(">%db" % ttisstdcnt fileobj.read(ttisstdcnt))
                        byte[] isstd = new byte[tzh_ttisstdcntn];
                        Array.ConstrainedCopy(bs, pos, isstd, 0, tzh_ttisstdcntn);
                        pos += tzh_ttisstdcntn;
                    }

                    if (tzh_ttisgmtcntn > 0)
                    {
                        //isgmt = struct.unpack(">%db" % ttisgmtcnt, fileobj.read(ttisgmtcnt))
                        byte[] isgmt = new byte[tzh_ttisgmtcntn];
                        Array.ConstrainedCopy(bs, pos, isgmt, 0, tzh_ttisgmtcntn);
                        pos += tzh_ttisgmtcntn;

                    }


                    js.Append(@"""Zone"":" + @"""TZST"",");
                    js.Append(@"""TransList"":" + @"[0],");
                    js.Append(@"""Offsets"":" + @"[1],");
                    js.Append(@"""Abbrs"":" + @"[3]");
                    bool filter = Properties.Settings.Default.usefiler;
                    int max = tzh_timecntn - 1;

                    string[][] transitionsn = new string[tzh_timecntn][];
                    for (int i = 0; i < tzh_timecntn; i++)
                    {
                        int type = transition_typesn[i];
                        transitionsn[i] = new string[4];
                        transitionsn[i][0] = transition_timesn[i].ToString();
                        transitionsn[i][1] = Convert.ToString(local_time_types_gmtn[type]);
                        transitionsn[i][2] = Convert.ToString(local_time_types_isdstn[type]);

                        byte[] tmp2 = new byte[20];
                        Array.ConstrainedCopy(abbr2, local_time_types_abbrn[type], tmp2, 0, 10);
                        char[] charArray = ByteArrayToCharArray(tmp2, Encoding.UTF8);
                        transitionsn[i][3] = TerminateAtNull(charArray);

                        if (filter == false)
                        {
                            tr = tr + transitionsn[i][0] + @",";
                            of = of + Convert.ToString(local_time_types_gmtn[type] / 3600) + @",";
                            ab = ab + @"""" + transitionsn[i][3] + @""",";
                        }
                        else
                        {
                            TimeSpan utcOffset = TimeSpan.FromHours(local_time_types_gmtn[type] / 3600);
                            // DateTimeOffsetに変換
                            DateTimeOffset dateTimeWithOffset = DateTimeOffset.FromUnixTimeSeconds(transition_timesn[i]).ToOffset(utcOffset);

                            // オフセット付きのフォーマットに変換
                            string y = dateTimeWithOffset.ToString("yyyy");
                            int YY = Convert.ToInt32(y);
                            if (YY >= Y_filter_st && YY <= Y_filter_en)
                            {
                                tr = tr + transitionsn[i][0] + @",";
                                of = of + Convert.ToString(local_time_types_gmtn[type] / 3600) + @",";
                                ab = ab + @"""" + transitionsn[i][3] + @""",";
                            }
                        }

                        if (true)
                        {
                            transitionsn[i][1] = Convert.ToString(local_time_types_gmtn[type] / 3600);
                            // UTCオフセット (例: +09:00)
                            TimeSpan utcOffset = TimeSpan.FromHours(local_time_types_gmtn[type] / 3600);
                            // DateTimeOffsetに変換
                            DateTimeOffset dateTimeWithOffset = DateTimeOffset.FromUnixTimeSeconds(transition_timesn[i]).ToOffset(utcOffset);

                            // オフセット付きのフォーマットに変換
                            string formattedDate = dateTimeWithOffset.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
                            transitionsn[i][0] = formattedDate;

                        }
                        sb.Append(transitionsn[i][0]);
                        sb.Append(",");
                        sb.Append(transitionsn[i][1]);
                        sb.Append(",");
                        sb.Append(transitionsn[i][2]);
                        sb.Append(",");
                        sb.AppendLine(transitionsn[i][3]);


                    }
                    if (tr == "")
                    {
                        if (tzh_timecntn > 0)
                        {
                            tr = tr + transition_timesn[max].ToString() + @",";
                            of = of + transitionsn[max][1] + @",";
                            ab = ab + @"""" + transitionsn[max][3] + @""",";
                            finaltz = Y_filter_st +"～"+ Y_filter_en +"年期間内にはゾーン情報存在しませんが、から配列回避のため最後のゾーン情報を追加しています(から配列＝UTCになるため)";
                        }
                    }
                    else
                    {
                        finaltz = "カッティングしたゾーン情報になっています" +Y_filter_st + "～" + Y_filter_en + "年期間内のみ";

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
            sb.AppendLine();
            sb.AppendLine(mkjson);


            // JSONパース
            TimeZoneData tzData = JsonSerializer.Deserialize<TimeZoneData>(mkjson);
            Properties.Settings.Default.TZJSON = mkjson;

            // TimeZoneTransitionsインスタンスを作成
            TimeZoneTransitions tzTransitions = new TimeZoneTransitions(
                tzData.TransList,
                tzData.Offsets,
                tzData.Abbrs
            );
            string tester = Properties.Settings.Default.datetester;
            DateTime testDateTime;

            // TryParseを使って文字列をDateTimeに変換
            if (DateTime.TryParse(tester, out testDateTime))
            {
            

            int lastTransitionIdx = tzTransitions.FindLastTransition(testDateTime);

            string localt = testDateTime.ToString();
            string utct = testDateTime.ToUniversalTime().ToString();


            if (lastTransitionIdx >= 0)
            {
                int uo = tzData.Offsets[lastTransitionIdx];
                string abb = tzData.Abbrs[lastTransitionIdx];
                string iso8601tz = testDateTime.ToUniversalTime().AddHours(uo).ToString("yyyy-MM-dd'T'HH:mm:ss" + abb);

                sb.AppendLine(finaltz);
                sb.AppendLine("local utc tzdate:" + localt + "\r\n" + utct + "\r\n" + iso8601tz);
                sb.AppendLine($"Last transition index: {lastTransitionIdx}");
                sb.AppendLine($"Timestamp: {tzData.TransList[lastTransitionIdx]}");
                sb.AppendLine($"Offset: {tzData.Offsets[lastTransitionIdx]}");
                sb.AppendLine($"Abbreviation: {tzData.Abbrs[lastTransitionIdx]}");
            }
            else
            {
                string iso8601tz = testDateTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ssUTC");
                sb.AppendLine("期間内に偏移ファイルがみつかりませんでした、暫定でUTCになります");
                sb.AppendLine("local utc tzdate:" + localt + "\r\n" + utct + "\r\n" + iso8601tz);
            }
            }
            else
            {
                sb.AppendLine("日付てすとのパースに失敗しました、正しい日付を入力してください");
            }


            return sb.ToString();


        }

        public class TimeZoneData
        {
            public string Zone { get; set; }
            public List<long> TransList { get; set; }
            public List<int> Offsets { get; set; }
            public List<string> Abbrs { get; set; }
        }

        public class TimeZoneTransitions
        {
            private List<long> transList;
            private List<int> offsets;
            private List<string> abbrs;

            public TimeZoneTransitions(List<long> transList, List<int> offsets, List<string> abbrs)
            {
                this.transList = transList ?? new List<long>();
                this.offsets = offsets ?? new List<int>();
                this.abbrs = abbrs ?? new List<string>();
            }

            public int FindLastTransition(DateTime dt, bool inUtc = false)
            {
                if (transList == null || transList.Count == 0)
                {
                    return -1; // No transitions available
                }

                // Convert DateTime to a Unix timestamp
                long timestamp = DateTimeToUnixTimestamp(dt);

                // Find the index where the timestamp would fit
                //int idx = transList.BinarySearch(timestamp);
                // if (idx < 0)     { idx = ~idx; // Adjust index if not found  }
                int idx= BisectRight(transList, timestamp);


                return idx - 1; // Return the index of the previous transition
            }

            //C#とpythonの2分検索はあるごがちがう
            //https://chatgpt.com/c/6759448c-e904-800f-ad63-213ed43db5e0
            public static int BisectRight(List<long> list, long value)
            {
                int low = 0, high = list.Count;

                while (low < high)
                {
                    int mid = (low + high) / 2;

                    if (list[mid] > value)
                    {
                        high = mid;
                    }
                    else
                    {
                        low = mid + 1;
                    }
                }

                return low;
            }

            private long DateTimeToUnixTimestamp(DateTime dt)
            {
                DateTime utcDateTime = dt.ToUniversalTime();
                DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return (long)(utcDateTime - unixEpoch).TotalSeconds;
            }
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


        public static byte[] Bigval64(byte[] bs, int pos)
        {
            byte[] swapbin = new byte[8];
            Array.ConstrainedCopy(bs, pos, swapbin, 0, 8);
            Array.Reverse(swapbin);
            return swapbin;
        }

        public static byte[] Bigval(byte[] bs, int pos)
        {
            byte[] swapbin = new byte[4];
            Array.ConstrainedCopy(bs, pos, swapbin, 0, 4);
            Array.Reverse(swapbin);
            return swapbin;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            var url = "https?://[ -_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$";

            var m = Regex.Match(comboBox5.Text, url);
            if (m.Success)
            {
                Properties.Settings.Default.api = m.Value;
            }

        }

        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            var url = "https?://[ -_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$";

            var m = Regex.Match(comboBox5.Text, url);
            if (m.Success)
            {
                Properties.Settings.Default.api = m.Value;
            }
            var path = Properties.Settings.Default.api.ToString();
            if (File.Exists(path))
            {
                Properties.Settings.Default.api = path;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            ofd.FileName = "default.html";
            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = Properties.Settings.Default.lastfile;
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "jsonファイル(*.json)|*.json|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに選択されるものを指定する
            //2番目の「すべてのファイル」が選択されているようにする
            ofd.FilterIndex = 2;
            //タイトルを設定する
            ofd.Title = "開くファイルを選択してください";
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
                comboBox5.Text = ofd.FileName;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.usefiler = checkBox4.Checked;
            checkBox3_CheckedChanged(sender, e);
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.enfilter = comboBox8.Text;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.stfilter = comboBox7.Text;
            int st = Convert.ToInt32(comboBox7.Text);
            int en = Convert.ToInt32(comboBox8.Text);
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            DateTime d;
            if (DateTime.TryParse(textBox6.Text, out d)) { 
                Properties.Settings.Default.datetester = textBox6.Text;
            }
        }

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
    }
}
