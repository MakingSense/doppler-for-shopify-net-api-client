using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ShopifySharp.Infrastructure;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating Shopify discounts.
    /// </summary>
    public class DiscountService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="DiscountService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public DiscountService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a list of up to 250 of the shop's discounts.
        /// </summary>
        /// <returns></returns>
        public virtual List<Discount> List()
        {
            var req = PrepareRequest("discounts.json");

            return ExecuteRequest<List<Discount>>(req, HttpMethod.Get, rootElement: "discounts");
        }

        /// <summary>
        /// Retrieves the <see cref="Discount"/> with the given id.
        /// </summary>
        /// <param name="discountId">The id of the discount to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="Discount"/>.</returns>
        public virtual Discount Get(long discountId, string fields = null)
        {
            var req = PrepareRequest(string.Format("discounts/{0}.json", discountId));

            if (string.IsNullOrEmpty(fields) == false)
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<Discount>(req, HttpMethod.Get, rootElement: "discount");
        }

        /// <summary>
        /// Creates a new <see cref="Discount"/> on the store.
        /// </summary>
        /// <param name="discount">A new <see cref="Discount"/>. Id should be set to null.</param>
        /// <returns>The new <see cref="Discount"/>.</returns>
        public virtual Discount Create(Discount discount)
        {
            var req = PrepareRequest("discounts.json");

            // Build the request body as a dictionary. Necessary because the create options must be added to the
            // 'discount' property.
            var discountBody = discount.ToDictionary();
            var content = new JsonContent(new
            {
                discount = discountBody
            });

            return ExecuteRequest<Discount>(req, HttpMethod.Post, content, "discount");
        }

        /// <summary>
        /// Enables the <see cref="Discount"/> with the Id specified.
        /// </summary>
        /// <param name="discountId">The Id of the <see cref="Discount"/> to be enabled.</param>
        /// <returns>The updated <see cref="Discount"/>.</returns>
        public virtual Discount Enable(long discountId)
        {
            var req = PrepareRequest(string.Format("discounts/{0}/enable.json", discountId));

            return ExecuteRequest<Discount>(req, HttpMethod.Put, rootElement: "discount");
        }

        /// <summary>
        /// Disables the <see cref="Discount"/> with the Id specified.
        /// </summary>
        /// <param name="discountId">The Id of the <see cref="Discount"/> to be disabled.</param>
        /// <returns>The updated <see cref="Discount"/>.</returns>
        public virtual Discount Disable(long discountId)
        {
            var req = PrepareRequest(string.Format("discounts/{0}/disable.json", discountId));

            return ExecuteRequest<Discount>(req, HttpMethod.Put, rootElement: "discount");
        }

        /// <summary>
        /// Removes the discount with the specified Id.
        /// </summary>
        /// <param name="discountId">The discount object's Id.</param>
        public virtual void Delete(long discountId)
        {
            var req = PrepareRequest(string.Format("discounts/{0}.json",discountId));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}
