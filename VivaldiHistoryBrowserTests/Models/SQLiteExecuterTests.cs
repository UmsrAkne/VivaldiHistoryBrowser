using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivaldiHistoryBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaldiHistoryBrowser.Models.Tests {
    [TestClass()]
    public class SQLiteExecuterTests {
        [TestMethod()]
        public void selectTest() {
            var s = new SQLiteExecuter();
            var list = s.select("select * from visits limit 10;");
            Assert.Fail();
        }
    }
}