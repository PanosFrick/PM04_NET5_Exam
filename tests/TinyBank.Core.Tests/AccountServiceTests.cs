using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class AccountServiceTests : IClassFixture<TinyBankFixture>
    {
        private readonly IAccountService _accounts;
        private readonly CustomerServiceTests _customerTests;

        public AccountServiceTests(TinyBankFixture fixture)
        {
            _accounts = fixture.GetService<IAccountService>();
            _customerTests = new CustomerServiceTests(fixture);
        }

        [Fact]
        public void CreateAccount_Success()
        {
            var customer = _customerTests.RegisterCustomer_Success(
                Constants.Country.GreekCountryCode);

            Assert.NotNull(customer);

            var accountOptions = new CreateAccountOptions() {
                CurrencyCode = Constants.CurrencyCode.Euro,
                Description = "My test account"
            };

            var accountResult = _accounts.Create(
                customer.CustomerId, accountOptions);
            Assert.True(accountResult.IsSuccessful());

            var account = accountResult.Data;
            Assert.StartsWith(customer.CountryCode, account.AccountId);
            Assert.Equal(customer.CustomerId, account.CustomerId);
            Assert.Equal(0M, account.Balance);
            Assert.Equal(Constants.AccountState.Active, account.State);
        }
    }
}
