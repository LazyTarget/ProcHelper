using System;

namespace Remotus.Base
{
    public class FunctionResult : IFunctionResult
    {
        protected Type _type;

        public FunctionResult()
        {
            
        }

        public IFunctionArguments Arguments { get; set; }
        public IError Error { get; set; }
        public object Result { get; set; }

        public virtual Type ResultType
        {
            get { return _type ?? Result?.GetType(); }
            set { _type = value; }
        }
    }

    public class FunctionResult<TResult> : FunctionResult, IFunctionResult<TResult>
    {
        public new TResult Result
        {
            get { return (TResult)base.Result; }
            set { base.Result = value; }
        }

        public override Type ResultType
        {
            get { return _type ?? typeof (TResult) ?? Result?.GetType(); }
            set { _type = value; }
        }
    }
}