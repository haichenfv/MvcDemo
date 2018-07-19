using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using Core.CostFlow.Repositories;
using ProjectBase.Utils.Entitles;
using Core.Filters;

namespace Core.CostFlow
{
    /// <summary>
    /// 运单成本
    /// </summary>
    public class WayBillCost : DomainObject<WayBillCost, int, IWayBillCostRepository>
    {
        #region property
        /// <summary>
        /// 运单号
        /// </summary>
        public virtual string ExpressNo { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public virtual string BatchNO { get; set; }

        /// <summary>
        /// 收寄日期
        /// </summary>
        public virtual DateTime PostingTime { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public virtual decimal Weight { get; set; }

        /// <summary>
        ///寄达地
        /// </summary>
        public virtual string SendAddress { get; set; }

        /// <summary>
        /// 邮件处理费
        /// </summary>
        public virtual decimal ProcessingFee { get; set; }

        /// <summary>
        /// 邮资
        /// </summary>
        public virtual decimal WayBillFee { get; set; }

        /// <summary>
        ///快递类型
        /// </summary>
        public virtual string Product { get; set; }
        #endregion

        public virtual DateTime ReconcileDate { get; set; }//结算月份
        public virtual int PayStatus { get; set; }  //结算状态
        public virtual DateTime PayTime { get; set; } //结算时间

        #region common method
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IPageOfList<WayBillCost> GetByFilter(WayBillCostFilter filter)
        {
            return Dao.GetByFilter(filter);
        }

        public static IList<string> GetProblemLoadBillNum(IList<string> loadBillNums)
        {
            return Dao.GetProblemLoadBillNum(loadBillNums);
        }

        /// <summary>
        /// 根据运单号获取成本详细
        /// </summary>
        /// <param name="ExpressNo"></param>
        /// <returns></returns>
        public static IList<WayBillCost> GetCostByExpressNo(string ExpressNo)
        {
            return Dao.GetCostByExpressNo(ExpressNo);
        }

        public static IList<LoadBillStatistics> GetLoadBillStatistics(List<string> loadBillNum)
        {
            return Dao.GetLoadBillStatistics(loadBillNum);
        }

        public static long GetCountByLoadBillNum(string loadBillNum)
        {
            return Dao.GetCountByLoadBillNum(loadBillNum);
        }
        #endregion

        #region entity method
        #endregion
    }
    public class LoadBillStatistics
    {
        /// <summary>
        /// 提货单号
        /// </summary>
        public string LoadBillNum { get; set; }
        /// <summary>
        /// 操作费
        /// </summary>
        public decimal ProcessingFee { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal WayBillFee { get; set; }
    }
}
