using Newtonsoft.Json.Linq;
using System.Net.Http;
using ShopifySharp.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopifySharp.Infrastructure;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating Shopify price rules.
    /// </summary>
    public class PriceRuleService : ShopifyService
    {

        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public PriceRuleService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a list of up to 250 of the shop's price rules.
        /// </summary>
        public virtual List<PriceRule> List(PriceRuleFilter options = null)
        {
            var req = PrepareRequest("price_rules.json");

            if (options != null)
            {
                req.QueryParams.AddRange(options.ToParameters());
            }

            return ExecuteRequest<List<PriceRule>>(req, HttpMethod.Get, rootElement: "price_rules");
        }

        /// <summary>
        /// Gets a list of up to 250 of the shop's price rules.
        /// </summary>
        public virtual List<PriceRule> List(Dictionary<string, object> options)
        {
            var req = PrepareRequest("price_rules.json");

            if (options != null)
            {
                req.QueryParams.AddRange(options);
            }

            return ExecuteRequest<List<PriceRule>>(req, HttpMethod.Get, rootElement: "price_rules");
        }

        /// <summary>
        /// Retrieves the object with the given id.
        /// </summary>
        /// <param name="id">The id of the object to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        public virtual PriceRule Get(long id, string fields = null)
        {
            var req = PrepareRequest(string.Format("price_rules/{0}.json", id));

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<PriceRule>(req, HttpMethod.Get, rootElement: "price_rule");
        }

        /// <summary>
        /// Creates a new price rule.
        /// </summary>
        /// <param name="rule">A new price rule. Id should be set to null.</param>
        public virtual PriceRule Create(PriceRule rule)
        {
            var req = PrepareRequest("price_rules.json");
            var body = rule.ToDictionary();

            var content = new JsonContent(new
            {
                price_rule = body
            });

            return ExecuteRequest<PriceRule>(req, HttpMethod.Post, content, "price_rule");
        }

        /// <summary>
        /// Updates the given object.
        /// </summary>
        /// <param name="id">Id of the object being updated.</param>
        /// <param name="rule">The updated rule.</param>
        public virtual PriceRule Update(long id, PriceRule rule)
        {
            var req = PrepareRequest(string.Format("price_rules/{0}.json", id));
            var content = new JsonContent(new
            {
                price_rule = rule
            });

            return ExecuteRequest<PriceRule>(req, HttpMethod.Put, content, "price_rule");
        }

        /// <summary>
        /// Deletes the object with the given Id.
        /// </summary>
        /// <param name="id">The object's Id.</param>
        public virtual void Delete(long id)
        {
            var req = PrepareRequest(string.Format("price_rules/{0}.json", id));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}
