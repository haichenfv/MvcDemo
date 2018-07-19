using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Data;

namespace Core.Customer.Repositories
{
    public interface ICustomerInfoRepository : IDao<CustomerInfo, int>
    {
    }
}
