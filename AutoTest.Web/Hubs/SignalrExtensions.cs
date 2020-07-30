 using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;

namespace AutoTest.Web.Hubs
{
    public static class SignalRExtensions
    {
        public static HttpContext? GetHttpContext(this HubCallerContext context) =>
            context
                ?.Features
                .Select(x => x.Value as IHttpContextFeature)
                .FirstOrDefault(x => x != null)
                ?.HttpContext;

        public static T GetQueryParameterValue<T>(this IQueryCollection httpQuery, string queryParameterName) =>
            (httpQuery.TryGetValue(queryParameterName, out var value) && value.Any()
                ? (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T))
                : default)!;
    }
}
