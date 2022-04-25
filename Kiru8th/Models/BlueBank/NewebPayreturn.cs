using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.BlueBank
{
    /// <summary>
    /// 藍星回傳格式
    /// </summary>
    public class NewebPayreturn
    {
        public string Status { get; set; }
        public string MerchantID { get; set; }
        public string TradeInfo { get; set; }
        public string TradeSha { get; set; }
        public string Version { get; set; }
    }
}