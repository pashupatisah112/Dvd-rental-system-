using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Coursework;

namespace Coursework.Controllers
{
    public class StudiosController : Controller
    {
        private EverestDvdEntities db = new EverestDvdEntities();

        // GET: Studios
        public ActionResult Index()
        {
            return View(db.studio.ToList());
        }

        // GET: Studios/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            studio studio = db.studio.Find(id);
            if (studio == null)
            {
                return HttpNotFound();
            }
            return View(studio);
        }

        // GET: Studios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Studios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,owner,founder,founded_date,headquarter,movies_produced,website,logo")] studio studio,HttpPostedFileBase logo)
        {
            if (ModelState.IsValid)
            {
                if (logo != null)
                {
                    string pic = System.IO.Path.GetFileName(logo.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/media/castImage"), pic);
                    studio.logo = logo.FileName;
                    logo.SaveAs(path);

                }
                db.studio.Add(studio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studio);
        }

        // GET: Studios/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            studio studio = db.studio.Find(id);
            if (studio == null)
            {
                return HttpNotFound();
            }
            return View(studio);
        }

        // POST: Studios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,owner,founder,founded_date,headquarter,movies_produced,website,logo")] studio studio,HttpPostedFileBase logo)
        {
            if (ModelState.IsValid)
            {
                if (logo != null)
                {
                    string pic = System.IO.Path.GetFileName(logo.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/media/studioImage"), pic);
                    studio.logo = logo.FileName;
                    logo.SaveAs(path);

                }
                db.Entry(studio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studio);
        }

        // GET: Studios/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            studio studio = db.studio.Find(id);
            if (studio == null)
            {
                return HttpNotFound();
            }
            return View(studio);
        }

        // POST: Studios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            studio studio = db.studio.Find(id);
            db.studio.Remove(studio);
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
