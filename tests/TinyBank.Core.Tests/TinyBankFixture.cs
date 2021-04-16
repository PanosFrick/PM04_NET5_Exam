using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

using TinyBank.Core.Implementation.Data;

namespace TinyBank.Core.Tests
{
    public class TinyBankFixture : IDisposable
    {
        public TinyBankDbContext DbContext { get; private set; }
        public IServiceScope Scope { get; private set; }

        public TinyBankFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            IServiceCollection services = new ServiceCollection();
            services.AddAppServices(config);

            Scope = services.BuildServiceProvider().CreateScope();
            DbContext = GetService<TinyBankDbContext>();
        }

        public void Dispose()
        {
            Scope.Dispose();
        }

        public T GetService<T>()
        {
            return Scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
