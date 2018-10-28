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

        /// <summary>
        /// Retrieves the <see cref="Product"/> with the given id.
        /// </summary>
        /// <param name="productId">The id of the product to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="Product"/>.</returns>
        public virtual Product Get(long productId, string fields = null)
        {
            var req = PrepareRequest(string.Format("products/{0}.json", productId));

            if (string.IsNullOrEmpty(fields) == false)
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<Product>(req, HttpMethod.Get, rootElement: "product");
        }

        /// <summary>
        /// Creates a new <see cref="Product"/> on the store.
        /// </summary>
        /// <param name="product">A new <see cref="Product"/>. Id should be set to null.</param>
        /// <param name="options">Options for creating the product.</param>
        /// <returns>The new <see cref="Product"/>.</returns>
        public virtual Product Create(Product product, ProductCreateOptions options = null)
        {
            var req = PrepareRequest("products.json");
            var body = product.ToDictionary();

            if (options != null)
            {
                foreach (var option in options.ToDictionary())
                {
                    body.Add(option);
                }
            }

            var content = new JsonContent(new
            {
                product = body
            });

            return ExecuteRequest<Product>(req, HttpMethod.Post, content, "product");
        }

        /// <summary>
        /// Updates the given <see cref="Product"/>.
        /// </summary>
        /// <param name="productId">Id of the object being updated.</param>
        /// <param name="product">The <see cref="Product"/> to update.</param>
        /// <returns>The updated <see cref="Product"/>.</returns>
        public virtual Product Update(long productId, Product product)
        {
            var req = PrepareRequest(string.Format("products/{0}.json", productId));
            var content = new JsonContent(new
            {
                product = product
            });

            return ExecuteRequest<Product>(req, HttpMethod.Put, content, "product");
        }

        /// <summary>
        /// Deletes a product with the given Id.
        /// </summary>
        /// <param name="productId">The product object's Id.</param>
        public virtual void Delete(long productId)
        {
            var req = PrepareRequest(string.Format("products/{0}.json", productId));

            ExecuteRequest(req, HttpMethod.Delete);
        }

        /// <summary>
        /// Publishes an unpublished <see cref="Product"/>.
        /// </summary>
        /// <param name="id">The product's id.</param>
        /// <returns>The published <see cref="Product"/></returns>
        public virtual Product Publish(long id)
        {
            var req = PrepareRequest(string.Format("products/{0}.json", id));
            var content = new JsonContent(new
            {
                product = new
                {
                    id = id,
                    published = true
                }
            });

            return ExecuteRequest<Product>(req, HttpMethod.Put, content, "product");
        }

        /// <summary>
        /// Unpublishes an published <see cref="Product"/>.
        /// </summary>
        /// <param name="id">The product's id.</param>
        /// <returns>The unpublished <see cref="Product"/></returns>
        public virtual Product Unpublish(long id)
        {
            var req = PrepareRequest(string.Format("products/{0}.json", id));
            var content = new JsonContent(new
            {
                product = new
                {
                    id = id,
                    published = false
                }
            });

            return ExecuteRequest<Product>(req, HttpMethod.Put, content, "product");
        }
    }
}
