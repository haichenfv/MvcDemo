using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using ProjectBase.Utils;
using System.IO;
using NHibernate.Cache;
using NHibernate.Cfg;
using System.Reflection;
using NHibernate.Mapping.ByCode;
using System.Threading;
using System.Web;
using System.Collections;

namespace ProjectBase.Data
{
    public sealed class NHibernateSessionManager
    {
        #region 返回一个线程安全的单例

        /// <summary>
        /// 返回一个线程安全的单例
        /// </summary>
        public static NHibernateSessionManager Instance
        {
            get
            {
                return Nested.NHibernateSessionManager;
            }
        }

        private NHibernateSessionManager() { }

        /// <summary>
        /// 返回一个线程安全的单例
        /// </summary>
        private class Nested
        {
            static Nested() { }
            internal static readonly NHibernateSessionManager NHibernateSessionManager =
                new NHibernateSessionManager();
        }

        #endregion

        bool IsAutoFlush = false;

        /// <summary>
        /// 根据 <see cref="sessionFactories" /> 返回一个session factory，如果没有找到则创建一个新的
        /// </summary>
        /// <param name="sessionFactoryConfigPath">配置文件路径</param>
        private ISessionFactory GetSessionFactoryFor(string sessionFactoryConfigPath)
        {
            Check.Require(string.IsNullOrEmpty(sessionFactoryConfigPath),
                "sessionFactoryConfigPath may not be null nor empty");

            //  从hashTable里面取出一个已经存在的ISessionFactory
            ISessionFactory sessionFactory = (ISessionFactory)sessionFactories[sessionFactoryConfigPath];

            //  如果没有匹配的 SessionFactory 则创建一个新的
            if (sessionFactory == null)
            {
                Check.Require(!File.Exists(sessionFactoryConfigPath),
                    "The config file at '" + sessionFactoryConfigPath + "' could not be found");

                NHibernate.Cfg.Environment.UseReflectionOptimizer = false;
                Configuration cfg = new Configuration();
                cfg.Configure(sessionFactoryConfigPath);

                var data = System.Configuration.ConfigurationManager.AppSettings["Data"];
                if (!string.IsNullOrEmpty(data))
                {
                    ModelMapper mapper = new ModelMapper();
                    foreach (var assembly in data.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        MapModel(assembly, mapper);
                    }
                    var hbmMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
                    cfg.AddMapping(hbmMapping);
                }

                sessionFactory = cfg.BuildSessionFactory();


                if (sessionFactory == null)
                {
                    throw new InvalidOperationException("cfg.BuildSessionFactory() returned null.");
                }

                sessionFactories[sessionFactoryConfigPath] = sessionFactory;
            }

            return sessionFactory;
        }

        /// <summary>
        /// 从程序集中间映射类到Nhibernate
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mapper"></param>
        private static void MapModel(string data, ModelMapper mapper)
        {
            var assembly = Assembly.Load(data);
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.GetInterface("IEntitySqlsMapper") != null)
                {
                    mapper.AddMapping(type);
                }
            }
        }


        /// <summary>
        /// 注册推截器到没有打开过的Session里面，这个操作最好在IHttpModule里面完成
        /// </summary>
        public void RegisterInterceptorOn(string sessionFactoryConfigPath, IInterceptor interceptor)
        {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen)
            {
                throw new CacheException("You cannot register an interceptor once a session has already been opened");
            }

            GetSessionFrom(sessionFactoryConfigPath, interceptor);
        }

        /// <summary>
        /// 根据 <see cref="sessionFactories" /> 返回一个session，如果没有找到则创建一个新的
        /// </summary>
        /// <param name="sessionFactoryConfigPath"></param>
        /// <returns></returns>
        public ISession GetSessionFrom(string sessionFactoryConfigPath)
        {
            return GetSessionFrom(sessionFactoryConfigPath, null);
        }

        /// <summary>
        /// 根据 <see cref="sessionFactories" /> 返回一个session，并注入指定的拦截器，如果没有找到则创建一个新的
        /// </summary>
        private ISession GetSessionFrom(string sessionFactoryConfigPath, IInterceptor interceptor)
        {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session == null)
            {
                if (interceptor != null)
                {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession(interceptor);
                }
                else
                {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession();
                }
                session.FlushMode = FlushMode.Never;
                ContextSessions[sessionFactoryConfigPath] = session;
            }

            Check.Ensure(session == null, "session was null");
            return session;
        }

        /// <summary>
        /// Flush指定的Session后关闭数据库连接。
        /// </summary>
        public void CloseSessionOn(string sessionFactoryConfigPath)
        {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen)
            {
                if (IsAutoFlush)
                    session.Flush();
                session.Close();
            }

            ContextSessions.Remove(sessionFactoryConfigPath);
        }

        /// <summary>
        /// Flush和关闭所有的Session
        /// </summary>
        public void CloseAllSession()
        {
            System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();
            foreach (string item in ContextSessions.Keys)
                keys.Add(item);
            foreach (string item in keys)
                CloseSessionOn(item);
        }


        public ITransaction BeginTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            if (transaction == null)
            {
                transaction = GetSessionFrom(sessionFactoryConfigPath).BeginTransaction();
                ContextTransactions.Add(sessionFactoryConfigPath, transaction);
            }

            return transaction;
        }

        public void CommitTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            try
            {
                if (HasOpenTransactionOn(sessionFactoryConfigPath))
                {
                    transaction.Commit();
                    ContextTransactions.Remove(sessionFactoryConfigPath);
                }
            }
            catch (HibernateException)
            {
                RollbackTransactionOn(sessionFactoryConfigPath);
                throw;
            }
        }

        public bool HasOpenTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        public void RollbackTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            try
            {
                if (HasOpenTransactionOn(sessionFactoryConfigPath))
                {
                    transaction.Rollback();
                }

                ContextTransactions.Remove(sessionFactoryConfigPath);
            }
            finally
            {
                CloseSessionOn(sessionFactoryConfigPath);
            }
        }

        private static Hashtable _ContextTransactions = new Hashtable();

        /// <summary>
        /// 保存基于请求的事务集合，一个request一个事务集合
        /// </summary>
        private Hashtable ContextTransactions
        {
            get
            {
                if (IsInWebContext())
                {
                    if (HttpContext.Current.Items[TRANSACTION_KEY] == null)
                        HttpContext.Current.Items[TRANSACTION_KEY] = new Hashtable();

                    return (Hashtable)HttpContext.Current.Items[TRANSACTION_KEY];
                }
                return new Hashtable();
            }
        }

        /// <summary>
        /// 保存基于请求的Session集合，一个request一个Session集合，不同的数据库Session可放在此集合内
        /// </summary>
        private Hashtable ContextSessions
        {
            get
            {
                if (IsInWebContext())
                {
                    if (HttpContext.Current.Items[SESSION_KEY] == null)
                        HttpContext.Current.Items[SESSION_KEY] = new Hashtable();

                    return (Hashtable)HttpContext.Current.Items[SESSION_KEY];
                }
                return _ContextTransactions;
            }
        }

        private bool IsInWebContext()
        {
            return HttpContext.Current != null;
        }

        private Hashtable sessionFactories = new Hashtable();
        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTIONS";
        private const string SESSION_KEY = "CONTEXT_SESSIONS";
        private const string ANALYZE_PROPERTIES = "connection.connection_string";

    }
}
