using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiru8th.Email
{
    public class Sendmail
    {
        /// <summary>
        /// 寄信
        /// </summary>
        /// <param name="id">註冊好資料庫回傳的ID</param>
        /// <param name="username">註冊帳號</param>
        /// <param name="name">註冊者名字</param>
        /// <param name="mailaddress">註冊者信箱</param>
        /// <param name="emailidentify">信箱驗證號碼</param>
        public static void Sendemail(int id, string username, string name, string mailaddress, string emailidentify)
        {
            //宣告使用 MimeMessage
            var message = new MimeMessage();
            //設定發信地址 ("發信人", "發信 email")
            message.From.Add(new MailboxAddress("Kirukiru", "michaelbmw520@gmail.com"));
            //設定收信地址 ("收信人", "收信 email")
            message.To.Add(new MailboxAddress(name, mailaddress));
            //寄件副本email
            message.Cc.Add(new MailboxAddress(name, mailaddress));
            //設定優先權
            //message.Priority = MessagePriority.Normal;
            //信件標題
            message.Subject = "Kirukiru Auto Email(信箱驗證)";

            var web = @"https://kirukiru.rocket-coding.com/checkmail?ID=";

            //建立 html 郵件格式
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody =
                $"<h1>感謝{name}註冊切切,大家一起來切切!</h1>" +
                $"<h3>姓名 : {name}</h3>" +
                $"<h3>信箱 : {mailaddress}</h3>" +
                $"<h3>帳號 : {username}</h3>" +
                $"<h3>請按此網址驗證 : </h3>" +
                $"<p><a href='{web}{id}&email={emailidentify}'>按此驗證</a></p>";
            //設定郵件內容
            message.Body = bodyBuilder.ToMessageBody(); //轉成郵件內容格式

            using (var client = new SmtpClient())
            {
                //有開防毒時需設定 false 關閉檢查
                client.CheckCertificateRevocation = false;
                //設定連線 gmail ("smtp Server", Port, SSL加密) 
                client.Connect("smtp.gmail.com", 587, false); // localhost 測試使用加密需先關閉 

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("michaelbmw520@gmail.com", "atibywlkrblacmsy");
                //發信
                client.Send(message);
                //結束連線
                client.Disconnect(true);
            }
        }
        /// <summary>
        /// 忘記密碼寄信
        /// </summary>
        /// <param name="username">使用者帳號</param>
        /// <param name="name">使用者名字</param>
        /// <param name="mailaddress">使用者信箱</param>
        /// <param name="passwords">預設密碼</param>
        public static void SendForgetPassWordsMail(string username, string name, string mailaddress, string passwords)
        {
            //宣告使用 MimeMessage
            var message = new MimeMessage();
            //設定發信地址 ("發信人", "發信 email")
            message.From.Add(new MailboxAddress("Kirukiru", "michaelbmw520@gmail.com"));
            //設定收信地址 ("收信人", "收信 email")
            message.To.Add(new MailboxAddress(name, mailaddress));
            //寄件副本email
            message.Cc.Add(new MailboxAddress(name, mailaddress));
            //設定優先權
            //message.Priority = MessagePriority.Normal;
            //信件標題
            message.Subject = "Kirukiru Auto Email(您的新的密碼)";

            var web = @"https://kirukiru.rocket-coding.com/checkmail?ID=";

            //建立 html 郵件格式
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody =
                $"<h1>親愛的{name},不要再忘記密碼囉!</h1>" +
                $"<h3>姓名 : {name}</h3>" +
                $"<h3>信箱 : {mailaddress}</h3>" +
                $"<h3>帳號 : {username}</h3>" +
                $"<h3>這是您新的密碼請盡速修改密碼 : </h3>" +
                $"<p>密碼為{passwords}</p>";
            //設定郵件內容
            message.Body = bodyBuilder.ToMessageBody(); //轉成郵件內容格式

            using (var client = new SmtpClient())
            {
                //有開防毒時需設定 false 關閉檢查
                client.CheckCertificateRevocation = false;
                //設定連線 gmail ("smtp Server", Port, SSL加密) 
                client.Connect("smtp.gmail.com", 587, false); // localhost 測試使用加密需先關閉 

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("michaelbmw520@gmail.com", "atibywlkrblacmsy");
                //發信
                client.Send(message);
                //結束連線
                client.Disconnect(true);
            }
        }
    }
}