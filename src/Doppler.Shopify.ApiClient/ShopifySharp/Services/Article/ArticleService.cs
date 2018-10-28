﻿using ShopifySharp.Filters;
using ShopifySharp.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating a blog's articles.
    /// </summary>
    public class ArticleService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of the service.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public ArticleService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a list of up to 250 articles belonging to the given blog.
        /// </summary>
        /// <param name="blogId">The blog that the articles belong to.</param>
        /// <param name="filter">Options for filtering the result.</param>
        public virtual List<Article> List(long blogId, ArticleFilter filter = null)
        {
            var req = PrepareRequest(string.Format("blogs/{0}/articles.json", blogId));

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<List<Article>>(req, HttpMethod.Get, rootElement: "articles");
        }

        /// <summary>
        /// Gets a count of the articles belonging to the given blog.
        /// </summary>
        /// <param name="blogId">The blog that the articles belong to.</param>
        /// <param name="filter">Options for filtering the result.</param>
        public virtual int Count(long blogId, PublishableCountFilter filter = null)
        {
            var req = PrepareRequest(string.Format("blogs/{0}/articles/count.json", blogId));

            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToParameters());
            }

            return ExecuteRequest<int>(req, HttpMethod.Get, rootElement: "count");
        }

        /// <summary>
        /// Gets an article with the given id.
        /// </summary>
        /// <param name="blogId">The blog that the article belongs to.</param>
        /// <param name="articleId">The article's id.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        public virtual Article Get(long blogId, long articleId, string fields = null)
        {
            var req = PrepareRequest(string.Format("blogs/{0}/articles/{1}.json", blogId, articleId));

            if (fields != null)
            {
                req.QueryParams.Add("fields", fields);
            }

            return ExecuteRequest<Article>(req, HttpMethod.Get, rootElement: "article");
        }

        /// <summary>
        /// Creates a new article on the given blog.
        /// </summary>
        /// <param name="blogId">The blog that the article will belong to.</param>
        /// <param name="article">The article being created. Id should be null.</param>
        /// <param name="metafields">Optional metafield data that can be returned by the <see cref="MetaFieldService"/>.</param>
        public virtual Article Create(long blogId, Article article, IEnumerable<MetaField> metafields = null)
        {
            var req = PrepareRequest(string.Format("blogs/{0}/articles.json", blogId));
            var body = article.ToDictionary();

            if (metafields != null)
            {
                body.Add("metafields", metafields);
            }

            var content = new JsonContent(new
            {
                article = body
            });

            return ExecuteRequest<Article>(req, HttpMethod.Post, content, "article");
        }

        /// <summary>
        /// Updates an article.
        /// </summary>
        /// <param name="blogId">The blog that the article belongs to.</param>
        /// <param name="articleId">Id of the object being updated.</param>
        /// <param name="article">The article being updated.</param>
        /// <param name="metafields">Optional metafield data that can be returned by the <see cref="MetaFieldService"/>.</param>
        public virtual Article Update(long blogId, long articleId, Article article, IEnumerable<MetaField> metafields = null)
        {
            var req = PrepareRequest(string.Format("blogs/{0}/articles/{1}.json", blogId, articleId));
            var body = article.ToDictionary();

            if (metafields != null)
            {
                body.Add("metafields", metafields);
            }

            var content = new JsonContent(new
            {
                article = body
            });

            return ExecuteRequest<Article>(req, HttpMethod.Put, content, "article");
        }

        /// <summary>
        /// Deletes an article with the given id.
        /// </summary>
        /// <param name="blogId">The blog that the article belongs to.</param>
        /// <param name="articleId">The article benig deleted.</param>
        public virtual void Delete(long blogId, long articleId)
        {
            var req = PrepareRequest(string.Format("blogs/{0}/articles/{1}.json", blogId, articleId));

            ExecuteRequest(req, HttpMethod.Delete);
        }

        /// <summary>
        /// Gets a list of all article authors.
        /// </summary>
        public virtual List<string> ListAuthors()
        {
            var req = PrepareRequest("articles/authors.json");

            return ExecuteRequest<List<string>>(req, HttpMethod.Get, rootElement: "authors");
        }

        /// <summary>
        /// Gets a list of all article tags.
        /// </summary>
        /// <param name="limit">The number of tags to return</param>
        /// <param name="popular">A flag to indicate only to a certain number of the most popular tags.</param>
        public virtual List<string> ListTags(int? popular = null, int? limit = null)
        {
            var req = PrepareRequest("articles/tags.json");

            if (popular.HasValue)
            {
                req.QueryParams.Add("popular", popular.Value);
            }

            if (limit.HasValue)
            {
                req.QueryParams.Add("limit", limit.Value);
            }

            return ExecuteRequest<List<string>>(req, HttpMethod.Get, rootElement: "tags");
        }

        /// <summary>
        /// Gets a list of all article tags for the given blog.
        /// </summary>
        /// <param name="blogId">The blog that the tags belong to.</param>
        /// <param name="limit">The number of tags to return</param>
        /// <param name="popular">A flag to indicate only to a certain number of the most popular tags.</param>
        public virtual List<string> ListTagsForBlog(long blogId, int? popular = null, int? limit = null)
        {
            var req = PrepareRequest(string.Format("blogs/{0}/articles/tags.json", blogId));

            if (popular.HasValue)
            {
                req.QueryParams.Add("popular", popular.Value);
            }

            if (limit.HasValue)
            {
                req.QueryParams.Add("limit", limit.Value);
            }

            return ExecuteRequest<List<string>>(req, HttpMethod.Get, rootElement: "tags");
        }
    }
}
