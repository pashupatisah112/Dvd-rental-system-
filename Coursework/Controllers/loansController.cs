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
    public class loansController : Controller
    {
        private EverestDvdEntities db = new EverestDvdEntities();

        // GET: loans
        public ActionResult Index()
        {
            var loan = db.loan.Include(l => l.dvd).Include(l=>l.member).Include(l=>l.loan_status);
            return View(loan.ToList());
        }

        //GET: pending loans
        public ActionResult PendingLoan()
        {
            var pending = db.loan.Include(l => l.dvd).Include(l => l.member).Include(l => l.loan_status).Where(l => l.loan_status_id == 1);
            ViewBag.pending = pending;
            return View();
        }


        // GET: loans/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loan loan = db.loan.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // GET: loans/Create
        public ActionResult Create()
        {
            ViewBag.dvd_id = new SelectList(db.dvd, "id", "copy_number");
            ViewBag.member_id = new SelectList(db.member, "id", "first_name");
            ViewBag.loan_status_id = new SelectList(db.loan_status, "id", "loan_status1");
            return View();
        }

        // POST: loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,date_out,num_copy,amount_paid,due,date_returned,penalty_charge,dvd_id,member_id,loan_status_id")] loan loan,dvd dvd)
        {
            if (ModelState.IsValid)
            {
                db.loan.Add(loan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.dvd_id = new SelectList(db.dvd, "id", "title", loan.dvd_id);
            ViewBag.member_id = new SelectList(db.member, "id", "first_name", loan.member_id);
            ViewBag.loan_status_id = new SelectList(db.loan_status, "id", "loan_status", loan.loan_status_id);

            /*
            db.Entry(dvd).State = EntityState.Modified;
            var dv = db.dvd.Find(loan.dvd_id);
            var result=dv.stock - loan.num_copy;
            dvd dvd = new dvd()
            {
               
            };
            */
   
            //db.SaveChanges();
            return View(loan);
        }

        // GET: loans/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loan loan = db.loan.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.dvd_id = new SelectList(db.dvd, "id", "title", loan.dvd_id);
            ViewBag.member_id = new SelectList(db.member, "id", "first_name",loan.member_id);
            ViewBag.loan_status_id = new SelectList(db.loan_status, "id", "loan_status1",loan.loan_status_id);
            return View(loan);
        }

        // POST: loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,date_out,num_copy,amount_paid,due,date_returned,penalty_charge,dvd_id,member_id,loan_status_id")] loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dvd_id = new SelectList(db.dvd, "id", "title", loan.dvd_id);
            ViewBag.member_id = new SelectList(db.member, "id", "first_name", loan.member_id);
            ViewBag.loan_status_id = new SelectList(db.loan_status, "id", "loan_status", loan.loan_status_id);
            return View(loan);
        }

        // GET: loans/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            loan loan = db.loan.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // POST: loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            loan loan = db.loan.Find(id);
            db.loan.Remove(loan);
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
