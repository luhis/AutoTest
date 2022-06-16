using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AutoTest.Web.Authorization
{
    public class OAuth2OperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var isAuthorized = context.MethodInfo.DeclaringType != null && (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                                                                            context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any());

            if (!isAuthorized) return;

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            var oauth2SecurityScheme = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" },
            };

            operation.Security.Add(new OpenApiSecurityRequirement()
            {
                [oauth2SecurityScheme] = new[] { "email" }
            });
        }
    }
}