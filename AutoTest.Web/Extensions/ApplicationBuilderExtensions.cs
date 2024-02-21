using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace AutoTest.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSpaStaticFileCaching(this IApplicationBuilder app)
        {
            var sfOptions = new StaticFileOptions
            {
                OnPrepareResponse = r =>
                {
                    var path = r.File.PhysicalPath ?? "";
                    var cacheExtensions =
                        new[] { ".css", ".js", ".gif", ".jpg", ".png", ".svg", ".ico", ".json" };
                    if (cacheExtensions.Any(path.EndsWith))
                    {
                        var maxAge = TimeSpan.FromDays(7);
                        _ = r.Context.Response.Headers.Append(KeyValuePair.Create<string, StringValues>(
                            "Cache-Control",
                            $"max-age={maxAge.TotalSeconds: 0}"));
                    }
                }
            };
            app.UseSpaStaticFiles(sfOptions);

            return app;
        }
    }
}
