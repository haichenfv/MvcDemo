using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Filters
{
    public class WayBillReconciliationFilter : ParameterFilter
    {
        /// <summary>
        /// 客户简称
        /// </summary>
        public string CusShortName { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public string LoadBillNum { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 收寄开始日期
        /// </summary>
        public DateTime? PostingTime { get; set; }

        /// <summary>
        /// 收寄结束日期
        /// </summary>
        public DateTime? PostingTimeTo { get; set; }

        /// <summary>
        /// 毛利
        /// </summary>
        public string Margin { get; set; }

        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(CusShortName))
            {
                result["CusShortName"] = string.Format("{0}%", CusShortName.Trim());
            }
            if (!string.IsNullOrEmpty(LoadBillNum))
            {
                result["LoadBillNum"] = LoadBillNum.Trim();
            }
            if (!string.IsNullOrEmpty(ExpressNo))
            {
                result["ExpressNo"] = ExpressNo.Trim();
            }
            if (PostingTime.HasValue)
            {
                result["PostingTime"] = PostingTime.Value;
            }
            if (PostingTimeTo.HasValue)
            {
                result["PostingTimeTo"] = PostingTimeTo.Value;
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
            if (!string.IsNullOrEmpty(CusShortName))
            {
                hql += " and c.Cus_Name like :CusShortName ";
            }
            if (!string.IsNullOrEmpty(LoadBillNum))
            {
                hql += " and d.LoadBillNum =:LoadBillNum ";
            }
            if (!string.IsNullOrEmpty(ExpressNo))
            {
                hql += " and b.ExpressNo =:ExpressNo ";
            }
            if (PostingTime.HasValue)
            {
                hql += " and a.PostingTime>= :PostingTime ";
            }
            if (PostingTimeTo.HasValue)
            {
                hql += "and a.PostingTime <= :PostingTimeTo ";
            }
            if (Margin == "+")
            {
                hql += " and b.ExpressFee-a.WayBillFee>=0";
            }
            else if (Margin == "-")
            {
                hql += " and b.ExpressFee-a.WayBillFee<0";
            }
            return hql;
        }
    }
}
