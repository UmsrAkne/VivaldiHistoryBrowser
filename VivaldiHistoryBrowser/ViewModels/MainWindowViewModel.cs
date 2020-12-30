using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using VivaldiHistoryBrowser.Models;
using VivaldiHistoryBrowser.Views;

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
        private WebPage selectedItem;
        private DelegateCommand<object> moveDateCommand;
        private DelegateCommand showConfirmationDialogCommand;
        private DelegateCommand pageTitleSearchCommand;
        private DelegateCommand<String> setOrderByColumnNameCommand;
        private DelegateCommand openInBroserCommand;
        private IDialogService dialogService;

        public MainWindowViewModel(IDialogService dialogService) {
            this.dialogService = dialogService;
            DatabaseHelper.SearchStartDateTime = currentDate;
            DatabaseHelper.SearchEndDateTime = currentDate.AddDays(1);

            WebPages = DatabaseHelper.getHistory();
        }

        public DBHelper DatabaseHelper { get; private set; } = new DBHelper();

        public List<WebPage> WebPages { 
            get => webPages;
            set {
                SetProperty(ref webPages, value);
                updateStatusBarText();
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

        public WebPage SelectedItem {
            get => selectedItem;
            set {
                SetProperty(ref selectedItem, value);
                updateStatusBarText();
            }
        }

        public DelegateCommand<object> MoveDateCommand {
            get => moveDateCommand ?? (moveDateCommand = new DelegateCommand<object>((daysCount) => {
                currentDate = currentDate.AddDays((int.Parse((String)daysCount)));
                DatabaseHelper.SearchWord = "";
                reloadList();

                RaisePropertyChanged(nameof(CurrentDateString));
            }));
        }

        public DelegateCommand ShowConfirmationDialogCommand {
            get => showConfirmationDialogCommand ?? (showConfirmationDialogCommand = new DelegateCommand(() => {
                dialogService.ShowDialog(nameof(ConfirmationDialog), new DialogParameters(),
                    (IDialogResult result) => {
                        if(result?.Result == ButtonResult.Yes) {
                            HistoryFileGetter.CopyHistoryFile();
                        }
                    }
                );
            }));
        }

        public DelegateCommand PageTitleSearchCommand {
            get => pageTitleSearchCommand ?? (pageTitleSearchCommand = new DelegateCommand(() => {
                reloadList();
            }));
        }

        public DelegateCommand<String> SetOrderByColumnNameCommand{
            get => setOrderByColumnNameCommand ?? (setOrderByColumnNameCommand = new DelegateCommand<String>((colName) => {
                Enum.TryParse(colName, out OrderByColumn c);
                DatabaseHelper.OrderByColumn = c;
                reloadList();
            }));
        }


        public DelegateCommand OpenInBrowserCommand {
            get => openInBroserCommand ?? (openInBroserCommand = new DelegateCommand(() => {
                if(selectedItem != null) {
                    System.Diagnostics.Process.Start(SelectedItem.URL);
                }
            }));
        }

        private void reloadList() {
            DatabaseHelper.SearchStartDateTime = currentDate;
            DatabaseHelper.SearchEndDateTime = currentDate.AddDays(1);

            WebPages = DatabaseHelper.getHistory();
        }

        private void updateStatusBarText() {
            StatusBarText = $"{WebPages.Count} 件のデータを表示しています。";
            StatusBarText += SelectedItem != null ? $"\"{SelectedItem.PageTitle}\" ({SelectedItem.VisitDateTimeShortString}) を選択中" : "";
        }

    }
}
