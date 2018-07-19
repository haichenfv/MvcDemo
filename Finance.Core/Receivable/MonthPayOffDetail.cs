using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using Core.Receivable.Repositories;

namespace Core.Receivable
{
    public class MonthPayOffDetail : DomainObject<MonthPayOffDetail, int, IMonthPayOffDetailRepository>
    {
        #region property
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
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 提单收入信息
        /// </summary>
        public virtual LoadBillInCome LoadBillBy { get; set; }
        /// <summary>
        /// 月结主信息
        /// </summary>
        public virtual MonthPayOff MonthPayOffBy { get; set; }
        #endregion

        #region common method
        /// <summary>
        /// 根据提单号获取添加月结信息
        /// </summary>
        /// <param name="loadBillIDs"></param>
        /// <returns></returns>
        public static IList<MonthPayOff> GetPayOffMonth(IList<int> loadBillIDs)
        {
            return Dao.GetPayOffMonth(loadBillIDs);
        }

        public static MonthPayOffDetail GetByLoadBillID(int loadBillID)
        {
            return Dao.GetByLoadBillID(loadBillID);
        }
        #endregion

        #region entity method

        #endregion
    }
}
