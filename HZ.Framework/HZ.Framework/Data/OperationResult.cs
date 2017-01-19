using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZ.Framework.Data
{
    public abstract class OperationResult<TResultType,TData>:IOperationResult<TResultType,TData>
    {

        protected OperationResult()
            : this(default(TResultType))
        { }


        protected OperationResult(TResultType type)
            : this(type, default(TData), null)
        { }


        protected OperationResult(TResultType type, string message)
            : this(type, default(TData), message)
        { }

        protected string _message;

        public virtual string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public TResultType ResultType { get; set; }

        public TData Data { get; set; }

        protected OperationResult(TResultType type, TData data, string message)
        {
            ResultType = type;
            Data = data;
            _message = message;
        }
    }

    public abstract class OperationResult<TResultType> : OperationResult<TResultType, object>, IOperationResult<TResultType>
    {
        protected OperationResult() : this(default(TResultType)) { }

        protected OperationResult(TResultType type)
            : this(type, null, null)
        { }

        protected OperationResult(TResultType type, string message)
            : this(type, null, message)
        { }

        protected OperationResult(TResultType type, object data, string message)
            : base(type, data, message)
        { }
    }
}
