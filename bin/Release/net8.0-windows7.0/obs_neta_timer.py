#original src https://obsproject.com/forum/resources/date-time.906/
#py -m pip install python-dateutil

# 導入手順　https://photos.app.goo.gl/puPDpiXsFb41YjW77
#work on OBS  python312
#2024/12/22 YMDHm＋TZ文字の補助パース追加　イベント日時　OS依存/UTCマスターメニューのやつ/日本時間固定+9 を追加　ローケールをついか、落ちやすくなるので使わない　＃推奨　
#2024/07/21　RFC2822だかの形式パース、スパンがゼロのとき例外処理,spanの文字処理をdttime関数に
#2024/07/20　UTCMATERにM$の標準時リストUTC+??を追加  zoneinfoからの全取得コードを追加（デフォは57行コメントアウト）,開始終了時刻がどちらかが未確定のときエラーを減らした
#2024/06/08　ISO8601以外の通常の時刻を変換できるようにした　例:2024/06/08 03:28
#2024/05/21　zoneinfoのデータが2006年前でしかはいってないようなので除外（）
#2024/05/16 開始の変換でzone影響あり　tzdataからpythondateutil　変更
#2024/05/16 ISO8601でのイベントタイマーに改造

import obspython as obs
import datetime
import math
import time
from dateutil import tz
import re
import locale

# 対応書式,例
# 2024/12/22 17:55
# 2024-12-22 17:56:00
# 2024-12-22T17:56:00:00Z
# 2024-12-22T17:56:00:00+09:00
# Tue, 25 Dec 2023 13:45:30 +0900
# 25 Dec 2023 13:45 +0900
# 25 Dec 2023 13:45 JST
# 2024-12-22 17:56 JST
# 2024-12-22T17:56:00:00JST

#書式コード    説明    例 ゾーン影響あり
#%Y    西暦（4桁表記。0埋め）    2021
#%m    月（2桁表記。0埋め）    11
#%d    日（2桁表記。0埋め）    04
#%H    時（24時間制。2桁表記。0埋め）    17
#%M    分（2桁表記。0埋め）    37
#%S    秒（2桁表記。0埋め）    28
#%y    西暦の下2桁（0埋め）    21
#%l    AM／PMを表す文字列    PM
#%x    日付をMM/DD/YY形式にしたもの    11/04/21
#%X    時刻をhh:mm:ss形式にしたもの    17:37:28
#%a    曜日の短縮形    Thu
#%A    曜日    Thursday
#%z    現在のタイムゾーンとUTC（協定世界時）とのオフセット    +0900
#%Z    現在のタイムゾーン    JST

##拡張部分 ゾーン影響なし
#OS %OS　　awareなんでタイムゾーンは欠損　time_formatで出力
#JST %JST　　日本時間time_formatで出力　常にGMT＋９
#UTC %UTC　　UTC MASTER  time_formatで出力
#ZULU %ZULU　UTC協定時間 ISO8601
#ISO %ISO　　zone影響あり ISO8601
#
#イベント名:%E
#開始時刻:%SO　OS依存,OBS起動時のみ
#終了時刻:%EO　OS依存,OBS起動時のみ
#開始時刻:%SJ　日本時間固定
#終了時刻:%EJ　日本時間固定
#開始時刻:%SU　UTC MASTER影響あり
#終了時刻:%EU　UTC MASTER影響あり
#開始時刻:%ST　zone影響あり
#終了時刻:%EN　zone影響あり
#イベ期間:%SP
#経過時間:%EL
#残り時間:%LF
#進捗状況:%Q %P%%

# localeモジュールで時間のロケールを'ja_JP.UTF-8'に変更する　　月曜日とかの曜日
#unix linux用
#locale.setlocale(locale.LC_TIME, 'ja_JP.UTF-8')
#windowsx用 ねぜかおちるのでおすすめはしない（）
#locale.setlocale(locale.LC_ALL, 'Japanese_Japan.932')


interval    = 10  #更新間隔0.1秒
source_name = ""
time_string = "%Y/%m/%d %H:%M:%S %z"
time_format = "%Y/%m/%d %H:%M:%S %Z %a"
iso_format = "%Y-%m-%dT%H:%M:%S%z"
zone        ="Asia/Tokyo"

# 全てのタイムゾーンを取得を使いたい場合下のコード
#おそらくzoneinfoのインストールが必要　　python -m pip install tzdata
#from zoneinfo import available_timezones
#zones = sorted(available_timezones())
zones       = ["Asia/Tokyo","Asia/Seoul","Asia/Taipei","Asia/Hong_Kong","America/Los_Angeles"]

#https://learn.microsoft.com/ja-jp/windows-hardware/manufacture/desktop/default-time-zones?view=windows-11
mstz = ["UTC-11:00","UTC-10:00","UTC-08:00","UTC-07:00","UTC-06:00","UTC-05:00","UTC-04:30","UTC-04:00","UTC-03:00","UTC-02:00","UTC-01:00","UTC+00:00","UTC+01:00","UTC+02:00","UTC+03:00","UTC+03:30","UTC+04:00","UTC+04:30","UTC+05:00","UTC+05:30","UTC+05:45","UTC+06:00","UTC+06:30","UTC+07:00","UTC+08:00","UTC+09:00","UTC+10:00","UTC+11:00","UTC+12:00","UTC+13:00"]



ibe='invaid PST-PDT ambious'
st = '2024-03-09T17:00:00Z'
en = '2024-11-02T16:00:00Z'
obsbar =3
utc =9
JST=""
UTC=""
utc_string = ""


# Regular expression patterns for the various formats
patterns = [
    r"^\d{4}/\d{2}/\d{2} \d{2}:\d{2}$",           # YYYY/MM/DD HH:MM
    r"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}$",           # YYYY-MM-DD HH:MM
    r"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$",      # YYYY-MM-DDTHH:MM:SS
    r"^\d{4}/\d{2}/\d{2}$",                        # YYYY/MM/DD
    r"^\d{4}-\d{2}-\d{2}$",                         # YYYY-MM-DD
    r"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:Z|[\+\-]\d{2}:\d{2})?$",
    r"^(?:(Mon|Tue|Wed|Thu|Fri|Sat|Sun), )?(\d{2}) (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) (\d{4}) (\d{2}):(\d{2})(?::(\d{2}))? ([-+]\d{4}|[A-Za-z]{2,4})$", ## RFC 2822形式
    r"^\d{4}[\-/]\d{2}[\-/]\d{2}[ T]\d{2}:\d{2}(?::\d{2}(?:\.\d+)?)? ?([A-Za-z]{2,4})$"  #YMDHmTZst
]


def parse_ymshmtzst(datetime_str):
    try:
        # 正規表現で入力を解析
        match = re.match(r"^(\d{4}[\-/]\d{2}[\-/]\d{2}[ T]\d{2}:\d{2}(?::\d{2}(?:\.\d+)?)?) ?([A-Za-z]{2,4})?$", datetime_str)
        if match:
            date_part = match.group(1)
            timezone = match.group(2) if match.group(2) else "UTC"  # タイムゾーンがない場合はデフォルトを適用

            # タイムゾーンを解析
            offset = timezone_parse(timezone)
            if offset is None:
                print(f"Unknown timezone: {timezone}")
                return None

            # フォーマットを正規化
            date_tmp = re.sub(r'[\/]', '-', date_part)  # スラッシュをハイフンに置換
            date_tmp = date_tmp.replace(" ", "T")      # スペースを "T" に置換

            # 秒がない場合に追加
            if re.search(r'T\d{2}:\d{2}$', date_tmp):
                date_tmp += ":00"

            # タイムゾーンオフセットをフォーマット
            offset = format_offset(offset)

            # ISO 8601 フォーマットに結合
            iso_date = f"{date_tmp}{offset}"

            # 日付を検証
            if validate_parsed_date(iso_date) is not None:
                return iso_date

    except Exception as e:
        print(f"Error parsing date: {e}")
        return None

    return None

def format_offset(offset):
    """Convert offset from ±HHMM to ±HH:MM format"""
    if len(offset) == 5:  # format like +0900
        return f"{offset[:3]}:{offset[3:]}"
    return offset

# Additional validation test - Parse and convert back
def validate_parsed_date(iso_date):
    try:
        dt = datetime.datetime.fromisoformat(iso_date)
        print(f"Successfully parsed: {dt}")
        return iso_date
    except ValueError as e:
        print(f"Invalid ISO format: {e}")
        return None

def timezone_parse(tz):
    global utc_string
    # Timezone mapping with abbreviation to offset
    timezone = [
         ("ACDT","+1030"),
        ("ACST","+0930"),
        ("AEDT","+1100"),
        ("AEST","+1000"),
        ("AFT","+0430"),
        ("AKDT","-0800"),
        ("AKST","-0900"),
        ("ART","-0300"),
        ("AWDT","+0900"),
        ("AWST","+0800"),
        ("BDT","+0600"),
        ("BNT","+0800"),
        ("BOT","-0400"),
        ("BRT","-0300"),
        ("BST","+0100"),
        ("BTT","+0600"),
        ("CAT","+0200"),
        ("CCT","+0630"),
        ("cDT","-0400"),
        ("CDT","-0500"),
        ("CEST","+0200"),
        ("CET","+0100"),
        ("CLST","-0300"),
        ("CLT","-0400"),
        ("COT","-0500"),
        ("cst","+0800"),
        ("cST","-0500"),
        ("CST","-0600"),
        ("ChST","+1000"),
        ("EAT","+0300"),
        ("ECT","-0500"),
        ("EDT","-0400"),
        ("EEST","+0300"),
        ("EET","+0200"),
        ("EST","-0500"),
        ("FJST","+1300"),
        ("FJT","+1200"),
        ("GMT","+0000"),
        ("GST","+0400"),
        ("HKT","+0800"),
        ("HST","-1000"),
        ("ICT","+0700"),
        ("IDT","+0300"),
        ("iST","+0200"),
        ("IST","+0530"),
        ("IRDT","+0430"),
        ("IRST","+0330"),
        ("JST","+0900"),
        ("KST","+0900"),
        ("MDT","-0600"),
        ("MMT","+0630"),
        ("MST","-0700"),
        ("MYT","+0800"),
        ("NPT","+0545"),
        ("NZDT","+1300"),
        ("NZST","+1200"),
        ("PDT","-0700"),
        ("PET","-0500"),
        ("PHT","+0800"),
        ("PKT","+0500"),
        ("PST","-0800"),
        ("PWT","+0900"),
        ("SST","-1100"),
        ("UT","+0000"),
        ("UTC","+0000"),
        ("UYT","-0300"),
        ("WAT","+0100"),
        ("WEST","+0100"),
        ("WET","+0000"),
        ("WIB","+0700"),
        ("WIT","+0900"),
        ("WITA","+0800"),
        ("Zulu","+0000")
    ]

    # Special case for UU timezone
    if tz == "UU":
        return utc_string.replace("UTC","")

    # Search for matching timezone
    for abbr, offset in timezone:
        if tz == abbr:
            return offset

    # Return None if no match found (equivalent to Lua's nil)
    return None


# 月の略称を対応する数値にマッピング
month_mapping = {
    "Jan": 1, "Feb": 2, "Mar": 3, "Apr": 4, "May": 5, "Jun": 6,
    "Jul": 7, "Aug": 8, "Sep": 9, "Oct": 10, "Nov": 11, "Dec": 12
}

# 使用例
#rfc2822_date = "Tue, 25 Dec 2023 13:45:30 +0900"
#parsed_date = parse_rfc2822(rfc2822_date)
#print(parsed_date)

# RFC 2822の日付文字列をパースする関数
def parse_rfc2822(date_str):
# RFC 2822形式の日付をパースする正規表現
    pattern = re.compile(r'^(?:(Mon|Tue|Wed|Thu|Fri|Sat|Sun), )?(\d{2}) (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) (\d{4}) (\d{2}):(\d{2})(?::(\d{2}))? ([-+]\d{4}|[A-Z]{1,3})$')
    match = pattern.match(date_str)
    if not match:
        raise ValueError("Invalid RFC 2822 date format")

    day, date, month, year, hour, minute, second, offset = match.groups()
    date = int(date)
    month = month_mapping[month]
    year = int(year)
    hour = int(hour)
    minute = int(minute)
    second = int(second) if second else 0

    tz_offset=0
    if  offset.startswith(('+', '-')):
        tz_hours = int(offset[1:3])
        tz_minutes = int(offset[3:5])
        tz_offset = tz_hours+tz_minutes/60
        if offset.startswith('-'):
            tz_offset = -tz_offset

    t_delta = datetime.timedelta(hours=tz_offset)
    rfc = datetime.timezone(t_delta, 'RFC')
    # datetimeオブジェクトを作成
    dt =  datetime.datetime(year, month, date, hour, minute, second, tzinfo=rfc)

    return dt.isoformat()

def dtime(dt):
    if dt<0:
            return "0日0時間0分"
    dt=abs(dt)
    seconds  = math.floor((dt / 10) % 60)
    minutes  = math.floor((dt / 60) % 60)
    hours    = math.floor((dt / 3600) % 24)
    days     = math.floor(dt / 86400)
    tmp = str(days) +"日" +str(hours)+"時間"+str(minutes) +"分"
    return tmp


def makebar(p):
    global obsbar

    base ="="
    q=obsbar

    p=p/q

    p=math.floor(p)
    s=""

    for i in range(p):
        s= s + base

    s=s+">"

    q=math.floor(100/q)
    for i in range(p+1,q, 1):
        s= s +"_"

    bar = "["+s+"]"
    return bar

def get_tz_time(dt,time_format,zone):
    stt = dt.astimezone(zone)
    ts = stt.strftime(time_format)
    return ts

def update_text():
    global interval
    global source_name
    global time_string
    global time_format
    global zone
    global ibe
    global st
    global en
    global iso_format
    global UTC
    global JST

    # 変換前後のタイムゾーンを指定
    cv_tz = tz.gettz(zone)
    temp=time_string
    nn=time.time()
    if(st !="----"):
        stt  = datetime.datetime.fromisoformat(st)
        #stt = stt.astimezone(cv_tz)
        #ts = stt.strftime(time_format)
        sttmp=stt.astimezone(cv_tz).timestamp()
        stt=datetime.datetime.fromtimestamp(sttmp)
        elapsed=dtime(nn-sttmp)
        temp=temp.replace('%ST',get_tz_time(stt,time_format,cv_tz))
        temp=temp.replace('%SJ',get_tz_time(stt,time_format,JST))
        temp=temp.replace('%SU',get_tz_time(stt,time_format,UTC))
        temp=temp.replace('%SO',get_tz_time(stt,time_format,None))
        temp=temp.replace('%EL',elapsed)
    else:
        temp=temp.replace('%EL',"----")
        temp=temp.replace('%ST',"----")
        temp=temp.replace('%SJ',"----")
        temp=temp.replace('%SU',"----")
        temp=temp.replace('%SO',"----")

    if(en !="----"):
        ent  = datetime.datetime.fromisoformat(en)
        #ent = ent.astimezone(cv_tz)
        #te = ent.strftime(time_format)
        entmp=ent.astimezone(cv_tz).timestamp()
        ent=datetime.datetime.fromtimestamp(entmp)
        left= dtime(entmp-nn)
        temp=temp.replace('%EN',get_tz_time(ent,time_format,cv_tz))
        temp=temp.replace('%EJ',get_tz_time(ent,time_format,JST))
        temp=temp.replace('%EU',get_tz_time(ent,time_format,UTC))
        temp=temp.replace('%EO',get_tz_time(ent,time_format,None))
        temp=temp.replace('%LF',left)
        if(st !="----"):
            if(entmp-sttmp !=0):
                x = (nn-sttmp)/abs(entmp-sttmp)*100
                n = 2
                y = math.floor(x * 10 ** n) / (10 ** n)
                if y>100:
                    y=100
                if y<0:
                    y=0
                bar=makebar(y)
                span=dtime(entmp-sttmp)
                #span= abs(ent-stt)
                temp=temp.replace('%SP',str(span))
                temp=temp.replace('%Q',bar)
                temp=temp.replace('%P',str(y))
    else:
        temp=temp.replace('%EN',"----")
        temp=temp.replace('%LF',"----")
        temp=temp.replace('%EJ',"----")
        temp=temp.replace('%EU',"----")
        temp=temp.replace('%EO',"----")


    temp=temp.replace('%E',ibe)
    temp=temp.replace('%OS',datetime.datetime.now(tz=None).strftime(time_format))
    temp=temp.replace('%JST',datetime.datetime.now(JST).strftime(time_format))
    temp=temp.replace('%UTC',datetime.datetime.now(UTC).strftime(time_format))
    temp=temp.replace('%ZULU',datetime.datetime.now(datetime.timezone.utc).strftime(iso_format))
    temp=temp.replace('%ISO',datetime.datetime.now().astimezone(cv_tz).strftime(iso_format))
    temp=re.sub('%(P|Q|SP)', '----', temp)

    source = obs.obs_get_source_by_name(source_name)
    if source is not None:
        settings = obs.obs_data_create()
        now = datetime.datetime.now()
        now=now.astimezone(cv_tz)
        obs.obs_data_set_string(settings, "text", now.strftime(temp))
        obs.obs_source_update(source, settings)
        obs.obs_data_release(settings)
        obs.obs_source_release(source)

def refresh_pressed(props, prop):
    update_text()

# ------------------------------------------------------------

def script_description():
    return "Updates a text source to the current date and time"

def script_defaults(settings):
    obs.obs_data_set_default_int(settings, "interval", interval)
    obs.obs_data_set_default_string(settings, "utc","UTC")
    obs.obs_data_set_default_string(settings, "format", time_string)
    obs.obs_data_set_default_string(settings, "time_format", time_format)
    obs.obs_data_set_default_string(settings, "zone", zone )
    obs.obs_data_set_default_string(settings, "eve", ibe)
    obs.obs_data_set_default_string(settings, "start", st)
    obs.obs_data_set_default_string(settings, "end", en)
    obs.obs_data_set_default_int(settings, "bar", obsbar)

def script_properties():
    props = obs.obs_properties_create()

    obs.obs_properties_add_int(props, "interval", "Update Interval (seconds)", 1, 3600, 1)


    # Add sources select dropdown
    p = obs.obs_properties_add_list(props, "source", "Text Source", obs.OBS_COMBO_TYPE_EDITABLE, obs.OBS_COMBO_FORMAT_STRING)

    # Make a list of all the text sources
    obs.obs_property_list_add_string(p, "[No text source]", "[No text source]")

    sources = obs.obs_enum_sources()

    if sources is not None:
        for source in sources:
            name = obs.obs_source_get_name(source)
            source_id = obs.obs_source_get_unversioned_id(source)
            if source_id == "text_gdiplus" or source_id == "text_ft2_source":
                name = obs.obs_source_get_name(source)
                obs.obs_property_list_add_string(p, name, name)
        obs.source_list_release(sources)

    mstime_zone_list = obs.obs_properties_add_list(
        props, "utc", "UTC MASTER", obs.OBS_COMBO_TYPE_LIST, obs.OBS_COMBO_FORMAT_STRING
    )

    for mszone in mstz:
        obs.obs_property_list_add_string(mstime_zone_list, mszone, mszone)


    time_zone_list = obs.obs_properties_add_list(
        props, "zone", "Time zone", obs.OBS_COMBO_TYPE_LIST, obs.OBS_COMBO_FORMAT_STRING
    )

    for timezone in zones:
        obs.obs_property_list_add_string(time_zone_list, timezone, timezone)

    obs.obs_properties_add_text(props, "format", "time_string", obs.OBS_TEXT_MULTILINE)
    obs.obs_properties_add_text(props, "time_format", "time_format", obs.OBS_TEXT_DEFAULT)
    obs.obs_properties_add_text(props, "eve", "EVENT", obs.OBS_TEXT_DEFAULT)
    obs.obs_properties_add_text(props, "start", "START", obs.OBS_TEXT_DEFAULT)
    obs.obs_properties_add_text(props, "end", "END", obs.OBS_TEXT_DEFAULT)
    obs.obs_properties_add_int(props, "bar", "BAR LENGTH(100÷X)", 1, 10, 1)

    obs.obs_properties_add_button(props, "button", "Refresh", refresh_pressed)
    return props

def normalize_time(input_time_str):
    iso8601_time=parse_datetime(input_time_str)
    if (iso8601_time is not None): # Check if iso8601_time not time
        iso8601_time =iso8601_time.isoformat()
        return iso8601_time
    else:
        return "----" # Return an error message if parsing fails

def parse_datetime(datetime_str):
    datetime_str=re.sub(" +"," ",datetime_str.strip())
    for pattern in patterns:
        if re.match(pattern, datetime_str):
            try:
                if pattern == patterns[0]:
                    return datetime.datetime.strptime(datetime_str, "%Y/%m/%d %H:%M")
                elif pattern == patterns[1]:
                    return datetime.datetime.strptime(datetime_str, "%Y-%m-%d %H:%M")
                elif pattern == patterns[2]:
                    return datetime.datetime.strptime(datetime_str, "%Y-%m-%dT%H:%M:%S")
                elif pattern == patterns[3]:
                    return datetime.datetime.strptime(datetime_str, "%Y/%m/%d")
                elif pattern == patterns[4]:
                    return datetime.datetime.strptime(datetime_str, "%Y-%m-%d")
                elif pattern == patterns[5]:
                    datetime_str=datetime_str.replace('Z', '+00:00')
                    return datetime.datetime.fromisoformat(datetime_str) # Call fromisoformat directly on datetime
                elif pattern == patterns[6]:
                    dt = parse_rfc2822(datetime_str)
                    return datetime.datetime.fromisoformat(dt)
                elif pattern == patterns[7]:
                    dt = parse_ymshmtzst(datetime_str)
                    if (dt is not None):
                        return datetime.datetime.fromisoformat(dt)
            except ValueError:
                return None
    return None

def script_update(settings):
    global interval
    global source_name
    global utc_string
    global time_string
    global time_format
    global zone
    global st
    global en
    global obsbar
    global utc
    global UTC
    global JST
    global ibe
    ibe    =obs.obs_data_get_string(settings, "eve")
    utc_string    = obs.obs_data_get_string(settings, "utc")
    interval    = obs.obs_data_get_int(settings, "interval")
    source_name = obs.obs_data_get_string(settings, "source")
    time_string = obs.obs_data_get_string(settings, "format")
    time_format = obs.obs_data_get_string(settings, "time_format")
    zone = obs.obs_data_get_string(settings, "zone")
    st = obs.obs_data_get_string(settings, "start")
    en = obs.obs_data_get_string(settings, "end")
    st = normalize_time(str(st))
    en = normalize_time(str(en))
    obsbar = obs.obs_data_get_int(settings, "bar")
    # 正規表現パターン
    pattern = r"UTC(.)(\d{2}):(\d{2})"
    # 正規表現検索
    match = re.search(pattern, utc_string)
    utc=0
    utc_mstring=""
    if match:
        sign=match.group(1)
        hh = match.group(2)
        mm = match.group(3)
        utc=int(hh)+int(mm)/60
        if sign=="-":
            utc=-utc

    t_delta = datetime.timedelta(hours=9)  # 9時間
    JST = datetime.timezone(t_delta, 'JST')
    t_delta = datetime.timedelta(hours=utc)
    UTC = datetime.timezone(t_delta, utc_string)

    obs.timer_remove(update_text)

    if source_name != "":
        obs.timer_add(update_text, interval * 100)
