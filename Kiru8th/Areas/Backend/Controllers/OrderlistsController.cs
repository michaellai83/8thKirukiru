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
    public class OrderlistsController : Controller
    {
        private KiruDb db = new KiruDb();

        // GET: Backend/Orderlists
        public ActionResult Index()
        {
            var orderlists = db.Orderlists.Include(o => o.Members);
            return View(orderlists.ToList());
        }

        // GET: Backend/Orderlists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderlist orderlist = db.Orderlists.Find(id);
            if (orderlist == null)
            {
                return HttpNotFound();
            }
            return View(orderlist);
        }

        // GET: Backend/Orderlists/Create
        public ActionResult Create()
        {
            ViewBag.MemberID = new SelectList(db.Members, "ID", "UserName");
            return View();
        }

        // POST: Backend/Orderlists/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Ordernumber,MemberID,AuthorName,Amount,Issuccess,InitDateTime")] Orderlist orderlist)
        {
            if (ModelState.IsValid)
            {
                db.Orderlists.Add(orderlist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberID = new SelectList(db.Members, "ID", "UserName", orderlist.MemberID);
            return View(orderlist);
        }

        // GET: Backend/Orderlists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderlist orderlist = db.Orderlists.Find(id);
            if (orderlist == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberID = new SelectList(db.Members, "ID", "UserName", orderlist.MemberID);
            return View(orderlist);
        }

        // POST: Backend/Orderlists/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ordernumber,MemberID,AuthorName,Amount,Issuccess,InitDateTime")] Orderlist orderlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderlist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberID = new SelectList(db.Members, "ID", "UserName", orderlist.MemberID);
            return View(orderlist);
        }

        // GET: Backend/Orderlists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orderlist orderlist = db.Orderlists.Find(id);
            if (orderlist == null)
            {
                return HttpNotFound();
            }
            return View(orderlist);
        }

        // POST: Backend/Orderlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orderlist orderlist = db.Orderlists.Find(id);
            db.Orderlists.Remove(orderlist);
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
