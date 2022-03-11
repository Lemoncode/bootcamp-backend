using Lemoncode.Books.Application;
using Lemoncode.Books.Application.Services;
using Lemoncode.Books.WebApi.Extensions;
using Lemoncode.Books.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lemoncode.Books.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOpenApi();
            services.AddControllers();

            var booksConnectionString = _configuration.GetValue<string>("ConnectionStrings:BooksDatabase");
            services.AddDbContext<BooksDbContext>(options => options.UseSqlServer(booksConnectionString));

            services.AddTransient<CommandService>();
            services.AddTransient<QueryService>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
            app.UseMiddleware<BasicAuthMiddleware>();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
