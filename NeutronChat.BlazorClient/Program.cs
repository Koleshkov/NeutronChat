using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NeutronChat.BlazorClient;
using NeutronChat.BlazorClient.Data;
using NeutronChat.BlazorClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var services = builder.Services;
var configurations = builder.Configuration;
var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);


services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

services.AddOptions();

services.AddAuthorizationCore();

services.AddBlazoredLocalStorage();

services.AddHttpClient<IAuthenticationService, AuthenticationService>
(
    "NeutronChatClient",
    client => client.BaseAddress = baseAddress
);

services.AddSingleton<HttpClient>();


await builder.Build().RunAsync();
