using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating Shopify transactions.
    /// </summary>
    public class TransactionService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="TransactionService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public TransactionService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all of the shop's transactions.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <returns>The count of all fulfillments for the shop.</returns>
        public virtual int Count(long orderId)
        {
            var req = PrepareRequest(string.Format("orders/{0}/transactions/count.json", orderId));

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets a list of up to 250 of the shop's transactions.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="sinceId">Filters the results to after the specified id.</param>
        /// <returns>The list of transactions.</returns>
        public virtual List<Transaction> List(long orderId, long? sinceId = null)
        {
            var req = PrepareRequest(string.Format("orders/{0}/transactions.json", orderId));

            if (sinceId.HasValue)
            {
                req.QueryParams.Add("since_id", sinceId.Value);
            }

            return ExecuteRequest<List<Transaction>>(req, HttpMethod.Get, rootElement: "transactions");
        }

        /// <summary>
        /// Retrieves the <see cref="Transaction"/> with the given id.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="transactionId">The id of the Transaction to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="Transaction"/>.</returns>
        public virtual Transaction Get(long orderId, long transactionId, string fields = null)
        {
            var req = PrepareRequest(string.Format("orders/{0}/transactions/{1}.json", orderId, transactionId));

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<Transaction>(req, HttpMethod.Get, rootElement: "transaction");
        }

        /// <summary>
        /// Creates a new <see cref="Transaction"/> of the given kind.
        /// </summary>
        /// <param name="orderId">The order id to which the fulfillments belong.</param>
        /// <param name="transaction">The transaction.</param>
        /// <returns>The new <see cref="Transaction"/>.</returns>
        public virtual Transaction Create(long orderId, Transaction transaction)
        {
            var req = PrepareRequest(string.Format("orders/{0}/transactions.json", orderId));
            var content = new JsonContent(new
            {
                transaction = transaction
            });

            return ExecuteRequest<Transaction>(req, HttpMethod.Post, content, "transaction");
        }
    }
}
