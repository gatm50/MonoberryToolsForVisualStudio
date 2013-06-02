using System.Windows;

namespace WizardApplication.Utils
{
    class ModalDialogService : IModalDialogService
    {
        public bool Show(Window view)
        {
            view.ShowDialog();
            return true;
        }
    }
}
