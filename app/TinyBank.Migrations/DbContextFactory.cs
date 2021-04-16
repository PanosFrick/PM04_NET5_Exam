using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using TinyBank.Core.Implementation.Data;

namespace TinyBank.Migrations
{
    public class DbContextFactory : IDesignTimeDbContextFactory<TinyBankDbContext>
    {
        public TinyBankDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(
                config.GetConnectionString("tinyBank"),
                options => {
                    options.MigrationsAssembly("TinyBank.Migrations");
                });

            return new TinyBankDbContext(builder.Options);
        }
    }
}
