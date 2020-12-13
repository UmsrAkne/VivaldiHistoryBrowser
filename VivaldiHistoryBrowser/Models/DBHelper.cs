using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaldiHistoryBrowser.Models {
    public class DBHelper {

        SQLiteExecuter Executer { get; } = new SQLiteExecuter();

        public List<WebPage> getHistory() {
            var hashs = Executer.select(
                "SELECT vt.url, vt.visit_time, ut.url, ut.title " +
                "FROM visits as vt inner join urls as ut " +
                "on vt.url = ut.id " +
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
                };

                long microSecVisiTime = (long)h["visit_time"];
                var elapsedTicks = new TimeSpan(microSecVisiTime * 10);
                p.VisitDateTime = startDateTime + elapsedTicks;

                pages.Add(p);
            });

            return pages;
        }
    }
}
