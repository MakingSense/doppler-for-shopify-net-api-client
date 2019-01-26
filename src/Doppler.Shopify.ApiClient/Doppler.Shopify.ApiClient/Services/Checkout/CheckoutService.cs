﻿using System.Net.Http;
using Doppler.Shopify.ApiClient.Filters;
using System.Collections.Generic;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating Shopify orders.
    /// </summary>
    public class CheckoutService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="OrderService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public CheckoutService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all of the shop's abandoned checkouts.
        /// </summary>
        /// <param name="filter">Options for filtering the count.</param>
        /// <returns>The count of all orders for the shop.</returns>
        public virtual int Count(CheckoutFilter filter = null)
        {
            var req = PrepareRequest("checkouts/count.json");

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets a list of up to 250 of the shop's abandoned carts.
        /// </summary>
        /// <returns></returns>
        public virtual List<Checkout> List(CheckoutFilter options = null)
        {
            var req = PrepareRequest("checkouts.json");

            if (options != null)
            {
                req.QueryParams.AddRange(options.ToParameters());
            }

            return ExecuteRequest<List<Checkout>>(req, HttpMethod.Get, rootElement: "checkouts");
        }

    }
}
