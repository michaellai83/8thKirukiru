using Kiru8th.JWT;
using Kiru8th.Models;
using Kiru8th.Secret;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace Kiru8th.Controllers
{
    /// <summary>
    /// 後台專區
    /// </summary>
    public class BackmemberController : ApiController
    {
        KiruDb db = new KiruDb();
        /// <summary>
        /// 註冊後台管理員
        /// </summary>
        /// <param name="username">帳號</param>
        /// <param name="userpassword">密碼</param>
        /// <param name="email">信箱</param>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult CreateBackMember(string username, string userpassword, string email, string name)
        //{
        //    PasswordWithSaltHasher passwordWithSalt = new PasswordWithSaltHasher();
        //    HashWithSaltResult hashResultSha256 = passwordWithSalt.HashWithSalt(userpassword, 64, SHA256.Create());
        //    Backmember backmember = new Backmember();
        //    backmember.Username = username;
        //    backmember.Password = hashResultSha256.Digest;
        //    backmember.Salt = hashResultSha256.Salt;
        //    backmember.Email = email;
        //    backmember.Name = name;
        //    backmember.Photo = "origin.jpg";
        //    backmember.IniDateTime = DateTime.Now;
        //    db.Backmembers.Add(backmember);
        //    db.SaveChanges();
        //    return Ok(new { status = "success" });
        //}
   
        private HashWithSaltResult HashWithSaltResult(string password, string salt, HashAlgorithm hashAlgo)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(saltBytes);
            byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());
            return new HashWithSaltResult(Convert.ToBase64String(saltBytes), Convert.ToBase64String(digestBytes));
        }

        /// <summary>
        /// 取得所有QA
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Showallqa()
        {
            var data = db.BackQas.ToList();
            return Ok(data);
        }

        /// <summary>
        /// 信箱驗證
        /// </summary>
        /// <param name="ID">註冊者ID</param>
        /// <param name="email">信箱認證的字串</param>
        /// <returns></returns>
        [Route("checkmail")]
        [HttpGet]
        public IHttpActionResult Checkemail(string ID, string email)
        {
            int id = Convert.ToInt32(ID);
            var member = db.Members.FirstOrDefault(x => x.ID == id);
            if (member == null)
            {
                return Ok<string>("驗證失敗");
            }
            var q = from p in db.Members where p.ID == id select p;
            foreach (var p in q)
            {
                p.Isidentify = true;
            }

            db.SaveChanges();
            //回傳網址
            return Redirect("https://kirukiru.rocket-coding.com/#/signin");
        }

        /// <summary>
        /// 取得精選文章
        /// </summary>
        /// <returns></returns>
        [Route("api/Backmember/GetAllBackArticles")]
        [HttpGet]
        public IHttpActionResult GetAllBackArticles()
        {
            var data = db.BackArticles.OrderByDescending(x => x.IniDateTime).Select(x=>new
            {
                artId = x.ID,
                author = x.Backmembers.Name,
                authorPic = x.Backmembers.Photo,
                title = x.Title,
                main = x.Main,
                firstPhoto = x.Titlepic,
                ArtIniteDate = x.IniDateTime
            }).ToList();
           

            return Ok(new
            {
                success = true,
                data = data
            });
        }
        /// <summary>
        /// 加入文章類別
        /// </summary>
        /// <param name="str">輸入文章類別</param>
        /// <returns></returns>
        [Route("api/Back/CreateArticlecategory")]
        [HttpPost]
        public IHttpActionResult CreateArticlecategory(string str)
        {
            Articlecategory articlecategory = new Articlecategory();
            articlecategory.Name = str;
            db.Articlecategory.Add(articlecategory);
            db.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "添加成功"
            });
        }
        /// <summary>
        /// 查所有文章類別
        /// </summary>
        /// <returns></returns>
        [Route("api/Back/GetArticlecategory")]
        [HttpGet]
        public IHttpActionResult GetArticlecategory()
        {
            var articlecategory = db.Articlecategory.Select(x=>new
            {
                x.Id,
                x.Name
            }).ToList();

          

            return Ok(articlecategory);
        }
        /// <summary>
        /// 刪除文章類別
        /// </summary>
        /// <param name="Artid">刪除文章類別的id</param>
        /// <returns></returns>
        [Route("api/Back/DeleteArtlog")]
        [HttpDelete]
        public IHttpActionResult DeleteArtlog(int Artid)
        {
            var articlecategory = db.Articlecategory.Where(m => m.Id == Artid).FirstOrDefault();
            db.Articlecategory.Remove(articlecategory);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "刪除成功"

            });
        }
        /// <summary>
        /// 找到文章類別
        /// </summary>
        /// <param name="Artid">找到文章類別的ID</param>
        /// <returns></returns>
        [Route("api/Back/GetArtlog")]
        [HttpGet]
        public IHttpActionResult GetArtlog(int Artid)
        {
            try
            {
                var articlog = db.Articlecategory.Where(m => m.Id == Artid).FirstOrDefault();
                //匿名型別
                var result = new
                {
                    Id = articlog.Id,
                    Name = articlog.Name
                };


                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok((ex));
            }
        }
    }
}
