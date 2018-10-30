using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "UsageCharge")]
    public class UsageCharge_Tests
    {
        private UsageChargeService _service = new UsageChargeService(Utils.MyShopifyUrl, Utils.AccessToken);

        private UsageChargeService _Service { get { return _service; } set { _service = value; } }

        [Fact(Skip = "Usage charges cannot be tested with a private application.")]
        public void Creates_Charges()
        {
            // Can't be tested.
        }

        [Fact(Skip = "Usage charges cannot be tested with a private application.")]
        public void Activates_Charges()
        {
            // Can't be tested.
        }

        [Fact(Skip = "Usage charges cannot be tested with a private application.")]
        public void Lists_Charges()
        {
            // Can't be tested.
        }

        [Fact(Skip = "Usage charges cannot be tested with a private application.")]
        public void Gets_Charges()
        {
            // Can't be tested.
        }

        [Fact(Skip = "Usage charges cannot be tested with a private application.")]
        public void Deletes_Charges()
        {
            // Can't be tested.    
        }
    }
}