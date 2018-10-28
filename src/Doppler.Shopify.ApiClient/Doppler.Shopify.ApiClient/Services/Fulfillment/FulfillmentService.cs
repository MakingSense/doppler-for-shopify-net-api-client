using Newtonsoft.Json.Linq;
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
    public class FulfillmentService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="FulfillmentService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public FulfillmentService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all of the order's fulfillments.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="filter">Options for filtering the count.</param>
        /// <returns>The count of all fulfillments for the shop.</returns>
        public virtual int Count(long orderId, CountFilter filter = null)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments/count.json", orderId));

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets a list of up to 250 of the order's fulfillments.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="options">Options for filtering the list.</param>
        /// <returns>The list of fulfillments matching the filter.</returns>
        public virtual List<Fulfillment> List(long orderId, ListFilter options = null)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments.json", orderId));

            if (options != null)
            {
                req.QueryParams.AddRange(options.ToParameters());
            }

            return ExecuteRequest<List<Fulfillment>>(req, HttpMethod.Get, rootElement: "fulfillments");
        }

        /// <summary>
        /// Retrieves the <see cref="Fulfillment"/> with the given id.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="fulfillmentId">The id of the Fulfillment to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="Fulfillment"/>.</returns>
        public virtual Fulfillment Get(long orderId, long fulfillmentId, string fields = null)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments/{1}.json", orderId, fulfillmentId));

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<Fulfillment>(req, HttpMethod.Get, rootElement: "fulfillment");
        }

        /// <summary>
        /// Creates a new <see cref="Fulfillment"/> on the order.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="fulfillment">A new <see cref="Fulfillment"/>. Id should be set to null.</param>
        /// <param name="notifyCustomer">Whether the customer should be notified that the fulfillment
        /// has been created.</param>
        /// <returns>The new <see cref="Fulfillment"/>.</returns>
        public virtual Fulfillment Create(long orderId, Fulfillment fulfillment, bool notifyCustomer)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments.json", orderId));
            var body = fulfillment.ToDictionary();
            body.Add("notify_customer", notifyCustomer);

            var content = new JsonContent(new
            {
                fulfillment = body
            });

            return ExecuteRequest<Fulfillment>(req, HttpMethod.Post, content, "fulfillment");
        }

        /// <summary>
        /// Updates the given <see cref="Fulfillment"/>.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="">Id of the object being updated.</param>
        /// <param name="fulfillment">The <see cref="Fulfillment"/> to update.</param>
        /// <returns>The updated <see cref="Fulfillment"/>.</returns>
        public virtual Fulfillment Update(long orderId, long fulfillmentId, Fulfillment fulfillment, bool notifyCustomer=false)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments/{1}.json", orderId, fulfillmentId));

            var body = fulfillment.ToDictionary();
            body.Add("notify_customer", notifyCustomer);

            var content = new JsonContent(new
            {
                fulfillment = body
            });

            return ExecuteRequest<Fulfillment>(req, HttpMethod.Put, content, "fulfillment");
        }

        /// <summary>
        /// Completes a pending fulfillment with the given id.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="fulfillmentId">The fulfillment's id.</param>
        public virtual Fulfillment Complete(long orderId, long fulfillmentId)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments/{1}/complete.json", orderId, fulfillmentId));

            return ExecuteRequest<Fulfillment>(req, HttpMethod.Post, rootElement: "fulfillment");
        }

        /// <summary>
        /// Cancels a pending fulfillment with the given id.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="fulfillmentId">The fulfillment's id.</param>
        public virtual Fulfillment Cancel(long orderId, long fulfillmentId)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments/{1}/cancel.json", orderId, fulfillmentId));

            return ExecuteRequest<Fulfillment>(req, HttpMethod.Post, rootElement: "fulfillment");
        }


        /// <summary>
        /// Opens a pending fulfillment with the given id.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="fulfillmentId">The fulfillment's id.</param>
        public virtual Fulfillment Open(long orderId, long fulfillmentId)
        {
            var req = PrepareRequest(string.Format("orders/{0}/fulfillments/{1}/open.json", orderId, fulfillmentId));

            return ExecuteRequest<Fulfillment>(req, HttpMethod.Post, rootElement: "fulfillment");
        }

    }
}
