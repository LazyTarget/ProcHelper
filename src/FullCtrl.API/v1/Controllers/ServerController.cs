using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using FullCtrl.API.Models;
using FullCtrl.Base;

namespace FullCtrl.API.v1.Controllers
{
    public class ServerController : BaseController
    {
        // todo: methods for registering and managing connected clients

        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/server/clients")]
        public async Task<IResponseBase<IEnumerable<IClientInfo>>> GetClients()
        {
            IEnumerable<IClientInfo> result = null;
            try
            {
                result = Program._server?.GetClients();

                var response = CreateResponse(result);
                return response;
            }
            catch (Exception ex)
            {
                var response = CreateError<IEnumerable<IClientInfo>>(DefaultError.FromException(ex));
                return response;
            }
        }


        [HttpPost, HttpPut]
        [Route("api/v1/server/clients/register")]
        public async Task<IResponseBase<object>> RegisterClient(ClientInfo clientInfo)
        {
            try
            {
                object result = Program._server?.RegisterClient(clientInfo);

                var response = CreateResponse(result);
                return response;
            }
            catch (Exception ex)
            {
                var response = CreateError<object>(DefaultError.FromException(ex));
                return response;
            }
        }
    }
}
