﻿using System.Net.Http;
using Doppler.Shopify.ApiClient.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating a Shopify product's variants.
    /// </summary>
    public class ProductVariantService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="ProductVariantService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public ProductVariantService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all variants belonging to the given product.
        /// </summary>
        /// <param name="productId">The product that the variants belong to.</param>
        public virtual int Count(long productId)
        {
            var req = PrepareRequest(string.Format("products/{0}/variants/count.json", productId));

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets a list of variants belonging to the given product.
        /// </summary>
        /// <param name="productId">The product that the variants belong to.</param>
        /// <param name="filterOptions">Options for filtering the result.</param>
        public virtual List<ProductVariant> List(long productId, ListFilter filterOptions = null)
        {
            var req = PrepareRequest(string.Format("products/{0}/variants.json", productId));

            if (filterOptions != null)
            {
                req.QueryParams.AddRange(filterOptions.ToParameters());
            }

            return ExecuteRequest<List<ProductVariant>>(req, HttpMethod.Get, rootElement: "variants");
        }

        /// <summary>
        /// Retrieves the <see cref="ProductVariant"/> with the given id.
        /// </summary>
        /// <param name="variantId">The id of the product variant to retrieve.</param>
        public virtual ProductVariant Get(long variantId)
        {
            var req = PrepareRequest(string.Format("variants/{0}.json", variantId));

            return ExecuteRequest<ProductVariant>(req, HttpMethod.Get, rootElement: "variant");
        }

        /// <summary>
        /// Creates a new <see cref="ProductVariant"/>.
        /// </summary>
        /// <param name="productId">The product that the new variant will belong to.</param>
        /// <param name="variant">A new <see cref="ProductVariant"/>. Id should be set to null.</param>
        public virtual ProductVariant Create(long productId, ProductVariant variant)
        {
            var req = PrepareRequest(string.Format("products/{0}/variants.json", productId));
            var content = new JsonContent(new
            {
                variant = variant
            });

            return ExecuteRequest<ProductVariant>(req, HttpMethod.Post, content, "variant");
        }

        /// <summary>
        /// Updates the given <see cref="ProductVariant"/>.
        /// </summary>
        /// <param name="productVariantId">Id of the object being updated.</param>
        /// <param name="variant">The variant to update.</param>
        public virtual ProductVariant Update(long productVariantId, ProductVariant variant)
        {
            var req = PrepareRequest(string.Format("variants/{0}.json", productVariantId));
            var content = new JsonContent(new
            {
                variant = variant
            });

            return ExecuteRequest<ProductVariant>(req, HttpMethod.Put, content, "variant");
        }

        /// <summary>
        /// Deletes a product variant with the given Id.
        /// </summary>
        /// <param name="productId">The product that the variant belongs to.</param>
        /// <param name="variantId">The product variant's id.</param>
        public virtual void Delete(long productId, long variantId)
        {
            var req = PrepareRequest(string.Format("products/{0}/variants/{1}.json", productId, variantId));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}