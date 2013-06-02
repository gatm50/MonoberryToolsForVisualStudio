using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;
using OreoMvvm.Wizard;
using OreoMvvm.Wizard.ViewModels;
using OreoMvvm.Wizard.Views;
using WizardApplication.Model;
using WizardApplication.View;
using WizardApplication.ViewModel;

namespace WizardApplication
{
    /// <summary>
    /// Interaction logic for WizardWindow.xaml
    /// </summary>
    public partial class WizardWindow : MetroWindow
    {
        public IWizardViewModel WizardVM
        {
            get { return (IWizardViewModel)GetValue(WizardVMProperty); }
            set { SetValue(WizardVMProperty, value); }
        }

        public static readonly DependencyProperty WizardVMProperty = DependencyProperty.Register("WizardVM", typeof(IWizardViewModel), typeof(WizardWindow));

        private readonly WizardView MonoberryProjectWizardView;
        public event PropertyChangedEventHandler PropertyChanged;

        public MonoberryProjectObject BusinessObject { get; set; }

        public WizardWindow()
        {
            InitializeComponent();
            DataContext = this;
            MonoberryProjectWizardView = GetPlainWizardView();
            this.BusinessObject = (MonoberryProjectWizardView.DataContext as WizardViewModel<MonoberryProjectObject>).BusinessObject;

            wizardHost.Children.Clear();
            wizardHost.Children.Add(MonoberryProjectWizardView);
            this.FirePropsChanged(MonoberryProjectWizardView.DataContext as IWizardViewModel);
        }

        private void FirePropsChanged(IWizardViewModel wizardViewModel)
        {
            WizardVM = wizardViewModel;
            this.OnPropertyChanged("WizardVM");
        }

        private static WizardView GetPlainWizardView()
        {
            /// 1)
            /// Create a WizardViewModel passing the type of the object the wizard will model.
            /// The type it's modeling must have parameterless constructor; WizardViewModel will create it.
            var wizModel = new WizardViewModel<MonoberryProjectObject>();

            /// 2)
            /// Create / provide the steps for the wizard.  See comments in the CreateSteps method.
            wizModel.ProvideSteps(CreateMonoberryProjectSteps(wizModel.BusinessObject));

            /// 3)
            /// Create the actual wizard view / control.  Set it's DataContext to the WizardViewModel object created above.
            return new WizardView() { Height = 400, Width = 600, DataContext = wizModel };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericModel">This is the instance created by the WizardViewModel</param>
        /// <returns></returns>
        private static List<CompleteStep<MonoberryProjectObject>> CreateMonoberryProjectSteps(MonoberryProjectObject genericModel)
        {
            /// 2.1) Create a view model for each step.
            ///     Each of these descend from WizardStepViewModelBase
            var step1ViewModel = new WelcomeStepViewModel(genericModel);

            /// This ViewModel contains a RouteOptionGroupViewModel (a group of options that may alter the workflow of the wizard).
            /// See TypeSizeStepViewModel.CreateAvailableDrinkSizes.
            var step2ViewModel = new SDKConfigurationViewModel(genericModel);
            var step3ViewModel = new DeviceSetupViewModel(genericModel);
            var step4ViewModel = new ApplicationInfoViewModel(genericModel);
            var step5ViewModel = new FinishStepViewModel(genericModel);

            /// 2.2) Create a list of steps.
            ///     We pass the same type param (CupOfCoffee) that we passed to WizardViewModel in Button_Click above.
            return new List<CompleteStep<MonoberryProjectObject>>() 
            {
                /// Each step contains a ViewModel and a View type (the type representing the actual Xaml to be shown).
                new CompleteStep<MonoberryProjectObject>() { ViewModel = step1ViewModel, ViewType = typeof(WelcomeView), Visited = true },
                new CompleteStep<MonoberryProjectObject>() { ViewModel = step2ViewModel, ViewType = typeof(SDKConfigurationView) },
                new CompleteStep<MonoberryProjectObject>() { ViewModel = step3ViewModel, ViewType = typeof(DeviceSetupView) },
                new CompleteStep<MonoberryProjectObject>() { ViewModel = step4ViewModel, ViewType = typeof(ApplicationInfoView) },
                new CompleteStep<MonoberryProjectObject>() { ViewModel = step5ViewModel, ViewType = typeof(FinishView) },
            };
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
