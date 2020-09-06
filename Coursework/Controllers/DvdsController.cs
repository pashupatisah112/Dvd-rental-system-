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
    [Authorize]   //allows only authenticated users to view and uses
    public class DvdsController : Controller
    {
        
        private EverestDvdEntities db = new EverestDvdEntities();

        // GET: Dvds
        public ActionResult Index()
        {
            var dvd = db.dvd.Include(d => d.cast).Include(d => d.Genres).Include(d => d.producer).Include(d => d.studio).OrderBy(d=>d.released_date);
            return View(dvd.ToList());
        }

        //Searches for dvd copies which are older than 365 days
        public ActionResult OlderCopies()
        {
            DateTime dtTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //var dvd_older = db.loan.Include(l => l.dvd).Include(l => l.loan_status).Where(l.date_added > System.Data.Entity.DbFunctions.AddDays(db.dvd.date_added, 365));
                
            var dvd_older = (from l in db.loan
                            join d in db.dvd on l.dvd_id equals d.id
                            join s in db.loan_status on l.loan_status_id equals s.id
                            where d.date_added <= System.Data.Entity.DbFunctions.AddDays(dtTo, -365) && s.loan_status1 == "Cleared"
                            select l).ToList();
                            
                            
            ViewBag.dvd_older = dvd_older;
            return View();
        }

        //Finds the details about the dvd on loan
        public ActionResult LoanCopy()
        {
            var dvd_loan = db.loan.Include(l => l.dvd).Include(l => l.member).Where(l => l.date_returned == null).OrderBy(l => l.date_out);
            ViewBag.dvd_loan = dvd_loan;
            return View();
        }

        //Gets the detsils of dvd which is not loaned often i.e. loaned before 31 days
        public ActionResult LeastLoaned()
        {
            DateTime dtTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //var date_range = System.Data.Entity.DbFunctions.AddDays(dtTo, 31);
            var least = db.loan.Include(l=>l.member).Where(l=>l.date_out <= System.Data.Entity.DbFunctions.AddDays(dtTo, 31));
            ViewBag.least = least;
            return View();
        }

        // GET: Dvds/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dvd dvd = db.dvd.Find(id);
            if (dvd == null)
            {
                return HttpNotFound();
            }
            return View(dvd);
        }

        // GET: Dvds/Create
        public ActionResult Create()
        {
            ViewBag.cast_id = new SelectList(db.cast, "id", "first_name");
            ViewBag.genres_id = new SelectList(db.Genres, "id", "genres1");
            ViewBag.producer_id = new SelectList(db.producer, "id", "first_name");
            ViewBag.studio_id = new SelectList(db.studio, "id", "name");
            return View();
        }

        // POST: Dvds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,copy_number,title,released_date,genres_id,stock,date_added,studio_id,producer_id,cast_id,cover")] dvd dvd,HttpPostedFileBase cover)
        {
            if (ModelState.IsValid)
            {
                if (cover != null)
                {
                    string pic = System.IO.Path.GetFileName(cover.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/media/dvdPoster"), pic);
                    dvd.cover = cover.FileName;
                    cover.SaveAs(path);

                }

                db.dvd.Add(dvd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cast_id = new SelectList(db.cast, "id", "first_name", dvd.cast_id);
            ViewBag.genres_id = new SelectList(db.Genres, "id", "genres1", dvd.genres_id);
            ViewBag.producer_id = new SelectList(db.producer, "id", "first_name", dvd.producer_id);
            ViewBag.studio_id = new SelectList(db.studio, "id", "name", dvd.studio_id);
            return View(dvd);
        }

        // GET: Dvds/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dvd dvd = db.dvd.Find(id);
            if (dvd == null)
            {
                return HttpNotFound();
            }
            ViewBag.cast_id = new SelectList(db.cast, "id", "first_name", dvd.cast_id);
            ViewBag.genres_id = new SelectList(db.Genres, "id", "genres1", dvd.genres_id);
            ViewBag.producer_id = new SelectList(db.producer, "id", "first_name", dvd.producer_id);
            ViewBag.studio_id = new SelectList(db.studio, "id", "name", dvd.studio_id);
            return View(dvd);
        }

        // POST: Dvds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,copy_number,title,released_date,genres_id,stock,date_added,studio_id,producer_id,cast_id,cover")] dvd dvd,HttpPostedFileBase cover)
        {
            if (ModelState.IsValid)
            {
                if (cover != null)
                {
                    string pic = System.IO.Path.GetFileName(cover.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/media/dvdPoster"), pic);
                    dvd.cover = cover.FileName;
                    cover.SaveAs(path);

                }
                db.Entry(dvd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cast_id = new SelectList(db.cast, "id", "first_name", dvd.cast_id);
            ViewBag.genres_id = new SelectList(db.Genres, "id", "genres1", dvd.genres_id);
            ViewBag.producer_id = new SelectList(db.producer, "id", "first_name", dvd.producer_id);
            ViewBag.studio_id = new SelectList(db.studio, "id", "name", dvd.studio_id);
            return View(dvd);
        }

        // GET: Dvds/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dvd dvd = db.dvd.Find(id);
            if (dvd == null)
            {
                return HttpNotFound();
            }
            return View(dvd);
        }

        // POST: Dvds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            dvd dvd = db.dvd.Find(id);
            db.dvd.Remove(dvd);
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
