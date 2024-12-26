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
            label1 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            custom_local = new System.Windows.Forms.CheckBox();
            panel3 = new System.Windows.Forms.Panel();
            label5 = new System.Windows.Forms.Label();
            button2 = new System.Windows.Forms.Button();
            checkBox3 = new System.Windows.Forms.CheckBox();
            comboBox4 = new System.Windows.Forms.ComboBox();
            textBox5 = new System.Windows.Forms.TextBox();
            panel2 = new System.Windows.Forms.Panel();
            button3 = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            textBox4 = new System.Windows.Forms.TextBox();
            comboBox5 = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            comboBox3 = new System.Windows.Forms.ComboBox();
            comboBox2 = new System.Windows.Forms.ComboBox();
            checkBox2 = new System.Windows.Forms.CheckBox();
            comboBox1 = new System.Windows.Forms.ComboBox();
            checkBox1 = new System.Windows.Forms.CheckBox();
            label3 = new System.Windows.Forms.Label();
            textBox2 = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            textBox3 = new System.Windows.Forms.TextBox();
            comboBox7 = new System.Windows.Forms.ComboBox();
            comboBox8 = new System.Windows.Forms.ComboBox();
            checkBox4 = new System.Windows.Forms.CheckBox();
            label8 = new System.Windows.Forms.Label();
            textBox6 = new System.Windows.Forms.TextBox();
            label9 = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(28, 24);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(73, 20);
            label1.TabIndex = 0;
            label1.Text = "経過/残り:";
            label1.Click += label1_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(custom_local);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(comboBox3);
            panel1.Controls.Add(comboBox2);
            panel1.Controls.Add(checkBox2);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(checkBox1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label1);
            panel1.Location = new System.Drawing.Point(17, 45);
            panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(403, 500);
            panel1.TabIndex = 1;
            // 
            // custom_local
            // 
            custom_local.AutoSize = true;
            custom_local.Location = new System.Drawing.Point(231, 346);
            custom_local.Name = "custom_local";
            custom_local.Size = new System.Drawing.Size(158, 24);
            custom_local.TabIndex = 27;
            custom_local.Text = "時差なしにゾーン適用";
            custom_local.UseVisualStyleBackColor = true;
            custom_local.CheckedChanged += custom_local_CheckedChanged;
            // 
            // panel3
            // 
            panel3.Controls.Add(label5);
            panel3.Controls.Add(button2);
            panel3.Controls.Add(checkBox3);
            panel3.Controls.Add(comboBox4);
            panel3.Controls.Add(textBox5);
            panel3.Location = new System.Drawing.Point(25, 265);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(372, 73);
            panel3.TabIndex = 26;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(4, 13);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(89, 20);
            label5.TabIndex = 23;
            label5.Text = "tzdateのパス:";
            label5.Click += label5_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(324, 10);
            button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(24, 29);
            button2.TabIndex = 24;
            button2.Text = "..";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click_1;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new System.Drawing.Point(4, 44);
            checkBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new System.Drawing.Size(211, 24);
            checkBox3.TabIndex = 25;
            checkBox3.Text = "TZDBを使う(夏時間修正あり)";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // comboBox4
            // 
            comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox4.FormattingEnabled = true;
            comboBox4.ImeMode = System.Windows.Forms.ImeMode.Disable;
            comboBox4.Items.AddRange(new object[] { "Africa/Abidjan", "Africa/Accra", "Africa/Addis_Ababa", "Africa/Algiers", "Africa/Asmara", "Africa/Asmera", "Africa/Bamako", "Africa/Bangui", "Africa/Banjul", "Africa/Bissau", "Africa/Blantyre", "Africa/Brazzaville", "Africa/Bujumbura", "Africa/Cairo", "Africa/Casablanca", "Africa/Ceuta", "Africa/Conakry", "Africa/Dakar", "Africa/Dar_es_Salaam", "Africa/Djibouti", "Africa/Douala", "Africa/El_Aaiun", "Africa/Freetown", "Africa/Gaborone", "Africa/Harare", "Africa/Johannesburg", "Africa/Juba", "Africa/Kampala", "Africa/Khartoum", "Africa/Kigali", "Africa/Kinshasa", "Africa/Lagos", "Africa/Libreville", "Africa/Lome", "Africa/Luanda", "Africa/Lubumbashi", "Africa/Lusaka", "Africa/Malabo", "Africa/Maputo", "Africa/Maseru", "Africa/Mbabane", "Africa/Mogadishu", "Africa/Monrovia", "Africa/Nairobi", "Africa/Ndjamena", "Africa/Niamey", "Africa/Nouakchott", "Africa/Ouagadougou", "Africa/Porto-Novo", "Africa/Sao_Tome", "Africa/Timbuktu", "Africa/Tripoli", "Africa/Tunis", "Africa/Windhoek", "America/Adak", "America/Anchorage", "America/Anguilla", "America/Antigua", "America/Araguaina", "America/Argentina/Buenos_Aires", "America/Argentina/Catamarca", "America/Argentina/ComodRivadavia", "America/Argentina/Cordoba", "America/Argentina/Jujuy", "America/Argentina/La_Rioja", "America/Argentina/Mendoza", "America/Argentina/Rio_Gallegos", "America/Argentina/Salta", "America/Argentina/San_Juan", "America/Argentina/San_Luis", "America/Argentina/Tucuman", "America/Argentina/Ushuaia", "America/Aruba", "America/Asuncion", "America/Atikokan", "America/Atka", "America/Bahia", "America/Bahia_Banderas", "America/Barbados", "America/Belem", "America/Belize", "America/Blanc-Sablon", "America/Boa_Vista", "America/Bogota", "America/Boise", "America/Buenos_Aires", "America/Cambridge_Bay", "America/Campo_Grande", "America/Cancun", "America/Caracas", "America/Catamarca", "America/Cayenne", "America/Cayman", "America/Chicago", "America/Chihuahua", "America/Ciudad_Juarez", "America/Coral_Harbour", "America/Cordoba", "America/Costa_Rica", "America/Creston", "America/Cuiaba", "America/Curacao", "America/Danmarkshavn", "America/Dawson", "America/Dawson_Creek", "America/Denver", "America/Detroit", "America/Dominica", "America/Edmonton", "America/Eirunepe", "America/El_Salvador", "America/Ensenada", "America/Fort_Nelson", "America/Fort_Wayne", "America/Fortaleza", "America/Glace_Bay", "America/Godthab", "America/Goose_Bay", "America/Grand_Turk", "America/Grenada", "America/Guadeloupe", "America/Guatemala", "America/Guayaquil", "America/Guyana", "America/Halifax", "America/Havana", "America/Hermosillo", "America/Indiana/Indianapolis", "America/Indiana/Knox", "America/Indiana/Marengo", "America/Indiana/Petersburg", "America/Indiana/Tell_City", "America/Indiana/Vevay", "America/Indiana/Vincennes", "America/Indiana/Winamac", "America/Indianapolis", "America/Inuvik", "America/Iqaluit", "America/Jamaica", "America/Jujuy", "America/Juneau", "America/Kentucky/Louisville", "America/Kentucky/Monticello", "America/Knox_IN", "America/Kralendijk", "America/La_Paz", "America/Lima", "America/Los_Angeles", "America/Louisville", "America/Lower_Princes", "America/Maceio", "America/Managua", "America/Manaus", "America/Marigot", "America/Martinique", "America/Matamoros", "America/Mazatlan", "America/Mendoza", "America/Menominee", "America/Merida", "America/Metlakatla", "America/Mexico_City", "America/Miquelon", "America/Moncton", "America/Monterrey", "America/Montevideo", "America/Montreal", "America/Montserrat", "America/Nassau", "America/New_York", "America/Nipigon", "America/Nome", "America/Noronha", "America/North_Dakota/Beulah", "America/North_Dakota/Center", "America/North_Dakota/New_Salem", "America/Nuuk", "America/Ojinaga", "America/Panama", "America/Pangnirtung", "America/Paramaribo", "America/Phoenix", "America/Port-au-Prince", "America/Port_of_Spain", "America/Porto_Acre", "America/Porto_Velho", "America/Puerto_Rico", "America/Punta_Arenas", "America/Rainy_River", "America/Rankin_Inlet", "America/Recife", "America/Regina", "America/Resolute", "America/Rio_Branco", "America/Rosario", "America/Santa_Isabel", "America/Santarem", "America/Santiago", "America/Santo_Domingo", "America/Sao_Paulo", "America/Scoresbysund", "America/Shiprock", "America/Sitka", "America/St_Barthelemy", "America/St_Johns", "America/St_Kitts", "America/St_Lucia", "America/St_Thomas", "America/St_Vincent", "America/Swift_Current", "America/Tegucigalpa", "America/Thule", "America/Thunder_Bay", "America/Tijuana", "America/Toronto", "America/Tortola", "America/Vancouver", "America/Virgin", "America/Whitehorse", "America/Winnipeg", "America/Yakutat", "America/Yellowknife", "Antarctica/Casey", "Antarctica/Davis", "Antarctica/DumontDUrville", "Antarctica/Macquarie", "Antarctica/Mawson", "Antarctica/McMurdo", "Antarctica/Palmer", "Antarctica/Rothera", "Antarctica/South_Pole", "Antarctica/Syowa", "Antarctica/Troll", "Antarctica/Vostok", "Arctic/Longyearbyen", "Asia/Aden", "Asia/Almaty", "Asia/Amman", "Asia/Anadyr", "Asia/Aqtau", "Asia/Aqtobe", "Asia/Ashgabat", "Asia/Ashkhabad", "Asia/Atyrau", "Asia/Baghdad", "Asia/Bahrain", "Asia/Baku", "Asia/Bangkok", "Asia/Barnaul", "Asia/Beirut", "Asia/Bishkek", "Asia/Brunei", "Asia/Calcutta", "Asia/Chita", "Asia/Choibalsan", "Asia/Chongqing", "Asia/Chungking", "Asia/Colombo", "Asia/Dacca", "Asia/Damascus", "Asia/Dhaka", "Asia/Dili", "Asia/Dubai", "Asia/Dushanbe", "Asia/Famagusta", "Asia/Gaza", "Asia/Hanoi", "Asia/Harbin", "Asia/Hebron", "Asia/Ho_Chi_Minh", "Asia/Hong_Kong", "Asia/Hovd", "Asia/Irkutsk", "Asia/Istanbul", "Asia/Jakarta", "Asia/Jayapura", "Asia/Jerusalem", "Asia/Kabul", "Asia/Kamchatka", "Asia/Karachi", "Asia/Kashgar", "Asia/Kathmandu", "Asia/Katmandu", "Asia/Khandyga", "Asia/Kolkata", "Asia/Krasnoyarsk", "Asia/Kuala_Lumpur", "Asia/Kuching", "Asia/Kuwait", "Asia/Macao", "Asia/Macau", "Asia/Magadan", "Asia/Makassar", "Asia/Manila", "Asia/Muscat", "Asia/Nicosia", "Asia/Novokuznetsk", "Asia/Novosibirsk", "Asia/Omsk", "Asia/Oral", "Asia/Phnom_Penh", "Asia/Pontianak", "Asia/Pyongyang", "Asia/Qatar", "Asia/Qostanay", "Asia/Qyzylorda", "Asia/Rangoon", "Asia/Riyadh", "Asia/Saigon", "Asia/Sakhalin", "Asia/Samarkand", "Asia/Seoul", "Asia/Shanghai", "Asia/Singapore", "Asia/Srednekolymsk", "Asia/Taipei", "Asia/Tashkent", "Asia/Tbilisi", "Asia/Tehran", "Asia/Tel_Aviv", "Asia/Thimbu", "Asia/Thimphu", "Asia/Tokyo", "Asia/Tomsk", "Asia/Ujung_Pandang", "Asia/Ulaanbaatar", "Asia/Ulan_Bator", "Asia/Urumqi", "Asia/Ust-Nera", "Asia/Vientiane", "Asia/Vladivostok", "Asia/Yakutsk", "Asia/Yangon", "Asia/Yekaterinburg", "Asia/Yerevan", "Atlantic/Azores", "Atlantic/Bermuda", "Atlantic/Canary", "Atlantic/Cape_Verde", "Atlantic/Faeroe", "Atlantic/Faroe", "Atlantic/Jan_Mayen", "Atlantic/Madeira", "Atlantic/Reykjavik", "Atlantic/South_Georgia", "Atlantic/St_Helena", "Atlantic/Stanley", "Australia/ACT", "Australia/Adelaide", "Australia/Brisbane", "Australia/Broken_Hill", "Australia/Canberra", "Australia/Currie", "Australia/Darwin", "Australia/Eucla", "Australia/Hobart", "Australia/LHI", "Australia/Lindeman", "Australia/Lord_Howe", "Australia/Melbourne", "Australia/NSW", "Australia/North", "Australia/Perth", "Australia/Queensland", "Australia/South", "Australia/Sydney", "Australia/Tasmania", "Australia/Victoria", "Australia/West", "Australia/Yancowinna", "Brazil/Acre", "Brazil/DeNoronha", "Brazil/East", "Brazil/West", "CET", "CST6CDT", "Canada/Atlantic", "Canada/Central", "Canada/Eastern", "Canada/Mountain", "Canada/Newfoundland", "Canada/Pacific", "Canada/Saskatchewan", "Canada/Yukon", "Chile/Continental", "Chile/EasterIsland", "Cuba", "EET", "EST", "EST5EDT", "Egypt", "Eire", "Etc/GMT", "Etc/GMT+0", "Etc/GMT+1", "Etc/GMT+10", "Etc/GMT+11", "Etc/GMT+12", "Etc/GMT+2", "Etc/GMT+3", "Etc/GMT+4", "Etc/GMT+5", "Etc/GMT+6", "Etc/GMT+7", "Etc/GMT+8", "Etc/GMT+9", "Etc/GMT-0", "Etc/GMT-1", "Etc/GMT-10", "Etc/GMT-11", "Etc/GMT-12", "Etc/GMT-13", "Etc/GMT-14", "Etc/GMT-2", "Etc/GMT-3", "Etc/GMT-4", "Etc/GMT-5", "Etc/GMT-6", "Etc/GMT-7", "Etc/GMT-8", "Etc/GMT-9", "Etc/GMT0", "Etc/Greenwich", "Etc/UCT", "Etc/UTC", "Etc/Universal", "Etc/Zulu", "Europe/Amsterdam", "Europe/Andorra", "Europe/Astrakhan", "Europe/Athens", "Europe/Belfast", "Europe/Belgrade", "Europe/Berlin", "Europe/Bratislava", "Europe/Brussels", "Europe/Bucharest", "Europe/Budapest", "Europe/Busingen", "Europe/Chisinau", "Europe/Copenhagen", "Europe/Dublin", "Europe/Gibraltar", "Europe/Guernsey", "Europe/Helsinki", "Europe/Isle_of_Man", "Europe/Istanbul", "Europe/Jersey", "Europe/Kaliningrad", "Europe/Kiev", "Europe/Kirov", "Europe/Kyiv", "Europe/Lisbon", "Europe/Ljubljana", "Europe/London", "Europe/Luxembourg", "Europe/Madrid", "Europe/Malta", "Europe/Mariehamn", "Europe/Minsk", "Europe/Monaco", "Europe/Moscow", "Europe/Nicosia", "Europe/Oslo", "Europe/Paris", "Europe/Podgorica", "Europe/Prague", "Europe/Riga", "Europe/Rome", "Europe/Samara", "Europe/San_Marino", "Europe/Sarajevo", "Europe/Saratov", "Europe/Simferopol", "Europe/Skopje", "Europe/Sofia", "Europe/Stockholm", "Europe/Tallinn", "Europe/Tirane", "Europe/Tiraspol", "Europe/Ulyanovsk", "Europe/Uzhgorod", "Europe/Vaduz", "Europe/Vatican", "Europe/Vienna", "Europe/Vilnius", "Europe/Volgograd", "Europe/Warsaw", "Europe/Zagreb", "Europe/Zaporozhye", "Europe/Zurich", "Factory", "GB", "GB-Eire", "GMT", "GMT+0", "GMT-0", "GMT0", "Greenwich", "HST", "Hongkong", "Iceland", "Indian/Antananarivo", "Indian/Chagos", "Indian/Christmas", "Indian/Cocos", "Indian/Comoro", "Indian/Kerguelen", "Indian/Mahe", "Indian/Maldives", "Indian/Mauritius", "Indian/Mayotte", "Indian/Reunion", "Iran", "Israel", "Jamaica", "Japan", "Kwajalein", "Libya", "MET", "MST", "MST7MDT", "Mexico/BajaNorte", "Mexico/BajaSur", "Mexico/General", "NZ", "NZ-CHAT", "Navajo", "PRC", "PST8PDT", "Pacific/Apia", "Pacific/Auckland", "Pacific/Bougainville", "Pacific/Chatham", "Pacific/Chuuk", "Pacific/Easter", "Pacific/Efate", "Pacific/Enderbury", "Pacific/Fakaofo", "Pacific/Fiji", "Pacific/Funafuti", "Pacific/Galapagos", "Pacific/Gambier", "Pacific/Guadalcanal", "Pacific/Guam", "Pacific/Honolulu", "Pacific/Johnston", "Pacific/Kanton", "Pacific/Kiritimati", "Pacific/Kosrae", "Pacific/Kwajalein", "Pacific/Majuro", "Pacific/Marquesas", "Pacific/Midway", "Pacific/Nauru", "Pacific/Niue", "Pacific/Norfolk", "Pacific/Noumea", "Pacific/Pago_Pago", "Pacific/Palau", "Pacific/Pitcairn", "Pacific/Pohnpei", "Pacific/Ponape", "Pacific/Port_Moresby", "Pacific/Rarotonga", "Pacific/Saipan", "Pacific/Samoa", "Pacific/Tahiti", "Pacific/Tarawa", "Pacific/Tongatapu", "Pacific/Truk", "Pacific/Wake", "Pacific/Wallis", "Pacific/Yap", "Poland", "Portugal", "ROC", "ROK", "Singapore", "Turkey", "UCT", "US/Alaska", "US/Aleutian", "US/Arizona", "US/Central", "US/East-Indiana", "US/Eastern", "US/Hawaii", "US/Indiana-Starke", "US/Michigan", "US/Mountain", "US/Pacific", "US/Samoa", "UTC", "Universal", "W-SU", "WET", "Zulu" });
            comboBox4.Location = new System.Drawing.Point(219, 40);
            comboBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new System.Drawing.Size(139, 28);
            comboBox4.TabIndex = 21;
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            // 
            // textBox5
            // 
            textBox5.Location = new System.Drawing.Point(101, 10);
            textBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new System.Drawing.Size(215, 27);
            textBox5.TabIndex = 22;
            textBox5.TextChanged += textBox5_TextChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(textBox4);
            panel2.Controls.Add(comboBox5);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label6);
            panel2.Location = new System.Drawing.Point(21, 378);
            panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(379, 113);
            panel2.TabIndex = 20;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(327, 22);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(28, 29);
            button3.TabIndex = 22;
            button3.Text = "...";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_1;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(323, 66);
            button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(39, 39);
            button1.TabIndex = 16;
            button1.Text = "🎄";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox4
            // 
            textBox4.Location = new System.Drawing.Point(84, 72);
            textBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(231, 27);
            textBox4.TabIndex = 12;
            textBox4.Text = "/data/name,/data/start,/data/end";
            textBox4.TextChanged += textBox4_TextChanged;
            // 
            // comboBox5
            // 
            comboBox5.FormattingEnabled = true;
            comboBox5.ImeMode = System.Windows.Forms.ImeMode.On;
            comboBox5.Items.AddRange(new object[] { "JSONサンプル{} https://script.google.com/macros/s/AKfycbw27NVH4JZWekqFZIXiNsZ08e3S2MxL6sFW01rZpBFLIVEIK1VAPkiMffpCE46r6ZRL/exec", "JSONサンプル[] https://script.google.com/macros/s/AKfycbyW31ZFRACSjXccnna6ZfYOJaIZj1LMJa-fg9tKVmfW01acsc2sJP2HR6CJa7rpt2xA/exec", "旧ぐぐるのパラメーター対応版()このあぷりのでふぉ　https://script.google.com/macros/s/AKfycbxiN0USvNN0hQyO5b3Ep_oJy_qQxCRAlT4NU954QXKYZ6GrGyzsBnhi8RgMHLZHct-QJg/exec?game=all", "テストケース多重ねすと　https://script.google.com/macros/s/AKfycbwhsv6LlhTmFM0CIc7vlXrNhMMQ9II23HxuTIfWYUSTyNRewsOJAm-des6xI2uhWYKN/exec", "さしゅうあいまも含んだやつ()これだけあった()　https://script.google.com/macros/s/AKfycbyVRqHhnG40A5RSAyIe9nX4kypbQ4_67hqXHCErEnKv3ZwoycmtUo-492jfq3Hy4rCxEw/exec", "...でのファイル読み込みいちおうローカルファイルも対応している" });
            comboBox5.Location = new System.Drawing.Point(82, 20);
            comboBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBox5.Name = "comboBox5";
            comboBox5.Size = new System.Drawing.Size(233, 28);
            comboBox5.TabIndex = 21;
            comboBox5.SelectedIndexChanged += comboBox5_SelectedIndexChanged;
            comboBox5.TextChanged += comboBox5_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(4, 28);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(70, 20);
            label7.TabIndex = 14;
            label7.Text = "かすたむJS";
            label7.Click += label6_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(4, 79);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(72, 20);
            label6.TabIndex = 14;
            label6.Text = "パース対象";
            label6.Click += label6_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(21, 354);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(98, 20);
            label4.TabIndex = 10;
            label4.Text = "進捗バーの長さ";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            comboBox3.Items.AddRange(new object[] { "390", "145", "130", "97", "78" });
            comboBox3.Location = new System.Drawing.Point(127, 346);
            comboBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new System.Drawing.Size(97, 28);
            comboBox3.TabIndex = 9;
            comboBox3.Text = "390";
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            comboBox3.TextChanged += comboBox3_TextChanged;
            // 
            // comboBox2
            // 
            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox2.FormattingEnabled = true;
            comboBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            comboBox2.Location = new System.Drawing.Point(28, 228);
            comboBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(355, 28);
            comboBox2.TabIndex = 8;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new System.Drawing.Point(31, 203);
            checkBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new System.Drawing.Size(237, 24);
            checkBox2.TabIndex = 7;
            checkBox2.Text = "M$のタイムゾーン(夏時間修正あり)";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            comboBox1.Items.AddRange(new object[] { "(GMT-12:00)国際日付変更線西側 日付変更線標準時,M$0", "(GMT-11:00)ミッドウェー島、サモア サモア標準時,M$1", "(GMT-10:00)ハワイ ハワイ標準時,M$2", "(GMT-09:00)アラスカ アラスカ標準時,M$3", "(GMT-08:00)(米国およびカナダ) は、太平洋標準時ティファナ 太平洋標準時,M$4", "(GMT-07:00)(米国およびカナダ)、山地標準時 山地標準時,M$A", "(GMT-07:00)チワワ、ラパス、マサトラン メキシコ標準時 2,M$D", "(GMT-07:00)アリゾナ州 米国山地標準時,M$F", "(GMT-06:00)(米国およびカナダ) の中部標準時 中部標準時,M$14", "(GMT-06:00)サスカチェワン カナダ中部標準時,M$19", "(GMT-06:00)グアダラハラ、メキシコシティ、モンテレイ メキシコ山地標準時,M$1E", "(GMT-06:00)中央アメリカ 中央アメリカ標準時,M$21", "(GMT-05:00)(米国およびカナダ)、東部標準時 東部標準時,M$23", "(GMT-05:00)インディアナ (東部) 米国東部標準時,M$28", "(GMT-05:00)ボゴタ、リマ、Quito 亜北極地帯の太平洋標準時,M$2D", "(GMT-04:00)大西洋標準時 (カナダ) 大西洋標準時,M$32", "(GMT-04:00)ジョージタウン、ラパス、サン ・ ファン 亜北極地帯西部標準時,M$37", "(GMT-04:00)サンティアゴ 太平洋亜北極地帯 (標準時),M$38", "(GMT-03:30)ニューファンドランド ニューファンドランドおよびラブラドル標準時,M$3C", "(GMT-03:00)ブラジリア 南アメリカ東部標準時,M$41", "(GMT-03:00)ジョージタウン 亜北極地帯東部標準時,M$46", "(GMT-03:00)グリーンランド グリーンランド標準時,M$49", "(GMT-02:00)中部大西洋 中央大西洋標準時,M$4B", "(GMT-01:00)アゾレス諸島 アゾレス諸島標準時,M$50", "(GMT-01:00)カーボベルデ諸島 カーボベルデ標準時,M$53", "(GMT+00:00)グリニッジ標準時: ダブリン、エジンバラ、リスボン、ロンドン GMT 標準時,M$55", "(GMT+00:00)モンロビア、レイキャビク グリニッジ標準時,M$5A", "(GMT+01:00)サニーベイル, カリフォルニア州、ブラチスラバ、ブダペスト、Ljubljana、プラハ 中央ヨーロッパ標準時,M$5F", "(GMT+01:00)サラエボ、Skopje、ワルシャワ、Zagreb 中央ヨーロッパ標準時,M$64", "(GMT+01:00)ブリュッセル、コペンハーゲン、マドリッド、パリ ロマンス標準時,M$69", "(GMT+01:00)アムステルダム、ベルリン、ベルン、ローマ、ストックホルム、ウィーン 西ヨーロッパ標準時,M$6E", "(GMT+01:00)西中央アフリカ 西中央アフリカ標準時,M$71", "(GMT+02:00)ミンスク 東ヨーロッパ標準時,M$73", "(GMT+02:00)カイロ エジプト標準時,M$78", "(GMT+02:00)ヘルシンキ、キエフ、リガ、ソフィア、Tallinn、Vilnius ファイル (標準時),M$7D", "(GMT+02:00)アテネ、ブカレスト、イスタンブール GTB 標準時,M$82", "(GMT+02:00)エルサレム イスラエル標準時,M$87", "(GMT+02:00)ハラレ、プレトリア 南アフリカ標準時,M$8C", "(GMT+03:00)モスクワ、サンクト ペテルスブルグ、ボルゴグラード ロシア標準時,M$91", "(GMT+03:00)クウェート、リヤド アラブ標準時,M$96", "(GMT+03:00)ナイロビ 東アフリカ標準時,M$9B", "(GMT+03:00)バグダッド アラブ標準時,M$9E", "(GMT+03:30)テヘラン イラン標準時,M$A0", "(GMT+04:00)アブダビ、マスカット アラビア標準時,M$A5", "(GMT+04:00)バクー、トビリシ、エレバン コーカサス標準時,M$AA", "(GMT+04:30)カブール 移行アフガニスタン標準時,M$AF", "(GMT+05:00)エカテリンバーグ エカテリンバーグ標準時,M$B4", "(GMT+05:00)タシケント 西アジア標準時,M$B9", "(GMT+05:30)チェンナイ、カルカッタ、ムンバイ、ニューデリー インド標準時,M$BE", "(GMT+05:45)カトマンズ ネパール標準時,M$C1", "(GMT+06:00)アスタナ、ダッカ 中央アジア標準時,M$C3", "(GMT+06:00)スリジャヤワルダナプラコッテ スリランカ標準時,M$C8", "(GMT+06:00)アルマアトイ、ノボシビルスク 北中央アジア標準時,M$C9", "(GMT+06:30)ヤンゴン (ラングーン) ミャンマー標準時,M$CB", "(GMT+07:00)バンコク、ハノイ、ジャカルタ 東南アジア標準時,M$CD", "(GMT+07:00)クラスノヤルスク 北アジア標準時,M$CF", "(GMT+08:00)北京、重慶、ホンコン、ウルムチ 中国 (標準時),M$D2", "(GMT+08:00)クアラルンプール、シンガポール シンガポール標準時,M$D7", "(GMT+08:00)台北 台北標準時,M$DC", "(GMT+08:00)パース 西オーストラリア標準時,M$E1", "(GMT+08:00)イルクーツク、ウランバートル 北アジア東部標準時,M$E3", "(GMT+09:00)(ソウル) 韓国 (標準時),M$E6", "(GMT+09:00)大阪、札幌、東京 東京 (標準時),M$EB", "(GMT+09:00)ヤクーツク ヤクーツク標準時,M$F0", "(GMT+09:30)ダーウィン オーストラリア中央標準時,M$F5", "(GMT+09:30)アデレード 中央オーストラリア標準時,M$FA", "(GMT+10:00)キャンベラ、メルボルン、シドニー オーストラリア東部標準時,M$FF", "(GMT+10:00)ブリスベン 東オーストラリア標準時,M$104", "(GMT+10:00)ホバート タスマニア標準時,M$109", "(GMT+10:00)ウラジオ ストック ウラジオ ストック標準時,M$10E", "(GMT+10:00)グアム、ポートモレスビー 西太平洋標準時,M$113", "(GMT+11:00)マガダン、ソロモン諸島、ニューカレドニア 中央太平洋標準時,M$118", "(GMT+12:00)フィジー、カムチャツカ、マーシャル フィジー諸島標準時,M$11D", "(GMT+12:00)オークランド、ウェリントン ニュージーランド標準時,M$122", "(GMT+13:00)ヌクアロファ トンガ標準時,M$12C", "(GMT-03:00)ブエノスアイレス アゼルバイジャン標準時,M$80000040", "(GMT+02:00)コロンバス, ジョージア州 中東標準時,M$80000041", "(GMT+02:00)Amman ヨルダン標準時,M$80000042", "(GMT-06:00)グアダラハラ、メキシコシティ、モンテレー - 新規 中部標準時 (メキシコ),M$80000043", "(GMT-07:00)チワワ、ラパス、マサトラン - 新規 山地標準時 (メキシコ),M$80000044", "(GMT-08:00)ティファナ、バハカリフォルニア 太平洋標準時 (メキシコ),M$80000045", "(GMT+02:00)Windhoek ナミビア標準時,M$80000046", "(GMT+03:00)トビリシ グルジア標準時,M$80000047", "(GMT-04:00)Manaus 中央ブラジル標準時,M$80000048", "(GMT-03:00)モンテビデオ モンテビデオ標準時,M$80000049", "(GMT+04:00)エレバン アルメニア標準時,M$8000004A", "(GMT-04:30)カラカス ベネズエラ標準時,M$8000004B", "(GMT-03:00)ブエノスアイレス アルゼンチン標準時,M$8000004C", "(GMT+00:00)カサブランカ モロッコ標準時,M$8000004D", "(GMT+05:00)イスラマバード、カラチ パキスタン標準時,M$8000004E", "(GMT+04:00)ポートルイス モーリシャス標準時,M$8000004F", "(GMT+00:00)世界協定時刻 UTC,M$80000050", "(GMT-04:00)Asuncion パラグアイ標準時,M$80000051", "(GMT+12:00)Petropavlovsk Kamchatsky カムチャツカ標準時,M$80000052" });
            comboBox1.Location = new System.Drawing.Point(28, 165);
            comboBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(355, 28);
            comboBox1.TabIndex = 6;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(31, 136);
            checkBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(262, 24);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "UTC任意時間を使う(夏時間修正なし)";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
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
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(108, 80);
            textBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(244, 27);
            textBox2.TabIndex = 3;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(31, 85);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(72, 20);
            label2.TabIndex = 2;
            label2.Text = "現在時刻:";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(105, 19);
            textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(247, 27);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(439, 45);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox3.Size = new System.Drawing.Size(476, 446);
            textBox3.TabIndex = 2;
            // 
            // comboBox7
            // 
            comboBox7.FormattingEnabled = true;
            comboBox7.Items.AddRange(new object[] { "1900", "1950", "2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029", "2030", "2031", "2032", "2033", "2034", "2035", "2036", "2037", "2038" });
            comboBox7.Location = new System.Drawing.Point(541, 502);
            comboBox7.Name = "comboBox7";
            comboBox7.Size = new System.Drawing.Size(73, 28);
            comboBox7.TabIndex = 4;
            comboBox7.Text = "2000";
            comboBox7.SelectedIndexChanged += comboBox7_SelectedIndexChanged;
            // 
            // comboBox8
            // 
            comboBox8.FormattingEnabled = true;
            comboBox8.Items.AddRange(new object[] { "2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029", "2030", "2031", "2032", "2033", "2034", "2035", "2036", "2037", "2038" });
            comboBox8.Location = new System.Drawing.Point(649, 502);
            comboBox8.Name = "comboBox8";
            comboBox8.Size = new System.Drawing.Size(88, 28);
            comboBox8.TabIndex = 5;
            comboBox8.Text = "2037";
            comboBox8.SelectedIndexChanged += comboBox8_SelectedIndexChanged;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new System.Drawing.Point(437, 506);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new System.Drawing.Size(98, 24);
            checkBox4.TabIndex = 6;
            checkBox4.Text = "年フィルター";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += checkBox4_CheckedChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(620, 506);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(24, 20);
            label8.TabIndex = 7;
            label8.Text = "～";
            // 
            // textBox6
            // 
            textBox6.Location = new System.Drawing.Point(822, 501);
            textBox6.Name = "textBox6";
            textBox6.Size = new System.Drawing.Size(113, 27);
            textBox6.TabIndex = 8;
            textBox6.Text = "2024/12/11";
            textBox6.TextChanged += textBox6_TextChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(743, 505);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(73, 20);
            label9.TabIndex = 9;
            label9.Text = "日付てすと";
            // 
            // dtformat
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(957, 549);
            Controls.Add(label9);
            Controls.Add(textBox6);
            Controls.Add(label8);
            Controls.Add(checkBox4);
            Controls.Add(comboBox8);
            Controls.Add(comboBox7);
            Controls.Add(textBox3);
            Controls.Add(panel1);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "dtformat";
            Text = "datetimeformat";
            FormClosed += dtformat_FormClosed;
            Load += Form3_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.ComboBox comboBox8;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox custom_local;
    }
}