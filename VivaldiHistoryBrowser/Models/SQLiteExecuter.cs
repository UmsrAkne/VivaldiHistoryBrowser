using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaldiHistoryBrowser.Models {
    public class SQLiteExecuter {

        //-------------------------------------------------- 
        // フィールド

        private SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder() {
            DataSource = "History"
        };

        //-------------------------------------------------- 
        // コンストラクタ

        public SQLiteExecuter() {
        }

        //-------------------------------------------------- 
        // プロパティ

        private SQLiteConnection Connection {
            get => new SQLiteConnection(builder.ToString());
        }

        //-------------------------------------------------- 
        // メソッド

        public List<Hashtable> select(string commandText) {
            using (var con = Connection) {
                List<Hashtable> resultList = new List<Hashtable>();
                con.Open();
                var command = new SQLiteCommand(commandText, con);
                var dataReader = command.ExecuteReader();

                while (dataReader.Read()) {
                    var hashtable = new Hashtable();
                    for (int i = 0; i < dataReader.FieldCount; i++) {
                        hashtable[dataReader.GetName(i)] = dataReader.GetValue(i);
                    }
                    resultList.Add(hashtable);
                }

                return resultList;
            };
        }

    }
}
