using Doppler.Shopify.ApiClient;
using Doppler.Shopify.ApiClient.Filters;
using System;

namespace Doppler.Shopify.ConsoleApiClient.Actions
{
    class CheckoutsAction : ShopifyApiAction
    {
        private readonly DateTimeOffset? _since;
        private readonly DateTimeOffset? _until;

        public CheckoutsAction(string shop, string accessToken, DateTimeOffset? since, DateTimeOffset? until) : base (shop, accessToken)
        {
            _since = since;
            _until = until;
        }

        public override object Execute()
        {
            var checkoutsService = new CheckoutService(_shop, _accessToken);

            var filter = new CheckoutFilter
            {
                CreatedAtMin = _since,
                CreatedAtMax = _until
            };

            return checkoutsService.List(filter);
        }
    }
}
