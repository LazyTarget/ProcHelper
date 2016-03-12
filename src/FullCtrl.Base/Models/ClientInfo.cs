using System;

namespace FullCtrl.Base
{
    public class ClientInfo : IClientInfo
    {
        public string ClientID { get; set; }
        public Uri ApiAddress { get; set; }
    }
}
