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
    public class membersController : Controller
    {
        private EverestDvdEntities db = new EverestDvdEntities();

        // GET: members
        public ActionResult Index()
        {
            var member = db.member.Include(m => m.member_category).Include(m => m.roles);
            //var result = db.loan.GroupBy(l => l.member_id);
            var loan = (from l in db.loan
                        group l by l.member_id into copy
                        
                        select new
                        {
                           dvd_copy = copy.Sum(pc => pc.num_copy).ToString(),
                           
                       }).ToList();
            ViewBag.loan = loan;
            return View(member.ToList());
        }


        //Gets the members who has not borrowed any dvd in last 31 days
        public ActionResult LeastActive()
        {
            DateTime dtTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                
            var member = (from l in db.loan
                          join m in db.member on l.member_id equals m.id
                          join d in db.dvd on l.dvd_id equals d.id
                          where l.date_out >= System.Data.Entity.DbFunctions.AddDays(dtTo, 31) orderby l.date_out
                          group l by l.member_id).Last();
                        
            ViewBag.member = member;
            return View();
        }
        // GET: members/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: members/Create
        public ActionResult Create()
        {
            ViewBag.mem_Cat_Id = new SelectList(db.member_category, "id", "category_name");
            ViewBag.role_Id = new SelectList(db.roles, "id", "role");
            return View();
        }

        // POST: members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,first_name,last_name,email_address,address,phone,dob,password,profile,mem_Cat_Id,role_Id")] member member)
        {
            if (ModelState.IsValid)
            {
                db.member.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.mem_Cat_Id = new SelectList(db.member_category, "id", "category_name", member.mem_Cat_Id);
            ViewBag.role_Id = new SelectList(db.roles, "id", "role", member.role_Id);
            return View(member);
        }

        // GET: members/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.mem_Cat_Id = new SelectList(db.member_category, "id", "category_name", member.mem_Cat_Id);
            ViewBag.role_Id = new SelectList(db.roles, "id", "role", member.role_Id);
            return View(member);
        }

        // POST: members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,first_name,last_name,email_address,address,phone,dob,password,profile,mem_Cat_Id,role_Id")] member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.mem_Cat_Id = new SelectList(db.member_category, "id", "category_name", member.mem_Cat_Id);
            ViewBag.role_Id = new SelectList(db.roles, "id", "role", member.role_Id);
            return View(member);
        }

        // GET: members/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.member.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            member member = db.member.Find(id);
            db.member.Remove(member);
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
