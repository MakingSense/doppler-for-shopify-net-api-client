using Newtonsoft.Json.Linq;
using System.Net.Http;
using Doppler.Shopify.ApiClient.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating Shopify products.
    /// </summary>
    public class ProductService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="ProductService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public ProductService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all of the shop's products.
        /// </summary>
        /// <returns>The count of all products for the shop.</returns>
        public virtual int Count(ProductFilter filter = null)
        {
            var req = PrepareRequest("products/count.json");

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets a list of up to 250 of the shop's products.
        /// </summary>
        /// <returns></returns>
        public virtual List<Product> List(ProductFilter options = null)
        {
            var req = PrepareRequest("products.json");

            if (options != null)
            {
                req.QueryParams.AddRange(options.ToParameters());
            }

            return ExecuteRequest<List<Product>>(req, HttpMethod.Get, rootElement: "products");
        }
    }
}
