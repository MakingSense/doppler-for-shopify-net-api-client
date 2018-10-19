using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MakingSense.DopplerForShopify.ApiClient
{
    public interface IDopplerForShopifyApiClient
    {
        Shop[] GetShops(string apiKey);
        IntegrationStatus GetIntegrationStatus(string shopDomain);
        AbandonedCarts[] GetAbandonedCarts(string shopDomain);
    }
}
