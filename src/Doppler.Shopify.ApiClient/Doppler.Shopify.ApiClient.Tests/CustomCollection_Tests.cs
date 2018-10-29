using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "CustomCollection")]
    public class CustomCollection_Tests : IClassFixture<CustomCollection_Tests_Fixture>
    {
        private CustomCollection_Tests_Fixture Fixture { get; set; }

        public CustomCollection_Tests(CustomCollection_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_CustomCollections()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_CustomCollections()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Gets_CustomCollections()
        {
            var collection = Fixture.Service.Get(Fixture.Created.First().Id.Value);

            Assert.NotNull(collection);
            Assert.True(collection.Id.HasValue);
            Assert.Equal(Fixture.Title, collection.Title);
        }

        [Fact]
        public void Deletes_CustomCollections()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_CustomCollections failed. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Creates_CustomCollections()
        {
            var collection = Fixture.Create();

            Assert.NotNull(collection);
            Assert.True(collection.Id.HasValue);
            Assert.Equal(Fixture.Title, collection.Title);
        }

        [Fact]
        public void Updates_CustomCollections()
        {
            string newTitle = "New Title";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Title = newTitle;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(newTitle, updated.Title);   
        }
    }

    public class CustomCollection_Tests_Fixture : IDisposable
    {
        public CustomCollectionService Service { get; private set; }

        public List<CustomCollection> Created { get; private set; }

        public string Title { get { return "Things"; } }

        public CustomCollection_Tests_Fixture()
        {
            Service  = new CustomCollectionService(Utils.MyShopifyUrl, Utils.AccessToken);
            Created  = new List<CustomCollection>();
            Initialize();
        }

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
                        Console.WriteLine(string.Format("Failed to delete created CustomCollection with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public CustomCollection Create(bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(new CustomCollection()
            {
                Title = Title,
                Published = false
            });

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}