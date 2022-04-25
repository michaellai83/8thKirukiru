using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.BlueBank
{
    /// <summary>
    /// 解密後資料
    /// </summary>
    public class Returndata
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Result Result { get; set; }
    }
    public class Result
    {
        /// <summary>
        /// 商店代號
        /// </summary>
        public string MerchantID { get; set; }
        /// <summary>
        /// 交易金額
        /// </summary>
        public string Amt { get; set; }
        /// <summary>
        /// 藍星交易序號
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 商店訂單編號
        /// </summary>
        public string MerchantOrderNo { get; set; }
        public string RespondType { get; set; }
        public string IP { get; set; }
        public string EscrowBank { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// 金融機構回應碼(電子錢包專屬)
        /// </summary>
        public string RespondCode { get; set; }
        /// <summary>
        /// 收單銀行
        /// </summary>
        public string Auth { get; set; }
        /// <summary>
        /// 信用卡前6碼
        /// </summary>
        public string Card6No { get; set; }
        /// <summary>
        /// 信用卡後4碼
        /// </summary>
        public string Card4No { get; set; }
        public string Exp { get; set; }
        public string TokenUseStatus { get; set; }
        /// <summary>
        /// 分期-首期金額(信用卡專屬)
        /// </summary>
        public string InstFirst { get; set; }
        /// <summary>
        /// 分期-每期金額(信用卡專屬)
        /// </summary>
        public string InstEach { get; set; }
        /// <summary>
        /// 分期-期別(信用卡專屬)
        /// </summary>
        public string Inst { get; set; }
        /// <summary>
        /// ECI(信用卡專屬)
        /// </summary>
        public string ECI { get; set; }
        /// <summary>
        /// 支付完成時間
        /// </summary>
        public string PayTime { get; set; }
        /// <summary>
        /// 交易類別(電子支付)
        /// </summary>
        public string PaymentMethod { get; set; }
    }
}