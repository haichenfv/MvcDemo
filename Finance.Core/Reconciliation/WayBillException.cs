using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using ProjectBase.Utils.Entitles;
using Core.Reconciliation.Repositories;
using Core.CostFlow;

namespace Core.Reconciliation
{
    public class WayBillException : DomainObject<WayBillException, int, IWayBillExceptionRepository>
    {
        #region property
        /// <summary>
        /// 收寄日期
        /// </summary>
        public virtual DateTime? PostingTime { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public virtual string ExpressNo { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public virtual string LoadBillNum { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public virtual decimal? Weight { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string CusName { get; set; }
        /// <summary>
        /// 邮政邮资
        /// </summary>
        public virtual decimal WayBillFee { get; set; }
        /// <summary>
        /// 邮政邮件处理费
        /// </summary>
        public virtual decimal ProcessingFee { get; set; }
        /// <summary>
        /// 客户运费
        /// </summary>
        public virtual decimal ExpressFee { get; set; }
        /// <summary>
        /// 客户操作费
        /// </summary>
        public virtual decimal OperateFee { get; set; }
        /// <summary>
        /// 运费毛利
        /// </summary>
        public virtual decimal WayBillProfit { get; set; }

        /// <summary>
        /// 总毛利
        /// </summary>
        public virtual decimal TotalProfit { get; set; }

        /// <summary>
        /// 对账日期
        /// </summary>
        public virtual DateTime ReconcileDate { get; set; }

        /// <summary>
        /// 其他成本费
        /// </summary>
        public virtual decimal CostOtherFee { get; set; }

        /// <summary>
        /// 其他收入费
        /// </summary>
        public virtual decimal InComeOtherFee { get; set; }

        /// <summary>
        /// 总收入
        /// </summary>
        public virtual decimal? InComeTotalFee { get; set; }

        /// <summary>
        /// 总成本费
        /// </summary>
        public virtual decimal? CostTotalFee { get; set; }

        /// <summary>
        /// 成本状态
        /// </summary>
        public virtual PayStatus CostStatus { get; set; }

        /// <summary>
        /// 收入状态
        /// </summary>
        public virtual PayStatus InComeStatus { get; set; }

        /// <summary>
        /// 异常说明
        /// </summary>
        public virtual string ExceptionMsg { get; set; }

        /// <summary>
        /// 是否已导入成本
        /// </summary>
        public virtual int IsInputCost { get; set; }

        /// <summary>
        /// 异常状态
        /// </summary>
        public virtual WayBillExceptionTypeEnum ExceptionType { get; set; }

        /// <summary>
        /// 运单成本ID
        /// </summary>
        public virtual int WayBillCostID { get; set; }

        #endregion

        #region common method
        public static IPageOfList<WayBillException> GetByFilter(ParameterFilter filter)
        {
            return Dao.GetByFilter(filter);
        }

        public static long GetExceptionCount(ParameterFilter strWhere)
        {
            return Dao.GetExceptionCount(strWhere);
        }

        public static IList<WayBillException> GetByExpressNo(string expressNo)
        {
            return Dao.GetByExpressNo(expressNo);
        }

        public static int DeleteException(int wayBillCostID, WayBillException Excmodel, WayBillCost Costmodel)
        {
            return Dao.DeleteException(wayBillCostID, Excmodel, Costmodel);
        }

        /// <summary>
        /// 修改后 删除异常记录
        /// </summary>
        /// <param name="costinfo"></param>
        /// <returns></returns>
        public static int UpdateException(WayBillCost costinfo, WayBillException model, int WayBillCostID)
        {
            return Dao.UpdateException(costinfo, model, WayBillCostID);
        }
        /// <summary>
        /// 根据运单号获取成本详细
        /// </summary>
        /// <param name="ExpressNo"></param>
        /// <returns></returns>
        public static IList<WayBillException> GetExceByID(string exceID)
        {
            return Dao.GetExceByID(exceID);
        }

        public static IList<WayBillCost> GetCostByExpByID(string CostID)
        {
            return Dao.GetCostByExpByID(CostID);
        }
        #endregion
    }
}
