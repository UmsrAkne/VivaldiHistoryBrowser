using Prism.Mvvm;
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

        public MainWindowViewModel()
        {
        }

        public DBHelper DatabaseHelper { get; private set; } = new DBHelper();

        public List<WebPage> WebPages { 
            get => webPages;
            set => SetProperty(ref webPages, value); 
        }
    }
}
