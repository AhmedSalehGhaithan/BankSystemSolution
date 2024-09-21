using BankSystem.Application.BankAccountConcreteDecorators;
using BankSystem.Application.Interface;
using BankSystem.Domain.Authentication;
using BankSystem.Infrastructure.BankAccountConcreteDecorators;
using BankSystem.Infrastructure.Data;
using BankSystem.Infrastructure.Repositories;
using BankSystem.SharedLibrarySolution.Middleware.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BankSystem.Application.TransactionConcreteDecorators;
using BankSystem.Infrastructure.TransactionConcreteDecorators;
public static class ServiceContainer
{
    
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration _config)
        {
            SharedServiceContainer.AddSharedService<BankAccountDbContext>(services, _config, _config["MySerilog:FileName"]!);

            services.AddScoped<BankAccountRepository>(); 
            services.AddScoped<TransactionRepository>(); 
        
            // Register cache service
            services.AddScoped<IMyCacheService, MyCacheService>();

            // Register decorators
            services.AddScoped<IBankAccount>(provider =>
                new LoggingBankAccountServiceDecorator(
                    new CachingBankAccountServiceDecorator(
                        provider.GetRequiredService<BankAccountRepository>(),
                        provider.GetRequiredService<IMyCacheService>()
                    )
                )
            );

        services.AddScoped<ITransaction>(provider =>
           new LoggingTransactionServiceDecorator(
               new CachingTransactionServiceDecorator(
                   provider.GetRequiredService<TransactionRepository>(),
                   provider.GetRequiredService<IMyCacheService>()
               )
           )
       );

        services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<BankAccountDbContext>()
                    .AddSignInManager();

            services.AddAuthentication();

            services.AddAuthorization();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyBlazorApp", builder =>
                    builder.WithOrigins("https://localhost:7113")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials());
            });

            services.AddSingleton<IAdminConstants, AdminCreation>();

            services.AddScoped<IAccount, AccountRepository>();

            return services;
        }
    }


