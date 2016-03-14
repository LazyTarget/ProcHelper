using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using FullCtrl.API.Models;

namespace FullCtrl.API.v1.Controllers
{
    public class ServerController : BaseController
    {
        // todo: methods for registering and managing connected clients

        [HttpPost, HttpPut]
        [Route("api/v1/server/register")]
        public async Task RegisterClient(ClientInfo clientInfo)
        {
            
        }
    }
}
