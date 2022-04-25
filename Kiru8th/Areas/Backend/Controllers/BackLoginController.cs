using Kiru8th.Models;
using Kiru8th.Secret;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Kiru8th.Areas.Backend.Controllers
{
    public class BackLoginController : Controller
    {
        KiruDb db = new KiruDb();
        // GET: Backend/BackLogin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var member = db.Backmembers.FirstOrDefault(x => x.Username == email);
            if (member == null)
            {
                ViewBag.Err = "帳號密碼錯誤";
                return View();
            }

            var checkpassword = member.Password;
            var checksalt = member.Salt;
            string Rightpassword = HashWithSaltResult(password, checksalt, SHA256.Create()).Digest.ToString();
            if (checkpassword == Rightpassword)
            {
                string userdata = member.ID + "|" + member.Name + "|" + member.Photo;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    member.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    true,
                    userdata,
                    FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "BackMember");
            }
            else
            {
                ViewBag.Err = "帳號密碼錯誤";
            }

            return View();
        }

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

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        //public ActionResult Register(string email, string password,string username, string confirmpassword)
        public ActionResult Register([Bind(Include = "Id,Username,Password,Email")] Backmember backmember, string confirmpassword)
        {
            if (ModelState.IsValid)
            {
                if (backmember.Password != confirmpassword)
                {
                    ViewBag.Message = "密碼輸入不同";
                    return View();
                }

                var checkEmail = db.Backmembers.FirstOrDefault(x => x.Username == backmember.Email);
                if (checkEmail != null)
                {
                    ViewBag.Message = "已有相同帳號";
                    return View();
                }
                PasswordWithSaltHasher passwordWithSalt = new PasswordWithSaltHasher();
                HashWithSaltResult hashResultSha256 = passwordWithSalt.HashWithSalt(backmember.Password, 64, SHA256.Create());
                Backmember member = new Backmember();
                member.Username = backmember.Email;
                member.Email = backmember.Email;
                member.Password = hashResultSha256.Digest;
                member.Salt = hashResultSha256.Salt;
                member.Name = "管理員";
                member.Photo = "origin.jpg";
                member.IniDateTime = DateTime.Now;
                db.Backmembers.Add(member);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            //System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Login","BackLogin");
        }
    }
}