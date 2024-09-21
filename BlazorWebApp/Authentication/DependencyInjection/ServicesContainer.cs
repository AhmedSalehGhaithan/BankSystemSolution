using BlazorWebApp.Authentication.DTOs.Extensions;
using BlazorWebApp.Authentication.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NetcodeHub.Packages.Extensions.LocalStorage;

namespace BlazorWebApp.Authentication.DependencyInjection
{
    public static class ServicesContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddAuthorizationCore();
            services.AddNetcodeHubLocalStorageService();
            services.AddScoped<DTOs.Extensions.LocalStorageService>();
            services.AddScoped<HttpClientService>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddScoped<CustomAuthenticationStateProvider>();
            services.AddTransient<CustomHttpHandler>();
            services.AddCascadingAuthenticationState();
            services.AddHttpClient("WebUiClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7114");
            }).AddHttpMessageHandler<CustomHttpHandler>();

            return services;
        }
    }
}
