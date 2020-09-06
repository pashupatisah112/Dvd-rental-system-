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
    public class ProducersController : Controller
    {
        private EverestDvdEntities db = new EverestDvdEntities();

        // GET: Producers
        public ActionResult Index()
        {
            return View(db.producer.ToList());
        }

        // GET: Producers/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            producer producer = db.producer.Find(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // GET: Producers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,first_name,last_name,dob,movies_produced,net_worth,about,profile")] producer producer, HttpPostedFileBase profile)
        {
            if (ModelState.IsValid)
            {
                if (profile != null)
                {
                    string pic = System.IO.Path.GetFileName(profile.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/media/producerImage"), pic);
                    
                    producer.profile = profile.FileName;
                    profile.SaveAs(path);
                }
                db.producer.Add(producer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producer);
        }

        // GET: Producers/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            producer producer = db.producer.Find(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // POST: Producers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,first_name,last_name,dob,movies_produced,net_worth,about,profile")] producer producer, HttpPostedFileBase profile)
        {
            if (ModelState.IsValid)
            {
                if (profile != null)
                {
                    string pic = System.IO.Path.GetFileName(profile.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/media/producerImage"), pic);
                    // file is uploaded
                    producer.profile = profile.FileName;
                    profile.SaveAs(path);
                }
                db.Entry(producer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producer);
        }

        // GET: Producers/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            producer producer = db.producer.Find(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            producer producer = db.producer.Find(id);
            db.producer.Remove(producer);
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
