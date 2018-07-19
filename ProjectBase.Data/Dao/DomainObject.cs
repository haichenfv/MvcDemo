using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBase.Data
{
    /// <summary>
    /// 带数据持久化操作的领域模型基类
    /// </summary>
    /// <typeparam name="T">对象数据类型</typeparam>
    /// <typeparam name="TID">对象主键类型</typeparam>
    /// <typeparam name="TDao">操作对象的Dao类型</typeparam>
    public abstract class DomainObject<T, TID, TDao> : DomainObjectBase<TID>
        where T : DomainObject<T, TID, TDao>
        where TDao : IDao<T, TID>
    {
        protected static TDao Dao
        {
            get { return IocContainer.Instance.Resolve<TDao>(); }
        }

        #region Methods

        /// <summary>
        /// 根据给定的ID重建对象
        /// </summary>
        /// <param name="id">对象的唯一标识</param>
        public static T Load(TID id)
        {
            return Dao.GetById(id);
        }

        /// <summary>
        /// 返回所有
        /// </summary>
        /// <returns></returns>
        public static IList<T> GetAll()
        {
            return Dao.GetAll();
        }

        /// <summary>
        /// 从数据库删除当前对象
        /// </summary>
        public virtual void Delete()
        {
            Dao.Delete((T)this);
        }

        /// <summary>
        /// 把当前对象添加到数据库
        /// </summary>
        public virtual void Save()
        {
            if (!IsValid)
                return;

            SavedEventArgs result = OnSaving(this, SaveAction.Insert);
            if (result != null && result.IsCancel)
                return;
            Dao.Save((T)this);
            OnSaved(this, SaveAction.Insert);
        }

        public virtual void SaveWithTransaction()
        {
            using (var tran = Dao.BeginTransaction())
            {
                Save();
                tran.Commit();
            }
        }

        /// <summary>
        /// 提交当前对象的修改到数据库
        /// </summary>
        public virtual void Update()
        {
            if (!IsValid)
                return;

            SavedEventArgs result = OnSaving(this, SaveAction.Update);
            if (result != null && result.IsCancel)
                return;
            Dao.SaveOrUpdate((T)this);
            OnSaved(this, SaveAction.Update);
        }

        ///// <summary>
        ///// 把当前对象保存到数据库
        ///// </summary>
        //virtual public void Save(string userName, string password)
        //{
        //    bool isValidated;
        //    if (userName == string.Empty)
        //        isValidated = false;
        //    else
        //        isValidated = Membership.ValidateUser(userName, password);

        //    Dao.Save((T)this);
        //}
        #endregion

        //领域对象发生修改时的触发事件，暂时没实现
        #region Events

        /// <summary>
        /// 保存对象后发生的事件
        /// </summary>
        public virtual event EventHandler<SavedEventArgs> Saved;
        /// <summary>
        /// 触发 <see cref="Saved"/> 事件.
        /// </summary>
        protected virtual void OnSaved(DomainObjectBase<TID> businessObject, SaveAction action)
        {
            if (Saved != null)
            {
                Saved(businessObject, new SavedEventArgs(action));
            }
        }

        /// <summary>
        /// 保存对象前发生的事件
        /// </summary>
        public virtual event EventHandler<SavedEventArgs> Saving;
        /// <summary>
        /// 触发 <see cref="Saving"/> 事件.
        /// </summary>
        protected virtual SavedEventArgs OnSaving(DomainObjectBase<TID> businessObject, SaveAction action)
        {
            if (Saving != null)
            {
                SavedEventArgs arge = new SavedEventArgs(action);
                Saving(businessObject, arge);
                return arge;
            }
            return null;
        }

        ///// <summary>
        ///// Occurs when this instance is marked dirty. 
        ///// It means the instance has been changed but not saved.
        ///// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;
        ///// <summary>
        ///// Raises the PropertyChanged event safely.
        ///// </summary>
        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        #endregion

        //#region IChangeTracking Members

        ///// <summary>
        ///// Resets the object抯 state to unchanged by accepting the modifications.
        ///// </summary>
        //void IChangeTracking.AcceptChanges()
        //{
        //    Save();
        //}

        //virtual public bool IsChanged
        //{
        //    get { throw new NotImplementedException(); }
        //}
        //#endregion
        //*/
    }
}
