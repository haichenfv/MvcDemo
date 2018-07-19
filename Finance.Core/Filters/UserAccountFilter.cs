using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Filters
{
    public class UserAccountFilter : ParameterFilter
    {
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public virtual bool? IsAvailable { get; set; }

        public override string ToHql()
        {
            string hql = "";
            if (!string.IsNullOrEmpty(Account))
            {
                hql += " and Account =:Account ";
            }
            if (IsAvailable.HasValue)
            {
                hql += " and IsAvailable =:IsAvailable ";
            }
            return hql;
        }

        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Account))
            {
                result["Account"] = Account.Trim();
            }
            if (IsAvailable.HasValue)
            {
                result["IsAvailable"] = IsAvailable.Value;
            }
            return result;
        }
    }
}
