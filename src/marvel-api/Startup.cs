using System;
using marvel_api.Auth;
using marvel_api.Characters;
using marvel_api.Config;
using marvel_api.Rest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace marvel_api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("App_Config/appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"App_Config/appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("App_Config/marvel.auth.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            ServiceProvider = serviceProvider;
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ServiceProvider { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddOptions();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            });
            
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
