using Lemoncode.Soccer.Application;
using Lemoncode.Soccer.Application.Mappers;
using Lemoncode.Soccer.Application.Services;
using Lemoncode.Soccer.Infra.Repository.EfCore;
using Lemoncode.Soccer.Infra.Repository.InMemory;
using Microsoft.EntityFrameworkCore;
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

            const string connectionString = "Server=localhost;Database=efcore_soccer_api;user=sa;password=Lem0nCode!";
            services.AddDbContext<SoccerContext>(options =>
                options.UseSqlServer(connectionString));

            // Registro del repositorio a utilizar
            //services.AddSingleton<IGamesRepository, InMemoryGamesRepository>();
            services.AddTransient<IGamesRepository, EntityFrameworkRepository>();

            return services;
        }
    }
}