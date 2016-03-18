using System.Web.Http;
using Remotus.Base;

namespace Remotus.API.v1.Models
{
    public interface IResponseBaseActionResult : IHttpActionResult
    {
        IResponseBase Response { get; }
    }
}
