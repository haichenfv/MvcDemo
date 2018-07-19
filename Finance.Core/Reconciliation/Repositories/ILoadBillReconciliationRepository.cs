using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using ProjectBase.Utils.Entitles;

namespace Core.Reconciliation.Repositories
{
    public interface ILoadBillReconciliationRepository : IDao<LoadBillReconciliation, int>
    {
        IPageOfList<LoadBillReconciliation> GetByMonthPayOffFilter(ParameterFilter filter);
        LoadBillReconciliation GetByLoadBillNum(string loadBillNum);
    }
}
