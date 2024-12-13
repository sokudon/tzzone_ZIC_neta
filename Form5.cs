using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using static System.TimeZoneInfo;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using System.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using Force.Crc32;
using System.Security.Cryptography;


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

        }

        public byte[] Bigval64(byte[] bs,int pos) {
            byte[] swapbin = new byte[8];
            Array.ConstrainedCopy(bs, pos, swapbin, 0, 8);
            Array.Reverse(swapbin);
            return swapbin;
        }

        public byte[] Bigval(byte[] bs, int pos)
        {
            byte[] swapbin = new byte[4];
            Array.ConstrainedCopy(bs, pos, swapbin, 0, 4);
            Array.Reverse(swapbin);
            return swapbin;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog ofd = new OpenFileDialog();
                //ダイアログを表示する
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ofd.InitialDirectory = Properties.Settings.Default.lasttzdatapath;
                    ofd.Title = "unix usr/share/tzinfoやpython pytz dateutilのtzdatabaseフォルダのバイナリふぁいるを選択してください";
                    //python dateutil一部をC#ぽくしただけのもの 下にgoogle colabでうごくやつも貼っとく 
                    //zicバイナリの情報　https://tex2e.github.io/rfc-translater/html/rfc8536.html
                    string tzdata = ofd.FileName;// "Tokyo";
                    if (File.Exists(tzdata))
                    {
                        System.IO.FileStream fs = new FileStream(tzdata, FileMode.Open, FileAccess.Read);
                        byte[] bs = new byte[fs.Length];
                        fs.Read(bs, 0, bs.Length);
                        fs.Close();
                        var encoding = Encoding.GetEncoding(0);
                        var header = encoding.GetString(bs).Substring(0, 4);
                        string footer = "";
                        int nexttzpos = -1;
                        int finalpos = -1;
                        string versionn="";
                        bool tzcv= tzutc.Checked;

                        StringBuilder sb = new StringBuilder();

                        try
                        {
                            if (header.Contains("TZif") == false)
                            {
                                sb.AppendLine("Zone infomation compiler で作成されたtzdataバイナリTZifファイルではありません");
                            }
                            else
                            {
                                //python tzinfoのうるう秒こみのDBには非対応
                                //#unix/linux系 google colabとかもcygwinとか
                                //#/usr/share/lib/zoneinfo/

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

                                    int tzh_ttisgmtcnt = BitConverter.ToInt32(Bigval(bs, pos), 0);

                                    int tzh_ttisstdcnt = BitConverter.ToInt32(Bigval(bs, pos + 4), 0);

                                    int tzh_leapcnt = BitConverter.ToInt32(Bigval(bs, pos + 8), 0);

                                    int tzh_timecnt = BitConverter.ToInt32(Bigval(bs, pos + 12), 0);

                                    int tzh_typecnt = BitConverter.ToInt32(Bigval(bs, pos + 16), 0);

                                    int tzh_charcnt = BitConverter.ToInt32(Bigval(bs, pos + 20), 0);



                                    //C# 適切なメンテナンスをしないと2038年問題が起きる可能性がある transition_timeが32bitのため
                                    int[] transition_times = new int[tzh_timecnt];
                                    int[] transition_types = new int[tzh_timecnt];

                                    pos = 44;
                                    if (tzh_timecnt != 0)
                                    {
                                        for (int i = 0; i < tzh_timecnt; i++)
                                        {
                                            transition_times[i] = BitConverter.ToInt32(Bigval(bs, pos + 4 * i), 0);
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
                                        local_time_types_gmt[i] = BitConverter.ToInt32(Bigval(bs, pos + i * 6), 0);
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
                                            int hours = (int)Math.Abs(totalHours); // 絶対値を取った整数部の時間
                                            double totalMinutes = Math.Abs(utcOffset.TotalMinutes % 60); // 分部分（絶対値）

                                            // フォーマット
                                            string formattedOffset = $"{sign}{hours:00}:{totalMinutes:00.00}";

                                            try
                                            {
                                                // HH:MM の形式に変換可能かどうかを判定
                                                if (utcOffset.Seconds == 0)
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


                                    int tzh_ttisgmtcntn = 0;
                                    int tzh_ttisstdcntn = 0;
                                    int tzh_leapcntn = 0;
                                    int tzh_timecntn = 0;
                                    int tzh_typecntn = 0;
                                    int tzh_charcntn = 0;

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
                                                if (tzcv)
                                                {

                                                    transitionsn[i][1] = Convert.ToString(Convert.ToDouble(local_time_types_gmtn[type]) / 3600);
                                                    double localTimeOffseth = Convert.ToDouble(local_time_types_gmtn[type]) / 3600;

                                                    long unixTimestamp = transition_timesn[i];


                                                    TimeSpan utcOffset = TimeSpan.FromHours(localTimeOffseth);
                                                    // TotalHours で符号を判定
                                                    double totalHours = utcOffset.TotalHours; // 全体の時間（小数部分を含む）
                                                    string sign = totalHours >= 0 ? "+" : "-";
                                                    // HH と MM を取得
                                                    int hours = (int)Math.Abs(totalHours); // 絶対値を取った整数部の時間
                                                    double totalMinutes = Math.Abs(utcOffset.TotalMinutes % 60); // 分部分（絶対値）

                                                    // フォーマット
                                                    string formattedOffset = $"{sign}{hours:00}:{totalMinutes:00.00}";

                                                    try
                                                    {
                                                        // HH:MM の形式に変換可能かどうかを判定
                                                        if (utcOffset.Seconds == 0)
                                                        {
                                                            // HH:MM 形式が可能
                                                            DateTimeOffset dateTimeWithOffset = DateTimeOffset
                                                                .FromUnixTimeSeconds(unixTimestamp)
                                                                .ToOffset(utcOffset);

                                                            // 標準フォーマットで変換
                                                            string formattedDateh = dateTimeWithOffset.ToString("yyyy-MM-ddTHH:mm:sszzz");
                                                            transitionsn[i][0] = formattedDateh;
                                                        }
                                                        else
                                                        {
                                                            // カスタムフォーマット
                                                            DateTimeOffset originalTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).AddHours(localTimeOffseth);
                                                            string formattedDate = $"{originalTime:yyyy-MM-ddTHH:mm:ss.fff} {formattedOffset}";
                                                            transitionsn[i][0] = formattedDate;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        sb.AppendLine(ex.ToString());
                                                        break;
                                                    }


                                                }
                                                sb.Append(transitionsn[i][0]);
                                                sb.Append(",");
                                                sb.Append(transitionsn[i][1]);
                                                sb.Append(",");
                                                sb.Append(transitionsn[i][2]);
                                                sb.Append(",");
                                                sb.AppendLine(transitionsn[i][3]);
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

                                    string crc32 = Crc32Algorithm.Compute(bs).ToString("X");
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
                                    sb.AppendLine("2nd tzh_ttisgmtcnt:" + tzh_ttisgmtcntn.ToString());
                                    sb.AppendLine("2nd tzh_ttisstdcnt :" + tzh_ttisstdcntn.ToString());
                                    sb.AppendLine("2nd tzh_leapcnt:" + tzh_leapcntn.ToString());
                                    sb.AppendLine("2nd tzh_timecnt:" + tzh_timecntn.ToString());
                                    sb.AppendLine("2nd tzh_typecnt:" + tzh_typecntn.ToString());
                                    sb.AppendLine("2nd tzh_charcnt:" + tzh_charcntn.ToString());




                                }
                            }
                            Properties.Settings.Default.lasttzdatapath = System.IO.Path.GetDirectoryName(tzdata);
                        }
                        catch (Exception ex) {
                            sb.AppendLine(ex.ToString());
                        }
                        textBox1.Text = sb.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.ToString();
                throw;
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

