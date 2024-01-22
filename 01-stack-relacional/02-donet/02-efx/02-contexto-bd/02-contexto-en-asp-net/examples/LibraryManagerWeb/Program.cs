using LibraryManagerWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddDbContext<LibraryContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions =>
	{
		sqlServerOptions.EnableRetryOnFailure();
	});
	options.EnableSensitiveDataLogging(true);
})
    .AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
