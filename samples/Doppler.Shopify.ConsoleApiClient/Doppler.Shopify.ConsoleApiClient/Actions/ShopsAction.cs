using Doppler.Shopify.ApiClient;

namespace Doppler.Shopify.ConsoleApiClient.Actions
{
    class ShopsAction : IAction
    {
        private readonly string _dopplerApiKey;
        private readonly string _dopplerForShopifyUrl;

        public ShopsAction(string dopplerApiKey, string dopplerForShopifyUrl)
        {
            _dopplerApiKey = dopplerApiKey;
            _dopplerForShopifyUrl = dopplerForShopifyUrl;
        }

        public object Execute()
        {
            var dopplerIntegrationService = new DopplerIntegrationService(_dopplerForShopifyUrl);
            return dopplerIntegrationService.GetShops(_dopplerApiKey);
        }
    }
}
