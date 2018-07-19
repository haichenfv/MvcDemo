using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Filters
{
    public class BillOfLadingFeeFilter : ParameterFilter
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string CusName { get; set; }

        public virtual string LoadBillNum { get; set; }
        /// <summary>
        /// 提单状态
        /// </summary>
        public virtual Core.BillStatusEnum? BillStatus { get; set; }
        /// <summary>
        /// 结算状态
        /// </summary>
        public virtual Core.PayStatus? PayStatus { get; set; }
        /// <summary>
        /// 清关完成时间 开始
        /// </summary>
        public virtual DateTime? CompletionTimeFrom { get; set; }
        /// <summary>
        /// 清关完成时间 结束
        /// </summary>
        public virtual DateTime? CompletionTimeTo { get; set; }

        /// <summary>
        /// 生产NHQL查询语句
        /// </summary>
        /// <returns></returns>
        public override string ToHql()
        {
            string hql = "";
            if (!string.IsNullOrEmpty(CusName))
                hql += " and CustomerInfoBy.CusName like:CusName ";
            if (!string.IsNullOrEmpty(LoadBillNum))
                hql += " and LoadBillNum =:LoadBillNum ";
            if (BillStatus.HasValue)
                hql += " and BillStatus =:BillStatus ";
            if (PayStatus.HasValue)
                hql += " and PayStatus =:PayStatus ";
            if (CompletionTimeFrom.HasValue)
                hql += " and CompletionTime >=:CompletionTimeFrom ";
            if (CompletionTimeTo.HasValue)
                hql += " and CompletionTime <=:CompletionTimeTo";
            return hql;
        }

        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(CusName))
                result["CusName"] = string.Format("{0}%", CusName.Trim());
            if (!string.IsNullOrEmpty(LoadBillNum))
                result["LoadBillNum"] = LoadBillNum.Trim();
            if (BillStatus.HasValue)
                result["BillStatus"] = BillStatus;
            if (PayStatus.HasValue)
                result["PayStatus"] = PayStatus;
            if (CompletionTimeFrom.HasValue)
                result["CompletionTimeFrom"] = CompletionTimeFrom;
            if (CompletionTimeTo.HasValue)
                result["CompletionTimeTo"] = CompletionTimeTo;
            return result;
        }
    }
}
