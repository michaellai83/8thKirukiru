using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Kiru8th.JWT
{
    public class JwtAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string secret = "kirukiru";//加解密的key,如果不一樣會無法成功解密
            var request = actionContext.Request;
            if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
            {
                var errorMessage = new HttpResponseMessage()
                {
                    ReasonPhrase = "Lost Token",
                    Content = new StringContent("success=false,message=Lost Token"),//自訂的code
                };
                throw new HttpResponseException(errorMessage);

            }
            else
            {
                try
                {
                    var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                        request.Headers.Authorization.Parameter,
                        Encoding.UTF8.GetBytes(secret),
                        JwsAlgorithm.HS512);
                    if (IsTokenExpired(jwtObject["Exp"].ToString()))
                    {
                        var errorMessage = new HttpResponseMessage()
                        {
                            ReasonPhrase = "Token Expired",
                            Content = new StringContent(" success=false,message=Token Expired"),
                        };
                        throw new HttpResponseException(errorMessage);

                    }
                }
                catch (Exception e)
                {
                    var errorMessage = new HttpResponseMessage()
                    {
                        ReasonPhrase = "Lost Token",
                        Content = new StringContent($"success=false,發生錯誤：{e},message=Lost Token"),
                    };
                    throw new HttpResponseException(errorMessage);
                }
            }
        }
        //驗證token時效
        public bool IsTokenExpired(string dateTime)
        {
            return Convert.ToDateTime(dateTime) < DateTime.Now;
        }
    }
}