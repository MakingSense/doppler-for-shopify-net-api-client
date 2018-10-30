using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Filters;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "InventoryItem")]
    public class InventoryItem_Tests : IClassFixture<InventoryItem_Tests_Fixture>
    {
        private InventoryItem_Tests_Fixture Fixture { get; set; }

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

    public class InventoryItem_Tests_Fixture : IDisposable
    {
        private InventoryItemService _service = new InventoryItemService(Utils.MyShopifyUrl, Utils.AccessToken);
        private ProductVariantService _variantService = new ProductVariantService(Utils.MyShopifyUrl, Utils.AccessToken);
        private List<ProductVariant> _created = new List<ProductVariant>();

        public InventoryItemService Service { get { return _service; } private set { _service = value; } }
        public ProductVariantService VariantService { get { return _variantService; } private set { _variantService = value; } }
        public List<ProductVariant> Created { get { return _created; } private set { _created = value; } }
        public decimal Price
        {
            get
            {
                return 123.45m;
            }
        }

        public long ProductId { get; set; }

        public InventoryItem_Tests_Fixture()
        {
            Initialize();
        }

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
                if (!obj.Id.HasValue)
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
                        Console.WriteLine(string.Format("Failed to delete created ProductVariant with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public ProductVariant Create(string option1 = null, bool skipAddToCreatedList = false)
        {
            var obj = VariantService.Create(ProductId, new ProductVariant()
            {
                Option1 = Guid.NewGuid().ToString(),
                Price = Price,
                InventoryManagement = "shopify",
                SKU = "Some sku"
            });

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
