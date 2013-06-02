using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using OreoMvvm.Wizard.ViewModels;
using WizardApplication.Model;

namespace WizardApplication.ViewModel
{
    sealed class ApplicationInfoViewModel : WizardStepViewModelBase<MonoberryProjectObject>
    {
        private WizardStepViewModelBase<MonoberryProjectObject> _parentViewModel;

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; this.NotifyPropertyChanged(() => this.Name); _parentViewModel.IsValid(); }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; this.NotifyPropertyChanged(() => this.Description); }
        }
        private string _autor;
        public string Autor
        {
            get { return _autor; }
            set { _autor = value; this.NotifyPropertyChanged(() => this.Autor); _parentViewModel.IsValid(); }
        }
        private string _autorId;
        public string AutorId
        {
            get { return _autorId; }
            set { _autorId = value; this.NotifyPropertyChanged(() => this.AutorId); _parentViewModel.IsValid(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; this.NotifyPropertyChanged(() => this.ErrorMessage); }
        }

        private bool _accessShared;
        public bool AccessShared
        {
            get { return _accessShared; }
            set { _accessShared = value; this.NotifyPropertyChanged(() => this.AccessShared); }
        }
        private bool _recordAudio;
        public bool RecordAudio
        {
            get { return _recordAudio; }
            set { _recordAudio = value; this.NotifyPropertyChanged(() => this.RecordAudio); }
        }
        private bool _readGeolocation;
        public bool ReadGeolocation
        {
            get { return _readGeolocation; }
            set { _readGeolocation = value; this.NotifyPropertyChanged(() => this.ReadGeolocation); }
        }
        private bool _useCamera;
        public bool UseCamera
        {
            get { return _useCamera; }
            set { _useCamera = value; this.NotifyPropertyChanged(() => this.UseCamera); }
        }
        private bool _accessInternet;
        public bool AccessInternet
        {
            get { return _accessInternet; }
            set { _accessInternet = value; this.NotifyPropertyChanged(() => this.AccessInternet); }
        }
        private bool _playAudio;
        public bool PlayAudio
        {
            get { return _playAudio; }
            set { _playAudio = value; this.NotifyPropertyChanged(() => this.PlayAudio); }
        }
        private bool _postNotification;
        public bool PostNotification
        {
            get { return _postNotification; }
            set { _postNotification = value; this.NotifyPropertyChanged(() => this.PostNotification); }
        }
        private bool _setAudioVolume;
        public bool SetAudioVolume
        {
            get { return _setAudioVolume; }
            set { _setAudioVolume = value; this.NotifyPropertyChanged(() => this.SetAudioVolume); }
        }
        private bool _readDeviceIdentifyingInformation;
        public bool ReadDeviceIdentifyingInformation
        {
            get { return _readDeviceIdentifyingInformation; }
            set { _readDeviceIdentifyingInformation = value; this.NotifyPropertyChanged(() => this.ReadDeviceIdentifyingInformation); }
        }
        private bool _accessLedControl;
        public bool AccessLedControl
        {
            get { return _accessLedControl; }
            set { _accessLedControl = value; this.NotifyPropertyChanged(() => this.AccessLedControl); }
        }

        public ApplicationInfoViewModel(MonoberryProjectObject model)
            : base(model)
        {
            _parentViewModel = (this.ParentViewModel as WizardStepViewModelBase<MonoberryProjectObject>);
        }

        public override string DisplayName
        {
            get { return "Application Information"; }
        }

        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(this.Name) ||
                string.IsNullOrEmpty(this.Autor) ||
                string.IsNullOrEmpty(this.AutorId))
            {
                this.ErrorMessage = "Name, Autor and AuthorID are mandatory fields";
                return false;
            }
            else
                this.ErrorMessage = string.Empty;

            return true;
        }

        public override OreoMvvm.Wizard.RouteModifier OnNext()
        {
            this.BusinessObject.ApplicationName = this.Name;
            this.BusinessObject.ApplicationDescription = this.Description;
            this.BusinessObject.ApplicationAutor = this.Autor;
            this.BusinessObject.ApplicationAutorId = this.AutorId;

            this.BusinessObject.Permissions.Clear();

            this.BusinessObject.Permissions.Add(this.GetPropertyName(() => this.AccessShared), this.AccessShared);
            this.BusinessObject.Permissions.Add(this.GetPropertyName(() => this.RecordAudio), this.RecordAudio);
            this.BusinessObject.Permissions.Add(this.GetPropertyName(() => this.ReadGeolocation), this.ReadGeolocation);
            this.BusinessObject.Permissions.Add(this.GetPropertyName(() => this.UseCamera), this.UseCamera);
            this.BusinessObject.Permissions.Add(this.GetPropertyName(() => this.AccessInternet), this.AccessInternet);
            this.BusinessObject.Permissions.Add(this.GetPropertyName(() => this.PlayAudio), this.PlayAudio);
            this.BusinessObject.Permissions.Add(this.GetPropertyName(() => this.PostNotification), this.PostNotification);
            this.BusinessObject.Permissions.Add(this.GetPropertyName(() => this.SetAudioVolume), this.SetAudioVolume);
            this.BusinessObject.Permissions.Add(this.GetPropertyName(() => this.ReadDeviceIdentifyingInformation), this.ReadDeviceIdentifyingInformation);
            this.BusinessObject.Permissions.Add(this.GetPropertyName(() => this.AccessLedControl), this.AccessLedControl);

            return base.OnNext();
        }

        public string GetPropertyName(Expression<Func<object>> property)
        {
            MemberExpression memberExpression;
            if (property.Body is UnaryExpression)
                memberExpression = (property.Body as UnaryExpression).Operand as MemberExpression;
            else
                memberExpression = property.Body as MemberExpression;

            var propertyInfo = memberExpression.Member as PropertyInfo;

            StringBuilder name = new StringBuilder();
            name.Append(propertyInfo.Name[0]);

            for (int i = 1; i < propertyInfo.Name.Length; i++)
            {
                char item = propertyInfo.Name[i];

                if (char.IsUpper(item))
                {
                    name.Append('_');
                    name.Append(item);
                    continue;
                }
                name.Append(item);
            }

            return name.ToString().ToLower(CultureInfo.InvariantCulture);
        }
    }
}
