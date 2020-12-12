using Masivian.Roulette.Core;
using Masivian.Roulette.Core.Filters;
using Masivian.Roulette.Domain.Config;
using Masivian.Roulette.Infrastructure.Interfaces;
using Masivian.Roulette.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Masivian.Roulette.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(string.Format("{0}", "/swagger/v1/swagger.json"), "Masivian Roulette API");
                c.RoutePrefix = string.Empty;
            });

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegisteredSwagger(services);
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });

            services.Configure<GlobalSettings>(Configuration);

            services.AddTransient<IRouletteService, RouletteService>();
            services.AddTransient<IRouletteRepository, RouletteRepository>();
        }

        private void RegisteredSwagger(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {

                options.OperationFilter<Swashbuckle.AspNetCore.Filters.SecurityRequirementsOperationFilter>();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Roulette Api",
                    Description = "Roulette Masivian",
                    TermsOfService = new Uri("http://www.masiv.com/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Masivian",
                        Email = string.Empty,
                        Url = new Uri("http://www.masiv.com/"),
                    },
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
    }
}
