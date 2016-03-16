using System;

namespace FullCtrl.API.v1
{
    public partial class FullCtrlAPI : RestClientBase
    {
        public FullCtrlAPI()
        {
            BaseUri = new Uri("http://localhost:9000/api/v1/");
        }

    }
}
