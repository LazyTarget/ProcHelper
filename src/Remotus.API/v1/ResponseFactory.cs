using System;
using System.Web.Http;
using Lux.Interfaces;
using Remotus.API.v1.Models;
using Remotus.Base;

namespace Remotus.API.v1
{
    public class ResponseFactory
    {
        public virtual IResponseBase<TResult> CreateResponse<TResult>(ApiController controller, TResult result = default(TResult), IError error = null)
        {
            var response = ResponseBase.Create<TResult>(controller?.Request, result, error);
            return response;
        }
        
        public virtual IResponseBase<TResult> CreateFunctionResponse<TResult>(ApiController controller, IFunctionResult functionResult)
        {
            IResponseBase<TResult> response = null;
            if (functionResult == null)
            {
                response = CreateResponse<TResult>(controller);
            }
            //else if (functionResult.Error != null)
            //{
            //    response = CreateResponse<TResult>(controller, error: functionResult.Error);
            //}
            else
            {
                //IConverter converter = controller?.Configuration?.Services?.GetService(typeof (IConverter)) as IConverter;
                IConverter converter = Lux.Framework.DependencyContainer?.Resolve(typeof (IConverter), "Converter", null) as IConverter;
                var result = default(TResult);
                if (converter != null)
                    result = converter.Convert<TResult>(functionResult.Result);
                else if (functionResult.Result is TResult)
                    result = (TResult) functionResult.Result;
                else if (functionResult.Result != null)
                {
                    throw new InvalidOperationException($"Cannot convert result types. Actual: {functionResult.Result.GetType()}, Expected: {typeof(TResult)}");
                }

                response = CreateResponse<TResult>(controller, result: result, error: functionResult.Error);
            }
            return response;
        }


        public virtual IResponseBaseActionResult CreateActionResult(ApiController controller, IResponseBase responseBase)
        {
            var actionResult = new ResponseBaseActionResult(controller, responseBase);
            return actionResult;
        }
        
        public virtual IResponseBaseActionResult<TResult> CreateActionResult<TResult>(ApiController controller, IResponseBase<TResult> responseBase)
        {
            var actionResult = new ResponseBaseActionResult<TResult>(controller, responseBase);
            return actionResult;
        }

    }
}
