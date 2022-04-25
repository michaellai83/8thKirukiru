using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.BlueBank
{
    public class LogUtil
    {
        /// <summary>
        /// 寫入Log檔
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(string msg)
        {
            try
            {
                string logDirectory = PathUtil.MapPath(ConfigurationUtility.GetAppSetting("LogDirectory"));

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string nowString = DateTime.Now.ToString("yyyyMMdd");
                string logFile = logDirectory + string.Format("log_{0}.txt", nowString);

                File.AppendAllText(logFile, string.Format("{0}: {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg, Environment.NewLine));
            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>
        /// 寫入Log檔
        /// </summary>
        /// <param name="exception"></param>
        public static void WriteLog(Exception exception)
        {
            try
            {
                string logDirectory = PathUtil.MapPath(ConfigurationUtility.GetAppSetting("LogDirectory"));

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string nowString = DateTime.Now.ToString("yyyyMMdd");
                string logFile = logDirectory + string.Format("log_{0}.txt", nowString);

                File.AppendAllText(logFile, string.Format("{0}: {1}{2}{3}{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine));
            }
            catch
            {
            }

        }
    }
}