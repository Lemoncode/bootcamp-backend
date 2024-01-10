using Lemoncode.Soccer.WebApi.Extensions;
using Lemoncode.Soccer.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lemoncode.Soccer.WebApi
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
                
            services
                .AddOpenApi()
                .AddSoccerDependencies();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMiddleware<BasicAuthMiddleware>(); // basic auth middleware

            app.UseDeveloperExceptionPage();
            app.UseOpenApi();

            app.UseMiddleware<BasicAuthMiddleware>();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}