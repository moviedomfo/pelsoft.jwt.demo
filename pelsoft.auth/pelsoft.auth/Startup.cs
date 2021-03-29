using pelsoft.auth.Authenticators;
using pelsoft.auth.common;
using pelsoft.auth.common.storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using fwkSec = fwk.security.identity.core;

namespace pelsoft.auth
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
            #region load appSettings

            services.AddMemoryCache();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IStore, Store>();
            services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();

            var mokloging = new pelsoft.auth.helpers.MockLogin();
            Configuration.Bind("mokloging", mokloging);

            var connectionStrings = new Fwk.Database.ConnectionStrings();
            Configuration.Bind("ConnectionStrings", connectionStrings);

            var apiConfig = new apiConfig();
            Configuration.Bind("apiConfig", apiConfig);

            apiAppSettings.connectionStrings = connectionStrings;
            apiAppSettings.apiConfig = apiConfig;

            var providers = new List<Fwk.Security.Identity.jwtSecurityProvider>();
            Configuration.Bind("fwk_securityProviders", providers);

            var secConfig = new Fwk.Security.Identity.secConfig();
            secConfig.fwk_securityProviders = providers;
            secConfig.cnnStrings = connectionStrings;


            fwkSec.SecurityManager.set_secConfig(secConfig);
            #endregion

            services.AddScoped<MockAuthenticator>();
            services.AddScoped<AmazonAuthenticator>();

            // configure DI for application services
            services.AddControllers();

            #region CORS
            //CORS  policy :  global cors policy allow all
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                       builder.WithOrigins("http://192.168.0.74:4200/")
                      .AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
            });

            #endregion
            IdentityModelEventSource.ShowPII = true;

            #region servicios de swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new OpenApiInfo
                {
                    Title = "Konecta api auth ",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            #endregion

            //Permite conservar los nombres de los atributos 
            //Core 3 uses System.Text.Json, which by default does not preserve the case. 
            //Setting the PropertyNamingPolicy to null will fix the problem.
            services.AddControllers()
              .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region servicios middleware de swagger

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "pelsoft auth API v1");
                c.RoutePrefix = "";
            });

            #endregion


            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
