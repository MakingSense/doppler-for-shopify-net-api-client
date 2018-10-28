﻿using Newtonsoft.Json.Linq;
using System.Net.Http;
using Doppler.Shopify.ApiClient.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating mapping between custom collections and collections
    /// </summary>
    public class CustomCollectionService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="CustomCollectionService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public CustomCollectionService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// default: 50
        /// Gets a list of up to 250 custom collections for the corresponding productId
        /// </summary>
        /// <param name="filter">The <see cref="CustomCollection"/>. used to filter results</param>
        /// <returns></returns>
        public virtual List<CustomCollection> List(CustomCollectionFilter filter = null)
        {
            var req = PrepareRequest("custom_collections.json");

            //Add optional parameters to request
            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<List<CustomCollection>>(req, HttpMethod.Get, rootElement: "custom_collections");
        }

        /// <summary>
        /// Creates a new <see cref="CustomCollection"/> Custom Collection
        /// </summary>
        /// <param name="customCollection">A new <see cref="CustomCollection"/>. Id should be set to null.</param>
        /// <returns>The new <see cref="CustomCollection"/>.</returns>
        public virtual CustomCollection Create(CustomCollection customCollection)
        {
            var req = PrepareRequest("custom_collections.json");
            var content = new JsonContent(new
            {
                custom_collection = customCollection
            });

            return ExecuteRequest<CustomCollection>(req, HttpMethod.Post, content, "custom_collection");
        }

        /// <summary>
        /// Gets a count of all of the custom collections
        /// </summary>
        /// <returns>The count of all collects for the shop.</returns>
        public virtual int Count(CustomCollectionFilter options = null)
        {
            var req = PrepareRequest("custom_collections/count.json");

            if (options != null)
            {
                req.QueryParams.AddRange(options.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Retrieves the <see cref="CustomCollection"/> with the given id.
        /// </summary>
        /// <param name="customCollectionId">The id of the custom collection to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="CustomCollection"/>.</returns>
        public virtual CustomCollection Get(long customCollectionId, string fields = null)
        {
            var req = PrepareRequest(string.Format("custom_collections/{0}.json", customCollectionId));

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<CustomCollection>(req, HttpMethod.Get, rootElement: "custom_collection");
        }

        /// <summary>
        /// Deletes a custom collection with the given Id.
        /// </summary>
        /// <param name="customCollectionId">The custom collection's Id.</param>
        public virtual void Delete(long customCollectionId)
        {
            var req = PrepareRequest(string.Format("custom_collections/{0}.json", customCollectionId));

            ExecuteRequest(req, HttpMethod.Delete);
        }

        /// <summary>
        /// Updates the given <see cref="CustomCollection"/>.
        /// </summary>
        /// <param name="customCollectionId">Id of the object being updated.</param>
        /// <param name="customCollection">The <see cref="CustomCollection"/> to update.</param>
        /// <returns>The updated <see cref="CustomCollection"/>.</returns>
        public virtual CustomCollection Update(long customCollectionId, CustomCollection customCollection)
        {
            var req = PrepareRequest(string.Format("custom_collections/{0}.json", customCollectionId));
            var content = new JsonContent(new
            {
                custom_collection = customCollection
            });

            return ExecuteRequest<CustomCollection>(req, HttpMethod.Put, content, "custom_collection");
        }
    }
}
