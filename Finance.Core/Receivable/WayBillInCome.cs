using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using ProjectBase.Utils.Entitles;
using System.ComponentModel;
using Core.Customer;
using Core.Receivable.Repositories;

namespace Core.Receivable
{
    public class WayBillInCome : DomainObject<WayBillInCome, int, IWayBillInComeRepository>
    {
        #region property
        /// <summary>
        /// 提货单信息
        /// </summary>
        public virtual LoadBillInCome LoadBillBy { get; set; }
        /// <summary>
        /// 对应客户信息
        /// </summary>
        public virtual CustomerInfo CustomerInfoBy { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public virtual string ExpressNo { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNo { get; set; }
        /// <summary>
        /// 包裹重量
        /// </summary>
        public virtual decimal Weight { get; set; }
        /// <summary>
        /// 收件人名
        /// </summary>
        public virtual string ReceiverName { get; set; }
        /// <summary>
        /// 收件人省份
        /// </summary>
        public virtual string ReceiverProvince { get; set; }
        /// <summary>
        /// 收件人详细地址
        /// </summary>
        public virtual string ReceiverAddress { get; set; }
        /// <summary>
        /// 发件人省份
        /// </summary>
        public virtual string SenderProvince { get; set; }
        /// <summary>
        /// 结算状态
        /// </summary>
        public virtual PayStatus PayStatus { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public virtual DateTime? PayTime { get; set; }
        /// <summary>
        /// 运单收入类型
        /// </summary>
        public virtual WayBillType WayBillType { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public virtual decimal ExpressFee { get; set; }
        /// <summary>
        /// 操作费
        /// </summary>
        public virtual decimal OperateFee { get; set; }
        /// <summary>
        /// 换单次数
        /// </summary>
        public virtual int ReplaceWayBillCount { get; set; }
        /// <summary>
        /// 二次派送费
        /// </summary>
        public virtual decimal DispatchAgainFee { get; set; }
        /// <summary>
        /// 税费
        /// </summary>
        public virtual decimal TaxFee { get; set; }
        /// <summary>
        /// 赔偿费
        /// </summary>
        public virtual decimal CompensationFee { get; set; }
        /// <summary>
        /// 退货费
        /// </summary>
        public virtual decimal ReturnedCargoFee { get; set; }
        /// <summary>
        /// 快递类型
        /// </summary>
        public virtual ExpressType ExpressType { get; set; }
        /// <summary>
        /// 对应业务系统公司ID
        /// </summary>
        public virtual int? CompanyID { get; set; }
        /// <summary>
        /// 对应业务系统收货商ID
        /// </summary>
        public virtual int? DeliveryID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        #endregion

        #region common method
        public static IPageOfList<WayBillInCome> GetByFilter(ParameterFilter filter)
        {
            return Dao.GetByFilter(filter);
        }
        #endregion
    }
    public enum ExpressType
    {
        [Description("标准")]
        Standard = 0,
        [Description("经济")]
        Economy = 1
    }
    public enum WayBillType
    {
        [Description("二次派送")]
        Dispatch
    }
}
