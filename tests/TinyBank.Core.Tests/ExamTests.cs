using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TinyBank.Core.Constants;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using Xunit;

namespace TinyBank.Core.Tests
{
    public class ExamTests : IClassFixture<TinyBankFixture>
    {
        private readonly TinyBankDbContext _dbContext;
        private readonly ICheckoutService _checkoutService;

        public ExamTests(TinyBankFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _checkoutService = fixture.GetService<ICheckoutService>();
        }


        [Fact]
        public void AmountChargeSuccess()
        {
            var options = new CheckoutOptions {
                CardNumber = "4444333322221111",
                Amount = 100,
                ExpirationMonth = 4,
                ExpirationYear = 2022
            };
            var beforeChargeCard = _dbContext.Set<Card>()
                .Where(x => x.CardNumber == options.CardNumber)
                .Include(x => x.Accounts)
                .FirstOrDefault();
            var beforBalance = beforeChargeCard.Accounts.First().Balance;

            _checkoutService.Checkout(options);
            var afterChargeCard = _dbContext.Set<Card>()
                .Where(x => x.CardNumber == options.CardNumber)
                .Include(x => x.Accounts)
                .FirstOrDefault();

            var afterBalance = afterChargeCard.Accounts.First().Balance;

            Assert.Equal(beforBalance, afterBalance + options.Amount);
        }
        [Fact]
        public void Card_Register_For_Exam()
        {
            var customer = new Customer {
                CustomerId = Guid.NewGuid(),
                Firstname = "Panos",
                Lastname = "Pasias",
                VatNumber = "987654321",
                Email = "panos@panos.com",
                IsActive = true,
                DateOfBirth = new DateTimeOffset(new DateTime(1983, 5, 31)),
                CountryCode = "GR",
                Type = CustomerType.PhysicalEntity
            };

            var account = new Account {
                AccountId = "GR9608100010000001234567890",
                Balance = 10000M,
                CurrencyCode = "EUR",
                Description = "Exam Account"
            };

            var card = new Card {
                CardNumber = "4444333322221111",
                Active = true,
                Expiration = new DateTimeOffset(new DateTime(2022, 04, 15)),
                CardType = CardType.Debit
            };

            customer.Accounts.Add(account);
            account.Cards.Add(card);
            _dbContext.Add(customer);
            _dbContext.SaveChanges();

            var getCustomerFromDb = _dbContext.Set<Customer>()
                .Where(x => x.VatNumber == "987654321")
                .Include(x => x.Accounts)
                .ThenInclude(y => y.Cards)
                .FirstOrDefault();

            var getCardFromCustomer = getCustomerFromDb.Accounts
                .SelectMany(a => a.Cards)
                .Where(c => c.CardNumber == "4444333322221111")
                .SingleOrDefault();

            Assert.NotNull(getCardFromCustomer);
            Assert.Equal(CardType.Debit, getCardFromCustomer.CardType);
            Assert.True(getCardFromCustomer.Active);
        }

    }
}
