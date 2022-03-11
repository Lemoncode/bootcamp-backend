using Lemoncode.Soccer.Application;
using Lemoncode.Soccer.Application.Mappers;
using Lemoncode.Soccer.Application.Services;
using Lemoncode.Soccer.Infra.Repository.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Lemoncode.Soccer.WebApi.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSoccerDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddTransient<GamesCommandService>();
            services.AddTransient<GamesQueryService>();
            services.AddSingleton<GameToGameReportMapper>();
            
            // Registro del repositorio a utilizar
            services.AddSingleton<IGamesRepository, InMemoryGamesRepository>();
            
            return services;
        }
    }
}