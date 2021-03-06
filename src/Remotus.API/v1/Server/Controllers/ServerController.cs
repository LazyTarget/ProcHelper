﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Remotus.Base;

namespace Remotus.API.v1.Server.Controllers
{
    [ControllerCategory("Server")]
    public class ServerController : API.v1.Client.Controllers.BaseController
    {
        // todo: methods for registering and managing connected clients

        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/server/clients")]
        public async Task<IResponseBase<IEnumerable<IClientInfo>>> GetClients()
        {
            IEnumerable<IClientInfo> result = null;
            try
            {
                result = Program.Service?.GetClients();

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
                object result = Program.Service?.RegisterClient(clientInfo);

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
