using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EVCharging.Utilities.Helpers
{
    public class LogHelper
    {
        private ConfigHelper configHelper;
        public LogHelper()
        {
            configHelper = FoundationObject.FoundationObj.configHelper;
        }


        public void LogError(string method, string controller, Exception ex)
        {
            try
            {
                var tw = new StringBuilder();
                tw.AppendLine("Date         : " + DateTime.Now);
                tw.AppendLine("Method         : " + method);
                tw.AppendLine("Controller         : " + controller);
                tw.AppendLine("Error         : " + ex.Message);
                tw.AppendLine("--------------------------------------------------");
                tw.AppendLine("TRace   :    ");
                tw.AppendLine(Convert.ToString(ex.StackTrace));
                tw.AppendLine("-----------------------------------------");
                tw.AppendLine("Inner Exception   :    ");
                tw.AppendLine("-----------------------------------------");
                tw.AppendLine(Convert.ToString(ex.InnerException));
                tw.AppendLine("----------------------------------------------------------------------------------------------");
                tw.AppendLine();
                tw.ToString();
                string logFilePath = Path.Combine(configHelper.AppSettings["LogFolderPath"], "logError_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
                {
                    using (StreamWriter log = new StreamWriter(fileStream))
                    {
                        log.WriteLine(tw.ToString());
                    }
                    fileStream.Close();
                }
            }
            catch (Exception)
            {

            }

        }
        public void ErrorLogs(string nmspace, string method, string values, Exception ex)
        {
            try
            {
                var tw = new StringBuilder();
                tw.AppendLine();
                tw.AppendLine("-------------------------------------");
                tw.AppendLine("Date :: " + DateTime.Now);
                tw.AppendLine("NameSpace :: " + nmspace);
                tw.AppendLine("Method :: " + method);
                tw.AppendLine("Values :: " + values);
                tw.AppendLine("Error Message :: " + ex.Message);
                tw.AppendLine("Exception ::" + Convert.ToString(ex));
                tw.AppendLine("-------------------------------------");
                tw.AppendLine();
                tw.ToString();
                string startupPath = System.IO.Directory.GetCurrentDirectory();
                string logFilePath = Path.Combine(startupPath,configHelper.AppSettings["LogFolderPath"], "logErrors_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                FileInfo logFileInfo = new FileInfo(logFilePath);
                DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
                {
                    using (StreamWriter log = new StreamWriter(fileStream))
                    {
                        log.WriteLine(tw.ToString());
                    }
                    fileStream.Close();

                }

            }
            catch (Exception)
            {

            }

        }
        public void LogApplication(string method, string controller, string values)
        {
            try
            {
                var tw = new StringBuilder();
                tw.AppendLine("----------------------------------------------------------------------------------------------");
                tw.AppendLine();
                tw.AppendLine("Date :: " + DateTime.Now);
                tw.AppendLine("Method :: " + method);
                tw.AppendLine(":: Controller :: " + controller);
                tw.AppendLine(":: Values :: " + values);
                tw.AppendLine("----------------------------------------------------------------------------------------------");
                tw.AppendLine();
                tw.ToString();
                string logFilePath = Path.Combine(configHelper.AppSettings["LogFolderPath"], "LogInfo_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
                {
                    using (StreamWriter log = new StreamWriter(fileStream))
                    {
                        log.WriteLine(tw.ToString());
                    }
                }

            }
            catch (Exception)
            {

            }

        }
    }
}
