using System.Net.Http;
using ShopifySharp.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopifySharp.Infrastructure;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating Shopify's smart collections.
    /// </summary>
    public class SmartCollectionService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="SmartCollectionService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public SmartCollectionService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all smart collections on the store.
        /// </summary>
        /// <param name="filterOptions">Options for filtering the count.</param>
        public virtual int Count(SmartCollectionFilter filterOptions = null)
        {
            var req = PrepareRequest("smart_collections/count.json");

            if (filterOptions != null)
            {
                req.QueryParams.AddRange(filterOptions.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets a list of up to 250 smart collections.
        /// </summary>
        /// <param name="filterOptions">Options for filtering the result.</param>
        public virtual List<SmartCollection> List(SmartCollectionFilter filterOptions = null)
        {
            var req = PrepareRequest("smart_collections.json");

            if (filterOptions != null)
            {
                req.QueryParams.AddRange(filterOptions.ToParameters());
            }

            return ExecuteRequest<List<SmartCollection>>(req, HttpMethod.Get, rootElement: "smart_collections");
        }

        /// <summary>
        /// Retrieves the <see cref="SmartCollection"/> with the given id.
        /// </summary>
        /// <param name="collectionId">The id of the smart collection to retrieve.</param>
        public virtual SmartCollection Get(long collectionId)
        {
            var req = PrepareRequest(string.Format("smart_collections/{0}.json", collectionId));

            return ExecuteRequest<SmartCollection>(req, HttpMethod.Get, rootElement: "smart_collection");
        }

        /// <summary>
        /// Creates a new <see cref="SmartCollection"/>.
        /// </summary>
        /// <param name="collection">A new <see cref="SmartCollection"/>. Id should be set to null.</param>
        public virtual SmartCollection Create(SmartCollection collection, bool published = true)
        {
            var req = PrepareRequest("smart_collections.json");
            var body = collection.ToDictionary();

            body.Add("published", published);

            var content = new JsonContent(new
            {
                smart_collection = body
            });

            return ExecuteRequest<SmartCollection>(req, HttpMethod.Post, content, "smart_collection");
        }

        /// <summary>
        /// Updates the given <see cref="SmartCollection"/>.
        /// </summary>
        /// <param name="smartCollectionId">Id of the object being updated.</param>
        /// <param name="collection">The smart collection to update.</param>
        public virtual SmartCollection Update(long smartCollectionId, SmartCollection collection)
        {
            var req = PrepareRequest(string.Format("smart_collections/{0}.json", smartCollectionId));
            var content = new JsonContent(new
            {
                smart_collection = collection
            });
            return ExecuteRequest<SmartCollection>(req, HttpMethod.Put, content, "smart_collection");
        }

        /// <summary>
        /// Publishes an unpublished smart collection.
        /// </summary>
        /// <param name="smartCollectionId">The collection's id.</param>
        public virtual SmartCollection Publish(long smartCollectionId)
        {
            var req = PrepareRequest(string.Format("smart_collections/{0}.json", smartCollectionId));
            var body = new Dictionary<string, object>()
            {
                { "id", smartCollectionId },
                { "published", true }
            };
            var content = new JsonContent(new 
            {
                smart_collection = body
            });

            return ExecuteRequest<SmartCollection>(req, HttpMethod.Put, content, "smart_collection");
        }

        /// <summary>
        /// Publishes an unpublished smart collection.
        /// </summary>
        /// <param name="smartCollectionId">The collection's id.</param>
        public virtual SmartCollection Unpublish(long smartCollectionId)
        {
            var req = PrepareRequest(string.Format("smart_collections/{0}.json", smartCollectionId));
            var body = new Dictionary<string, object>()
            {
                { "id", smartCollectionId },
                { "published", false }
            };
            var content = new JsonContent(new 
            {
                smart_collection = body
            });

            return ExecuteRequest<SmartCollection>(req, HttpMethod.Put, content, "smart_collection");
        }

        /// <summary>
        /// Updates the order of products when a SmartCollection's sort-by method is set to "manual".
        /// </summary>
        /// <param name="smartCollectionId">Id of the object being updated.</param>
        /// <param name="sortOrder">The order in which products in the smart collection appear. Note that specifying productIds parameter will have no effect unless the sort order is "manual"</param>
        /// <param name="productIds">An array of product ids sorted in the order you want them to appear in.</param>
        public virtual void UpdateProductOrder(long smartCollectionId, string sortOrder = null, params long[] productIds)
        {
            var req = PrepareRequest(string.Format("smart_collections/{0}/order.json", smartCollectionId));
            var content = new JsonContent(new
            {
                sort_order = sortOrder,
                products = productIds
            });
            ExecuteRequest(req, HttpMethod.Put, content);
        }

        /// <summary>
        /// Deletes a smart collection with the given Id.
        /// </summary>
        /// <param name="collectionId">The smart collection's id.</param>
        public virtual void Delete(long collectionId)
        {
            var req = PrepareRequest(string.Format("smart_collections/{0}.json", collectionId));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}
