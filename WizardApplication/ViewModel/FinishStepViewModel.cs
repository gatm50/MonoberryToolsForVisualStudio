using System.Text;
using OreoMvvm.Wizard.ViewModels;
using WizardApplication.Model;

namespace WizardApplication.ViewModel
{
    sealed class FinishStepViewModel : WizardStepViewModelBase<MonoberryProjectObject>
    {
        public string Device { get; set; }
        public string SDK { get; set; }
        public string Name { get; set; }
        public string Autor { get; set; }
        public string Description { get; set; }
        public string AutorID { get; set; }
        public string Permissions { get; set; }

        public FinishStepViewModel(MonoberryProjectObject model)
            : base(model)
        {
        }

        public override void BeforeShow()
        {
            base.BeforeShow();
            this.Device = this.BusinessObject.DeviceObject.Name + " - " + this.BusinessObject.DeviceObject.IPAddress;
            this.SDK = this.BusinessObject.SdkObject.Name + " - " + this.BusinessObject.SdkObject.Path;
            this.Name = this.BusinessObject.ApplicationName;
            this.Autor = this.BusinessObject.ApplicationAutor;
            this.Description = this.BusinessObject.ApplicationDescription;
            this.AutorID = this.BusinessObject.ApplicationAutorId;

            StringBuilder builder = new StringBuilder();
            foreach (var item in this.BusinessObject.Permissions)
            {
                if (item.Value)
                    builder.AppendFormat("{0} - ", item.Key);
            }
            if (builder.Length > 0)
                this.Permissions = builder.ToString(0, builder.Length - 2);
            else
                this.Permissions = "No extra permission asigned";
        }

        public override string DisplayName
        {
            get { return "Finish"; }
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
