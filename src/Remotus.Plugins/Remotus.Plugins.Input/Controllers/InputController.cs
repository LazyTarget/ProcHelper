using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Remotus.Base;

namespace Remotus.Plugins.Input.Controllers
{
    public class InputController : ApiController
    {
        public InputController()
        {
            
        }

        [HttpGet]
        [Route("api/v1/web/input")]
        public async Task<IHttpActionResult> Index()
        {
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("Hello World!");

                var result = new ResponseMessageResult(response);
                return result;
            }
            catch (Exception ex)
            {
                var response = new InternalServerErrorResult(this);
                return response;
            }
        }
    }
}
