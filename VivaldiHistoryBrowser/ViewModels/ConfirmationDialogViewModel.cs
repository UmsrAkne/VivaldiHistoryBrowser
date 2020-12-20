using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VivaldiHistoryBrowser.Models;

namespace VivaldiHistoryBrowser.ViewModels {
    class ConfirmationDialogViewModel : BindableBase, IDialogAware {
        public string Title => "履歴取得に関する確認";

        public event Action<IDialogResult> RequestClose;

        private String confirmationText;
        private DelegateCommand yesCommand;
        private DelegateCommand cancelCommand;

        public ConfirmationDialogViewModel() {
            confirmationText = $"{HistoryFileGetter.HistoryFilePath()} \n" +
                "上記から履歴ファイルを取得して複製します\n" +
                "既に履歴ファイルが存在していた場合、古いファイルに上書きします。\n" +
                "取得、複製を実行してもよろしいですか？\n";
        }

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

        public String ConfirmationText {
            get => confirmationText;
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() {

        }

        public void OnDialogOpened(IDialogParameters parameters) {

        }




    }
}
