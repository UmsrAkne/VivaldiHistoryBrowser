﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaldiHistoryBrowser.Models {
    public class DBHelper {

        public DateTime SearchStartDateTime { get; set; } = new DateTime(1601, 1, 1).AddHours(9);
        public DateTime SearchEndDateTime { get; set; } = DateTime.Now;

        private SQLiteExecuter Executer { get; } = new SQLiteExecuter();

        public List<WebPage> getHistory() {
            var hashs = Executer.select(
                "SELECT vt.url, vt.visit_time, ut.url, ut.title, ut.visit_count " +
                "FROM visits as vt inner join urls as ut " +
                "on vt.url = ut.id " +
                "WHERE 1=1 " +
                $"AND vt.visit_time > {toChromeHistoryDateTimeNumber(SearchStartDateTime)} " +
                $"AND vt.visit_time < {toChromeHistoryDateTimeNumber(SearchEndDateTime)} " +
                "LIMIT 200;"
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
    }
}
