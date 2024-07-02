using Microsoft.EntityFrameworkCore;
using tour_of_heroes_api.Models;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IHeroRepository, HeroRepository>();
builder.Services.AddControllers(); builder.Services.AddDbContext<HeroContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "tour_of_heroes_api", Version = "v1" });
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.UseDeveloperExceptionPage();

app.Run();

