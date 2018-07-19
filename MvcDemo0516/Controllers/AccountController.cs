using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Filters;
using Core.UserModule;
using ProjectBase.Utils.Entitles;
using ProjectBase.Utils;

namespace MvcDemo0516.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AccountList(UserAccountFilter filter)
        {
            filter.PageSize = int.MaxValue;
            var dataSource = UserAccount.GetByFilter(filter);
            List<UserAccount> queryData = dataSource.ToList();
            var data = queryData.Select(u => new
            {
                ID = u.ID,
                Account = u.Account,
                PassWord = u.PassWord,
                RealName = u.RealName,
                CreateTime = u.CreateTime.ToStringDateTime(),
                IsAvailable = u.IsAvailable ? "启用" : "禁用"
            });
            var result = new { iTotalRecords = queryData.Count, iTotalDisplayRecords = 10, data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAccount(UserAccount model)
        {
            string message = string.Empty;
            if (model.Insert(out message))
            {
                ViewBag.Msg = string.Format("<script>frameElement.api.opener.hidePublishWin('{0}', '{1}','{2}'); </script>", message, true, "0");
            }
            else
            {
                ViewBag.Msg = string.Format("<script>frameElement.api.opener.hidePublishWin('{0}', '{1}','{2}'); </script>", message, false, "1");
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditAccount(int id)
        {
            return View(UserAccount.Load(id).Instance());
        }

        [HttpPost]
        public ActionResult EditAccount(UserAccount model)
        {
            UserAccount old = UserAccount.Load(model.ID);
            if (old != null)
            {
                old.RealName = model.RealName;
                old.IsAvailable = model.IsAvailable;
                old.Update();
                ViewBag.Msg = string.Format("<script>frameElement.api.opener.hidePublishWin('{0}', '{1}','{2}'); </script>", "成功修改", true, "0");
            }
            return View();
        }
    }
}
