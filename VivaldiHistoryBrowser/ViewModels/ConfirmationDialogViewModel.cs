using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivaldiHistoryBrowser.ViewModels {
    class ConfirmationDialogViewModel : BindableBase, IDialogAware {
        public string Title => "履歴取得に関する確認";

        public event Action<IDialogResult> RequestClose;

        private DelegateCommand yesCommand;
        private DelegateCommand cancelCommand;

        public DelegateCommand YesCommand{
            get => yesCommand ?? (yesCommand = new DelegateCommand(() => {
                this.RequestClose?.Invoke(new DialogResult(ButtonResult.Yes));
            }));
        }

        public DelegateCommand CancelCommand {
            get => cancelCommand ?? (cancelCommand = new DelegateCommand(() => {
                this.RequestClose?.Invoke(new DialogResult(ButtonResult.No));
            }));
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() {

        }

        public void OnDialogOpened(IDialogParameters parameters) {

        }




    }
}
