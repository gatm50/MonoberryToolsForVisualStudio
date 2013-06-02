using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Xml;
using OreoMvvm.Command;
using OreoMvvm.Wizard.ViewModels;
using WizardApplication.Model;
using WizardApplication.Utils;

namespace WizardApplication.ViewModel
{
    sealed class DeviceSetupViewModel : WizardStepViewModelBase<MonoberryProjectObject>
    {
        private List<Device> _devices;
        public ObservableCollection<Device> Devices { get; set; }

        private string _ipAddress;
        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; this.NotifyPropertyChanged(() => this.IPAddress); this.AddDeviceCommand.RaiseCanExecuteChanged(); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; this.NotifyPropertyChanged(() => this.Name); this.AddDeviceCommand.RaiseCanExecuteChanged(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; this.NotifyPropertyChanged(() => this.ErrorMessage); }
        }

        private Device _selectedDevice;
        public Device SelectedDevice
        {
            get { return _selectedDevice; }
            set { _selectedDevice = value; this.NotifyPropertyChanged(() => this.SelectedDevice); this.RemoveDeviceCommand.RaiseCanExecuteChanged(); _hasValidationErrors = false; }
        }

        private bool _hasValidationErrors;

        public DeviceSetupViewModel(MonoberryProjectObject model)
            : base(model)
        {
            this.SearchDeviceCommand = new DelegateCommand(this.SearchDevice_Execute, this.SearchDevice_CanExecute);
            this.AddDeviceCommand = new DelegateCommand(this.AddDevice_Execute, this.AddDevice_CanExecute);
            this.RemoveDeviceCommand = new DelegateCommand(this.RemoveDevice_Execute, this.RemoveDevice_CanExecute);

            this.Devices = new ObservableCollection<Device>();
            _devices = new List<Device>();

            this.LoadDevices();
            this.UpdateDevices();
        }

        private void LoadDevices()
        {
            _devices = ConfigFileManager.GetDevices();
        }

        private void UpdateDevices()
        {
            this.Devices.Clear();

            foreach (var sdk in _devices)
                this.Devices.Add(sdk);

            this.RemoveDeviceCommand.RaiseCanExecuteChanged();
        }

        public override string DisplayName
        {
            get { return "Device Setup"; }
        }

        public override bool IsValid()
        {
            if (_hasValidationErrors)
                return false;

            if (this.SelectedDevice == null)
            {
                this.ErrorMessage = "Select a Device";
                return false;
            }
            else
                this.ErrorMessage = string.Empty;
            return true;
        }

        private bool ValidateIPAddress(string ipAddress)
        {
            System.Net.IPAddress address;
            if (System.Net.IPAddress.TryParse(ipAddress, out address))
                return false;

            this.ErrorMessage = "Use a valid IP Address";
            return true;
        }

        public override bool RunOnNextAsyncOperations()
        {
            if (this.ValidateIPAddress(this.SelectedDevice.IPAddress))
            {
                _hasValidationErrors = true;
                return true;
            }

            _hasValidationErrors = false;
            return false;
        }

        public override OreoMvvm.Wizard.RouteModifier OnNext()
        {
            this.BusinessObject.DeviceObject.IPAddress = this.SelectedDevice.IPAddress;
            this.BusinessObject.DeviceObject.Name = this.SelectedDevice.Name;
            return base.OnNext();
        }

        #region Add SDK Command
        public DelegateCommand AddDeviceCommand
        {
            get;
            private set;
        }

        void AddDevice_Execute(object parameters)
        {
            if (this.ValidateIPAddress(this.IPAddress))
            {
                _hasValidationErrors = true;
                return;
            }

            _hasValidationErrors = false;
            this.Devices.Add(new Device() { Name = this.Name, IPAddress = this.IPAddress });
            ConfigFileManager.AddDevice(this.IPAddress, this.Name);
            this.Name = string.Empty;
            this.IPAddress = string.Empty;
        }

        bool AddDevice_CanExecute(object parameters)
        {
            if (string.IsNullOrEmpty(this.IPAddress) || string.IsNullOrEmpty(this.Name))
                return false;
            return true;
        }
        #endregion

        #region Remove Device Command
        public DelegateCommand RemoveDeviceCommand { get; private set; }
        void RemoveDevice_Execute(object parameters)
        {
            this.DeleteDeviceEntryFromConfigFile(this.SelectedDevice);
            _devices.Remove(this.SelectedDevice);
            this.UpdateDevices();
        }

        bool RemoveDevice_CanExecute(object parameters)
        {
            if (this.SelectedDevice == null)
                return false;
            return true;
        }

        private void DeleteDeviceEntryFromConfigFile(Device device)
        {
            string filePath = ConfigFileManager.GetConfigurationFilePath();

            XmlDocument doc = ConfigFileManager.LoadConfigFile(filePath);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes[1].ChildNodes)
            {
                //It is necessary an unique id for serves in the XML file.
                var deviceNode = Device.GetDeviceFromXmlNode(node);
                if (device.IPAddress == deviceNode.IPAddress)
                {
                    doc.DocumentElement.ChildNodes[1].RemoveChild(node);
                    doc.Save(filePath);
                    break;
                }
            }
        }
        #endregion

        #region Search Device Command
        public DelegateCommand SearchDeviceCommand
        {
            get;
            private set;
        }

        void SearchDevice_Execute(object parameters)
        {
            foreach (NetworkInterface Interface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (Interface.SupportsMulticast)
                {
                    IPInterfaceProperties IPProperties = Interface.GetIPProperties();
                    foreach (IPAddressInformation address in IPProperties.UnicastAddresses)
                    {
                        // The following information is not useful for loopback adapters. 
                        if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                            continue;

                        Console.WriteLine(Interface.Name);
                        if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            this.DiscoverDevices(address);
                    }
                }
            }
        }

        private void DiscoverDevices(IPAddressInformation address)
        {
            Ping pingSender = new Ping();
            string[] val = address.Address.ToString().Split('.');
            List<string> devices = new List<string>();
            for (int i = 1; i < 255; i++)
            {
                string currentIpToPing = string.Format("{0}.{1}.{2}.{3}", val[0], val[1], val[2], i);
                if (pingSender.Send(currentIpToPing, 250).Status == IPStatus.Success)
                    devices.Add(currentIpToPing);
            }
            Console.WriteLine("Items: {0}", devices.Count);
        }

        bool SearchDevice_CanExecute(object parameters)
        {
            return true;
        }
        #endregion

    }
}
