using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
                    var path = r.File.PhysicalPath;
                    var cacheExtensions =
                        new[] { ".css", ".js", ".gif", ".jpg", ".png", ".svg", ".ico", ".json" };
                    if (cacheExtensions.Any(path.EndsWith))
                    {
                        var maxAge = TimeSpan.FromDays(7);
                        r.Context.Response.Headers.Add(
                            "Cache-Control",
                            "max-age=" + maxAge.TotalSeconds.ToString("0"));
                    }
                }
            };
            app.UseSpaStaticFiles(sfOptions);

            return app;
        }
    }
}
