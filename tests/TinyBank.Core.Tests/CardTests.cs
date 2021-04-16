using Microsoft.EntityFrameworkCore;

using System.Linq;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class CardTests : IClassFixture<TinyBankFixture>
    {
        private readonly TinyBankDbContext _dbContext;

        public CardTests(TinyBankFixture fixture)
        {
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public void Card_Register_Success()
        {
            var customer = new Customer() {
                Firstname = "Dimitris",
                Lastname = "Pnevmatikos",
                VatNumber = "117008855",
                Email = "dpnevmatikos@codehub.gr",
                IsActive = true
            };

            var account = new Account() {
                Balance = 1000M,
                CurrencyCode = "EUR",
                State = Constants.AccountState.Active,
                AccountId = "GR123456789121"
            };

            customer.Accounts.Add(account);

            var card = new Card() {
                Active = true,
                CardNumber = "4111111111111111",
                CardType = Constants.CardType.Debit
            };

            account.Cards.Add(card);

            _dbContext.Add(customer);
            _dbContext.SaveChanges();

            var customerFromDb = _dbContext.Set<Customer>()
                .Where(c => c.VatNumber == "117008855")
                .Include(c => c.Accounts)
                .ThenInclude(a => a.Cards)
                .SingleOrDefault();

            var customerCard = customerFromDb.Accounts
                .SelectMany(a => a.Cards)
                .Where(c => c.CardNumber == "4111111111111111")
                .SingleOrDefault();

            Assert.NotNull(customerCard);
            Assert.Equal(Constants.CardType.Debit, customerCard.CardType);
            Assert.True(customerCard.Active);
        }
    }
}
