using Microsoft.AspNetCore.Mvc;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

namespace TinyBank.Web.Controllers
{
    [Route("card")]
    public class CardController : Controller
    {
        private readonly ICheckoutService _checkoutService;

        // Path : '/card'
        public CardController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        // Path : '/card'
        public IActionResult Index()
        {

            return View(new CheckoutOptions());
        }

        // Path : '/card/checkout'
        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] CheckoutOptions options)
        {
            return Ok(_checkoutService.Checkout(options));
        }
    }
}
