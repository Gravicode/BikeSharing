using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BikeSharing.Service.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;

namespace BikeSharing.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var redis = new RedisDB(Configuration.GetConnectionString("RedisCon"));
            var sqlConnectionString = Configuration.GetConnectionString("MySqlCon");

            services.AddDbContext<BikeSharingDB>(options =>
                options.UseMySql(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("BikeSharing.Service")
                )
            );
            services.AddSingleton<RedisDB>((x) => redis);
            //services.AddCors();

            services.AddMvcCore()
            //.AddAuthorization()
            //.AddJsonFormatters()
            .AddApiExplorer();

            //services.AddAuthentication("Bearer")
            //.AddIdentityServerAuthentication(options =>
            //{
            //    options.Authority = Configuration.GetValue<string>("server:identityurl");
            //    options.RequireHttpsMetadata = false;
            //    options.ApiName = Configuration.GetValue<string>("server:apiname");
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "BikeSharing Data API",
                    Version = "v1",
                    Description = "Rest API for accessing data",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "BikeSharing Team", Email = "team@BikeSharing.com", Url = "http://twitter.com/BikeSharing" },
                    License = new License { Name = "For developers only", Url = "http://BikeSharing.com" }
                });
            });
            services.AddDistributedMemoryCache();

            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromSeconds(10);
            //    options.CookieHttpOnly = true;
            //});

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //app.UseSession();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseIdentity();
            app.UseWebSockets();

            //app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "{controller=Default}/{action=Home}/{id?}"
                );
            });

            app.UseSwagger();

            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BikeSharing Data API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseMvc();
        }
    }
}