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
    [Authorize]
    public class CastsController : Controller
    {
        private EverestDvdEntities db = new EverestDvdEntities();

        // GET: Casts
        [Authorize(Roles ="Manager,Member")]
        public ActionResult Index()
        {
            var cast = db.cast.OrderBy(c => c.last_name);
            return View(cast.ToList());
        }

        // GET: Casts/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cast cast = db.cast.Find(id);
            if (cast == null)
            {
                return HttpNotFound();
            }
            return View(cast);
        }

        // GET: Casts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Casts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,first_name,last_name,dob,height,age,total_movies,about,profile")] cast cast, HttpPostedFileBase profile)
        {
            if (ModelState.IsValid)
            {
                if (profile != null)
                {
                    string pic = System.IO.Path.GetFileName(profile.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/media/castImage"), pic);
                    cast.profile = profile.FileName;
                    profile.SaveAs(path);
                    
                }

                db.cast.Add(cast);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(cast);
        }

        // GET: Casts/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cast cast = db.cast.Find(id);
            if (cast == null)
            {
                return HttpNotFound();
            }
            return View(cast);
        }

        // POST: Casts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,first_name,last_name,dob,height,age,total_movies,about,profile")] cast cast, HttpPostedFileBase profile)
        {
            if (ModelState.IsValid)
            {
                if (profile != null)
                {
                    string pic = System.IO.Path.GetFileName(profile.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/media/castImage"), pic);
                    
                    cast.profile = profile.FileName;
                    profile.SaveAs(path);
                }
                db.Entry(cast).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cast);
        }

        // GET: Casts/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cast cast = db.cast.Find(id);
            if (cast == null)
            {
                return HttpNotFound();
            }
            return View(cast);
        }

        // POST: Casts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            cast cast = db.cast.Find(id);
            db.cast.Remove(cast);
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
