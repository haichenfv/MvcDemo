using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using Core.Customer;
using Core.Receivable.Repositories;
using ProjectBase.Utils.Entitles;
using System.Data;
using NPOI;
using Core.Filters;
using ProjectBase.Utils;

namespace Core.Receivable
{
    public class LoadBillInCome : DomainObject<LoadBillInCome, int, ILoadBillInComeRepository>
    {
        #region property
        /// <summary>
        /// 提货单号
        /// </summary>
        public virtual string LoadBillNum { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual CustomerInfo CustomerInfoBy { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public virtual int BussinessType { get; set; }
        /// <summary>
        /// 结算状态
        /// </summary>
        public virtual PayStatus PayStatus { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public virtual DateTime PayTime { get; set; }
        /// <summary>
        /// 仓租
        /// </summary>
        public virtual decimal StoreFee { get; set; }
        /// <summary>
        /// 提货费
        /// </summary>
        public virtual decimal LoadFee { get; set; }
        /// <summary>
        /// 业务系统时间
        /// </summary>
        public virtual DateTime BusinessTime { get; set; }
        /// <summary>
        /// 运费总额
        /// </summary>
        public virtual decimal TotalCollectFees { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public virtual int CompanyID { get; set; }
        /// <summary>
        /// 收货商ID
        /// </summary>
        public virtual int DeliveryID { get; set; }
        /// <summary>
        /// 操作费总额
        /// </summary>
        public virtual decimal TotalOperateFee { get; set; }
        /// <summary>
        /// 其他费用
        /// </summary>
        public virtual decimal OtherFee { get; set; }
        /// <summary>
        /// 其他费用备注
        /// </summary>
        public virtual string FeeRemark { get; set; }
        /// <summary>
        /// 提单状态
        /// </summary>
        public virtual BillStatusEnum BillStatus { get; set; }
        /// <summary>
        /// 清关完成时间
        /// </summary>
        public virtual DateTime CompletionTime { get; set; }
        /// <summary>
        /// 应收总金额
        /// </summary>
        public virtual decimal TotalReceivableFee { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public virtual int BusinessID { get; set; }
        /// <summary>
        /// 提单重量(KG)
        /// </summary>
        public virtual float BillWeight { get; set; }
        /// <summary>
        /// 包裹总数
        /// </summary>
        public virtual int OrderCounts { get; set; }
        /// <summary>
        /// 月结时间
        /// </summary>
        public virtual DateTime MonthPayTime { get; set; }
        /// <summary>
        /// 是否已经添加到月结
        /// </summary>
        public virtual bool IsAddMonthPayOff { get; set; }
        /// <summary>
        /// 仓租产生原因
        /// </summary>
        public virtual string StorageFeeReason { get; set; }
        /// <summary>
        /// 提货日期
        /// </summary>
        public virtual DateTime LoadTime { get; set; }
        /// <summary>
        /// 到货时间
        /// </summary>
        public virtual DateTime DeliveryTime { get; set; }
        #endregion

        #region common method
        /// <summary>
        /// 分页获取提单数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IPageOfList<LoadBillInCome> GetByFilter(BillOfLadingFeeFilter filter)
        {
            return Dao.GetByFilter(filter);
        }

        public static LoadBillInCome GetByLoadBillNum(string loadBillNum)
        {
            return Dao.GetByLoadBillNum(loadBillNum);
        }

        public static IList<LoadBillInCome> GetByLoadBillNums(IEnumerable<string> loadBillNums)
        {
            return Dao.GetByLoadBillNums(loadBillNums);
        }

        public static bool ImportFee(System.IO.Stream s, string fileName, out string message)
        {
            bool result = true;
            message = string.Empty;
            int updateCount = 0;
            DataTable dt = NPOIHelper.ExcelToDataSet(fileName, s, new int[] { 0 }, true).Tables[0];
            if (!dt.Columns.Contains("提单号") || !dt.Columns.Contains("其他费用") || !dt.Columns.Contains("备注"))
            {
                message = "请导入包含“提单号”,“其他费用”,“备注”正确的模板";
                return false;
            }
            IList<LoadBillInCome> li = GetByLoadBillNums(dt.AsEnumerable().Select(a => a["提单号"].ToString().Trim()).ToList<string>());
            foreach (DataRow item in dt.Rows)
            {
                decimal otherFee;
                LoadBillInCome i = li.Where(a => a.LoadBillNum == item["提单号"].ToString().Trim()).FirstOrDefault();
                if (i != null && decimal.TryParse(item["其他费用"].ToString().Trim(), out otherFee))
                {
                    i.OtherFee = otherFee;
                    i.FeeRemark = item["备注"].ToString().Trim();
                    i.Update();
                    updateCount++;
                }
            }
            message = string.Format("导入excel共{0}条提单,成功更新{1}", dt.Rows.Count, updateCount);
            return result;
        }

        public static IList<string> GetProblemLoadBillCost(IList<string> loadBillNums)
        {
            return Dao.GetProblemLoadBillCost(loadBillNums);
        }

        public static bool ReplaceMonthPay(bool isNeglect, MonthPayOffData data, DateTime MonthPayTime, out string message)
        {
            message = string.Empty;
            #region 错误提示
            var UnsomePayOffMonths = MonthPayOffDetail.GetPayOffMonth(new List<int>(new int[] { data.LoadBillBy.ID })).FirstOrDefault();
            if (UnsomePayOffMonths.Status == MonthlyBalanceStatus.Locked)
            {
                message = string.Format("月结“{0}”已锁定", UnsomePayOffMonths.PayOffMonth.ToString("yyyyMMdd"));
                return false;
            }
            var monthPayOff = MonthPayOff.GetByPayOffMonth(MonthPayTime).FirstOrDefault();
            if (monthPayOff != null && monthPayOff.Status == MonthlyBalanceStatus.Locked)
            {
                message = string.Format("异常:{0}已锁定,不可添加提单数据", MonthPayTime.ToString("yyyyMMdd"));
                return false;
            }
            #endregion
            if (!isNeglect)
            {
                #region 提醒
                var problemLoadBill = LoadBillInCome.GetProblemLoadBillCost(new List<string>(new string[] { data.LoadBillBy.LoadBillNum }));
                if (problemLoadBill.Count > 0)
                {
                    message = string.Format("提醒:提单集合中存在异常提单:<br/>“{0}”", string.Join("”,<br/>“", problemLoadBill));
                    return false;
                }
                problemLoadBill = Core.CostFlow.WayBillCost.GetProblemLoadBillNum(new List<string>(new string[] { data.LoadBillBy.LoadBillNum }));
                if (problemLoadBill.Count > 0)
                {
                    message = string.Format("提醒:提单集合中存在运单异常的异常提单:<br/>“{0}”<br/>", string.Join("”,<br/>“", problemLoadBill));
                    return false;
                }
                if (UnsomePayOffMonths.PayOffMonth == MonthPayTime)
                {
                    message = string.Format("提单“{0}”已在当前月份，是否确定更新", data.LoadBillBy.LoadBillNum);
                    return false;
                }
                #endregion
            }
            if (monthPayOff == null)
            {
                monthPayOff = new MonthPayOff() { };
            }
            List<MonthPayOffData> li = new List<MonthPayOffData>();
            li.Add(data);
            monthPayOff.InsertLoadBill(MonthPayTime, li);
            return true;
        }
        #endregion

        #region entity method
        #endregion
    }
}
