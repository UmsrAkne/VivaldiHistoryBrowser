using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaldiHistoryBrowser.Models {
    class HistoryFileGetter {

        private HistoryFileGetter() { }

        public static String HistoryFilePath() {
            System.Diagnostics.Debug.WriteLine(File.Exists($@"C:\Users\{Environment.UserName}\AppData\Local\Vivaldi\User Data\Default\History"));
            return $@"C:\Users\{Environment.UserName}\AppData\Local\Vivaldi\User Data\Default\History";
        }
    }
}
