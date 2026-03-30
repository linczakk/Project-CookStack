using CookStackClient;
using CookStackClient.Services;
using CookStackClient.Services.ToastMessage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7107/") });
builder.Services.AddScoped<RecipesApiClient>();
builder.Services.AddScoped<ShoppingListApiClient>();
builder.Services.AddScoped<LoadingService>();
builder.Services.AddSingleton<ToastService>();


await builder.Build().RunAsync();
