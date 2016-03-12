using System;
using FullCtrl.Base;

namespace FullCtrl.API.v1
{
    public partial class FullCtrlAPI : RestClientBase
    {
        public FullCtrlAPI()
        {
            BaseUri = new Uri("http://localhost:9000/api/v1/");
        }


        public static FullCtrlAPI FromClientInfo(IClientInfo clientInfo)
        {
            var api = new v1.FullCtrlAPI();
            if (clientInfo?.ApiAddress != null)
                api.BaseUri = clientInfo.ApiAddress;
            return api;
        }

    }
}
