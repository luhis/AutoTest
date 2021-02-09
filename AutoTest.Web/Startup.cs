using System;
using AutoTest.Web.Extensions;
using AutoTest.Web.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;

namespace AutoTest.Web
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using AutoTest.Persistence;
    using AutoTest.Service.Messages;
    using AutoTest.Web.Authorization;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using WebMarkupMin.AspNetCore3;

    public class Startup
    {
        const string swaggerHash = "Tui7QoFlnLXkJCSl1/JvEZdIXTmBttnWNxzJpXomQjg=";
        const string swagger2Hash = "edNyF0T6h+RbJ9Kl1HXk6KaORyz6MmKnkP3XL/kRb4o=";
        const string googleCom = "https://*.google.com";
        const string googleAnal = "https://www.google-analytics.com";
        private const string baseCssHash = "C7pSgOvwamNBfKv77Bqchu7cIbEY9b+iP5BpHSToCZE=";

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.Configuration = configuration;
            this.AdminEmails = new HashSet<string>(configuration.GetSection("RootAdminIds").Get<IEnumerable<string>>());
            var authSection = configuration.GetSection("Authentication");
            this.ClientSecret = authSection["ClientSecret"];
            this.ClientId = authSection["ClientId"];
            this.env = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        private ISet<string> AdminEmails { get; }
        private const string Authority = "https://accounts.google.com";
        private readonly string ClientSecret;
        private readonly string ClientId;
        private readonly IWebHostEnvironment env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMediatR(typeof(GetClubs).Assembly);
            services.AddPersistence();
            services.AddWeb(this.Configuration);
            services.AddHttpContextAccessor();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = Authority;
                o.Audience = ClientId;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ClientSecret)),

                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAuthorization(o =>
            {
                o.AddPolicy(Policies.Admin, p =>
                {
                    p.RequireAuthenticatedUser();
                    p.RequireClaim(ClaimTypes.Email, this.AdminEmails);
                });
                o.AddPolicy(Policies.ClubAdmin, p =>
                {
                    p.RequireAuthenticatedUser();
                    p.AddRequirements(new ClubAdminRequirement());
                });
                o.AddPolicy(Policies.Marshal, p =>
                {
                    p.RequireAuthenticatedUser();
                    p.AddRequirements(new MarshalRequirement());
                });
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

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyRentals API", Version = "v1" });
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
                    if (env.IsDevelopment())
                    {
                        options.EnableDetailedErrors = true;
                    }
                }).AddMessagePackProtocol();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AutoTestContext autoTestContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseSpaStaticFileCaching();

            app.UseRouting();
            app.UseAuthentication().UseAuthorization();
            app.UseWebMarkupMin();
            app.UseSecurityHeaders(
                policies => policies.AddDefaultSecurityHeaders()
                    .AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 63072000)
                    .RemoveServerHeader().AddContentSecurityPolicy(builder =>
                    {
                        builder.AddDefaultSrc().Self();
                        var scripts = builder.AddScriptSrc().Self()
                            .From(googleCom)
                            .From("https://www.gstatic.com")
                            .From(googleAnal)
                            .WithHash256(swaggerHash)
                            .WithHash256(swagger2Hash);
                        if (env.IsDevelopment())
                        {
                            scripts.UnsafeEval();
                        }

                        builder.AddFrameSource().Self().From(googleCom);
                        var style = builder.AddStyleSrc().Self();
                        if (env.IsDevelopment())
                        {
                            style.UnsafeInline();
                        }
                        else
                        {
                            style.WithHash256(baseCssHash);
                        }

                        var connect = builder.AddConnectSrc().Self().From(googleCom);
                        if (env.IsDevelopment())
                        {
                            connect.From("https://localhost:*");
                        }

                        builder.AddUpgradeInsecureRequests();
                    }));


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ResultsHub>("/resultsHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.OAuthClientId("implicit");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.OAuthConfigObject.ClientId = ClientId;
            });
            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                {
                    spa.Options.SourcePath = "ClientApp/";
                    spa.UseReactDevelopmentServer(npmScript: "dev");
                }
                else
                {
                    spa.Options.SourcePath = "ClientApp/build/";
                }
            });
            autoTestContext.SeedDatabase();
        }
    }
}
