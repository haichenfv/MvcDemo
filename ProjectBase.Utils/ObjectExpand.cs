using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace ProjectBase.Utils
{
    public static class ObjectExpand
    {
        /// <summary>
        /// 获取枚举变量值的 Description 属性
        /// </summary>
        /// <param name="obj">枚举变量</param>
        /// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
        /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
        public static string GetDescription(this object obj, bool isTop)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                Type _enumType = obj.GetType();
                DescriptionAttribute dna = null;
                if (isTop)
                {
                    //返回类的或枚举头的Description 属性
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(_enumType, typeof(DescriptionAttribute));
                }
                else
                {
                    FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, obj));
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
                       fi, typeof(DescriptionAttribute));
                }
                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                    return dna.Description;
            }
            catch
            {
            }
            return obj.ToString();
        }

        /// <summary>
        /// 日期格式
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="str"></param>
        /// <returns>返回数据格式为 yyyy-MM-dd</returns>
        public static string ToStringDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd") == "0001-01-01" ? string.Empty : date.ToString("yyyy-MM-dd");

        }

        /// <summary>
        /// 时间格式
        /// </summary>
        /// <param name="date"></param>
        /// <returns>返回数据格式为 yyyy-MM-dd HH:mm:ss</returns>
        public static string ToStringDateTime(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 月份格式
        /// </summary>
        /// <param name="date"></param>
        /// <returns>返回数据格式为 yyyy-MM</·returns>
        public static string ToStringMonth(this DateTime date)
        {
            return date.ToString("yyyy-MM");
        }
        /// <summary>
        /// 月份格式
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format">显示</param>
        /// <returns>返回数据格式为 yyyy-MM</·returns>
        public static string ToStringDate(this DateTime date, string format)
        {
            return date.ToString(format);
        }

        /// <summary>
        /// 转换月份格式
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string ToStringMonth(this int month)
        {
            return month < 10 ? "0" + month : month.ToString();
        }

        /// <summary>
        /// 格式化decimal 保留2位小数
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static decimal DecimalFormat(this decimal d)
        {
            return decimal.Round(d, 2);
        }

        /// <summary>
        /// 格式化float 保留2位小数
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string FloatFormat(this float f)
        {
            return f.ToString("0.00");
        }

        /// <summary>
        /// 格式化decimal 保留2位小数
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static decimal DecimalFormat(this decimal? d)
        {
            return decimal.Round(d.Value, 2);
        }
    }
}
