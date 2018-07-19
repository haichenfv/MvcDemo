using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBase.Data
{
    /// <summary>
    /// 数据保存事件
    /// </summary>
    public class SavedEventArgs : EventArgs
    {

        /// <summary>
        /// 初始化 <see cref="SavedEventArgs"/> 类.
        /// </summary>
        /// <param name="action">动作类型.</param>
        public SavedEventArgs(SaveAction action)
        {
            Action = action;
        }

        private SaveAction _Action;
        /// <summary>
        /// 返回或设置动作类型.
        /// </summary>
        public SaveAction Action
        {
            get { return _Action; }
            set { _Action = value; }
        }

        public bool IsCancel
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 描述保存时的动作类型.
    /// </summary>
    public enum SaveAction
    {
        /// <summary>
        /// 默认值，无发生动作.
        /// </summary>
        None,
        /// <summary>
        /// 添加对象.
        /// </summary>
        Insert,
        /// <summary>
        /// 更新对象.
        /// </summary>
        Update,
        /// <summary>
        /// 删除对象.
        /// </summary>
        Delete
    }
}
