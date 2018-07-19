using Core.Customer;
using Core.Customer.Repositories;
using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Customer
{
    public class CustomerInfoRepository : AbstractNHibernateDao<CustomerInfo, int>, ICustomerInfoRepository
    {
    }
}
