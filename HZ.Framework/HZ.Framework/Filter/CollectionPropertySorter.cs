using HZ.Framework.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.Filter
{
    public static class CollectionPropertySorter<T>
    {
        private static readonly ConcurrentDictionary<string, LambdaExpression> Cache = new ConcurrentDictionary<string, LambdaExpression>();

        /// <summary>
        /// 按指定属性名称对序列进行排序
        /// </summary>
        /// <param name="source">要排序的集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> OrderBy(IEnumerable<T> source, string propertyName, ListSortDirection sortDirection)
        {
            propertyName.CheckNotNullOrEmpty("propertyName");
            dynamic expression = GetKeySelector(propertyName);
            dynamic keySelector = expression.Compile();
            return sortDirection == ListSortDirection.Ascending
                ? Enumerable.OrderBy(source, keySelector)
                : Enumerable.OrderByDescending(source, keySelector);
        }

         /// <summary>
         /// 按指定属性名对序列进行排序
         /// </summary>
         /// <param name="source">要排序的集合</param>
         /// <param name="propertyName">属性名</param>
         /// <param name="sortDirection"></param>
         /// <returns></returns>
        public static IOrderedEnumerable<T> ThenBy(IOrderedEnumerable<T> source, string propertyName, ListSortDirection sortDirection)
        {
            propertyName.CheckNotNullOrEmpty("propertyName");
            dynamic expression = GetKeySelector(propertyName);
            dynamic keySelector = expression.Compile();
            return sortDirection == ListSortDirection.Ascending
                ? Enumerable.ThenBy(source, keySelector)
                : Enumerable.ThenByDescending(source, keySelector);
        }

        /// <summary>
        ///  按指定属性对序列进行排序
        /// </summary>
        /// <param name="source">要排序的集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy(IQueryable<T> source, string propertyName, ListSortDirection sortDirection)
        {
            propertyName.CheckNotNullOrEmpty("propertyName");
            dynamic keySelector = GetKeySelector(propertyName);
            return sortDirection == ListSortDirection.Ascending
                ? Queryable.OrderBy(source, keySelector)
                : Queryable.OrderByDescending(source, keySelector);
        }


        /// <summary>
        /// 按指定属性对序列进行排序
        /// </summary>
        /// <param name="source">要排序的集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy(IOrderedQueryable<T> source, string propertyName, ListSortDirection sortDirection)
        {
            propertyName.CheckNotNullOrEmpty("propertyName");
            dynamic keySelector = GetKeySelector(propertyName);
            return sortDirection == ListSortDirection.Ascending
                ? Queryable.ThenBy(source, keySelector)
                : Queryable.ThenByDescending(source, keySelector);
        }


        private static LambdaExpression GetKeySelector(string keyName)
        {
            Type type = typeof(T);
            string key = type.FullName + "." + keyName;
            if (Cache.ContainsKey(key)) return Cache[key];

            ParameterExpression param = Expression.Parameter(type);
            string[] propertyNames = keyName.Split('.');
            Expression propertyAcess = param;
            foreach (string propertyName in propertyNames)
            {
                PropertyInfo property = type.GetProperty(propertyName);
                if (property == null)
                {
                    throw new Exception(string.Format(Resource.ObjectExtensions_PropertyNameNotExistsInType, propertyName));
                }
                type=property.PropertyType;
                propertyAcess=Expression.MakeMemberAccess(propertyAcess,property);
            }
            LambdaExpression keySelector = Expression.Lambda(propertyAcess, param);
            Cache[key] = keySelector;
            return keySelector;
        }

    }
}
