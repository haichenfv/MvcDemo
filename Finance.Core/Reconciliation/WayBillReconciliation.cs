using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using Core.Reconciliation.Repositories;
using ProjectBase.Utils.Entitles;

namespace Core.Reconciliation
{
    ///// <summary>
    ///// 结算状态
    ///// </summary>
    //public enum PayStatus
    //{
    //    /// <summary>
    //    /// 待付款
    //    /// </summary>
    //    [Description("待付款")]
    //    UnPaid=0,
    //    /// <summary>
    //    /// 已付款
    //    /// </summary>
    //   [Description("已付款")]
    //    Paid=1

    //}
    public class WayBillReconciliation : DomainObject<WayBillReconciliation, int, IWayBillReconciliationRepository>
    {
        #region property
        /// <summary>
        /// 收寄日期
        /// </summary>
        public DateTime? PostingTime { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public string ExpressNo { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public string LoadBillNum { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CusName { get; set; }
        /// <summary>
        /// 邮政邮资
        /// </summary>
        public decimal WayBillFee { get; set; }
        /// <summary>
        /// 邮政邮件处理费
        /// </summary>
        public decimal ProcessingFee { get; set; }
        /// <summary>
        /// 客户运费
        /// </summary>
        public decimal ExpressFee { get; set; }
        /// <summary>
        /// 客户操作费
        /// </summary>
        public decimal OperateFee { get; set; }
        /// <summary>
        /// 运费毛利
        /// </summary>
        public decimal WayBillProfit { get; set; }

        /// <summary>
        /// 总毛利
        /// </summary>
        public decimal TotalProfit { get; set; }

        /// <summary>
        /// 对账日期
        /// </summary>
        public DateTime ReconcileDate { get; set; }

        /// <summary>
        /// 其他成本费
        /// </summary>
        public decimal CostOtherFee { get; set; }

        /// <summary>
        /// 其他收入费
        /// </summary>
        public decimal InComeOtherFee { get; set; }

        /// <summary>
        /// 总收入
        /// </summary>
        public decimal? InComeTotalFee { get; set; }

        /// <summary>
        /// 总成本费
        /// </summary>
        public decimal? CostTotalFee { get; set; }

        /// <summary>
        /// 成本状态
        /// </summary>
        public PayStatus CostStatus { get; set; }

        /// <summary>
        /// 收入状态
        /// </summary>
        public PayStatus InComeStatus { get; set; }

        /// <summary>
        /// 对账单状态
        /// </summary>
        public string Statement { get; set; }

        /// <summary>
        /// 是否已导入成本
        /// </summary>
        public int IsInputCost { get; set; }

        /// <summary>
        /// 清关完成时间
        /// </summary>
        public virtual DateTime CompletionTime { get; set; }

        /// <summary>
        /// 收件人省份
        /// </summary>
        public virtual string ReceiverProvince { get; set; }

        /// <summary>
        /// 快递类型
        /// </summary>
        public virtual ExpressTypeEnum ExpressTypeget { get; set; }
        #endregion

        #region common method
        public static IPageOfList<WayBillReconciliation> GetByFilter(ParameterFilter filter)
        {
            return Dao.GetByFilter(filter);
        }

        /// <summary>
        /// 根据运单号获取成本详细
        /// </summary>
        /// <param name="ExpressNo"></param>
        /// <returns></returns>
        public static IList<WayBillReconciliation> GetWayBillInComeByExpressNo(string ExpressNo)
        {
            return Dao.GetWayBillInComeByExpressNo(ExpressNo);
        }
        #endregion
    }
}
