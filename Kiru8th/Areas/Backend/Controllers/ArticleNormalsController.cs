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
    public class ArticleNormalsController : Controller
    {
        private KiruDb db = new KiruDb();

        // GET: Backend/ArticleNormals
        public ActionResult Index()
        {
            var articleNormals = db.ArticleNormals.Include(a => a.Articlecategory).Include(a => a.Member);
            return View(articleNormals.ToList());
        }

        // GET: Backend/ArticleNormals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleNormal articleNormal = db.ArticleNormals.Find(id);
            if (articleNormal == null)
            {
                return HttpNotFound();
            }
            return View(articleNormal);
        }

        // GET: Backend/ArticleNormals/Create
        public ActionResult Create()
        {
            ViewBag.ArticlecategoryId = new SelectList(db.Articlecategory, "Id", "Name");
            ViewBag.MemberId = new SelectList(db.Members, "ID", "UserName");
            return View();
        }

        // POST: Backend/ArticleNormals/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MemberId,Title,Introduction,Main,ArticlecategoryId,IsFree,IsPush,InitDate,Lovecount,IsManangerStop")] ArticleNormal articleNormal)
        {
            if (ModelState.IsValid)
            {
                db.ArticleNormals.Add(articleNormal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticlecategoryId = new SelectList(db.Articlecategory, "Id", "Name", articleNormal.ArticlecategoryId);
            ViewBag.MemberId = new SelectList(db.Members, "ID", "UserName", articleNormal.MemberId);
            return View(articleNormal);
        }

        // GET: Backend/ArticleNormals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleNormal articleNormal = db.ArticleNormals.Find(id);
            if (articleNormal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticlecategoryId = new SelectList(db.Articlecategory, "Id", "Name", articleNormal.ArticlecategoryId);
            ViewBag.MemberId = new SelectList(db.Members, "ID", "UserName", articleNormal.MemberId);
            return View(articleNormal);
        }

        // POST: Backend/ArticleNormals/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MemberId,Title,Introduction,Main,ArticlecategoryId,IsFree,IsPush,InitDate,Lovecount,IsManangerStop")] ArticleNormal articleNormal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleNormal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticlecategoryId = new SelectList(db.Articlecategory, "Id", "Name", articleNormal.ArticlecategoryId);
            ViewBag.MemberId = new SelectList(db.Members, "ID", "UserName", articleNormal.MemberId);
            return View(articleNormal);
        }

        // GET: Backend/ArticleNormals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleNormal articleNormal = db.ArticleNormals.Find(id);
            if (articleNormal == null)
            {
                return HttpNotFound();
            }
            return View(articleNormal);
        }

        // POST: Backend/ArticleNormals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArticleNormal articleNormal = db.ArticleNormals.Find(id);
            db.ArticleNormals.Remove(articleNormal);
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
