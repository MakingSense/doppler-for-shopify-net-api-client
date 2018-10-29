using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Discount")]
    public class Discount_Tests
    {
        private DiscountService _Service { get; set; }

        public Discount_Tests()
        {
            _Service =  new DiscountService(Utils.MyShopifyUrl, Utils.AccessToken);
        }

        [Fact(Skip = "Shopify Discount API requires a Shopify Plus account, which is difficult to use when testing a lib.")]
        public void Creates_An_Application_Credit()
        {
            var created = _Service.Create(new Discount()
            {
                DiscountType = "fixed_amount",
                Value = "10.00",
                DiscountCode = "AuntieDot",
                MinimumOrderAmount = "40.00",
            });

            Assert.NotNull(created);
        }
    }
}
