using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kiru8th.Models;

namespace Kiru8th.Areas.Backend.Controllers
{
    public class BackUserMembersController : Controller
    {
        private KiruDb db = new KiruDb();

        // GET: Backend/BackUserMembers
        public ActionResult Index()
        {
            return View(db.Backmembers.ToList());
        }

        // GET: Backend/BackUserMembers/Details/5
        public ActionResult Details(int? id)
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
            return View(backmember);
        }

        // GET: Backend/BackUserMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Backend/BackUserMembers/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Username,Password,Salt,Name,Email,Photo,IniDateTime")] Backmember backmember)
        {
            if (ModelState.IsValid)
            {
                db.Backmembers.Add(backmember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(backmember);
        }

        // GET: Backend/BackUserMembers/Edit/5
        public ActionResult Edit(int? id)
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
            return View(backmember);
        }

        // POST: Backend/BackUserMembers/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Username,Password,Salt,Name,Email,Photo,IniDateTime")] Backmember backmember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(backmember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(backmember);
        }

        // GET: Backend/BackUserMembers/Delete/5
        public ActionResult Delete(int? id)
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
            return View(backmember);
        }

        // POST: Backend/BackUserMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Backmember backmember = db.Backmembers.Find(id);
            db.Backmembers.Remove(backmember);
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

        public ActionResult EditPic(int?id)
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
            return View(backmember);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPic(Backmember backmember , HttpPostedFileBase Photos)
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
                    db.Entry(backmember).State = EntityState.Modified;
                    db.SaveChanges();
                    return View(backmember);
                }

                

            }
            return View(backmember);
        }
        
    }
}
