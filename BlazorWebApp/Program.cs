using BlazorWebApp;
using BlazorWebApp.Authentication.DependencyInjection;
using BlazorWebApp.Authentication.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<BankAccountService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7114");
});


builder.Services.AddHttpClient<TransactionService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7114");
});

builder.Services.AddApplicationService();

await builder.Build().RunAsync();

