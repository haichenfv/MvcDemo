using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.CostFlow.Repositories
{
    /// <summary>
    /// 运单成本信息
    /// </summary>
    public interface IWayBillCostRepository : IDao<WayBillCost, int>
    {
        IList<string> GetProblemLoadBillNum(IList<string> loadBillNums);

        /// <summary>
        /// 根据运单号获取成本详细
        /// </summary>
        /// <param name="ExpressNo"></param>
        /// <returns></returns>
        IList<WayBillCost> GetCostByExpressNo(string ExpressNo);

        IList<LoadBillStatistics> GetLoadBillStatistics(List<string> loadBillNum);

        long GetCountByLoadBillNum(string loadBillNum);
    }
}
