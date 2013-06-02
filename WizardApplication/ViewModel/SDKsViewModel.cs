using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Xml;
using OreoMvvm.Command;
using OreoMvvm.ViewModel;
using WizardApplication.Model;
using WizardApplication.Utils;

namespace WizardApplication.ViewModel
{
    sealed class SDKsViewModel : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; this.NotifyPropertyChanged(() => this.Name); this.AddSDKCommand.RaiseCanExecuteChanged(); }
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; this.NotifyPropertyChanged(() => this.Path); this.AddSDKCommand.RaiseCanExecuteChanged(); }
        }

        private SDK _currentItem;
        public SDK CurrentItem
        {
            get { return _currentItem; }
            set { _currentItem = value; this.NotifyPropertyChanged(() => this.CurrentItem); this.DeleteSDKCommand.RaiseCanExecuteChanged(); }
        }

        private List<SDK> _sdks;
        public ObservableCollection<SDK> Items { get; set; }

        public SDKsViewModel()
        {
            this.AddSDKCommand = new DelegateCommand(this.AddSDK_Execute, this.AddSDK_CanExecute);
            this.DeleteSDKCommand = new DelegateCommand(this.DeleteSDK_Execute, this.DeleteSDK_CanExecute);
            this.OkSDKCommand = new DelegateCommand(this.OkSDK_Execute, this.OkSDK_CanExecute);
            this.ChooseDirectoryCommand = new DelegateCommand(this.ChooseDirectory_Execute, this.ChooseDirectory_CanExecute);

            this.Items = new ObservableCollection<SDK>();
            _sdks = new List<SDK>();

            this.LoadSDKs();
            this.UpdateSDKs();
        }

        private void LoadSDKs()
        {
            _sdks = ConfigFileManager.GetSDKs();
        }

        private void UpdateSDKs()
        {
            this.Items.Clear();

            foreach (var sdk in _sdks)
                this.Items.Add(sdk);

            this.DeleteSDKCommand.RaiseCanExecuteChanged();
        }

        #region Add SDK Command
        public DelegateCommand AddSDKCommand
        {
            get;
            private set;
        }

        void AddSDK_Execute(object parameters)
        {
            this.Items.Add(new SDK() { Name = this.Name, Path = this.Path });
            ConfigFileManager.AddSDK(this.Path, this.Name);
            this.Name = string.Empty;
            this.Path = string.Empty;
        }

        bool AddSDK_CanExecute(object parameters)
        {
            if (string.IsNullOrEmpty(this.Path) || string.IsNullOrEmpty(this.Name))
                return false;
            return true;
        }
        #endregion

        #region Delete SDK Command
        public DelegateCommand DeleteSDKCommand
        {
            get;
            private set;
        }

        void DeleteSDK_Execute(object parameters)
        {
            this.DeleteSDKEntryFromConfigFile(this.CurrentItem);
            _sdks.Remove(this.CurrentItem);
            this.UpdateSDKs();
        }

        bool DeleteSDK_CanExecute(object parameters)
        {
            if (this.CurrentItem == null)
                return false;
            return true;
        }

        private void DeleteSDKEntryFromConfigFile(SDK sdk)
        {
            string filePath = ConfigFileManager.GetConfigurationFilePath();

            XmlDocument doc = ConfigFileManager.LoadConfigFile(filePath);

            foreach (XmlNode node in doc.DocumentElement.FirstChild.ChildNodes)
            {
                //It is necessary an unique id for serves in the XML file.
                var sdkNode = SDK.GetSDKFromXmlNode(node);
                if (sdk.Path == sdkNode.Path)
                {
                    doc.DocumentElement.FirstChild.RemoveChild(node);
                    doc.Save(filePath);
                    break;
                }
            }
        }
        #endregion

        #region Ok SDK Command
        public DelegateCommand OkSDKCommand
        {
            get;
            private set;
        }

        void OkSDK_Execute(object parameters)
        {
            
        }

        bool OkSDK_CanExecute(object parameters)
        {
            return true;
        }
        #endregion

        #region Choose Directory Command
        public DelegateCommand ChooseDirectoryCommand
        {
            get;
            private set;
        }

        void ChooseDirectory_Execute(object parameters)
        {
            var dlg = new FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            this.Path = dlg.SelectedPath;
        }

        bool ChooseDirectory_CanExecute(object parameters)
        {
            return true;
        }
        #endregion
    }
}
