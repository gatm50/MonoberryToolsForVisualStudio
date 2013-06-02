using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using WizardApplication.Model;

namespace WizardApplication.Utils
{
    class ConfigFileManager
    {
        public const string COMPANY_NAME = "Monoberry";
        public const string CONFIGURATION_FILENAME = "Monoberry.Config";
        public const string LOG_FILENAME = "Monoberry.log";

        public static bool CreateConfigFile(string filePath)
        {
            try
            {
                // Creates an XML file is not exist 
                XmlTextWriter writer = new XmlTextWriter(filePath, null);

                // Starts a new document 
                writer.WriteStartDocument();
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;

                // Add elements to the file 
                writer.WriteStartElement("MonoberryConfig");
                writer.WriteStartElement("SDKs");
                writer.WriteEndElement();
                writer.WriteStartElement("Devices");
                writer.WriteEndElement();
                writer.WriteStartElement("Other");
                writer.WriteValue(20);
                writer.WriteEndElement();
                writer.WriteEndElement();

                // Ends the document 
                writer.WriteEndDocument();
                writer.Close();
                return true;
            }
            catch (Exception e)
            {
                Log.AddMessageLog(e.Message);
                Log.AddMessageLog(e.StackTrace);
                return false;
            }
        }

        public static XmlDocument LoadConfigFile(string filePath)
        {
            if (!File.Exists(filePath))
                ConfigFileManager.CreateConfigFile(filePath);

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            return doc;
        }

        public static void AddSDK(string sdkPath, string sdkName)
        {
            string configFilePath = ConfigFileManager.GetConfigurationFilePath();

            var xmlConfigFile = ConfigFileManager.LoadConfigFile(configFilePath);

            XmlNode xNodeServer = xmlConfigFile.CreateNode(XmlNodeType.Element, "SDK", string.Empty);

            XmlAttribute xAttributeSite = xmlConfigFile.CreateAttribute("Path");
            xAttributeSite.Value = sdkPath;
            xNodeServer.Attributes.Append(xAttributeSite);

            XmlAttribute xAttributeName = xmlConfigFile.CreateAttribute("Name");
            xAttributeName.Value = sdkName;
            xNodeServer.Attributes.Append(xAttributeName);

            xmlConfigFile.GetElementsByTagName("SDKs")[0]
                .InsertAfter(xNodeServer, xmlConfigFile.GetElementsByTagName("SDKs")[0].LastChild);

            xmlConfigFile.Save(configFilePath);
        }

        public static void AddDevice(string deviceIPAddress, string deviceName)
        {
            string configFilePath = ConfigFileManager.GetConfigurationFilePath();

            var xmlConfigFile = ConfigFileManager.LoadConfigFile(configFilePath);

            XmlNode xNodeServer = xmlConfigFile.CreateNode(XmlNodeType.Element, "Device", string.Empty);

            XmlAttribute xAttributeSite = xmlConfigFile.CreateAttribute("IPAddress");
            xAttributeSite.Value = deviceIPAddress;
            xNodeServer.Attributes.Append(xAttributeSite);

            XmlAttribute xAttributeName = xmlConfigFile.CreateAttribute("Name");
            xAttributeName.Value = deviceName;
            xNodeServer.Attributes.Append(xAttributeName);

            xmlConfigFile.GetElementsByTagName("Devices")[0]
                .InsertAfter(xNodeServer, xmlConfigFile.GetElementsByTagName("Devices")[0].LastChild);

            xmlConfigFile.Save(configFilePath);
        }

        public static List<SDK> GetSDKs()
        {
            string filePath = GetConfigurationFilePath();

            if (!File.Exists(filePath))
                ConfigFileManager.CreateConfigFile(filePath);

            var sdks = new List<SDK>();

            var doc = ConfigFileManager.LoadConfigFile(filePath);

            XmlNode xServersNode = doc.DocumentElement.FirstChild;

            foreach (XmlNode node in xServersNode.ChildNodes)
            {
                SDK sdk = SDK.GetSDKFromXmlNode(node);
                sdks.Add(sdk);
            }

            return sdks;
        }

        public static List<Device> GetDevices()
        {
            string filePath = GetConfigurationFilePath();

            if (!File.Exists(filePath))
                ConfigFileManager.CreateConfigFile(filePath);

            var devices = new List<Device>();

            var doc = ConfigFileManager.LoadConfigFile(filePath);

            XmlNode xServersNode = doc.DocumentElement.ChildNodes[1];

            foreach (XmlNode node in xServersNode.ChildNodes)
            {
                Device sdk = Device.GetDeviceFromXmlNode(node);
                devices.Add(sdk);
            }

            return devices;
        }

        public static string GetConfigurationFilePath()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                COMPANY_NAME,
                "Configuration");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Path.Combine(path, CONFIGURATION_FILENAME);
        }

        public static string GetExecutionPath()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory;

            return fullPath;
        }

        public static string GetLogPath()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                COMPANY_NAME,
                "Log");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
    }
}
