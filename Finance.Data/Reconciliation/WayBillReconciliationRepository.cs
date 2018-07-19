using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using Core.Reconciliation;
using Core.Reconciliation.Repositories;
using ProjectBase.Utils.Entitles;

namespace Data.Reconciliation
{
    public class WayBillReconciliationRepository : AbstractNHibernateDao<WayBillReconciliation, int>, IWayBillReconciliationRepository
    {
        public override IPageOfList<WayBillReconciliation> GetByFilter(ParameterFilter filter)
        {
            //--对账日期 --收寄日期 --运单号  --提单号  --重量 --客户名称 --邮政邮资  --邮政邮件处理费 --客户运费  --客户操作费 --成本状态--收入状态--运费毛利 --操作费毛利
            string column = @"a.ReconcileDate as ReconcileDate,
b.ID AS ID,
a.ID AS IsInputCost,
a.PostingTime AS PostingTime, 
b.ExpressNo AS ExpressNo,  
b.LoadBillNO AS LoadBillNum, 
a.Weight AS Weight,       
c.Cus_Name AS CusName,		 
(CASE WHEN a.ExpressNo IS NULL THEN b.PreExpressFee ELSE a.WayBillFee END) AS WayBillFee,  
(CASE WHEN a.ExpressNo IS NULL THEN b.PreOperateFee ELSE a.ProcessingFee END) AS ProcessingFee,
b.ExpressFee AS ExpressFee,  
b.OperateFee AS OperateFee, 
a.PayStatus AS CostStatus,    
b.PayStatus AS InComeStatus,  
b.ExpressFee-a.WayBillFee AS WayBillProfit,
CASE IFNULL(d.IsAddMonthPayOff,0) WHEN 0 THEN '待添加到月结表' ELSE '已添加月结表' END AS Statement ";
            string sql = @" FROM WayBillInCome b 
INNER JOIN LoadBillInCome d ON b.LoadBillID=d.ID
LEFT JOIN CustomerInfo c ON d.CustomerID=c.ID
LEFT JOIN WayBillCost a ON a.ExpressNo=b.ExpressNo
LEFT JOIN ExpressNoExceptionDetail e ON a.ID=e.WayBillCostID
where 1=1 AND e.ID is NULL";
            if (filter.HasQueryString)
                sql = filter.ToHql();
            else
                sql += filter.ToHql();

            var paras = filter.GetParameters();
            //string strSQL = "select COUNT(b.ID) as Count" + sql;
            string strSQL = @"select COUNT(a.ID) as Count,SUM(a.Weight) as TotalWeight,SUM(a.WayBillFee) as TotalWayBillFee,SUM(a.ProcessingFee) as TotalProcessingFee,
            SUM(b.ExpressFee) as TotalExpressFee,SUM(b.OperateFee) as TotalOperateFee,SUM(b.ExpressFee-a.WayBillFee) AS TotalWayBillProfit" + sql;
            var countQuery = NHibernateSession.CreateSQLQuery(strSQL);
            var query = NHibernateSession.CreateSQLQuery(string.Format("select {0} {1} {2} ", column, sql, filter.GetOrderString()));

            foreach (var key in paras.Keys)
            {
                countQuery.SetParameter(key, paras[key]);
                query.SetParameter(key, paras[key]);
            }

            int pageIndex = filter.PageIndex;
            int pageSize = filter.PageSize;
            var Count = countQuery.List<object[]>()[0];
            //long totalCounts = Count;

            decimal totalWeight = Count[1] == null ? 0m : decimal.Parse(Count[1].ToString());  //总重量
            decimal totalWayBillFee = Count[2] == null ? 0m : decimal.Parse(Count[2].ToString()); //总运费
            decimal totalProcessingFee = Count[3] == null ? 0m : decimal.Parse(Count[3].ToString());
            decimal totalExpressFee = Count[4] == null ? 0m : decimal.Parse(Count[4].ToString());
            decimal totalOperateFee = Count[5] == null ? 0m : decimal.Parse(Count[5].ToString());
            decimal totalWayBillProfit = Count[6] == null ? 0m : decimal.Parse(Count[6].ToString());

            StatModel model = new StatModel
            {
                TotalWeight = totalWeight,
                TotalWayBillFee = totalWayBillFee,
                TotalProcessingFee = totalProcessingFee,
                TotalExpressFee = totalExpressFee,
                TotalOperateFee = totalOperateFee,
                TotalWayBillProfit = totalWayBillProfit
            };
            //StatModel model = new StatModel();
            var list = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(WayBillReconciliation))).SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).List<WayBillReconciliation>().ToList();
            return new WRPageOfList<WayBillReconciliation>(list, pageIndex, pageSize, Convert.ToInt32(Count[0]), model);
        }


        /// <summary>
        /// 根据运单查询运单收入详细
        /// </summary>
        /// <param name="ExpressNo"></param>
        /// <returns></returns>    
        public IList<WayBillReconciliation> GetWayBillInComeByExpressNo(string ExpressNo)
        {
            string column = @"a.ExpressNo,a.LoadBillNO as LoadBillNum,a.Weight,a.ReceiverProvince,a.ExpressType as ExpressTypeget,a.ExpressFee,a.OperateFee,b.CompletionTime ";
            string sql = @"from WayBillInCome a LEFT JOIN LoadBillInCome b on (a.LoadBillNO=b.LoadBillNum)  where a.ExpressNo='" + ExpressNo + "'";

            var query = NHibernateSession.CreateSQLQuery(string.Format("select {0} {1}  ", column, sql));
            var list = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(WayBillReconciliation))).List<WayBillReconciliation>().ToList();

            //StatModel model = new StatModel();
            return list;// query.List<WayBillReconciliation>();
        }
    }

    public class WRPageOfList<T> : List<T>, IList<T>, IPageOfList, IPageOfList<T>
    {
        public WRPageOfList(IEnumerable<T> items, int pageIndex, int pageSize, long recordTotal, StatModel model)
        {
            if (items != null)
                AddRange(items);
            PageIndex = pageIndex;
            PageSize = pageSize;
            RecordTotal = recordTotal;
            StatModelBy = model;
        }

        public WRPageOfList(int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentException("pageSize must gart 0", "pageSize");
            }
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageTotal
        {
            get
            {
                return (int)RecordTotal / PageSize + (RecordTotal % PageSize > 0 ? 1 : 0);
            }
        }

        public long RecordTotal { get; set; }

        public long CurrentStart
        {
            get
            {
                return PageIndex * PageSize + 1;
            }
        }

        public long CurrentEnd
        {
            get
            {
                return (PageIndex + 1) * PageSize > RecordTotal ? RecordTotal : (PageIndex + 1) * PageSize;
            }
        }

        public StatModel StatModelBy { get; set; }

    }

    /// <summary>
    /// 统计类
    /// </summary>
    public class StatModel
    {
        /// <summary>
        /// 总重量
        /// </summary>
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// 总运费 邮资
        /// </summary>
        public decimal TotalWayBillFee { get; set; }

        /// <summary>
        /// 总邮件处理费
        /// </summary>
        public decimal TotalProcessingFee { get; set; }

        /// <summary>
        /// 总的客户运费
        /// </summary>
        public decimal TotalExpressFee { get; set; }

        /// <summary>
        /// 总的客户操作费
        /// </summary>
        public decimal TotalOperateFee { get; set; }

        /// <summary>
        /// 总的运费毛利
        /// </summary>
        public decimal TotalWayBillProfit { get; set; }

    }
}
