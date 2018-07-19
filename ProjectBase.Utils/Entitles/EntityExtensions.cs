using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ProjectBase.Utils.Entitles
{
    public static class EntityExtensions
    {
        /// <summary>
        /// 返回一个不为空的对象，如果对象为空则实例化对象，否则直接返回对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Instance<T>(this T obj) where T : new()
        {
            if (obj == null)
                return new T();
            else
                return obj;
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection)
        {
            if (collection == null)
                return;
            if (list == null)
            {
                return;
            }
            foreach (T item in collection)
            {
                list.Add(item);
            }
        }

        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            if (list == null)
            {
                return;
            }
            if (action == null)
            {
                return;
            }
            foreach (T item in list)
                action(item);
        }

        /// <summary>
        /// 合并两个集合对象,将collection与list进行比较,
        /// 将两者都包含的数据通过调用editValue进行设值,
        /// 将仅collection包含的数据添加到list,
        /// 将仅list包含的数据删除,
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="list">原始数据集合</param>
        /// <param name="collection">新数据集合</param>
        /// <param name="predicate">判断对象相等的函数,第一个参数为list中的对象，第二个参数为collection中的对象</param>
        /// <param name="editValue">进行赋值的方法,第一个参数为list中的对象，第二个参数为collection中的对象</param>
        public static void Merge<T>(this IList<T> list, IEnumerable<T> collection, Func<T, T, bool> predicate, Action<T, T> editValue)
        {
            Check.Assert(collection == null, String.Empty);
            if (editValue == null)
            {
                return;
            }
            if (list == null)
            {
                return;
            }
            foreach (var item in collection)
            {
                var target = list.Where(p => predicate(p, item)).FirstOrDefault();
                if (target == null)
                    list.Add(item);
                else
                    editValue(target, item);
            }

            var deleteItems = list.Where(p => !collection.Any(po => predicate(p, po))).ToList();
            foreach (var item in deleteItems)
                list.Remove(item);
        }


        public static IPageOfList<T> ToPageOfList<T>(this IList<T> list, int pageIndex, int pageSize)
        {
            if (list == null)
                return null;
            return new PageOfList<T>(list.Skip(pageIndex * pageSize).Take(pageSize), pageIndex, pageSize, list.Count);
        }

        public static T As<T>(this Object entity)
        {
            return (T)entity;
        }


        public static T CreateObject<T>(this string subName)
        {
            Assembly asmb = Assembly.GetAssembly(typeof(T));
            Type supType = asmb.GetType(subName);
            return (T)supType.Assembly.CreateInstance(supType.FullName);
        }
    }
}
