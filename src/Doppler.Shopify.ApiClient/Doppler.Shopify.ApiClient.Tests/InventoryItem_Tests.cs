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
    [Trait("Category", "InventoryItem")]
    public class InventoryItem_Tests : IClassFixture<InventoryItem_Tests_Fixture>
    {
        private InventoryItem_Tests_Fixture Fixture { get; private set; }

        public InventoryItem_Tests(InventoryItem_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Lists_Items()
        {
            var list = Fixture.Service.List(new ListFilter { Ids = new[] { Fixture.Created.First().InventoryItemId.Value } });
            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Gets_Item()
        {
            var created = Fixture.Service.Get(Fixture.Created.First().InventoryItemId.Value);
            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
        }

        [Fact]
        public void Updates_Item()
        {
            var created = Fixture.Service.Get( Fixture.Created.First().InventoryItemId.Value );
            long id = created.Id.Value;
            string sku = "Some Updated sku";

            created.SKU = sku;

            var updated = Fixture.Service.Update( id, created );

            Assert.Equal( sku, updated.SKU );
        }
    }

    public class InventoryItem_Tests_Fixture : IAsyncLifetime
    {
        public InventoryItemService Service { get; private set; } = new InventoryItemService(Utils.MyShopifyUrl, Utils.AccessToken);

        public ProductVariantService VariantService { get; private set; } = new ProductVariantService(Utils.MyShopifyUrl, Utils.AccessToken);

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
                    VariantService.Delete(ProductId, obj.Id.Value);
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
            var obj = VariantService.Create(ProductId, new ProductVariant()
            {
                Option1 = Guid.NewGuid().ToString(),
                Price = Price,
                InventoryManagement = "shopify",
                SKU = "Some sku"
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
