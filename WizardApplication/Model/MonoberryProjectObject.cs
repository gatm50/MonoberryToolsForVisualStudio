using System.Collections.Generic;
using OreoMvvm.Wizard;

namespace WizardApplication.Model
{
    public sealed class MonoberryProjectObject : IWizardBusinessObject
    {
        public SDK SdkObject { get; set; }
        public Device DeviceObject { get; set; }

        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }
        public string ApplicationAutor { get; set; }
        public string ApplicationAutorId { get; set; }
        public Dictionary<string, bool> Permissions { get; set; }

        public void Cancel()
        {
        }

        public void Dispose()
        {
        }

        public MonoberryProjectObject()
        {
            this.SdkObject = new SDK();
            this.DeviceObject = new Device();
            this.Permissions = new Dictionary<string, bool>();
        }
    }
}
