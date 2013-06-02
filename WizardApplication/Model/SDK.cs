using System.Xml;

namespace WizardApplication.Model
{
    public sealed class SDK
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public static SDK GetSDKFromXmlNode(XmlNode node)
        {
            var sdk = new SDK()
            {
                Path = node.Attributes["Path"].Value,
                Name = node.Attributes["Name"].Value
            };

            return sdk;
        }
    }
}
