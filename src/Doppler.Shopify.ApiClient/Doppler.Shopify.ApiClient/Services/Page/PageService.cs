﻿using Newtonsoft.Json.Linq;
using System.Net.Http;
using Doppler.Shopify.ApiClient.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating Shopify pages.
    /// </summary>
    public class PageService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="PageService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public PageService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a count of all of the shop's pages.
        /// </summary>
        /// <returns>The count of all pages for the shop.</returns>
        public virtual int Count(PageFilter filter = null)
        {
            var req = PrepareRequest("pages/count.json");

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets a list of up to 250 of the shop's pages.
        /// </summary>
        /// <returns></returns>
        public virtual List<Page> List(PageFilter options = null)
        {
            var req = PrepareRequest("pages.json");

            if (options != null)
            {
                req.QueryParams.AddRange(options.ToParameters());
            }

            return ExecuteRequest<List<Page>>(req, HttpMethod.Get, rootElement: "pages");
        }

        /// <summary>
        /// Retrieves the <see cref="Page"/> with the given id.
        /// </summary>
        /// <param name="pageId">The id of the page to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="Page"/>.</returns>
        public virtual Page Get(long pageId, string fields = null)
        {
            var req = PrepareRequest(string.Format("pages/{0}.json", pageId));

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<Page>(req, HttpMethod.Get, rootElement: "page");
        }

        /// <summary>
        /// Creates a new <see cref="Page"/> on the store.
        /// </summary>
        /// <param name="page">A new <see cref="Page"/>. Id should be set to null.</param>
        /// <param name="options">Options for creating the page.</param>
        /// <returns>The new <see cref="Page"/>.</returns>
        public virtual Page Create(Page page, PageCreateOptions options = null)
        {
            var req = PrepareRequest("pages.json");
            var body = page.ToDictionary();

            if (options != null)
            {
                foreach (var option in options.ToDictionary())
                {
                    body.Add(option);
                }
            }

            var content = new JsonContent(new
            {
                page = body
            });

            return ExecuteRequest<Page>(req, HttpMethod.Post, content, "page");
        }

        /// <summary>
        /// Updates the given <see cref="Page"/>.
        /// </summary>
        /// <param name="pageId">Id of the object being updated.</param>
        /// <param name="page">The <see cref="Page"/> to update.</param>
        /// <returns>The updated <see cref="Page"/>.</returns>
        public virtual Page Update(long pageId, Page page)
        {
            var req = PrepareRequest(string.Format("pages/{0}.json", pageId));
            var content = new JsonContent(new
            {
                page = page
            });

            return ExecuteRequest<Page>(req, HttpMethod.Put, content, "page");
        }

        /// <summary>
        /// Deletes a page with the given Id.
        /// </summary>
        /// <param name="pageId">The page object's Id.</param>
        public virtual void Delete(long pageId)
        {
            var req = PrepareRequest(string.Format("pages/{0}.json", pageId));

            ExecuteRequest(req, HttpMethod.Delete);
        }
    }
}
