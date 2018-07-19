using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Filters
{
    public class MonthPayOffExportFilter : ParameterFilter
    {
        /// <summary>
        /// 结算月份
        /// </summary>
        public DateTime? PayOffMonth { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        public string ListID { get; set; }

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
            if (!string.IsNullOrEmpty(ListID))
            {
                result["ListID"] = ListID;
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
                hql += " AND a.PayOffMonth >= :PayOffMonth ";
            }
            if (!string.IsNullOrEmpty(ListID))
            {
                hql += " AND a.ID IN (:ListID) ";
            }
            return hql;
        }
    }
}
