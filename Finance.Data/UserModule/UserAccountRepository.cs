using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.UserModule;
using Core.UserModule.Repositories;
using ProjectBase.Data;
using ProjectBase.Utils;

namespace Data.UserModule
{
    public class UserAccountRepository : AbstractNHibernateDao<UserAccount, int>, IUserAccountRepository
    {
        public UserAccount GetByAccount(string account)
        {
            var query = NHibernateSession.CreateQuery("from Core.UserModule.UserAccount where Account=:account");
            query.SetParameter("account", account);
            return query.UniqueResult<UserAccount>();
        }
    }
}
