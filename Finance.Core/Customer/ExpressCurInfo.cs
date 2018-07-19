using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using ProjectBase.Utils.Entitles;
using Core.Customer.Repositories;
using Core.Filters;

namespace Core.Customer
{
    public class ExpressCurInfo : DomainObject<ExpressCurInfo, int, IExpressCurInfoRepository>
    {
        #region property
        /// <summary>
        /// 收货商ID
        /// </summary>
        public virtual int DeliveryID { get; set; }
        /// <summary>
        /// 收货商名称
        /// </summary>
        public virtual string DeliveryName { get; set; }
        /// <summary>
        /// 账户名称
        /// </summary>
        public virtual string AccountName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 收货商
        /// </summary>
        public virtual CustomerInfo CustomerInfoBy { get; set; }
        /// <summary>
        /// 是否匹配   true: 已匹配， false: 未匹配
        /// </summary>
        public virtual bool IsMatch { get; set; }
        #endregion

        #region common method
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IPageOfList<ExpressCurInfo> GetByFilter(ExpressCurInfoFilter filter)
        {
            return Dao.GetByFilter(filter);
        }
        #endregion

        #region entity method
        public virtual void MatchCustomer(CustomerInfo ci)
        {
            using (var tran = Dao.BeginTransaction())
            {
                try
                {
                    this.CustomerInfoBy = ci;
                    this.IsMatch = true;
                    this.Update();
                    Dao.ExpressMath(this.DeliveryID, ci.ID);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        #endregion
    }
}
