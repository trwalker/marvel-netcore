﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using marvel_api.Auth;
using marvel_api.Characters;
using marvel_api.Config;
using marvel_api.Rest;

namespace marvel_api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("App_Config/appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"App_Config/appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("App_Config/marvel.auth.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            LoadConfiguration(services);
            RegisterDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());
        }

        private void LoadConfiguration(IServiceCollection services)
        {
            services.Configure<AuthConfigModel>(Configuration.GetSection("Marvel.Auth"));
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddSingleton<ICharacterService, CharacterService>();
            services.AddSingleton<ICredentialsService, CredentialsService>();
            services.AddSingleton<ICharacterCacheService, CharacterCacheService>();
            
            services.AddSingleton<ICharacterRepository, CharacterRepository>();

            services.AddTransient<IHttpClientAdapter, HttpClientAdapter>();
        }
    }
}
