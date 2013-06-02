using System.Xml;

namespace WizardApplication.Model
{
    public sealed class Device
    {
        public string IPAddress { get; set; }
        public string Name { get; set; }

        public static Device GetDeviceFromXmlNode(XmlNode node)
        {
            var device = new Device()
            {
                IPAddress = node.Attributes["IPAddress"].Value,
                Name = node.Attributes["Name"].Value
            };

            return device;
        }
    }
}
