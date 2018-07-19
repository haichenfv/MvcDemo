using Core.Receivable;
using Core.Receivable.Repositories;
using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Receivable
{
    public class MonthPayOffDetailRepository : AbstractNHibernateDao<MonthPayOffDetail, int>, IMonthPayOffDetailRepository
    {
        public IList<MonthPayOff> GetPayOffMonth(IList<int> loadBillIDs)
        {
            var query = NHibernateSession.CreateQuery("SELECT a.MonthPayOffBy FROM MonthPayOffDetail a where a.LoadBillBy.ID IN (:loadBillIDs);");
            query.SetParameterList("loadBillIDs", loadBillIDs);
            return query.List<MonthPayOff>();
        }

        public MonthPayOffDetail GetByLoadBillID(int loadBillID)
        {
            var query = NHibernateSession.CreateQuery("FROM MonthPayOffDetail where LoadBillBy.ID=:loadBillID");
            query.SetParameter("loadBillID", loadBillID);
            return query.List<MonthPayOffDetail>().FirstOrDefault();
        }
    }
}
