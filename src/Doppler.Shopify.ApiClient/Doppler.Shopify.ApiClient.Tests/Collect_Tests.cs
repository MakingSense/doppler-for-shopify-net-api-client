using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ShopifySharp.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Collect")]
    public class Collect_Tests : IClassFixture<Collect_Tests_Fixture>
    {
        private Collect_Tests_Fixture Fixture { get; private set; }

        public Collect_Tests(Collect_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Collects()
        {
            var count = Fixture.Service.Count();

            Assert.NotNull(count);
        }

        [Fact]
        public void Lists_Collects()
        {
            var collects = Fixture.Service.List();

            Assert.True(collects.Count() > 0);
        }

        [Fact]
        public void Lists_Collects_With_A_Filter()
        {
            var productId = Fixture.Created.First().ProductId;
            var collects = Fixture.Service.List(new CollectFilter()
            {
                ProductId = productId,
            });

            Assert.True(collects.Count() > 0);
            Assert.All(collects, collect => Assert.True(collect.ProductId > 0));
        }

        [Fact]
        public void Gets_Collects()
        {
            var collect = Fixture.Service.Get(Fixture.Created.First().Id.Value);

            Assert.NotNull(collect);
            Assert.True(collect.Id.HasValue);
            Assert.Equal(Fixture.CollectionId, collect.CollectionId);
            Assert.True(collect.ProductId > 0);
        }

        [Fact]
        public void Deletes_Collects()
        {
            var created = Fixture.Create(true);
            bool thrown = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.Write(string.Format("{nameof(Deletes_Collects)} failed. {ex.Message}.");
                
                thrown = true;
            }

            Assert.False(thrown);
        }

        [Fact]
        public void Creates_Collects()
        {
            var collect = Fixture.Create();

            Assert.NotNull(collect);
            Assert.True(collect.Id.HasValue);
            Assert.Equal(Fixture.CollectionId, collect.CollectionId);
            Assert.True(collect.ProductId > 0);
        }
    }

    public class Collect_Tests_Fixture : IAsyncLifetime
    {
        public CollectService Service { get; private set; } = new CollectService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<Collect> Created { get; private set; } = new List<Collect>();

        /// <remarks>
        /// Hardcoded collection id used in previous versions was 27369427.
        /// </remarks>
        public long CollectionId { get; set; }

        public void Initialize()
        {
            // Create a collection to use with these tests.
            var collection = new CustomCollectionService(Utils.MyShopifyUrl, Utils.AccessToken).Create(new CustomCollection()
            {
                Title = "Things",
                Published = false,
                Image = new CustomCollectionImage()
                {
                    Src = "http://placehold.it/250x250"
                }
            });

            CollectionId = collection.Id.Value;

            // Create a collection to use with get, list, count, etc. tests.
            Create();
        }

        public void Dispose()
        {
            var productService = new ProductService(Utils.MyShopifyUrl, Utils.AccessToken);

            foreach (var obj in Created)
            {
                try
                {
                    Service.Delete(obj.Id.Value);
                    productService.Delete(obj.ProductId.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete created Collect with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }

            // Delete the collection
            new CustomCollectionService(Utils.MyShopifyUrl, Utils.AccessToken).Delete(CollectionId);
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<Collect> Create(bool skipAddToCreatedList = false)
        {
            // Create a product to use with these tests.
            var product = new ProductService(Utils.MyShopifyUrl, Utils.AccessToken).Create(new ShopifySharp.Product()
            {
                CreatedAt = DateTime.UtcNow,
                Title = "Burton Custom Freestlye 151",
                Vendor = "Burton",
                BodyHtml = "<strong>Good snowboard!</strong>",
                ProductType = "Snowboard",
                Handle = Guid.NewGuid().ToString(),
                Images = new List<ProductImage> { new ProductImage { Attachment = "R0lGODlhAQABAIAAAAAAAAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==" } },
                PublishedScope = "published"
            });
            var obj = Service.Create(new Collect()
            {
                CollectionId = CollectionId,
                ProductId = product.Id.Value,
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
