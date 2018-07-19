using Core.Receivable;
using Core.Receivable.Repositories;
using ProjectBase.Data;
using ProjectBase.Utils.Entitles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Receivable
{
    public class MonthPayOffRepository : AbstractNHibernateDao<MonthPayOff, int>, IMonthPayOffRepository
    {
        public IList<MonthPayOff> GetByPayOffMonth(DateTime dt)
        {
            var query = NHibernateSession.CreateQuery("from MonthPayOff where PayOffMonth=:dt");
            query.SetParameter("dt",dt);
            return query.List<MonthPayOff>();
        }

        public override IPageOfList<MonthPayOff> GetByFilter(ParameterFilter filter)
        {
            string column = @"a.ID,
a.PayOffMonth,
cast(COUNT(b.ID)as signed) as LoadBillCounts,
cast(SUM(c.OrderCounts)as signed) AS OrderCounts,
SUM(b.PreTotalCostFee) AS PreTotalCostFee,
SUM(b.TotalCostFee) AS TotalCostFee,
SUM(b.PreInComeFee) AS PreInComeFee,
SUM(b.InComeFee) AS InComeFee,
SUM(b.TotalMargin) AS TotalMargin,
SUM(b.TotalMargin)/SUM(b.PreInComeFee) AS MarginRate,
a.Remark,
a.`Status`";
            string sql = @" FROM MonthPayOff a LEFT JOIN MonthPayOffDetail b ON a.ID=b.MonthPayOffID LEFT JOIN LoadBillInCome c ON b.LoadBillID=c.ID where 1=1";
            if (filter.HasQueryString)
                sql = filter.ToHql();
            else
                sql += filter.ToHql();

            var paras = filter.GetParameters();
            var countQuery = NHibernateSession.CreateSQLQuery(string.Format("select COUNT(DISTINCT a.ID) as Count {0}", sql));
            var query = NHibernateSession.CreateSQLQuery(string.Format("select {0} {1} group by a.ID {2} ", column, sql, filter.GetOrderString()));
            foreach (var key in paras.Keys)
            {
                countQuery.SetParameter(key, paras[key]);
                query.SetParameter(key, paras[key]);
            }
            int pageIndex = filter.PageIndex;
            int pageSize = filter.PageSize;
            //var Count = countQuery.List<object[]>()[0];
            var Count = countQuery.UniqueResult<long>();
            if (Count==0)
            {
                return new PageOfList<MonthPayOff>(new List<MonthPayOff>(),pageIndex,pageSize,Count);
            }
            else
            {
                var list = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(MonthPayOff))).SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).List<MonthPayOff>().ToList();
                return new PageOfList<MonthPayOff>(list, pageIndex, pageSize, Count);
            }
        }

        
    }
}
