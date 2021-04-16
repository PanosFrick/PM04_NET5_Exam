using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using TinyBank.Core.Config;
using TinyBank.Core.Implementation.Config;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Implementation.Services;
using TinyBank.Core.Services;

public static class ServiceCollectionExtentions
{
    public static void AddAppServices(
        this IServiceCollection @this,
        IConfiguration config)
    {
        @this.AddSingleton(config.ReadAppConfiguration());

        @this.AddDbContext<TinyBankDbContext>(
        (serviceProvider, optionsBuilder) => {
            var appConfig = serviceProvider.GetRequiredService<AppConfig>();

            optionsBuilder.UseSqlServer(appConfig.DatabaseConnectionString);
        });

        @this.AddScoped<ICustomerService, CustomerService>();
        @this.AddScoped<IAccountService, AccountService>();
    }
}