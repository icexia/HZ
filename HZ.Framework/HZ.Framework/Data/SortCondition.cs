﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.Data
{
    public  class SortCondition
    {
        public SortCondition(string sortField)
            : this(sortField, ListSortDirection.Ascending)
        { }

        public SortCondition(string sortField, ListSortDirection listSortDirection)
        {
            SortField = sortField;
            ListSortDirection = listSortDirection;
        }

        /// <summary>
        /// 获取或设置排序字段名称
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        ///获取或设置排序方向 
        /// </summary>
        public ListSortDirection ListSortDirection { get; set; }
    }

    public class SortCondition<T> : SortCondition
    { 
        /// <summary>
        /// 使用排序字段 初始化一个<see cref="SortCondition"/>类型的新实例
        /// </summary>
        public SortCondition(Expression<Func<T, object>> keySelector)
            : this(keySelector, ListSortDirection.Ascending)
        { }

        /// <summary>
        /// 使用排序字段与排序方式 初始化一个<see cref="SortCondition"/>类型的新实例
        /// </summary>
        public SortCondition(Expression<Func<T, object>> keySelector, ListSortDirection listSortDirection)
            : base(GetPropertyName(keySelector), listSortDirection)
        { }

        /// <summary>
        /// 从泛型委托获取属性名
        /// </summary>
        private static string GetPropertyName(Expression<Func<T, object>> keySelector)
        {
            string param = keySelector.Parameters.First().Name;
            string operand = (((dynamic)keySelector.Body).Operand).ToString();
            operand = operand.Substring(param.Length + 1, operand.Length - param.Length - 1);
            return operand;
        }
    }
}
