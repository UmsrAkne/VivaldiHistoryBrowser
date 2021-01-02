using VivaldiHistoryBrowser.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using VivaldiHistoryBrowser.ViewModels;
using System.Threading;

namespace VivaldiHistoryBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<ConfirmationDialog, ConfirmationDialogViewModel>();

        }
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            // mutex を使用してアプリの多重起動をチェック
            App.mutex = new Mutex(false, "vivaldiHistoryBrowser_id");
            if (!App.mutex.WaitOne(0, false)) {
                App.mutex.Close();
                App.mutex = null;
                this.Shutdown();
                return;
            }
        }

        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);

            App.mutex.ReleaseMutex();
            App.mutex.Close();
            App.mutex = null;
        }

        private static Mutex mutex;
    }
}
