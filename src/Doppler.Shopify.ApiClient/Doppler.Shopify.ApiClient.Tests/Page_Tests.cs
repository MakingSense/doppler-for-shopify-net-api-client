using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Page")]
    public class Page_Tests : IClassFixture<Page_Tests_Fixture>
    {
        private Page_Tests_Fixture Fixture { get; private set; }

        public Page_Tests(Page_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Pages()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Pages()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_Pages()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Deletes_Pages)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_Pages()
        {
            var page = Fixture.Service.Get(Fixture.Created.First().Id.Value);

            Assert.NotNull(page);
            Assert.Equal(Fixture.Title, page.Title);
            Assert.Equal(Fixture.Html, page.BodyHtml);
        }

        [Fact]
        public void Creates_Pages()
        {
            var created = Fixture.Create();

            Assert.NotNull(created);
            Assert.Equal(Fixture.Title, created.Title);
            Assert.Equal(Fixture.Html, created.BodyHtml);
        }

        [Fact]
        public void Updates_Pages()
        {
            string html = "<h1>This string was updated while testing ShopifySharp!</h1>";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.BodyHtml = html;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(html, updated.BodyHtml);
        }
    }

    public class Page_Tests_Fixture : IAsyncLifetime
    {
        public PageService Service { get; private set; } = new PageService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<Page> Created { get; private set; } = new List<Page>();

        public string Title => "ShopifySharp Page API Tests";

        public string Html => "<strong>This string was created by ShopifySharp!</strong>";

        public void Initialize()
        {
            // Create one for count, list, get, etc. orders.
            Create();
        }

        public void Dispose()
        {
            foreach (var obj in Created)
            {
                try
                {
                    Service.Delete(obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete created Page with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<Page> Create(bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(new ShopifySharp.Page()
            {
                CreatedAt = DateTime.UtcNow,
                Title = Title,
                BodyHtml = Html,
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
