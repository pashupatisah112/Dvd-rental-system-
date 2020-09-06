using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coursework.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        EverestDvdEntities context = new EverestDvdEntities();

        // GET: Search using actor's or cast's last name
        [HttpPost]
        public ActionResult ActorSearch(string last_name,string stock)
        {
            if(stock == "false")
            {
                var dvd = (from d in context.dvd
                           join c in context.cast on d.cast_id equals c.id
                           where c.last_name == last_name
                           select c).ToList();
                ViewBag.result = dvd;
            }
            else
            {
                var dvd = (from d in context.dvd
                           join c in context.cast on d.cast_id equals c.id
                           where c.last_name == last_name && d.stock >0 
                           select c).ToList();
                ViewBag.result = dvd;
            }


            return RedirectToAction("Index", "Dvds");
        }

        //Search for the member loans in last 31 days
        [HttpPost]
        public ActionResult MemberDvdLoanSearch(string last_name)
        {

            DateTime dtTo = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
           
            var dvd = (from l in context.loan
                       join d in context.dvd on l.dvd_id equals d.id 
                       join m in context.member on l.member_id equals m.id 
                       join s in context.loan_status on l.loan_status_id equals s.id
                       where m.last_name == last_name && l.date_out >= System.Data.Entity.DbFunctions.AddDays(dtTo, -31)
                       select d).ToList();
            ViewBag.result = dvd;

            return RedirectToAction("Index", "Dvds");
        }

        //Gets the details of a dvd copy on loan and its status
        [HttpPost]
        public ActionResult CopyNumberDetailSearch(string copy_number)
        {
            var copy_detail =(from l in context.loan
                             join m in context.member on l.member_id equals m.id
                             join d in context.dvd on l.dvd_id equals d.id
                             join s in context.loan_status on l.loan_status_id equals s.id
                             where d.copy_number==copy_number orderby l.date_out select l
                             ).ToList().Last();
            ViewBag.copy_detail = copy_detail;
            return View();
        }
        public ActionResult MemberActorSearch(string last_name, string stock)
        {
            if (stock == "false")
            {
                var dvd = (from d in context.dvd
                           join c in context.cast on d.cast_id equals c.id
                           where c.last_name == last_name
                           select c).ToList();
                ViewBag.result = dvd;
            }
            else
            {
                var dvd = (from d in context.dvd
                           join c in context.cast on d.cast_id equals c.id
                           where c.last_name == last_name && d.stock > 0
                           select c).ToList();
                ViewBag.result = dvd;
            }


            return View();
        }
        public ActionResult MemberDvdLoan(string last_name)
        {
            
            DateTime dtTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var dvd = (from l in context.loan
                       join d in context.dvd on l.dvd_id equals d.id
                       join m in context.member on l.member_id equals m.id
                       join s in context.loan_status on l.loan_status_id equals s.id
                       where m.last_name == last_name && l.date_out >= System.Data.Entity.DbFunctions.AddDays(dtTo, -31)
                       select d).ToList();
            ViewBag.result = dvd;

            return View();
        }
    }
}