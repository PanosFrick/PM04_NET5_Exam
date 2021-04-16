using TinyBank.Core.Model;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Services
{
    public interface ICheckoutService
    {
        ApiResult<Card> Checkout(CheckoutOptions options);
    }
}
