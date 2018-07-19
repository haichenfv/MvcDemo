using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Filters
{
    public class LoadBillReconciliationFilter : ParameterFilter
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CusName { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public string LoadBillNum { get; set; }
        /// <summary>
        /// 清关开始日期
        /// </summary>
        public virtual DateTime? CompletionSTime { get; set; }
        /// <summary>
        /// 清关结束日期
        /// </summary>
        public virtual DateTime? CompletionETime { get; set; }
        /// <summary>
        /// 对账开始日期
        /// </summary>
        public virtual DateTime? ReconcileSTime { get; set; }
        /// <summary>
        /// 对账结束日期
        /// </summary>
        public virtual DateTime? ReconcileETime { get; set; }
        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(CusName))
            {
                result["CusName"] = string.Format("{0}%", CusName.Trim());
            }
            if (!string.IsNullOrEmpty(LoadBillNum))
            {
                result["LoadBillNum"] = LoadBillNum.Trim();
            }
            if (CompletionSTime.HasValue)
            {
                result["CompletionSTime"] = CompletionSTime.Value;
            }
            if (CompletionETime.HasValue)
            {
                result["CompletionETime"] = CompletionETime.Value;
            }
            if (ReconcileSTime.HasValue)
            {
                result["ReconcileSTime"] = ReconcileSTime.Value;
            }
            if (ReconcileETime.HasValue)
            {
                result["ReconcileETime"] = ReconcileETime.Value;
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
            if (!string.IsNullOrEmpty(CusName))
            {
                hql += " and c.Cus_Name like :CusName";
            }
            if (!string.IsNullOrEmpty(LoadBillNum))
            {
                hql += " and a.LoadBillNum =:LoadBillNum";
            }
            if (CompletionSTime.HasValue)
            {
                hql += " and a.CompletionTime >=:CompletionSTime";
            }
            if (CompletionETime.HasValue)
            {
                hql += " and a.CompletionTime <=:CompletionETime";
            }
            if (ReconcileSTime.HasValue)
            {
                hql += " and b.ReconcileDate>=:ReconcileSTime";
            }
            if (ReconcileETime.HasValue)
            {
                hql += " and b.ReconcileDate<=:ReconcileETime";
            }
            return hql;
        }
    }

    /// <summary>
    /// 根据月结主键ID获取提货单数据
    /// </summary>
    public class LBRForMonthPayOffFilter : ParameterFilter
    {
        /// <summary>
        /// 对应月结表ID
        /// </summary>
        public int? MonthPayOffID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CusName { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public string LoadBillNum { get; set; }
        /// <summary>
        /// 清关开始日期
        /// </summary>
        public virtual DateTime? CompletionSTime { get; set; }
        /// <summary>
        /// 清关结束日期
        /// </summary>
        public virtual DateTime? CompletionETime { get; set; }
        /// <summary>
        /// 对账开始日期
        /// </summary>
        public virtual DateTime? ReconcileSTime { get; set; }
        /// <summary>
        /// 对账结束日期
        /// </summary>
        public virtual DateTime? ReconcileETime { get; set; }
        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (MonthPayOffID.HasValue)
            {
                result["MonthPayOffID"] = MonthPayOffID.Value;
            }
            if (!string.IsNullOrEmpty(CusName))
            {
                result["CusName"] = string.Format("{0}%", CusName.Trim());
            }
            if (!string.IsNullOrEmpty(LoadBillNum))
            {
                result["LoadBillNum"] = LoadBillNum.Trim();
            }
            if (CompletionSTime.HasValue)
            {
                result["CompletionSTime"] = CompletionSTime.Value;
            }
            if (CompletionETime.HasValue)
            {
                result["CompletionETime"] = CompletionETime.Value;
            }
            if (ReconcileSTime.HasValue)
            {
                result["ReconcileSTime"] = ReconcileSTime.Value;
            }
            if (ReconcileETime.HasValue)
            {
                result["ReconcileETime"] = ReconcileETime.Value;
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
            if (MonthPayOffID.HasValue)
            {
                hql += " and d.MonthPayOffID =:MonthPayOffID";
            }
            if (!string.IsNullOrEmpty(CusName))
            {
                hql += " and c.Cus_Name like :CusName";
            }
            if (!string.IsNullOrEmpty(LoadBillNum))
            {
                hql += " and a.LoadBillNum =:LoadBillNum";
            }
            if (CompletionSTime.HasValue)
            {
                hql += " and a.CompletionTime >=:CompletionSTime";
            }
            if (CompletionETime.HasValue)
            {
                hql += " and a.CompletionTime <=:CompletionETime";
            }
            if (ReconcileSTime.HasValue)
            {
                hql += " and b.ReconcileDate>=:ReconcileSTime";
            }
            if (ReconcileETime.HasValue)
            {
                hql += " and b.ReconcileDate<=:ReconcileETime";
            }
            return hql;
        }
    }

    public class LBRForMonthPayOffExportFilter : ParameterFilter
    {
        /// <summary>
        /// 对应月结表ID集合
        /// </summary>
        public string MonthPayOffIDList { get; set; }

        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(MonthPayOffIDList))
            {
                result["MonthPayOffIDList"] = MonthPayOffIDList;
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
            if (!string.IsNullOrEmpty(MonthPayOffIDList))
            {
                hql += " and d.MonthPayOffID IN (:MonthPayOffIDList)";
            }
            return hql;
        }
    }
}
