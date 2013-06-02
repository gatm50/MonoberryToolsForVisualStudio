using System.Collections.Generic;
using System.Collections.ObjectModel;
using OreoMvvm.Command;
using OreoMvvm.Wizard.ViewModels;
using WizardApplication.Model;
using WizardApplication.Utils;
using WizardApplication.View;

namespace WizardApplication.ViewModel
{
    sealed class SDKConfigurationViewModel : WizardStepViewModelBase<MonoberryProjectObject>
    {
        public ObservableCollection<SDK> SDKs { get; set; }
        private List<SDK> _sdks;

        private readonly IModalDialogService _messageBoxService;

        private SDK _currentSDK;
        public SDK CurrentSDK
        {
            get { return _currentSDK; }
            set { _currentSDK = value; this.NotifyPropertyChanged(() => this.CurrentSDK); }
        }

        public SDKConfigurationViewModel(MonoberryProjectObject model)
            : base(model)
        {
            _messageBoxService = new ModalDialogService();
            this.ManageSDKCommand = new DelegateCommand(this.ManageSDK_Execute, this.ManageSDK_CanExecute);
            this.SDKs = new ObservableCollection<SDK>();
            _sdks = new List<SDK>();

            this.LoadSDKs();
            this.UpdateSDKs();
        }

        public override string DisplayName
        {
            get { return "SDK Configuration"; }
        }

        public override bool IsValid()
        {
            if (this.CurrentSDK == null)
                return false;
            return true;
        }

        #region Manage SDK Command
        public DelegateCommand ManageSDKCommand
        {
            get;
            private set;
        }

        void ManageSDK_Execute(object parameters)
        {
            SDKsView view = new SDKsView();
            _messageBoxService.Show(view);
            this.LoadSDKs();
            this.UpdateSDKs();
        }

        bool ManageSDK_CanExecute(object parameters)
        {
            return true;
        }
        #endregion

        public override OreoMvvm.Wizard.RouteModifier OnNext()
        {
            this.BusinessObject.SdkObject.Path = this.CurrentSDK.Path;
            this.BusinessObject.SdkObject.Name = this.CurrentSDK.Name;
            return base.OnNext();
        }

        private void LoadSDKs()
        {
            _sdks = ConfigFileManager.GetSDKs();
        }

        private void UpdateSDKs()
        {
            this.SDKs.Clear();

            foreach (var sdk in _sdks)
                this.SDKs.Add(sdk);
        }
    }
}
