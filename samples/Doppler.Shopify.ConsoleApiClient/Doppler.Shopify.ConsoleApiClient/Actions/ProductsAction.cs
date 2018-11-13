using Doppler.Shopify.ApiClient;
using Doppler.Shopify.ApiClient.Filters;
using System;

namespace Doppler.Shopify.ConsoleApiClient.Actions
{
    class ProductsAction : ShopifyApiAction
    {
        private readonly DateTimeOffset? _since;
        private readonly DateTimeOffset? _until;
        private readonly bool _publishedOnly;

        public ProductsAction(string shop, string accessToken, DateTimeOffset? since, DateTimeOffset? until, bool publishedOnly) : base(shop, accessToken)
        {
            _since = since;
            _until = until;
            _publishedOnly = publishedOnly;
        }

        public override object Execute()
        {
            var prodcutService = new ProductService(_shop, _accessToken);

            var filter = new ProductFilter
            {
                PublishedStatus= _publishedOnly ? "published" : "any" // TODO: move to a constant
            };

            return prodcutService.List(filter);
        }
    }
}
