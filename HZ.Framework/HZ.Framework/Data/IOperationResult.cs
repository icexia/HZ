using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.Data
{
    public interface IOperationResult<TResultType>:IOperationResult<TResultType,object>
    {}

    public interface IOperationResult<TResultType, TData>
    {
        /// <summary>
        ///获取或设置结果类型 
        /// </summary>
        TResultType ResultType { get; set; }


        /// <summary>
        /// 获取或设置返回信息
        /// </summary>
        string Message { get; set; }


        /// <summary>
        /// 获取或设置结果数据
        /// </summary>
        TData Data { get; set; }
    }
}
