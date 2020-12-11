using Prism.Mvvm;
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

        public MainWindowViewModel()
        {

        }

        public DBHelper DatabaseHelper { get; private set; } = new DBHelper();
    }
}
