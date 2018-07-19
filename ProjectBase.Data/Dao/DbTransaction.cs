using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace ProjectBase.Data
{
    public class DbTransaction : IDisposable
    {
        ITransaction _transaction;

        public DbTransaction(ITransaction transaction)
        {

            _transaction = transaction;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        #endregion

        #region ITransaction 成员

        public void Begin(System.Data.IsolationLevel isolationLevel)
        {
            _transaction.Begin(isolationLevel);
        }

        public void Begin()
        {
            _transaction.Begin();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Enlist(System.Data.IDbCommand command)
        {
            _transaction.Enlist(command);
        }

        public bool IsActive
        {
            get { return _transaction.IsActive; }
        }

        public void RegisterSynchronization(NHibernate.Transaction.ISynchronization synchronization)
        {
            _transaction.RegisterSynchronization(synchronization);
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public bool WasCommitted
        {
            get { return _transaction.WasCommitted; }
        }

        public bool WasRolledBack
        {
            get { return _transaction.WasRolledBack; }
        }

        #endregion
    }
}
