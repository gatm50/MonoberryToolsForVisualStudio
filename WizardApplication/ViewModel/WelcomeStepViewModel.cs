using OreoMvvm.Wizard.ViewModels;
using WizardApplication.Model;

namespace WizardApplication.ViewModel
{
    sealed class WelcomeStepViewModel : WizardStepViewModelBase<MonoberryProjectObject>
    {
        public WelcomeStepViewModel(MonoberryProjectObject model)
            : base(model)
        {
        }

        public override string DisplayName
        {
            get { return "Welcome"; }
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
