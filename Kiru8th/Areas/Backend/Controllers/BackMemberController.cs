using Kiru8th.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Kiru8th.Models.EunData;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Net;

namespace Kiru8th.Areas.Backend.Controllers
{
    [Authorize]
    public class BackMemberController : Controller
    {
        KiruDb db = new KiruDb();
        // GET: Backend/BackMember
        public ActionResult Index()
        {
            var memberData = db.Members;
            ViewBag.memberCount = memberData.Count();
            var memberIsCheckEmail = memberData.Where(x => x.Isidentify == true).Count();
            ViewBag.memberIsCheckEmailNum = memberIsCheckEmail;
            var kiruArticles = db.Articles.Count();
            ViewBag.kiruNum = kiruArticles;
            var NormalArticles = db.ArticleNormals.Count();
            ViewBag.NormalArticlesNum = NormalArticles;
            var orederList = db.Orderlists.GroupBy(x=>x.AuthorName).Select(x=>new
            {
                Key =x.Key,
                Num = x.Count()
            }).Join(db.Members, a => a.Key, b => b.UserName, (a, b) => new MemberOrderNum
            {
                Username = a.Key,
                Author = b.Name,
                AuthorPic = b.MemberPicName,
                Num = a.Num
            }).OrderByDescending(x=>x.Num).Take(4).ToList();
            ViewBag.orderList = orederList;
            //ViewBag.orderList = JsonConvert.SerializeObject(orederList);
            var kiruCollects = db.Collects.GroupBy(x => x.ArticleId).Select(x => new
            {
                Key = x.Key,
                Num = x.Count()
            }).OrderByDescending(x=>x.Num).
                Join(db.Articles,a=>a.Key,b=>b.ID,(a,b)=>new KiruCollectNum()
                {
                    artId = a.Key,
                    artTitle = b.Title,
                    artTitlePic = b.FirstPicName,
                    artInfo = b.Introduction,
                    artLog = b.Articlecategory.Name
                })
                .Take(4).ToList();
            ViewBag.kiruCollects = kiruCollects;
            var OrderCount = db.Orderlists.Where(x => x.Issuccess == true).Count();
            ViewBag.OrderCount = OrderCount;
            var today = DateTime.Now;
            var yesterday = today.AddDays(-1);
            var todayKiru = db.Articles.Where(m => m.InitDate >= yesterday && m.InitDate < today && m.IsPush == true)
                .Count();
            var todayNormal = db.ArticleNormals
                .Where(m => m.InitDate >= yesterday && m.InitDate < today && m.IsPush == true).Count();
            int todayArticlesNum = todayKiru + todayNormal;
            ViewBag.TodayNewArtilces = todayArticlesNum;
            var week = today.AddDays(-7);
            var weekKiru = db.Articles.Where(m => m.InitDate >= week && m.InitDate < today && m.IsPush == true)
                .Count();
            var weekNormal = db.ArticleNormals
                .Where(m => m.InitDate >= week && m.InitDate < today && m.IsPush == true).Count();
            int weekArticlesNum = weekKiru + weekNormal;
            var month = today.AddMonths(-1);
            var monthKiru = db.Articles.Where(m => m.InitDate >= month && m.InitDate < today && m.IsPush == true)
                .Count();
            var monthNormal = db.ArticleNormals
                .Where(m => m.InitDate >= month && m.InitDate < today && m.IsPush == true).Count();
            int monthArticlesNum = monthKiru + monthNormal;
            ViewBag.WeekNewArtilces = weekArticlesNum;
            ViewBag.MonthNewArtilces = monthArticlesNum;
            return View();
        }
        public ActionResult AddNewArticle()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddNewArticle(string title, string photo, string ContentText)
        {
            var data = GetUserData();
            var memberData = data.Split('|');
            var memberId = memberData[0];
            var memberpic = memberData[2];
            BackArticle backArticle = new BackArticle();
            backArticle.BackmemberID = Convert.ToInt32(memberId);
            backArticle.Main = ContentText;
            backArticle.Title = title;
            backArticle.Titlepic = photo;
            backArticle.IniDateTime = DateTime.Now;
            db.BackArticles.Add(backArticle);
            db.SaveChanges();
            var artId = backArticle.ID;
            return RedirectToAction("ShowNewArticle");
        }

        public ActionResult ShowNewArticle()
        {
            var data = db.BackArticles.OrderByDescending(x => x.IniDateTime).ToList();
            return View(data);
        }

        public ActionResult DetailNewArticle(int? artId)
        {
            var data = db.BackArticles.FirstOrDefault(x => x.ID == artId);
            var artID = data.ID;
            var artTitle = data.Title;
            var artTitlePhoto = data.Titlepic;
            var artMain = HttpUtility.HtmlDecode(data.Main);
            ViewData["artID"] = artID;
            ViewData["artTitle"] = artTitle;
            ViewData["artTitlePhoto"] = "/images/" + artTitlePhoto;
            ViewData["artMain"] = artMain;
            return View();

        }

        public ActionResult EditArticle(int? artId)
        {
            var data = db.BackArticles.FirstOrDefault(x => x.ID == artId);
            var artID = data.ID;
            var artTitle = data.Title;
            var artTitlePhoto = data.Titlepic;
            var artMain = HttpUtility.HtmlDecode(data.Main);
            ViewData["artID"] = artID;
            ViewData["artTitle"] = artTitle;
            ViewData["artTitlePhoto"] = "/images/" + artTitlePhoto;
            ViewData["artMain"] = artMain;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditArticle(string artId, string title, string ContentText)
        {
            var artID = Convert.ToInt32(artId);
            var data = from q in db.BackArticles
                       where q.ID == artID
                       select q;
            foreach (var str in data.ToList())
            {
                str.Title = title;
                str.Main = ContentText;
            }

            db.SaveChanges();
            return RedirectToAction("ShowNewArticle");
        }

        public ActionResult DeleteArticle(int artId)
        {
            var data = db.BackArticles.FirstOrDefault(x => x.ID == artId);
            db.BackArticles.Remove(data);
            db.SaveChanges();
            return RedirectToAction("ShowNewArticle");
        }
        [HttpPost]
        public ActionResult UploadPhoto()
        {
            string fileName = "";
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase f = Request.Files["photo"];
                var filedata = f.FileName.Split('.');
                var fileFile = filedata[1];
                fileName = "FF" + DateTime.Now.ToFileTime().ToString() + "." + fileFile;
                var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
                var outputPath = Path.Combine(root, fileName);
                f.SaveAs(outputPath);
                //f.SaveAs(@"C:\Users\賴彥融\Desktop\Project\Project\testdatamodel\testdatamodel\images\"+fileName);
            }

            TempData["Photo"] = fileName;
            return RedirectToAction("AddNewArticle", "BackMember");
            //return Content(fileName);
        }

        public static string GetUserData()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = System.Web.HttpContext.Current.User.Identity as FormsIdentity;

                FormsAuthenticationTicket ticked = id.Ticket;
                var userinfo = id.Ticket.UserData;
                return userinfo;
            }

            return "";
        }

        public ActionResult GetAdminName()
        {
            var data = GetUserData();
            var memberData = data.Split('|');
            var memberId = memberData[0];
            var memberName = memberData[1];
            var memberpic = memberData[2];
            return Content(memberName);
        }


        public ActionResult GetAdminPic()
        {
            var data = GetUserData();
            var memberData = data.Split('|');
            var memberId = memberData[0];
            var memberName = memberData[1];
            var memberpic = memberData[2];
            return Content(memberpic);
        }

        public ActionResult MyProfile()
        {
            var data = GetUserData();
            var memberData = data.Split('|');
            var memberId = memberData[0];
            int memberID = Convert.ToInt32(memberId);
            var memberName = memberData[1];
            var memberpic = memberData[2];
            var memberList = db.Backmembers.Where(n => n.ID== memberID);
            var Data = db.Backmembers.FirstOrDefault(x => x.ID == memberID);
            ViewBag.Name = Data.Name;
            ViewBag.UserName = Data.Username;
            ViewBag.Pic = Data.Photo;
            ViewBag.Id = memberId;
            return View(memberList.ToList());
        }
        public ActionResult MyProfileEdit(int?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Backmember backmember = db.Backmembers.Find(id);
            if (backmember == null)
            {
                return HttpNotFound();
            }
            ViewBag.Name = backmember.Name;
            ViewBag.UserName = backmember.Username;
            return View(backmember);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MyProfileEdit(Backmember backmember, HttpPostedFileBase Photos)
        {
            if (ModelState.IsValid)
            {
                if (Photos != null)
                {
                    if (Photos.ContentType.IndexOf("image", System.StringComparison.Ordinal) == -1)
                    {
                        ViewBag.Message = "檔案型態錯誤";
                        return View(backmember);
                    }
                    backmember.Photo = Upload.SaveUpImage(Photos);
                    Upload.GenerateThumbnailImage(backmember.Photo, Photos.InputStream, Server.MapPath("~/images"), "S", 225, 368);

                }
                db.Entry(backmember).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Name = backmember.Name;
                ViewBag.UserName = backmember.Username;
                return View(backmember);
            }
            return View(backmember);
        }

        public ActionResult AddQA()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddQA([Bind(Include = "Title,Answer,InitDateTime")] BackQA backQa)
        {
            backQa.InitDateTime = DateTime.Now;
            db.BackQas.Add(backQa);
            db.SaveChanges();
            return RedirectToAction("ShowAllQA");
        }

        public ActionResult ShowAllQA()
        {
            var data = db.BackQas.ToList();
            return View(data);
        }
        public ActionResult DetailQA(int? qaId)
        {
            if (qaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BackQA backQa = db.BackQas.Find(qaId);
            if (backQa == null)
            {
                return HttpNotFound();
            }
            return View(backQa);
        }

        //public ActionResult DetailQA(int? qaId)
        //{
        //    var data = db.BackQas.FirstOrDefault(x => x.ID == qaId);
        //    var artID = data.ID;
        //    var artTitle = data.Title;
        //    var artMain = HttpUtility.HtmlDecode(data.Answer);
        //    ViewData["artID"] = artID;
        //    ViewData["artTitle"] = artTitle;
        //    ViewData["artMain"] = artMain;
        //    return View();
        //}

        public ActionResult DeleteQA(int? qaId)
        {
            var data = db.BackQas.FirstOrDefault(x => x.ID == qaId);
            db.BackQas.Remove(data);
            db.SaveChanges();
            return RedirectToAction("ShowAllQA");
        }
        public ActionResult EditQA(int? qaId)
        {
            if (qaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BackQA backQa = db.BackQas.Find(qaId);
            if (backQa == null)
            {
                return HttpNotFound();
            }
            
            return View(backQa);
            
        }
        //public ActionResult EditQA(int? qaId)
        //{
        //    var data = db.BackQas.FirstOrDefault(x => x.ID == qaId);
        //    var artID = data.ID;
        //    var artTitle = data.Title;
        //    var artMain = HttpUtility.HtmlDecode(data.Answer);
        //    ViewData["artID"] = artID;
        //    ViewData["artTitle"] = artTitle;
        //    ViewData["artMain"] = artMain;
        //    return View();
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQA(/*[Bind(Include = "Id,Title,Answer,InitDateTime")]*/ BackQA backQa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(backQa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowAllQA");
            }

            return View(backQa);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}