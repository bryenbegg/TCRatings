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
using TCRatings.API.Models;
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
            services.AddDbContext<RatingsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("RatingsContext")));

            // Automatically perform database migration
            services.BuildServiceProvider().GetService<RatingsContext>().Database.Migrate();

            // Try to seed the database (for Azure Web App that doesnt run Program.cs)
            SeedData.Initialize(services.BuildServiceProvider());

            // Add the API controllers
            services.AddControllers();

            // Register the OpenAPI/Swagger document services
            services.AddOpenApiDocument(doc =>
                {
                    doc.DocumentName = "v1";
                    doc.Title = "TC Ratings API";
                    doc.Description = "A simple api for TC CS-03 Hackathon.";                   
                }
            );

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
            app.UseOpenApi(); // host the document
            app.UseSwaggerUi3(); // host /swagger UI
            app.UseReDoc(opt => {
                opt.DocumentPath = "/swagger/v1/swagger.json";
                opt.Path = "/redoc";
              }
            ); // Host the /redoc UI (alternative look and feel)

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
