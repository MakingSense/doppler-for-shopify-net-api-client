using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ShopifySharp.Filters;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "ProductVariant")]
    public class ProductVariant_Tests : IClassFixture<ProductVariant_Tests_Fixture>
    {
        private ProductVariant_Tests_Fixture Fixture { get; private set; }

        public ProductVariant_Tests(ProductVariant_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Variants()
        {
            var count = Fixture.Service.Count(Fixture.ProductId);

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Variants()
        {
            var list = Fixture.Service.List(Fixture.ProductId);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_Variants()
        {
            var created = Fixture.Create(skipAddToCreatedList: true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(Fixture.ProductId, created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Deletes_Variants)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_Variants()
        {
            var created = Fixture.Service.Get(Fixture.Created.First().Id.Value);

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal(Fixture.Price, created.Price);
            EmptyAssert.NotNullOrEmpty(created.Option1);
        }

        [Fact]
        public void Creates_Variants()
        {
            var created = Fixture.Create();

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal(Fixture.Price, created.Price);
            EmptyAssert.NotNullOrEmpty(created.Option1);
        }

        [Fact]
        public void Updates_Variants()
        {
            decimal newPrice = (decimal) 11.22;
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Price = newPrice;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(newPrice, updated.Price);
        }
    }

    public class ProductVariant_Tests_Fixture : IAsyncLifetime
    {
        public ProductVariantService Service { get; private set; } = new ProductVariantService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<ProductVariant> Created { get; private set; } = new List<ProductVariant>();

        public decimal Price => 123.45m;

        public long ProductId { get; set; }

        public void Initialize()
        {
            // Get a product id to use with these tests.
            ProductId = (new ProductService(Utils.MyShopifyUrl, Utils.AccessToken).List(new ProductFilter()
            {
                Limit = 1
            })).First().Id.Value;

            // Create one for use with count, list, get, etc. tests.
            Create();
        }

        public void Dispose()
        {
            foreach (var obj in Created)
            {
                if (! obj.Id.HasValue) 
                {
                    continue; 
                }

                try
                {
                    Service.Delete(ProductId, obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete created ProductVariant with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<ProductVariant> Create(string option1 = null, bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(ProductId, new ProductVariant()
            {
                Option1 = Guid.NewGuid().ToString(),
                Price = Price,
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
