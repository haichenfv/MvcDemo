using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDemo0516.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
