using System;

namespace Remotus.API.v1
{
    public partial class FullCtrlAPI : RestClientBase
    {
        public FullCtrlAPI()
        {
            BaseUri = new Uri("http://localhost:9001/api/v1/");
        }

    }
}
