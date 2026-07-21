using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Web.Authorization;
using AutoTest.Web.Authorization.Attributes;
using AutoTest.Web.Extensions;
using AutoTest.Web.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using WebMarkupMin.AspNetCoreLatest;

namespace AutoTest.Web;

public class Startup
{
    private const string SwaggerHash = "A9ZGkjzNbSHK5HWS6UkGpaaIuyNt/7a8gVIu6p70YPo=";
    private const string Swagger2Hash = "edNyF0T6h+RbJ9Kl1HXk6KaORyz6MmKnkP3XL/kRb4o=";
    private const string GoogleCom = "https://*.google.com";
    private const string GoogleAnal = "https://www.google-analytics.com";
    private readonly IReadOnlyList<string> _baseCssHashs = ["jwMoKfjpMtCZvgc6jvf++3CnNz9TZRnk6Xn0fh2uX3E=", "lmto2U1o7YINyHPg9TOCjIt+o5pSFNU/T2oLxDPF+uw="];
    private readonly IReadOnlyList<string> _toastHashes = ["E/nvqET/9zpctDshjbx7JreRM/gAx3JcoKF+f+rglGY=", "u3OrwPmUPyFEOg2MH8iSt1Kq+OEIL7vVcAdbanb0T68="];

    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;
        AdminEmails = new HashSet<string>(configuration.GetSection("RootAdminIds").Get<IEnumerable<string>>()!);
        var authSection = configuration.GetSection("Authentication");
        _clientSecret = authSection["ClientSecret"] ?? "";
        _clientId = authSection["ClientId"] ?? "";
        _env = webHostEnvironment;
    }

    public IConfiguration Configuration { get; }
    private ISet<string> AdminEmails { get; }
    private const string Authority = "https://accounts.google.com";
    private readonly string _clientSecret;
    private readonly string _clientId;
    private readonly IWebHostEnvironment _env;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews(o => o.AllowEmptyInputInBodyModelBinding = true);
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Transient);
        services.AddPersistence();
        services.AddWeb(Configuration);
        services.AddHttpContextAccessor();
        if (_env.IsProduction())
        {
            services.AddApplicationInsightsTelemetry();
        }
        services.AddMemoryCache();
        services.AddHealthChecks();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https://localhost:44357", "http://localhost:44357");
            });
        });

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.Authority = Authority;
            o.Audience = _clientId;
            o.RequireHttpsMetadata = false;
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_clientSecret)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            o.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/authorisationHub"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.Admin, p =>
            {
                p.RequireAuthenticatedUser();
                p.RequireClaim(ClaimTypes.Email, AdminEmails);
            })
            .AddPolicy(Policies.ClubAdmin, p =>
            {
                p.RequireAuthenticatedUser();
                p.AddRequirements(new ClubAdminRequirement());
            })
            .AddPolicy(Policies.ClubAdminOrSelf, p =>
            {
                p.RequireAuthenticatedUser();
                p.AddRequirements(new ClubAdminOrSelfRequirement());
            })
            .AddPolicy(Policies.Marshal, p =>
            {
                p.RequireAuthenticatedUser();
                p.AddRequirements(new MarshalRequirement());
            });

        services.AddWebMarkupMin(options =>
            {
                options.AllowMinificationInDevelopmentEnvironment = false;
                options.AllowCompressionInDevelopmentEnvironment = false;
            })
            .AddHtmlMinification(
                options =>
                {
                    options.MinificationSettings.RemoveRedundantAttributes = true;
                    options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                    options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
                })
            .AddHttpCompression();

        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/build";
        });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoTest API", Version = "v1" });
            c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    Implicit = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/auth"),
                        Scopes = new Dictionary<string, string> { { "email", "View Email" }, { "profile", "View Profile" } }
                    },
                }
            });
            c.OperationFilter<OAuth2OperationFilter>();
        });

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });

        services.AddSignalR(options =>
            {
                if (_env.IsDevelopment())
                {
                    options.EnableDetailedErrors = true;
                }
            }).AddMessagePackProtocol();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionFeature != null)
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "An error occurred while processing your request.",
                    };
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/problem+json";
                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            });
        });

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        if (!env.IsProduction())
        {
            app.UseHttpsRedirection();
        }
        app.UseResponseCompression();
        app.UseSpaStaticFileCaching();

        app.UseRouting();
        app.UseCors();
        app.UseAuthentication().UseAuthorization();
        app.UseWebMarkupMin();
        app.UseSecurityHeaders(
            policies => policies.AddDefaultSecurityHeaders()
                .AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 63072000)
                .RemoveServerHeader()
                .AddCrossOriginOpenerPolicy(x => x.SameOriginAllowPopups())
                .AddContentSecurityPolicy(builder =>
                {
                    builder.AddDefaultSrc().Self();
                    var scripts = builder.AddScriptSrc().Self()
                        .From(GoogleCom)
                        .From("https://www.gstatic.com")
                        .From(GoogleAnal)
                        .WithHash256(SwaggerHash)
                        .WithHash256(Swagger2Hash);
                    if (env.IsDevelopment())
                    {
                        scripts.UnsafeEval();
                    }

                    builder.AddFrameSrc().Self().From(GoogleCom);
                    var style = builder.AddStyleSrc().Self().From(GoogleCom);
                    if (env.IsDevelopment())
                    {
                        style.UnsafeInline();
                    }
                    else
                    {
                        style.UnsafeHashes();
                        foreach (var h in _baseCssHashs)
                        {
                            style.WithHash256(h);
                        }
                        foreach (var h in _toastHashes)
                        {
                            style.WithHash256(h);
                        }
                    }

                    var connect = builder.AddConnectSrc().Self()
                        .From(GoogleCom)
                        .From("https://js.monitor.azure.com")
                        .From("https://dc.services.visualstudio.com");
                    if (env.IsDevelopment())
                    {
                        connect.From("https://localhost:*").From("ws://localhost:*");
                    }

                    builder.AddUpgradeInsecureRequests();
                }));

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<EventHub>("/resultsHub");
            endpoints.MapHub<AuthorisationHub>("/authorisationHub");
            endpoints.MapHealthChecks("/healthz");
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
        });
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.OAuthConfigObject.ClientId = _clientId;
            });
        }
        app.UseSpa(spa =>
        {
            if (env.IsDevelopment())
            {
                spa.Options.SourcePath = "../ClientApp/";
                spa.UseReactDevelopmentServer(npmScript: "dev");
            }
            else
            {
                spa.Options.SourcePath = "ClientApp/build/";
            }
        });
    }
}
