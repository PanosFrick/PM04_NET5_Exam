using AutoMapper;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

using TinyBank.Core.Constants;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Implementation.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly TinyBankDbContext _dbContext;
        private readonly IMapper _mapper;

        public CustomerService(TinyBankDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = new MapperConfiguration(
                    cfg => cfg.CreateMap<RegisterCustomerOptions, Customer>())
                .CreateMapper();
        }

        public ApiResult<Customer> Register(RegisterCustomerOptions options)
        {
            if (options == null) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null {nameof(options)}"
                };
            }

            if (string.IsNullOrWhiteSpace(options.Firstname)) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.Firstname)}"
                };
            }

            if (string.IsNullOrWhiteSpace(options.Lastname)) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.Lastname)}"
                };
            }

            if (options.Type == Constants.CustomerType.Undefined) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Invalid customer type {nameof(options.Type)}"
                };
            }

            if (!IsValidVatNumber(options.CountryCode, options.VatNumber)) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Invalid Vat number {options.VatNumber}"
                };
            }

            var customerExists = Exists(options.VatNumber);

            if (customerExists) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.Conflict,
                    ErrorText = $"Customer with Vat number {options.VatNumber} already exists"
                };
            }

            var customer = _mapper.Map<Customer>(options);
            customer.IsActive = true;

            _dbContext.Add(customer);

            try {
                _dbContext.SaveChanges();
            } catch(Exception ex) {
                // log
                Console.WriteLine(ex);

                return new ApiResult<Customer>() {
                    Code = ApiResultCode.InternalServerError,
                    ErrorText = $"Customer could not be saved"
                };
            }

            return new ApiResult<Customer>() {
                Data = customer
            };
        }

        public ApiResult<Customer> Update(
            Guid customerId, UpdateCustomerOptions options)
        {
            if (options == null) {
                return new TinyBank.Core.ApiResult<Customer>() {
                    Code = ApiResultCode.NotFound,
                    ErrorText = $"Bad request"
                };
            }

            var result = GetById(customerId);

            if (!result.IsSuccessful()) {
                return result;
            }

            var customer = result.Data;

            if (customer != null) {
                customer.Firstname = options.Firstname;
                customer.Lastname = options.Lastname;
                customer.VatNumber = options.VatNumber;
                customer.Type = options.Type;
                customer.CountryCode = options.CountryCode;

                _dbContext.SaveChanges();
            } else {
                return new TinyBank.Core.ApiResult<Customer>() {
                    Code = ApiResultCode.NotFound,
                    ErrorText = $"Customer not found !"
                };
            }

            return new ApiResult<Customer>() {
                Data = customer
            };
        }

        public bool IsValidVatNumber(
            string countryCode, string vatNumber)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) {
                return false;
            }

            if (string.IsNullOrWhiteSpace(vatNumber)) {
                return false;
            }

            if (!Constants.Country.VatLength.TryGetValue(
              countryCode, out var vatLength)) {
                return false;
            }

            return vatNumber.Length == vatLength;
        }

        public ApiResult<Customer> GetById(Guid customerId)
        {
            var customer = Search(
                new SearchCustomerOptions() {
                    CustomerId = customerId
                })
                .Include(c => c.Accounts)
                .SingleOrDefault();

            if (customer == null) {
                return new ApiResult<Customer>() {
                    Code = Constants.ApiResultCode.NotFound,
                    ErrorText = $"Customer {customerId} was not found"
                };
            }

            return new ApiResult<Customer>() {
                Data = customer
            };
        }

        public IQueryable<Customer> Search(SearchCustomerOptions options)
        {
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            // SELECT FROM CUSTOMER
            var q = _dbContext.Set<Customer>()
                .AsQueryable();

            // SELECT FROM CUSTOMER WHERE CustomerId = options.CustomerId
            if (options.CustomerId != null) {
                q = q.Where(c => c.CustomerId == options.CustomerId);
            }

            // SELECT FROM CUSTOMER WHERE CustomerId = options.CustomerId
            // AND VatNumber = options.VatNumber
            if (!string.IsNullOrWhiteSpace(options.VatNumber)) {
                q = q.Where(c => c.VatNumber == options.VatNumber);
            }

            if (!string.IsNullOrWhiteSpace(options.Firstname)) {
                q = q.Where(c => c.Firstname == options.Firstname);
            }

            if (!string.IsNullOrWhiteSpace(options.Lastname)) {
                q = q.Where(c => c.Lastname == options.Lastname);
            }

            if (options.CountryCodes.Any()) {
                q = q.Where(c => options.CountryCodes.Contains(
                    c.CountryCode));
            }

            if (options.TrackResults != null && 
              !options.TrackResults.Value) {
                q = q.AsNoTracking();
            }

            if (options.Skip != null) {
                q = q.Skip(options.Skip.Value);
            }
            
            q = q.Take(options.MaxResults ?? 500);

            return q;
        }

        private bool Exists(string vatNumber)
        {
            if (string.IsNullOrWhiteSpace(vatNumber)) {
                throw new ArgumentNullException(vatNumber);
            }

            return Search(
                new SearchCustomerOptions() {
                    VatNumber = vatNumber
                }).Any();
        }
    }
}
