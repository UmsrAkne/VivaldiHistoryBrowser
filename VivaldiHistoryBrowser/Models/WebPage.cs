using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaldiHistoryBrowser.Models {
    public class WebPage {
        public DateTime VisitDateTime { get; set; } = new DateTime();
        public String PageTitle { get; set; } = "";

        // System.Uri 型でもいいかと思ったけど、URLを使用して何か高度なことを行うわけではなく、
        // 今のところは文字列でURLが確認できれば充分なので、String 型で作成する。
        public String URL { get; set; } = "";

    }
}
