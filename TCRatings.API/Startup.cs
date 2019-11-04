using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TCRatings.DAL;

namespace TCRatings.API
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
            // Use SQL Database if in Azure, otherwise, use SQLite
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                services.AddDbContext<RatingsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("RatingsContext")));
            else
                services.AddDbContext<RatingsContext>(options => options.UseSqlite("Data Source=localdatabase.db"));



            // Automatically perform database migration
            services.BuildServiceProvider().GetService<RatingsContext>().Database.Migrate();

            services.AddControllers();

            // Register the Swagger services
            services.AddOpenApiDocument(doc =>
                {
                    doc.DocumentName = "v1";
                    doc.Title = "TC Ratings API";
                    doc.Description = "A simple api for TC CS-03 Hackathon.";                   
                }
            );

            /* // Register the Swagger generator, defining 1 or more Swagger documents
             services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new OpenApiInfo
                 {
                     Title = "TC Ratings API",
                     Version = "v1",
                     Description = "An API to handle service feedback ratings and comments."
                 });

                 // Set the comments path for the Swagger JSON and UI.
                 var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                 var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                 c.IncludeXmlComments(xmlPath);
             });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Register the Swagger generator, the Swagger UI, and ReDoc middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseReDoc(opt => {
                opt.DocumentPath = "/swagger/v1/swagger.json";
                opt.Path = "/redoc";
              }
            );


            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            /* app.UseSwaggerUI(c =>
             {
                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "TC Ratings API v1");
             });*/

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
