using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Lemoncode.Books.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Lemoncode.Books.WebApi",
                    Version = "v1",
                    Description = "Laboratorio API REST de Lemoncodes",
                    Contact = new OpenApiContact
                    {
                        Name = "Diego Martin",
                        Email = "diego.martin@sunnyatticsoftware.com"
                    }
                });

                var xmlCommentsWebApi = Path.Combine(System.AppContext.BaseDirectory, "Lemoncode.Books.WebApi.xml");
                c.IncludeXmlComments(xmlCommentsWebApi);
                var xmlCommentsApplication = Path.Combine(System.AppContext.BaseDirectory, "Lemoncode.Books.Application.xml");
                c.IncludeXmlComments(xmlCommentsApplication);

                c.AddSecurityDefinition(
                    "basic",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "basic",
                        In = ParameterLocation.Header,
                        Description = "Basic Authorization header"
                    });

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[]{}
                        }
                    });
            });

            return services;
        }

    }
}
