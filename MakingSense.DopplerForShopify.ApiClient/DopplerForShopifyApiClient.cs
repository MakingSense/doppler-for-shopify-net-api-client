using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MakingSense.DopplerForShopify.ApiClient
{
    public class DopplerForShopifyApiClient : IDopplerForShopifyApiClient
    {
        private readonly DopplerForShopifyApiClientConfiguration _configuration;

        public DopplerForShopifyApiClient(DopplerForShopifyApiClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AbandonedCarts[] GetAbandonedCarts(int userId)
        {
            // crear un request

            new GetRequest<AbandonedCarts[]>(_configuration.BaseUrl + "/abandoned-carts")
                .GetResponseAsJson()


            throw new NotImplementedException();
        }

        public IntegrationStatus GetIntegrationStatus(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
