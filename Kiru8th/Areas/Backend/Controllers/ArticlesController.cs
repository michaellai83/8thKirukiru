using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kiru8th.Models;

namespace Kiru8th.Areas.Backend.Controllers
{
    public class ArticlesController : Controller
    {
        private KiruDb db = new KiruDb();

        // GET: Backend/Articles
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.Articlecategory).Include(a => a.Member).OrderByDescending(a=>a.InitDate);
            return View(articles.ToList());
        }

        // GET: Backend/Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Backend/Articles/Create
        public ActionResult Create()
        {
            ViewBag.ArticlecategoryId = new SelectList(db.Articlecategory, "Id", "Name");
            ViewBag.MemberId = new SelectList(db.Members, "ID", "UserName");
            return View();
        }

        // POST: Backend/Articles/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MemberId,FirstPicName,Title,IsFree,Introduction,ArticlecategoryId,IsPush,InitDate,Lovecount,FinalText,IsManangerStop")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticlecategoryId = new SelectList(db.Articlecategory, "Id", "Name", article.ArticlecategoryId);
            ViewBag.MemberId = new SelectList(db.Members, "ID", "UserName", article.MemberId);
            return View(article);
        }

        // GET: Backend/Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticlecategoryId = new SelectList(db.Articlecategory, "Id", "Name", article.ArticlecategoryId);
            ViewBag.MemberId = new SelectList(db.Members, "ID", "UserName", article.MemberId);
            return View(article);
        }

        // POST: Backend/Articles/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MemberId,FirstPicName,Title,IsFree,Introduction,ArticlecategoryId,IsPush,InitDate,Lovecount,FinalText,IsManangerStop")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticlecategoryId = new SelectList(db.Articlecategory, "Id", "Name", article.ArticlecategoryId);
            ViewBag.MemberId = new SelectList(db.Members, "ID", "UserName", article.MemberId);
            return View(article);
        }

        // GET: Backend/Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Backend/Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
