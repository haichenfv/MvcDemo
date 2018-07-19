using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using System.ComponentModel;
using Core.Receivable.Repositories;
using ProjectBase.Utils.Entitles;
using Core.Filters;
using Core.CostFlow;
using Core.Reconciliation;
using Core.Excel;

namespace Core.Receivable
{
    public class MonthPayOff : DomainObject<MonthPayOff, int, IMonthPayOffRepository>
    {
        #region property
        /// <summary>
        /// 结算月份
        /// </summary>
        public virtual DateTime PayOffMonth { get; set; }
        /// <summary>
        /// 提单总数
        /// </summary>
        public virtual Int64 LoadBillCounts { get; set; }
        /// <summary>
        /// 包裹总数
        /// </summary>
        public virtual Int64 OrderCounts { get; set; }
        /// <summary>
        /// 预计总成本
        /// </summary>
        public virtual decimal PreTotalCostFee { get; set; }
        /// <summary>
        /// 真实总成本
        /// </summary>
        public virtual decimal TotalCostFee { get; set; }
        /// <summary>
        /// 预计总收入
        /// </summary>
        public virtual decimal PreInComeFee { get; set; }
        /// <summary>
        /// 真实总收入
        /// </summary>
        public virtual decimal InComeFee { get; set; }
        /// <summary>
        /// 总毛利
        /// </summary>
        public virtual decimal TotalMargin { get; set; }
        /// <summary>
        /// 毛利率
        /// </summary>
        public virtual decimal MarginRate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual MonthlyBalanceStatus Status { get; set; }
        /// <summary>
        /// 月结明细表
        /// </summary>
        public virtual IList<MonthPayOffDetail> Items { get; set; }
        #endregion

        #region common method
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IPageOfList<MonthPayOff> GetByFilter(MonthPayOffFilter filter)
        {
            return Dao.GetByFilter(filter);
        }

        public static IList<MonthPayOff> GetByPayOffMonth(DateTime dt)
        {
            return Dao.GetByPayOffMonth(dt);
        }

        public static IPageOfList<MonthPayOff> GetById(MonthPayOffExportFilter filter)
        {
            return Dao.GetByFilter(filter);
        }
        #endregion

        #region entity method
        /// <summary>
        /// 提单成本添加月结
        /// </summary>
        /// <param name="isNeglect"></param>
        /// <param name="data"></param>
        public static bool AddMonthPay(bool isNeglect, IList<MonthPayOffData> data, DateTime MonthPayTime, out string message)
        {
            message = string.Empty;
            #region 错误提示
            var monthPayOff = MonthPayOff.GetByPayOffMonth(MonthPayTime).FirstOrDefault();
            if (monthPayOff != null && monthPayOff.Status == MonthlyBalanceStatus.Locked)
            {
                message = string.Format("异常:{0}已锁定,不可添加提单数据", MonthPayTime.ToString("yyyyMMdd"));
                return false;
            }
            var errorLoadBill = data.Where(a => a.LoadBillBy.IsAddMonthPayOff == true).Select(a => a.LoadBillBy.LoadBillNum);
            if (errorLoadBill.Count() > 0)
            {
                message = string.Format("异常:以下提单已经添加月结了:“{0}”", string.Join("”,<br/>“", errorLoadBill));
                return false;
            }
            //var UnsomePayOffMonths = MonthPayOffDetail.GetPayOffMonth(data.Select(a => a.LoadBillBy.ID).ToList()).Where(a => a.PayOffMonth.ToString("yyyyMMdd") != MonthPayTime.ToString("yyyyMMdd")).Select(a => a.PayOffMonth.ToString("yyyyMMdd"));
            //if (UnsomePayOffMonths.Count() > 0)
            //{
            //    message = string.Format("异常:提单集合中已经有数据添加至“{0}”月结数据中", string.Join("”,“", UnsomePayOffMonths));
            //    return false;
            //}
            #endregion
            if (!isNeglect)
            {
                #region 提醒
                var problemLoadBill = LoadBillInCome.GetProblemLoadBillCost(data.Select(a => a.LoadBillBy.LoadBillNum).ToList());
                if (problemLoadBill.Count > 0)
                {
                    message = string.Format("提醒:提单集合中存在异常提单:<br/>“{0}”", string.Join("”,<br/>“", problemLoadBill));
                    return false;
                }
                problemLoadBill = WayBillCost.GetProblemLoadBillNum(data.Select(a => a.LoadBillBy.LoadBillNum).ToList());
                if (problemLoadBill.Count > 0)
                {
                    message = string.Format("提醒:提单集合中存在运单异常的异常提单:<br/>“{0}”<br/>", string.Join("”,<br/>“", problemLoadBill));
                    return false;
                }
                #endregion
            }
            if (monthPayOff == null)
            {
                monthPayOff = new MonthPayOff();
            }
            monthPayOff.InsertLoadBill(MonthPayTime, data);
            return true;
        }
        #endregion

        #region entity method
        public virtual void InsertLoadBill(DateTime MonthPayTime, IList<MonthPayOffData> data)
        {
            using (var tran = Dao.BeginTransaction())
            {
                try
                {
                    if (this.ID == 0)
                    {
                        List<MonthPayOffDetail> li = new List<MonthPayOffDetail>();
                        PayOffMonth = MonthPayTime;
                        Status = MonthlyBalanceStatus.WaitLocking;
                        foreach (var item in data)
                        {
                            MonthPayOffDetail de = MonthPayOffDetail.GetByLoadBillID(item.LoadBillBy.ID);
                            if (de == null)
                            {
                                li.Add(new MonthPayOffDetail()
                                {
                                    PreTotalCostFee = item.PreTotalCostFee,
                                    TotalCostFee = item.TotalCostFee,
                                    PreInComeFee = item.PreInComeFee,
                                    InComeFee = item.InComeFee,
                                    TotalMargin = item.TotalMargin,
                                    CreateTime = DateTime.Now,
                                    LoadBillBy = item.LoadBillBy,
                                    MonthPayOffBy = this
                                });
                            }
                            else
                            {
                                de.MonthPayOffBy = this;
                                de.PreTotalCostFee = item.PreTotalCostFee;
                                de.TotalCostFee = item.TotalCostFee;
                                de.PreInComeFee = item.PreInComeFee;
                                de.InComeFee = item.InComeFee;
                                de.TotalMargin = item.TotalMargin;
                                de.CreateTime = DateTime.Now;
                                de.LoadBillBy = item.LoadBillBy;
                                li.Add(de);
                            }
                            item.LoadBillBy.IsAddMonthPayOff = true;
                            item.LoadBillBy.Update();
                        }
                        Items = li;
                        this.Save();
                    }
                    else
                    {
                        foreach (var item in data)
                        {
                            MonthPayOffDetail monthPayOffDetail = MonthPayOffDetail.GetByLoadBillID(item.LoadBillBy.ID);
                            if (monthPayOffDetail != null)
                            {
                                monthPayOffDetail.PreTotalCostFee = item.PreTotalCostFee;
                                monthPayOffDetail.TotalCostFee = item.TotalCostFee;
                                monthPayOffDetail.PreInComeFee = item.PreInComeFee;
                                monthPayOffDetail.InComeFee = item.InComeFee;
                                monthPayOffDetail.TotalMargin = item.TotalMargin;
                                monthPayOffDetail.CreateTime = DateTime.Now;
                                monthPayOffDetail.MonthPayOffBy = this;
                            }
                            else
                            {
                                monthPayOffDetail = new MonthPayOffDetail();
                                monthPayOffDetail.PreTotalCostFee = item.PreTotalCostFee;
                                monthPayOffDetail.TotalCostFee = item.TotalCostFee;
                                monthPayOffDetail.PreInComeFee = item.PreInComeFee;
                                monthPayOffDetail.InComeFee = item.InComeFee;
                                monthPayOffDetail.TotalMargin = item.TotalMargin;
                                monthPayOffDetail.CreateTime = DateTime.Now;
                                monthPayOffDetail.LoadBillBy = item.LoadBillBy;
                                monthPayOffDetail.MonthPayOffBy = this;
                                this.Items.Add(monthPayOffDetail);
                            }
                            item.LoadBillBy.IsAddMonthPayOff = true;
                            item.LoadBillBy.Update();
                        }
                        this.Update();
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        #endregion

        public static void ExportExcel(string excelPath, MonthPayOffExportFilter filter)
        {
            var dataSource = Core.Receivable.MonthPayOff.GetById(filter);
            List<MonthPayOff> queryData = dataSource.ToList();

            var loadBillData = LoadBillReconciliation.GetByMonthPayOffExportFilter(new LBRForMonthPayOffExportFilter() { MonthPayOffIDList = filter.ListID });
            List<LoadBillReconciliation> loadBillList = loadBillData.ToList();

            GenerateExcel genExcel = new GenerateExcel();
            genExcel.SheetList.Add(new MonthPayOffSheet(queryData, "总表"));
            genExcel.SheetList.Add(new LoadBillSheet(loadBillList, "提单汇总"));
            genExcel.SheetList.Add(new WayBillSummarySheet(new List<WayBillReconciliation>(), "运单汇总"));
            genExcel.ExportExcel(excelPath);
        }
    }
    public class MonthPayOffData
    {
        /// <summary>
        /// 月结时间
        /// </summary>
        public DateTime MonthPayTime { get; set; }
        /// <summary>
        /// 对账时间
        /// </summary>
        public DateTime ReconcileTime { get; set; }
        /// <summary>
        /// 预计总成本
        /// </summary>
        public decimal PreTotalCostFee { get; set; }
        /// <summary>
        /// 真实总成本
        /// </summary>
        public decimal TotalCostFee { get; set; }
        /// <summary>
        /// 预计总收入
        /// </summary>
        public decimal PreInComeFee { get; set; }
        /// <summary>
        /// 真实总收入
        /// </summary>
        public decimal InComeFee { get; set; }
        /// <summary>
        /// 总毛利
        /// </summary>
        public decimal TotalMargin { get; set; }
        /// <summary>
        /// 提单信息
        /// </summary>
        public LoadBillInCome LoadBillBy { get; set; }
    }

    /// <summary>
    /// 月结状态
    /// </summary>
    public enum MonthlyBalanceStatus
    {
        /// <summary>
        /// 待锁定
        /// </summary>
        [Description("待锁定")]
        WaitLocking = 0,
        /// <summary>
        /// 已锁定
        /// </summary>
        [Description("已锁定")]
        Locked = 1
    }
}
