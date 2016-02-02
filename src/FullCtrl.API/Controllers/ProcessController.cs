using System.Collections.Generic;
using System.Web.Http;

namespace FullCtrl.API.Controllers
{
    public class ProcessController : ApiController
    {
        // GET api/process
        public IEnumerable<string> Get()
        {
            return new List<string> { {"qwerty"}, {"azerty"} };
        }

    }
}
