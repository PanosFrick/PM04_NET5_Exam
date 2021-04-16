using System;
using System.Linq;

using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class CustomerServiceTests : IClassFixture<TinyBankFixture>
    {
        private readonly ICustomerService _customers;

        public CustomerServiceTests(TinyBankFixture fixture)
        {
            _customers = fixture.GetService<ICustomerService>();
        }

        [Theory]
        [InlineData(Constants.Country.GreekCountryCode)]
        [InlineData(Constants.Country.ItalyCountryCode)]
        [InlineData(Constants.Country.CyprusCountryCode)]
        public Customer RegisterCustomer_Success(string countryCode)
        {
            var options = new RegisterCustomerOptions() {
                Firstname = "Georgia",
                Lastname = "Papadopoulou",
                Type = Constants.CustomerType.PhysicalEntity,
                CountryCode = countryCode,
                VatNumber = GenerateVat(countryCode)
            };

            var result = _customers.Register(options);

            Assert.True(result.IsSuccessful());
            Assert.NotNull(result.Data);

            var customer = result.Data;
            Assert.Equal(options.Firstname, customer.Firstname);
            Assert.Equal(options.Lastname, customer.Lastname);
            Assert.Equal(options.Type, customer.Type);
            Assert.Equal(options.VatNumber, customer.VatNumber);
            Assert.True(customer.IsActive);

            return customer;
        }

        [Fact]
        public void RegisterCustomer_Fail_Customer_Exists()
        {
            var customer = RegisterCustomer_Success(Constants.Country.GreekCountryCode);
            Assert.NotNull(customer);

            var options = new RegisterCustomerOptions() {
                CountryCode = customer.CountryCode,
                VatNumber = customer.VatNumber,
                Firstname = "Name",
                Lastname = "Lastname",
                Type = Constants.CustomerType.PhysicalEntity
            };

            var result = _customers.Register(options);
            Assert.False(result.IsSuccessful());
            Assert.Equal(Constants.ApiResultCode.Conflict, result.Code);
        }

        [Fact]
        public void RegisterCustomer_Fail_InvalidOptions()
        {
            var options = new RegisterCustomerOptions() {
                Lastname = "Pnevmatikos",
                Type = Constants.CustomerType.PhysicalEntity,
                CountryCode = Constants.Country.GreekCountryCode,
                VatNumber = "1111111"
            };

            // Firstname
            var result = _customers.Register(options);
            Assert.False(result.IsSuccessful());
            Assert.Equal(Constants.ApiResultCode.BadRequest, result.Code);

            // lastname
            options.Firstname = "Dimitris";
            options.Lastname = null;

            result = _customers.Register(options);
            Assert.False(result.IsSuccessful());
            Assert.Equal(Constants.ApiResultCode.BadRequest, result.Code);
        }

        [Fact]
        public void ValidateVatNumber()
        {
            // success - happy path
            var result = _customers.IsValidVatNumber(
            Constants.Country.GreekCountryCode, "123456789");
            Assert.True(result);

            result = _customers.IsValidVatNumber(
           Constants.Country.ItalyCountryCode, "1234567891");
            Assert.True(result);

            result = _customers.IsValidVatNumber(
           Constants.Country.CyprusCountryCode, "12345678911");
            Assert.True(result);

            // fail
            result = _customers.IsValidVatNumber(
            "GB", "123456789");
            Assert.False(result);

            result = _customers.IsValidVatNumber(
           Constants.Country.GreekCountryCode, " ");
            Assert.False(result);

            result = _customers.IsValidVatNumber(
           "gR", "123456789");
            Assert.True(result);
        }

        [Fact]
        public void SearchVat_Success()
        {
            var customer = RegisterCustomer_Success(Constants.Country.GreekCountryCode);
            Assert.NotNull(customer);

            var options = new SearchCustomerOptions() {
                VatNumber = customer.VatNumber
            };

            var result = _customers.Search(options).SingleOrDefault();

            Assert.NotNull(result);
        }

        [Fact]
        public void SearchCountry_Success()
        {
            var customer1 = RegisterCustomer_Success(Constants.Country.GreekCountryCode);
            Assert.NotNull(customer1);

            var customer2 = RegisterCustomer_Success(Constants.Country.CyprusCountryCode);
            Assert.NotNull(customer2);

            var options = new SearchCustomerOptions() {
                CountryCodes = { Constants.Country.GreekCountryCode, Constants.Country.CyprusCountryCode }
            };

            var result = _customers.Search(options).ToList();
            Assert.NotNull(result);

            var res1 = result.Where(r => r.CustomerId == customer1.CustomerId)
           .SingleOrDefault();
            var res2 = result.Where(r => r.CustomerId == customer2.CustomerId)
            .SingleOrDefault();

            Assert.NotNull(res1);
            Assert.NotNull(res2);
        }

        [Fact]
        public void Test_GenerateVat()
        {
            var grVat = GenerateVat(Constants.Country.GreekCountryCode);
            var valid = _customers.IsValidVatNumber(
                Constants.Country.GreekCountryCode, grVat);
            Assert.True(valid);

            var itVat = GenerateVat(Constants.Country.ItalyCountryCode);
            valid = _customers.IsValidVatNumber(
                Constants.Country.ItalyCountryCode, itVat);
            Assert.True(valid);

            var cyVat = GenerateVat(Constants.Country.CyprusCountryCode);
            valid = _customers.IsValidVatNumber(
                Constants.Country.CyprusCountryCode, cyVat);
            Assert.True(valid);
        }

        private string GenerateVat(string countryCode)
        {
            switch (countryCode) {
                case Constants.Country.GreekCountryCode:
                    return $"{DateTimeOffset.Now:ssfffffff}";

                case Constants.Country.ItalyCountryCode:
                    return $"{DateTimeOffset.Now:mmssffffff}";

                case Constants.Country.CyprusCountryCode:
                    return $"{DateTimeOffset.Now:mmssfffffff}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}