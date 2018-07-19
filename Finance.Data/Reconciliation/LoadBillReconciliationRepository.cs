using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Reconciliation;
using Core.Reconciliation.Repositories;
using ProjectBase.Data;
using ProjectBase.Utils.Entitles;

namespace Data.Reconciliation
{
    public class LoadBillReconciliationRepository : AbstractNHibernateDao<LoadBillReconciliation, int>, ILoadBillReconciliationRepository
    {
        public override IPageOfList<LoadBillReconciliation> GetByFilter(ParameterFilter filter)
        {
            string column = @"a.ID,
b.ReconcileDate AS ReconcileDate,
c.Cus_Name AS CusName,
a.LoadBillNum AS LoadBillNum,
a.BillWeight AS FeeWeight,
a.OrderCounts AS ExpressCount,
a.CompletionTime AS CompletionTime,
IFNULL(b.GroundHandlingFee,0) AS GroundHandlingFee,
IFNULL(b.StoreFee,0) AS CostStoreFee,
CASE WHEN b.ID IS NULL THEN a.PreTotalCollectFees ELSE 0 END AS CostExpressFee,
CASE WHEN b.ID IS NULL THEN a.PreTotalOperateFee ELSE 0 END AS CostOperateFee,
null AS CostOtherFee,
null AS CostTotalFee,
b.PayStatus AS CostStatus,
a.LoadFee AS InComeLoadFee,
a.StoreFee AS InComeStoreFee,
a.TotalCollectFees AS InComeExpressFee,
a.TotalOperateFee AS InComeOperateFee,
a.OtherFee AS InComeOtherFee,
null AS InComeTotalFee,
a.PayStatus AS InComeStatus,
null AS TotalGrossProfit,
null AS GrossProfitRate,
CASE IFNULL(a.IsAddMonthPayOff,0) WHEN 0 THEN '待添加到月结表' ELSE '已添加月结表' END AS Status,
cast(a.IsAddMonthPayOff as signed) as IsAddMonthPayOff,
b.ID AS IsReal,
a.OrderWeight as ExpressWeight";
            string sql = @" FROM LoadBillInCome a LEFT JOIN LoadBillCost b ON a.LoadBillNum=b.LoadBillNum LEFT JOIN CustomerInfo c ON a.CustomerID=c.ID WHERE IFNULL(b.`Status`,0)=0";
            if (filter.HasQueryString)
                sql = filter.ToHql();
            else
                sql += filter.ToHql();

            var paras = filter.GetParameters();
            var countQuery = NHibernateSession.CreateSQLQuery(string.Format("select COUNT(a.ID) as Count {0}",sql));
            var query = NHibernateSession.CreateSQLQuery(string.Format("select {0} {1} {2} ", column, sql, filter.GetOrderString()));
            foreach (var key in paras.Keys)
            {
                countQuery.SetParameter(key, paras[key]);
                query.SetParameter(key, paras[key]);
            }
            int pageIndex = filter.PageIndex;
            int pageSize = filter.PageSize;
            //var Count = countQuery.List<object[]>()[0];
            var Count = countQuery.UniqueResult<long>();
            var list = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(LoadBillReconciliation))).SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).List<LoadBillReconciliation>().ToList();
            return new LRPageOfList<LoadBillReconciliation>(list, pageIndex, pageSize, Count);
        }

        public IPageOfList<LoadBillReconciliation> GetByMonthPayOffFilter(ParameterFilter filter)
        {
            string column = @"a.ID,
b.ReconcileDate AS ReconcileDate,
c.Cus_Name AS CusName,
a.LoadBillNum AS LoadBillNum,
a.BillWeight AS FeeWeight,
a.OrderCounts AS ExpressCount,
a.CompletionTime AS CompletionTime,
IFNULL(b.GroundHandlingFee,0) AS GroundHandlingFee,
IFNULL(b.StoreFee,0) AS CostStoreFee,
CASE WHEN b.ID IS NULL THEN a.PreTotalCollectFees ELSE 0 END AS CostExpressFee,
CASE WHEN b.ID IS NULL THEN a.PreTotalOperateFee ELSE 0 END AS CostOperateFee,
null AS CostOtherFee,
null AS CostTotalFee,
b.PayStatus AS CostStatus,
a.LoadFee AS InComeLoadFee,
a.StoreFee AS InComeStoreFee,
a.TotalCollectFees AS InComeExpressFee,
a.TotalOperateFee AS InComeOperateFee,
a.OtherFee AS InComeOtherFee,
null AS InComeTotalFee,
a.PayStatus AS InComeStatus,
null AS TotalGrossProfit,
null AS GrossProfitRate,
CASE IFNULL(a.IsAddMonthPayOff,0) WHEN 0 THEN '待添加到月结表' ELSE '已添加月结表' END AS Status,
cast(a.IsAddMonthPayOff as signed) as IsAddMonthPayOff,
b.ID AS IsReal,
a.OrderWeight as ExpressWeight";
            string sql = @" FROM LoadBillInCome a INNER JOIN MonthPayOffDetail d ON a.ID=d.LoadBillID LEFT JOIN LoadBillCost b ON a.LoadBillNum=b.LoadBillNum LEFT JOIN CustomerInfo c ON a.CustomerID=c.ID WHERE 1=1";
            if (filter.HasQueryString)
                sql = filter.ToHql();
            else
                sql += filter.ToHql();

            var paras = filter.GetParameters();
            var countQuery = NHibernateSession.CreateSQLQuery(string.Format("select COUNT(a.ID) as Count {0}", sql));
            var query = NHibernateSession.CreateSQLQuery(string.Format("select {0} {1} {2} ", column, sql, filter.GetOrderString()));
            foreach (var key in paras.Keys)
            {
                countQuery.SetParameter(key, paras[key]);
                query.SetParameter(key, paras[key]);
            }
            int pageIndex = filter.PageIndex;
            int pageSize = filter.PageSize;
            //var Count = countQuery.List<object[]>()[0];
            var Count = countQuery.UniqueResult<long>();
            var list = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(LoadBillReconciliation))).SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).List<LoadBillReconciliation>().ToList();
            return new LRPageOfList<LoadBillReconciliation>(list, pageIndex, pageSize, Count);
        }


        public LoadBillReconciliation GetByLoadBillNum(string loadBillNum)
        {
            string sql = @"SELECT
a.ID,
b.ReconcileDate AS ReconcileDate,
c.Cus_Name AS CusName,
a.LoadBillNum AS LoadBillNum,
a.BillWeight AS FeeWeight,
a.OrderCounts AS ExpressCount,
a.CompletionTime AS CompletionTime,
IFNULL(b.GroundHandlingFee,0) AS GroundHandlingFee,
IFNULL(b.StoreFee,0) AS CostStoreFee,
SUM(d.WayBillFee) CostExpressFee,
SUM(d.ProcessingFee) AS CostOperateFee,
null AS CostOtherFee,
IFNULL(b.GroundHandlingFee,0)+IFNULL(b.StoreFee,0)+IFNULL(SUM(d.WayBillFee),0)+IFNULL(SUM(d.ProcessingFee),0) AS CostTotalFee,
b.PayStatus AS CostStatus,
a.LoadFee AS InComeLoadFee,
a.StoreFee AS InComeStoreFee,
a.TotalCollectFees AS InComeExpressFee,
a.TotalOperateFee AS InComeOperateFee,
a.OtherFee AS InComeOtherFee,
a.LoadFee+a.StoreFee+a.TotalCollectFees+a.TotalOperateFee+a.OtherFee AS InComeTotalFee,
a.PayStatus AS InComeStatus,
null AS TotalGrossProfit,
null AS GrossProfitRate,
CASE IFNULL(a.IsAddMonthPayOff,0) WHEN 0 THEN '待添加到月结表' ELSE '已添加月结表' END AS Status,
b.ID AS IsReal,
a.OrderWeight as ExpressWeight
FROM LoadBillInCome a 
LEFT JOIN LoadBillCost b ON a.LoadBillNum=b.LoadBillNum 
LEFT JOIN CustomerInfo c ON a.CustomerID=c.ID
LEFT JOIN WayBillCost d ON a.LoadBillNum=d.BatchNO
WHERE b.`Status`=0 AND a.LoadBillNum=:loadBillNum
GROUP BY a.ID;";
            var query = NHibernateSession.CreateSQLQuery(sql);
            query.SetParameter("loadBillNum", loadBillNum);
            return query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(LoadBillReconciliation))).UniqueResult<LoadBillReconciliation>();
        }
    }

    public class LRPageOfList<T> : List<T>, IList<T>, IPageOfList, IPageOfList<T>
    {
        public LRPageOfList(IEnumerable<T> items, int pageIndex, int pageSize, long recordTotal)
        {
            if (items != null)
                AddRange(items);
            PageIndex = pageIndex;
            PageSize = pageSize;
            RecordTotal = recordTotal;
        }

        public LRPageOfList(int pageSize)
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
    }
}
