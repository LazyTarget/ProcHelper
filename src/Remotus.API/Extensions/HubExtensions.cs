using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Hubs;

namespace Remotus.API
{
    public static class HubExtensions
    {
        public static dynamic Groups<T>(this IHubConnectionContext<T> context, params string[] groupNames)
        {
            if (groupNames == null)
                throw new ArgumentNullException(nameof(groupNames));
            var list = new List<string>(groupNames);
            var result = context.Groups(list);
            return result;
        }

    }
}