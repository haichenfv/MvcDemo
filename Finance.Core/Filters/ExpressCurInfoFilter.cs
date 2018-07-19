using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Filters
{
    public class ExpressCurInfoFilter : ParameterFilter
    {
        /// <summary>
        /// 收货商名称
        /// </summary>
        public virtual string DeliveryName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual string ComplayName { get; set; }

        /// <summary>
        /// 生产NHQL查询语句
        /// </summary>
        /// <returns></returns>
        public override string ToHql()
        {
            string hql = "";
            if (!string.IsNullOrEmpty(DeliveryName))
            {
                hql += " and DeliveryName =:DeliveryName ";
            }
            if (!string.IsNullOrEmpty(ComplayName))
            {
                hql += " and ComplayName =:ComplayName ";
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
            if (!string.IsNullOrEmpty(DeliveryName))
            {
                result["DeliveryName"] = DeliveryName.Trim();
            }
            if (!string.IsNullOrEmpty(ComplayName))
            {
                result["ComplayName"] = ComplayName.Trim();
            }
            return result;
        }
    }
}
