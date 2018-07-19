using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectBase.Utils.Entitles;

namespace ProjectBase.Data
{
    public abstract class ParameterFilter
    {
        public ParameterFilter()
        {
            HasQueryString = false;
            PageSize = 10;
        }

        public string OrderBy { get; set; }

        public abstract string ToHql();

        public override string ToString()
        {
            return ToHql();
        }

        public abstract Dictionary<string, object> GetParameters();

        public string GetOrderString()
        {
            if (OrderBy.HasValue())
                return " Order By " + OrderBy;
            return String.Empty;
        }

        protected string GetLike(string value)
        {
            return "%" + value + "%";
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        /// 标识此构造器是包含全部查询语句。
        /// 若为 False,则ToHql() 只需要构造条件查询，系统会自动在前面加上<code>" from " + typeof(T).Name + " a where 1=1 "</code>
        /// 若为 True, ToHql() 需要返回 连form在类的完整Hql语句
        /// </summary>
        public bool HasQueryString { get; set; }

        protected static bool HasValue(string str)
        {
            return str.HasValue();
        }

        public static bool HasValue<T>(System.Nullable<T> value) where T : struct
        {
            return value.HasValue;
        }

    }
}
