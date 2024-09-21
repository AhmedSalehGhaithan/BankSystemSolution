using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BankSystem.SharedLibrarySolution.Middleware.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedService<TContext>(
            this IServiceCollection services, IConfiguration config, string fileName) where TContext : DbContext
        {
            // Add generic database context
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(config.GetConnectionString("BankSystemAdvanceProgramming"),
                    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

            // Configure Serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} => {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(
            this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();

            return app;
        }
    }
}
