﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace neta.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("neta.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   data:image/jpeg;base64, に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string base64jpg {
            get {
                return ResourceManager.GetString("base64jpg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   @-moz-document url-prefix(&quot;https://claude.ai/chat/&quot;) {
        ///   [data-theme=claude],
        ///   [data-theme=claude][data-mode=light] {
        ///
        ///       --bg-000: #62285442;
        ///       --bg-100: #ad1f6442;
        ///       --bg-200: #62284b42;
        ///       --bg-300: #62286042;
        ///       --bg-400: #62286242;
        ///       --bg-500: #62285842;
        ///   }
        ///   .font-user-message {
        ///
        ///      background-color: #e1bf5be8;
        ///   }
        ///   .font-claude-message {
        ///       background-color: #f0a15be6;
        ///   }
        ///   .code-block__code {
        ///       opacity: 0.7;
        ///   }
        ///
        ///   .overflow- [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string claude {
            get {
                return ResourceManager.GetString("claude", resourceCulture);
            }
        }
        
        /// <summary>
        ///   @-moz-document url-prefix(&quot;https://gemini.google.com/&quot;) {
        ////*‼️次で始まるURL https://gemini.google.com/  */
        ///:where(.theme-host) {  /* デフォの白背景削除 */
        /// --gem-sys-color--surface: transparent, !important; 
        ///}
        ///
        ///body {  /*上のバナー 明るい色*/
        ///  background-color: rgba(255, 255, 255, 0.562);
        ///}
        ///.main-content{  /*子を透過する */
        ///  background-color: rgba(255, 255, 255, 0.562);
        ///}
        ///
        ///.chat-app {
        ///  /*opacity: 0.3;  全体を透過 */
        ///  /*backdrop-filter: blur(5px); */ /* 背景をぼかす (adjust the value as needed) */
        ///  background-color:transparent [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string gemini {
            get {
                return ResourceManager.GetString("gemini", resourceCulture);
            }
        }
        
        /// <summary>
        ///   BEGIN:VCALENDAR
        ///VERSION:2.0
        ///PRODID:-//はんようたいまー//NONSGML v1.0//EN
        ///BEGIN:VEVENT
        ///DTSTART:20200423T150000Z
        ///DTEND:20200424T150000Z
        ///SUMMARY:うづき
        ///END:VEVENT
        ///END:VCALENDAR に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ical {
            get {
                return ResourceManager.GetString("ical", resourceCulture);
            }
        }
        
        /// <summary>
        ///   型 System.Drawing.Bitmap のローカライズされたリソースを検索します。
        /// </summary>
        internal static System.Drawing.Bitmap syougtu_kurinuki {
            get {
                object obj = ResourceManager.GetObject("syougtu_kurinuki", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
