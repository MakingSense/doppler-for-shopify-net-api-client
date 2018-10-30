using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "RecurringCharge")]
    public class RecurringCharge_Tests
    {
        private RecurringChargeService _service = new RecurringChargeService(Utils.MyShopifyUrl, Utils.AccessToken);

        private RecurringChargeService _Service { get { return _service; } set { _service = value; } }
        [Fact(Skip = "Recurring charges cannot be tested with a private application.")]
        public void Creates_Charges()
        {
            // Can't be tested.
        }

        [Fact(Skip = "Recurring charges cannot be tested with a private application.")]
        public void Activates_Charges()
        {
            // Can't be tested.
        }

        [Fact(Skip = "Recurring charges cannot be tested with a private application.")]
        public void Lists_Charges()
        {
            // Can't be tested.
        }

        [Fact(Skip = "Recurring charges cannot be tested with a private application.")]
        public void Gets_Charges()
        {
            // Can't be tested.
        }

        [Fact(Skip = "Recurring charges cannot be tested with a private application.")]
        public void Deletes_Charges()
        {
            // Can't be tested.    
        }
    }
}