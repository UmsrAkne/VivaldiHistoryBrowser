using VivaldiHistoryBrowser.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using VivaldiHistoryBrowser.ViewModels;

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
    }
}
