using FullCtrl.API.v1.Models;
using FullCtrl.Base;

namespace FullCtrl.API.Interfaces
{
    public interface IBaseController
    {
        ResponseBase<TResult> CreateResponse<TResult>(TResult result = default(TResult), IError error = null);
    }
}
