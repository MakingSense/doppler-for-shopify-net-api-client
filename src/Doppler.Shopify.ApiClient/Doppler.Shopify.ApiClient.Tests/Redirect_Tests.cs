using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Redirect")]
    public class Redirect_Tests : IClassFixture<Redirect_Tests_Fixture>
    {
        private Redirect_Tests_Fixture Fixture { get; private set; }

        public Redirect_Tests(Redirect_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Redirects()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Redirects()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_Redirects()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Deletes_Redirects)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_Redirects()
        {
            var created = Fixture.Create();
            var obj = Fixture.Service.Get(created.Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.Target, obj.Target);
            EmptyAssert.NotNullOrEmpty(obj.Path);
        }

        [Fact]
        public void Creates_Redirects()
        {
            var created = Fixture.Create();
            
            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal(Fixture.Target, created.Target);
            EmptyAssert.NotNullOrEmpty(created.Path);
        }

        [Fact]
        public void Updates_Redirects()
        {
            string newVal = "https://example.com/updated";
            var created = Fixture.Create();
            long id = created.Id.Value;
            
            created.Target = newVal;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(newVal, updated.Target);   
        }
    }

    public class Redirect_Tests_Fixture : IAsyncLifetime
    {
        public RedirectService Service { get; private set; } = new RedirectService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<Redirect> Created { get; private set; } = new List<Redirect>();

        public string Target => "https://www.example.com/";

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
                        Console.WriteLine(string.Format("Failed to delete created Redirect with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<Redirect> Create(bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(new Redirect()
            {
                Path = Guid.NewGuid().ToString(),
                Target = Target,
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}