using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AutoTest.Unit.Test.Fixtures
{
    public static class HttpContextFixture
    {
        public static HttpContext GetHttpContext(IEnumerable<(string, string)> routeValues)
        {
            var ctx = new DefaultHttpContext();
            foreach (var (prop, value) in routeValues)
            {
                ctx.Request.RouteValues.Add(prop, value);
            }

            return ctx;
        }
    }
}
