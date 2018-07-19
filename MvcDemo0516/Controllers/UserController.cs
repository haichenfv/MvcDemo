using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Filters;
using Core.UserModule;
using System.Web.Security;

namespace MvcDemo0516.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        //
        // GET: /User/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string userName, string passWord, bool? isRemember)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (UserAccount.LoginValid(userName, passWord, out message))
                {
                    FormsAuthenticationTicket ticket;
                    if (isRemember.HasValue && isRemember.Value)
                    {
                        ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), false, "admin");
                    }
                    else
                    {
                        ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddHours(2), false, "admin");
                    }
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Home");
                }
                
                ViewBag.Message = message;
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult LoginOut()
        {
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

    }
}
