using Doppler.Shopify.ApiClient;

namespace Doppler.Shopify.ConsoleApiClient.Actions
{
    class SynchronizeCustomersAction : IAction
    {
        private readonly string _dopplerApiKey;
        private readonly string _dopplerForShopifyUrl;
        private readonly string _shop;

        public SynchronizeCustomersAction(string dopplerApiKey, string dopplerForShopifyUrl, string shop)
        {
            _dopplerApiKey = dopplerApiKey;
            _dopplerForShopifyUrl = dopplerForShopifyUrl;
            _shop = shop;
        }

        public object Execute()
        {
            var dopplerIntegrationService = new DopplerIntegrationService(_dopplerForShopifyUrl);
            dopplerIntegrationService.SynchronizeCustomers(_dopplerApiKey, _shop);
            return true;
        }
    }
}
