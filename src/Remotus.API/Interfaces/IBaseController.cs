using Remotus.API.v1.Models;
using Remotus.Base;

namespace Remotus.API
{
    public interface IBaseController
    {
        ResponseBase<TResult> CreateResponse<TResult>(TResult result = default(TResult), IError error = null);
    }
}
