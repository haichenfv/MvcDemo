using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.UserModule;
using ProjectBase.Utils.Entitles;

namespace MvcDemo0516.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Message = "123";
            return View();
        }

        public ActionResult Top()
        {
            ViewBag.UserName = UserAccount.GetByAccount(User.Identity.Name).Instance().RealName;
            ViewBag.AvailableBalance = "8888.00";
            return View();
        }

        public ActionResult Left()
        {
            return View();
        }

        public ActionResult Right()
        {
            return View();
        }

        public ActionResult Main()
        {
            return RedirectToAction("MonthPayOff", "Statistical");
        }
    }
}
