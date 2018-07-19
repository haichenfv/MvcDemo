using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Filters;
using Core.Receivable;
using ProjectBase.Utils;
using System.IO;
using Core.Reconciliation;
using ProjectBase.Utils.Entitles;

namespace MvcDemo0516.Controllers
{
    [Authorize]
    public class StatisticalController : Controller
    {
        #region 月结表
        public ActionResult MonthPayOff()
        {
            return View();
        }

        public JsonResult MonthPayOffList(MonthPayOffFilter filter)
        {
            filter.PageSize = int.MaxValue;
            var dataSource = Core.Receivable.MonthPayOff.GetByFilter(filter);

            List<MonthPayOff> queryData = dataSource.ToList();

            int i = 0;
            var data = queryData.Select(u => new
            {
                Index = ++i, //行号
                ID = u.ID,
                PayOffMonth = u.PayOffMonth.ToStringMonth(),
                LoadBillCounts = u.LoadBillCounts,
                OrderCounts = u.OrderCounts,
                PreTotalCostFee = u.PreTotalCostFee,
                TotalCostFee = u.TotalCostFee,
                PreInComeFee = u.PreInComeFee,
                InComeFee = u.InComeFee,
                TotalMargin = u.TotalMargin,
                MarginRate = Math.Round((u.MarginRate) * 100, 2),
                Remark = u.Remark,
                Status = u.Status.GetDescription(false)
            });

            //构造成Json的格式传递
            var result = new { iTotalRecords = queryData.Count, iTotalDisplayRecords = 10, data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 锁定月结表操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult MonthPayOffLockStatus(string id)
        {
            //string result = null;
            try
            {
                var model = Core.Receivable.MonthPayOff.Load(Convert.ToInt32(id));
                model.Status = MonthlyBalanceStatus.Locked;
                model.Update();
                return Json("操作成功", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("操作失败" + ex, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult MonthPayOffDetail(int id)
        {
            return View(Core.Receivable.MonthPayOff.GetByFilter(new MonthPayOffFilter() { ID = id, PageIndex = 0, PageSize = 1 }).ToList().FirstOrDefault());
        }

        /// <summary>
        /// 修改月结表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isNeglect">是否忽视异常</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditMonthPayOff(Models.MonthPayOffModel data, bool isNeglect, DateTime payOffMonth)
        {
            string message = string.Empty;
            var dealdata =new MonthPayOffData()
            {
                MonthPayTime = payOffMonth,
                ReconcileTime = data.ReconcileTime,
                PreTotalCostFee = data.PreTotalCostFee,
                TotalCostFee = data.TotalCostFee,
                PreInComeFee = data.PreInComeFee,
                InComeFee = data.InComeFee,
                TotalMargin = data.TotalMargin,
                LoadBillBy = LoadBillInCome.GetByLoadBillNum(data.LoadBillNum)
            };
            if (Core.Receivable.LoadBillInCome.ReplaceMonthPay(isNeglect, dealdata, payOffMonth, out message))
            {
                return Json(new { IsSuccess = true, Message = string.Format("提单号“{0}”修改完毕",data.LoadBillNum) });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = message, IsPoint = message.Substring(0, 2) == "提醒" ? true : false });
            }
        }

        [HttpPost]
        public JsonResult LoadBillList(LBRForMonthPayOffFilter filter)
        {
            DataTablesRequest parm = new DataTablesRequest(this.Request);    //处理对象
            int pageIndex = parm.iDisplayLength == 0 ? 0 : parm.iDisplayStart / parm.iDisplayLength;
            filter.PageIndex = pageIndex;    //页索引
            filter.PageSize = parm.iDisplayLength;    //页行数
            var DataSource = LoadBillReconciliation.GetByMonthPayOffFilter(filter);
            int i = parm.iDisplayLength * pageIndex;
            List<LoadBillReconciliation> queryData = DataSource.ToList();
            var data = queryData.Select(u => new
            {
                Index = ++i, //行号
                ID = u.ID,
                CusName = u.CusName,//客户简称
                LoadBillNum = u.LoadBillNum,//提单号
                FeeWeight = u.FeeWeight,//提单重量
                ExpressCount = u.ExpressCount,//提单包裹数量
                CompletionTime = u.CompletionTime.ToStringDate(),//清关完成时间
                GroundHandlingFee = u.GroundHandlingFee,//邮政地勤费
                CostStoreFee = u.CostStoreFee,//邮政仓租
                CostExpressFee = u.CostExpressFee,//邮政邮资
                CostOperateFee = u.CostOperateFee,//邮件处理费
                CostOtherFee = u.CostOtherFee,//邮政其他费用
                CostTotalFee = u.GroundHandlingFee + u.CostStoreFee + u.CostExpressFee + u.CostOperateFee + u.CostOtherFee,//邮政总成本
                CostStatus = u.CostStatus.ToChinese(),//邮政结算状态
                InComeLoadFee = u.InComeLoadFee,//客户提货费
                InComeStoreFee = u.InComeStoreFee,//客户仓租
                InComeExpressFee = u.InComeExpressFee,//客户运费
                InComeOperateFee = u.InComeOperateFee,//客户操作费
                InComeOtherFee = u.InComeOtherFee,//其他费用
                InComeTotalFee = u.InComeLoadFee + u.InComeStoreFee + u.InComeExpressFee + u.InComeOperateFee + u.InComeOtherFee,//总收入
                InComeStatus = u.InComeStatus.ToChinese(),//结算状态
                TotalGrossProfit = (u.InComeLoadFee + u.InComeStoreFee + u.InComeExpressFee + u.InComeOperateFee + u.InComeOtherFee) - (u.GroundHandlingFee + u.CostStoreFee + u.CostExpressFee + u.CostOperateFee + u.CostOtherFee),//总毛利
                GrossProfitRate = Math.Round((u.InComeLoadFee + u.InComeStoreFee + u.InComeExpressFee + u.InComeOperateFee + u.InComeOtherFee) == 0 ? 0 : (1 - (u.GroundHandlingFee + u.CostStoreFee + u.CostExpressFee + u.CostOperateFee + u.CostOtherFee) / (u.InComeLoadFee + u.InComeStoreFee + u.InComeExpressFee + u.InComeOperateFee + u.InComeOtherFee)) * 100, 2),//毛利率
                Status = u.Status,
                IsReal = u.IsReal,
                ExpressWeight = u.ExpressWeight,
                IsAddMonthPayOff = u.IsAddMonthPayOff,
                ReconcileDate = u.ReconcileDate.ToStringDate()
            });
            //构造成Json的格式传递
            var result = new
            {
                iTotalRecords = DataSource.Count,
                iTotalDisplayRecords = DataSource.RecordTotal,
                data = data
            };
            return Json(result);
        }

        public JsonResult ExportExcel(MonthPayOffExportFilter filter)
        {
            string excelPath = this.Server.MapPath(string.Format("/Excel/月结表_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss") ));
            Core.Receivable.MonthPayOff.ExportExcel(excelPath, filter);

            var result = new { IsSuccess=true,Message="成功"};

            return Json(result);
        }

        /// <summary>
        /// 已生成的月结表列表
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadExcelList()
        {
            string myDir = Server.MapPath("~/Excel");

            if (Directory.Exists(Server.MapPath("~/Excel")) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(Server.MapPath("~/Excel"));
            }
            DirectoryInfo dirInfo = new DirectoryInfo(myDir);
            List<LinkEntity> list = LinkEntityExt.ForFileLength(dirInfo);

            return View(list);
        }

        #endregion
    }

}
