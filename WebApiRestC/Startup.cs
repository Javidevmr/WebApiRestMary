using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//si se quiere swagger es necesaria esta libreria
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApiRestC.Models;
//using WebApiRestC.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;


namespace WebApiRestC
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
            //DB : In Memory = database temporal
            //services.AddDbContext<DatosTraceDBContext>(opt =>
            //                                opt.UseInMemoryDatabase("TodoList"));

            services.AddDbContext<DatosTraceDBContext>(opt =>
                                opt.UseSqlServer("Server=JAVILAB\\SQLSERVER2019DEV;Database=DatosTraceDB;User ID=IDENTI;Password=SOFT"));

            services.AddControllers();

            services.AddHealthChecks()
                                    .AddSqlServer(connectionString: Configuration.GetConnectionString("StoreDBContext"),
                              healthQuery: "SELECT 1;",
                              name: "Sql Server",
                              failureStatus: HealthStatus.Degraded);

            services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.AddHealthCheckEndpoint("Basic healthcheck", "http://localhost:44315/healthcheck");
            });

            //swagger
            //agregar el using - verificar la version
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                //Se agrega logger
                logger.LogInformation("En entorno de desarrollo");
                //Excepcion para entorno con o sin swagger y logger
                app.UseDeveloperExceptionPage();
                //Se agrega Swagger
                //Si tienes un json esto no seria necesario - consultar documentacion
                //¿A que se refiere?
                app.UseSwagger();

                app.UseSwaggerUI(c =>

                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiRestC v1");
                });
                            

            }

            //Se agrega Swagger
            //Si tienes un json esto no seria necesario - consultar documentacion
            //¿A que se refiere?

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Habilito la página de health y habilitamos el uso del HealChecksUI
            app.UseHealthChecks("/healthcheck", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            })
            .UseHealthChecksUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
