namespace TinyBank.Core.Services.Options
{
    public class CheckoutOptions
    {
        public string CardNumber { get; set; }
        public int ExpirationYear { get; set; }
        public int ExpirationMonth { get; set; }
        public decimal Amount { get; set; }
    }
}
