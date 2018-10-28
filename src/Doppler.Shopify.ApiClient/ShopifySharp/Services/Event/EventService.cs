using Newtonsoft.Json.Linq;
using System.Net.Http;
using ShopifySharp.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopifySharp
{
    /// <summary>
    /// A service for getting Shopify Events
    /// Reference: https://help.shopify.com/api/reference/event
    /// </summary>
    public class EventService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="EventService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public EventService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all site events.
        /// </summary>
        /// <param name="filter">Supports CreatedAtMin and CreatedAtMax Properties</param>
        /// <returns>The count of all site events.</returns>
        public virtual int Count(CountFilter filter = null)
        {
            var req = PrepareRequest("events/count.json");

            //Add optional parameters to request
            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Retrieves the <see cref="Event"/> with the given id.
        /// </summary>
        /// <param name="eventId">The id of the event to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="Event"/>.</returns>
        public virtual Event Get(long eventId, string fields = null)
        {
            var req = PrepareRequest(string.Format("events/{0}.json", eventId));

            if (string.IsNullOrEmpty(fields) == false)
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<Event>(req, HttpMethod.Get, rootElement: "event");
        }

        /// <summary>
        /// Returns a list of events for the given subject and subject type. A full list of supported subject types can be found at https://help.shopify.com/api/reference/event
        /// </summary>
        /// <param name="options">Options for filtering the result.</param>
        /// <param name="subjectId">Restricts results to just one subject item, e.g. all changes on a product.</param>
        /// <param name="subjectType">The subject's type, e.g. 'Order' or 'Product'. Known subject types are 'Articles', 'Blogs', 'Custom_Collections', 'Comments', 'Orders', 'Pages', 'Products' and 'Smart_Collections'.  A current list of subject types can be found at https://help.shopify.com/api/reference/event </param>
        public virtual List<Event> List(long subjectId, string subjectType, EventListFilter options = null)
        {
            // Ensure the subject type is plural
            if (!subjectType.Substring(subjectType.Length - 1).Equals("s", System.StringComparison.OrdinalIgnoreCase))
            {
                subjectType = subjectType + "s";
            }

            var req = PrepareRequest(string.Format("{0}/{1}/events.json", subjectType != null ? subjectType.ToLower() : "", subjectId));

            //Add optional parameters to request
            if (options != null)
            {
                req.QueryParams.AddRange(options.ToParameters());
            }

            return ExecuteRequest<List<Event>>(req, HttpMethod.Get, rootElement: "events");
        }

        /// <summary>
        /// Returns a list of events.
        /// </summary>
        /// <param name="options">Options for filtering the result.</param>
        public virtual List<Event> List(EventListFilter options = null)
        {
            var req = PrepareRequest("events.json");

            //Add optional parameters to request
            if (options != null)
            {
                req.QueryParams.AddRange(options.ToParameters());
            }

            return ExecuteRequest<List<Event>>(req, HttpMethod.Get, rootElement: "events");
        }
    }
}
