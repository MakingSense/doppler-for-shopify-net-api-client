using System.Net.Http;
using ShopifySharp.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopifySharp.Infrastructure;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating Shopify discount codes.
    /// </summary>
    public class DiscountCodeService : ShopifyService
    {

        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public DiscountCodeService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a list of up to 250 of the shop's discount codes.
        /// </summary>
        /// <returns></returns>
        public virtual List<PriceRuleDiscountCode> List(long priceRuleId)
        {
            var req = PrepareRequest(string.Format("price_rules/{0}/discount_codes.json", priceRuleId));

            return ExecuteRequest<List<PriceRuleDiscountCode>>(req, HttpMethod.Get, rootElement: "discount_codes");
        }

        /// <summary>
        /// Retrieves the <see cref="PriceRuleDiscountCode"/> with the given id.
        /// </summary>
        /// <param name="priceRuleId">The id of the associated price rule.</param>
        /// <param name="discountId">The id of the discount to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="PriceRuleDiscountCode"/>.</returns>
        public virtual PriceRuleDiscountCode Get(long priceRuleId, long discountId, string fields = null)
        {
            var req = PrepareRequest(string.Format("price_rules/{0}/discount_codes/{1}.json", priceRuleId, discountId));

            if (string.IsNullOrEmpty(fields) == false)
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<PriceRuleDiscountCode>(req, HttpMethod.Get, rootElement: "discount_code");
        }

        /// <summary>
        /// Creates a new discount code.
        /// </summary>
        /// <param name="priceRuleId">Id of an existing price rule.</param>
        public virtual PriceRuleDiscountCode Create(long priceRuleId, PriceRuleDiscountCode code)
        {
            var req = PrepareRequest(string.Format("price_rules/{0}/discount_codes.json", priceRuleId));
            var body = code.ToDictionary();

            var content = new JsonContent(new
            {
                discount_code = body
            });

            return ExecuteRequest<PriceRuleDiscountCode>(req, HttpMethod.Post, content, "discount_code");
        }

        /// <summary>
        /// Updates the given object.
        /// </summary>
        /// <param name="priceRuleId">The Id of the Price Rule being updated.</param>
        /// <param name="code">The code being updated.</param>
        public virtual PriceRuleDiscountCode Update(long priceRuleId, PriceRuleDiscountCode code)
        {
            var req = PrepareRequest(string.Format("price_rules/{0}/discount_codes/{1}.json", priceRuleId, code.Id.Value));
            var content = new JsonContent(new
            {
                discount_code = code
            });

            return ExecuteRequest<PriceRuleDiscountCode>(req, HttpMethod.Put, content, "discount_code");
        }

        /// <summary>
        /// Removes the discount with the specified Id.
        /// </summary>
        /// /// <param name="priceRuleId">The price rule object's Id.</param>
        /// <param name="discountId">The discount object's Id.</param>
        public virtual void Delete(long priceRuleId, long discountCodeId)
        {
            var req = PrepareRequest(string.Format("price_rules/{0}/discount_codes/{1}.json", priceRuleId, discountCodeId));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}