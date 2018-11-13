using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "DopplerIntegration")]
    public class DopplerIntegration_Tests
    {
        private readonly DopplerIntegrationService _service;

        public DopplerIntegration_Tests()
        {
            _service = new DopplerIntegrationService();
        }

        [Fact]
        public void Get_Shops()
        {
            var shops = _service.GetShops("F53A92B7C4B23C874EEE6A4CB44E5150");
            Assert.True(shops.Count > 0);
        }
    }
}
