using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.BlueBank
{
    public class ConfigurationUtility
    {
        /// <summary>
        /// 取得Web.config的AppSettings設定
        /// </summary>
        /// <param name="appSettingKey">讀取的appSettingKey</param>
        /// <returns></returns>
        public static string GetAppSetting(string appSettingKey)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[appSettingKey]))
            {
                result = ConfigurationManager.AppSettings[appSettingKey].Trim();
            }

            return result;

        }
    }
}