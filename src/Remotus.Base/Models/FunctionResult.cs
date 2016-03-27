using System;

namespace Remotus.Base
{
    public class FunctionResult : IFunctionResult
    {
        private Type _type;

        public FunctionResult()
        {
            
        }

        public IFunctionArguments Arguments { get; set; }
        public IError Error { get; set; }
        public object Result { get; set; }

        public Type ResultType
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
    }
}