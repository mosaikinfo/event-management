using AutoMapper;
using EventManagement.ApplicationCore.Interfaces;
using EventManagement.ApplicationCore.Services;
using EventManagement.Identity;
using EventManagement.Infrastructure.Data;
using IdentityServer4;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System;

namespace EventManagement.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EventsDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("EventManagement")));

            services.AddTransient<EventsDbContextSeed>();

            services.TryAddTransient<IUserStore, UserStore>();
            services.TryAddTransient<ITicketNumberService, TicketNumberService>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential(persistKey: true)
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddClientStore<EventManagementLocalClientStore>()
                .AddProfileService<UserProfileService>();

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
            });

            // Configure authentication to protect our web api.
            services
                .AddAuthentication()
                .AddLocalApi(options =>
                {
                    options.ExpectedScope = "eventmanagement.admin";
                })
                .AddCookie(Constants.MasterQrCodeAuthenticationScheme, options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Expiration = TimeSpan.FromDays(1);
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.EventManagementApiPolicy, policy =>
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

            services.AddOpenApiDocument(options =>
            {
                options.PostProcess = doc =>
                {
                    doc.Info.Title = "Event Management API";
                };
            });
            services.AddAutoMapper(GetType());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseSwaggerUi3(options =>
            {
                options.WithCredentials = true;
            });

            app.UseIdentityServer();

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
                    // spa.UseAngularCliServer(npmScript: "start");

                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}