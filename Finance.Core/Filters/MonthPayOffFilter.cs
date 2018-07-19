using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Filters
{
    public class MonthPayOffFilter : ParameterFilter
    {
        /// <summary>
        /// 结算月份
        /// </summary>
        public virtual DateTime? PayOffMonth { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        public virtual int? ID { get; set; }

        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (PayOffMonth.HasValue)
            {
                result["PayOffMonth"] = PayOffMonth.Value;
            }
            if (ID.HasValue)
            {
                result["ID"] = ID.Value;
            }
            return result;
        }

        /// <summary>
        /// 生产NHQL查询语句
        /// </summary>
        /// <returns></returns>
        public override string ToHql()
        {
            string hql = "";
            if (PayOffMonth.HasValue)
            {
                hql += " AND a.PayOffMonth = :PayOffMonth ";
            }
            if (ID.HasValue)
            {
                hql += " AND a.ID = :ID ";
            }
            return hql;
        }
    }
}
