using System;
using Remotus.Base;

namespace Remotus.API.v1
{
    public partial class FullCtrlAPI : RestClientBase, IRemotusAPI
    {
        public FullCtrlAPI()
        {
            BaseUri = new Uri("http://localhost:9001/api/v1/");
        }

    }
}
