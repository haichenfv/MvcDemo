using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Filters;
using ProjectBase.Utils;
using Core.Reconciliation;
using Data.Reconciliation;
using ProjectBase.Utils.Entitles;

namespace MvcDemo0516.Controllers
{
    [Authorize]
    public class ReconciliationController : Controller
    {
        private string message = "<script>frameElement.api.opener.hidePublishWin('{0}', '{1}','{2}'); </script>"; //消息，是否关闭弹出窗，是否停留在当前分页（0，1）
        Dictionary<int, string> dicSort = new Dictionary<int, string>(); //排序字段键值对列表 （列序号，列名称）

        //运单对账
        public ActionResult WayBill(WayBillReconciliationFilter filter)
        {
            //ViewBag.ExceptionCount = Core.Reconciliation.WayBillException.GetExceptionCount(filter);

            return View();
        }

        [HttpPost]
        public JsonResult WayBillList(WayBillReconciliationFilter filter)
        {
            WayBillExceptionFilter exceptionfilter = new WayBillExceptionFilter() { CusShortName = filter.CusShortName, LoadBillNum = filter.LoadBillNum, ExpressNo = filter.ExpressNo, PostingTime = filter.PostingTime, PostingTimeTo = filter.PostingTimeTo };
            long counts = Core.Reconciliation.WayBillException.GetExceptionCount(exceptionfilter);
            //
            DataTablesRequest parm = new DataTablesRequest(this.Request);    //处理对象
            int pageIndex = parm.iDisplayLength == 0 ? 0 : parm.iDisplayStart / parm.iDisplayLength;
            filter.PageIndex = pageIndex;    //页索引
            filter.PageSize = parm.iDisplayLength;    //页行数
            var DataSource = WayBillReconciliation.GetByFilter(filter) as WRPageOfList<WayBillReconciliation>;

            int i = parm.iDisplayLength * pageIndex;

            List<WayBillReconciliation> queryData = DataSource.ToList();
            var data = queryData.Select(u => new
            {
                Index = ++i, //行号
                ID = u.ID,
                IsInputCost = u.IsInputCost,
                CusName = u.CusName, //客户简称
                PostingTime = u.PostingTime == null ? string.Empty : u.PostingTime.Value.ToStringDate(),//收寄日期
                ExpressNo = u.ExpressNo, //运单号
                BatchNO = u.LoadBillNum, //提单号
                Weight = u.Weight == null ? 0m : u.Weight / 1000, //重量
                WayBillFee = u.WayBillFee, //邮资
                ProcessingFee = u.ProcessingFee, //邮政邮件处理费
                InComeWayBillFee = u.ExpressFee, //客户运费
                InComeOprateFee = u.OperateFee, //客户操作费
                WayBillMargins = u.WayBillProfit, //运费毛利
                TotalMargins = u.ExpressFee + u.OperateFee + u.InComeOtherFee - (u.WayBillFee + u.ProcessingFee + u.CostOtherFee), //总毛利
                Margin = Math.Round((u.ExpressFee + u.OperateFee + u.InComeOtherFee == 0 ? 0m : (u.ExpressFee + u.OperateFee + u.InComeOtherFee - (u.WayBillFee + u.ProcessingFee + u.CostOtherFee)) / (u.ExpressFee + u.OperateFee + u.InComeOtherFee) * 100), 2), //毛利率 毛利率=(总收入-总的支出的成本)/总收入*100% 
                ReconcileDate = u.ReconcileDate.ToStringDate(), //对账日期
                CostOtherFee = u.CostOtherFee, //成本 其他费用
                CostTotalFee = u.WayBillFee + u.ProcessingFee + u.CostOtherFee, //成本 总费用
                CostStatus = u.CostStatus.ToChinese(),  //成本 状态
                InComeOtherFee = u.InComeOtherFee, //收入 其他费用
                InComeTotalFee = u.ExpressFee + u.OperateFee + u.InComeOtherFee, //收入 总费用
                InComeStatus = u.InComeStatus.ToChinese(),  //收入 状态
                Statement = u.Statement  //对账单状态
            });
            //构造成Json的格式传递
            var result = new
            {
                ExceptionCount = counts,
                iTotalRecords = DataSource.Count,
                iTotalDisplayRecords = DataSource.RecordTotal,
                data = data,
                TotalWeight = DataSource.StatModelBy.TotalWeight / 1000,
                TotalWayBillFee = DataSource.StatModelBy.TotalWayBillFee,
                TotalProcessingFee = DataSource.StatModelBy.TotalProcessingFee,
                TotalExpressFee = DataSource.StatModelBy.TotalExpressFee,
                TotalOperateFee = DataSource.StatModelBy.TotalOperateFee,
                SumWayBillProfit = DataSource.StatModelBy.TotalWayBillProfit,
                SumTotalProfit = 0m //总毛利求和
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 运单异常数据
        /// </summary>
        /// <returns></returns>
        public ActionResult WayBillException(WayBillExceptionFilter filter)
        {
            return View(filter);
        }

        public JsonResult WayBillExceptionList(WayBillExceptionFilter filter)
        {
            dicSort.Add(2, "w.PostingTime");

            DataTablesRequest parm = new DataTablesRequest(this.Request);    //处理对象
            int pageIndex = parm.iDisplayLength == 0 ? 0 : parm.iDisplayStart / parm.iDisplayLength;
            filter.PageIndex = pageIndex;    //页索引
            filter.PageSize = parm.iDisplayLength;    //页行数

            string strSortField = dicSort.Where(x => x.Key == parm.SortColumns[0].Index).Select(x => x.Value).FirstOrDefault();
            string strSortDire = parm.SortColumns[0].Direction == SortDirection.Asc ? "asc" : "desc";

            filter.OrderBy = " " + strSortField + " " + strSortDire;

            var DataSource = Core.Reconciliation.WayBillException.GetByFilter(filter) as WRPageOfList<WayBillException>;

            int i = parm.iDisplayLength * pageIndex;

            List<WayBillException> queryData = DataSource.ToList();
            var data = queryData.Select(u => new
            {
                Index = ++i, //行号
                ID = u.ID,
                IsInputCost = u.IsInputCost,
                CusName = u.CusName, //客户简称
                PostingTime = u.PostingTime == null ? string.Empty : u.PostingTime.Value.ToStringDate(),//收寄日期
                ExpressNo = u.ExpressNo, //运单号
                BatchNO = u.LoadBillNum, //提单号
                Weight = u.Weight == null ? 0m : u.Weight / 1000, //重量
                WayBillFee = u.WayBillFee, //邮资
                ProcessingFee = u.ProcessingFee, //邮政邮件处理费
                InComeWayBillFee = u.ExpressFee, //客户运费
                InComeOprateFee = u.OperateFee, //客户操作费
                WayBillMargins = u.WayBillProfit, //运费毛利
                TotalMargins = u.ExpressFee + u.OperateFee + u.InComeOtherFee - (u.WayBillFee + u.ProcessingFee + u.CostOtherFee), //总毛利
                Margin = Math.Round((u.ExpressFee + u.OperateFee + u.InComeOtherFee == 0 ? 0m : (u.ExpressFee + u.OperateFee + u.InComeOtherFee - (u.WayBillFee + u.ProcessingFee + u.CostOtherFee)) / (u.ExpressFee + u.OperateFee + u.InComeOtherFee) * 100), 2) + "%", //毛利率 毛利率=(总收入-总的支出的成本)/总收入*100% 
                ReconcileDate = u.ReconcileDate.ToStringDate(), //对账日期
                CostOtherFee = u.CostOtherFee, //成本 其他费用
                CostTotalFee = u.WayBillFee + u.ProcessingFee + u.CostOtherFee, //成本 总费用
                CostStatus = u.CostStatus.ToChinese(),  //成本 状态
                InComeOtherFee = u.InComeOtherFee, //收入 其他费用
                InComeTotalFee = u.ExpressFee + u.OperateFee + u.InComeOtherFee, //收入 总费用
                InComeStatus = u.InComeStatus.ToChinese(),  //收入 状态
                ExceptionMsg = u.ExceptionMsg, //运单异常原因
                WayBillCostID = u.WayBillCostID //运单成本ID
                // ExceptionType = u.ExceptionType  //运单异常状态
            });
            //decimal totalProfit = 0m;      //总毛利求和
            //构造成Json的格式传递
            var result = new
            {
                iTotalRecords = DataSource.Count,
                iTotalDisplayRecords = DataSource.RecordTotal,
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult WayBillExceptionDelete(int wayBillCostID, int ID)
        {

            //ViewBag.Title = "删除异常数据";
            //ViewBag.ExpressNo = wayBillCostID;

            //异常数据对象
            var Excmodel = Core.Reconciliation.WayBillException.Load(ID);
            //成本数据对象
            var Costmodel = Core.CostFlow.WayBillCost.Load(wayBillCostID);
            //执行方法
            Core.Reconciliation.WayBillException.DeleteException(wayBillCostID, Excmodel, Costmodel);

            return View();
        }

    }
}
