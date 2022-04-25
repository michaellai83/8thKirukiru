using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kiru8th.JWT
{
    public class JwtAuthUtil
    {
        /// <summary>
        /// 產生Token
        /// </summary>
        /// <param name="id">使用者id</param>
        /// <param name="username">帳號</param>
        /// <param name="name">名字</param>
        /// <returns></returns>
        public string GenerateToken(int id, string username, string name)
        {
            string secret = "kirukiru";//加解密的key,如果不一樣會無法成功解密 (可以修改可以自創)
            Dictionary<string, Object> claim = new Dictionary<string, Object>();//payload 需透過token傳遞的資料
            claim.Add("Id", id);
            claim.Add("Username", username);
            claim.Add("Name", name);
            claim.Add("iat", DateTime.Now.ToString());//建立時間
            claim.Add("Exp", DateTime.Now.AddSeconds(Convert.ToInt32("86400")).ToString());//Token 時效設定100秒
            var payload = claim;
            var token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS512);//產生token
            return token;
        }
        /// <summary>
        /// 查詢使用者ID
        /// </summary>
        /// <param name="Token">輸入前面拿到的Token key</param>
        /// <returns></returns>
        public static int GetId(string Token)
        {
            string secret = "kirukiru";//加解密的key,如果不一樣會無法成功解密
            var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                Token,
                Encoding.UTF8.GetBytes(secret),
                JwsAlgorithm.HS512);
            return Convert.ToInt32(jwtObject["Id"]);
        }
        /// <summary>
        /// 查詢使用者名字
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static string GetUsername(string Token)
        {
            string secret = "kirukiru";
            var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                Token,
                Encoding.UTF8.GetBytes(secret),
                JwsAlgorithm.HS512);
            string username = (jwtObject["Username"]).ToString();
            return username;
        }

        /// <summary>
        /// 查詢使用者Token的內容
        /// </summary>
        /// <param name="Token">Token</param>
        /// <returns></returns>
        public static Tuple<int, string, string> GetuserList(string Token)
        {
            string secret = "kirukiru";
            var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                Token,
                Encoding.UTF8.GetBytes(secret),
                JwsAlgorithm.HS512);
            //var result = new
            //{
            //    ID = Convert.ToInt32(jwtObject["Id"]),
            //    Username = jwtObject["Username"].ToString(),
            //    Name = jwtObject["Name"].ToString(),
            //};
            var result = Tuple.Create<int, string, string>(Convert.ToInt32(jwtObject["Id"]), jwtObject["Username"].ToString(), jwtObject["Name"].ToString());
            return result;


        }
    }
}