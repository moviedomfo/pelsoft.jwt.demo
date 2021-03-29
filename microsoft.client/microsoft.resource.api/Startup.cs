using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using microsoft.resource.api.common;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.AspNetCore.Internal;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using fwkSec = fwk.security.identity.core;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using microsoft.resource.api.service;

namespace microsoft.resource.api
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

            var connectionStrings = new Fwk.Database.ConnectionStrings();
            Configuration.Bind("ConnectionStrings", connectionStrings);

            var apiConfig = new apiConfig();
            Configuration.Bind("apiConfig", apiConfig);      //  <--- This


            apiAppSettings.connectionStrings = connectionStrings;
            apiAppSettings.apiConfig = apiConfig;


            //var providers = new List<Fwk.Security.Identity.jwtSecurityProvider>();
            //Configuration.Bind("fwk_securityProviders", providers);

            var secConfig = new Fwk.Security.Identity.secConfig();
            //secConfig.fwk_securityProviders = providers;
            secConfig.cnnStrings = connectionStrings;


            fwkSec.SecurityManager.set_secConfig(secConfig);

            #endregion

            // configure DI for application services
            services.AddScoped<ITestService, TestService>();
  
            services.AddControllers();
            #region configure jwt authentication


            //  var appSettings = appSettingsSection.Get<serverSettings>();
            //var key = Encoding.UTF8.GetBytes(apiAppSettings.apiConfig.api_secretKey);
            var secretKey = Encoding.ASCII.GetBytes(apiAppSettings.apiConfig.api_secretKey);

            var securityKey = new SymmetricSecurityKey(secretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.Events = new JwtBearerEvents()
                {
                    //captura el evento de autenticaci�n
                    OnTokenValidated = context =>
                    {

                        return Task.CompletedTask;
                    }
                 
                };

                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.SaveToken = true;

                //https://tools.ietf.org.html/rfc7519
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    SaveSigninToken = true,
                    ValidateActor = true,
                    ValidateIssuer = false, //emisor
                    ValidateAudience = true, //destinatarios del token 
                    ValidateLifetime = true,//
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    RequireExpirationTime = false,
                    ValidIssuer = apiAppSettings.apiConfig.api_issuerToken,
                    ValidAudience = apiAppSettings.apiConfig.api_audienceToken,
                    

                };
            });

            #endregion


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
            #region servicios  de swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new OpenApiInfo
                {
                    Title = "microsoft Whatsapp Service API ",
                    Version = "v1"
                });
            });
            
            #endregion

            //Permite conservar los nombres de los atributos 
            //Core 3 uses System.Text.Json, which by default does not preserve the case. 
            //Setting the PropertyNamingPolicy to null will fix the problem.
            services.AddControllers()
              .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            
            //services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region servicios meidleware de swagger




            app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        //https://localhost:44359/swagger/v1/swagger.json
        //https://localhost:44351/index.html
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "microsoft Whatsapp Service API");
                c.RoutePrefix = "";

            });


           
            #endregion


            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();
            //app.UseStaticFiles();

            //requiere store folder
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"store")),
            //    RequestPath = new PathString("/store")
            //});

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
