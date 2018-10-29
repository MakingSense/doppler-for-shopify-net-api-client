using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Webhook")]
    public class Webhook_Tests : IClassFixture<Webhook_Tests_Fixture>
    {
        private Webhook_Tests_Fixture Fixture { get; private set; }

        public Webhook_Tests(Webhook_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Webhooks()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Webhooks()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_Webhooks()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Deletes_Webhooks)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_Webhooks()
        {
            var obj = Fixture.Service.Get(Fixture.Created.First().Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.Format, obj.Format);
            Assert.StartsWith(Fixture.UrlPrefix, obj.Address);
        }

        [Fact]
        public void Creates_Webhooks()
        {
            var obj = Fixture.Create();

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.Format, obj.Format);
            Assert.StartsWith(Fixture.UrlPrefix, obj.Address);
        }

        [Fact]
        public void Updates_Webhooks()
        {
            string newValue = "https://requestb.in/" + Guid.NewGuid();
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Address = newValue;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(newValue, updated.Address);
        }
    }

    public class Webhook_Tests_Fixture : IAsyncLifetime
    {
        public WebhookService Service { get; private set; } = new WebhookService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<Webhook> Created { get; private set; } = new List<Webhook>();

        public string UrlPrefix => "https://requestb.in/";

        public string Format => "json";

        public void Initialize()
        {
            // Create one collection for use with count, list, get, etc. tests.
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
                        Console.WriteLine(string.Format("Failed to delete created Webhook with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<Webhook> Create(bool skipAddToCreatedList = false, string topic = "orders/create")
        {
            var obj = Service.Create(new Webhook()
            {
                Address = UrlPrefix + Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Fields = new string[] { "field1", "field2" },
                Format = Format,
                Topic = topic,
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}