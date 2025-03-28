//ぐろっくたん生成ドキュメント
tzzone_ZIC_neta プロジェクト概要ドキュメント
1. プロジェクトの目的
tzzone_ZIC_neta は、zic で生成された tzif バイナリデータを解析するツールであり、Android の androidtz データやレガシーの POSIXTimezone にも対応します。
さらに、ウェブ上の JSON 形式の時刻データを取得し、指定したタイムゾーンに変換する機能を提供します。タイムゾーン処理を多様な形式（tzif、MicrosoftTimezone、NodaTime など）でサポートし、実験的かつ実用的なユーティリティを目指しています。sokudon の開発スタイルを反映し、ゲームイベント管理やスケジュール調整での活用が想定されます。

2. 主な機能（何ができるか）
以下は、tzzone_ZIC_neta が提供する主要な機能です。

2.1 tzif バイナリデータの解析
概要: zic コンパイラで生成された tzif ファイルを解析。
できること:
タイムゾーン識別子、オフセット、夏時間ルールを抽出。
特定日時の UTC オフセットや夏時間適用状況を計算。
Android の androidtz データにも対応。
2.2 レガシー POSIXTimezone の解析
概要: POSIXTimezone（例: TZ=EST5EDT,M3.2.0,M11.1.0）形式のレガシータイムゾーンに対応。ただし、メニュー表示機能はなく、解析処理のみ提供。
できること:
POSIX 文字列からオフセットや夏時間ルールを解釈。
指定日時のローカル時刻を計算。
例: TZ=JST-9 を解析し、日本時間のオフセットを抽出。
2.3 ウェブ JSON 時刻データの受信と変換
概要: インターネット上の JSON 形式の時刻データを取得し、指定したタイムゾーンに変換。
できること:
外部 API（例: WorldTimeAPI）から JSON データを取得。
UTC 時刻やローカル時刻を任意のタイムゾーンに変換。
例: {"datetime": "2025-03-27T12:00:00Z"} を Asia/Tokyo で表示（21:00 JST）。
2.4 タイムゾーン表示モード
概要: 複数のタイムゾーン形式に対応した時刻表示モード。
モードとできること:

MicrosoftTimezone（固定オフセット）
Microsoft のタイムゾーン定義（例: "Pacific Standard Time"）を使用し、固定オフセットで時刻表示。
夏時間は無視。
例: PST (-08:00) で固定表示。

MicrosoftTimezone（夏時間対応）
Microsoft のタイムゾーンで夏時間ルールを適用。
例: "Eastern Standard Time" で 3月27日が EDT (-04:00) か EST (-05:00) かを判定。

NodaTime
NodaTime（.NET のタイムゾーンライブラリ）の形式で時刻を処理。
IANA タイムゾーン（例: America/New_York）に対応し、高精度な計算。
例: NodaTime で夏時間移行を正確に反映。

tzif バイナリデータ`
tzif ファイルから直接時刻を計算・表示。
Android の androidtz データも利用可能。

2.5 スケジュール計算と可視化
概要: 解析したデータをスケジュール管理や可視化に活用。
できること:
複数タイムゾーンでのイベント時間を表示。
JSON や CSV 形式で解析結果をエクスポート。
夏時間移行点のリスト化。

3. 想定されるユースケース
レガシーデータの検証
古いシステムで使われる POSIXTimezone の動作確認。
ウェブベースの時刻同期
JSON API から取得した時刻を、指定タイムゾーンで表示。
クロスプラットフォーム開発
MicrosoftTimezone や NodaTime を用いたアプリのタイムゾーン互換性テスト。
ゲームイベント管理
グローバルイベントの時刻を tzif や他のモードで調整・表示。

4. 技術的特徴
対応形式: tzif（全バージョン）、POSIXTimezone、MicrosoftTimezone、NodaTime。
入力ソース: tzif ファイル、Android androidtz、ウェブ JSON。
使用言語: C（バイナリ解析）、スクリプト言語（JSON 処理や表示）。
依存関係: NodaTime ライブラリ（オプション）、ネットワーク接続（JSON 用）。

5. 制限事項
メニュー表示なし: POSIXTimezone は解析のみで、UI 選択肢は提供されない。
生成機能なし: タイムゾーンデータの作成・編集は不可。
ネットワーク依存: JSON 時刻データ取得にはインターネット接続が必要。ローカルJSONも利用可能

6. まとめ
tzzone_ZIC_neta は、tzif 解析を基盤に、POSIXTimezone、ウェブ JSON 時刻データ、MicrosoftTimezone（固定/夏時間）、NodaTime などの多様なタイムゾーン形式に対応するツールです。
Android androidtz もサポートし、スケジュール計算やデータ変換に活用できます。実験的な要素を含むため、技術者やイベント管理者向けに柔軟な応用が可能です。

これでご要望をすべて反映しました。具体的な質問や修正点があればお知らせください！