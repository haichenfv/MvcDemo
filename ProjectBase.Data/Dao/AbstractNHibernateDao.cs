using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using NHibernate;
using NHibernate.Criterion;
using ProjectBase.Utils.Entitles;

namespace ProjectBase.Data
{
    /// <summary>
    /// 数据持久化基本
    /// </summary>
    /// <typeparam name="T">要持久化的数据类型</typeparam>
    /// <typeparam name="TID">ID字段的数据类型</typeparam>
    public abstract class AbstractNHibernateDao<T, TId> : IDao<T, TId>
    {
        private const string APPSETTING_NAME = "sessionPath";

        /// <param name="sessionFactoryConfigPath">指定Session工厂的配置文件</param>
        protected AbstractNHibernateDao(string sessionFactoryConfigPath)
        {
            if (string.IsNullOrEmpty(sessionFactoryConfigPath))
            {
                sessionFactoryConfigPath = "NHibernate.config";
            }
            //如果路径中包含 :(盘符) 符号，则证明传入的是一个绝对路径，可直接使用。
            //否则就是相对路径，在WEB环境中，用Server.MapPath转换成绝对路径，在App环境中，则加上当前运行目录
            if (sessionFactoryConfigPath.Contains(":"))
                SessionFactoryConfigPath = sessionFactoryConfigPath;
            else if (System.Web.HttpContext.Current != null)
                SessionFactoryConfigPath = System.Web.HttpContext.Current.Server.MapPath("~/" + sessionFactoryConfigPath);
            else
                SessionFactoryConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sessionFactoryConfigPath);
        }

        /// <summary>
        /// 初始化AbstractNHibernateDao类，默认使用config文件Appsetting中 sessionPath所指定的文件进行初始化
        /// </summary>
        protected AbstractNHibernateDao()
            : this(ConfigurationManager.AppSettings[APPSETTING_NAME])
        {
        }

        /// <summary>
        /// 根据ID从数据库获取一个类型为T的实例
        /// </summary>
        public T GetById(TId id, bool shouldLock)
        {
            T entity;

            if (shouldLock)
            {
                entity = NHibernateSession.Get<T>(id, LockMode.Upgrade);
            }
            else
            {
                entity = NHibernateSession.Get<T>(id);
            }

            return entity;
        }

        /// <summary>
        /// 根据ID从数据库获取一个类型为T的实例
        /// </summary>
        public T GetById(TId id)
        {
            return GetById(id, false);
        }

        /// <summary>
        /// 获取所有的类型为T的对象
        /// </summary>
        public IList<T> GetAll()
        {
            return GetByCriteria();
        }

        /// <summary>
        /// 根据给定的 <see cref="ICriterion" /> 来查询结果
        /// 如果没有传入 <see cref="ICriterion" />, 效果与 <see cref="GetAll" />一致.
        /// </summary>
        public IList<T> GetByCriteria(params ICriterion[] criterion)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);

            foreach (ICriterion criterium in criterion)
            {
                criteria.Add(criterium);
            }
            criteria.AddOrder(new Order("ID", false));
            return criteria.List<T>();
        }

        /// <summary>
        /// 根据exampleInstance的属性值来查找对象，返回与其值一样的对象对表。
        /// exampleInstance中值为0或NULL的属性将不做为查找条件
        /// </summary>
        /// <param name="exampleInstance">参考对象</param>
        /// <param name="propertiesToExclude">要排除的查询条件属性名</param>
        /// <returns></returns>
        public IList<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(exampleInstance.GetType());
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }
            example.ExcludeNone();
            example.ExcludeNulls();
            example.ExcludeZeroes();
            criteria.Add(example);
            criteria.AddOrder(new Order("ID", false));
            return criteria.List<T>();
        }

        public IPageOfList<T> GetByExample(T exampleInstance, int pageIndex, int pageSize, params string[] propertiesToExclude)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }
            example.ExcludeNone();
            example.ExcludeNulls();
            example.ExcludeZeroes();

            int recordTotal = Convert.ToInt32((criteria.Clone() as ICriteria).SetProjection(Projections.Count("ID")).UniqueResult());

            criteria.AddOrder(new Order("ID", false));
            criteria.Add(example).SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize);

            return new PageOfList<T>(criteria.List<T>(), pageIndex, pageSize, recordTotal);
        }

        /// <summary>
        /// 使用<see cref="GetByExample"/>来返回一个唯一的结果，如果结果不唯一会抛出异常
        /// </summary>
        /// <exception cref="NonUniqueResultException" />
        public T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            IList<T> foundList = GetByExample(exampleInstance, propertiesToExclude);

            if (foundList.Count > 1)
            {
                throw new NonUniqueResultException(foundList.Count);
            }

            if (foundList.Count > 0)
            {
                return foundList[0];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 将指定的对象保存到数据库，并立限提交，并返回更新后的ID
        /// See http://www.hibernate.org/hib_docs/reference/en/html/mapping.html#mapping-declaration-id-assigned.
        /// </summary>
        public T Save(T entity)
        {
            NHibernateSession.Save(entity);
            NHibernateSession.Flush();
            return entity;
        }

        /// <summary>
        /// 将指定的对象保存或更新到数据库，并返回更新后的ID
        /// </summary>
        public T SaveOrUpdate(T entity)
        {
            NHibernateSession.SaveOrUpdate(entity);
            NHibernateSession.Flush();
            return entity;
        }

        /// <summary>
        /// 从数据库中删除指定的对象
        /// </summary>
        public void Delete(T entity)
        {
            NHibernateSession.Delete(entity);
            NHibernateSession.Flush();
        }

        public DbTransaction BeginTransaction()
        {
            ITransaction tran = NHibernateSession.BeginTransaction();// NHibernateSessionManager.Instance.BeginTransactionOn(SessionFactoryConfigPath);
            return new DbTransaction(tran);
        }

        /// <summary>
        /// 提交所有的事务对象，并Flush到数据库
        /// </summary>
        public void CommitChanges()
        {
            if (NHibernateSessionManager.Instance.HasOpenTransactionOn(SessionFactoryConfigPath))
            {
                NHibernateSessionManager.Instance.CommitTransactionOn(SessionFactoryConfigPath);
            }
            else
            {
                // 如果不是事务模式，就直接调用Flush来更新                
                NHibernateSession.Flush();
            }
        }

        /// <summary>
        /// 返回对应的Session.
        /// </summary>
        protected ISession NHibernateSession
        {
            get
            {
                return NHibernateSessionManager.Instance.GetSessionFrom(SessionFactoryConfigPath);
            }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        private Type persitentType = typeof(T);

        /// <summary>
        /// Session配置文件路径
        /// </summary>
        protected readonly string SessionFactoryConfigPath;

        public virtual IPageOfList<T> GetByFilter(ParameterFilter filter)
        {
            string sql = " from " + typeof(T).Name + " a where 1=1 ";
            if (filter.HasQueryString)
                sql = filter.ToHql();
            else
                sql += filter.ToHql();

            var paras = filter.GetParameters();
            var countQuery = NHibernateSession.CreateQuery("select count(*) " + sql);
            var query = NHibernateSession.CreateQuery("select a " + sql + filter.GetOrderString());

            foreach (var key in paras.Keys)
            {
                countQuery.SetParameter(key, paras[key]);
                query.SetParameter(key, paras[key]);
            }

            int pageIndex = filter.PageIndex;
            int pageSize = filter.PageSize;
            long recodTotal = Convert.ToInt64(countQuery.UniqueResult());
            var list = query.SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).List<T>();
            return new PageOfList<T>(list, pageIndex, pageSize, recodTotal);
        }
    }
}
