using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.Data
{
    public enum OperationResultType
    {
        [Description("输入信息验证失败.")]
        ValidError,

        [Description("指定参数的数据源不存在.")]
        QueryNull,

        [Description("操作没引起任何变化,提交取消.")]
        NoChanged,

        [Description("操作成功")]
        Success,

        [Description("操作引发的错误")]
        Error,

        [Description("警告")]
        Warning,

        [Description("数据已存在.")]
        Exist
    }
}
