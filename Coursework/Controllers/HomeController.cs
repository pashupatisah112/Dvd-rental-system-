using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coursework.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        EverestDvdEntities db = new EverestDvdEntities();

        //Returns the homepage for member or customer
        public ActionResult Index()
        {
            return View();
        }

        //Returns home page for manager home
        public ActionResult ManagerHome()
        {
            ViewBag.dvd_id = new SelectList(db.dvd, "id", "copy_number");
            return View();
        }

        //Returns hompage for assitant
        public ActionResult AssistantHome()
        {
            return View();
        }
    }
}