using System;

namespace FullCtrl.Base
{
    public interface IClientInfo
    {
        string ClientID { get; }
        Uri ApiAddress { get; }
    }
}
