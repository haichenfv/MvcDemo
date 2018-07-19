using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Reconciliation.Repositories
{
    public interface IWayBillReconciliationRepository : IDao<WayBillReconciliation, int>
    {
        /// <summary>
        /// 根据运单号获取收入详细
        /// </summary>
        /// <param name="ExpressNo"></param>
        /// <returns></returns>
        IList<WayBillReconciliation> GetWayBillInComeByExpressNo(string ExpressNo);
    }
}
