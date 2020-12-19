using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using VivaldiHistoryBrowser.Models;

namespace VivaldiHistoryBrowser.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private List<WebPage> webPages = new List<WebPage>();
        private DateTime currentDate = DateTime.Parse(DateTime.Now.ToLongDateString()); // 検索当日の０時丁度。
        private String statusBarText = "";
        private DelegateCommand<object> moveDateCommand;

        public MainWindowViewModel() {
            DatabaseHelper.SearchStartDateTime = currentDate;
            DatabaseHelper.SearchEndDateTime = currentDate.AddDays(1);

            WebPages = DatabaseHelper.getHistory();
        }

        public DBHelper DatabaseHelper { get; private set; } = new DBHelper();

        public List<WebPage> WebPages { 
            get => webPages;
            set {
                SetProperty(ref webPages, value);
                StatusBarText = $"{WebPages.Count} 件のデータを表示しています。";
            }
        }

        public String CurrentDateString {
            get => currentDate.ToString("yyyy/MM/dd");
        }

        // 現在表示中の履歴リストの情報を表示するためのプロパティ
        public String StatusBarText {
            get => statusBarText; 
            set => SetProperty(ref statusBarText, value);
        }

        public DelegateCommand<object> MoveDateCommand {
            get => moveDateCommand ?? (moveDateCommand = new DelegateCommand<object>((daysCount) => {
                currentDate = currentDate.AddDays((int.Parse((String)daysCount)));
                reloadList();

                RaisePropertyChanged(nameof(CurrentDateString));
            }));
        }

        private void reloadList() {
            DatabaseHelper.SearchStartDateTime = currentDate;
            DatabaseHelper.SearchEndDateTime = currentDate.AddDays(1);

            WebPages = DatabaseHelper.getHistory();
        }

    }
}
