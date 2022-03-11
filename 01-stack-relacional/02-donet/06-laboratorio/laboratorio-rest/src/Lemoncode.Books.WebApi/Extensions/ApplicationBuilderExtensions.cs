using Microsoft.AspNetCore.Builder;

namespace Lemoncode.Books.WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseOpenApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lemoncode.Books.WebApi v1"));
        }
    }
}
