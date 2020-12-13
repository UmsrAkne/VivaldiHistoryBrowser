using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivaldiHistoryBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaldiHistoryBrowser.Models.Tests {
    [TestClass()]
    public class DBHelperTests {
        [TestMethod()]
        public void getHistoryInNewOrderTest() {
            var dbHelper = new DBHelper();
            dbHelper.getHistory();
        }
    }
}