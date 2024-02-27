using SignalRMessagingTourOfHeroes.Hubs;


const string corsPolicy = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Add SignalR
// builder.Services.AddSignalR();
builder.Services.AddSignalR().AddAzureSignalR();

//Configure CORS
builder.Services.AddCors(options =>
 {
     options.AddPolicy(corsPolicy, builder =>
     builder.WithOrigins("http://localhost:4200","https://calm-tree-0c3af6910-9.centralus.azurestaticapps.net") //It should be Angular URL
     .AllowCredentials()
     .AllowAnyMethod()
     .AllowAnyHeader());
 });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(corsPolicy);

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapHub<SignalRHub>("/messaging");

});

//app.MapRazorPages();

app.Run();
