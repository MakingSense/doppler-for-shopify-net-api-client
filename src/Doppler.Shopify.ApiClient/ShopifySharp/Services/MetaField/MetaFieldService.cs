using Newtonsoft.Json.Linq;
using System.Net.Http;
using ShopifySharp.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopifySharp.Infrastructure;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating Shopify metafields.
    /// </summary>
    public class MetaFieldService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="MetaFieldService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public MetaFieldService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        private int _Count(string path, MetaFieldFilter filter = null)
        {
            var req = PrepareRequest(path);

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets a count of the metafields on the shop itself.
        /// </summary>
        /// <param name="filter">Options to filter the results.</param>
        public virtual int Count(MetaFieldFilter filter = null)
        {
            return _Count("metafields/count.json", filter);
        }

        /// <summary>
        /// Gets a count of the metafields for the given entity type and filter options.
        /// </summary>
        /// <param name="resourceType">The type of shopify resource to obtain metafields for. This could be variants, products, orders, customers, custom_collections.</param>
        /// <param name="resourceId">The Id for the resource type.</param>
        /// <param name="filter">Options to filter the results.</param>
        public virtual int Count(long resourceId, string resourceType, MetaFieldFilter filter = null)
        {
            return _Count(string.Format("{0}/{1}/metafields/count.json", resourceType, resourceId), filter);
        }

        /// <summary>
        /// Gets a count of the metafields for the given entity type and filter options.
        /// </summary>
        /// <param name="resourceType">The type of shopify resource to obtain metafields for. This could be variants, products, orders, customers, custom_collections.</param>
        /// <param name="resourceId">The Id for the resource type.</param>
        /// <param name="parentResourceType">The type of shopify parent resource to obtain metafields for. This could be blogs, products.</param>
        /// <param name="parentResourceId">The Id for the resource type.</param>
        /// <param name="filter">Options to filter the results.</param>
        public virtual int Count(long resourceId, string resourceType, long parentResourceId, string parentResourceType,  MetaFieldFilter filter = null)
        {
            return _Count(string.Format("{0}/{1}/{2}/{3}/metafields/count.json", parentResourceType, parentResourceId, resourceType, resourceId), filter);
        }

        private List<MetaField> _List(string path, MetaFieldFilter filter = null)
        {
            var req = PrepareRequest(path);

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<List<MetaField>>(req, HttpMethod.Get, rootElement: "metafields");
        }

        /// <summary>
        /// Gets a list of the metafields for the shop itself.
        /// </summary>
        /// <param name="filter">Options to filter the results.</param>
        public virtual List<MetaField> List(MetaFieldFilter filter = null)
        {
            return _List("metafields.json", filter);
        }

        /// <summary>
        /// Gets a list of the metafields for the given entity type and filter options.
        /// </summary>
        /// <param name="resourceType">The type of shopify resource to obtain metafields for. This could be variants, products, orders, customers, custom_collections.</param>
        /// <param name="resourceId">The Id for the resource type.</param>
        /// <param name="filter">Options to filter the results.</param>
        public virtual List<MetaField> List(long resourceId, string resourceType, MetaFieldFilter filter = null)
        {
            return _List(string.Format("{0}/{1}/metafields.json", resourceType, resourceId), filter);
        }

        /// <summary>
        /// Gets a list of the metafields for the given entity type and filter options.
        /// </summary>
        /// <param name="parentResourceType">The type of shopify parentresource to obtain metafields for. This could be blogs, products.</param>
        /// <param name="parentResourceId">The Id for the parent resource type.</param>
        /// <param name="resourceType">The type of shopify resource to obtain metafields for. This could be variants, products, orders, customers, custom_collections.</param>
        /// <param name="resourceId">The Id for the resource type.</param>
        /// <param name="filter">Options to filter the results.</param>
        public virtual List<MetaField> List(long resourceId, string resourceType, long parentResourceId, string parentResourceType, MetaFieldFilter filter = null)
        {
            return _List(string.Format("{0}/{1}/{2}/{3}/metafields.json", parentResourceType, parentResourceId, resourceType, resourceId), filter);
        }


        /// <summary>
        /// Retrieves the metafield with the given id.
        /// </summary>
        /// <param name="metafieldId">The id of the metafield to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        public virtual MetaField Get(long metafieldId, string fields = null)
        {
            var req = PrepareRequest(string.Format("metafields/{0}.json", metafieldId));

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<MetaField>(req, HttpMethod.Get, rootElement: "metafield");
        }

        /// <summary>
        /// Creates a new metafield on the shop itself.
        /// </summary>
        /// <param name="metafield">A new metafield. Id should be set to null.</param>
        public virtual MetaField Create(MetaField metafield)
        {
            var req = PrepareRequest("metafields.json");
            var content = new JsonContent(new
            {
                metafield = metafield
            });

            return ExecuteRequest<MetaField>(req, HttpMethod.Post, content, "metafield");
        }

        /// <summary>
        /// Creates a new metafield on the given entity.
        /// </summary>
        /// <param name="metafield">A new metafield. Id should be set to null.</param>
        /// <param name="resourceId">The Id of the resource the metafield will be associated with. This can be variants, products, orders, customers, custom_collections, etc.</param>
        /// <param name="resourceType">The resource type the metaifeld will be associated with. This can be variants, products, orders, customers, custom_collections, etc.</param>
        /// <param name="parentResourceId">The Id of the parent resource the metafield will be associated with. This can be blogs, products.</param>
        /// <param name="parentResourceType">The resource type the metaifeld will be associated with. This can be articles, variants.</param>
        public virtual MetaField Create(MetaField metafield, long resourceId, string resourceType, long parentResourceId, string parentResourceType)
        {
            var req = PrepareRequest(string.Format("{0}/{1}/{2}/{3}/metafields.json", parentResourceType, parentResourceId, resourceType, resourceId));
            var content = new JsonContent(new
            {
                metafield = metafield
            });

            return ExecuteRequest<MetaField>(req, HttpMethod.Post, content, "metafield");
        }

        /// <summary>
        /// Creates a new metafield on the given entity.
        /// </summary>
        /// <param name="metafield">A new metafield. Id should be set to null.</param>
        /// <param name="resourceId">The Id of the resource the metafield will be associated with. This can be variants, products, orders, customers, custom_collections, etc.</param>
        /// <param name="resourceType">The resource type the metaifeld will be associated with. This can be variants, products, orders, customers, custom_collections, etc.</param>
        public virtual MetaField Create(MetaField metafield, long resourceId, string resourceType)
        {
            var req = PrepareRequest(string.Format("{0}/{1}/metafields.json", resourceType, resourceId));
            var content = new JsonContent(new
            {
                metafield = metafield
            });

            return ExecuteRequest<MetaField>(req, HttpMethod.Post, content, "metafield");
        }

        /// <summary>
        /// Updates the given metafield.
        /// </summary>
        /// <param name="metafieldId">Id of the object being updated.</param>
        /// <param name="metafield">The metafield to update.</param>
        public virtual MetaField Update(long metafieldId, MetaField metafield)
        {
            var req = PrepareRequest(string.Format("metafields/{0}.json", metafieldId));
            var content = new JsonContent(new
            {
                metafield = metafield
            });

            return ExecuteRequest<MetaField>(req, HttpMethod.Put, content, "metafield");
        }

        /// <summary>
        /// Deletes a metafield with the given Id.
        /// </summary>
        /// <param name="metafieldId">The metafield object's Id.</param>
        public virtual void Delete(long metafieldId)
        {
            var req = PrepareRequest(string.Format("metafields/{0}.json", metafieldId));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}
