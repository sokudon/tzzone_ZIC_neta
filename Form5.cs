﻿using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Force.Crc32;
using System.Security.Cryptography;
using System.Web;
using NodaTime;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static neta.ZIC.PosixTimeZoneParser;
using System.Globalization;
using System.Text.Encodings.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Diagnostics.Tracing;
using OBSWebsocketDotNet.Types;
using static System.Windows.Forms.DataFormats;
using System.Collections.Generic;
using System.Linq;
using TimeZoneConverter;
using TimeZoneConverter.Posix;
using NodaTime.TimeZones;
using System.Security.Policy;


namespace neta
{
    public partial class ZIC : Form
    {
        public ZIC()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            posix_list_add();

            androidTzSeek.Text = Properties.Settings.Default.android_tzseek;

            android_tz.Checked = Properties.Settings.Default.android_tz;

            tzutc.Checked = Properties.Settings.Default.cv_unixtime;

            test_date.Text = Properties.Settings.Default.posix_testdate;

            posix.Checked = Properties.Settings.Default.view_posix_info;

            Properties.Settings.Default.posix_test = true;
        }

        private void ZIC_FormClosing(object sender, FormClosingEventArgs e)
        {

            Properties.Settings.Default.posix_test = false;
        }

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


        //https://android.googlesource.com/platform/libcore/+/jb-mr2-release/luni/src/main/java/libcore/util/ZoneInfoDB.java
        //https://github.com/RumovZ/android-tzdata/blob/main/src/tzdata.rs
        public static int[] android_tzreader(string tzdata, bool andseek, string andseekst)
        {
            string filePath = tzdata;
            int[] target = new int[2];
            target[0] = -1;
            target[1] = -1;

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    //fs.Seek(24, SeekOrigin.Begin);
                    // 52バイトを610回読み込む
                    byte[] buffer = new byte[52];
                    fs.Read(buffer, 0, 24);

                    int indexOffset = (int)(buffer[12] << 24 | buffer[13] << 16 | buffer[14] << 8 | buffer[15]);
                    int dataOffset = (int)(buffer[16] << 24 | buffer[17] << 16 | buffer[18] << 8 | buffer[19]);
                    int zonetabOffset = (int)(buffer[20] << 24 | buffer[21] << 16 | buffer[22] << 8 | buffer[23]);

                    int indexSize = (dataOffset - indexOffset);
                    int sectionCount = 0;
                    int offset = 0;
                    int maxSections = indexSize / 52;
                    string tzname = "";
                    string tznamebk = "";
                    StringBuilder sb = new StringBuilder();

                    sb.Append($"Section,");
                    sb.Append($"tzname,");
                    sb.Append($"offset,");
                    sb.AppendLine($"tz_length");


                    while (fs.Read(buffer, 0, 52) == 52 && sectionCount < maxSections)
                    {
                        tzname = Encoding.ASCII.GetString(buffer, 0, 20).TrimEnd('\0');


                        if (tzname == "TZif2")
                        {
                            break;
                        }
                        tznamebk = tzname;

                        offset = (int)(buffer[40] << 24 | buffer[41] << 16 | buffer[42] << 8 | buffer[43]);
                        string offsetHex = $"0x{offset:X8}";
                        int tzLength = (int)(buffer[44] << 24 | buffer[45] << 16 | buffer[46] << 8 | buffer[47]);

                        sb.AppendLine($"{sectionCount + 1},{tzname},{offsetHex},{tzLength}");

                        if (andseek == true && tzname == andseekst)
                        {
                            target[0] = offset;
                            target[1] = tzLength;
                        }

                        sectionCount++;
                    }
                    int Tzif_pos = sectionCount * 52 + 24;
                    sb.AppendLine($"Total sections parsed: {sectionCount},zonetab_size:{zonetabOffset}");
                    sb.AppendLine($"1stTzif pos: 0x{Tzif_pos:X8}");
                    sb.AppendLine($"{tznamebk}_pos is 1stTzif: 0x{Tzif_pos + offset:X8}");
                    sb.AppendLine();
                    Properties.Settings.Default.android_tzfie_info = sb.ToString();

                    if (target[1] > 0)
                    {
                        target[0] += Tzif_pos;
                    }
                    return target;
                }
            }
            catch (Exception ex)
            {
                Properties.Settings.Default.android_tzfie_info = $"Error: {ex.Message}\r\n";
                return target;
            }
        }

        public static string tzif_reader(string tzdata, string android_tzseek, bool android_tz, bool tzutc, bool posix)
        {
            StringBuilder sb = new StringBuilder();
            try
            {

                if (File.Exists(tzdata))
                {
                    System.IO.FileStream fs = new FileStream(tzdata, FileMode.Open, FileAccess.Read);
                    byte[] bs = new byte[fs.Length];
                    fs.Read(bs, 0, bs.Length);
                    fs.Close();
                    var encoding = Encoding.GetEncoding(0);
                    var header = encoding.GetString(bs).Substring(0, 4);

                    if (header == "tzda")
                    {
                        Properties.Settings.Default.android_tzfie_info = "";

                        int[] target = new int[2];
                        target = android_tzreader(tzdata, android_tz, android_tzseek);
                        if (target[0] == -1)
                        {
                            sb.Append(Properties.Settings.Default.android_tzfie_info);
                            return sb.ToString();
                        }
                        var tzver = encoding.GetString(bs).Substring(0, 12).TrimEnd('\0');
                        byte[] bss = new byte[target[1]];
                        Array.ConstrainedCopy(bs, target[0], bss, 0, target[1]);
                        bs = bss;
                        sb.Append(Properties.Settings.Default.android_tzfie_info);
                        sb.AppendLine($"Android tzdata binary reader success\r\n" +
                            $"tzver:{tzver},target: {tzdata} pos:{target[0]},size{target[1]}");
                        sb.AppendLine();
                        header = encoding.GetString(bs).Substring(0, 4);
                    }
                    string footer = "";
                    int nexttzpos = -1;
                    int finalpos = -1;
                    string versionn = "";
                    bool tzcv = tzutc;


                    if (header.Contains("TZif") == false)
                    {
                        sb.AppendLine("Zone infomation compiler で作成されたtzdataバイナリTZifファイルではありません");
                    }
                    else
                    {
                        //python tzinfoのうるう秒こみのDBには非対応
                        //#unix/linux系 google colabとかもcygwinとか
                        //#/usr/share/zoneinfo/

                        //#python系　tar.gzなので解凍必要
                        //#Python312\Lib\site-packages\dateutil\zoneinfo
                        //#Python312\Lib\site-packages\pytz\zoneinfo

                        //#64bitデータベースだけ読む模様,しかしロサンゼルスが2006年までしか入ってないのでいまいち
                        //#Python312\Lib\site-packages\tzdata\zoneinfo

                        //version NULL(1) 0x32=2 0x33=3がある模様
                        string version = encoding.GetString(bs).Substring(4, 1);

                        if (bs[4] == 0)
                        {
                            sb.AppendLine("Zone infomation compiler で作成されたtzdataバイナリTZif version NULL(1) はMUST BE IGNOREです");
                        }
                        else
                        {

                            int pos = 20;

                            int tzh_ttisgmtcnt = BitConverter.ToInt32(Bigval32(bs, pos), 0);
                            int tzh_ttisstdcnt = BitConverter.ToInt32(Bigval32(bs, pos + 4), 0);
                            int tzh_leapcnt = BitConverter.ToInt32(Bigval32(bs, pos + 8), 0);
                            int tzh_timecnt = BitConverter.ToInt32(Bigval32(bs, pos + 12), 0);
                            int tzh_typecnt = BitConverter.ToInt32(Bigval32(bs, pos + 16), 0);
                            int tzh_charcnt = BitConverter.ToInt32(Bigval32(bs, pos + 20), 0);


                            //C# 適切なメンテナンスをしないと2038年問題が起きる可能性がある transition_timeが32bitのため
                            int[] transition_times = new int[tzh_timecnt];
                            int[] transition_types = new int[tzh_timecnt];

                            pos = 44;
                            if (tzh_timecnt != 0)
                            {
                                for (int i = 0; i < tzh_timecnt; i++)
                                {
                                    transition_times[i] = BitConverter.ToInt32(Bigval32(bs, pos + 4 * i), 0);
                                    transition_types[i] = bs[pos + tzh_timecnt * 4 + i];
                                }
                            }
                            else
                            {
                                transition_times = [];
                                transition_types = [];
                            }

                            pos = 44 + tzh_timecnt * 5;
                            int[] local_time_types_gmt = new int[tzh_typecnt];
                            int[] local_time_types_isdst = new int[tzh_typecnt];
                            int[] local_time_types_abbr = new int[tzh_typecnt];

                            for (int i = 0; i < tzh_typecnt; i++)
                            {
                                local_time_types_gmt[i] = BitConverter.ToInt32(Bigval32(bs, pos + i * 6), 0);
                                local_time_types_isdst[i] = bs[pos + 4 + i * 6];
                                local_time_types_abbr[i] = bs[pos + 5 + i * 6];
                            }

                            pos = pos + 6 * tzh_typecnt;
                            byte[] abbr = new byte[tzh_charcnt + 10];
                            Array.ConstrainedCopy(bs, pos, abbr, 0, tzh_charcnt);

                            sb.AppendLine("1stTZif transitions,gmtoffset,isdst,abbr");
                            pos = pos + tzh_charcnt;
                            if (tzh_leapcnt > 0)
                            {
                                pos = pos + tzh_leapcnt * 8;
                            }


                            if (tzh_ttisstdcnt > 0)
                            {
                                //isstd = struct.unpack(">%db" % ttisstdcnt fileobj.read(ttisstdcnt))
                                byte[] isstd = new byte[tzh_ttisstdcnt];
                                Array.ConstrainedCopy(bs, pos, isstd, 0, tzh_ttisstdcnt);
                                pos += tzh_ttisstdcnt;
                            }

                            if (tzh_ttisgmtcnt > 0)
                            {
                                //isgmt = struct.unpack(">%db" % ttisgmtcnt, fileobj.read(ttisgmtcnt))
                                byte[] isgmt = new byte[tzh_ttisgmtcnt];
                                Array.ConstrainedCopy(bs, pos, isgmt, 0, tzh_ttisgmtcnt);
                                pos += tzh_ttisgmtcnt;

                            }

                            //UTCバイナリの場合　tzh_timecntはないがオフセット posixストリングはある
                            if (tzh_timecnt == 0)
                            {
                                int type = 0;
                                string[][] transitions_next_zero = new string[1][];
                                transitions_next_zero[0] = new string[4];
                                transitions_next_zero[0][0] = "";
                                transitions_next_zero[0][1] = Convert.ToString(local_time_types_gmt[type]);
                                transitions_next_zero[0][2] = Convert.ToString(local_time_types_isdst[type]);

                                byte[] tmp2 = new byte[20];
                                Array.ConstrainedCopy(abbr, local_time_types_abbr[type], tmp2, 0, 10);
                                char[] charArray = ByteArrayToCharArray(tmp2, Encoding.UTF8);
                                transitions_next_zero[0][3] = TerminateAtNull(charArray);
                                if (tzcv)
                                {

                                    transitions_next_zero[0][1] = Convert.ToString(Convert.ToDouble(local_time_types_gmt[type]) / 3600);
                                }

                                sb.Append("null");
                                sb.Append(",");
                                sb.Append(transitions_next_zero[0][1]);
                                sb.Append(",");
                                sb.Append(transitions_next_zero[0][2]);
                                sb.Append(",");
                                sb.AppendLine(transitions_next_zero[0][3]);
                            }

                            string[][] transitions = new string[tzh_timecnt][];
                            for (int i = 0; i < tzh_timecnt; i++)
                            {
                                int type = transition_types[i];
                                transitions[i] = new string[4];
                                transitions[i][0] = transition_times[i].ToString();
                                transitions[i][1] = Convert.ToString(local_time_types_gmt[type]);
                                transitions[i][2] = Convert.ToString(local_time_types_isdst[type]);
                                byte[] tmp = new byte[20];
                                Array.ConstrainedCopy(abbr, local_time_types_abbr[type], tmp, 0, 10);
                                char[] charArray = ByteArrayToCharArray(tmp, Encoding.UTF8);
                                transitions[i][3] = TerminateAtNull(charArray);

                                if (tzcv)
                                {

                                    transitions[i][1] = Convert.ToString(Convert.ToDouble(local_time_types_gmt[type]) / 3600);
                                    double localTimeOffseth = Convert.ToDouble(local_time_types_gmt[type]) / 3600;

                                    long unixTimestamp = transition_times[i];


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
                                            transitions[i][0] = formattedDateh;
                                        }
                                        else
                                        {
                                            // カスタムフォーマット
                                            DateTimeOffset originalTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).AddHours(localTimeOffseth);
                                            string formattedDate = $"{originalTime:yyyy-MM-ddTHH:mm:ss.fff} {formattedOffset}";
                                            transitions[i][0] = formattedDate;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sb.AppendLine(ex.ToString());
                                        break;
                                    }


                                }

                                sb.Append(transitions[i][0]);
                                sb.Append(",");
                                sb.Append(transitions[i][1]);
                                sb.Append(",");
                                sb.Append(transitions[i][2]);
                                sb.Append(",");
                                sb.AppendLine(transitions[i][3]);
                            }

                            sb.AppendLine();

                            byte[] TZifnext = new byte[4];
                            Array.ConstrainedCopy(bs, pos, TZifnext, 0, 4);
                            var header2 = encoding.GetString(TZifnext).Substring(0, 4);
                            nexttzpos = pos;
                            finalpos = -1;


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
                                versionn = encoding.GetString(v);

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


                                    //UTCバイナリの場合　tzh_timecntはないがオフセット posixストリングはある
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
                                        if (tzcv)
                                        {
                                            transitions_next_zero[0][1] = Convert.ToString(Convert.ToDouble(local_time_types_gmtn[type]) / 3600);
                                        }
                                        sb.Append("null");
                                        sb.Append(",");
                                        sb.Append(transitions_next_zero[0][1]);
                                        sb.Append(",");
                                        sb.Append(transitions_next_zero[0][2]);
                                        sb.Append(",");
                                        sb.AppendLine(transitions_next_zero[0][3]);
                                    }

                                    string[][] transitions_next = new string[tzh_timecnt_next][];
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
                                        if (tzcv)
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
                                        sb.Append(transitions_next[i][0]);
                                        sb.Append(",");
                                        sb.Append(transitions_next[i][1]);
                                        sb.Append(",");
                                        sb.Append(transitions_next[i][2]);
                                        sb.Append(",");
                                        sb.AppendLine(transitions_next[i][3]);
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

                            string crc32 = Crc32Algorithm.Compute(bs).ToString("X8");
                            string hashsha;
                            string hashmd;

                            using (MD5 md5 = MD5.Create())
                            {
                                byte[] hashBytes = md5.ComputeHash(bs);
                                hashmd = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                            }
                            using (SHA1 sha1 = SHA1.Create())
                            {
                                byte[] hashBytes2 = sha1.ComputeHash(bs);
                                hashsha = BitConverter.ToString(hashBytes2).Replace("-", "").ToLowerInvariant();
                            }

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


                            sb.Append("crc32:");
                            sb.Append(crc32);
                            sb.AppendLine();
                            sb.Append("md5:");
                            sb.Append(hashmd);
                            sb.AppendLine();
                            sb.Append("sha1:");
                            sb.AppendLine(hashsha);
                            sb.AppendLine();
                            sb.AppendLine();

                            sb.Append("tzif version:");
                            sb.Append("Tzif" + version);
                            sb.AppendLine();
                            sb.AppendLine("tzh_ttisgmtcnt:" + tzh_ttisgmtcnt.ToString());
                            sb.AppendLine("tzh_ttisstdcnt :" + tzh_ttisstdcnt.ToString());
                            sb.AppendLine("tzh_leapcnt:" + tzh_leapcnt.ToString());
                            sb.AppendLine("tzh_timecnt:" + tzh_timecnt.ToString());
                            sb.AppendLine("tzh_typecnt:" + tzh_typecnt.ToString());
                            sb.AppendLine("tzh_charcnt:" + tzh_charcnt.ToString());


                            sb.AppendLine();
                            sb.Append("2nd TZif position:");
                            sb.Append(nexttzpos.ToString());
                            sb.AppendLine();
                            sb.Append("2nd tzif version:");
                            sb.Append("Tzif" + versionn);
                            sb.AppendLine();
                            sb.AppendLine("2nd tzh_ttisgmtcnt:" + tzh_ttisgmtcnt_next.ToString());
                            sb.AppendLine("2nd tzh_ttisstdcnt :" + tzh_ttisstdcnt_next.ToString());
                            sb.AppendLine("2nd tzh_leapcnt:" + tzh_leapcnt_next.ToString());
                            sb.AppendLine("2nd tzh_timecnt:" + tzh_timecnt_next.ToString());
                            sb.AppendLine("2nd tzh_typecnt:" + tzh_typecnt_next.ToString());
                            sb.AppendLine("2nd tzh_charcnt:" + tzh_charcnt_next.ToString());

                            if (posix)
                            {
                                sb.AppendLine();
                                sb.AppendLine($"footer_posix timezoneの詳細表示 {footer}");
                                string footer_info = PosixTimeZoneParser.LoadPosix(footer);
                                sb.AppendLine(footer_info);
                                Properties.Settings.Default.posix_json = footer_info;
                            }


                        }
                    }
                    Properties.Settings.Default.lasttzdatapath = System.IO.Path.GetDirectoryName(tzdata);
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.ToString());
            }
            return sb.ToString();
        }

        string last_tzdata = "";

        private void button1_Click(object sender, EventArgs e)
        {
            string tmp = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.lasttzdatapath);
            ofd.Title = "unix usr/share/tzinfoやpython pytz dateutilのtzdatabaseフォルダのバイナリふぁいるを選択してください";

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = "";
                //python dateutil一部をC#ぽくしただけのもの 下にgoogle colabでうごくやつも貼っとく 
                //zicバイナリの情報　https://tex2e.github.io/rfc-translater/html/rfc8536.html
                string tzdata = ofd.FileName;// "Tokyo";
                last_tzdata = ofd.FileName;

                tmp = tzif_reader(tzdata, androidTzSeek.Text, android_tz.Checked, tzutc.Checked, posix.Checked);
            }
            textBox1.Text = tmp;
            if (posix.Checked)
            {
                ruletester();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.android_tzseek = androidTzSeek.Text;

            textBox1.Text = tzif_reader(last_tzdata, androidTzSeek.Text, android_tz.Checked, tzutc.Checked, posix.Checked);

            if (posix.Checked)
            {
                ruletester();
            }

        }

        private void android_tz_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.android_tz = android_tz.Checked;
            androidTzSeek.Enabled = android_tz.Checked;
        }

        private void tzutc_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.cv_unixtime = tzutc.Checked;
        }

        private void posix_list_add() {
            /// コンボボックスのアイテムをリストに格納// android_tzseek のアイテムをリストに格納
            List<string> ianaList = GetComboBoxItemsAsList(androidTzSeek);
            // IANA を POSIX に変換したリストを作成（仮の変換ロジック）
            List<string> posixList = ConvertIanaToPosix(ianaList);
            // ソートと重複除去
            posixList = posixList.Distinct()       // 重複を除去
                                 .OrderBy(x => x)  // アルファベット順にソート
                                 .ToList();
            // posix_timezone コンボボックスにリストからアイテムを追加
            AddListToComboBox(posixTimezone, posixList);

            // 確認メッセージ
            //MessageBox.Show($"POSIX コンボボックスに {posixList.Count} 件追加しました。");

        }
        private List<string> GetComboBoxItemsAsList(ComboBox comboBox)
        {
            // コンボボックスのアイテムをリストに変換
            return comboBox.Items.Cast<object>().Select(item => item.ToString()).ToList();
        }

        private List<string> ConvertIanaToPosix(List<string> ianaList)
        {
            List<string> posixList = new List<string>();
            var tzdb = DateTimeZoneProviders.Tzdb; // NodaTime の TZDB プロバイダ

            foreach (var iana in ianaList)
            {
                try
                {
                    // IANA タイムゾーンが TZDB に存在するか確認
                    var zone = tzdb.GetZoneOrNull(iana);
                    if (zone == null)
                    {
                        Console.WriteLine($"スキップ: {iana} は NodaTime TZDB に存在しません。");
                        continue; // 存在しない場合はスキップ
                    }

                    // TimeZoneConverter で POSIX に変換
                    var tzInfo = TZConvert.GetTimeZoneInfo(iana);
                    string posix = PosixTimeZone.FromIanaTimeZoneName(iana);
                    posixList.Add(posix);
                }
                catch (DateTimeZoneNotFoundException ex)
                {
                    // NodaTime 例外が発生した場合もスキップ
                    Console.WriteLine($"スキップ: {iana} - {ex.Message}");
                }            }
                return posixList;
            } 
        

        private void AddListToComboBox(ComboBox comboBox, List<string> items)
        {
            // 既存アイテムをクリア（必要に応じてコメントアウト）
            comboBox.Items.Clear();

            // リストからコンボボックスにアイテムを追加
            comboBox.Items.AddRange(items.ToArray());
        }



        //https://github.com/nayarsystems/posix_tz_db posiz_tz_dbのリストらしい
        private void button2_Click(object sender, EventArgs e)
        {
            var stdMatch = Regex.Match(posixTimezone.Text, @"^(?:<(?<stdName>[^>]+)>|(?<stdName2>[A-Za-z]+))(?<stdOffset>[+-]?\d+(?::\d+)?)(?:(?:<(?<dstName>[^>]+)>|(?<dstName2>[A-Za-z]+))(?<dstOffset>[+-]?\d+(?::\d+)?))?");
            if (!stdMatch.Success)
            {
                textBox1.Text = $"Invalid posix_timezone format:{posixTimezone.Text} \r\nRegex:^(?:<(?<stdName>[^>]+)>|(?<stdName2>[A-Za-z]+))(?<stdOffset>[+-]?\\d+(?::\\d+)?)(?:(?:<(?<dstName>[^>]+)>|(?<dstName2>[A-Za-z]+))(?<dstOffset>[+-]?\\d+(?::\\d+)?))?\")";
                return;
            }
            string tmp = PosixTimeZoneParser.test(posixTimezone.Text);
            textBox1.Text = tmp;

            Properties.Settings.Default.posix_json = tmp;
            if (posix.Checked)
            {
                ruletester();
            }
        }

        // 1. データモデル (クラス) の定義:
        public class TimeZoneData
        {
            [JsonPropertyName("Parsing")]
            public string? Parsing { get; set; }

            [JsonPropertyName("Posix_Normalize")]
            public string? PosixNormalize { get; set; }

            [JsonPropertyName("StandardName")]
            public string? StandardName { get; set; }

            [JsonPropertyName("StandardOffset")]
            public string? StandardOffset { get; set; }

            [JsonPropertyName("DaylightName")]
            public string? DaylightName { get; set; }

            [JsonPropertyName("DaylightOffset")]
            public string? DaylightOffset { get; set; }

            [JsonPropertyName("StartRule")]
            public Rule? StartRule { get; set; }

            [JsonPropertyName("EndRule")]
            public Rule? EndRule { get; set; }
        }

        public class Rule
        {
            [JsonPropertyName("Month")]
            public int Month { get; set; }

            [JsonPropertyName("Week")]
            public int Week { get; set; }

            [JsonPropertyName("DayOfWeek")]
            public int DayOfWeek { get; set; }

            [JsonPropertyName("Time")]
            public string? Time { get; set; }

            [JsonPropertyName("CurrentYearDate")]
            public string? CurrentYearDate { get; set; }
        }


        private void ruletester()
        {
            try
            {
                string json = Properties.Settings.Default.posix_json;
                if (string.IsNullOrEmpty(json))
                {
                    this.textBox1.Text += "JSON data is missing.\r\n";
                    return;
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                TimeZoneData? tzData = JsonSerializer.Deserialize<TimeZoneData>(json, options);

                if (tzData == null)
                {
                    this.textBox1.Text += "Failed to deserialize JSON.\r\n";
                    return;
                }

                // --- Microsoft Docs style TimeZoneInfo creation ---

                // 1. Parse TimeSpans
                if (!TimeSpan.TryParse(tzData.StandardOffset, out TimeSpan standardOffset))
                {
                    this.textBox1.Text += "Failed to parse StandardOffset.\r\n";
                    return;
                }
                if (!TimeSpan.TryParse(tzData.DaylightOffset, out TimeSpan daylightDelta))
                {
                    daylightDelta = TimeSpan.Zero;
                }
                daylightDelta -= standardOffset; // Calculate the delta

                // 2. Get Transition Times
                System.TimeZoneInfo.TransitionTime transitionRuleStart = default, transitionRuleEnd = default;

                // --- Start Rule ---
                if (tzData.StartRule != null)
                {
                    if (!TimeSpan.TryParse(tzData.StartRule.Time, out TimeSpan startTime))
                    {
                        this.textBox1.Text += "Failed to parse StartRule.Time.\r\n";
                        return;
                    }

                    transitionRuleStart = System.TimeZoneInfo.TransitionTime.CreateFloatingDateRule(
                        new DateTime(1, 1, 1, startTime.Hours, startTime.Minutes, startTime.Seconds),
                        tzData.StartRule.Month,
                        tzData.StartRule.Week,
                        (DayOfWeek)tzData.StartRule.DayOfWeek
                    );
                }

                // --- End Rule ---
                if (tzData.EndRule != null)
                {
                    if (!TimeSpan.TryParse(tzData.EndRule.Time, out TimeSpan endTime))
                    {
                        this.textBox1.Text += "Failed to parse EndRule.Time.\r\n";
                        return;
                    }
                    transitionRuleEnd = System.TimeZoneInfo.TransitionTime.CreateFloatingDateRule(
                        new DateTime(1, 1, 1, endTime.Hours, endTime.Minutes, endTime.Seconds),
                        tzData.EndRule.Month,
                        tzData.EndRule.Week,
                        (DayOfWeek)tzData.EndRule.DayOfWeek
                    );
                }

                // 3. Create Adjustment Rule (for the specified year)
                int thisyear = DateTime.Now.Year;
                // --- Time Conversion and Output ---
                string date = Properties.Settings.Default.posix_testdate;
                DateTimeOffset utcNowOffset;
                if (date != "")
                {
                    utcNowOffset = DateTimeOffset.Parse(date);
                }
                else
                {
                    utcNowOffset = DateTimeOffset.UtcNow;
                }



                System.TimeZoneInfo.AdjustmentRule adjustmentRule;
                System.TimeZoneInfo customTimeZone;
                if (tzData.StartRule == null || tzData.EndRule == null)
                {
                    TimeSpan offset = standardOffset;
                    customTimeZone = System.TimeZoneInfo.CreateCustomTimeZone(
                        id: tzData.Parsing?.Split(',')[0] ?? "Custom Time Zone", // Use Parsing property for ID
                        baseUtcOffset: standardOffset,
                        displayName: tzData.StandardName + standardOffset,  // Or construct as needed
                        standardDisplayName: tzData.StandardName ?? "Standard Time"
                    ); ;

                }
                else
                {
                    adjustmentRule = System.TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(
                        dateStart: new DateTime(thisyear, 1, 1),
                        dateEnd: new DateTime(thisyear, 12, 31),
                        daylightDelta: daylightDelta,
                        daylightTransitionStart: transitionRuleStart,
                        daylightTransitionEnd: transitionRuleEnd
                    );

                    // 4. Create the custom TimeZoneInfo
                    customTimeZone = System.TimeZoneInfo.CreateCustomTimeZone(
                        id: tzData.Parsing?.Split(',')[0] ?? "Custom Time Zone", // Use Parsing property for ID
                        baseUtcOffset: standardOffset,
                        displayName: tzData.StandardName + standardOffset,  // Or construct as needed
                        standardDisplayName: tzData.StandardName ?? "Standard Time",
                        daylightDisplayName: tzData.DaylightName ?? "Daylight Time",
                        adjustmentRules: new[] { adjustmentRule } // Pass the single rule
                    );
                }

                DateTimeOffset localTimeOffset = System.TimeZoneInfo.ConvertTime(utcNowOffset, customTimeZone);
                DateTime dt = localTimeOffset.LocalDateTime;
                this.textBox1.Text += $"UTC Datetime: {utcNowOffset:yyyy-MM-dd HH:mm:sszzz} UTC\r\n";
                this.textBox1.Text += $"posix TimeZone  Datetime: {localTimeOffset:yyyy-MM-dd HH:mm:sszzz}\r\n";
                this.textBox1.Text += $"OS Localtime: {dt:yyyy-MM-dd HH:mm:sszzz}\r\n";
            }
            catch (Exception ex)
            {
                this.textBox1.Text += $"Error: {ex}\r\n";
            }
        }


        //https://github.com/pganssle/zoneinfo/blob/master/src/backports/zoneinfo/_zoneinfo.py#L422
        //https://github.com/dateutil/dateutil/blob/master/src/dateutil/tz/tz.py#L1037
        //https://learn.microsoft.com/ja-jp/dotnet/standard/datetime/create-time-zones-without-adjustment-rules
        //https://learn.microsoft.com/ja-jp/dotnet/standard/datetime/create-time-zones-with-adjustment-rules
        public class PosixTimeZoneParser
        {
            public class TimeZoneInfo
            {
                public string StandardName { get; set; }
                public TimeSpan StandardOffset { get; set; }
                public string? DaylightName { get; set; }
                public TimeSpan? DaylightOffset { get; set; }
                public TransitionRule? StartRule { get; set; }
                public TransitionRule? EndRule { get; set; }

                public override string ToString()
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"Standard: {StandardName} UTC{FormatOffset(StandardOffset)}");

                    if (!string.IsNullOrEmpty(DaylightName))
                    {
                        sb.Append($", Daylight: {DaylightName} UTC{FormatOffset(DaylightOffset ?? TimeSpan.Zero)}");
                    }
                    else
                    {
                        sb.Append(", Daylight: None");
                    }

                    sb.Append($", Start: {StartRule}, End: {EndRule}");
                    return sb.ToString();
                }

                private string FormatOffset(TimeSpan ts)
                {
                    return ts.Ticks >= 0 ? $"+{ts:hh\\:mm}" : $"-{ts:hh\\:mm}";
                }
            }

            public class TransitionRule
            {
                public int Month { get; set; }
                public int Week { get; set; }
                public int DayOfWeek { get; set; }
                public TimeSpan Time { get; set; }

                [JsonIgnore]
                public DateTimeOffset CurrentYearDateTimeOffset
                {
                    get
                    {
                        // Get the DateTime in the specified year.
                        DateTime dateTime = GetDateTime(DateTime.Now.Year, this);

                        // Determine if daylight saving is in effect at this time.
                        TimeSpan offset = _tzInfo.StandardOffset; // Default to standard offset.
                        TimeSpan offset_std = offset;
                        TimeSpan offset_dst = _tzInfo.DaylightOffset ?? offset;

                        bool dst = false;
                        // If both start and end rules are defined, check for daylight saving.
                        if (_tzInfo.StartRule != null && _tzInfo.EndRule != null)
                        {
                            DateTime start = TransitionRule.GetDateTime(DateTime.Now.Year, _tzInfo.StartRule);
                            DateTime end = TransitionRule.GetDateTime(DateTime.Now.Year, _tzInfo.EndRule);

                            // Handle cases where start/end rules cross the year boundary.
                            if (start > end)
                            {
                                if (dateTime > start || dateTime <= end)
                                {
                                    dst = true;
                                    offset = _tzInfo.DaylightOffset ?? offset; // Apply daylight offset.
                                }
                            }
                            else
                            {
                                if (dateTime > start && dateTime <= end)
                                {
                                    dst = true;
                                    offset = _tzInfo.DaylightOffset ?? offset;
                                }
                            }
                        }
                        DateTimeOffset dt = new DateTimeOffset(dateTime);
                        var offset_st = offset.Hours.ToString("00") + ":" + offset.Minutes.ToString("00");
                        if (offset >= TimeSpan.Zero)
                        {
                            offset_st = "+" + offset_st;
                        }
                        string timest = dt.ToString("yyyy-MM-ddTHH:mm:ss" + offset_st);

                        DateTimeOffset parsedDateTime = ParseIso8601(timest);
                        Double offset_h = 0;
                        if (dst == false)
                        {
                            offset_h = offset_dst.TotalHours;
                        }
                        else
                        {
                            offset_h = offset_std.TotalHours;
                        }

                        TimeSpan newOffset = TimeSpan.FromHours(offset_h);
                        parsedDateTime = parsedDateTime.ToOffset(newOffset);

                        return parsedDateTime;

                    }
                }


                public static DateTimeOffset ParseIso8601(string iso8601String)
                {
                    try
                    {
                        return DateTimeOffset.Parse(iso8601String, null, DateTimeStyles.RoundtripKind);
                    }
                    catch (FormatException ex)
                    {
                        // パースに失敗した場合のエラーハンドリング
                        Console.WriteLine($"ISO 8601 形式の日時のパースに失敗しました: {ex.Message}");
                        throw;
                    }
                }

                // Store a reference to the TimeZoneInfo.
                [JsonIgnore] // Don't serialize this.
                internal TimeZoneInfo _tzInfo;


                // Helper function to get the specific DateTime
                public static DateTime GetDateTime(int year, TransitionRule rule)
                {
                    // Get the first day of the month
                    DateTime firstDayOfMonth = new DateTime(year, rule.Month, 1);

                    // Calculate the day of the week for the first day of the month
                    int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

                    // Calculate the difference between the target day of the week and the first day of the month
                    int diff = (rule.DayOfWeek - firstDayOfWeek + 7) % 7;

                    // Calculate the day of the month for the first occurrence of the target day of the week
                    int dayOfMonth = diff + 1;

                    // If it's not the first week, add the required number of weeks
                    if (rule.Week > 1)
                    {
                        dayOfMonth += (rule.Week - 1) * 7;
                    }

                    // Handle "last week" case (Week = 5)
                    if (rule.Week == 5)
                    {
                        // Get the last day of the month.
                        int lastDay = DateTime.DaysInMonth(year, rule.Month);

                        // Create a date at the end of the month and work backwards.
                        DateTime lastDayOfMonth = new DateTime(year, rule.Month, lastDay);
                        int lastDayOfWeek = (int)lastDayOfMonth.DayOfWeek;

                        int dayOffset = (lastDayOfWeek - rule.DayOfWeek + 7) % 7;
                        dayOfMonth = lastDay - dayOffset;

                        if (dayOfMonth < 1)  // Make sure the date remains within the month
                        {
                            dayOfMonth += 7;
                        }

                    }
                    else if (dayOfMonth > DateTime.DaysInMonth(year, rule.Month)) //Handle cases when days go over the month.
                    {
                        dayOfMonth -= 7;
                    }

                    // Construct the DateTime object
                    return new DateTime(year, rule.Month, dayOfMonth, rule.Time.Hours, rule.Time.Minutes, rule.Time.Seconds, DateTimeKind.Utc);

                }


                public override string ToString()
                {
                    return $"M{Month}.{Week}.{DayOfWeek}/{Time:hh\\:mm}";
                }
            }

            public static TimeZoneInfo Parse(string posixTz)
            {
                if (string.IsNullOrEmpty(posixTz))
                    throw new ArgumentException("POSIX timezone string cannot be empty.");

                posixTz = posixTz.Trim();

                var parts = posixTz.Split(',');
                if (parts.Length < 1 || parts.Length > 3)
                    throw new FormatException($"Invalid POSIX timezone format: {posixTz}");

                string stdDstPart = parts[0];
                TimeZoneInfo tzInfo = new TimeZoneInfo();

                // More robust regex to handle all cases, including those with <>
                var stdMatch = Regex.Match(stdDstPart, @"^(?:<(?<stdName>[^>]+)>|(?<stdName2>[A-Za-z]+))(?<stdOffset>[+-]?\d+(?::\d+)?)(?:(?:<(?<dstName>[^>]+)>|(?<dstName2>[A-Za-z]+))(?<dstOffset>[+-]?\d+(?::\d+)?))?");
                if (!stdMatch.Success)
                    throw new FormatException($"Invalid std/dst format: {stdDstPart}");

                // Extract standard time name and offset
                tzInfo.StandardName = stdMatch.Groups["stdName"].Success ? stdMatch.Groups["stdName"].Value : stdMatch.Groups["stdName2"].Value;
                tzInfo.StandardOffset = NegateOffset(ParseOffset(stdMatch.Groups["stdOffset"].Value));

                // Extract daylight saving time name and offset (if present)
                if (stdMatch.Groups["dstName"].Success || stdMatch.Groups["dstName2"].Success)
                {
                    tzInfo.DaylightName = stdMatch.Groups["dstName"].Success ? stdMatch.Groups["dstName"].Value : stdMatch.Groups["dstName2"].Value;
                    string dstOffsetStr = stdMatch.Groups["dstOffset"].Value;
                    tzInfo.DaylightOffset = !string.IsNullOrEmpty(dstOffsetStr) ? NegateOffset(ParseOffset(dstOffsetStr)) : tzInfo.StandardOffset + TimeSpan.FromHours(1);
                }
                else if (parts[0].Length != stdMatch.Value.Length)
                {
                    tzInfo.DaylightName = stdDstPart.Replace(tzInfo.StandardName + stdMatch.Groups["stdOffset"].Value, "");
                    string dstOffsetStr = "";
                    tzInfo.DaylightOffset = !string.IsNullOrEmpty(dstOffsetStr) ? NegateOffset(ParseOffset(dstOffsetStr)) : tzInfo.StandardOffset + TimeSpan.FromHours(1);
                }


                // Parse transition rules
                if (parts.Length > 1)
                    tzInfo.StartRule = ParseTransitionRule(parts[1], tzInfo); // Pass tzInfo
                if (parts.Length > 2)
                    tzInfo.EndRule = ParseTransitionRule(parts[2], tzInfo); // Pass tzInfo

                return tzInfo;
            }
            private static TimeSpan ParseOffset(string offsetStr)
            {
                bool isNegative = offsetStr.StartsWith("-");
                string cleanOffset = offsetStr.TrimStart('+', '-');

                if (cleanOffset.Contains(":"))
                {
                    var parts = cleanOffset.Split(':');
                    int hours = int.Parse(parts[0]);
                    int minutes = parts.Length > 1 ? int.Parse(parts[1]) : 0;
                    return (TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes)) * (isNegative ? -1 : 1);
                }
                else
                {
                    int hours = int.Parse(cleanOffset);
                    return TimeSpan.FromHours(hours) * (isNegative ? -1 : 1);
                }
            }

            private static TimeSpan NegateOffset(TimeSpan offset)
            {
                return -offset;
            }

            private static TransitionRule ParseTransitionRule(string rule, TimeZoneInfo tzInfo) // Add tzInfo parameter
            {
                var match = Regex.Match(rule, @"^M(\d+)\.(\d+)\.(\d+)(?:/(\d+(?::\d+)?))?$");
                if (!match.Success)
                    throw new FormatException($"Invalid transition rule: {rule}");

                int month = int.Parse(match.Groups[1].Value);
                int week = int.Parse(match.Groups[2].Value);
                int dayOfWeek = int.Parse(match.Groups[3].Value);
                string timeStr = match.Groups[4].Success ? match.Groups[4].Value : "2";

                if (month < 1 || month > 12 || week < 1 || week > 5 || dayOfWeek < 0 || dayOfWeek > 6)
                    throw new FormatException("Transition rule values out of range.");

                TimeSpan time = timeStr.Contains(":") ? TimeSpan.Parse(timeStr) : TimeSpan.FromHours(int.Parse(timeStr));
                return new TransitionRule
                {
                    Month = month,
                    Week = week,
                    DayOfWeek = dayOfWeek,
                    Time = time,
                    _tzInfo = tzInfo  // Store the TimeZoneInfo
                };
            }

            public class TransitionRuleConverter : JsonConverter<TransitionRule>
            {
                public override TransitionRule Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    throw new NotImplementedException();
                }

                public override void Write(Utf8JsonWriter writer, TransitionRule value, JsonSerializerOptions options)
                {
                    writer.WriteStartObject();
                    writer.WriteNumber("Month", value.Month);
                    writer.WriteNumber("Week", value.Week);
                    writer.WriteNumber("DayOfWeek", value.DayOfWeek);
                    writer.WriteString("Time", value.Time.ToString("hh\\:mm\\:ss"));
                    writer.WriteString("CurrentYearDate", value.CurrentYearDateTimeOffset.ToString("yyyy-MM-ddTHH:mm:sszzz")); // Use DateTimeOffset
                    writer.WriteEndObject();
                }
            }



            public static string LoadPosix(string posixTz)
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    var tzInfo = Parse(posixTz);

                    // Use System.Text.Json for serialization with custom converter
                    var options = new JsonSerializerOptions
                    {
                        //Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true,
                        Converters = { new TransitionRuleConverter() } // Add the converter
                    };
                    string json = JsonSerializer.Serialize(tzInfo, options);
                    string json_tmp = ($"{{\"Parsing\": \"{posixTz.Trim()}\",");
                    json_tmp += ($"\"Posix_Normalize\": \"{tzInfo.ToString()}\",");
                    json_tmp += (json.Substring(2));

                    sb.AppendLine(json_tmp);

                }
                catch (Exception ex)
                {
                    string json_error = JsonSerializer.Serialize(ex.Message);
                    sb.AppendLine(json_error);
                }
                return sb.ToString();
            }

            public static string test(string posixTz)
            {
                StringBuilder sb = new StringBuilder();


                sb.AppendLine(LoadPosix(posixTz));
                sb.AppendLine();
                return sb.ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string input = test_date.Text;
            if (TryParseDateTimeCutom(input, out DateTime dt))
            {
                Properties.Settings.Default.posix_testdate = test_date.Text;
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

        private void posix_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.view_posix_info = posix.Checked;
        }



        /*  python demo  googleのcolabで動くはず python dateutilの改変
       #https://colab.research.google.com/?hl=ja GOOGLE colabで貼り付けて　tzdatabaseの内容を表示 ＋　JSON化
       #tzdata自体はzone imfomation complier(ZIC) で作成されて　unix系だとman zdump で表示できる
       #pythonでdateutilの一部を改変してzdumpもどきともいう（）
       #dateutil/pytzのパーサーが古いレガシーのTZifしかたいおうしてないので
       #適切なメンテナンスをしないと2038年問題が起きる可能性がある transition_timeが32bitのため

       from io import DEFAULT_BUFFER_SIZE
       import json
       from datetime import datetime
       from dateutil import tz
       from dateutil.tz.tz import tzfile
       import struct

       tzname="Asia/Tokyo"

       def read_tzif(tzfile_path):
           with open(tzfile_path, 'rb') as f:
               # Read the header
               header = f.read(44)  # TZif headers are 44 bytes
               (magic, version, tzh_ttisgmtcnt, tzh_ttisstdcnt, tzh_leapcnt,
                tzh_timecnt, tzh_typecnt, tzh_charcnt) = struct.unpack('>4s c 15x 6I', header)

               if magic != b'TZif':
                   raise ValueError('Invalid TZif file')

               # Read the transition times
               if tzh_timecnt:
                   transition_times = struct.unpack('>%dl' % tzh_timecnt, f.read(tzh_timecnt * 4))
               else:
                   transition_times = []

               # Read the transition types
               if tzh_timecnt:
                    transition_types = struct.unpack(">%dB" % tzh_timecnt,
                                                 f.read(tzh_timecnt))
               else:
                    transition_types =[]

               # Read the local time types
               local_time_types = []
               for i in range(tzh_typecnt):
                   local_time_types.append(struct.unpack(">lbb", f.read(6)))

               abbr = f.read(tzh_charcnt).decode()


               # Create a list of transitions
               transitions = []
               for i in range(tzh_timecnt):
                   transition_time = transition_times[i]
                   local_time_type = local_time_types[transition_types[i]]
                   gmtoff, isdst, abbrind = local_time_type
                   transitions.append({
                       'transition_time': transition_time,
                       'gmt_offset': gmtoff/3600,   #//raw だとgmtoffのまま
                       "local" :cvt_local(transition_time),
                       'isdst': isdst,
                       "abbr": abbr[abbrind:abbr.find('\x00', abbrind)],
                       #"abbra": abbr
                   })

               return {
                   'version': version.decode(),
                   'transitions': transitions
               }

       def cvt_local(sec):
           global tzname
           dt_utc = datetime.utcfromtimestamp(sec)
           jst = tz.gettz(tzname)
           dt_jst = dt_utc.replace(tzinfo=tz.UTC).astimezone(jst)
           return dt_jst.strftime('%Y-%m-%dT%X%z')

       # パスを指定して、TZifファイルを読み込む
       tzif_path = '/usr/share/zoneinfo/'+tzname  # タイムゾーンファイルのパス
       tzif_data = read_tzif(tzif_path)

       for item in tzif_data["transitions"]:
           print(item)

       json_str = json.dumps(tzif_data)
       print(json_str)


       ##出力結果
       #{'transition_time': -2147483648, 'gmt_offset': 9.0, 'local': '1901-12-14T05:45:52+0900', 'isdst': 0, 'abbr': 'JST'}
       #{'transition_time': -683802000, 'gmt_offset': 10.0, 'local': '1948-05-02T01:00:00+1000', 'isdst': 1, 'abbr': 'JDT'}
       #{'transition_time': -672310800, 'gmt_offset': 9.0, 'local': '1948-09-12T00:00:00+0900', 'isdst': 0, 'abbr': 'JST'}
       #{'transition_time': -654771600, 'gmt_offset': 10.0, 'local': '1949-04-03T01:00:00+1000', 'isdst': 1, 'abbr': 'JDT'}
       #{'transition_time': -640861200, 'gmt_offset': 9.0, 'local': '1949-09-11T00:00:00+0900', 'isdst': 0, 'abbr': 'JST'}
       #{'transition_time': -620298000, 'gmt_offset': 10.0, 'local': '1950-05-07T01:00:00+1000', 'isdst': 1, 'abbr': 'JDT'}
       #{'transition_time': -609411600, 'gmt_offset': 9.0, 'local': '1950-09-10T00:00:00+0900', 'isdst': 0, 'abbr': 'JST'}
       #{'transition_time': -588848400, 'gmt_offset': 10.0, 'local': '1951-05-06T01:00:00+1000', 'isdst': 1, 'abbr': 'JDT'}
       #{'transition_time': -577962000, 'gmt_offset': 9.0, 'local': '1951-09-09T00:00:00+0900', 'isdst': 0, 'abbr': 'JST'}

       #{"version": "2", "transitions": [{"transition_time": -2147483648, "gmt_offset": 9.0, "local": "1901-12-14T05:45:52+0900", "isdst": 0, "abbr": "JST"}, {"transition_time": -683802000, "gmt_offset": 10.0, "local": "1948-05-02T01:00:00+1000", "isdst": 1, "abbr": "JDT"}, {"transition_time": -672310800, "gmt_offset": 9.0, "local": "1948-09-12T00:00:00+0900", "isdst": 0, "abbr": "JST"}, {"transition_time": -654771600, "gmt_offset": 10.0, "local": "1949-04-03T01:00:00+1000", "isdst": 1, "abbr": "JDT"}, {"transition_time": -640861200, "gmt_offset": 9.0, "local": "1949-09-11T00:00:00+0900", "isdst": 0, "abbr": "JST"}, {"transition_time": -620298000, "gmt_offset": 10.0, "local": "1950-05-07T01:00:00+1000", "isdst": 1, "abbr": "JDT"}, {"transition_time": -609411600, "gmt_offset": 9.0, "local": "1950-09-10T00:00:00+0900", "isdst": 0, "abbr": "JST"}, {"transition_time": -588848400, "gmt_offset": 10.0, "local": "1951-05-06T01:00:00+1000", "isdst": 1, "abbr": "JDT"}, {"transition_time": -577962000, "gmt_offset": 9.0, "local": "1951-09-09T00:00:00+0900", "isdst": 0, "abbr": "JST"}]}

                    */


    }
}

