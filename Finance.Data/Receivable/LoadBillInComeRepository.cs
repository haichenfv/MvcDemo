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
    public class LoadBillInComeRepository : AbstractNHibernateDao<LoadBillInCome, int>, ILoadBillInComeRepository
    {
        public IList<LoadBillInCome> GetByLoadBillNums(IEnumerable<string> loadBillNums)
        {
            var query = NHibernateSession.CreateQuery("from LoadBillInCome where LoadBillNum in (:loadBillNums)");
            query.SetParameterList("loadBillNums", loadBillNums);
            return query.List<LoadBillInCome>();
        }

        public LoadBillInCome GetByLoadBillNum(string loadBillNum)
        {
            var query = NHibernateSession.CreateQuery("from LoadBillInCome where LoadBillNum=:loadBillNum");
            query.SetParameter("loadBillNum", loadBillNum);
            return query.List<LoadBillInCome>().FirstOrDefault();
        }

        public IList<string> GetProblemLoadBillCost(IList<string> loadBillNums)
        {
            var query = NHibernateSession.CreateSQLQuery("SELECT DISTINCT LoadBillNum FROM LoadBillCost where LoadBillNum IN (:loadBillNums) AND `Status`<>0;");
            query.SetParameterList("loadBillNums", loadBillNums);
            return query.List<string>();
        }
    }
}
