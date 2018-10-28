using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating Shopify's UsageCharge API.
    /// </summary>
    public class UsageChargeService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="UsageChargeService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public UsageChargeService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Creates a <see cref="UsageCharge"/>.
        /// </summary>
        /// <param name="recurringChargeId">The id of the <see cref="UsageCharge"/> that this usage charge belongs to.</param>
        /// <param name="description">The name or description of the usage charge.</param>
        /// <param name="price">The price of the usage charge.</param>
        /// <returns>The new <see cref="UsageCharge"/>.</returns>
        public virtual UsageCharge Create(long recurringChargeId, string description, decimal price)
        {
            var req = PrepareRequest(string.Format("recurring_application_charges/{0}/usage_charges.json", recurringChargeId));
            var content = new JsonContent(new
            {
                usage_charge = new
                {
                    description = description,
                    price = price
                }
            });

            return ExecuteRequest<UsageCharge>(req, HttpMethod.Post, content, "usage_charge");
        }

        /// <summary>
        /// Retrieves the <see cref="UsageCharge"/> with the given id.
        /// </summary>
        /// <param name="recurringChargeId">The id of the recurring charge that this usage charge belongs to.</param>
        /// <param name="id">The id of the charge to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="UsageCharge"/>.</returns>
        public virtual UsageCharge Get(long recurringChargeId, long id, string fields = null)
        {
            var req = PrepareRequest(string.Format("recurring_application_charges/{0}/usage_charges/{1}.json", recurringChargeId, id));

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<UsageCharge>(req, HttpMethod.Get, rootElement: "usage_charge");
        }

        /// <summary>
        /// Retrieves a list of all past and present <see cref="UsageCharge"/> objects.
        /// </summary>
        /// <param name="recurringChargeId">The id of the recurring charge that these usage charges belong to.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The list of <see cref="UsageCharge"/> objects.</returns>
        public virtual List<UsageCharge> List(long recurringChargeId, string fields = null)
        {
            var req = PrepareRequest(string.Format("recurring_application_charges/{0}/usage_charges.json", recurringChargeId));

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<List<UsageCharge>>(req, HttpMethod.Get, rootElement: "usage_charges");
        }
    }
}
