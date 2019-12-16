using AutoMapper;
using EventManagement.ApplicationCore.Auditing;
using EventManagement.ApplicationCore.Identity;
using EventManagement.ApplicationCore.Interfaces;
using EventManagement.ApplicationCore.Services;
using EventManagement.ApplicationCore.TicketDelivery;
using EventManagement.ApplicationCore.TicketGeneration;
using EventManagement.ApplicationCore.Tickets;
using EventManagement.Identity;
using EventManagement.Infrastructure.Data;
using EventManagement.Infrastructure.Data.Repositories;
using EventManagement.Infrastructure.Identity;
using EventManagement.Infrastructure.Messaging;
using EventManagement.WebApp.Shared.Hangfire;
using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using IdentityServer4;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using static EventManagement.EventManagementConstants;

namespace EventManagement.WebApp
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment Environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EventsDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("EventManagement")));

            services.AddTransient<EventsDbContextSeed>();
            services.AddTransient<EventManagementLocalClientStore>();
            services.TryAddTransient<IUserStore, DatabaseUserStore>();
            services.TryAddTransient<IEventManagementClientStore, DatabaseClientStore>();
            services.TryAddTransient<ITicketsRepository, TicketsRepository>();
            services.TryAddTransient<ITicketDeliveryDataRepository, TicketDeliveryDataRepository>();
            services.TryAddTransient<IAuditEventLog, AuditEventLog>();
            services.TryAddTransient<IEmailService, EmailService>();
            services.TryAddTransient<ITicketNumberService, TicketNumberService>();
            services.TryAddTransient<ITicketDeliveryService, TicketDeliveryService>();
            services.TryAddTransient<IPdfTicketService, PdfTicketService>();
            services.TryAddTransient<ITicketRedirectService, TicketRedirectService>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential(persistKey: true)
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddClientStore<EventManagementClientStore>()
                .AddProfileService<UserProfileService>();

            services.TryAddTransient<IJwtTokenService, JwtTokenService>();

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("EventManagement"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                })
                .UseConsole()
                .UseFilter(new JobContext()));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            // Custom authorization filter for the Hangfire Dashboard.
            services.AddTransient<BackgroundJobsDashboardAuthorizationFilter>();

            services.Configure<RouteOptions>(options =>
            {
                // Generated path urls should be lowercase.
                options.LowercaseUrls = true;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApplicationPart(typeof(AccountController).Assembly)
                .AddJsonOptions(options =>
                {
                    // Important: ASP.NET Core is serializing dates to JSON as local time.
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddLessBundle("css/site.css", "css/site.less");
                pipeline.AddLessBundle("css/ticket-validation.css", "css/ticket-validation.less");
                pipeline.AddLessBundle("css/conference-dialog.css", "conference-dialog/styles.less");

                var confDialogBundler = pipeline
                    .AddBundle("js/conference-dialog.js",
                        "text/javascript; charset=UTF-8",
                        "lib/jquery/jquery.min.js",
                        "lib/handlebars/handlebars.min.js",
                        "conference-dialog/main.js")
                    .Concatenate();

                if (!Environment.IsDevelopment())
                    confDialogBundler.MinifyJavaScript();

            });

            // Configure authentication to protect our web api.
            services
                .AddAuthentication()
                .AddLocalApi(options =>
                {
                    options.ExpectedScope = AdminApi.ScopeName;
                })
                .AddCookie(MasterQrCode.AuthenticationScheme, options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Expiration = TimeSpan.FromDays(1);
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AdminApi.PolicyName, policy =>
                {
                    policy.AddAuthenticationSchemes(
                        IdentityServerConstants.DefaultCookieAuthenticationScheme,
                        IdentityServerConstants.LocalApi.AuthenticationScheme);

                    policy.RequireAuthenticatedUser();
                });
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddOpenApiDocument(document =>
            {
                document.PostProcess = doc =>
                {
                    doc.Info.Title = "Event Management API";
                };

                document.DocumentProcessors.Add(
                    new SecurityDefinitionAppender(
                        "bearer", Enumerable.Empty<string>(),
                        new OpenApiSecurityScheme
                        {
                            Type = OpenApiSecuritySchemeType.OAuth2,
                            Flow = OpenApiOAuth2Flow.Implicit,
                            Flows = new OpenApiOAuthFlows
                            {
                                Implicit = new OpenApiOAuthFlow
                                {
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { AdminApi.ScopeName, AdminApi.DisplayName },
                                    },
                                    AuthorizationUrl = "/connect/authorize",
                                    TokenUrl = "/connect/token"
                                },
                                ClientCredentials = new OpenApiOAuthFlow
                                {
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { AdminApi.ScopeName, AdminApi.DisplayName },
                                    },
                                    TokenUrl = "/connect/token"
                                }
                            },
                        }));

                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("bearer"));
            });
            services.AddAutoMapper(GetType());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                              BackgroundJobsDashboardAuthorizationFilter authorizationFilter)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // show personal identifiable information from access tokens
                // in the logs during development.
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseWebOptimizer();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = "swaggerui",
                    AppName = "Swagger UI",
                    Realm = "Event Management IdP"
                };
            });

            app.UseIdentityServer();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { authorizationFilter }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    // Set an uri (eg: http://localhost:4200) to the environment
                    // variable SPA__Proxy__DevServerBaseUri to forward all incoming
                    // requests to your local SPA development server.
                    // If not set the SPA development server will be started internally
                    // within the ASP.NET Core application.
                    var devServerBaseUri =
                        Configuration["SPA:Proxy:DevServerBaseUri"];

                    if (string.IsNullOrEmpty(devServerBaseUri))
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                    else
                    {
                        spa.UseProxyToSpaDevelopmentServer(devServerBaseUri);
                    }
                }
            });
        }
    }
}