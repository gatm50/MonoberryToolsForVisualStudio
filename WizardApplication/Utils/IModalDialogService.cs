using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WizardApplication.Utils
{
    interface IModalDialogService
    {
        bool Show(Window view);
    }
}
