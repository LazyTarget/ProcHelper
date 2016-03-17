using System;

namespace Remotus.Base
{
    public class ApiEndpoint : IApiEndpoint
    {
        public Uri BaseAddress { get; set; }
    }
}