using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Utils;
using System.Threading;
using System.Collections.Specialized;

namespace ProjectBase.Data
{
    /// <summary>
    /// 提供一个最基本的领域模型基类，不带数据持久化操作
    /// </summary>
    /// <typeparam name="IdT">指定ID属性的数据类型</typeparam>
    public class DomainObjectBase<TID> : IDisposable//, IDataErrorInfo
    {

        private TID id = default(TID);

        /// <summary>
        /// 领域模型的唯一标识，可以是类形数据类型
        /// </summary>
        public virtual TID ID
        {
            get { return id; }
            set { id = value; }
        }

        #region Equals
        public override bool Equals(object obj)
        {
            DomainObjectBase<TID> compareTo = obj as DomainObjectBase<TID>;

            return (compareTo != null) &&
                   (HasSameNonDefaultIdAs(compareTo) ||
                // Since the IDs aren't the same, either of them must be transient to 
                // compare business value signatures
                    (((IsTransient()) || compareTo.IsTransient()) &&
                     HasSameBusinessSignatureAs(compareTo)));
        }

        /// <summary>
        /// 判断对象ID字段是否有值，来判断对象是否是一个临时对象，没有经过持久化
        /// 如是他的ID值为零，则将其视为一个临时的对象
        /// </summary>
        public virtual bool IsTransient()
        {
            return ID == null || ID.Equals(default(TID));
        }

        /// <summary>
        /// 用于比较两个对象
        /// </summary>
        public override int GetHashCode()
        {
            return (GetType().FullName + "|" + ID).GetHashCode();
        }

        private bool HasSameBusinessSignatureAs(DomainObjectBase<TID> compareTo)
        {
            Check.Require(compareTo == null, "compareTo may not be null");

            return GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>
        /// 判断两个对象是否是同一个领域对象，如果两个对象 <see cref="ID"/> 不是默认值且值相等，则认为是同一对象,返回true;
        /// 否则返回false。
        /// </summary>
        private bool HasSameNonDefaultIdAs(DomainObjectBase<TID> compareTo)
        {
            Check.Require(compareTo == null, "compareTo may not be null");

            return (ID != null && !ID.Equals(default(TID))) &&
                   (compareTo.ID != null && !compareTo.ID.Equals(default(TID))) &&
                   ID.Equals(compareTo.ID);
        }
        #endregion

        /// <summary>
        /// 当前用户是否通过了验证
        /// </summary>
        /// <value>
        /// 	<c>true</c> 如果是验证用户; 否则, <c>false</c>.
        /// </value>
        protected bool IsAuthenticated
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.IsAuthenticated;
            }
        }

        #region Validation

        private StringDictionary _BrokenRules = new StringDictionary();

        /// <summary>
        /// 添加或删除一个不能通过的业务规则
        /// </summary>
        /// <param name="propertyName">属性名.</param>
        /// <param name="errorMessage">错误描述</param>
        /// <param name="isBroken">如果为 true 则添加规则，否则删除.</param>
        protected virtual void AddRule(string propertyName, string errorMessage, bool isBroken)
        {
            if (isBroken)
            {
                _BrokenRules[propertyName] = errorMessage;
            }
            else
            {
                if (_BrokenRules.ContainsKey(propertyName))
                {
                    _BrokenRules.Remove(propertyName);
                }
            }
        }

        /// <summary>
        /// 验证对象是否符合业务规则
        /// </summary>
        protected virtual void ValidationRules()
        {
            ValidationObjectAttribute();
        }

        /// <summary>
        /// 验证对象是否符合自身标记的特性规则
        /// </summary>
        protected void ValidationObjectAttribute()
        {
            IEnumerable<ErrorInfo> errors = DataAnnotationsValidationRunner.GetErrors(this);
            foreach (ErrorInfo error in errors)
            {
                AddRule(error.Name, error.Message, true);
            }
        }

        /// <summary>
        /// 返回对象是否通过了业务规则
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                ValidationRules();
                return this._BrokenRules.Count == 0;
            }
        }

        /// <summary>
        /// 返回所有不符合业务逻辑的属性
        /// </summary>
        public virtual string ValidationMessage
        {
            get
            {
                if (!IsValid)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string messages in this._BrokenRules.Values)
                    {
                        sb.AppendLine(messages);
                    }

                    return sb.ToString();
                }

                return string.Empty;
            }
        }

        public virtual string GetError(string name)
        {
            if (_BrokenRules.ContainsKey(name))
                return _BrokenRules[name];

            return String.Empty;
        }

        #endregion

        #region IDisposable

        private bool _IsDisposed;
        /// <summary>
        /// 返回对象是否已经被销毁.
        /// </summary>
        protected bool IsDisposed
        {
            get { return _IsDisposed; }
        }

        /// <summary>
        /// 释放当前对象占用的集合空间
        /// </summary>
        /// <param name="disposing">为真是销毁.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.IsDisposed)
                return;

            if (disposing)
            {
                //_ChangedProperties.Clear();
                _BrokenRules.Clear();
                _IsDisposed = true;
            }
        }

        /// <summary>
        /// 销毁当前对象，释放集合空间
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


    }
}
