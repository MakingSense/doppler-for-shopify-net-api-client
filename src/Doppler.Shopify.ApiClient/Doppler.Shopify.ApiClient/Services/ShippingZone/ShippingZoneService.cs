﻿using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for retrieve Shopify shipping zones.
    /// </summary>
    public class ShippingZoneService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="ShippingZoneService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public ShippingZoneService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Retrieves a list of all <see cref="ShippingZone"/> objects.
        /// </summary>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The list of <see cref="ShippingZone"/> objects.</returns>
        public virtual List<ShippingZone> List(string fields = null)
        {
            var req = PrepareRequest("shipping_zones.json");

            if (string.IsNullOrEmpty(fields) == false)
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<List<ShippingZone>>(req, HttpMethod.Get, rootElement: "shipping_zones");
        }
    }
}
