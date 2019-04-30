using AutoMapper;
using EventManagement.DataAccess;
using EventManagement.Identity;
using EventManagement.WebApp.Configuration;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSwag.AspNetCore;

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
            services.AddTransient<EventsDbInitializer>();

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

            services.AddIdentityServer()
                .AddDeveloperSigningCredential(persistKey: true)
                .AddInMemoryApiResources(IdentityServerConfig.GetApis())
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddProfileService<UserProfileService>();
            services.AddTransient<IUserStore, UserStore>();

            // The authentication is to protect the web api.
            services.AddAuthentication()
                .AddIdentityServerAuthentication(Constants.JwtAuthScheme, options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.ApiName = "eventmanagement.admin";
                    options.RequireHttpsMetadata = false;
                });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSwaggerDocument();
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, EventsDbContext dbContext, EventsDbInitializer dbInitializer)
        {
            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                dbInitializer.EnsureData(new TestData());
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUi3();

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
