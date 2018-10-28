using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Filters;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient 
{
    /// <summary>
    /// A service for manipulating Shopify draft orders.
    /// </summary>
    public class DraftOrderService : ShopifyService
    {

        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public DraftOrderService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }        

        public virtual int Count(DraftOrderFilter filter = null)
        {
            var req = PrepareRequest("draft_orders/count.json");

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets a list of up to 250 of the shop's draft orders.
        /// </summary>
        /// <param name="filter">Options for filtering the list.</param>
        public virtual List<DraftOrder> List(ListFilter filter = null)
        {
            var req = PrepareRequest("draft_orders.json");

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<List<DraftOrder>>(req, HttpMethod.Get, rootElement: "draft_orders");
        }

        /// <summary>
        /// Retrieves the object with the given id.
        /// </summary>
        /// <param name="id">The id of the object to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        public virtual DraftOrder Get(long id, string fields = null)
        {
            var req = PrepareRequest(string.Format("draft_orders/{0}.json", id));

            if (string.IsNullOrEmpty(fields) == false)
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<DraftOrder>(req, HttpMethod.Get, rootElement: "draft_order");
        }

        /// <summary>
        /// Creates a new draft order.
        /// </summary>
        /// <param name="order">A new DraftOrder. Id should be set to null.</param>
        /// <param name="useCustomerDefaultAddress">Optional boolean that you can send as part of a draft order object to load customer shipping information. Defaults to false.</param>
        public virtual DraftOrder Create(DraftOrder order, bool useCustomerDefaultAddress = false)
        {
            var req = PrepareRequest("draft_orders.json");
            var body = order.ToDictionary();

            body.Add("use_customer_default_address", useCustomerDefaultAddress);

            var content = new JsonContent(new 
            {
                draft_order = body
            });

            return ExecuteRequest<DraftOrder>(req, HttpMethod.Post, content, "draft_order");
        }

        /// <summary>
        /// Updates the draft order with the given id.
        /// </summary>
        /// <param name="id">The id of the item being updated.</param>
        /// <param name="order">The updated draft order.</param>
        public virtual DraftOrder Update(long id, DraftOrder order)
        {
            var req = PrepareRequest(string.Format("draft_orders/{0}.json", id));
            var content = new JsonContent(new 
            {
                draft_order = order.ToDictionary()
            });

            return ExecuteRequest<DraftOrder>(req, HttpMethod.Put, content, "draft_order");
        }

        /// <summary>
        /// Deletes the draft order with the given id.
        /// </summary>
        /// <param name="id">The id of the item being deleted.</param>
        public virtual void Delete(long id)
        {
            var req = PrepareRequest(string.Format("draft_orders/{0}.json", id));

            ExecuteRequest(req, HttpMethod.Delete);
        }

        /// <summary>
        /// Completes the draft order, transitioning it to a full order.
        /// </summary>
        /// <param name="id">The id of the item being completed.</param>
        /// <param name="paymentPending">A bool indicating whether payment is pending or not. True if payment is pending, false if payment is not pending and order has been paid. Defaults to false (payment not pending).</param>
        public virtual DraftOrder Complete(long id, bool paymentPending = false)
        {
            var req = PrepareRequest(string.Format("draft_orders/{0}/complete.json", id));
            req.QueryParams.Add("payment_pending", paymentPending);

            return ExecuteRequest<DraftOrder>(req, HttpMethod.Put, rootElement: "draft_order");
        }

        /// <summary>
        /// Send an invoice for the draft order.
        /// </summary>
        /// <param name="id">The id of the item with the invoice.</param>
        public virtual DraftOrderInvoice SendInvoice(long id, DraftOrderInvoice customInvoice = null)
        {
            var req = PrepareRequest(string.Format("draft_orders/{0}/send_invoice.json", id));
            // If the custom invoice is not null, use that as the body. Else use an empty dictionary object which will send the default invoice
            var body = customInvoice != null ? customInvoice.ToDictionary() : new Dictionary<string, object>();
            var content = new JsonContent(new 
            {
                draft_order_invoice = body
            });

            return ExecuteRequest<DraftOrderInvoice>(req, HttpMethod.Post, content, "draft_order_invoice");
        }
    }
}