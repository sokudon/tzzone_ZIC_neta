namespace neta
{
    partial class dtformat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            time_span = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            invaid_ambigous = new System.Windows.Forms.CheckBox();
            custom_local = new System.Windows.Forms.CheckBox();
            noda_timezone_items = new System.Windows.Forms.ComboBox();
            noda_timezone = new System.Windows.Forms.CheckBox();
            TIMEZONE_MODE = new System.Windows.Forms.Label();
            panel3 = new System.Windows.Forms.Panel();
            tzbinary_path = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            tzbinary_dir_select = new System.Windows.Forms.Button();
            tzbinary_timezone = new System.Windows.Forms.CheckBox();
            tzbinary_tzst = new System.Windows.Forms.ComboBox();
            ms_utcoffset = new System.Windows.Forms.CheckBox();
            ms_utcoffset_items = new System.Windows.Forms.ComboBox();
            ms_timezone = new System.Windows.Forms.CheckBox();
            ms_timezone_items = new System.Windows.Forms.ComboBox();
            panel5 = new System.Windows.Forms.Panel();
            locale = new System.Windows.Forms.Label();
            localeBox = new System.Windows.Forms.ComboBox();
            elapst_left = new System.Windows.Forms.ComboBox();
            panel6 = new System.Windows.Forms.Panel();
            linkLabel2 = new System.Windows.Forms.LinkLabel();
            noda_dateformat = new System.Windows.Forms.ComboBox();
            normal_dateformat = new System.Windows.Forms.ComboBox();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            TIME_FORMAT = new System.Windows.Forms.Label();
            bar_length = new System.Windows.Forms.ComboBox();
            progressbar_length = new System.Windows.Forms.Label();
            panel2 = new System.Windows.Forms.Panel();
            button2 = new System.Windows.Forms.Button();
            key_value = new System.Windows.Forms.Label();
            change_baseurl = new System.Windows.Forms.CheckBox();
            baseurl_keyval = new System.Windows.Forms.TextBox();
            baseurl_txt = new System.Windows.Forms.TextBox();
            label13 = new System.Windows.Forms.Label();
            button3 = new System.Windows.Forms.Button();
            json_test = new System.Windows.Forms.Button();
            parse_target = new System.Windows.Forms.TextBox();
            custom_url_path = new System.Windows.Forms.ComboBox();
            custom_uri = new System.Windows.Forms.Label();
            target_path = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            textBox3 = new System.Windows.Forms.TextBox();
            y_start = new System.Windows.Forms.ComboBox();
            y_end = new System.Windows.Forms.ComboBox();
            use_year_filter = new System.Windows.Forms.CheckBox();
            label8 = new System.Windows.Forms.Label();
            parse_test = new System.Windows.Forms.TextBox();
            test_date = new System.Windows.Forms.Label();
            INFOMATION = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // time_span
            // 
            time_span.AutoSize = true;
            time_span.Location = new System.Drawing.Point(5, 33);
            time_span.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            time_span.Name = "time_span";
            time_span.Size = new System.Drawing.Size(73, 20);
            time_span.TabIndex = 0;
            time_span.Text = "経過/残り:";
            // 
            // panel1
            // 
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label3);
            panel1.Location = new System.Drawing.Point(17, 45);
            panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(400, 567);
            panel1.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(invaid_ambigous);
            panel4.Controls.Add(custom_local);
            panel4.Controls.Add(noda_timezone_items);
            panel4.Controls.Add(noda_timezone);
            panel4.Controls.Add(TIMEZONE_MODE);
            panel4.Controls.Add(panel3);
            panel4.Controls.Add(ms_utcoffset);
            panel4.Controls.Add(ms_utcoffset_items);
            panel4.Controls.Add(ms_timezone);
            panel4.Controls.Add(ms_timezone_items);
            panel4.Location = new System.Drawing.Point(20, 148);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(380, 268);
            panel4.TabIndex = 26;
            // 
            // invaid_ambigous
            // 
            invaid_ambigous.AutoSize = true;
            invaid_ambigous.Location = new System.Drawing.Point(172, 157);
            invaid_ambigous.Name = "invaid_ambigous";
            invaid_ambigous.Size = new System.Drawing.Size(61, 24);
            invaid_ambigous.TabIndex = 29;
            invaid_ambigous.Text = "厳格";
            invaid_ambigous.UseVisualStyleBackColor = true;
            invaid_ambigous.CheckedChanged += invaid_ambigous_CheckedChanged;
            // 
            // custom_local
            // 
            custom_local.AutoSize = true;
            custom_local.Location = new System.Drawing.Point(157, 0);
            custom_local.Name = "custom_local";
            custom_local.Size = new System.Drawing.Size(221, 24);
            custom_local.TabIndex = 27;
            custom_local.Text = "時差情報がない日付zone適用";
            custom_local.UseVisualStyleBackColor = true;
            custom_local.CheckedChanged += custom_local_CheckedChanged;
            // 
            // noda_timezone_items
            // 
            noda_timezone_items.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            noda_timezone_items.FormattingEnabled = true;
            noda_timezone_items.ImeMode = System.Windows.Forms.ImeMode.Disable;
            noda_timezone_items.Items.AddRange(new object[] { "Africa/Abidjan", "Africa/Accra", "Africa/Addis_Ababa", "Africa/Algiers", "Africa/Asmara", "Africa/Asmera", "Africa/Bamako", "Africa/Bangui", "Africa/Banjul", "Africa/Bissau", "Africa/Blantyre", "Africa/Brazzaville", "Africa/Bujumbura", "Africa/Cairo", "Africa/Casablanca", "Africa/Ceuta", "Africa/Conakry", "Africa/Dakar", "Africa/Dar_es_Salaam", "Africa/Djibouti", "Africa/Douala", "Africa/El_Aaiun", "Africa/Freetown", "Africa/Gaborone", "Africa/Harare", "Africa/Johannesburg", "Africa/Juba", "Africa/Kampala", "Africa/Khartoum", "Africa/Kigali", "Africa/Kinshasa", "Africa/Lagos", "Africa/Libreville", "Africa/Lome", "Africa/Luanda", "Africa/Lubumbashi", "Africa/Lusaka", "Africa/Malabo", "Africa/Maputo", "Africa/Maseru", "Africa/Mbabane", "Africa/Mogadishu", "Africa/Monrovia", "Africa/Nairobi", "Africa/Ndjamena", "Africa/Niamey", "Africa/Nouakchott", "Africa/Ouagadougou", "Africa/Porto-Novo", "Africa/Sao_Tome", "Africa/Timbuktu", "Africa/Tripoli", "Africa/Tunis", "Africa/Windhoek", "America/Adak", "America/Anchorage", "America/Anguilla", "America/Antigua", "America/Araguaina", "America/Argentina/Buenos_Aires", "America/Argentina/Catamarca", "America/Argentina/ComodRivadavia", "America/Argentina/Cordoba", "America/Argentina/Jujuy", "America/Argentina/La_Rioja", "America/Argentina/Mendoza", "America/Argentina/Rio_Gallegos", "America/Argentina/Salta", "America/Argentina/San_Juan", "America/Argentina/San_Luis", "America/Argentina/Tucuman", "America/Argentina/Ushuaia", "America/Aruba", "America/Asuncion", "America/Atikokan", "America/Atka", "America/Bahia", "America/Bahia_Banderas", "America/Barbados", "America/Belem", "America/Belize", "America/Blanc-Sablon", "America/Boa_Vista", "America/Bogota", "America/Boise", "America/Buenos_Aires", "America/Cambridge_Bay", "America/Campo_Grande", "America/Cancun", "America/Caracas", "America/Catamarca", "America/Cayenne", "America/Cayman", "America/Chicago", "America/Chihuahua", "America/Ciudad_Juarez", "America/Coral_Harbour", "America/Cordoba", "America/Costa_Rica", "America/Creston", "America/Cuiaba", "America/Curacao", "America/Danmarkshavn", "America/Dawson", "America/Dawson_Creek", "America/Denver", "America/Detroit", "America/Dominica", "America/Edmonton", "America/Eirunepe", "America/El_Salvador", "America/Ensenada", "America/Fort_Nelson", "America/Fort_Wayne", "America/Fortaleza", "America/Glace_Bay", "America/Godthab", "America/Goose_Bay", "America/Grand_Turk", "America/Grenada", "America/Guadeloupe", "America/Guatemala", "America/Guayaquil", "America/Guyana", "America/Halifax", "America/Havana", "America/Hermosillo", "America/Indiana/Indianapolis", "America/Indiana/Knox", "America/Indiana/Marengo", "America/Indiana/Petersburg", "America/Indiana/Tell_City", "America/Indiana/Vevay", "America/Indiana/Vincennes", "America/Indiana/Winamac", "America/Indianapolis", "America/Inuvik", "America/Iqaluit", "America/Jamaica", "America/Jujuy", "America/Juneau", "America/Kentucky/Louisville", "America/Kentucky/Monticello", "America/Knox_IN", "America/Kralendijk", "America/La_Paz", "America/Lima", "America/Los_Angeles", "America/Louisville", "America/Lower_Princes", "America/Maceio", "America/Managua", "America/Manaus", "America/Marigot", "America/Martinique", "America/Matamoros", "America/Mazatlan", "America/Mendoza", "America/Menominee", "America/Merida", "America/Metlakatla", "America/Mexico_City", "America/Miquelon", "America/Moncton", "America/Monterrey", "America/Montevideo", "America/Montreal", "America/Montserrat", "America/Nassau", "America/New_York", "America/Nipigon", "America/Nome", "America/Noronha", "America/North_Dakota/Beulah", "America/North_Dakota/Center", "America/North_Dakota/New_Salem", "America/Nuuk", "America/Ojinaga", "America/Panama", "America/Pangnirtung", "America/Paramaribo", "America/Phoenix", "America/Port-au-Prince", "America/Port_of_Spain", "America/Porto_Acre", "America/Porto_Velho", "America/Puerto_Rico", "America/Punta_Arenas", "America/Rainy_River", "America/Rankin_Inlet", "America/Recife", "America/Regina", "America/Resolute", "America/Rio_Branco", "America/Rosario", "America/Santa_Isabel", "America/Santarem", "America/Santiago", "America/Santo_Domingo", "America/Sao_Paulo", "America/Scoresbysund", "America/Shiprock", "America/Sitka", "America/St_Barthelemy", "America/St_Johns", "America/St_Kitts", "America/St_Lucia", "America/St_Thomas", "America/St_Vincent", "America/Swift_Current", "America/Tegucigalpa", "America/Thule", "America/Thunder_Bay", "America/Tijuana", "America/Toronto", "America/Tortola", "America/Vancouver", "America/Virgin", "America/Whitehorse", "America/Winnipeg", "America/Yakutat", "America/Yellowknife", "Antarctica/Casey", "Antarctica/Davis", "Antarctica/DumontDUrville", "Antarctica/Macquarie", "Antarctica/Mawson", "Antarctica/McMurdo", "Antarctica/Palmer", "Antarctica/Rothera", "Antarctica/South_Pole", "Antarctica/Syowa", "Antarctica/Troll", "Antarctica/Vostok", "Arctic/Longyearbyen", "Asia/Aden", "Asia/Almaty", "Asia/Amman", "Asia/Anadyr", "Asia/Aqtau", "Asia/Aqtobe", "Asia/Ashgabat", "Asia/Ashkhabad", "Asia/Atyrau", "Asia/Baghdad", "Asia/Bahrain", "Asia/Baku", "Asia/Bangkok", "Asia/Barnaul", "Asia/Beirut", "Asia/Bishkek", "Asia/Brunei", "Asia/Calcutta", "Asia/Chita", "Asia/Choibalsan", "Asia/Chongqing", "Asia/Chungking", "Asia/Colombo", "Asia/Dacca", "Asia/Damascus", "Asia/Dhaka", "Asia/Dili", "Asia/Dubai", "Asia/Dushanbe", "Asia/Famagusta", "Asia/Gaza", "Asia/Hanoi", "Asia/Harbin", "Asia/Hebron", "Asia/Ho_Chi_Minh", "Asia/Hong_Kong", "Asia/Hovd", "Asia/Irkutsk", "Asia/Istanbul", "Asia/Jakarta", "Asia/Jayapura", "Asia/Jerusalem", "Asia/Kabul", "Asia/Kamchatka", "Asia/Karachi", "Asia/Kashgar", "Asia/Kathmandu", "Asia/Katmandu", "Asia/Khandyga", "Asia/Kolkata", "Asia/Krasnoyarsk", "Asia/Kuala_Lumpur", "Asia/Kuching", "Asia/Kuwait", "Asia/Macao", "Asia/Macau", "Asia/Magadan", "Asia/Makassar", "Asia/Manila", "Asia/Muscat", "Asia/Nicosia", "Asia/Novokuznetsk", "Asia/Novosibirsk", "Asia/Omsk", "Asia/Oral", "Asia/Phnom_Penh", "Asia/Pontianak", "Asia/Pyongyang", "Asia/Qatar", "Asia/Qostanay", "Asia/Qyzylorda", "Asia/Rangoon", "Asia/Riyadh", "Asia/Saigon", "Asia/Sakhalin", "Asia/Samarkand", "Asia/Seoul", "Asia/Shanghai", "Asia/Singapore", "Asia/Srednekolymsk", "Asia/Taipei", "Asia/Tashkent", "Asia/Tbilisi", "Asia/Tehran", "Asia/Tel_Aviv", "Asia/Thimbu", "Asia/Thimphu", "Asia/Tokyo", "Asia/Tomsk", "Asia/Ujung_Pandang", "Asia/Ulaanbaatar", "Asia/Ulan_Bator", "Asia/Urumqi", "Asia/Ust-Nera", "Asia/Vientiane", "Asia/Vladivostok", "Asia/Yakutsk", "Asia/Yangon", "Asia/Yekaterinburg", "Asia/Yerevan", "Atlantic/Azores", "Atlantic/Bermuda", "Atlantic/Canary", "Atlantic/Cape_Verde", "Atlantic/Faeroe", "Atlantic/Faroe", "Atlantic/Jan_Mayen", "Atlantic/Madeira", "Atlantic/Reykjavik", "Atlantic/South_Georgia", "Atlantic/St_Helena", "Atlantic/Stanley", "Australia/ACT", "Australia/Adelaide", "Australia/Brisbane", "Australia/Broken_Hill", "Australia/Canberra", "Australia/Currie", "Australia/Darwin", "Australia/Eucla", "Australia/Hobart", "Australia/LHI", "Australia/Lindeman", "Australia/Lord_Howe", "Australia/Melbourne", "Australia/NSW", "Australia/North", "Australia/Perth", "Australia/Queensland", "Australia/South", "Australia/Sydney", "Australia/Tasmania", "Australia/Victoria", "Australia/West", "Australia/Yancowinna", "Brazil/Acre", "Brazil/DeNoronha", "Brazil/East", "Brazil/West", "CET", "CST6CDT", "Canada/Atlantic", "Canada/Central", "Canada/Eastern", "Canada/Mountain", "Canada/Newfoundland", "Canada/Pacific", "Canada/Saskatchewan", "Canada/Yukon", "Chile/Continental", "Chile/EasterIsland", "Cuba", "EET", "EST", "EST5EDT", "Egypt", "Eire", "Etc/GMT", "Etc/GMT+0", "Etc/GMT+1", "Etc/GMT+10", "Etc/GMT+11", "Etc/GMT+12", "Etc/GMT+2", "Etc/GMT+3", "Etc/GMT+4", "Etc/GMT+5", "Etc/GMT+6", "Etc/GMT+7", "Etc/GMT+8", "Etc/GMT+9", "Etc/GMT-0", "Etc/GMT-1", "Etc/GMT-10", "Etc/GMT-11", "Etc/GMT-12", "Etc/GMT-13", "Etc/GMT-14", "Etc/GMT-2", "Etc/GMT-3", "Etc/GMT-4", "Etc/GMT-5", "Etc/GMT-6", "Etc/GMT-7", "Etc/GMT-8", "Etc/GMT-9", "Etc/GMT0", "Etc/Greenwich", "Etc/UCT", "Etc/UTC", "Etc/Universal", "Etc/Zulu", "Europe/Amsterdam", "Europe/Andorra", "Europe/Astrakhan", "Europe/Athens", "Europe/Belfast", "Europe/Belgrade", "Europe/Berlin", "Europe/Bratislava", "Europe/Brussels", "Europe/Bucharest", "Europe/Budapest", "Europe/Busingen", "Europe/Chisinau", "Europe/Copenhagen", "Europe/Dublin", "Europe/Gibraltar", "Europe/Guernsey", "Europe/Helsinki", "Europe/Isle_of_Man", "Europe/Istanbul", "Europe/Jersey", "Europe/Kaliningrad", "Europe/Kiev", "Europe/Kirov", "Europe/Kyiv", "Europe/Lisbon", "Europe/Ljubljana", "Europe/London", "Europe/Luxembourg", "Europe/Madrid", "Europe/Malta", "Europe/Mariehamn", "Europe/Minsk", "Europe/Monaco", "Europe/Moscow", "Europe/Nicosia", "Europe/Oslo", "Europe/Paris", "Europe/Podgorica", "Europe/Prague", "Europe/Riga", "Europe/Rome", "Europe/Samara", "Europe/San_Marino", "Europe/Sarajevo", "Europe/Saratov", "Europe/Simferopol", "Europe/Skopje", "Europe/Sofia", "Europe/Stockholm", "Europe/Tallinn", "Europe/Tirane", "Europe/Tiraspol", "Europe/Ulyanovsk", "Europe/Uzhgorod", "Europe/Vaduz", "Europe/Vatican", "Europe/Vienna", "Europe/Vilnius", "Europe/Volgograd", "Europe/Warsaw", "Europe/Zagreb", "Europe/Zaporozhye", "Europe/Zurich", "Factory", "GB", "GB-Eire", "GMT", "GMT+0", "GMT-0", "GMT0", "Greenwich", "HST", "Hongkong", "Iceland", "Indian/Antananarivo", "Indian/Chagos", "Indian/Christmas", "Indian/Cocos", "Indian/Comoro", "Indian/Kerguelen", "Indian/Mahe", "Indian/Maldives", "Indian/Mauritius", "Indian/Mayotte", "Indian/Reunion", "Iran", "Israel", "Jamaica", "Japan", "Kwajalein", "Libya", "MET", "MST", "MST7MDT", "Mexico/BajaNorte", "Mexico/BajaSur", "Mexico/General", "NZ", "NZ-CHAT", "Navajo", "PRC", "PST8PDT", "Pacific/Apia", "Pacific/Auckland", "Pacific/Bougainville", "Pacific/Chatham", "Pacific/Chuuk", "Pacific/Easter", "Pacific/Efate", "Pacific/Enderbury", "Pacific/Fakaofo", "Pacific/Fiji", "Pacific/Funafuti", "Pacific/Galapagos", "Pacific/Gambier", "Pacific/Guadalcanal", "Pacific/Guam", "Pacific/Honolulu", "Pacific/Johnston", "Pacific/Kanton", "Pacific/Kiritimati", "Pacific/Kosrae", "Pacific/Kwajalein", "Pacific/Majuro", "Pacific/Marquesas", "Pacific/Midway", "Pacific/Nauru", "Pacific/Niue", "Pacific/Norfolk", "Pacific/Noumea", "Pacific/Pago_Pago", "Pacific/Palau", "Pacific/Pitcairn", "Pacific/Pohnpei", "Pacific/Ponape", "Pacific/Port_Moresby", "Pacific/Rarotonga", "Pacific/Saipan", "Pacific/Samoa", "Pacific/Tahiti", "Pacific/Tarawa", "Pacific/Tongatapu", "Pacific/Truk", "Pacific/Wake", "Pacific/Wallis", "Pacific/Yap", "Poland", "Portugal", "ROC", "ROK", "Singapore", "Turkey", "UCT", "US/Alaska", "US/Aleutian", "US/Arizona", "US/Central", "US/East-Indiana", "US/Eastern", "US/Hawaii", "US/Indiana-Starke", "US/Michigan", "US/Mountain", "US/Pacific", "US/Samoa", "UTC", "Universal", "W-SU", "WET", "Zulu" });
            noda_timezone_items.Location = new System.Drawing.Point(230, 155);
            noda_timezone_items.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            noda_timezone_items.Name = "noda_timezone_items";
            noda_timezone_items.Size = new System.Drawing.Size(139, 28);
            noda_timezone_items.TabIndex = 28;
            noda_timezone_items.SelectedIndexChanged += comboBox6_SelectedIndexChanged;
            // 
            // noda_timezone
            // 
            noda_timezone.AutoSize = true;
            noda_timezone.Location = new System.Drawing.Point(8, 157);
            noda_timezone.Name = "noda_timezone";
            noda_timezone.Size = new System.Drawing.Size(168, 24);
            noda_timezone.TabIndex = 27;
            noda_timezone.Text = "のだたいむ(夏修正あり)";
            noda_timezone.UseVisualStyleBackColor = true;
            noda_timezone.CheckedChanged += noda_timezone_CheckedChanged;
            // 
            // TIMEZONE_MODE
            // 
            TIMEZONE_MODE.AutoSize = true;
            TIMEZONE_MODE.Location = new System.Drawing.Point(4, 0);
            TIMEZONE_MODE.Name = "TIMEZONE_MODE";
            TIMEZONE_MODE.Size = new System.Drawing.Size(147, 20);
            TIMEZONE_MODE.TabIndex = 6;
            TIMEZONE_MODE.Text = "[タイムゾーン設定モード]";
            // 
            // panel3
            // 
            panel3.Controls.Add(tzbinary_path);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(tzbinary_dir_select);
            panel3.Controls.Add(tzbinary_timezone);
            panel3.Controls.Add(tzbinary_tzst);
            panel3.Location = new System.Drawing.Point(8, 192);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(372, 73);
            panel3.TabIndex = 26;
            // 
            // tzbinary_path
            // 
            tzbinary_path.FormattingEnabled = true;
            tzbinary_path.Location = new System.Drawing.Point(94, 3);
            tzbinary_path.Name = "tzbinary_path";
            tzbinary_path.Size = new System.Drawing.Size(215, 28);
            tzbinary_path.TabIndex = 26;
            tzbinary_path.SelectedIndexChanged += tzbinary_path_SelectedIndexChanged;
            tzbinary_path.TextChanged += tzbinary_path_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(-1, 6);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(89, 20);
            label5.TabIndex = 23;
            label5.Text = "tzdateのパス:";
            label5.Click += label5_Click;
            // 
            // tzbinary_dir_select
            // 
            tzbinary_dir_select.Location = new System.Drawing.Point(317, 2);
            tzbinary_dir_select.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tzbinary_dir_select.Name = "tzbinary_dir_select";
            tzbinary_dir_select.Size = new System.Drawing.Size(24, 29);
            tzbinary_dir_select.TabIndex = 24;
            tzbinary_dir_select.Text = "..";
            tzbinary_dir_select.UseVisualStyleBackColor = true;
            tzbinary_dir_select.Click += button2_Click_1;
            // 
            // tzbinary_timezone
            // 
            tzbinary_timezone.AutoSize = true;
            tzbinary_timezone.Location = new System.Drawing.Point(0, 37);
            tzbinary_timezone.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tzbinary_timezone.Name = "tzbinary_timezone";
            tzbinary_timezone.Size = new System.Drawing.Size(215, 24);
            tzbinary_timezone.TabIndex = 25;
            tzbinary_timezone.Text = "TZBINを使う(夏時間修正あり)";
            tzbinary_timezone.UseVisualStyleBackColor = true;
            tzbinary_timezone.CheckedChanged += tzbinary_timezone_CheckedChanged;
            // 
            // tzbinary_tzst
            // 
            tzbinary_tzst.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            tzbinary_tzst.FormattingEnabled = true;
            tzbinary_tzst.ImeMode = System.Windows.Forms.ImeMode.Disable;
            tzbinary_tzst.Items.AddRange(new object[] { "Africa/Abidjan", "Africa/Accra", "Africa/Addis_Ababa", "Africa/Algiers", "Africa/Asmara", "Africa/Asmera", "Africa/Bamako", "Africa/Bangui", "Africa/Banjul", "Africa/Bissau", "Africa/Blantyre", "Africa/Brazzaville", "Africa/Bujumbura", "Africa/Cairo", "Africa/Casablanca", "Africa/Ceuta", "Africa/Conakry", "Africa/Dakar", "Africa/Dar_es_Salaam", "Africa/Djibouti", "Africa/Douala", "Africa/El_Aaiun", "Africa/Freetown", "Africa/Gaborone", "Africa/Harare", "Africa/Johannesburg", "Africa/Juba", "Africa/Kampala", "Africa/Khartoum", "Africa/Kigali", "Africa/Kinshasa", "Africa/Lagos", "Africa/Libreville", "Africa/Lome", "Africa/Luanda", "Africa/Lubumbashi", "Africa/Lusaka", "Africa/Malabo", "Africa/Maputo", "Africa/Maseru", "Africa/Mbabane", "Africa/Mogadishu", "Africa/Monrovia", "Africa/Nairobi", "Africa/Ndjamena", "Africa/Niamey", "Africa/Nouakchott", "Africa/Ouagadougou", "Africa/Porto-Novo", "Africa/Sao_Tome", "Africa/Timbuktu", "Africa/Tripoli", "Africa/Tunis", "Africa/Windhoek", "America/Adak", "America/Anchorage", "America/Anguilla", "America/Antigua", "America/Araguaina", "America/Argentina/Buenos_Aires", "America/Argentina/Catamarca", "America/Argentina/ComodRivadavia", "America/Argentina/Cordoba", "America/Argentina/Jujuy", "America/Argentina/La_Rioja", "America/Argentina/Mendoza", "America/Argentina/Rio_Gallegos", "America/Argentina/Salta", "America/Argentina/San_Juan", "America/Argentina/San_Luis", "America/Argentina/Tucuman", "America/Argentina/Ushuaia", "America/Aruba", "America/Asuncion", "America/Atikokan", "America/Atka", "America/Bahia", "America/Bahia_Banderas", "America/Barbados", "America/Belem", "America/Belize", "America/Blanc-Sablon", "America/Boa_Vista", "America/Bogota", "America/Boise", "America/Buenos_Aires", "America/Cambridge_Bay", "America/Campo_Grande", "America/Cancun", "America/Caracas", "America/Catamarca", "America/Cayenne", "America/Cayman", "America/Chicago", "America/Chihuahua", "America/Ciudad_Juarez", "America/Coral_Harbour", "America/Cordoba", "America/Costa_Rica", "America/Creston", "America/Cuiaba", "America/Curacao", "America/Danmarkshavn", "America/Dawson", "America/Dawson_Creek", "America/Denver", "America/Detroit", "America/Dominica", "America/Edmonton", "America/Eirunepe", "America/El_Salvador", "America/Ensenada", "America/Fort_Nelson", "America/Fort_Wayne", "America/Fortaleza", "America/Glace_Bay", "America/Godthab", "America/Goose_Bay", "America/Grand_Turk", "America/Grenada", "America/Guadeloupe", "America/Guatemala", "America/Guayaquil", "America/Guyana", "America/Halifax", "America/Havana", "America/Hermosillo", "America/Indiana/Indianapolis", "America/Indiana/Knox", "America/Indiana/Marengo", "America/Indiana/Petersburg", "America/Indiana/Tell_City", "America/Indiana/Vevay", "America/Indiana/Vincennes", "America/Indiana/Winamac", "America/Indianapolis", "America/Inuvik", "America/Iqaluit", "America/Jamaica", "America/Jujuy", "America/Juneau", "America/Kentucky/Louisville", "America/Kentucky/Monticello", "America/Knox_IN", "America/Kralendijk", "America/La_Paz", "America/Lima", "America/Los_Angeles", "America/Louisville", "America/Lower_Princes", "America/Maceio", "America/Managua", "America/Manaus", "America/Marigot", "America/Martinique", "America/Matamoros", "America/Mazatlan", "America/Mendoza", "America/Menominee", "America/Merida", "America/Metlakatla", "America/Mexico_City", "America/Miquelon", "America/Moncton", "America/Monterrey", "America/Montevideo", "America/Montreal", "America/Montserrat", "America/Nassau", "America/New_York", "America/Nipigon", "America/Nome", "America/Noronha", "America/North_Dakota/Beulah", "America/North_Dakota/Center", "America/North_Dakota/New_Salem", "America/Nuuk", "America/Ojinaga", "America/Panama", "America/Pangnirtung", "America/Paramaribo", "America/Phoenix", "America/Port-au-Prince", "America/Port_of_Spain", "America/Porto_Acre", "America/Porto_Velho", "America/Puerto_Rico", "America/Punta_Arenas", "America/Rainy_River", "America/Rankin_Inlet", "America/Recife", "America/Regina", "America/Resolute", "America/Rio_Branco", "America/Rosario", "America/Santa_Isabel", "America/Santarem", "America/Santiago", "America/Santo_Domingo", "America/Sao_Paulo", "America/Scoresbysund", "America/Shiprock", "America/Sitka", "America/St_Barthelemy", "America/St_Johns", "America/St_Kitts", "America/St_Lucia", "America/St_Thomas", "America/St_Vincent", "America/Swift_Current", "America/Tegucigalpa", "America/Thule", "America/Thunder_Bay", "America/Tijuana", "America/Toronto", "America/Tortola", "America/Vancouver", "America/Virgin", "America/Whitehorse", "America/Winnipeg", "America/Yakutat", "America/Yellowknife", "Antarctica/Casey", "Antarctica/Davis", "Antarctica/DumontDUrville", "Antarctica/Macquarie", "Antarctica/Mawson", "Antarctica/McMurdo", "Antarctica/Palmer", "Antarctica/Rothera", "Antarctica/South_Pole", "Antarctica/Syowa", "Antarctica/Troll", "Antarctica/Vostok", "Arctic/Longyearbyen", "Asia/Aden", "Asia/Almaty", "Asia/Amman", "Asia/Anadyr", "Asia/Aqtau", "Asia/Aqtobe", "Asia/Ashgabat", "Asia/Ashkhabad", "Asia/Atyrau", "Asia/Baghdad", "Asia/Bahrain", "Asia/Baku", "Asia/Bangkok", "Asia/Barnaul", "Asia/Beirut", "Asia/Bishkek", "Asia/Brunei", "Asia/Calcutta", "Asia/Chita", "Asia/Choibalsan", "Asia/Chongqing", "Asia/Chungking", "Asia/Colombo", "Asia/Dacca", "Asia/Damascus", "Asia/Dhaka", "Asia/Dili", "Asia/Dubai", "Asia/Dushanbe", "Asia/Famagusta", "Asia/Gaza", "Asia/Hanoi", "Asia/Harbin", "Asia/Hebron", "Asia/Ho_Chi_Minh", "Asia/Hong_Kong", "Asia/Hovd", "Asia/Irkutsk", "Asia/Istanbul", "Asia/Jakarta", "Asia/Jayapura", "Asia/Jerusalem", "Asia/Kabul", "Asia/Kamchatka", "Asia/Karachi", "Asia/Kashgar", "Asia/Kathmandu", "Asia/Katmandu", "Asia/Khandyga", "Asia/Kolkata", "Asia/Krasnoyarsk", "Asia/Kuala_Lumpur", "Asia/Kuching", "Asia/Kuwait", "Asia/Macao", "Asia/Macau", "Asia/Magadan", "Asia/Makassar", "Asia/Manila", "Asia/Muscat", "Asia/Nicosia", "Asia/Novokuznetsk", "Asia/Novosibirsk", "Asia/Omsk", "Asia/Oral", "Asia/Phnom_Penh", "Asia/Pontianak", "Asia/Pyongyang", "Asia/Qatar", "Asia/Qostanay", "Asia/Qyzylorda", "Asia/Rangoon", "Asia/Riyadh", "Asia/Saigon", "Asia/Sakhalin", "Asia/Samarkand", "Asia/Seoul", "Asia/Shanghai", "Asia/Singapore", "Asia/Srednekolymsk", "Asia/Taipei", "Asia/Tashkent", "Asia/Tbilisi", "Asia/Tehran", "Asia/Tel_Aviv", "Asia/Thimbu", "Asia/Thimphu", "Asia/Tokyo", "Asia/Tomsk", "Asia/Ujung_Pandang", "Asia/Ulaanbaatar", "Asia/Ulan_Bator", "Asia/Urumqi", "Asia/Ust-Nera", "Asia/Vientiane", "Asia/Vladivostok", "Asia/Yakutsk", "Asia/Yangon", "Asia/Yekaterinburg", "Asia/Yerevan", "Atlantic/Azores", "Atlantic/Bermuda", "Atlantic/Canary", "Atlantic/Cape_Verde", "Atlantic/Faeroe", "Atlantic/Faroe", "Atlantic/Jan_Mayen", "Atlantic/Madeira", "Atlantic/Reykjavik", "Atlantic/South_Georgia", "Atlantic/St_Helena", "Atlantic/Stanley", "Australia/ACT", "Australia/Adelaide", "Australia/Brisbane", "Australia/Broken_Hill", "Australia/Canberra", "Australia/Currie", "Australia/Darwin", "Australia/Eucla", "Australia/Hobart", "Australia/LHI", "Australia/Lindeman", "Australia/Lord_Howe", "Australia/Melbourne", "Australia/NSW", "Australia/North", "Australia/Perth", "Australia/Queensland", "Australia/South", "Australia/Sydney", "Australia/Tasmania", "Australia/Victoria", "Australia/West", "Australia/Yancowinna", "Brazil/Acre", "Brazil/DeNoronha", "Brazil/East", "Brazil/West", "CET", "CST6CDT", "Canada/Atlantic", "Canada/Central", "Canada/Eastern", "Canada/Mountain", "Canada/Newfoundland", "Canada/Pacific", "Canada/Saskatchewan", "Canada/Yukon", "Chile/Continental", "Chile/EasterIsland", "Cuba", "EET", "EST", "EST5EDT", "Egypt", "Eire", "Etc/GMT", "Etc/GMT+0", "Etc/GMT+1", "Etc/GMT+10", "Etc/GMT+11", "Etc/GMT+12", "Etc/GMT+2", "Etc/GMT+3", "Etc/GMT+4", "Etc/GMT+5", "Etc/GMT+6", "Etc/GMT+7", "Etc/GMT+8", "Etc/GMT+9", "Etc/GMT-0", "Etc/GMT-1", "Etc/GMT-10", "Etc/GMT-11", "Etc/GMT-12", "Etc/GMT-13", "Etc/GMT-14", "Etc/GMT-2", "Etc/GMT-3", "Etc/GMT-4", "Etc/GMT-5", "Etc/GMT-6", "Etc/GMT-7", "Etc/GMT-8", "Etc/GMT-9", "Etc/GMT0", "Etc/Greenwich", "Etc/UCT", "Etc/UTC", "Etc/Universal", "Etc/Zulu", "Europe/Amsterdam", "Europe/Andorra", "Europe/Astrakhan", "Europe/Athens", "Europe/Belfast", "Europe/Belgrade", "Europe/Berlin", "Europe/Bratislava", "Europe/Brussels", "Europe/Bucharest", "Europe/Budapest", "Europe/Busingen", "Europe/Chisinau", "Europe/Copenhagen", "Europe/Dublin", "Europe/Gibraltar", "Europe/Guernsey", "Europe/Helsinki", "Europe/Isle_of_Man", "Europe/Istanbul", "Europe/Jersey", "Europe/Kaliningrad", "Europe/Kiev", "Europe/Kirov", "Europe/Kyiv", "Europe/Lisbon", "Europe/Ljubljana", "Europe/London", "Europe/Luxembourg", "Europe/Madrid", "Europe/Malta", "Europe/Mariehamn", "Europe/Minsk", "Europe/Monaco", "Europe/Moscow", "Europe/Nicosia", "Europe/Oslo", "Europe/Paris", "Europe/Podgorica", "Europe/Prague", "Europe/Riga", "Europe/Rome", "Europe/Samara", "Europe/San_Marino", "Europe/Sarajevo", "Europe/Saratov", "Europe/Simferopol", "Europe/Skopje", "Europe/Sofia", "Europe/Stockholm", "Europe/Tallinn", "Europe/Tirane", "Europe/Tiraspol", "Europe/Ulyanovsk", "Europe/Uzhgorod", "Europe/Vaduz", "Europe/Vatican", "Europe/Vienna", "Europe/Vilnius", "Europe/Volgograd", "Europe/Warsaw", "Europe/Zagreb", "Europe/Zaporozhye", "Europe/Zurich", "Factory", "GB", "GB-Eire", "GMT", "GMT+0", "GMT-0", "GMT0", "Greenwich", "HST", "Hongkong", "Iceland", "Indian/Antananarivo", "Indian/Chagos", "Indian/Christmas", "Indian/Cocos", "Indian/Comoro", "Indian/Kerguelen", "Indian/Mahe", "Indian/Maldives", "Indian/Mauritius", "Indian/Mayotte", "Indian/Reunion", "Iran", "Israel", "Jamaica", "Japan", "Kwajalein", "Libya", "MET", "MST", "MST7MDT", "Mexico/BajaNorte", "Mexico/BajaSur", "Mexico/General", "NZ", "NZ-CHAT", "Navajo", "PRC", "PST8PDT", "Pacific/Apia", "Pacific/Auckland", "Pacific/Bougainville", "Pacific/Chatham", "Pacific/Chuuk", "Pacific/Easter", "Pacific/Efate", "Pacific/Enderbury", "Pacific/Fakaofo", "Pacific/Fiji", "Pacific/Funafuti", "Pacific/Galapagos", "Pacific/Gambier", "Pacific/Guadalcanal", "Pacific/Guam", "Pacific/Honolulu", "Pacific/Johnston", "Pacific/Kanton", "Pacific/Kiritimati", "Pacific/Kosrae", "Pacific/Kwajalein", "Pacific/Majuro", "Pacific/Marquesas", "Pacific/Midway", "Pacific/Nauru", "Pacific/Niue", "Pacific/Norfolk", "Pacific/Noumea", "Pacific/Pago_Pago", "Pacific/Palau", "Pacific/Pitcairn", "Pacific/Pohnpei", "Pacific/Ponape", "Pacific/Port_Moresby", "Pacific/Rarotonga", "Pacific/Saipan", "Pacific/Samoa", "Pacific/Tahiti", "Pacific/Tarawa", "Pacific/Tongatapu", "Pacific/Truk", "Pacific/Wake", "Pacific/Wallis", "Pacific/Yap", "Poland", "Portugal", "ROC", "ROK", "Singapore", "Turkey", "UCT", "US/Alaska", "US/Aleutian", "US/Arizona", "US/Central", "US/East-Indiana", "US/Eastern", "US/Hawaii", "US/Indiana-Starke", "US/Michigan", "US/Mountain", "US/Pacific", "US/Samoa", "UTC", "Universal", "W-SU", "WET", "Zulu" });
            tzbinary_tzst.Location = new System.Drawing.Point(222, 36);
            tzbinary_tzst.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tzbinary_tzst.Name = "tzbinary_tzst";
            tzbinary_tzst.Size = new System.Drawing.Size(139, 28);
            tzbinary_tzst.TabIndex = 21;
            tzbinary_tzst.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            // 
            // ms_utcoffset
            // 
            ms_utcoffset.AutoSize = true;
            ms_utcoffset.Location = new System.Drawing.Point(8, 25);
            ms_utcoffset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ms_utcoffset.Name = "ms_utcoffset";
            ms_utcoffset.Size = new System.Drawing.Size(262, 24);
            ms_utcoffset.TabIndex = 5;
            ms_utcoffset.Text = "UTC任意時間を使う(夏時間修正なし)";
            ms_utcoffset.UseVisualStyleBackColor = true;
            ms_utcoffset.CheckedChanged += ms_utcoffset_CheckedChanged;
            // 
            // ms_utcoffset_items
            // 
            ms_utcoffset_items.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ms_utcoffset_items.FormattingEnabled = true;
            ms_utcoffset_items.ImeMode = System.Windows.Forms.ImeMode.Disable;
            ms_utcoffset_items.Items.AddRange(new object[] { "(GMT-12:00)国際日付変更線西側 日付変更線標準時,M$0", "(GMT-11:00)ミッドウェー島、サモア サモア標準時,M$1", "(GMT-10:00)ハワイ ハワイ標準時,M$2", "(GMT-09:00)アラスカ アラスカ標準時,M$3", "(GMT-08:00)(米国およびカナダ) は、太平洋標準時ティファナ 太平洋標準時,M$4", "(GMT-07:00)(米国およびカナダ)、山地標準時 山地標準時,M$A", "(GMT-07:00)チワワ、ラパス、マサトラン メキシコ標準時 2,M$D", "(GMT-07:00)アリゾナ州 米国山地標準時,M$F", "(GMT-06:00)(米国およびカナダ) の中部標準時 中部標準時,M$14", "(GMT-06:00)サスカチェワン カナダ中部標準時,M$19", "(GMT-06:00)グアダラハラ、メキシコシティ、モンテレイ メキシコ山地標準時,M$1E", "(GMT-06:00)中央アメリカ 中央アメリカ標準時,M$21", "(GMT-05:00)(米国およびカナダ)、東部標準時 東部標準時,M$23", "(GMT-05:00)インディアナ (東部) 米国東部標準時,M$28", "(GMT-05:00)ボゴタ、リマ、Quito 亜北極地帯の太平洋標準時,M$2D", "(GMT-04:00)大西洋標準時 (カナダ) 大西洋標準時,M$32", "(GMT-04:00)ジョージタウン、ラパス、サン ・ ファン 亜北極地帯西部標準時,M$37", "(GMT-04:00)サンティアゴ 太平洋亜北極地帯 (標準時),M$38", "(GMT-03:30)ニューファンドランド ニューファンドランドおよびラブラドル標準時,M$3C", "(GMT-03:00)ブラジリア 南アメリカ東部標準時,M$41", "(GMT-03:00)ジョージタウン 亜北極地帯東部標準時,M$46", "(GMT-03:00)グリーンランド グリーンランド標準時,M$49", "(GMT-02:00)中部大西洋 中央大西洋標準時,M$4B", "(GMT-01:00)アゾレス諸島 アゾレス諸島標準時,M$50", "(GMT-01:00)カーボベルデ諸島 カーボベルデ標準時,M$53", "(GMT+00:00)グリニッジ標準時: ダブリン、エジンバラ、リスボン、ロンドン GMT 標準時,M$55", "(GMT+00:00)モンロビア、レイキャビク グリニッジ標準時,M$5A", "(GMT+01:00)サニーベイル, カリフォルニア州、ブラチスラバ、ブダペスト、Ljubljana、プラハ 中央ヨーロッパ標準時,M$5F", "(GMT+01:00)サラエボ、Skopje、ワルシャワ、Zagreb 中央ヨーロッパ標準時,M$64", "(GMT+01:00)ブリュッセル、コペンハーゲン、マドリッド、パリ ロマンス標準時,M$69", "(GMT+01:00)アムステルダム、ベルリン、ベルン、ローマ、ストックホルム、ウィーン 西ヨーロッパ標準時,M$6E", "(GMT+01:00)西中央アフリカ 西中央アフリカ標準時,M$71", "(GMT+02:00)ミンスク 東ヨーロッパ標準時,M$73", "(GMT+02:00)カイロ エジプト標準時,M$78", "(GMT+02:00)ヘルシンキ、キエフ、リガ、ソフィア、Tallinn、Vilnius ファイル (標準時),M$7D", "(GMT+02:00)アテネ、ブカレスト、イスタンブール GTB 標準時,M$82", "(GMT+02:00)エルサレム イスラエル標準時,M$87", "(GMT+02:00)ハラレ、プレトリア 南アフリカ標準時,M$8C", "(GMT+03:00)モスクワ、サンクト ペテルスブルグ、ボルゴグラード ロシア標準時,M$91", "(GMT+03:00)クウェート、リヤド アラブ標準時,M$96", "(GMT+03:00)ナイロビ 東アフリカ標準時,M$9B", "(GMT+03:00)バグダッド アラブ標準時,M$9E", "(GMT+03:30)テヘラン イラン標準時,M$A0", "(GMT+04:00)アブダビ、マスカット アラビア標準時,M$A5", "(GMT+04:00)バクー、トビリシ、エレバン コーカサス標準時,M$AA", "(GMT+04:30)カブール 移行アフガニスタン標準時,M$AF", "(GMT+05:00)エカテリンバーグ エカテリンバーグ標準時,M$B4", "(GMT+05:00)タシケント 西アジア標準時,M$B9", "(GMT+05:30)チェンナイ、カルカッタ、ムンバイ、ニューデリー インド標準時,M$BE", "(GMT+05:45)カトマンズ ネパール標準時,M$C1", "(GMT+06:00)アスタナ、ダッカ 中央アジア標準時,M$C3", "(GMT+06:00)スリジャヤワルダナプラコッテ スリランカ標準時,M$C8", "(GMT+06:00)アルマアトイ、ノボシビルスク 北中央アジア標準時,M$C9", "(GMT+06:30)ヤンゴン (ラングーン) ミャンマー標準時,M$CB", "(GMT+07:00)バンコク、ハノイ、ジャカルタ 東南アジア標準時,M$CD", "(GMT+07:00)クラスノヤルスク 北アジア標準時,M$CF", "(GMT+08:00)北京、重慶、ホンコン、ウルムチ 中国 (標準時),M$D2", "(GMT+08:00)クアラルンプール、シンガポール シンガポール標準時,M$D7", "(GMT+08:00)台北 台北標準時,M$DC", "(GMT+08:00)パース 西オーストラリア標準時,M$E1", "(GMT+08:00)イルクーツク、ウランバートル 北アジア東部標準時,M$E3", "(GMT+09:00)(ソウル) 韓国 (標準時),M$E6", "(GMT+09:00)大阪、札幌、東京 東京 (標準時),M$EB", "(GMT+09:00)ヤクーツク ヤクーツク標準時,M$F0", "(GMT+09:30)ダーウィン オーストラリア中央標準時,M$F5", "(GMT+09:30)アデレード 中央オーストラリア標準時,M$FA", "(GMT+10:00)キャンベラ、メルボルン、シドニー オーストラリア東部標準時,M$FF", "(GMT+10:00)ブリスベン 東オーストラリア標準時,M$104", "(GMT+10:00)ホバート タスマニア標準時,M$109", "(GMT+10:00)ウラジオ ストック ウラジオ ストック標準時,M$10E", "(GMT+10:00)グアム、ポートモレスビー 西太平洋標準時,M$113", "(GMT+11:00)マガダン、ソロモン諸島、ニューカレドニア 中央太平洋標準時,M$118", "(GMT+12:00)フィジー、カムチャツカ、マーシャル フィジー諸島標準時,M$11D", "(GMT+12:00)オークランド、ウェリントン ニュージーランド標準時,M$122", "(GMT+13:00)ヌクアロファ トンガ標準時,M$12C", "(GMT-03:00)ブエノスアイレス アゼルバイジャン標準時,M$80000040", "(GMT+02:00)コロンバス, ジョージア州 中東標準時,M$80000041", "(GMT+02:00)Amman ヨルダン標準時,M$80000042", "(GMT-06:00)グアダラハラ、メキシコシティ、モンテレー - 新規 中部標準時 (メキシコ),M$80000043", "(GMT-07:00)チワワ、ラパス、マサトラン - 新規 山地標準時 (メキシコ),M$80000044", "(GMT-08:00)ティファナ、バハカリフォルニア 太平洋標準時 (メキシコ),M$80000045", "(GMT+02:00)Windhoek ナミビア標準時,M$80000046", "(GMT+03:00)トビリシ グルジア標準時,M$80000047", "(GMT-04:00)Manaus 中央ブラジル標準時,M$80000048", "(GMT-03:00)モンテビデオ モンテビデオ標準時,M$80000049", "(GMT+04:00)エレバン アルメニア標準時,M$8000004A", "(GMT-04:30)カラカス ベネズエラ標準時,M$8000004B", "(GMT-03:00)ブエノスアイレス アルゼンチン標準時,M$8000004C", "(GMT+00:00)カサブランカ モロッコ標準時,M$8000004D", "(GMT+05:00)イスラマバード、カラチ パキスタン標準時,M$8000004E", "(GMT+04:00)ポートルイス モーリシャス標準時,M$8000004F", "(GMT+00:00)世界協定時刻 UTC,M$80000050", "(GMT-04:00)Asuncion パラグアイ標準時,M$80000051", "(GMT+12:00)Petropavlovsk Kamchatsky カムチャツカ標準時,M$80000052" });
            ms_utcoffset_items.Location = new System.Drawing.Point(0, 55);
            ms_utcoffset_items.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ms_utcoffset_items.Name = "ms_utcoffset_items";
            ms_utcoffset_items.Size = new System.Drawing.Size(365, 28);
            ms_utcoffset_items.TabIndex = 6;
            ms_utcoffset_items.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // ms_timezone
            // 
            ms_timezone.AutoSize = true;
            ms_timezone.Location = new System.Drawing.Point(8, 93);
            ms_timezone.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ms_timezone.Name = "ms_timezone";
            ms_timezone.Size = new System.Drawing.Size(237, 24);
            ms_timezone.TabIndex = 7;
            ms_timezone.Text = "M$のタイムゾーン(夏時間修正あり)";
            ms_timezone.UseVisualStyleBackColor = true;
            ms_timezone.CheckedChanged += ms_timezone_CheckedChanged;
            // 
            // ms_timezone_items
            // 
            ms_timezone_items.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ms_timezone_items.FormattingEnabled = true;
            ms_timezone_items.ImeMode = System.Windows.Forms.ImeMode.Disable;
            ms_timezone_items.Location = new System.Drawing.Point(4, 121);
            ms_timezone_items.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ms_timezone_items.Name = "ms_timezone_items";
            ms_timezone_items.Size = new System.Drawing.Size(365, 28);
            ms_timezone_items.TabIndex = 8;
            ms_timezone_items.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // panel5
            // 
            panel5.Controls.Add(locale);
            panel5.Controls.Add(localeBox);
            panel5.Controls.Add(elapst_left);
            panel5.Controls.Add(panel6);
            panel5.Controls.Add(normal_dateformat);
            panel5.Controls.Add(linkLabel1);
            panel5.Controls.Add(TIME_FORMAT);
            panel5.Controls.Add(time_span);
            panel5.Controls.Add(bar_length);
            panel5.Controls.Add(progressbar_length);
            panel5.Location = new System.Drawing.Point(20, 3);
            panel5.Name = "panel5";
            panel5.Size = new System.Drawing.Size(333, 137);
            panel5.TabIndex = 28;
            // 
            // locale
            // 
            locale.AutoSize = true;
            locale.Location = new System.Drawing.Point(163, 105);
            locale.Name = "locale";
            locale.Size = new System.Drawing.Size(53, 20);
            locale.TabIndex = 32;
            locale.Text = "ろけーる";
            // 
            // localeBox
            // 
            localeBox.FormattingEnabled = true;
            localeBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            localeBox.Items.AddRange(new object[] { "ja-JP: 日本語 (日本)", "en-US: 英語 (アメリカ)", "en-GB: 英語 (イギリス)", "es-ES: スペイン語 (スペイン)", "fr-FR: フランス語 (フランス)", "de-DE: ドイツ語 (ドイツ)", "zh-CN: 中国語 (中国)", "ko-KR: 韓国語 (韓国)" });
            localeBox.Location = new System.Drawing.Point(212, 102);
            localeBox.Name = "localeBox";
            localeBox.Size = new System.Drawing.Size(118, 28);
            localeBox.TabIndex = 31;
            localeBox.SelectedIndexChanged += localeBox_SelectedIndexChanged;
            // 
            // elapst_left
            // 
            elapst_left.FormattingEnabled = true;
            elapst_left.Items.AddRange(new object[] { "dd日hh時間mm分ss秒", "dd日hh時間mm分ss秒msミリ秒", "dd hh:mm:ss.ms", "dd hh:mm:ss", "HH:mm:ss", "MM:ss", "SS", "MS" });
            elapst_left.Location = new System.Drawing.Point(85, 28);
            elapst_left.Name = "elapst_left";
            elapst_left.Size = new System.Drawing.Size(239, 28);
            elapst_left.TabIndex = 29;
            elapst_left.Text = "dd日hh時間mm分ss秒";
            elapst_left.SelectedIndexChanged += comboBox11_SelectedIndexChanged;
            elapst_left.TextChanged += comboBox11_SelectedIndexChanged;
            // 
            // panel6
            // 
            panel6.Controls.Add(linkLabel2);
            panel6.Controls.Add(noda_dateformat);
            panel6.Location = new System.Drawing.Point(157, 57);
            panel6.Name = "panel6";
            panel6.Size = new System.Drawing.Size(339, 37);
            panel6.TabIndex = 11;
            panel6.Visible = false;
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.Location = new System.Drawing.Point(6, 9);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new System.Drawing.Size(66, 20);
            linkLabel2.TabIndex = 11;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "のだ時刻:";
            linkLabel2.LinkClicked += linkLabel2_LinkClicked;
            // 
            // noda_dateformat
            // 
            noda_dateformat.FormattingEnabled = true;
            noda_dateformat.Items.AddRange(new object[] { "HH':'mm", "MM'-'dd", "MM'-'dd HH':'mm", "yyyy'-'MM'-'dd HH':'mm", "yyyy'-'MM'-'dd'T'HH':'mm z", "yyyy'-'MM'-'dd'T'HH':'mm':'ss z", "yyyy'-'MM'-'dd'T'HH':'mm':'ss '('o<g>')'", "yyyy'-'MM'-'dd'T'HH':'mm':'ss z '('o<g>')'", "yyyy'-'MM'-'dd'T'HH':'mm':'ss;FFFFFFF z '('o<g>')'", "yyyy'年'MMMd日'('ddd')'HH':'mm':'ss", "G", "F" });
            noda_dateformat.Location = new System.Drawing.Point(78, 6);
            noda_dateformat.Name = "noda_dateformat";
            noda_dateformat.Size = new System.Drawing.Size(244, 28);
            noda_dateformat.TabIndex = 4;
            noda_dateformat.Text = "MM'-'dd HH':'mm";
            noda_dateformat.SelectedIndexChanged += noda_dateformat_SelectedIndexChanged;
            noda_dateformat.TextChanged += noda_dateformat_SelectedIndexChanged;
            // 
            // normal_dateformat
            // 
            normal_dateformat.FormattingEnabled = true;
            normal_dateformat.Items.AddRange(new object[] { "yyyy/MM/dd HH:mm:ss", "yyyy/MM/dd HH:mm", "yyyy/MM/dd HH", "yyyy/MM/dd", "MM/dd HH:mm:ss", "MM/dd HH:mm", "MM/dd HH", "MM/dd", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm", "yyyy-MM-dd HH", "yyyy-MM-dd", "MM-dd HH:mm:ss", "MM-dd HH:mm", "MM-dd HH", "MM-dd", "dd MMM yyyy HH:mm", "yyyy年MMMd日(ddd) HH:mm:ss", "yyyy-MM-ddTHH:mm:ss z", "yyyy-MM-ddTHH:mm:ss zz", "yyyy-MM-ddTHH:mm:sszzz", "yyyy-MM-ddTHH:mm:ss K", "yyyy-MM-ddTHH:mm:ss %TZ %Z %z", "yyyy-MM-ddTHH:mm:ss K zzz %TZ %Z %z", "%PO" });
            normal_dateformat.Location = new System.Drawing.Point(79, 66);
            normal_dateformat.Name = "normal_dateformat";
            normal_dateformat.Size = new System.Drawing.Size(246, 28);
            normal_dateformat.TabIndex = 12;
            normal_dateformat.Text = "yyyy/MM/dd HH:mm";
            normal_dateformat.SelectedIndexChanged += comboBox10_SelectedIndexChanged;
            normal_dateformat.TextChanged += comboBox10_SelectedIndexChanged;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new System.Drawing.Point(4, 71);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(69, 20);
            linkLabel1.TabIndex = 30;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "現在時刻";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // TIME_FORMAT
            // 
            TIME_FORMAT.AutoSize = true;
            TIME_FORMAT.Location = new System.Drawing.Point(5, 5);
            TIME_FORMAT.Name = "TIME_FORMAT";
            TIME_FORMAT.Size = new System.Drawing.Size(109, 20);
            TIME_FORMAT.TabIndex = 29;
            TIME_FORMAT.Text = "[時刻表示形式]";
            // 
            // bar_length
            // 
            bar_length.FormattingEnabled = true;
            bar_length.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            bar_length.Items.AddRange(new object[] { "390", "145", "130", "97", "78" });
            bar_length.Location = new System.Drawing.Point(109, 102);
            bar_length.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            bar_length.Name = "bar_length";
            bar_length.Size = new System.Drawing.Size(51, 28);
            bar_length.TabIndex = 9;
            bar_length.Text = "390";
            bar_length.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            bar_length.TextChanged += comboBox3_TextChanged;
            // 
            // progressbar_length
            // 
            progressbar_length.AutoSize = true;
            progressbar_length.Location = new System.Drawing.Point(5, 105);
            progressbar_length.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            progressbar_length.Name = "progressbar_length";
            progressbar_length.Size = new System.Drawing.Size(98, 20);
            progressbar_length.TabIndex = 10;
            progressbar_length.Text = "進捗バーの長さ";
            // 
            // panel2
            // 
            panel2.Controls.Add(button2);
            panel2.Controls.Add(key_value);
            panel2.Controls.Add(change_baseurl);
            panel2.Controls.Add(baseurl_keyval);
            panel2.Controls.Add(baseurl_txt);
            panel2.Controls.Add(label13);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(json_test);
            panel2.Controls.Add(parse_target);
            panel2.Controls.Add(custom_url_path);
            panel2.Controls.Add(custom_uri);
            panel2.Controls.Add(target_path);
            panel2.Location = new System.Drawing.Point(21, 410);
            panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(368, 147);
            panel2.TabIndex = 20;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(211, 36);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(28, 29);
            button2.TabIndex = 28;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            button2.Click += button2_Click_2;
            // 
            // key_value
            // 
            key_value.AutoSize = true;
            key_value.Location = new System.Drawing.Point(235, 38);
            key_value.Name = "key_value";
            key_value.Size = new System.Drawing.Size(31, 20);
            key_value.TabIndex = 27;
            key_value.Text = "key";
            // 
            // change_baseurl
            // 
            change_baseurl.AutoSize = true;
            change_baseurl.Location = new System.Drawing.Point(7, 37);
            change_baseurl.Name = "change_baseurl";
            change_baseurl.Size = new System.Drawing.Size(121, 24);
            change_baseurl.TabIndex = 26;
            change_baseurl.Text = "baseurlの変更";
            change_baseurl.UseVisualStyleBackColor = true;
            change_baseurl.CheckedChanged += change_baseurl_CheckedChanged;
            // 
            // baseurl_keyval
            // 
            baseurl_keyval.Location = new System.Drawing.Point(269, 35);
            baseurl_keyval.Name = "baseurl_keyval";
            baseurl_keyval.Size = new System.Drawing.Size(86, 27);
            baseurl_keyval.TabIndex = 25;
            baseurl_keyval.Text = "gakumasu,deresute,mirsita,shanimasu,syanison,yumesute,proseka,proseka_kr,proseka_los,proseka_hk";
            baseurl_keyval.TextChanged += baseurl_keyval_TextChanged;
            // 
            // baseurl_txt
            // 
            baseurl_txt.Location = new System.Drawing.Point(122, 37);
            baseurl_txt.Name = "baseurl_txt";
            baseurl_txt.Size = new System.Drawing.Size(83, 27);
            baseurl_txt.TabIndex = 24;
            baseurl_txt.Text = "https://script.google.com/macros/s/AKfycbxH2PF9yxHCZCp-e-n4LrGHRSi-Ag-E32trEdw_MhLrMf-cnkb8qwy27KwD7Deut1Mj2Q/exec";
            baseurl_txt.TextChanged += textBox1_TextChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(6, 9);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(347, 20);
            label13.TabIndex = 23;
            label13.Text = "[base/かすたむJS,時刻データの受信先,ふぁいるぱすかURl]";
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(324, 73);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(28, 29);
            button3.TabIndex = 22;
            button3.Text = "...";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_1;
            // 
            // json_test
            // 
            json_test.Location = new System.Drawing.Point(324, 103);
            json_test.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            json_test.Name = "json_test";
            json_test.Size = new System.Drawing.Size(39, 39);
            json_test.TabIndex = 16;
            json_test.Text = "🎄";
            json_test.UseVisualStyleBackColor = true;
            json_test.Click += button1_Click;
            // 
            // parse_target
            // 
            parse_target.Location = new System.Drawing.Point(85, 112);
            parse_target.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            parse_target.Name = "parse_target";
            parse_target.Size = new System.Drawing.Size(231, 27);
            parse_target.TabIndex = 12;
            parse_target.Text = "/data/name,/data/start,/data/end";
            parse_target.TextChanged += textBox4_TextChanged;
            // 
            // custom_url_path
            // 
            custom_url_path.FormattingEnabled = true;
            custom_url_path.ImeMode = System.Windows.Forms.ImeMode.On;
            custom_url_path.Items.AddRange(new object[] { "JSONサンプル{} https://script.google.com/macros/s/AKfycbxH2PF9yxHCZCp-e-n4LrGHRSi-Ag-E32trEdw_MhLrMf-cnkb8qwy27KwD7Deut1Mj2Q/exec", "JSONサンプル[] https://script.google.com/macros/s/AKfycbw27NVH4JZWekqFZIXiNsZ08e3S2MxL6sFW01rZpBFLIVEIK1VAPkiMffpCE46r6ZRL/exec", "旧ぐぐるのパラメーター対応版()このあぷりのでふぉ　https://script.google.com/macros/s/AKfycbxiN0USvNN0hQyO5b3Ep_oJy_qQxCRAlT4NU954QXKYZ6GrGyzsBnhi8RgMHLZHct-QJg/exec?game=all", "テストケース多重ねすと　https://script.google.com/macros/s/AKfycbwhsv6LlhTmFM0CIc7vlXrNhMMQ9II23HxuTIfWYUSTyNRewsOJAm-des6xI2uhWYKN/exec", "さしゅうあいまも含んだやつ()これだけあった()　https://script.google.com/macros/s/AKfycbyVRqHhnG40A5RSAyIe9nX4kypbQ4_67hqXHCErEnKv3ZwoycmtUo-492jfq3Hy4rCxEw/exec", "...でのファイル読み込みいちおうローカルファイルも対応している" });
            custom_url_path.Location = new System.Drawing.Point(124, 74);
            custom_url_path.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            custom_url_path.Name = "custom_url_path";
            custom_url_path.Size = new System.Drawing.Size(192, 28);
            custom_url_path.TabIndex = 21;
            custom_url_path.Text = " https://script.google.com/macros/s/AKfycbw27NVH4JZWekqFZIXiNsZ08e3S2MxL6sFW01rZpBFLIVEIK1VAPkiMffpCE46r6ZRL/exec";
            custom_url_path.SelectedIndexChanged += comboBox5_SelectedIndexChanged;
            custom_url_path.TextChanged += comboBox5_TextChanged;
            // 
            // custom_uri
            // 
            custom_uri.AutoSize = true;
            custom_uri.Location = new System.Drawing.Point(7, 77);
            custom_uri.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            custom_uri.Name = "custom_uri";
            custom_uri.Size = new System.Drawing.Size(109, 20);
            custom_uri.TabIndex = 14;
            custom_uri.Text = "かすたむパス/URI";
            custom_uri.Click += label6_Click;
            // 
            // target_path
            // 
            target_path.AutoSize = true;
            target_path.Location = new System.Drawing.Point(3, 115);
            target_path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            target_path.Name = "target_path";
            target_path.Size = new System.Drawing.Size(72, 20);
            target_path.TabIndex = 14;
            target_path.Text = "パース対象";
            target_path.Click += label6_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(33, 144);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(0, 20);
            label3.TabIndex = 4;
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(439, 45);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox3.Size = new System.Drawing.Size(476, 526);
            textBox3.TabIndex = 2;
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // y_start
            // 
            y_start.FormattingEnabled = true;
            y_start.Items.AddRange(new object[] { "1900", "1950", "2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029", "2030", "2031", "2032", "2033", "2034", "2035", "2036", "2037", "2038" });
            y_start.Location = new System.Drawing.Point(543, 577);
            y_start.Name = "y_start";
            y_start.Size = new System.Drawing.Size(73, 28);
            y_start.TabIndex = 4;
            y_start.Text = "2000";
            y_start.SelectedIndexChanged += comboBox7_SelectedIndexChanged;
            // 
            // y_end
            // 
            y_end.FormattingEnabled = true;
            y_end.Items.AddRange(new object[] { "2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029", "2030", "2031", "2032", "2033", "2034", "2035", "2036", "2037", "2038" });
            y_end.Location = new System.Drawing.Point(654, 577);
            y_end.Name = "y_end";
            y_end.Size = new System.Drawing.Size(88, 28);
            y_end.TabIndex = 5;
            y_end.Text = "2037";
            y_end.SelectedIndexChanged += comboBox8_SelectedIndexChanged;
            // 
            // use_year_filter
            // 
            use_year_filter.AutoSize = true;
            use_year_filter.Location = new System.Drawing.Point(439, 579);
            use_year_filter.Name = "use_year_filter";
            use_year_filter.Size = new System.Drawing.Size(98, 24);
            use_year_filter.TabIndex = 6;
            use_year_filter.Text = "年フィルター";
            use_year_filter.UseVisualStyleBackColor = true;
            use_year_filter.CheckedChanged += checkBox4_CheckedChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(624, 580);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(24, 20);
            label8.TabIndex = 7;
            label8.Text = "～";
            // 
            // parse_test
            // 
            parse_test.Location = new System.Drawing.Point(827, 577);
            parse_test.Name = "parse_test";
            parse_test.Size = new System.Drawing.Size(113, 27);
            parse_test.TabIndex = 8;
            parse_test.Text = "2024/12/11";
            parse_test.TextChanged += textBox6_TextChanged;
            // 
            // test_date
            // 
            test_date.AutoSize = true;
            test_date.Location = new System.Drawing.Point(748, 580);
            test_date.Name = "test_date";
            test_date.Size = new System.Drawing.Size(73, 20);
            test_date.TabIndex = 9;
            test_date.Text = "日付てすと";
            // 
            // INFOMATION
            // 
            INFOMATION.AutoSize = true;
            INFOMATION.Location = new System.Drawing.Point(439, 22);
            INFOMATION.Name = "INFOMATION";
            INFOMATION.Size = new System.Drawing.Size(211, 20);
            INFOMATION.TabIndex = 10;
            INFOMATION.Text = "tzdatabaseのバイナリ情報の確認";
            // 
            // dtformat
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(957, 615);
            Controls.Add(INFOMATION);
            Controls.Add(test_date);
            Controls.Add(parse_test);
            Controls.Add(label8);
            Controls.Add(use_year_filter);
            Controls.Add(y_end);
            Controls.Add(y_start);
            Controls.Add(textBox3);
            Controls.Add(panel1);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "dtformat";
            Text = "datetimeformat";
            FormClosed += dtformat_FormClosed;
            Load += Form3_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label time_span;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox ms_utcoffset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ms_utcoffset_items;
        private System.Windows.Forms.ComboBox ms_timezone_items;
        private System.Windows.Forms.CheckBox ms_timezone;
        private System.Windows.Forms.Label progressbar_length;
        private System.Windows.Forms.ComboBox bar_length;
        private System.Windows.Forms.Label target_path;
        private System.Windows.Forms.Button json_test;
        public System.Windows.Forms.TextBox parse_target;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox tzbinary_tzst;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button tzbinary_dir_select;
        private System.Windows.Forms.CheckBox tzbinary_timezone;
        private System.Windows.Forms.Label custom_uri;
        private System.Windows.Forms.ComboBox custom_url_path;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ComboBox y_start;
        private System.Windows.Forms.ComboBox y_end;
        private System.Windows.Forms.CheckBox use_year_filter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox parse_test;
        private System.Windows.Forms.Label test_date;
        private System.Windows.Forms.CheckBox custom_local;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label TIME_FORMAT;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label TIMEZONE_MODE;
        private System.Windows.Forms.CheckBox noda_timezone;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label INFOMATION;
        private System.Windows.Forms.ComboBox noda_timezone_items;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ComboBox noda_dateformat;
        private System.Windows.Forms.ComboBox normal_dateformat;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ComboBox elapst_left;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.TextBox baseurl_txt;
        private System.Windows.Forms.TextBox baseurl_keyval;
        private System.Windows.Forms.CheckBox change_baseurl;
        private System.Windows.Forms.Label key_value;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox invaid_ambigous;
        private System.Windows.Forms.Label locale;
        private System.Windows.Forms.ComboBox localeBox;
        private System.Windows.Forms.ComboBox tzbinary_path;
    }
}