using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaldiHistoryBrowser.Models {

    /// <summary>
    /// 並び替えの基準となる列を表す列挙型。
    /// </summary>
    public enum OrderByColumn {
        Date,
        VisitCount,
        URL,
        PageTitle
    }

    public class DBHelper :BindableBase{

        private String searchWord = "";

        public DateTime SearchStartDateTime { get; set; } = new DateTime(1601, 1, 1).AddHours(9);
        public DateTime SearchEndDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// ソートの基準となる列を示すプロパティ。デフォルトでは日時を基準に並び替えを行う
        /// </summary>
        public OrderByColumn OrderByColumn { get; set; } = OrderByColumn.Date;

        /// <summary>
        /// 履歴の検索時、日時を条件に含めて検索するかどうかを設定します。
        /// </summary>
        public Boolean DateTimeSearch { get; set; } = true;
        public String SearchWord {
            get => searchWord;
            set => SetProperty(ref searchWord, value);
        }

        private SQLiteExecuter Executer { get; } = new SQLiteExecuter();

        public List<WebPage> getHistory() {

            String textConditionalSentence = "";

            if (!String.IsNullOrEmpty(SearchWord)) {
                // 半角スペースで文字列を区切ってその回数だけループでクエリを追加する。
                ReadOnlyCollection<String> searchWords = new ReadOnlyCollection<String>(SearchWord.Split(' '));

                for(var i = 0; i < searchWords.Count; i++) {
                    textConditionalSentence += $"AND (ut.title LIKE '%{searchWords[i]}%' OR ut.url LIKE '%{searchWords[i]}%' )";
                }
            }

            String dateTimeConditionalSentence = "";
            if (DateTimeSearch) {
                dateTimeConditionalSentence = 
                $"AND vt.visit_time > {toChromeHistoryDateTimeNumber(SearchStartDateTime)} " +
                $"AND vt.visit_time < {toChromeHistoryDateTimeNumber(SearchEndDateTime)} ";
            }

            var hashs = Executer.select(
                "SELECT vt.url, vt.visit_time, ut.url, ut.title, ut.visit_count " +
                "FROM visits as vt inner join urls as ut " +
                "on vt.url = ut.id " +
                "WHERE 1=1 " +
                dateTimeConditionalSentence +
                textConditionalSentence +
                $"ORDER BY {toColumnName(OrderByColumn)} " +
                "LIMIT 500;"
                );

            var pages = new List<WebPage>();

            // 履歴の日時は 1601,1,1 からの経過マイクロ秒数で表されている。
            // 更に 9時間を足しているのは（多分）タイムゾーンによるズレがあり、訪問時刻がピッタリ9時間ずれているため。
            var startDateTime = new DateTime(1601, 1, 1).AddHours(9);

            hashs.ForEach(h => {
                var p = new WebPage() {
                    PageTitle = (String)h["title"],
                    URL = (String)h["url"],
                    VisitCount = (long)h["visit_count"]
                };

                long microSecVisiTime = (long)h["visit_time"];
                var elapsedTicks = new TimeSpan(microSecVisiTime * 10);
                p.VisitDateTime = startDateTime + elapsedTicks;

                pages.Add(p);
            });

            return pages;
        }

        /// <summary>
        /// 入力された日時を、chrome用履歴ファイルの日時を表す数値に変換します。
        /// </summary>
        /// <param name="dt">変換したい日時を入力します。
        /// chromeの日時を表す数値は 1601年1月1日(９時)が基準となるため、それより前の日時は例外を投げます。</param>
        /// <returns></returns>
        private long toChromeHistoryDateTimeNumber(DateTime dt) {
            var startDateTime = new DateTime(1601, 1, 1).AddHours(9);
            var elapsedTimeSpan = dt - startDateTime;
            if(elapsedTimeSpan.Ticks < 0) {
                throw new ArgumentException("1601年1月1日以前の数値は無効です");
            }

            return elapsedTimeSpan.Ticks / 10;
        }

        private String toColumnName(OrderByColumn columnName) {
            switch (columnName) {
                case OrderByColumn.Date : 
                    return "vt.visit_time";

                case OrderByColumn.PageTitle:
                    return "ut.title";

                case OrderByColumn.URL:
                    return "ut.url";

                case OrderByColumn.VisitCount:
                    return "ut.visit_count";

            }

            // enum側を変更した場合、こちらの変更忘れを防止するための一文。
            // 両方一緒に変更していればこの行に到達しない。
            throw new ArgumentException("不正な引数。");
        }
    }
}
