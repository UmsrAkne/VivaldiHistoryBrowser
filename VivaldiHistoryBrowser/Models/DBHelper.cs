using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaldiHistoryBrowser.Models {
    public class DBHelper {

        SQLiteExecuter Executer { get; } = new SQLiteExecuter();
    }
}
