using Core.Receivable;
using Core.Receivable.Repositories;
using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Receivable;

namespace Data.Receivable
{
    public class WayBillInComeRepository : AbstractNHibernateDao<WayBillInCome, int>, IWayBillInComeRepository
    {

    }
}
