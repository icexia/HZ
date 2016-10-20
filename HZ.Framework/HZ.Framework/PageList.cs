using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework
{
    public class PageList<T>
    {
        //每页数量，总数量，总页数，当前页码
        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int page_size = 0;//每页显示数量

        /// <summary>
        /// 总数量
        /// </summary>
        public int total = 0;//总数量

        /// <summary>
        /// 当前页码
        /// </summary>
        public int page = 0;//当前页码

        /// <summary>
        /// 返回的数据
        /// </summary>
        public List<T> rows = null;

        /// <summary>
        /// 页脚统计数据
        /// </summary>
        public List<T> footer = null;

        /// <summary>
        /// 总页数
        /// </summary>
        public readonly int page_count = 0;//总页数

        /// <summary>
        /// 当前页码
        /// </summary>
        public readonly int page_num = 0;//当前页码

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="num">当前页码</param>
        /// <param name="size">每页显示条数</param>
        /// <param name="count">总数量</param>
        public PageList(int page, int size, int count)
        {
            this.page_size = size;
            this.page = page;
            page_num = (page - 1) * page_size;//计算起始数
            this.total = count;
            if (page_size == 0)
                page_count = 1;
            else if (count % page_size != 0)//计算页码数
                page_count = count / page_size + 1;
            else
                page_count = count / page_size;
        }
        /// <summary>
        /// 无参数构造函数，但是必须为page，size，count赋值
        /// </summary>
        public PageList()
        { }
    }
}
