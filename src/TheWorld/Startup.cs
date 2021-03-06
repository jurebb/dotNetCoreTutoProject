﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWorld.Services;
using TheWorld.Models;
using AutoMapper;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TheWorld
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)      
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        //meth which sets up service containers, container for services that diff. parts  of thos app require
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config); //da bi injektirali u AppController()

            if(_env.IsEnvironment("Development") || _env.IsEnvironment("Testing"))
            {
                services.AddScoped<IMailService, DebugMailService>(); //injektira implementaciju interfacea IMailService (DebugMailService) pri aktivaciji kontrolera (App, za HttpPost)
            }
            else
            {
                //implement real mail service
            }

            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.User.RequireUniqueEmail = true;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                //API authentification
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()                    //API authentification
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if(ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();
                    }
                };
            })
                .AddEntityFrameworkStores<WorldContext>();

            services.AddLogging();
            services.AddDbContext<WorldContext>();
            services.AddScoped<IWorldRepository, WorldRepository>();
            services.AddTransient<WorldContextSeedData>();
            services.AddTransient<ShamGeoService>();
            services.AddMvc(config =>
            {
                if (_env.IsProduction())
                {
                    config.Filters.Add(new RequireHttpsAttribute());
                }
            })/*.
                AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();          //MVC now uses camel case by default
                })*/;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //meth - what to do when requests come in (every time), middleware
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, WorldContextSeedData seeder)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<TripsViewModel, Trip>().ReverseMap();
                config.CreateMap<StopsViewModel, Stop>().ReverseMap();
            });

            loggerFactory.AddConsole();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error);
            }
            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
                    );
            });

            seeder.EnsureSeedData().Wait();
        }
    }
}
