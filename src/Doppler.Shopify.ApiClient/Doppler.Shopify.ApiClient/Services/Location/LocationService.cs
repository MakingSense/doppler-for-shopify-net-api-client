﻿using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating Shopify's Location API.
    /// </summary>
    public class LocationService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="LocationService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public LocationService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }
        
        /// <summary>
        /// Retrieves the <see cref="Location"/> with the given id.
        /// </summary>
        /// <param name="id">The id of the charge to retrieve.</param>
        /// <returns>The <see cref="Location"/>.</returns>
        public virtual Location Get(long id)
        {
            var req = PrepareRequest(string.Format("locations/{0}.json", id));           

            return ExecuteRequest<Location>(req, HttpMethod.Get, rootElement: "location");
        }

        /// <summary>
        /// Retrieves a list of all <see cref="Location"/> objects.
        /// </summary>
        /// <returns>The list of <see cref="Location"/> objects.</returns>
        public virtual List<Location> List()
        {
            var req = PrepareRequest("locations.json");

            return ExecuteRequest<List<Location>>(req, HttpMethod.Get, rootElement: "locations");
        }
    }
}
