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
    public class ArticlecategoriesController : Controller
    {
        private KiruDb db = new KiruDb();

        // GET: Backend/Articlecategories
        public ActionResult Index()
        {
            return View(db.Articlecategory.ToList());
        }

        // GET: Backend/Articlecategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articlecategory articlecategory = db.Articlecategory.Find(id);
            if (articlecategory == null)
            {
                return HttpNotFound();
            }
            return View(articlecategory);
        }

        // GET: Backend/Articlecategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/Articlecategories/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Articlecategory articlecategory)
        {
            if (ModelState.IsValid)
            {
                db.Articlecategory.Add(articlecategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(articlecategory);
        }

        // GET: Backend/Articlecategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articlecategory articlecategory = db.Articlecategory.Find(id);
            if (articlecategory == null)
            {
                return HttpNotFound();
            }
            return View(articlecategory);
        }

        // POST: Backend/Articlecategories/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Articlecategory articlecategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articlecategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(articlecategory);
        }

        // GET: Backend/Articlecategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articlecategory articlecategory = db.Articlecategory.Find(id);
            if (articlecategory == null)
            {
                return HttpNotFound();
            }
            return View(articlecategory);
        }

        // POST: Backend/Articlecategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Articlecategory articlecategory = db.Articlecategory.Find(id);
            db.Articlecategory.Remove(articlecategory);
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
