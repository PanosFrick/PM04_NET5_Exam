using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;

namespace TinyBank.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            var connectionString = config.GetConnectionString("tinyBank");

            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(connectionString);

            using var context = new TinyBankDbContext(builder.Options);

            var customer = new Customer() {
                
                Firstname = "Spyro",
                Lastname = "Spyrou",
                VatNumber = "117003930",
                IsActive = true
            };

            customer.Accounts.Add(
                new Account() {
                    AccountId = "GR123456",
                    Balance = 1000,
                    CurrencyCode = "EUR",
                    Description = "A test account",
                    State = Core.Constants.AccountState.Active
                });

            var account = new Account() {
                AccountId = "GR123456789",
                Balance = 1000,
                CurrencyCode = "EUR",
                Description = "A secondary test account",
                State = Core.Constants.AccountState.Active,
                CustomerId = Guid.Parse("A45CC9FF-A3AE-4AA5-8CFB-5685D950CA2B")
            };

            context.Add(account);
            context.SaveChanges();
        }
    }
}
