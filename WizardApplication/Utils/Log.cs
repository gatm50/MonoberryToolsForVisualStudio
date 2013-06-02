using System;
using System.IO;

namespace WizardApplication.Utils
{
    public static class Log
    {
        private const string LOG_NAME = "CirrusAddinForm";

        public static void AddMessageLog(string message)
        {
            string directoryName = ConfigFileManager.GetLogPath();
            string realLogName = directoryName + "\\" + LOG_NAME + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt";
            StreamWriter log;

            if (!File.Exists(realLogName))
            {
                log = new StreamWriter(realLogName);
            }
            else
            {
                log = File.AppendText(realLogName);
            }

            log.WriteLine(message);
            log.WriteLine();
            log.Close();
        }
    }
}
