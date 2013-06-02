using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TemplateWizard;

namespace MonoberryWizard
{
    public class Wizard : IWizard
    {
        private WizardApplication.WizardWindow _wizardWindow;

        public Wizard()
        {
            _wizardWindow = new WizardApplication.WizardWindow();
        }

        #region IWizardImplementation
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
            Debug.WriteLine("Called BeforeOpeningFile");
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
            string dir = Path.GetDirectoryName(project.FullName);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(dir + "\\monoberry-descriptor.xml");

            XmlNode qnxNode = xDoc.LastChild;
            XmlNode lastChild = qnxNode.LastChild;

            foreach (var item in _wizardWindow.BusinessObject.Permissions)
            {
                if (item.Value)
                {
                    XmlElement newElement = xDoc.CreateElement("permission", xDoc.DocumentElement.NamespaceURI);
                    newElement.InnerText = item.Key;

                    qnxNode.AppendChild(newElement);
                }
            }

            xDoc.Save(dir + "\\monoberry-descriptor.xml");
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
            Debug.WriteLine("Called ProjectItemFinishedGenerating");
        }

        public void RunFinished()
        {
            Debug.WriteLine("Called RunFinished");
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _wizardWindow.ShowDialog();
            replacementsDictionary.Add("$deviceIpAddress$", _wizardWindow.BusinessObject.DeviceObject.IPAddress);
            replacementsDictionary.Add("$sdkPath$", _wizardWindow.BusinessObject.SdkObject.Path);
            replacementsDictionary.Add("$bundleName$", _wizardWindow.BusinessObject.ApplicationName);
            replacementsDictionary.Add("$bundleDescription$", _wizardWindow.BusinessObject.ApplicationDescription);
            replacementsDictionary.Add("$bundleAuthor$", _wizardWindow.BusinessObject.ApplicationAutor);
            replacementsDictionary.Add("$bundleAuthorId$", _wizardWindow.BusinessObject.ApplicationAutorId);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
        #endregion
    }
}
