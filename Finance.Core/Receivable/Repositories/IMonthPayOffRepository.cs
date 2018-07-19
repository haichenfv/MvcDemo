using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Receivable.Repositories
{
    public interface IMonthPayOffRepository : IDao<MonthPayOff, int>
    {
        IList<MonthPayOff> GetByPayOffMonth(DateTime dt);
    }
}
