using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using Core.CostFlow;
using Core.CostFlow.Repositories;

namespace Data.CostFlow
{
    public class WayBillCostRepository : AbstractNHibernateDao<WayBillCost, int>, IWayBillCostRepository
    {
        public IList<string> GetProblemLoadBillNum(IList<string> loadBillNums)
        {
            var query = NHibernateSession.CreateSQLQuery("select DISTINCT LoadBillNO FROM ExpressNoExceptionDetail where LoadBillNO IN (:loadBillNums)");
            query.SetParameterList("loadBillNums", loadBillNums);
            return query.List<string>();
        }

        /// <summary>
        /// 根据运单查询运单成本详细
        /// </summary>
        /// <param name="ExpressNo"></param>
        /// <returns></returns>    
        public IList<WayBillCost> GetCostByExpressNo(string expressNo)
        {
            var query = NHibernateSession.CreateQuery("from WayBillCost where ExpressNo=:expressNo");
            query.SetParameter("expressNo", expressNo);
            return query.List<WayBillCost>();
        }

        public IList<LoadBillStatistics> GetLoadBillStatistics(List<string> loadBillNum)
        {
            var query = NHibernateSession.CreateSQLQuery("SELECT BatchNO as LoadBillNum,SUM(WayBillFee) as WayBillFee,SUM(ProcessingFee) as ProcessingFee FROM WayBillCost WHERE BatchNO IN (:loadBillNum) GROUP BY BatchNO;");
            query.SetParameterList("loadBillNum", loadBillNum);
            return query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(LoadBillStatistics))).List<LoadBillStatistics>();
        }


        public long GetCountByLoadBillNum(string loadBillNum)
        {
            var query = NHibernateSession.CreateSQLQuery("SELECT COUNT(*) FROM WayBillCost WHERE BatchNO =:loadBillNum;");
            query.SetParameter("loadBillNum", loadBillNum);
            return query.UniqueResult<long>();
        }
    }
}
