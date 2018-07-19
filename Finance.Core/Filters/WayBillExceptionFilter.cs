using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Filters
{
    public class WayBillExceptionFilter : ParameterFilter
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
        public virtual DateTime? PostingTime { get; set; }

        /// <summary>
        /// 收寄结束日期
        /// </summary>
        public virtual DateTime? PostingTimeTo { get; set; }

        /// <summary>
        /// 异常原因
        /// </summary>
        public virtual WayBillExceptionTypeEnum? ExceptionType { get; set; }

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
            if (PostingTime != null && PostingTimeTo != null)
            {
                result["PostingTime"] = PostingTime.Value;
                result["PostingTimeTo"] = PostingTimeTo.Value;
            }
            if (ExceptionType.HasValue)
            {
                result["ExceptionType"] = ExceptionType.Value;
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
                hql += " and w.BatchNO =:LoadBillNum ";
            }
            if (!string.IsNullOrEmpty(ExpressNo))
            {
                hql += " and e.ExpressNo =:ExpressNo ";
            }
            if (PostingTime != null && PostingTimeTo != null)
            {
                hql += " and e.PostingTime>= :PostingTime and e.PostingTime <= :PostingTimeTo ";
            }
            if (ExceptionType.HasValue)
            {
                hql += " and e.ExceptionType = :ExceptionType";
            }

            return hql;
        }
    }
}
