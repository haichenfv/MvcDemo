using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ProjectBase.Data;
using System.ComponentModel.DataAnnotations;
using ProjectBase.Utils.Entitles;
using Core.Customer.Repositories;
using Core.Filters;

namespace Core.Customer
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public class CustomerInfo : DomainObject<CustomerInfo, int, ICustomerInfoRepository>
    {
        #region property
        /// <summary>
        /// 客户代码
        /// </summary>
        [Required(ErrorMessage = "客户代码不能为空！")]
        [StringLength(30, MinimumLength = 0, ErrorMessage = "客户代码最大长度为30个字符")]
        public virtual string CusCode { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        [Required(ErrorMessage = "客户名称不能为空！")]
        [StringLength(30, MinimumLength = 0, ErrorMessage = "客户名称最大长度为30个字符")]
        public virtual string CusName { get; set; }
        /// <summary>
        /// 客户业务类型
        /// </summary>
        public virtual Busssiness BusssinessType { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public virtual string Phone { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Tel { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [RegularExpression(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$", ErrorMessage = "邮箱格式不正确！")]
        public virtual string Email { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public virtual string Fax { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public virtual string Country { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual string CompanyName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public virtual decimal Balance { get; set; }
        /// <summary>
        /// 信用额度
        /// </summary>
        public virtual decimal CreditAmount { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual CustomerStatus Status { get; set; }
        /// <summary>
        /// 快件收货商信息
        /// </summary>
        public virtual IList<ExpressCurInfo> ExpressCurInfoBy { get; set; }
        #endregion

        #region common method
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IPageOfList<CustomerInfo> GetByFilter(CustomerFilter filter)
        {
            return Dao.GetByFilter(filter);
        }
        #endregion
    }

    /// <summary>
    /// 客户业务类型
    /// </summary>
    public enum Busssiness
    {
        /// <summary>
        /// 快件
        /// </summary>
        [Description("快件")]
        ExpressDelivery = 0,
    }
    /// <summary>
    /// 客户状态
    /// </summary>
    public enum CustomerStatus
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 0,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disabled = 1,
    }
}
