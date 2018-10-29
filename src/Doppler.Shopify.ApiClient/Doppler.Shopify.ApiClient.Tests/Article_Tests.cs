using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Article")]
    public class Article_Tests : IClassFixture<Article_Tests_Fixture>
    {
        private Article_Tests_Fixture Fixture { get; set; }

        public Article_Tests(Article_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Articles()
        {
            var count = Fixture.Service.Count(Fixture.BlogId.Value);

            Assert.True(count > 0);
        }

        [Fact]
        public void Creates_Articles()
        {
            var article = Fixture.Create();

            Assert.True(article.Id.HasValue);
            Assert.Equal(Fixture.Author, article.Author);
            Assert.Equal(Fixture.BodyHtml, article.BodyHtml);
            Assert.Equal(Fixture.BlogId, article.BlogId);
            Assert.StartsWith(Fixture.Title, article.Title);
            EmptyAssert.NotNullOrEmpty(article.Handle);
            EmptyAssert.NotNullOrEmpty(article.Tags);
        }

        [Fact]
        public void Deletes_Articles()
        {
            var article = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(Fixture.BlogId.Value, article.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_Articles threw exception. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Lists_Articles()
        {
            var articles = Fixture.Service.List(Fixture.BlogId.Value);

            Assert.True(articles.Count() > 0);
        }

        [Fact]
        public void Lists_Authors()
        {
            var authors = Fixture.Service.ListAuthors();

            Assert.True(authors.Count() > 0);
            Assert.True(authors.Any(a => a == Fixture.Author));
        }

        [Fact]
        public void Lists_Tags()
        {
            var tags = Fixture.Service.ListTags();

            Assert.True(tags.Count() > 0);
        }

        [Fact]
        public void Lists_Tags_For_Blog()
        {
            var tags = Fixture.Service.ListTagsForBlog(Fixture.BlogId.Value);

            Assert.True(tags.Count() > 0);
        }

        public void Updates_Articles()
        {
            string html = "<h1>Updated!</h1>";
            var article = Fixture.Create();
            long id = article.Id.Value;

            article.BodyHtml = html;
            article.Id = null;

            article = Fixture.Service.Update(Fixture.BlogId.Value, id, article);

            // Reset the id so the Fixture can properly delete this object.
            article.Id = id;

            Assert.Equal(article.BodyHtml, html);
        }
    }

    public class Article_Tests_Fixture : IDisposable
    {
        public ArticleService Service { get; private set; }

        public Article_Tests_Fixture()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            Service = new ArticleService(Utils.MyShopifyUrl, Utils.AccessToken);
            Created = new List<Article>();
            Initialize();
        }

        public string Title { get { return "My new Article title - "; } }

        public string Author { get { return "John Smith"; } }

        public string Tags { get { return "This Post, Has Been Tagged"; } }

        public string BodyHtml { get { return "<h1>I like articles</h1>\n<p><strong>Yea</strong>, I like posting them through <span class=\"caps\">REST</span>.</p>"; } }

        public long? BlogId { get; set; }

        public List<Article> Created { get; private set; }

        public void Initialize()
        {
            var blogService = new BlogService(Utils.MyShopifyUrl, Utils.AccessToken);
            var blogs = blogService.List();

            BlogId = blogs.First().Id;

            // Create at least one article for list, count, etc commands.
            Create();
        }

        public void Dispose()
        {
            foreach (var article in Created)
            {
                try
                {
                    Service.Delete(BlogId.Value, article.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete Article with id {0}. {1}", article.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates the object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public Article Create(bool skipAddToDeleteList = false)
        {
            var obj = Service.Create(BlogId.Value, new Article()
            {
                Title = Title + Guid.NewGuid(),
                Author = Author,
                Tags = Tags,
                BodyHtml = BodyHtml,
                Image = new ArticleImage()
                {
                    Attachment = "R0lGODlhAQABAIAAAAAAAAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==\n"
                }
            });

            if (!skipAddToDeleteList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
