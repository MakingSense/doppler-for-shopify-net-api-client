﻿using Newtonsoft.Json.Linq;
using System.Net.Http;
using Doppler.Shopify.ApiClient.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating Shopify fulfillments.
    /// </summary>
    public class FulfillmentServiceService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="FulfillmentServiceService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public FulfillmentServiceService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a list of your app's FulfillmentServices.
        /// </summary>
        /// <param name="scope">Set scope to all to retrieve all of the store's fulfillment services</param>
        /// <returns>The list of fulfillment services matching the filter.</returns>
        public virtual List<FulfillmentServiceEntity> List(string scope = null)
        {
            var req = PrepareRequest("fulfillment_services.json");

            if (!string.IsNullOrEmpty(scope))
            {
                req.QueryParams.Add("scope", scope);
            }

            return ExecuteRequest<List<FulfillmentServiceEntity>>(req, HttpMethod.Get, rootElement: "fulfillment_services");
        }

        /// <summary>
        /// Retrieves the <see cref="FulfillmentServiceEntity"/> with the given id.
        /// </summary>
        /// <param name="fulfillmentServiceId">The if of the fulfillment service.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="Fulfillment"/>.</returns>
        public virtual FulfillmentServiceEntity Get(long fulfillmentServiceId, string fields = null)
        {
            var req = PrepareRequest(string.Format("fulfillment_services/{0}.json", fulfillmentServiceId));

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<FulfillmentServiceEntity>(req, HttpMethod.Get, rootElement: "fulfillment_service");
        }

        /// <summary>
        /// Creates a new <see cref="FulfillmentServiceEntity"/>.
        /// </summary>
        /// <param name="fulfillmentServiceEntity">A new <see cref="FulfillmentServiceEntity"/>. Id should be set to null.</param>
        /// has been created.</param>
        /// <returns>The new <see cref="FulfillmentServiceEntity"/>.</returns>
        public virtual FulfillmentServiceEntity Create(FulfillmentServiceEntity fulfillmentServiceEntity)
        {
            var req = PrepareRequest("fulfillment_services.json");
            var body = fulfillmentServiceEntity.ToDictionary();

            var content = new JsonContent(new
            {
                fulfillment_service = body
            });

            return ExecuteRequest<FulfillmentServiceEntity>(req, HttpMethod.Post, content, "fulfillment_service");
        }

        /// <summary>
        /// Updates the given <see cref="FulfillmentServiceEntity"/>.
        /// </summary>
        /// <param name="fulfillmentServiceId">Id of the fulfillment service being updated.</param>
        /// <param name="fulfillmentServiceEntity">The <see cref="FulfillmentServiceEntity"/> to update.</param>
        /// <returns>The updated <see cref="FulfillmentServiceEntity"/>.</returns>
        public virtual FulfillmentServiceEntity Update(long fulfillmentServiceId, FulfillmentServiceEntity fulfillmentServiceEntity)
        {
            var req = PrepareRequest(string.Format("fulfillment_services/{0}.json", fulfillmentServiceId));
            var body = fulfillmentServiceEntity.ToDictionary();

            var content = new JsonContent(new
            {
                fulfillment_service = body
            });

            return ExecuteRequest<FulfillmentServiceEntity>(req, HttpMethod.Put, content, "fulfillment_service");
        }

        /// <summary>
        /// Deletes a fulfillment service with the given Id.
        /// </summary>
        /// <param name="fulfillmentServiceId">The fulfillment service object's Id.</param>
        public virtual void Delete(long fulfillmentServiceId)
        {
            var req = PrepareRequest(string.Format("fulfillment_services/{0}.json", fulfillmentServiceId));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}
