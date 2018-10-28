using Newtonsoft.Json.Linq;
using System.Net.Http;
using Doppler.Shopify.ApiClient.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating Shopify fulfillment events.
    /// </summary>
    public class FulfillmentEventService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="FulfillmentEventService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public FulfillmentEventService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Get a list of all fulfillment events for a fulfillment
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillment belongs to.</param>
        /// <param name="fulfillmentId">The fulfillment id to which the fulfillment events belong to.</param>
        /// <returns>The list of fulfillment events for the given fulfillment.</returns>
        public virtual List<FulfillmentEvent> List(long orderId, long fulfillmentId)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments/{1}/events.json", orderId, fulfillmentId));

            return ExecuteRequest<List<FulfillmentEvent>>(req, HttpMethod.Get, rootElement: "fulfillment_events");
        }

        /// <summary>
        /// Retrieves the <see cref="FulfillmentEvent"/> with the given id.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillment belongs to.</param>
        /// <param name="fulfillmentId">The id of the fulfillment to which the event belongs to.</param>
        /// <param name="fulfillmentEventId">The id of the fulfillment event to retrieve.</param>
        /// <returns>The <see cref="FulfillmentEvent"/>.</returns>
        public virtual FulfillmentEvent Get(long orderId, long fulfillmentId, long fulfillmentEventId)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments/{1}/events/{2}.json", orderId, fulfillmentId, fulfillmentEventId));

            return ExecuteRequest<FulfillmentEvent>(req, HttpMethod.Get, rootElement: "fulfillment_event");
        }

        /// <summary>
        /// Creates a new <see cref="FulfillmentEvent"/> on the fulfillment.
        /// </summary>
        /// <param name="event">A new <see cref="Fulfillment"/>. Id should be set to null.</param>
        /// <returns>The new <see cref="FulfillmentEvent"/>.</returns>
        public virtual FulfillmentEvent Create(long orderId, long fulfillmentId, FulfillmentEvent @event)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments/{1}/events.json", orderId, fulfillmentId));

            var content = new JsonContent(new
            {
                @event
            });

            return ExecuteRequest<FulfillmentEvent>(req, HttpMethod.Post, content, "fulfillment_event");
        }

        /// <summary>
        /// Deletes the <see cref="FulfillmentEvent"/> with the given Id.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillment belongs to.</param>
        /// <param name="fulfillmentId">The id of the fulfillment to which the event belongs to.</param>
        /// <param name="fulfillmentEventId">The id of the fulfillment event to retrieve.</param>
        public virtual void Delete(long orderId, long fulfillmentId, long fulfillmentEventId)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments/{1}/events/{2}.json", orderId, fulfillmentId, fulfillmentEventId));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}
