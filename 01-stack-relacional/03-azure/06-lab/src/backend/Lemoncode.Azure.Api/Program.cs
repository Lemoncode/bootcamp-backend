using Microsoft.EntityFrameworkCore;
using Lemoncode.Azure.Api.Data;
using Lemoncode.Azure.Models.Configuration;
using Lemoncode.Azure.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApiDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApiDBContext") ?? throw new InvalidOperationException("Connection string 'ApiDBContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsPolicyName = "EnableCors";
var corsSettings = builder.Configuration.GetSection(nameof(Lemoncode.Azure.Models.Configuration.CorsOptions)).Get<Lemoncode.Azure.Models.Configuration.CorsOptions>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName, 
                      policy =>
                        {
                            policy.AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .WithOrigins(corsSettings?.Origins ?? new[] { "*" });
                        });
});
builder.Services.AddOptions();
builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection(nameof(Lemoncode.Azure.Models.Configuration.StorageOptions)));

var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();
aiOptions.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
aiOptions.DeveloperMode = true;
aiOptions.EnableDebugLogger = true;
builder.Services.AddApplicationInsightsTelemetry(aiOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicyName);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
