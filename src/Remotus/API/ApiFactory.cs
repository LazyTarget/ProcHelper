using System;
using FullCtrl.Base;

namespace FullCtrl.API
{
    public class ApiFactory
    {
        //public IFullCtrlAPI Create(IClientInfo clientInfo)
        //{
        //    throw new NotImplementedException();
        //}

        public API.v1.FullCtrlAPI Create_V1(IClientInfo clientInfo)
        {
            var api = new API.v1.FullCtrlAPI();
            if (clientInfo?.ApiAddress != null)
                api.BaseUri = clientInfo.ApiAddress;
            return api;
        }

    }
}
