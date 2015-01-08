using System.Reflection;
using Funq;

namespace ProcHelper
{
    public class HttpAppHost : ServiceStack.AppHostHttpListenerBase
    {
        public HttpAppHost()
            : base("ProcHelper", new Assembly[] { typeof(HttpService).Assembly })
        {
        }

        public HttpAppHost(string handlerPath)
            : base("ProcHelper", handlerPath, new Assembly[] { typeof(HttpService).Assembly })
        {
        }



        public override void Configure(Container container)
        {
            Routes.Add<GetProcessesRequest>("/Process")
                  .Add<GetProcessesRequest>("/Process/{Name}");
        }

    }
}
