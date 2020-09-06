using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Coursework.Controllers
{
    public class AccountController : Controller
    {
        EverestDvdEntities db = new EverestDvdEntities();

        // GET: Account Login page
        public ActionResult Login()
        {
            return View();
        }

        //Checks post login request to identify user
        [HttpPost]
        public ActionResult Login(member model)
        {
            using (var context = new EverestDvdEntities())
            {
                bool isValid = context.member.Any(x => x.email_address == model.email_address && x.password == model.password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(model.email_address, false);
                    IEnumerable<string> emp = from mem in context.member
                                              join role in context.roles on mem.role_Id equals role.id
                                              where mem.email_address==model.email_address
                                              select role.role;
                    foreach(string output in emp)
                    {
                        if (output == "Manager")
                        {
                            return RedirectToAction("ManagerHome", "Home");
                        }
                        else if (output == "Assistant")
                        {
                            return RedirectToAction("AssistantHome", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }    
                }

                ModelState.AddModelError("", "Invalid email or password");
                return View();
            }
        }

        //Log out the user
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Changepwd()
        {
            return View();
        }

        //Change user password
        [HttpPost]
        public ActionResult Changepwd(string password)
        {
            var pwd = db.member.Where(m => m.password == User.Identity.Name).ToList();
            foreach(var item in pwd)
            {
                if (password == item.password)
                {
                    return RedirectToAction("Repassword");
                }
                else
                {
                    ModelState.AddModelError("", "Password did not match");
                }
            }
            return View();
        }

        //Confirm the password change
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmChangepwd([Bind(Include = "password")] member member)
        {
            var pwd = db.member.Where(m => m.password == User.Identity.Name).ToList();
            if (ModelState.IsValid)
            {
          
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        
            return View();
        }
    }
}