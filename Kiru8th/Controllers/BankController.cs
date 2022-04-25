using Kiru8th.JWT;
using Kiru8th.Models;
using Kiru8th.Models.BlueBank;
using Kiru8th.Models.EcpayBank;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Kiru8th.Controllers
{
    /// <summary>
    /// 金流串接
    /// </summary>
    public class BankController : ApiController
    {
        KiruDb db = new KiruDb();
        /// <summary>
        /// 金流基本資料(可再移到Web.config或資料庫設定)
        /// </summary>
        private BankInfoModel _bankInfoModel = new BankInfoModel
        {
            MerchantID = "MS132153993",
            HashKey = "2g38Fdjgzfhp2kG4H9Uvtv93y1bjpBBe",
            HashIV = "CJw6ToMX0fOFcKWP",
            //藍新回導入前端畫面的網站位置
            //ReturnURL = "https://kirukiru.rocket-coding.com",
            ReturnURL = "https://kirukiru.rocket-coding.com/#author/",
            //NotifyURL 為藍新打給後端告知是否訂單成功的連結 所以記得填上自己接資料的api
            NotifyURL = "https://kirukiru.rocket-coding.com/api/Bank/SpgatewayReturn",
            CustomerURL = "https://kirukiru.rocket-coding.com/Bank/SpgatewayCustomer",
            AuthUrl = "https://ccore.spgateway.com/MPG/mpg_gateway",
            CloseUrl = "https://core.newebpay.com/API/CreditCard/Close",
            PeriodUrl = "https://ccore.spgateway.com/MPG/period"
        };
        /// <summary>
        /// [智付通支付]金流介接
        /// </summary>
        /// <param name="main">訂單內容</param>
        /// <param name="payType">請款類型(CREDIT=信用卡付款\WEBATM=網路銀行轉帳付款)</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult SpgatewayPayBill(string main, string payType)
        {
            var userid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var memberdata = db.Members.FirstOrDefault(x => x.ID == userid);
            string username = memberdata.UserName;
            string email = memberdata.Email;
            if (memberdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "請登入帳號密碼"
                });
            }
            else
            {
                var haveSub = db.Subscriptionplans.FirstOrDefault(x => x.Members.UserName == main);
                if (haveSub == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "作者未開通訂閱"
                    });
                }
                var orderAmount = haveSub.Amount;
                var amount = Convert.ToInt32(orderAmount);

                string version = "1.5";

                // 目前時間轉換 +08:00, 防止傳入時間或Server時間時區不同造成錯誤
                DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));
                //訂單編號先處理好 存進去資料庫 先用狀態 是否成功去判斷回傳回來有沒有成功
                string orderno = $"T{DateTime.Now.ToString("yyyyMMddHHmm")}";
                Orderlist orderlist = new Orderlist();
                orderlist.Ordernumber = orderno;
                orderlist.MemberID = userid;
                orderlist.AuthorName = main;
                orderlist.Amount = amount;
                orderlist.Issuccess = false;
                orderlist.InitDateTime = DateTime.Now;
                db.Orderlists.Add(orderlist);
                db.SaveChanges();

                TradeInfo tradeInfo = new TradeInfo()
                {
                    // * 商店代號
                    MerchantID = _bankInfoModel.MerchantID,
                    // * 回傳格式 JSON / String
                    RespondType = "JSON",
                    // * TimeStamp
                    TimeStamp = taipeiStandardTimeOffset.ToUnixTimeSeconds().ToString(),
                    // * 串接程式版本
                    Version = version,
                    // * 商店訂單編號
                    MerchantOrderNo = orderno,
                    //MerchantOrderNo = ordernumber.ToString(),
                    // * 訂單金額
                    Amt = amount,
                    // * 商品資訊
                    ItemDesc = main,
                    // 繳費有效期限(適用於非即時交易)
                    ExpireDate = null,
                    // 支付完成 返回商店網址
                    ReturnURL = _bankInfoModel.ReturnURL + main,
                    // 支付通知網址
                    NotifyURL = _bankInfoModel.NotifyURL,
                    // 商店取號網址
                    //CustomerURL = _bankInfoModel.CustomerURL,
                    // 支付取消 返回商店網址
                    ClientBackURL = null,
                    // * 付款人電子信箱
                    Email = email,
                    // 付款人電子信箱 是否開放修改(1=可修改 0=不可修改)
                    EmailModify = 0,
                    // 商店備註
                    OrderComment = null,
                    // 信用卡 一次付清啟用(1=啟用、0或者未有此參數=不啟用)
                    CREDIT = null,
                    // WEBATM啟用(1=啟用、0或者未有此參數，即代表不開啟)
                    WEBATM = null,
                    // ATM 轉帳啟用(1=啟用、0或者未有此參數，即代表不開啟)
                    VACC = null,
                    // 超商代碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 30 元或超過 2 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                    CVS = null,
                    // 超商條碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 20 元或超過 4 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                    BARCODE = null
                };

                if (string.Equals(payType, "CREDIT"))
                {
                    tradeInfo.CREDIT = 1;
                }
                else if (string.Equals(payType, "WEBATM"))
                {
                    tradeInfo.WEBATM = 1;
                }
                else if (string.Equals(payType, "VACC"))
                {
                    // 設定繳費截止日期
                    tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
                    tradeInfo.VACC = 1;
                }
                else if (string.Equals(payType, "CVS"))
                {
                    // 設定繳費截止日期
                    tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
                    tradeInfo.CVS = 1;
                }
                else if (string.Equals(payType, "BARCODE"))
                {
                    // 設定繳費截止日期
                    tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
                    tradeInfo.BARCODE = 1;
                }

                Atom<string> result = new Atom<string>()
                {
                    IsSuccess = true
                };

                var inputModel = new SpgatewayInputModel
                {
                    MerchantID = _bankInfoModel.MerchantID,
                    Version = version
                };

                // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
                List<KeyValuePair<string, string>> tradeData = LambdaUtil.ModelToKeyValuePairList<TradeInfo>(tradeInfo);
                // 將List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
                var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
                // AES 加密
                inputModel.TradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, _bankInfoModel.HashKey, _bankInfoModel.HashIV);
                // SHA256 加密
                inputModel.TradeSha = CryptoUtil.EncryptSHA256($"HashKey={_bankInfoModel.HashKey}&{inputModel.TradeInfo}&HashIV={_bankInfoModel.HashIV}");


                // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
                List<KeyValuePair<string, string>> postData = LambdaUtil.ModelToKeyValuePairList<SpgatewayInputModel>(inputModel);

                //Response.Clear();

                //StringBuilder s = new StringBuilder();
                //s.Append("<html>");
                //s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
                //s.AppendFormat("<form name='form' action='{0}' method='post'>", _bankInfoModel.AuthUrl);
                //foreach (KeyValuePair<string, string> item in postData)
                //{
                //    s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", item.Key, item.Value);
                //}

                //s.Append("</form></body></html>");
                //s.ToString();
                ArrayList postary = new ArrayList();
                foreach (KeyValuePair<string, string> item in postData)
                {
                    var resultary = new
                    {
                        item.Key,
                        item.Value
                    };
                    postary.Add(resultary);
                }
                //return Ok(new { status = "success", web = s.ToString(), forblue = inputModel.TradeSha });
                //return Ok(s.ToString());

                //return Ok(HttpUtility.HtmlDecode(s.ToString()));

                return Ok(new
                {
                    Status = true,
                    PayData = postary
                });

            }


        }

        /// <summary>
        /// [智付通]金流介接(結果: 支付完成 返回商店網址)這是藍星傳給後端的資料讓後端去知道是否成功交易去更改訂單的狀態
        /// </summary>
        [HttpPost]
        public HttpResponseMessage SpgatewayReturn([FromBody] NewebPayreturn returndata)
        {

            var response = Request.CreateResponse(HttpStatusCode.OK);

            string decryptTradeInfo = CryptoUtil.DecryptAESHex(returndata.TradeInfo, _bankInfoModel.HashKey, _bankInfoModel.HashIV);
            Returndata result = JsonConvert.DeserializeObject<Returndata>(decryptTradeInfo);

            var orderNo = result.Result.MerchantOrderNo;

            var q = from p in db.Orderlists
                    where p.Ordernumber == orderNo
                    select p;
            foreach (var p in q)
            {
                p.Issuccess = true;
            }
            db.SaveChanges();

            return response;
        }

        /// <summary>
        /// 定期定額(藍新
        /// </summary>
        /// <param name="main">訂單內容</param>
        /// <param name="amount">訂單金額</param>
        /// <param name="peroidtype">訂閱(年/月)</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult SpgatewayPeriodic(string main, int amount, string peroidtype)
        {
            var userid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var memberdata = db.Members.FirstOrDefault(x => x.ID == userid);
            string username = memberdata.UserName;
            string email = memberdata.Email;
            if (memberdata == null)
            {
                return Ok(new { status = "請登入帳號密碼" });
            }
            else
            {
                string version = "1.5";

                // 目前時間轉換 +08:00, 防止傳入時間或Server時間時區不同造成錯誤
                DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));
                //訂單編號先處理好 存進去資料庫 先用狀態 是否成功去判斷回傳回來有沒有成功
                string orderno = $"T{DateTime.Now.ToString("yyyyMMddHHmm")}";
                //Orderlist orderlist = new Orderlist();
                //orderlist.Ordernumber = orderno;
                //orderlist.MemberID = userid;
                //orderlist.AuthorName = main;
                //orderlist.Issuccess = false;
                //orderlist.InitDateTime = DateTime.Now;
                //db.Orderlists.Add(orderlist);
                //db.SaveChanges();

                //TradeInfo tradeInfo = new TradeInfo()
                //{
                //    // * 商店代號
                //    MerchantID = _bankInfoModel.MerchantID,
                //    // * 回傳格式 JSON / String
                //    RespondType = "JSON",
                //    // * TimeStamp
                //    TimeStamp = taipeiStandardTimeOffset.ToUnixTimeSeconds().ToString(),
                //    // * 串接程式版本
                //    Version = version,
                //    // * 商店訂單編號
                //    MerchantOrderNo = orderno,
                //    //MerchantOrderNo = ordernumber.ToString(),
                //    // * 訂單金額
                //    PeriodAmt = amount,
                //    // * 商品資訊
                //    ProdDesc = main,
                //    // 繳費有效期限(適用於非即時交易)
                //    ExpireDate = null,
                //    // 支付完成 返回商店網址
                //    ReturnURL = _bankInfoModel.ReturnURL + "?orderno=" + orderno,
                //    // 支付通知網址
                //    NotifyURL = _bankInfoModel.NotifyURL,
                //    // 商店取號網址
                //    CustomerURL = _bankInfoModel.CustomerURL,
                //    // 支付取消 返回商店網址
                //    ClientBackURL = null,
                //    // * 付款人電子信箱
                //    Email = string.Empty,
                //    // 付款人電子信箱 是否開放修改(1=可修改 0=不可修改)
                //    EmailModify = 0,
                //    // 週期類別
                //    PeriodType = null,
                //    //交易週期授權時間
                //    PeriodPoint = null,
                //    //檢查卡號模式
                //    PeriodStartType = 1,
                //    //授權期數
                //    PeriodTimes = "99",
                //};

                //if (string.Equals(peroidtype, "Y"))
                //{
                //    tradeInfo.PeriodType = peroidtype;
                //    tradeInfo.PeriodPoint = "0105";
                //}
                //else if (string.Equals(peroidtype, "M"))
                //{
                //    tradeInfo.PeriodType = peroidtype;
                //    tradeInfo.PeriodPoint = "05";
                //}
                string tradeInfo_PeriodPoint = "";
                if (string.Equals(peroidtype, "Y"))
                {

                    tradeInfo_PeriodPoint = "0105";
                }
                else if (string.Equals(peroidtype, "M"))
                {

                    tradeInfo_PeriodPoint = "05";
                }

                //Atom<string> result = new Atom<string>()
                //{
                //    IsSuccess = true
                //};

                //var inputModel = new SpgatewayInputModel
                //{
                //    MerchantID = _bankInfoModel.MerchantID,
                //    Version = version
                //};

                var inputModel = new SpgatewayInputModel
                {
                    MerchantID = _bankInfoModel.MerchantID,
                    Version = version
                };


                // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
                //List<KeyValuePair<string, string>> tradeData = LambdaUtil.ModelToKeyValuePairList<TradeInfo>(tradeInfo);
                List<KeyValuePair<string, string>> tradeData = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("RespondType", "JSON"),
                    new KeyValuePair<string, string>("TimeStamp", taipeiStandardTimeOffset.ToUnixTimeSeconds().ToString()),
                    new KeyValuePair<string, string>("Version", version),
                    new KeyValuePair<string, string>("MerchantOrderNo", orderno.ToString()),
                    new KeyValuePair<string, string>("PeriodAmt", amount.ToString()),
                    new KeyValuePair<string, string>("ProdDesc", main),
                    new KeyValuePair<string, string>("PeriodType", peroidtype),
                    new KeyValuePair<string, string>("PayerEmail", email),
                    new KeyValuePair<string, string>("PeriodStartType", "1"),
                    new KeyValuePair<string, string>("PeriodPoint", tradeInfo_PeriodPoint),
                    new KeyValuePair<string, string>("PeriodTimes", "99" )
                };
                // 將List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
                var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
                // AES 加密
                inputModel.TradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, _bankInfoModel.HashKey, _bankInfoModel.HashIV);


                // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
                List<KeyValuePair<string, string>> postData = LambdaUtil.ModelToKeyValuePairList<SpgatewayInputModel>(inputModel);


                ArrayList postary = new ArrayList();
                foreach (KeyValuePair<string, string> item in postData)
                {
                    var resultary = new
                    {
                        item.Key,
                        item.Value
                    };
                    postary.Add(resultary);
                }
                return Ok(new
                {
                    Status = true,
                    PayData = postary
                });
            }

        }
        /// <summary>
        /// 綠界金流單筆
        /// </summary>
        /// <param name="main">內容(訂閱的帳號)</param>
        /// <param name="amount">價錢</param>
        /// <param name="Payment">預設(ALL)</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult Ecpay(string main, int amount, string Payment)
        {
            var userid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var memberdata = db.Members.FirstOrDefault(x => x.ID == userid);
            string username = memberdata.UserName;
            string email = memberdata.Email;
            string returnurl = "https://kirukiru.rocket-coding.com/api/Bank/ecreturn";
            if (memberdata == null)
            {
                return Ok(new { status = "請登入帳號密碼" });
            }
            else
            {
                string version = "1.5";
                string Merchantid = "2000132";
                // 目前時間轉換 +08:00, 防止傳入時間或Server時間時區不同造成錯誤
                DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));
                //訂單編號先處理好 存進去資料庫 先用狀態 是否成功去判斷回傳回來有沒有成功
                string orderno = $"T{DateTime.Now.ToString("yyyyMMddHHmm")}";



                var time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
                //List<KeyValuePair<string, string>> tradeData = LambdaUtil.ModelToKeyValuePairList<TradeInfo>(tradeInfo);
                List<KeyValuePair<string, string>> tradeData = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("ChoosePayment", Payment),
                    new KeyValuePair<string, string>("EncryptType", "1"),
                    new KeyValuePair<string, string>("ItemName", main),
                    new KeyValuePair<string, string>("MerchantID", Merchantid),
                    new KeyValuePair<string, string>("MerchantTradeDate", time),
                    new KeyValuePair<string, string>("MerchantTradeNo", orderno.ToString()),
                    new KeyValuePair<string, string>("PaymentType", "aio"),
                    new KeyValuePair<string, string>("ReturnURL", returnurl),
                    new KeyValuePair<string, string>("TotalAmount", amount.ToString()),
                    new KeyValuePair<string, string>("TradeDesc", "註記")
                };
                // 將List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
                var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
                // AES 加密
                string ceckno = BuildCheckMacValue(tradeQueryPara);

                Orderlist orderlist = new Orderlist();
                orderlist.MemberID = userid;
                orderlist.AuthorName = main;
                orderlist.Amount = amount;
                orderlist.InitDateTime = DateTime.Now;
                orderlist.Ordernumber = orderno;
                orderlist.Issuccess = false;
                db.Orderlists.Add(orderlist);
                db.SaveChanges();

                ArrayList postary = new ArrayList();
                var result = new
                {
                    ChoosePayment = Payment,
                    EncryptType = "1",
                    ItemName = main,
                    MerchantTradeDate = time,
                    MerchantTradeNo = orderno.ToString(),
                    PaymentType = "aio",
                    ReturnURL = _bankInfoModel.ReturnURL,
                    TotalAmount = amount.ToString(),
                    TradeDesc = "註記",
                    CheckMacValue = ceckno
                };
                postary.Add(result);
                return Ok(new
                {
                    Status = true,
                    PayData = postary
                });
            }

        }
        /// <summary>
        /// 綠界金流 定期
        /// </summary>
        /// <param name="main">內容(訂閱帳號)</param>
        /// <param name="PeriodAmount">訂閱金額</param>
        /// <param name="PeriodType">月費還是年費(M/Y)</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult Ecpayperiod(string main, int PeriodAmount, string PeriodType)
        {
            var userid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var memberdata = db.Members.FirstOrDefault(x => x.ID == userid);
            string username = memberdata.UserName;
            string email = memberdata.Email;
            if (memberdata == null)
            {
                return Ok(new { status = "請登入帳號密碼" });
            }
            else
            {
                string Merchantid = "2000132";
                string Payment = "Credit";
                int Frequency = 1;
                int ExecTimes = 0;
                // 目前時間轉換 +08:00, 防止傳入時間或Server時間時區不同造成錯誤
                DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));
                //訂單編號先處理好 存進去資料庫 先用狀態 是否成功去判斷回傳回來有沒有成功
                string orderno = $"T{DateTime.Now.ToString("yyyyMMddHHmm")}";
                var time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string returnurl = "https://kirukiru.rocket-coding.com/api/Bank/ecreturn";
                Orderlist orderlist = new Orderlist();
                orderlist.Ordernumber = orderno;
                orderlist.MemberID = userid;
                orderlist.AuthorName = main;
                orderlist.Issuccess = false;
                orderlist.InitDateTime = DateTime.Now;
                db.Orderlists.Add(orderlist);
                db.SaveChanges();

                if (string.Equals(PeriodType, "M"))
                {
                    ExecTimes = 99;
                }
                else if (string.Equals(PeriodType, "Y"))
                {
                    ExecTimes = 9;
                }


                // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
                //List<KeyValuePair<string, string>> tradeData = LambdaUtil.ModelToKeyValuePairList<TradeInfo>(tradeInfo);
                List<KeyValuePair<string, string>> tradeData = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("ChoosePayment", Payment),
                    new KeyValuePair<string, string>("EncryptType", "1"),
                    new KeyValuePair<string, string>("ExecTimes", ExecTimes.ToString()),
                    new KeyValuePair<string, string>("Frequency", Frequency.ToString()),
                    new KeyValuePair<string, string>("ItemName", main),
                    new KeyValuePair<string, string>("MerchantID", Merchantid),
                    new KeyValuePair<string, string>("MerchantTradeDate", time),
                    new KeyValuePair<string, string>("MerchantTradeNo", orderno.ToString()),
                    new KeyValuePair<string, string>("PaymentType", "aio"),
                    new KeyValuePair<string, string>("PeriodAmount", PeriodAmount.ToString()),
                    //new KeyValuePair<string, string>("PeriodReturnURL", _bankInfoModel.ReturnURL),
                    new KeyValuePair<string, string>("PeriodType", PeriodType),
                    new KeyValuePair<string, string>("ReturnURL", returnurl),
                    new KeyValuePair<string, string>("TotalAmount", PeriodAmount.ToString()),
                    new KeyValuePair<string, string>("TradeDesc", "註記")
                };
                // 將List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
                var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
                // AES 加密
                string ceckno = BuildCheckMacValue(tradeQueryPara);



                ArrayList postary = new ArrayList();
                var result = new
                {
                    ChoosePayment = Payment,
                    EncryptType = "1",
                    ItemName = main,
                    MerchantTradeDate = time,
                    MerchantTradeNo = orderno.ToString(),
                    PaymentType = "aio",
                    ReturnURL = returnurl,
                    TotalAmount = PeriodAmount,
                    TradeDesc = "註記",
                    PeriodAmount = PeriodAmount,
                    PeriodType = PeriodType,
                    Frequency = Frequency,
                    ExecTimes = ExecTimes,
                    CheckMacValue = ceckno
                };
                postary.Add(result);
                return Ok(new
                {
                    Status = true,
                    PayData = postary
                });
            }

        }
        /// <summary>
        /// 綠界回傳資訊
        /// </summary>
        /// <param name="returndata"></param>
        /// <returns></returns>
        [Route("api/Bank/ecreturn")]
        [HttpPost]
        public HttpResponseMessage EcReturn([FromBody] Ecreturndata returndata)
        {

            var response = Request.CreateResponse(HttpStatusCode.OK);

            var data = returndata.RtnCode;
            if (data != 1)
            {
                return response;
            }
            var orderNo = returndata.MerchantTradeNo;

            var q = from p in db.Orderlists
                    where p.Ordernumber == orderNo
                    select p;
            foreach (var p in q)
            {
                p.Issuccess = true;
            }
            db.SaveChanges();

            return response;

        }

        ///<summary>
        /// 產生檢查碼。
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// 傳遞 parameters 參數,需要先經過英文字母 A-Z 排序
        private string BuildCheckMacValue(string parameters, int encryptType = 1)
        {
            string key = "5294y06JbISpM5x9";
            string IV = "v77hoKGq4kWxNNIS";
            string szCheckMacValue = String.Empty;
            // 產生檢查碼。
            szCheckMacValue = $"HashKey={key}&{parameters}&HashIV={IV}";
            szCheckMacValue = HttpUtility.UrlEncode(szCheckMacValue).ToLower();
            if (encryptType == 1)
            {
                szCheckMacValue = SHA256Encoder.Encrypt(szCheckMacValue);
            }
            else
            {
                szCheckMacValue = MD5Encoder.Encrypt(szCheckMacValue);
            }

            return szCheckMacValue;
        }
    }
}
