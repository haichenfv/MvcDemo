using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Filters
{
    public class CustomerFilter : ParameterFilter
    {
        /// <summary>
        /// 客户代码
        /// </summary>
        public virtual string CusCode { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string CusName { get; set; }

        /// <summary>
        /// 生产NHQL查询语句
        /// </summary>
        /// <returns></returns>
        public override string ToHql()
        {
            string hql = "";
            if (!string.IsNullOrEmpty(CusCode))
            {
                hql += " and Cus_Code =:CusCode ";
            }
            if (!string.IsNullOrEmpty(CusName))
            {
                hql += " and Cus_Name =:CusName ";
            }

            return hql;
        }

        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(CusCode))
            {
                result["CusCode"] = CusCode.Trim();
            }
            if (!string.IsNullOrEmpty(CusName))
            {
                result["CusName"] = CusName.Trim();
            }
            return result;
        }
    }
}
