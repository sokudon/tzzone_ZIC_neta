using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;


//https://claude.ai/chat/5e3294c3-7dec-4e02-9be6-b82196e7bab1

namespace TZPASER
{
    public class TimeZoneOffsetParser
    {
        /// <summary>
        /// GMT/UTC オフセット文字列をTimeSpanに変換
        /// サポートする形式: 
        /// +HHMM, -HHMM, +HH:MM, -HH:MM
        /// +HHMMSS, -HHMMSS
        /// </summary>
        public static TimeSpan ParseGmtOffset(string offsetString)
        {
            // 入力文字列のバリデーション
            if (string.IsNullOrWhiteSpace(offsetString))
                throw new ArgumentException("オフセット文字列が空です");

            // 正規表現でオフセットパターンを抽出
            var offsetRegex = new Regex(@"^([+-])(\d{1,2})(?::?(\d{2}))?(?::?(\d{2}))?$");
            var match = offsetRegex.Match(offsetString);

            if (!match.Success)
                throw new FormatException($"無効なオフセット形式: {offsetString}");

            // 符号、時、分、秒を抽出
            bool isNegative = match.Groups[1].Value == "-";
            int hours = int.Parse(match.Groups[2].Value);
            int minutes = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0;
            int seconds = match.Groups[4].Success ? int.Parse(match.Groups[4].Value) : 0;

            // TimeSpanに変換
            var offset = new TimeSpan(hours, minutes, seconds);
            return isNegative ? offset.Negate() : offset;
        }

        /// <summary>
        /// オフセット文字列の推奨フォーマット
        /// </summary>
        public static string FormatGmtOffset(TimeSpan offset)
        {
            // ISO 8601形式 (+/-)HH:mm:ss
            return $"{(offset < TimeSpan.Zero ? "-" : "+")}{Math.Abs(offset.Hours):D2}:{Math.Abs(offset.Minutes):D2}:{Math.Abs(offset.Seconds):D2}";
        }

        /// <summary>
        /// アフリカなどの特殊な時差を処理するサンプルメソッド
        /// </summary>
        public static TimeSpan NormalizeOffset(string rawOffset)
        {
            try
            {
                // 通常のオフセット形式に変換
                return ParseGmtOffset(rawOffset);
            }
            catch
            {
                // 特殊なケース：秒単位のオフセットを処理
                var specialRegex = new Regex(@"^([+-])(\d+)$");
                var match = specialRegex.Match(rawOffset);

                if (match.Success)
                {
                    int totalSeconds = int.Parse(match.Groups[2].Value);
                    var offset = TimeSpan.FromSeconds(totalSeconds);
                    return match.Groups[1].Value == "-" ? offset.Negate() : offset;
                }

                throw new FormatException($"サポートされていないオフセット形式: {rawOffset}");
            }
        }


        public static string getoffset(DateTimeOffset dt, string format, TimeZoneInfo tz)
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

        //https://chatgpt.com/c/675c1ac5-6f48-800f-b683-ae9745604c89
        public static string ToCustomFormat(double value, bool useColon)
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

        public static string getleft(TimeSpan tspan,string leftformat)
        {
            //string leftformat = Properties.Settings.Default.lefttimeformat;

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
    }

    //    public static void Main()
    //    {
    //        // 使用例
    //        string[] testOffsets = {
    //        "+0200",     // 標準的な形式
    //        "-0530",     // 負のオフセット
    //        "+02:30",    // コロン付き形式
    //        "+023000",   // HHMMSS形式
    //        "+02:30:00", // ISO 8601形式
    //        "+10800"     // 秒単位のオフセット
    //    };

    //        foreach (var offset in testOffsets)
    //        {
    //            try
    //            {
    //                var timeSpan = ParseGmtOffset(offset);
    //                Console.WriteLine($"入力: {offset}");
    //                Console.WriteLine($"TimeSpan: {timeSpan}");
    //                Console.WriteLine($"フォーマット: {FormatGmtOffset(timeSpan)}\n");
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine($"エラー: {ex.Message}\n");
    //            }
    //        }
    //    }

    public class FastDateTimeParsing
    {
        //https://chatgpt.com/c/6760cf68-e848-800f-aed6-b38389647855
        // 高速な日付パターンマッチング用の正規表現
        private static readonly Regex[] DatePatterns = {
    new Regex(@"^\d{4}[\-/]\d{1,2}[\-/]\d{1,2}$"), // YYYY-MM-DD
    new Regex(@"^\d{1,2}/\d{1,2}/\d{4}$"), // MM/DD/YYYY
    new Regex(@"^\d{4}[\-/]\d{1,2}[\-/]\d{1,2} \d{1,2}:\d{1,2}:\d{1,2}$"), // YYYY-MM-DD HH:mm:ss
    new Regex(@"^\d{4}[\-/]\d{1,2}[\-/]\d{1,2} \d{1,2}:\d{1,2}$"), // YYYY-MM-DD HH:mm
    new Regex(@"^\d{4}[\-/]\d{1,2}[\-/]\d{1,2} \d{1,2}$"), // YYYY-MM-DD HH
    new Regex(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?(Z|[\+\-]\d{2}:\d{2})?$") // ISO 8601
};

        private static string [] formats = new[]
                   {
         "yyyy-M-d",                   // YYYY-M-D (月・日が1桁でもOK)
        "yyyy/M/d",                   // YYYY/M/D
        "M/d/yyyy",                   // M/D/YYYY
        "yyyy-M-d H:m:s",             // YYYY-M-D H:M:S
        "yyyy-M-d H:m",               // YYYY-M-D H:M
        "yyyy-M-d H",                 // YYYY-M-D H
        "yyyy-MM-ddTHH:mm",           // ISO 8601 basic
        "yyyy-MM-ddTHH:mm:ss",        // ISO 8601 時間のみ
        "yyyy-MM-ddTHH:mm:ssZ",       // ISO 8601 UTC (Z付き)
        "yyyy-MM-ddTHH:mm:sszzz",     // ISO 8601 タイムゾーンオフセット付き (+09:00)
        "yyyy-MM-ddTHH:mm:ss.fff", // ISO 8601 with milliseconds
        "yyyy-MM-ddTHH:mm:ssK", // ISO 8601 with timezone (Z or ±hh:mm)
        "yyyy-MM-ddTHH:mm:ss.fffK", // ISO 8601 with milliseconds +Z
    };

        public static bool TryParseFastDateTime(string input, out DateTime result)
        {
            result = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            Regex rg = new Regex(@"/");
            input = rg.Replace(input,"-");

            // パターンマッチングで事前フィルタリング
            foreach (var pattern in DatePatterns)
            {
                if (pattern.IsMatch(input))
                {
                    // TryParseExactで解析を試みる
                    return DateTime.TryParseExact(
                        input,
                        formats,
                        System.Globalization.CultureInfo.InvariantCulture,
                        //DateTimeStyles.None,  localparsr
                        System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal,
                        out result);
                }
            }

            return false;
        }

        //// パフォーマンス比較メソッド
        //public static void CompareParsing()
        //{
        //    string[] testDates = {
        //    "2023-12-16",
        //    "2023/12/16",
        //    "12/16/2023",
        //    "2023-12-16 14:30:45",
        //    "2023-12-16T14:30:45Z",
        //    "2023-12-16T14:30:45.123Z"
        //};

        //    // DateTime.TryParse
        //    var standardWatch = Stopwatch.StartNew();
        //    foreach (var date in testDates)
        //    {
        //        DateTime.TryParse(date, out _);
        //    }
        //    standardWatch.Stop();

        //    // カスタム高速パース
        //    var fastWatch = Stopwatch.StartNew();
        //    foreach (var date in testDates)
        //    {
        //        TryParseFastDateTime(date, out _);
        //    }
        //    fastWatch.Stop();

        //    Console.WriteLine($"標準TryParse時間: {standardWatch.ElapsedMilliseconds}ms");
        //    Console.WriteLine($"高速パース時間: {fastWatch.ElapsedMilliseconds}ms");
        //}

        //public static void Main()
        //{
        //    // パース検証
        //    string[] validDates = {
        //    "2023-12-16",
        //    "2023/12/16",
        //    "2023-12-16 14:30:45",
        //    "2023-12-16T14:30:45Z"
        //};

        //    string[] invalidDates = {
        //    "2023/13/45",  // 無効な日付
        //    "hello",       // 文字列
        //    ""             // 空文字
        //};

        //    Console.WriteLine("--- 有効な日付のパース ---");
        //    foreach (var date in validDates)
        //    {
        //        if (TryParseFastDateTime(date, out DateTime result))
        //            Console.WriteLine($"{date} → {result}");
        //        else
        //            Console.WriteLine($"{date} → パース失敗");
        //    }

        //    Console.WriteLine("\n--- 無効な日付のパース ---");
        //    foreach (var date in invalidDates)
        //    {
        //        if (TryParseFastDateTime(date, out DateTime result))
        //            Console.WriteLine($"{date} → {result}");
        //        else
        //            Console.WriteLine($"{date} → パース失敗");
        //    }

        //    // パフォーマンス比較
        //    CompareParsing();
        //}
    }


    public class RFC2822DateTimeParser
    {
        // タイムゾーンマッピング
        private static readonly Dictionary<string, TimeSpan> TimeZoneOffsets = new Dictionary<string, TimeSpan>(StringComparer.OrdinalIgnoreCase)
    {
        // 主要なタイムゾーン
        {"UTC", TimeSpan.Zero},
        {"GMT", TimeSpan.Zero},

        // 米国タイムゾーン
        {"EST", TimeSpan.FromHours(-5)},  // 東部標準時
        {"EDT", TimeSpan.FromHours(-4)},  // 東部夏時間
        {"CST", TimeSpan.FromHours(-6)},  // 中部標準時
        {"CDT", TimeSpan.FromHours(-5)},  // 中部夏時間
        {"MST", TimeSpan.FromHours(-7)},  // 山地標準時
        {"MDT", TimeSpan.FromHours(-6)},  // 山地夏時間
        {"PST", TimeSpan.FromHours(-8)},  // 太平洋標準時
        {"PDT", TimeSpan.FromHours(-7)},  // 太平洋夏時間

        // その他の主要タイムゾーン
        {"CEST", TimeSpan.FromHours(2)},  // 中央ヨーロッパ夏時間
        {"CET", TimeSpan.FromHours(1)},   // 中央ヨーロッパ時間
        {"JST", TimeSpan.FromHours(9)},   // 日本標準時
    };

        // RFC2822形式の正規表現
        private static readonly Regex[] RFC2822Patterns = {
        new Regex(@"^(?:(?:Mon|Tue|Wed|Thu|Fri|Sat|Sun), )?(\d{1,2}) (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) (\d{4}) (\d{1,2}):(\d{1,2}):(\d{1,2}) ([+-]\d{4}|[A-Z]{2,3})$", RegexOptions.Compiled),
        new Regex(@"^(?:(?:Mon|Tue|Wed|Thu|Fri|Sat|Sun), )?(\d{1,2}) (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) (\d{4}) (\d{1,2}):(\d{1,2}) ([+-]\d{4}|[A-Z]{2,3})$", RegexOptions.Compiled),
        new Regex(@"^(?:(?:Mon|Tue|Wed|Thu|Fri|Sat|Sun), )?(\d{1,2}) (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) (\d{4}) (\d{1,2}) ([+-]\d{4}|[A-Z]{2,3})$", RegexOptions.Compiled),
        new Regex(@"^(?:(?:Mon|Tue|Wed|Thu|Fri|Sat|Sun), )?(\d{1,2}) (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) (\d{4}) ([+-]\d{4}|[A-Z]{2,3})$", RegexOptions.Compiled),

    };

        // 月名からintへのマッピング
        private static readonly Dictionary<string, int> MonthNameToNumber = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
    {
        {"Jan", 1}, {"Feb", 2}, {"Mar", 3}, {"Apr", 4},
        {"May", 5}, {"Jun", 6}, {"Jul", 7}, {"Aug", 8},
        {"Sep", 9}, {"Oct", 10}, {"Nov", 11}, {"Dec", 12}
    };

        // RFC2822に対応するフォーマット文字列の配列
        private static string[] formats = new[]
        {
        "ddd, dd MMM yyyy HH:mm:ss zzz", // 曜日 + 時間 + タイムゾーン
        "dd MMM yyyy HH:mm:ss zzz",     // 時間 + タイムゾーン
        "ddd, dd MMM yyyy HH:mm zzz",   // 曜日 + 時間(分まで) + タイムゾーン
        "dd MMM yyyy HH:mm zzz",        // 時間(分まで) + タイムゾーン
        "ddd, dd MMM yyyy HH zzz",      // 曜日 + 時間(時まで) + タイムゾーン
        "dd MMM yyyy HH zzz",           // 時間(時まで) + タイムゾーン
        "ddd, dd MMM yyyy zzz",         // 曜日 + タイムゾーン
        "dd MMM yyyy zzz"               // タイムゾーン
    };

        /// <summary>
        /// RFC2822形式の日付文字列をDateTime型に変換
        /// </summary>
        public static bool TryParseRFC2822DateTime(string input, out DateTime result)
        {
            result = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            foreach (var pattern in RFC2822Patterns)
            {
                var match = pattern.Match(input);
                if (match.Success)
                {
                    try
                    {
                        // 日付コンポーネントの抽出
                        int day = int.Parse(match.Groups[1].Value);
                        int month = MonthNameToNumber[match.Groups[2].Value];
                        int year = int.Parse(match.Groups[3].Value);

                        int hour = 0;
                        int minute = 0;
                        int second = 0;
                        int rfcstring = match.Groups.Count;
                        if (rfcstring >= 6) { 
                            hour = int.Parse(match.Groups[4].Value);
                        }
                        if (rfcstring >= 7)
                        {
                            minute = int.Parse(match.Groups[5].Value);
                    }
                        if (rfcstring >= 8)
                        {
                            second = int.Parse(match.Groups[6].Value);
                        }

                Regex offsetrg = new Regex(@"([+-]\d{4}|[A-Z]{2,3})$");
                        Match m = offsetrg.Match(input);
                        string tzInfo = m.Value; //match.Groups[7].Value;

                        TimeSpan offset = ParseTimeZoneOffset(tzInfo);
                        // オフセットを文字列形式に変換
                        string offsetString = FormatOffset(offset);

                        input = input.Replace(tzInfo, offsetString);


                        string date = year.ToString("0000") + "-" + month.ToString("00") + "-" +
                            day.ToString("00") + "T" + hour.ToString("00") + ":"
                            + minute.ToString("00") + ":" + second.ToString("00") + offsetString;

                        if (FastDateTimeParsing.TryParseFastDateTime(date, out result))
                        {
                            return true;
                        }

                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// タイムゾーンオフセットを "Z" または "+09:00" の形式に変換
        /// </summary>
        private static string FormatOffset(TimeSpan offset)
        {
            if (offset == TimeSpan.Zero)
                return "Z";

            string sign = offset.TotalMinutes < 0 ? "-" : "+";
            int hours = Math.Abs(offset.Hours);
            int minutes = Math.Abs(offset.Minutes);

            return $"{sign}{hours:D2}:{minutes:D2}";
        }

        // タイムゾーンオフセットの解析
        private static TimeSpan ParseTimeZoneOffset(string tzInfo)
        {
            // 定義済みタイムゾーンチェック
            if (TimeZoneOffsets.TryGetValue(tzInfo, out TimeSpan predefinedOffset))
                return predefinedOffset;

            // +/-HHMM形式のオフセット解析
            var offsetMatch = Regex.Match(tzInfo, @"^([+-])(\d{2})(\d{2})$");
            if (offsetMatch.Success)
            {
                int hours = int.Parse(offsetMatch.Groups[2].Value);
                int minutes = int.Parse(offsetMatch.Groups[3].Value);
                if (offsetMatch.Groups[1].Value == "-")
                    hours = -hours;

                return new TimeSpan(hours, minutes, 0);
            }

            // デフォルトはUTC
            return TimeSpan.Zero;
        }

        //public static void Main()
        //{
        //    // テスト用の日付文字列
        //    string[] testDates = {
        //    "Wed, 02 Oct 2002 13:00:00 GMT",       // GMT形式
        //    "02 Oct 2002 13:00:00 PST",            // PST形式
        //    "02 Oct 2002 13:00:00 -0700",          // オフセット形式
        //    "Wed, 02 Oct 2002 13:00:00 EDT",       // EDT形式
        //};

        //    Console.WriteLine("RFC2822日付パース結果:");
        //    foreach (var dateStr in testDates)
        //    {
        //        if (TryParseRFC2822DateTime(dateStr, out DateTime result))
        //        {
        //            Console.WriteLine($"入力: {dateStr}");
        //            Console.WriteLine($"パース結果: {result.ToUniversalTime()} UTC");
        //            Console.WriteLine($"ローカル時間: {result.ToLocalTime()}");
        //            Console.WriteLine();
        //        }
        //        else
        //        {
        //            Console.WriteLine($"パース失敗: {dateStr}\n");
        //        }
        //    }
        //}
    }


}