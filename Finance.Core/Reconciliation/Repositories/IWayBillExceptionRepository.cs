using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using Core.Reconciliation;
using Core.CostFlow;


namespace Core.Reconciliation.Repositories
{
    public interface IWayBillExceptionRepository : IDao<WayBillException, int>
    {
        long GetExceptionCount(ParameterFilter strWhere);

        IList<WayBillException> GetByExpressNo(string expressNo);

        int DeleteException(int wayBillCostID, WayBillException Excmodel, WayBillCost Costmodel);

        int UpdateException(WayBillCost costinfo, WayBillException model, int WayBillCostID);

        IList<WayBillException> GetExceByID(string exceID);

        IList<WayBillCost> GetCostByExpByID(string CostID);
    }
}
