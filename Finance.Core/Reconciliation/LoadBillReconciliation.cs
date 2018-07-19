using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using ProjectBase.Utils.Entitles;
using Core.Reconciliation.Repositories;
using Core.Filters;

namespace Core.Reconciliation
{
    public class LoadBillReconciliation : DomainObject<LoadBillReconciliation, int, ILoadBillReconciliationRepository>
    {
        #region property
        /// <summary>
        /// 客户简称
        /// </summary>
        public virtual string CusName { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public virtual string LoadBillNum { get; set; }
        /// <summary>
        /// 计费重量（kg）
        /// </summary>
        public virtual decimal FeeWeight { get; set; }
        /// <summary>
        /// 包裹数量
        /// </summary>
        public virtual int ExpressCount { get; set; }
        /// <summary>
        /// 包裹重量
        /// </summary>
        public virtual decimal ExpressWeight { get; set; }
        /// <summary>
        /// 清关完成时间
        /// </summary>
        public virtual DateTime CompletionTime { get; set; }
        /// <summary>
        /// 邮政地勤费(邮政提货费)
        /// </summary>
        public virtual decimal GroundHandlingFee { get; set; }
        /// <summary>
        /// 邮政仓租
        /// </summary>
        public virtual decimal CostStoreFee { get; set; }
        /// <summary>
        /// 邮政邮资
        /// </summary>
        public virtual decimal CostExpressFee { get; set; }
        /// <summary>
        /// 邮件处理费
        /// </summary>
        public virtual decimal CostOperateFee { get; set; }
        /// <summary>
        /// 其他费用
        /// </summary>
        public virtual decimal CostOtherFee { get; set; }
        /// <summary>
        /// 总成本
        /// </summary>
        public virtual decimal CostTotalFee { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual PayStatus CostStatus { get; set; }
        /// <summary>
        /// 客户提货费
        /// </summary>
        public virtual decimal InComeLoadFee { get; set; }
        /// <summary>
        /// 客户仓租
        /// </summary>
        public virtual decimal InComeStoreFee { get; set; }
        /// <summary>
        /// 客户运费
        /// </summary>
        public virtual decimal InComeExpressFee { get; set; }
        /// <summary>
        /// 客户操作费
        /// </summary>
        public virtual decimal InComeOperateFee { get; set; }
        /// <summary>
        /// 其他费用
        /// </summary>
        public virtual decimal InComeOtherFee { get; set; }
        /// <summary>
        /// 总收入
        /// </summary>
        public virtual decimal InComeTotalFee { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual PayStatus InComeStatus { get; set; }
        /// <summary>
        /// 总毛利
        /// </summary>
        public virtual decimal TotalGrossProfit { get; set; }
        /// <summary>
        /// 毛利率
        /// </summary>
        public virtual decimal GrossProfitRate { get; set; }
        /// <summary>
        /// 状态(是否添加到月结表)
        /// </summary>
        public virtual string Status { get; set; }
        /// <summary>
        /// 是否真实成本,0为虚拟,1为真实
        /// </summary>
        public virtual int IsReal { get; set; }
        /// <summary>
        /// 是否已添加到月结表
        /// </summary>
        public virtual Int64 IsAddMonthPayOff { get; set; }
        /// <summary>
        /// 对账日期
        /// </summary>
        public virtual DateTime ReconcileDate { get; set; }
        #endregion

        #region common method
        public static IPageOfList<LoadBillReconciliation> GetByFilter(ParameterFilter filter)
        {
            return Dao.GetByFilter(filter);
        }

        public static LoadBillReconciliation GetByLoadBillNum(string loadBillNum)
        {
            return Dao.GetByLoadBillNum(loadBillNum);
        }

        public static IPageOfList<LoadBillReconciliation> GetByMonthPayOffFilter(ParameterFilter filter)
        {
            return Dao.GetByMonthPayOffFilter(filter);
        }

        public static IPageOfList<LoadBillReconciliation> GetByMonthPayOffExportFilter(LBRForMonthPayOffExportFilter filter)
        {
            return Dao.GetByMonthPayOffFilter(filter);
        }
        #endregion
    }
}
