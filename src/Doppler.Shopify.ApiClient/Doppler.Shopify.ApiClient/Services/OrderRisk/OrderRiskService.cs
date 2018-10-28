﻿using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating Shopify order risks.
    /// </summary>
    public class OrderRiskService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="OrderRiskService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public OrderRiskService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a list of all order risks for an order.
        /// </summary>
        /// <param name="orderId">The order the risks belong to.</param>
        public virtual List<OrderRisk> List(long orderId)
        {
            var req = PrepareRequest(string.Format("orders/{0}/risks.json", orderId));
            
            return ExecuteRequest<List<OrderRisk>>(req, HttpMethod.Get, rootElement: "risks");
        }
        
        /// <summary>
        /// Retrieves the <see cref="OrderRisk"/> with the given id.
        /// </summary>
        /// <param name="orderId">The order the risk belongs to.</param>
        /// <param name="riskId">The id of the risk to retrieve.</param>
        public virtual OrderRisk Get(long orderId, long riskId)
        {
            var req = PrepareRequest(string.Format("orders/{0}/risks/{1}.json", orderId, riskId));
            
            return ExecuteRequest<OrderRisk>(req, HttpMethod.Get, rootElement: "risk");
        }
        
        /// <summary>
        /// Creates a new <see cref="OrderRisk"/> on the order.
        /// </summary>
        /// <param name="orderId">The order the risk belongs to.</param>
        /// <param name="risk">A new <see cref="OrderRisk"/>. Id should be set to null.</param>
        public virtual OrderRisk Create(long orderId, OrderRisk risk)
        {
            var req = PrepareRequest(string.Format("orders/{0}/risks.json", orderId));
            var content = new JsonContent(new
            {
                risk = risk
            });
            
            return ExecuteRequest<OrderRisk>(req, HttpMethod.Post, content, "risk");
        }

        /// <summary>
        /// Updates the given <see cref="OrderRisk"/>.
        /// </summary>
        /// <param name="orderRiskId">Id of the object being updated.</param>
        /// <param name="orderId">The order the risk belongs to.</param>
        /// <param name="risk">The risk to update.</param>
        public virtual OrderRisk Update(long orderId, long orderRiskId, OrderRisk risk)
        {
            var req = PrepareRequest(string.Format("orders/{0}/risks/{1}.json", orderId, orderRiskId));
            var content = new JsonContent(new
            {
                risk = risk
            });

            return ExecuteRequest<OrderRisk>(req, HttpMethod.Put, content, "risk");
        }

        /// <summary>
        /// Deletes an order with the given Id.
        /// </summary>
        /// <param name="orderId">The order the risk belongs to.</param>
        /// <param name="riskId">The risk's id.</param>
        public virtual void Delete(long orderId, long riskId)
        {
            var req = PrepareRequest(string.Format("orders/{0}/risks/{1}.json", orderId, riskId));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}
