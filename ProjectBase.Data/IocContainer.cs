using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace ProjectBase.Data
{
    public class IocContainer : IDisposable
    {
        private IocContainer() { }

        #region 保证线程安全的延时加载单例

        public static IocContainer Instance
        {
            get { return Nested.IocContainer; }
        }

        /// <summary>
        /// 保证线程安全的延时加载单例
        /// </summary>
        private class Nested
        {
            static Nested() { }
            internal static readonly IocContainer IocContainer = new IocContainer();
        }
        #endregion

        UnityContainer _container = new UnityContainer();
        public UnityContainer Container { get { return _container; } }

        /// <summary>
        /// 从IOC容器内取回指定的接口类型
        /// </summary>
        /// <typeparam name="T">接口类形</typeparam>
        public T Resolve<T>()
        {
            try
            {
                return Container.Resolve<T>();
            }
            catch (ResolutionFailedException ex)
            {
                throw new ApplicationException(string.Format("不能取回{0}类型，请在 IocContainer.Instance.Container 中映射{0}类型", typeof(T)), ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container.Dispose();
                _container = null;
            }
        }
    }
}
