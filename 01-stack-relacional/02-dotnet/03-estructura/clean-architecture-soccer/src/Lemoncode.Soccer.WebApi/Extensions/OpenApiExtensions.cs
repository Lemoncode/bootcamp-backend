using System.IO;
using Lemoncode.Soccer.Application.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Lemoncode.Soccer.WebApi.Extensions
{
    public static class OpenApiExtensions
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            var mainAssemblyName = typeof(Startup).Assembly.GetName().Name;
            var applicationAssemblyName = typeof(NewGame).Assembly.GetName().Name;


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Lemoncode.Soccer.WebApi", 
                    Version = "v1",
                    Description = "Lemoncode Bootcamp - Arquitectura Limpia",
                    Contact = new OpenApiContact
                    {
                        Name = "Diego Martin",
                        Email = "diego.martin@sunnyatticsoftware.com"
                    }
                });

                var xmlCommentsWebApi = Path.Combine(System.AppContext.BaseDirectory, $"{mainAssemblyName}.xml");
                c.IncludeXmlComments(xmlCommentsWebApi);
                var xmlCommentsApplication = Path.Combine(System.AppContext.BaseDirectory, $"{applicationAssemblyName}.xml");
                c.IncludeXmlComments(xmlCommentsApplication);
            });
            
            return services;
        }

        public static void UseOpenApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lemoncode Soccer v1"));
        }
    }
}