using Newtonsoft.Json.Linq;
using System.Net.Http;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopifySharp.Infrastructure;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating mapping between shopify products and collections
    /// </summary>
    public class CollectService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="CustomerService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public CollectService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all of the collects (product-collection mappings).
        /// </summary>
        /// <returns>The count of all collects for the shop.</returns>
        public virtual int Count(CollectFilter filter = null)
        {
            var req = PrepareRequest("collects/count.json");

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets a list of up to 250 of the shop's collects.
        /// </summary>
        /// <returns></returns>
        public virtual List<Collect> List(CollectFilter options = null)
        {
            var req = PrepareRequest("collects.json");

            if (options != null)
            {
                req.QueryParams.AddRange(options.ToParameters());
            }

            return ExecuteRequest<List<Collect>>(req, HttpMethod.Get, rootElement: "collects");
        }

        /// <summary>
        /// Retrieves the <see cref="Collect"/> with the given id.
        /// </summary>
        /// <param name="collectId">The id of the collect to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="Collect"/>.</returns>
        public virtual Collect Get(long collectId, string fields = null)
        {
            var req = PrepareRequest(string.Format("collects/{0}.json", collectId));

            if (string.IsNullOrEmpty(fields) == false)
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<Collect>(req, HttpMethod.Get, rootElement: "collect");
        }


        /// <summary>
        /// Creates a new <see cref="Collect"/> on the store. Map product to collection
        /// </summary>
        /// <param name="collect">A new <see cref="Collect"/>. Id should be set to null.</param>
        /// <returns>The new <see cref="Collect"/>.</returns>
        public virtual Collect Create(Collect collect)
        {
            var req = PrepareRequest("collects.json");
            var content = new JsonContent(new
            {
                collect = collect
            });

            return ExecuteRequest<Collect>(req, HttpMethod.Post, content, "collect");
        }

        /// <summary>
        /// Deletes a collect with the given Id.
        /// </summary>
        /// <param name="collectId">The product object's Id.</param>
        public virtual void Delete(long collectId)
        {
            var req = PrepareRequest(string.Format("collects/{0}.json", collectId));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}
