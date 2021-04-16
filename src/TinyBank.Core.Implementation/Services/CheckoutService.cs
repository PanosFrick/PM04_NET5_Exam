using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TinyBank.Core.Constants;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Implementation.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly TinyBankDbContext _dbContext;

        public CheckoutService(TinyBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private bool ValidateCheckoutInput(CheckoutOptions options, out string errorMessage)
        {
            if (options == null) {
                errorMessage = "no input data detected";
                return false;
            }

            if (!ValidateCardNumber(options.CardNumber, out errorMessage))
                return false;
            if (!ValidateExpirationYear(options.ExpirationYear, out errorMessage))
                return false;
            if (!ValidateExpirationMonth(options.ExpirationMonth, out errorMessage))
                return false;
            if (!ValidateAmount(options.Amount, out errorMessage))
                return false;
            return true;
        }

        private bool ValidateExpirationYear(int year, out string errorMessage)
        {
            errorMessage = null;
            if (year < DateTime.Today.Year) {
                errorMessage = "Expiration year should be at least the current";
                return false;
            }

            if (year > DateTime.Today.AddYears(5).Year) {
                errorMessage = "Expiration year should within a 5 year period from today";
                return false;
            }
            return true;
        }

        private bool ValidateExpirationMonth(int month, out string errorMessage)
        {
            errorMessage = null;
            if (month < 1 || month > 12) {
                errorMessage = "Expiration month should be between 1 - 12";
                return false;
            }
            return true;
        }

        private bool ValidateAmount(decimal amount, out string errorMessage)
        {
            errorMessage = null;
            if (!(amount >= 0)) {
                errorMessage = "Amount should be a positive number";
                return false;
            }

            return true;
        }


        private bool ValidateCardNumber(string cardNumber, out string errorMessage)
        {
            errorMessage = null;
            if (string.IsNullOrEmpty(cardNumber)) {
                errorMessage = "Please fill in your card number";
                return false;
            }

            if (cardNumber.Trim().Length != 16) {
                errorMessage = "Card number should be a 16-digit number";
                return false;
            }

            // todo:  check if it contains non letters
            return true;
        }

        private Card GetCardFromDb(string cardNumber)
        {
            var card = _dbContext.Set<Card>()
                .Where(c => c.CardNumber.Trim() == cardNumber.Trim())
                .Include(c => c.Accounts).FirstOrDefault();
            return card;
        }

        private bool ValidateCardAndAmount(Card card, CheckoutOptions options, out Account accountWithSufficientBalance,
            out string errorMessage)
        {
            errorMessage = null;
            accountWithSufficientBalance = null;
            if (card == null) {
                errorMessage = "Card not found";
                return false;
            }

            if (!card.Active) {
                errorMessage = "Card is not active";
                return false;
            }

            if (card.Expiration.Year != options.ExpirationYear || card.Expiration.Month != options.ExpirationMonth) {
                errorMessage = "The expiration date MM/YYY does not match";
                return false;
            }

            var accounts = card.Accounts;
            if (accounts == null || !accounts.Any()) {
                errorMessage = "Card is not connected to any accounts";
                return false;
            }

            accountWithSufficientBalance = accounts.FirstOrDefault(x => x.Balance >= options.Amount);
            if (accountWithSufficientBalance == null) {
                errorMessage = "No connected account with sufficient balance found";
                return false;
            }

            return true;
        }

        private bool ChargeAccount(Account accountToCharge, decimal amount)
        {
            accountToCharge.Balance -= amount;
            var result = _dbContext.SaveChanges();
            return result == 1;
        }


        public ApiResult<Card> Checkout(CheckoutOptions options)
        {
            int returnCode = ApiResultCode.Success;
            string validationErrorMessage = null;

            if (!ValidateCheckoutInput(options, out validationErrorMessage))
                return ApiResult<Card>.CreateFailed(ApiResultCode.BadRequest, validationErrorMessage);


            var card = GetCardFromDb(options.CardNumber);
            Account accountWithSufficientBalance = null;
            if (!ValidateCardAndAmount(card, options, out accountWithSufficientBalance,
                out validationErrorMessage))
                return ApiResult<Card>.CreateFailed(ApiResultCode.BadRequest, validationErrorMessage);

            if (ChargeAccount(accountWithSufficientBalance, options.Amount))
                return ApiResult<Card>.CreateSuccessful(null);
            else {
                return ApiResult<Card>.CreateFailed(ApiResultCode.InternalServerError,
                    "An error occured during card charging. Please try again");
            }
        }
    }
}

