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
    public class ExpressCurInfoRepository : AbstractNHibernateDao<ExpressCurInfo, int>, IExpressCurInfoRepository
    {
        public void ExpressMath(int DeliveryID, int customerInfoID)
        {
            var query = NHibernateSession.CreateSQLQuery(@"UPDATE LoadBillInCome SET CustomerID=:customerInfoID where DeliveryID=:DeliveryID;
UPDATE WayBillInCome SET CustomerID=:customerInfoID where DeliveryID=:DeliveryID;
UPDATE IdentityCardDetail SET CustomerID=:customerInfoID where DeliveryID=:DeliveryID;");
            query.SetParameter("DeliveryID", DeliveryID);
            query.SetParameter("customerInfoID", customerInfoID);
            query.ExecuteUpdate();
        }
    }
}
