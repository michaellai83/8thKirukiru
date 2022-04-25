using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Models.BlueBank
{
    /// <summary>
    /// 智付通交易支付系統回傳解密後資料
    /// <para>1. 即時交易支付方式：信用卡(CREDIT)、WebATM(WEBATM) 、Pay2go 電子錢包(P2G)、Google Pay(ANDROIDPAY)、Samsung Pay(SAMSUNGPAY)。</para>
    /// <para>2. 非即時交易支付方式：超商代碼繳費(CVS)、ATM 轉帳(VACC)、超商條碼繳費(BARCODE) 、超商取貨付款(CVSCOM)。</para>
    /// </summary>
    public class SpgatewayOutputDataModel
    {
        /// <summary>
        /// 回傳狀態
        /// <para>1.若交易付款成功，則回傳SUCCESS。</para>
        /// <para>2.若交易付款失敗，則回傳錯誤代碼。</para>
        /// <para>3.若使用新增自訂支付欄位之交易，則回傳CUSTOM。</para>
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 回傳訊息(敘述此次交易狀態。)
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 商店代號(智付通商店代號)
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public int Amt { get; set; }

        /// <summary>
        /// 智付通交易序號
        /// <para>智付通在此筆交易取號成功時所產生的序號。</para>
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 商店訂單編號
        /// <para>商店自訂訂單編號。</para>
        /// </summary>
        public string MerchantOrderNo { get; set; }
    }
}