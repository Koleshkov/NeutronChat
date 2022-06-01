using NeutronChat.Application;
using NeutronChat.Application.Common.Mappings;
using NeutronChat.Application.Services;
using NeutronChat.Domain.Configurations;
using NeutronChat.Persistance;
using NeutronChat.WebApi.Middlewares;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Custom injection.

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IAppDbContext).Assembly));
});

builder.Services.AddApplication(builder.Configuration);

builder.Services.AddPersistance(builder.Configuration);

builder.Services.Configure<AuthenticationConfiguration>(
builder.Configuration.GetSection("Authentication"));

//ASP.Net Core injection

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

//Build
var app = builder.Build();




//Middlewares

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
