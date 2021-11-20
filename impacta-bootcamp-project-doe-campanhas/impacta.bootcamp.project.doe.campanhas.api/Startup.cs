using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using impacta.bootcamp.project.doe.campanhas.api.Helpers;
using impacta.bootcamp.project_doe.campanhas.ioc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace impacta.bootcamp.project.doe.campanhas.api
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

            services.AddControllers();
       
            services.AddSwaggerGen(c =>

           {
               var xmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlfile);

               c.IncludeXmlComments(xmlPath);
               c.SwaggerDoc("api", new Microsoft.OpenApi.Models.OpenApiInfo
               {
                   Title = "DOE Campanhas API",
                   Description = "Servico dedicado para as operacoes de campanhas",
                   Contact = new Microsoft.OpenApi.Models.OpenApiContact
                   {
                       Email = "m.mateus.moreira10@gmail.com",
                       Name = "Mateus Moreira"
                   }

               }) ;
               c.AddSecurityDefinition("bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
               {
                   Name = "Authorization",
                   Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                   Scheme = "bearer",
                   In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                   Description = "bearer Authorization header"

               });
               c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
               {
                   { 
                   new  Microsoft.OpenApi.Models.OpenApiSecurityScheme{
                       Reference = new Microsoft.OpenApi.Models.OpenApiReference
                       {
                           Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                           Id="bearer"
                       }
                   }, new string[]{ }
                   }
               });
           });
            services.AddCors();
            services.AddHttpContextAccessor();
            services.Configure<impacta.bootcamp.project_doe.campanhas.core.Entities.Settings.ConnectionStringsSetings>(Configuration.GetSection("ConnectionStrings"));
            services.AddAuthentication("BearerAuthentication").AddScheme<AuthenticationSchemeOptions, BearerAuthenticationHandler>("BearerAuthentication", null);

            ServiceInjection.RegisterServices(services);


            services.Configure<ApiBehaviorOptions>(
                options => { options.SuppressModelStateInvalidFilter = true; }
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseStaticFiles();
            app.UseSwaggerUI(conf => {
                conf.SwaggerEndpoint("swagger/api/swagger.json", "impacta.bootcamp.project.doe.campanhas.api v1");
                conf.RoutePrefix = string.Empty;
                conf.InjectStylesheet("custom.css");
            });  ;

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
