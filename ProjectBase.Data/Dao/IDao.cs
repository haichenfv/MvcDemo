using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Utils.Entitles;

namespace ProjectBase.Data
{
    /// <summary>
    /// 提供对数据对基本增，删，改，查操作的数据接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="IdT"></typeparam>
    public interface IDao<T, TID>
    {
        /// <summary>
        /// 根据ID从数据库获取一个类型为T的实例
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <returns>返回ID所对应的对象</returns>
        T GetById(TID id);

        /// <summary>
        /// 根据ID从数据库获取一个类型为T的实例
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <param name="shouldLock">是否使用锁定模式返回结果</param>
        /// <returns>返回ID所对应的对象</returns>
        T GetById(TID id, bool shouldLock);

        /// <summary>
        /// 返回所有的对象
        /// </summary>
        /// <returns>返回所有的对象</returns>
        IList<T> GetAll();

        /// <summary>
        /// 根据exampleInstance来查找对象，返回与其值一样的对象列表。exampleInstance中值为0或NULL的属性将不做为查找条件
        /// </summary>
        /// <param name="exampleInstance">参考对象</param>
        /// <param name="propertiesToExclude">要排除的查询条件属性名</param>
        /// <returns></returns>
        IList<T> GetByExample(T exampleInstance, params string[] propertiesToExclude);

        /// <summary>
        /// 根据exampleInstance来查找对象，返回分页后与其值一样的对象列表。exampleInstance中值为0或NULL的属性将不做为查找条件
        /// </summary>
        /// <param name="exampleInstance">参考对象</param>
        /// <param name="pageIndex">开始页码，从0开始记录</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="propertiesToExclude">要排除的查询条件属性名</param>
        /// <returns></returns>
        IPageOfList<T> GetByExample(T exampleInstance, int pageIndex, int pageSize, params string[] propertiesToExclude);

        /// <summary>
        /// 根据exampleInstance来查找对象，返回与其值一样的唯一对象。exampleInstance中值为0或NULL的属性将不做为查找条件
        /// </summary>
        /// <param name="exampleInstance">参考对象</param>
        /// <param name="propertiesToExclude">要排除的查询条件属性名</param>
        /// <returns></returns>
        T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude);

        /// <summary>
        /// 将指定的对象保存到数据库
        /// </summary>
        /// <param name="entity">要保存的对象</param>
        /// <returns>更新后的对象</returns>
        T Save(T entity);

        /// <summary>
        /// 将指定的对象保存或更新到数据库
        /// </summary>
        T SaveOrUpdate(T entity);

        /// <summary>
        /// 从数据库中删除指定的对象
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// 根据自定义的查询条件查询结果
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        IPageOfList<T> GetByFilter(ParameterFilter filter);

        /// <summary>
        /// 立即提交修改到数据库
        /// </summary>
        void CommitChanges();

        DbTransaction BeginTransaction();
    }
}
