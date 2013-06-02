using System.Windows;
using MahApps.Metro.Controls;
using WizardApplication.ViewModel;

namespace WizardApplication.View
{
    /// <summary>
    /// Interaction logic for SDKsView.xaml
    /// </summary>
    public partial class SDKsView : MetroWindow
    {
        public SDKsView()
        {
            InitializeComponent();
            this.DataContext = new SDKsViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
