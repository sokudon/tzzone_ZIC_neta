using System.Collections.Generic;
using System.Text;
using System;
using System.Windows.Forms;

//https://grok.com/chat/2761bb43-4dac-4fcb-b526-629e06c76c1c
namespace neta
{
    public partial class Form8 : Form
    {
        private ComboBox inputEncodingComboBox;
        private ComboBox outputEncodingComboBox;
        private Label inputLabel;
        private Label outputLabel;
        private Label selectedInputEncoding;
        private Label selectedOutputEncoding;

        public Form8()
        {
            InitializeComponent();
            InitializeEncodingSelectors();
        }
        // Custom class to hold encoding information
        public class CustomEncodingInfo
        {
            public string DisplayName { get; set; }
            public int CodePage { get; set; }

            public override string ToString()
            {
                return DisplayName; // This makes the ComboBox display the DisplayName
            }
        }

        private void InitializeEncodingSelectors()
        {
            // よく使用されるエンコーディングのリストを作成
            var encodings = new List<CustomEncodingInfo>
            {
                new CustomEncodingInfo { DisplayName = "cp65001 utf-8 Unicode (UTF-8)", CodePage = 65001 },
                new CustomEncodingInfo { DisplayName = "cp57011 x-iscii-pa ISCII パンジャブ語", CodePage = 57011 },
                new CustomEncodingInfo { DisplayName = "cp57010 x-iscii-gu ISCII グジャラート語", CodePage = 57010 },
                new CustomEncodingInfo { DisplayName = "cp57009 x-iscii-ma ISCII マラヤーラム語", CodePage = 57009 },
                new CustomEncodingInfo { DisplayName = "cp57008 x-iscii-ka ISCII カンナダ語", CodePage = 57008 },
                new CustomEncodingInfo { DisplayName = "cp57007 x-iscii-or ISCII Odia", CodePage = 57007 },
                new CustomEncodingInfo { DisplayName = "cp57006 x-iscii-as ISCII アッサム語", CodePage = 57006 },
                new CustomEncodingInfo { DisplayName = "cp57005 x-iscii-te ISCII テルグ語", CodePage = 57005 },
                new CustomEncodingInfo { DisplayName = "cp57004 x-iscii-ta ISCII タミール語", CodePage = 57004 },
                new CustomEncodingInfo { DisplayName = "cp57003 x-iscii-be ISCII Bangla", CodePage = 57003 },
                new CustomEncodingInfo { DisplayName = "cp57002 x-iscii-de ISCII デバナガリ文字", CodePage = 57002 },
                new CustomEncodingInfo { DisplayName = "cp54936 GB18030 Windows XP 以降: GB18030 簡体字中国語 (4 バイト)", CodePage = 54936 },
                new CustomEncodingInfo { DisplayName = "cp52936 hz-gb-2312 HZ-GB2312 簡体字中国語", CodePage = 52936 },
                new CustomEncodingInfo { DisplayName = "cp51949 euc-kr EUC 韓国語", CodePage = 51949 },
                new CustomEncodingInfo { DisplayName = "cp51936 EUC-CN EUC 簡体字中国語", CodePage = 51936 },
                new CustomEncodingInfo { DisplayName = "cp51932 euc-jp EUC 日本語", CodePage = 51932 },
                new CustomEncodingInfo { DisplayName = "cp50227 x-cp50227 ISO 2022 簡体字中国語", CodePage = 50227 },
                new CustomEncodingInfo { DisplayName = "cp50225 iso-2022-kr ISO 2022 韓国語", CodePage = 50225 },
                new CustomEncodingInfo { DisplayName = "cp50222 iso-2022-jp ISO 2022 日本語 JIS X 0201-1989", CodePage = 50222 },
                new CustomEncodingInfo { DisplayName = "cp50221 csISO2022JP ISO 2022 日本語(半角カタカナ)", CodePage = 50221 },
                new CustomEncodingInfo { DisplayName = "cp50220 iso-2022-jp 半角カタカナを持たないISO 2022日本語", CodePage = 50220 },
                new CustomEncodingInfo { DisplayName = "cp38598 iso-8859-8-i ISO 8859-8 ヘブライ語", CodePage = 38598 },
                new CustomEncodingInfo { DisplayName = "cp29001 x-Europa ヨーロッパ 3", CodePage = 29001 },
                new CustomEncodingInfo { DisplayName = "cp28605 iso-8859-15 ISO 8859-15 Latin 9", CodePage = 28605 },
                new CustomEncodingInfo { DisplayName = "cp28603 iso-8859-13 ISO 8859-13 エストニア語", CodePage = 28603 },
                new CustomEncodingInfo { DisplayName = "cp28599 iso-8859-9 ISO 8859-9 トルコ語", CodePage = 28599 },
                new CustomEncodingInfo { DisplayName = "cp28598 iso-8859-8 ISO 8859-8 ヘブライ語", CodePage = 28598 },
                new CustomEncodingInfo { DisplayName = "cp28597 iso-8859-7 ISO 8859-7 ギリシャ語", CodePage = 28597 },
                new CustomEncodingInfo { DisplayName = "cp28596 iso-8859-6 ISO 8859-6 アラビア語", CodePage = 28596 },
                new CustomEncodingInfo { DisplayName = "cp28595 iso-8859-5 ISO 8859-5 キリル語", CodePage = 28595 },
                new CustomEncodingInfo { DisplayName = "cp28594 iso-8859-4 ISO 8859-4 バルティック", CodePage = 28594 },
                new CustomEncodingInfo { DisplayName = "cp28593 iso-8859-3 ISO 8859-3 ラテン 3", CodePage = 28593 },
                new CustomEncodingInfo { DisplayName = "cp28592 iso-8859-2 ISO 8859-2 中央ヨーロッパ", CodePage = 28592 },
                new CustomEncodingInfo { DisplayName = "cp28591 iso-8859-1 ISO 8859-1 ラテン 1", CodePage = 28591 },
                new CustomEncodingInfo { DisplayName = "cp21866 koi8-u ウクライナ語 (KOI8-U)", CodePage = 21866 },
                new CustomEncodingInfo { DisplayName = "cp21025 cp1025 IBM EBCDIC キリル文字Serbian-Bulgarian", CodePage = 21025 },
                new CustomEncodingInfo { DisplayName = "cp20949 x-cp20949 韓国 Korean-wansung-unicode", CodePage = 20949 },
                new CustomEncodingInfo { DisplayName = "cp20936 x-cp20936 簡体字中国語 (GB2312)", CodePage = 20936 },
                new CustomEncodingInfo { DisplayName = "cp20932 EUC-JP 日本語 (JIS 0208-1990 および 0212-1990)", CodePage = 20932 },
                new CustomEncodingInfo { DisplayName = "cp20924 IBM00924 IBM EBCDIC Latin 1/Open System", CodePage = 20924 },
                new CustomEncodingInfo { DisplayName = "cp20905 IBM905 IBM EBCDIC トルコ語", CodePage = 20905 },
                new CustomEncodingInfo { DisplayName = "cp20880 IBM880 IBM EBCDIC キリルロシア語", CodePage = 20880 },
                new CustomEncodingInfo { DisplayName = "cp20871 IBM871 IBM EBCDIC アイスランド語", CodePage = 20871 },
                new CustomEncodingInfo { DisplayName = "cp20866 koi8-r ロシア語 (KOI8-R)", CodePage = 20866 },
                new CustomEncodingInfo { DisplayName = "cp20838 IBM-タイ語 IBM EBCDIC タイ語", CodePage = 20838 },
                new CustomEncodingInfo { DisplayName = "cp20833 x-EBCDIC-KoreanExtended IBM EBCDIC 韓国語拡張", CodePage = 20833 },
                new CustomEncodingInfo { DisplayName = "cp20424 IBM424 IBM EBCDIC ヘブライ語", CodePage = 20424 },
                new CustomEncodingInfo { DisplayName = "cp20423 IBM423 IBM EBCDIC ギリシャ語", CodePage = 20423 },
                new CustomEncodingInfo { DisplayName = "cp20420 IBM420 IBM EBCDIC アラビア語", CodePage = 20420 },
                new CustomEncodingInfo { DisplayName = "cp20297 IBM297 IBM EBCDIC France", CodePage = 20297 },
                new CustomEncodingInfo { DisplayName = "cp20290 IBM290 IBM EBCDIC 日本語カタカナ拡張", CodePage = 20290 },
                new CustomEncodingInfo { DisplayName = "cp20285 IBM285 IBM EBCDIC イギリス", CodePage = 20285 },
                new CustomEncodingInfo { DisplayName = "cp20284 IBM284 IBM EBCDIC Latin America-Spain", CodePage = 20284 },
                new CustomEncodingInfo { DisplayName = "cp20280 IBM280 IBM EBCDIC イタリア", CodePage = 20280 },
                new CustomEncodingInfo { DisplayName = "cp20278 IBM278 IBM EBCDIC Finland-Sweden", CodePage = 20278 },
                new CustomEncodingInfo { DisplayName = "cp20277 IBM277 IBM EBCDIC Denmark-Norway", CodePage = 20277 },
                new CustomEncodingInfo { DisplayName = "cp20273 IBM273 IBM EBCDIC Germany", CodePage = 20273 },
                new CustomEncodingInfo { DisplayName = "cp20269 x-cp20269 ISO 6937 スペースなしアクサン", CodePage = 20269 },
                new CustomEncodingInfo { DisplayName = "cp20261 x-cp20261 T. 61", CodePage = 20261 },
                new CustomEncodingInfo { DisplayName = "cp20127 us-ascii US-ASCII (7 ビット)", CodePage = 20127 },
                new CustomEncodingInfo { DisplayName = "cp20108 x-IA5-ノルウェー語 IA5 ノルウェー語 (7 ビット)", CodePage = 20108 },
                new CustomEncodingInfo { DisplayName = "cp20107 x-IA5-スウェーデン語 IA5 スウェーデン語 (7 ビット)", CodePage = 20107 },
                new CustomEncodingInfo { DisplayName = "cp20106 x-IA5-ドイツ語 IA5 ドイツ語 (7 ビット)", CodePage = 20106 },
                new CustomEncodingInfo { DisplayName = "cp20105 x-IA5 IA5 (IRV International Alphabet No. 5, 7-bit)", CodePage = 20105 },
                new CustomEncodingInfo { DisplayName = "cp20005 x-cp20005 Wang 台湾", CodePage = 20005 },
                new CustomEncodingInfo { DisplayName = "cp20004 x-cp20004 文字放送 (台湾)", CodePage = 20004 },
                new CustomEncodingInfo { DisplayName = "cp20003 x-cp20003 IBM5550 台湾", CodePage = 20003 },
                new CustomEncodingInfo { DisplayName = "cp20002 x_Chinese-Eten Eten Taiwan", CodePage = 20002 },
                new CustomEncodingInfo { DisplayName = "cp20001 x-cp20001 TCA 台湾", CodePage = 20001 },
                new CustomEncodingInfo { DisplayName = "cp20000 x-Chinese_CNS CNS 台湾", CodePage = 20000 },
                new CustomEncodingInfo { DisplayName = "cp12001 utf-32 Unicode UTF-32、ビッグ エンディアン", CodePage = 12001 },
                new CustomEncodingInfo { DisplayName = "cp12000 utf-8-32 Unicode UTF-32、リトル エンディアン", CodePage = 12000 },
                new CustomEncodingInfo { DisplayName = "cp10082 x-mac-クロアチア語 クロアチア語 (Mac)", CodePage = 10082 },
                new CustomEncodingInfo { DisplayName = "cp10081 x-mac-トルコ語 トルコ語 (Mac)", CodePage = 10081 },
                new CustomEncodingInfo { DisplayName = "cp10079 x-mac-アイスランド語 アイスランド語 (Mac)", CodePage = 10079 },
                new CustomEncodingInfo { DisplayName = "cp10029 x-mac-ce MAC ラテン 2", CodePage = 10029 },
                new CustomEncodingInfo { DisplayName = "cp10021 x-mac-タイ語 タイ語 (Mac)", CodePage = 10021 },
                new CustomEncodingInfo { DisplayName = "cp10017 x-mac-ウクライナ語 ウクライナ語 (Mac)", CodePage = 10017 },
                new CustomEncodingInfo { DisplayName = "cp10010 x-mac-ルーマニア語 ルーマニア語 (Mac)", CodePage = 10010 },
                new CustomEncodingInfo { DisplayName = "cp10008 chinesesimp MAC 簡体字中国語 (GB 2312)", CodePage = 10008 },
                new CustomEncodingInfo { DisplayName = "cp10007 x-mac-キリル文字 キリル語 (Mac)", CodePage = 10007 },
                new CustomEncodingInfo { DisplayName = "cp10006 x-mac-ギリシャ語 ギリシャ語 (Mac)", CodePage = 10006 },
                new CustomEncodingInfo { DisplayName = "cp10005 x-mac-ヘブライ語 ヘブライ語 (Mac)", CodePage = 10005 },
                new CustomEncodingInfo { DisplayName = "cp10004 x-mac-アラビア語 アラビア語 (Mac)", CodePage = 10004 },
                new CustomEncodingInfo { DisplayName = "cp10003 x-mac-韓国語 韓国語 (Mac)", CodePage = 10003 },
                new CustomEncodingInfo { DisplayName = "cp10002 chinesetrad MAC 繁体字中国語 (Big5)", CodePage = 10002 },
                new CustomEncodingInfo { DisplayName = "cp10001 x-mac-日本語 日本語 (Mac)", CodePage = 10001 },
                new CustomEncodingInfo { DisplayName = "cp10000 macintosh MAC Roman", CodePage = 10000 },
                new CustomEncodingInfo { DisplayName = "cp1361 Johab 韓国語 (Johab)", CodePage = 1361 },
                new CustomEncodingInfo { DisplayName = "cp1258 windows-1258 ANSI/OEM ベトナム語", CodePage = 1258 },
                new CustomEncodingInfo { DisplayName = "cp1257 windows-1257 ANSI バルト語", CodePage = 1257 },
                new CustomEncodingInfo { DisplayName = "cp1256 windows-1256 ANSI アラビア語", CodePage = 1256 },
                new CustomEncodingInfo { DisplayName = "cp1255 windows-1255 ANSI ヘブライ語", CodePage = 1255 },
                new CustomEncodingInfo { DisplayName = "cp1254 windows-1254 ANSI トルコ語", CodePage = 1254 },
                new CustomEncodingInfo { DisplayName = "cp1253 windows-1253 ANSI ギリシャ語", CodePage = 1253 },
                new CustomEncodingInfo { DisplayName = "cp1252 windows-1252 ANSI ラテン 1", CodePage = 1252 },
                new CustomEncodingInfo { DisplayName = "cp1251 windows-1251 ANSI キリル文字", CodePage = 1251 },
                new CustomEncodingInfo { DisplayName = "cp1250 windows-1250 ANSI 中央ヨーロッパ", CodePage = 1250 },
                new CustomEncodingInfo { DisplayName = "cp1201 unicodeFFFE Unicode UTF-16、ビッグ エンディアン", CodePage = 1201 },
                new CustomEncodingInfo { DisplayName = "cp1200 utf-16 Unicode UTF-16、リトル エンディアン", CodePage = 1200 },
                new CustomEncodingInfo { DisplayName = "cp1149 IBM01149 IBM EBCDIC アイスランド語 (ユーロ)", CodePage = 1149 },
                new CustomEncodingInfo { DisplayName = "cp1148 IBM01148 IBM EBCDIC International (ユーロ)", CodePage = 1148 },
                new CustomEncodingInfo { DisplayName = "cp1147 IBM01147 IBM EBCDIC フランス (ユーロ)", CodePage = 1147 },
                new CustomEncodingInfo { DisplayName = "cp1146 IBM01146 IBM EBCDIC 英国 (ユーロ)", CodePage = 1146 },
                new CustomEncodingInfo { DisplayName = "cp1145 IBM01145 IBM EBCDIC Latin America-Spain (ユーロ)", CodePage = 1145 },
                new CustomEncodingInfo { DisplayName = "cp1144 IBM01144 IBM EBCDIC イタリア (ユーロ)", CodePage = 1144 },
                new CustomEncodingInfo { DisplayName = "cp1143 IBM01143 IBM EBCDIC Finland-Sweden (ユーロ)", CodePage = 1143 },
                new CustomEncodingInfo { DisplayName = "cp1142 IBM01142 IBM EBCDIC Denmark-Norway (ユーロ)", CodePage = 1142 },
                new CustomEncodingInfo { DisplayName = "cp1141 IBM01141 IBM EBCDIC Germany (ユーロ)", CodePage = 1141 },
                new CustomEncodingInfo { DisplayName = "cp1140 IBM01140 IBM EBCDIC US-Canada (ユーロ)", CodePage = 1140 },
                new CustomEncodingInfo { DisplayName = "cp1047 IBM01047 IBM EBCDIC Latin 1/Open システム", CodePage = 1047 },
                new CustomEncodingInfo { DisplayName = "cp1026 IBM1026 IBM EBCDIC トルコ語 (ラテン 5)", CodePage = 1026 },
                new CustomEncodingInfo { DisplayName = "cp950 big5 ANSI/OEM 繁体字中国語 (Big5)", CodePage = 950 },
                new CustomEncodingInfo { DisplayName = "cp949 ks_c_5601-1987 ANSI/OEM 韓国語", CodePage = 949 },
                new CustomEncodingInfo { DisplayName = "cp936 gb2312 ANSI/OEM 簡体字中国語 (GB2312)", CodePage = 936 },
                new CustomEncodingInfo { DisplayName = "cp932 shift_jis ANSI/OEM 日本語 (Shift-JIS)", CodePage = 932 },
                new CustomEncodingInfo { DisplayName = "cp875 cp875 IBM EBCDIC ギリシャ語モダン", CodePage = 875 },
                new CustomEncodingInfo { DisplayName = "cp874 windows-874 タイ語 (Windows)", CodePage = 874 },
                new CustomEncodingInfo { DisplayName = "cp870 IBM870 IBM EBCDIC 多言語/ROECE", CodePage = 870 },
                new CustomEncodingInfo { DisplayName = "cp869 ibm869 OEM モダン ギリシャ語", CodePage = 869 },
                new CustomEncodingInfo { DisplayName = "cp866 cp866 OEM ロシア語", CodePage = 866 },
                new CustomEncodingInfo { DisplayName = "cp865 IBM865 OEM ノルディック", CodePage = 865 },
                new CustomEncodingInfo { DisplayName = "cp864 IBM864 OEM アラビア語", CodePage = 864 },
                new CustomEncodingInfo { DisplayName = "cp863 IBM863 OEM フランス語 (カナダ)", CodePage = 863 },
                new CustomEncodingInfo { DisplayName = "cp862 DOS-862 OEM ヘブライ語", CodePage = 862 },
                new CustomEncodingInfo { DisplayName = "cp861 ibm861 OEM アイスランド語", CodePage = 861 },
                new CustomEncodingInfo { DisplayName = "cp860 IBM860 OEM ポルトガル語", CodePage = 860 },
                new CustomEncodingInfo { DisplayName = "cp858 IBM00858 OEM 多言語ラテン 1 + ユーロ記号", CodePage = 858 },
                new CustomEncodingInfo { DisplayName = "cp857 ibm857 OEM トルコ語", CodePage = 857 },
                new CustomEncodingInfo { DisplayName = "cp855 IBM855 OEM キリル語 (主にロシア語)", CodePage = 855 },
                new CustomEncodingInfo { DisplayName = "cp852 ibm852 OEM ラテン 2", CodePage = 852 },
                new CustomEncodingInfo { DisplayName = "cp850 ibm850 OEM 多言語ラテン 1", CodePage = 850 },
                new CustomEncodingInfo { DisplayName = "cp775 ibm775 OEM バルト語", CodePage = 775 },
                new CustomEncodingInfo { DisplayName = "cp737 ibm737 OEM ギリシャ語 (旧称 437G)", CodePage = 737 },
                new CustomEncodingInfo { DisplayName = "cp708 ASMO-708 アラビア語 (ASMO 708)", CodePage = 708 },
                new CustomEncodingInfo { DisplayName = "cp500 IBM500 IBM EBCDIC International", CodePage = 500 },
                new CustomEncodingInfo { DisplayName = "cp437 IBM437 OEM 米国", CodePage = 437 },
                new CustomEncodingInfo { DisplayName = "cp37 IBM037 IBM EBCDIC US-Canada", CodePage = 37 },
                new CustomEncodingInfo { DisplayName = "cp0 OS default", CodePage = 0 }
            };

            // コンボボックスにエンコーディングを追加
            comboBox1.DataSource = new List<CustomEncodingInfo>(encodings);
            comboBox2.DataSource = new List<CustomEncodingInfo>(encodings);

            comboBox1.DisplayMember = "DisplayName";
            comboBox2.DisplayMember = "DisplayName";

            // Select the item based on the stored CodePage
            // You'll need to find the index of the CustomEncodingInfo object that matches the stored CodePage
            // For comboBox1
            int inputSelectedCodePage = neta.Properties.Settings.Default.encoder_in;
            CustomEncodingInfo selectedInput = encodings.Find(e => e.CodePage == inputSelectedCodePage);
            if (selectedInput != null)
            {
                comboBox1.SelectedItem = selectedInput;
            }
            else
            {
                comboBox1.SelectedIndex = 0; // Default to the first item if not found
            }

            // For comboBox2
            int outputSelectedCodePage = neta.Properties.Settings.Default.encoder_out;
            CustomEncodingInfo selectedOutput = encodings.Find(e => e.CodePage == outputSelectedCodePage);
            if (selectedOutput != null)
            {
                comboBox2.SelectedItem = selectedOutput;
            }
            else
            {
                comboBox2.SelectedIndex = 0; // Default to the first item if not found
            }

            // Event handlers for comboBox selection changes
            comboBox1.SelectedIndexChanged += new EventHandler(InputEncoding_SelectedIndexChanged);
            comboBox2.SelectedIndexChanged += new EventHandler(OutputEncoding_SelectedIndexChanged);
        }


        private void InputEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択されたエンコーディングを保存
            var selectedEncoding = (CustomEncodingInfo)comboBox1.SelectedItem;
            neta.Properties.Settings.Default.encoder_in = selectedEncoding.CodePage;
            neta.Properties.Settings.Default.Save(); // Save changes to settings
        }

        private void OutputEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択されたエンコーディングを保存
            var selectedEncoding = (CustomEncodingInfo)comboBox2.SelectedItem;
            neta.Properties.Settings.Default.encoder_out = selectedEncoding.CodePage;
            neta.Properties.Settings.Default.Save(); // Save changes to settings
        }



        private void Form8_Load(object sender, System.EventArgs e)
        {
            comboBox1.Text = Encoding.GetEncoding(neta.Properties.Settings.Default.encoder_in).BodyName;
            comboBox2.Text = Encoding.GetEncoding(neta.Properties.Settings.Default.encoder_out).BodyName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = wrong_encoder_codepage(
                textBox1.Text,
                Encoding.GetEncoding(neta.Properties.Settings.Default.encoder_in),
                Encoding.GetEncoding(neta.Properties.Settings.Default.encoder_out)
            );
        }

        private static string wrong_encoder_codepage(string input, Encoding incp, Encoding outcp)
        {
            byte[] st_bytes = incp.GetBytes(input);
            string wrong_st = outcp.GetString(st_bytes);

            return wrong_st;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // textBox2 のテキストをクリップボードにコピー
            if (textBox2.Text != "")
            {
                Clipboard.SetText(textBox2.Text);
            }
        }
    }
}
