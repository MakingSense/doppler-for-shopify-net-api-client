﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Doppler.Shopify.ApiClient
{
    public class PolicyService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="PolicyService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public PolicyService(string myShopifyUrl, string shopAccessToken): base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Get the policies and their contents for a shop
        /// </summary>
        public virtual List<Policy> List()
        {
            var request = PrepareRequest("policies.json");

            return ExecuteRequest<List<Policy>>(request, HttpMethod.Get, rootElement: "policies");
        }
    }
}
