using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;
using Core.Customer;

namespace Core.Receivable.Repositories
{
    public interface ILoadBillInComeRepository : IDao<LoadBillInCome, int>
    {
        IList<LoadBillInCome> GetByLoadBillNums(IEnumerable<string> loadBillNums);
        IList<string> GetProblemLoadBillCost(IList<string> loadBillNums);
        LoadBillInCome GetByLoadBillNum(string loadBillNum);
    }
}
