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

            hashs.ForEach(h => {
                var p = new WebPage() {
                    PageTitle = (String)h["title"],
                    URL = (String)h["url["],
                };

                pages.Add(p);
            });

            return pages;
        }
    }
}
