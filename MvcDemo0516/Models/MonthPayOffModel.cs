using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcDemo0516.Models
{
    public class MonthPayOffModel
    {
        /// <summary>
        /// 对账时间
        /// </summary>
        public DateTime ReconcileTime { get; set; }
        /// <summary>
        /// 预计总成本
        /// </summary>
        public decimal PreTotalCostFee { get; set; }
        /// <summary>
        /// 真实总成本
        /// </summary>
        public decimal TotalCostFee { get; set; }
        /// <summary>
        /// 预计总收入
        /// </summary>
        public decimal PreInComeFee { get; set; }
        /// <summary>
        /// 真实总收入
        /// </summary>
        public decimal InComeFee { get; set; }
        /// <summary>
        /// 总毛利
        /// </summary>
        public decimal TotalMargin { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public string LoadBillNum { get; set; }
    }
}