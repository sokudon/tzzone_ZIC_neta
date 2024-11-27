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
            textBox3.Text = Properties.Settings.Default.api;
            textBox4.Text = Properties.Settings.Default.parse;
            comboBox4.Text = Properties.Settings.Default.usetzdatabin;
            textBox5.Text = Properties.Settings.Default.lasttzdatapath;
            checkBox3.Checked=Properties.Settings.Default.usetz  ;
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
                else
                {

                }
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.usetzdatabin = comboBox4.Text;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.usetz = checkBox3.Checked;
            if (checkBox3.Checked == true)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {

                    string tzdata = Path.Combine(Properties.Settings.Default.lasttzdatapath, Properties.Settings.Default.usetzdatabin);
                    if (File.Exists(tzdata))
                    {
                        System.IO.FileStream fs = new FileStream(tzdata, FileMode.Open, FileAccess.Read);
                        byte[] bs = new byte[fs.Length];
                        fs.Read(bs, 0, bs.Length);
                        fs.Close();

                        ZTimeZoneInfo(bs, Properties.Settings.Default.usetzdatabin,false);
                        return;
                    }
                }
            }
        }


        private static bool IsUtcAlias(string id)
        {
            switch ((ushort)id[0])
            {
                case 69: // e
                case 101: // E
                    return string.Equals(id, "Etc/UTC", StringComparison.OrdinalIgnoreCase) ||
                           string.Equals(id, "Etc/UCT", StringComparison.OrdinalIgnoreCase) ||
                           string.Equals(id, "Etc/Universal", StringComparison.OrdinalIgnoreCase) ||
                           string.Equals(id, "Etc/Zulu", StringComparison.OrdinalIgnoreCase);
                case 85: // u
                case 117: // U
                    return string.Equals(id, "UCT", StringComparison.OrdinalIgnoreCase) ||
                           string.Equals(id, "UTC", StringComparison.OrdinalIgnoreCase) ||
                           string.Equals(id, "Universal", StringComparison.OrdinalIgnoreCase);
                case 90: // z
                case 122: // Z
                    return string.Equals(id, "Zulu", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        private void ZTimeZoneInfo(byte[] data, string id, bool dstDisabled)
        {
            string _id = id;

           bool HasIanaId = true;
            TimeSpan _baseUtcOffset=TimeSpan.Zero;
            AdjustmentRule[] _adjustmentRules = Array.Empty<AdjustmentRule>();

            if (IsUtcAlias(id))
            {
                //_baseUtcOffset = TimeSpan.Zero;
                //AdjustmentRule[] _adjustmentRules = Array.Empty<AdjustmentRule>();
                return;
            }

            DateTime[] dts;
            byte[] typeOfLocalTime;
            TZifType[] transitionType;
            string zoneAbbreviations;
            string futureTransitionsPosixFormat;
            string _standardAbbrevName;
            string _daylightAbbrevName;

            // parse the raw TZif bytes; this method can throw ArgumentException when the data is malformed.
            TZif_ParseRaw(data, out dts, out typeOfLocalTime, out transitionType, out zoneAbbreviations, out futureTransitionsPosixFormat);

            // find the best matching baseUtcOffset and display strings based on the current utcNow value.
            // NOTE: read the Standard and Daylight display strings from the tzfile now in case they can't be loaded later
            // from the globalization data.
            DateTime utcNow = DateTime.UtcNow;
            for (int i = 0; i < dts.Length && dts[i] <= utcNow; i++)
            {
                int type = typeOfLocalTime[i];
                if (!transitionType[type].IsDst)
                {
                  _baseUtcOffset = transitionType[type].UtcOffset;
                 _standardAbbrevName = TZif_GetZoneAbbreviation(zoneAbbreviations, transitionType[type].AbbreviationIndex);
                }
                else
                {
                   _daylightAbbrevName = TZif_GetZoneAbbreviation(zoneAbbreviations, transitionType[type].AbbreviationIndex);
                }
            }

            if (dts.Length == 0)
            {
                // time zones like Africa/Bujumbura and Etc/GMT* have no transition times but still contain
                // TZifType entries that may contain a baseUtcOffset and display strings
                for (int i = 0; i < transitionType.Length; i++)
                {
                    if (!transitionType[i].IsDst)
                    {
                        _baseUtcOffset = transitionType[i].UtcOffset;
                        _standardAbbrevName = TZif_GetZoneAbbreviation(zoneAbbreviations, transitionType[i].AbbreviationIndex);
                    }
                    else
                    {
                        _daylightAbbrevName = TZif_GetZoneAbbreviation(zoneAbbreviations, transitionType[i].AbbreviationIndex);
                    }
                }
            }

            // TZif supports seconds-level granularity with offsets but TimeZoneInfo only supports minutes since it aligns
            // with DateTimeOffset, SQL Server, and the W3C XML Specification
            if (_baseUtcOffset.Ticks % TimeSpan.TicksPerMinute != 0)
            {
                _baseUtcOffset = new TimeSpan(_baseUtcOffset.Hours, _baseUtcOffset.Minutes, 0);
            }

            if (!dstDisabled)
            {
                // only create the adjustment rule if DST is enabled
                //TZif_GenerateAdjustmentRules(out _adjustmentRules, _baseUtcOffset, dts, typeOfLocalTime, transitionType, futureTransitionsPosixFormat);
            }

            //ValidateTimeZoneInfo(_id, _baseUtcOffset, _adjustmentRules, out _supportsDaylightSavingTime);
        }


        private static void TZif_GenerateAdjustmentRules(out AdjustmentRule[]? rules, TimeSpan baseUtcOffset, DateTime[] dts, byte[] typeOfLocalTime,
    TZifType[] transitionType, string? futureTransitionsPosixFormat)
        {
            rules = null;

            if (dts.Length > 0)
            {
                int index = 0;
                List<AdjustmentRule> rulesList = new List<AdjustmentRule>();

                while (index <= dts.Length)
                {
                    TZif_GenerateAdjustmentRule(ref index, baseUtcOffset, rulesList, dts, typeOfLocalTime, transitionType, futureTransitionsPosixFormat);
                }

                rules = rulesList.ToArray();
                if (rules != null && rules.Length == 0)
                {
                    rules = null;
                }
            }
        }

        private static readonly TransitionTime s_daylightRuleMarker = TransitionTime.CreateFixedDateRule(DateTime.MinValue.AddMilliseconds(2), 1, 1);

        // Truncate the date and the time to Milliseconds precision
        private static DateTime GetTimeOnlyInMillisecondsPrecision(DateTime input) => new DateTime((input.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond);

 
        private static void TZif_GenerateAdjustmentRule(ref int index, TimeSpan timeZoneBaseUtcOffset, List<AdjustmentRule> rulesList, DateTime[] dts,
            byte[] typeOfLocalTime, TZifType[] transitionTypes, string? futureTransitionsPosixFormat)
        {
    
            while (index < dts.Length && dts[index] == DateTime.MinValue)
            {
                index++;
            }

            if (rulesList.Count == 0 && index < dts.Length)
            {
                TZifType transitionType = TZif_GetEarlyDateTransitionType(transitionTypes);
                DateTime endTransitionDate = dts[index];

                TimeSpan transitionOffset = TZif_CalculateTransitionOffsetFromBase(transitionType.UtcOffset, timeZoneBaseUtcOffset);
                TimeSpan daylightDelta = transitionType.IsDst ? transitionOffset : TimeSpan.Zero;
                TimeSpan baseUtcDelta = transitionType.IsDst ? TimeSpan.Zero : transitionOffset;

                //TimeZoneInfo palmer = TimeZoneInfo.CreateCustomTimeZone(standardName, offset, displayName, standardName,daylightName, adjustments, true);
                AdjustmentRule r = AdjustmentRule.CreateAdjustmentRule(
                        DateTime.MinValue,
                        endTransitionDate.AddTicks(-1),
                        daylightDelta,
                        default,
                        default,
                        baseUtcDelta);
                        //noDaylightTransitions: true);

                //if (!IsValidAdjustmentRuleOffset(timeZoneBaseUtcOffset, r))
                //{
                //    NormalizeAdjustmentRuleOffset(timeZoneBaseUtcOffset, ref r);
                //}

                rulesList.Add(r);
            }
            else if (index < dts.Length)
            {
                DateTime startTransitionDate = dts[index - 1];
                TZifType startTransitionType = transitionTypes[typeOfLocalTime[index - 1]];

                DateTime endTransitionDate = dts[index];

                TimeSpan transitionOffset = TZif_CalculateTransitionOffsetFromBase(startTransitionType.UtcOffset, timeZoneBaseUtcOffset);
                TimeSpan daylightDelta = startTransitionType.IsDst ? transitionOffset : TimeSpan.Zero;
                TimeSpan baseUtcDelta = startTransitionType.IsDst ? TimeSpan.Zero : transitionOffset;

                TransitionTime dstStart;
                if (startTransitionType.IsDst)
                {
                    // the TransitionTime fields are not used when AdjustmentRule.NoDaylightTransitions == true.
                    // However, there are some cases in the past where DST = true, and the daylight savings offset
                    // now equals what the current BaseUtcOffset is.  In that case, the AdjustmentRule.DaylightOffset
                    // is going to be TimeSpan.Zero.  But we still need to return 'true' from AdjustmentRule.HasDaylightSaving.
                    // To ensure we always return true from HasDaylightSaving, make a "special" dstStart that will make the logic
                    // in HasDaylightSaving return true.
                    dstStart = s_daylightRuleMarker;
                }
                else
                {
                    dstStart = default;
                }

                AdjustmentRule r = AdjustmentRule.CreateAdjustmentRule(
                        startTransitionDate,
                        endTransitionDate.AddTicks(-1),
                        daylightDelta,
                        dstStart,
                        default,
                        baseUtcDelta);
                        //noDaylightTransitions: true);

                //if (!IsValidAdjustmentRuleOffset(timeZoneBaseUtcOffset, r))
                //{
                //    NormalizeAdjustmentRuleOffset(timeZoneBaseUtcOffset, ref r);
                //}

                rulesList.Add(r);
            }
            else
            {
                // create the AdjustmentRule that will be used for all DateTimes after the last transition

                // NOTE: index == dts.Length
                DateTime startTransitionDate = dts[index - 1];

                AdjustmentRule? r = !string.IsNullOrEmpty(futureTransitionsPosixFormat) ?
                    TZif_CreateAdjustmentRuleForPosixFormat(futureTransitionsPosixFormat, startTransitionDate, timeZoneBaseUtcOffset) :
                    null;

                if (r == null)
                {
                    // just use the last transition as the rule which will be used until the end of time

                    TZifType transitionType = transitionTypes[typeOfLocalTime[index - 1]];
                    TimeSpan transitionOffset = TZif_CalculateTransitionOffsetFromBase(transitionType.UtcOffset, timeZoneBaseUtcOffset);
                    TimeSpan daylightDelta = transitionType.IsDst ? transitionOffset : TimeSpan.Zero;
                    TimeSpan baseUtcDelta = transitionType.IsDst ? TimeSpan.Zero : transitionOffset;

                    r = AdjustmentRule.CreateAdjustmentRule(
                        startTransitionDate,
                        DateTime.MaxValue,
                        daylightDelta,
                        default,
                        default,
                        baseUtcDelta);
                       // noDaylightTransitions: true);
                }

                //if (!IsValidAdjustmentRuleOffset(timeZoneBaseUtcOffset, r))
                //{
                //    NormalizeAdjustmentRuleOffset(timeZoneBaseUtcOffset, ref r);
                //}

                rulesList.Add(r);
            }

            index++;
        }

        private static TimeSpan? TZif_ParseOffsetString(ReadOnlySpan<char> offset)
        {
            TimeSpan? result = null;

            if (offset.Length > 0)
            {
                bool negative = offset[0] == '-';
                if (negative || offset[0] == '+')
                {
                    offset = offset.Slice(1);
                }

                // Try parsing just hours first.
                // Note, TimeSpan.TryParseExact "%h" can't be used here because some time zones using values
                // like "26" or "144" and TimeSpan parsing would turn that into 26 or 144 *days* instead of hours.
                int hours;
                if (int.TryParse(offset, out hours))
                {
                    result = new TimeSpan(hours, 0, 0);
                }
                else
                {
                    TimeSpan parsedTimeSpan;
                    if (TimeSpan.TryParseExact(offset, "g", CultureInfo.InvariantCulture, out parsedTimeSpan))
                    {
                        result = parsedTimeSpan;
                    }
                }

                if (result.HasValue && negative)
                {
                    result = result.GetValueOrDefault().Negate();
                }
            }

            return result;
        }


        private static AdjustmentRule? TZif_CreateAdjustmentRuleForPosixFormat(string posixFormat, DateTime startTransitionDate, TimeSpan timeZoneBaseUtcOffset)
        {
            if (TZif_ParsePosixFormat(posixFormat,
                out _,
                out ReadOnlySpan<char> standardOffset,
                out ReadOnlySpan<char> daylightSavingsName,
                out ReadOnlySpan<char> daylightSavingsOffset,
                out ReadOnlySpan<char> start,
                out ReadOnlySpan<char> startTime,
                out ReadOnlySpan<char> end,
                out ReadOnlySpan<char> endTime))
            {
                // a valid posixFormat has at least standardName and standardOffset

                TimeSpan? parsedBaseOffset = TZif_ParseOffsetString(standardOffset);
                if (parsedBaseOffset.HasValue)
                {
                    TimeSpan baseOffset = parsedBaseOffset.GetValueOrDefault().Negate(); // offsets are backwards in POSIX notation
                    baseOffset = TZif_CalculateTransitionOffsetFromBase(baseOffset, timeZoneBaseUtcOffset);

                    // having a daylightSavingsName means there is a DST rule
                    if (!daylightSavingsName.IsEmpty)
                    {
                        TimeSpan? parsedDaylightSavings = TZif_ParseOffsetString(daylightSavingsOffset);
                        TimeSpan daylightSavingsTimeSpan;
                        if (!parsedDaylightSavings.HasValue)
                        {
                            // default DST to 1 hour if it isn't specified
                            daylightSavingsTimeSpan = new TimeSpan(1, 0, 0);
                        }
                        else
                        {
                            daylightSavingsTimeSpan = parsedDaylightSavings.GetValueOrDefault().Negate(); // offsets are backwards in POSIX notation
                            daylightSavingsTimeSpan = TZif_CalculateTransitionOffsetFromBase(daylightSavingsTimeSpan, timeZoneBaseUtcOffset);
                            daylightSavingsTimeSpan = TZif_CalculateTransitionOffsetFromBase(daylightSavingsTimeSpan, baseOffset);
                        }

                        TransitionTime? dstStart = TZif_CreateTransitionTimeFromPosixRule(start, startTime);
                        TransitionTime? dstEnd = TZif_CreateTransitionTimeFromPosixRule(end, endTime);

                        if (dstStart == null || dstEnd == null)
                        {
                            return null;
                        }

                        return AdjustmentRule.CreateAdjustmentRule(
                            startTransitionDate,
                            DateTime.MaxValue,
                            daylightSavingsTimeSpan,
                            dstStart.GetValueOrDefault(),
                            dstEnd.GetValueOrDefault(),
                            baseOffset);
                            //noDaylightTransitions: false);
                    }
                    else
                    {
                        // if there is no daylightSavingsName, the whole AdjustmentRule should be with no transitions - just the baseOffset
                        return AdjustmentRule.CreateAdjustmentRule(
                               startTransitionDate,
                               DateTime.MaxValue,
                               TimeSpan.Zero,
                               default,
                               default,
                               baseOffset);
                              // noDaylightTransitions: true);
                    }
                }
            }

            return null;
        }

        private static DateTime ParseTimeOfDay(ReadOnlySpan<char> time)
        {
            DateTime timeOfDay;
            TimeSpan? timeOffset = TZif_ParseOffsetString(time);
            if (timeOffset.HasValue)
            {
                // This logic isn't correct and can't be corrected until https://github.com/dotnet/runtime/issues/14966 is fixed.
                // Some time zones use time values like, "26", "144", or "-2".
                // This allows the week to sometimes be week 4 and sometimes week 5 in the month.
                // For now, strip off any 'days' in the offset, and just get the time of day correct
                timeOffset = new TimeSpan(timeOffset.GetValueOrDefault().Hours, timeOffset.GetValueOrDefault().Minutes, timeOffset.GetValueOrDefault().Seconds);
                if (timeOffset.GetValueOrDefault() < TimeSpan.Zero)
                {
                    timeOfDay = new DateTime(1, 1, 2, 0, 0, 0);
                }
                else
                {
                    timeOfDay = new DateTime(1, 1, 1, 0, 0, 0);
                }

                timeOfDay += timeOffset.GetValueOrDefault();
            }
            else
            {
                // default to 2AM.
                timeOfDay = new DateTime(1, 1, 1, 2, 0, 0);
            }

            return timeOfDay;
        }
        private static bool TZif_ParseMDateRule(ReadOnlySpan<char> dateRule, out int month, out int week, out DayOfWeek dayOfWeek)
        {
            if (dateRule[0] == 'M')
            {
                int monthWeekDotIndex = dateRule.IndexOf('.');
                if (monthWeekDotIndex > 0)
                {
                    ReadOnlySpan<char> weekDaySpan = dateRule.Slice(monthWeekDotIndex + 1);
                    int weekDayDotIndex = weekDaySpan.IndexOf('.');
                    if (weekDayDotIndex > 0)
                    {
                        if (int.TryParse(dateRule.Slice(1, monthWeekDotIndex - 1), out month) &&
                            int.TryParse(weekDaySpan.Slice(0, weekDayDotIndex), out week) &&
                            int.TryParse(weekDaySpan.Slice(weekDayDotIndex + 1), out int day))
                        {
                            dayOfWeek = (DayOfWeek)day;
                            return true;
                        }
                    }
                }
            }

            month = 0;
            week = 0;
            dayOfWeek = default;
            return false;
        }

        private static TransitionTime? TZif_CreateTransitionTimeFromPosixRule(ReadOnlySpan<char> date, ReadOnlySpan<char> time)
        {
            if (date.IsEmpty)
            {
                return null;
            }

            if (date[0] == 'M')
            {
                // Mm.w.d
                // This specifies day d of week w of month m. The day d must be between 0(Sunday) and 6.The week w must be between 1 and 5;
                // week 1 is the first week in which day d occurs, and week 5 specifies the last d day in the month. The month m should be between 1 and 12.

                int month;
                int week;
                DayOfWeek day;
                if (!TZif_ParseMDateRule(date, out month, out week, out day))
                {
                    //throw new InvalidTimeZoneException(SR.Format(SR.InvalidTimeZone_UnparsablePosixMDateString, date.ToString()));
                }

                return TransitionTime.CreateFloatingDateRule(ParseTimeOfDay(time), month, week, day);
            }
            else
            {
                if (date[0] != 'J')
                {
                    // should be n Julian day format.
                    // This specifies the Julian day, with n between 0 and 365. February 29 is counted in leap years.
                    //
                    // n would be a relative number from the beginning of the year. which should handle if the
                    // the year is a leap year or not.
                    //
                    // In leap year, n would be counted as:
                    //
                    // 0                30 31              59 60              90      335            365
                    // |-------Jan--------|-------Feb--------|-------Mar--------|....|-------Dec--------|
                    //
                    // while in non leap year we'll have
                    //
                    // 0                30 31              58 59              89      334            364
                    // |-------Jan--------|-------Feb--------|-------Mar--------|....|-------Dec--------|
                    //
                    // For example if n is specified as 60, this means in leap year the rule will start at Mar 1,
                    // while in non leap year the rule will start at Mar 2.
                    //
                    // This n Julian day format is very uncommon and mostly  used for convenience to specify dates like January 1st
                    // which we can support without any major modification to the Adjustment rules. We'll support this rule  for day
                    // numbers less than 59 (up to Feb 28). Otherwise we'll skip this POSIX rule.
                    // We've never encountered any time zone file using this format for days beyond Feb 28.

                    if (int.TryParse(date, out int julianDay) && julianDay < 59)
                    {
                        int d, m;
                        if (julianDay <= 30) // January
                        {
                            m = 1;
                            d = julianDay + 1;
                        }
                        else // February
                        {
                            m = 2;
                            d = julianDay - 30;
                        }

                        return TransitionTime.CreateFixedDateRule(ParseTimeOfDay(time), m, d);
                    }

                    // Since we can't support this rule, return null to indicate to skip the POSIX rule.
                    return null;
                }

                // Julian day
                TZif_ParseJulianDay(date, out int month, out int day);
                return TransitionTime.CreateFixedDateRule(ParseTimeOfDay(time), month, day);
            }
        }

        private static bool TZif_ParsePosixFormat(
            ReadOnlySpan<char> posixFormat,
            out ReadOnlySpan<char> standardName,
            out ReadOnlySpan<char> standardOffset,
            out ReadOnlySpan<char> daylightSavingsName,
            out ReadOnlySpan<char> daylightSavingsOffset,
            out ReadOnlySpan<char> start,
            out ReadOnlySpan<char> startTime,
            out ReadOnlySpan<char> end,
            out ReadOnlySpan<char> endTime)
        {
            daylightSavingsOffset = null;
            start = null;
            startTime = null;
            end = null;
            endTime = null;

            int index = 0;
            standardName = TZif_ParsePosixName(posixFormat, ref index);
            standardOffset = TZif_ParsePosixOffset(posixFormat, ref index);

            daylightSavingsName = TZif_ParsePosixName(posixFormat, ref index);
            if (!daylightSavingsName.IsEmpty)
            {
                daylightSavingsOffset = TZif_ParsePosixOffset(posixFormat, ref index);

                if (index < posixFormat.Length && posixFormat[index] == ',')
                {
                    index++;
                    TZif_ParsePosixDateTime(posixFormat, ref index, out start, out startTime);

                    if (index < posixFormat.Length && posixFormat[index] == ',')
                    {
                        index++;
                        TZif_ParsePosixDateTime(posixFormat, ref index, out end, out endTime);
                    }
                }
            }

            return !standardName.IsEmpty && !standardOffset.IsEmpty;
        }

        private static ReadOnlySpan<char> TZif_ParsePosixName(ReadOnlySpan<char> posixFormat, scoped ref int index)
        {
            bool isBracketEnclosed = index < posixFormat.Length && posixFormat[index] == '<';
            if (isBracketEnclosed)
            {
                // move past the opening bracket
                index++;

                ReadOnlySpan<char> result = TZif_ParsePosixString(posixFormat, ref index, c => c == '>');

                // move past the closing bracket
                if (index < posixFormat.Length && posixFormat[index] == '>')
                {
                    index++;
                }

                return result;
            }
            else
            {
                return TZif_ParsePosixString(
                    posixFormat,
                    ref index,
                    c => char.IsDigit(c) || c == '+' || c == '-' || c == ',');
            }
        }

        private static ReadOnlySpan<char> TZif_ParsePosixOffset(ReadOnlySpan<char> posixFormat, scoped ref int index) =>
            TZif_ParsePosixString(posixFormat, ref index, c => !char.IsDigit(c) && c != '+' && c != '-' && c != ':');

        private static void TZif_ParsePosixDateTime(ReadOnlySpan<char> posixFormat, scoped ref int index, out ReadOnlySpan<char> date, out ReadOnlySpan<char> time)
        {
            time = null;

            date = TZif_ParsePosixDate(posixFormat, ref index);
            if (index < posixFormat.Length && posixFormat[index] == '/')
            {
                index++;
                time = TZif_ParsePosixTime(posixFormat, ref index);
            }
        }

        private static ReadOnlySpan<char> TZif_ParsePosixDate(ReadOnlySpan<char> posixFormat, scoped ref int index) =>
            TZif_ParsePosixString(posixFormat, ref index, c => c == '/' || c == ',');

        private static ReadOnlySpan<char> TZif_ParsePosixTime(ReadOnlySpan<char> posixFormat, scoped ref int index) =>
            TZif_ParsePosixString(posixFormat, ref index, c => c == ',');

        private static ReadOnlySpan<char> TZif_ParsePosixString(ReadOnlySpan<char> posixFormat, scoped ref int index, Func<char, bool> breakCondition)
        {
            int startIndex = index;
            for (; index < posixFormat.Length; index++)
            {
                char current = posixFormat[index];
                if (breakCondition(current))
                {
                    break;
                }
            }

            return posixFormat.Slice(startIndex, index - startIndex);
        }

        private static TimeSpan TZif_CalculateTransitionOffsetFromBase(TimeSpan transitionOffset, TimeSpan timeZoneBaseUtcOffset)
        {
            TimeSpan result = transitionOffset - timeZoneBaseUtcOffset;

            // TZif supports seconds-level granularity with offsets but TimeZoneInfo only supports minutes since it aligns
            // with DateTimeOffset, SQL Server, and the W3C XML Specification
            if (result.Ticks % TimeSpan.TicksPerMinute != 0)
            {
                result = new TimeSpan(result.Hours, result.Minutes, 0);
            }

            return result;
        }

   

        private static TZifType TZif_GetEarlyDateTransitionType(TZifType[] transitionTypes)
        {
            foreach (TZifType transitionType in transitionTypes)
            {
                if (!transitionType.IsDst)
                {
                    return transitionType;
                }
            }

            if (transitionTypes.Length > 0)
            {
                return transitionTypes[0];
            }

            //throw new InvalidTimeZoneException(SR.InvalidTimeZone_NoTTInfoStructures);
            return transitionTypes[0];
        }

        private static void TZif_ParseJulianDay(ReadOnlySpan<char> date, out int month, out int day)
        {
            // Jn
            // This specifies the Julian day, with n between 1 and 365.February 29 is never counted, even in leap years.
            Debug.Assert(!date.IsEmpty);
            Debug.Assert(date[0] == 'J');
            month = day = 0;

            int index = 1;

            if ((uint)index >= (uint)date.Length || !char.IsAsciiDigit(date[index]))
            {
                //throw new InvalidTimeZoneException(SR.InvalidTimeZone_InvalidJulianDay);
            }

            int julianDay = 0;

            do
            {
                julianDay = julianDay * 10 + (int)(date[index] - '0');
                index++;
            } while ((uint)index < (uint)date.Length && char.IsAsciiDigit(date[index]));


        int[] DaysToMonth365 =
        {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365
        };

        ReadOnlySpan<int> days = DaysToMonth365;

            if (julianDay == 0 || julianDay > days[days.Length - 1])
            {
                //throw new InvalidTimeZoneException(SR.InvalidTimeZone_InvalidJulianDay);
            }

            int i = 1;
            while (i < days.Length && julianDay > days[i])
            {
                i++;
            }

            Debug.Assert(i > 0 && i < days.Length);

            month = i;
            day = julianDay - days[i - 1];
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

        // Returns the Substring from zoneAbbreviations starting at index and ending at '\0'
        // zoneAbbreviations is expected to be in the form: "PST\0PDT\0PWT\0\PPT"
        private static string TZif_GetZoneAbbreviation(string zoneAbbreviations, int index)
        {
            int lastIndex = zoneAbbreviations.IndexOf('\0', index);
            return lastIndex > 0 ?
                zoneAbbreviations.Substring(index, lastIndex - index) :
                zoneAbbreviations.Substring(index);
        }

        // Converts a span of bytes into a long - always using standard byte order (Big Endian)
        // per TZif file standard
        private static short TZif_ToInt16(ReadOnlySpan<byte> value)
            => BinaryPrimitives.ReadInt16BigEndian(value);

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


        private static long TZif_ToUnixTime(byte[] value, int startIndex, TZVersion version) =>
            version != TZVersion.V1 ?
                TZif_ToInt64(value, startIndex) :
                TZif_ToInt32(value, startIndex);


        private static DateTime TZif_UnixTimeToDateTime(long unixTime) =>
             unixTime < -9223372036854775808 ? DateTimeOffset.FromUnixTimeSeconds(-9223372036854775808).UtcDateTime :
             unixTime > 9223372036854775807 ? DateTimeOffset.FromUnixTimeSeconds(+9223372036854775807).UtcDateTime :
             DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;


        private static void TZif_ParseRaw(byte[] data, out DateTime[] dts, out byte[] typeOfLocalTime, out TZifType[] transitionType,
                                          out string zoneAbbreviations, out string? futureTransitionsPosixFormat)
        {
            futureTransitionsPosixFormat = null;

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

            // initialize the containers for the rest of the TZ data
            dts = new DateTime[t.TimeCount];
            typeOfLocalTime = new byte[t.TimeCount];
            transitionType = new TZifType[t.TypeCount];

            // read in the UTC transition points and convert them to Windows
            //
            for (int i = 0; i < t.TimeCount; i++)
            {
                long unixTime = TZif_ToUnixTime(data, index, t.Version);
                dts[i] = TZif_UnixTimeToDateTime(unixTime);
                index += timeValuesLength;
            }

            // read in the Type Indices; there is a 1:1 mapping of UTC transition points to Type Indices
            // these indices directly map to the array index in the transitionType array below
            //
            for (int i = 0; i < t.TimeCount; i++)
            {
                typeOfLocalTime[i] = data[index];
                index++;
            }

            // read in the Type table.  Each 6-byte entry represents
            // {UtcOffset, IsDst, AbbreviationIndex}
            //
            // each AbbreviationIndex is a character index into the zoneAbbreviations string below
            //
            for (int i = 0; i < t.TypeCount; i++)
            {
                transitionType[i] = new TZifType(data, index);
                index += 6;
            }

            // read in the Abbreviation ASCII string.  This string will be in the form:
            // "PST\0PDT\0PWT\0\PPT"
            //
            Encoding enc = Encoding.UTF8;
            zoneAbbreviations = enc.GetString(data, index, (int)t.CharCount);
            index += (int)t.CharCount;

            // skip ahead of the Leap-Seconds Adjustment data.  In a future release, consider adding
            // support for Leap-Seconds
            //
            index += (int)(t.LeapCount * (timeValuesLength + 4)); // skip the leap second transition times

            // read in the Standard Time table.  There should be a 1:1 mapping between Type-Index and Standard
            // Time table entries.
            //
            // TRUE     =     transition time is standard time
            // FALSE    =     transition time is wall clock time
            // ABSENT   =     transition time is wall clock time
            //
            index += (int)Math.Min(t.IsStdCount, t.TypeCount);

            // read in the GMT Time table.  There should be a 1:1 mapping between Type-Index and GMT Time table
            // entries.
            //
            // TRUE     =     transition time is UTC
            // FALSE    =     transition time is local time
            // ABSENT   =     transition time is local time
            //
            index += (int)Math.Min(t.IsGmtCount, t.TypeCount);

            if (t.Version != TZVersion.V1)
            {
                // read the POSIX-style format, which should be wrapped in newlines with the last newline at the end of the file
                if (data[index++] == '\n' && data[data.Length - 1] == '\n')
                {
                    futureTransitionsPosixFormat = enc.GetString(data, index, data.Length - index - 1);
                }
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
