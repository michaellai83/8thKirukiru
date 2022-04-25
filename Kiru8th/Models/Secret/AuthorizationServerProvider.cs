using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Kiru8th.Models.Secret
{
    public class AuthorizationServerProvider:OAuthAuthorizationServerProvider
    {
        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            // 依請求來源配置 CORS 允許類型設定
            SetCORSPolicy(context.OwinContext);

            // 如果請求為預檢請求則設為完成直接回傳
            if (context.Request.Method == "OPTIONS")
            {
                context.RequestCompleted();
                return Task.FromResult(0);
            }

            return base.MatchEndpoint(context);
        }
        private void SetCORSPolicy(IOwinContext context)
        {
            // 取出允許跨域的網址 (放在 Web.config 的 appSettings)
            string allowedUrls = ConfigurationManager.AppSettings["allowedOrigins"];

            // 有填寫允許跨域的網址，就分割取出判斷請求的來源是否等於允許跨域的網址，並將允許網址加入 Headers
            if (!String.IsNullOrWhiteSpace(allowedUrls))
            {
                var list = allowedUrls.Split(',');
                if (list.Length > 0)
                {
                    string origin = context.Request.Headers.Get("Origin");
                    var found = list.Where(item => item == origin).Any();
                    if (found)
                    {
                        context.Response.Headers.Add("Access-Control-Allow-Origin",
                            new string[] { origin });
                    }
                }
            }
            // 配置允許請求的 Headers 內容
            context.Response.Headers.Add("Access-Control-Allow-Headers",
                new string[] { "Authorization", "Content-Type" });
            // 配置允許請求的 Headers 方法
            context.Response.Headers.Add("Access-Control-Allow-Methods",
                new string[] { "OPTIONS", "GET", "POST", "PUT", "DELETE" });
        }
    }
}