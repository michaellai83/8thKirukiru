using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.EcpayBank
{
    /// <summary>
    /// 綠界回傳資料
    /// </summary>
    public class Ecreturndata
    {
        public string MerchantID { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string MerchantTradeNo { get; set; }
        public string StoreID { get; set; }
        /// <summary>
        /// RtnCode成功為1
        /// </summary>
        public int RtnCode { get; set; }
        public string RtnMsg { get; set; }
        public string TradeNo { get; set; }
        public int TradeAmt { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string TradeDate { get; set; }
        public int SimulatePaid { get; set; }
    }
}