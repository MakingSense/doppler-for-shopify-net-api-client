using ShopifySharp.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "InventoryLevel")]
    public class InventoryLevel_Tests : IClassFixture<InventoryLevel_Tests_Fixture>
    {
        private InventoryLevel_Tests_Fixture Fixture { get; private set; }

        public InventoryLevel_Tests(InventoryLevel_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Lists_Items()
        {
            var list = Fixture.Service.List(new InventoryLevelFilter { InventoryItemIds = new[] { Fixture.InventoryItemId } });
            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Creates_InventoryLevel()
        {
            var created = Fixture.Create();
            Assert.NotNull(created);
        }

        [Fact]
        public void Updates_InventoryLevel()
        {
            var invLevel = (Fixture.Service.List(new InventoryLevelFilter
            {
                InventoryItemIds = new long[] { Fixture.InventoryItemId }
            })).First();

            Random newRandom = new Random();
            int newQty, currQty;
            newQty = currQty = invLevel.Available ?? 0;
            while (newQty == currQty)
            {
                invLevel.Available = newQty = newRandom.Next(5, 55);
            }

            var updated = Fixture.Service.Set(invLevel, true);

            Assert.Equal(newQty, updated.Available);
            Assert.NotEqual(currQty, updated.Available);
        }

        [Fact]
        public void Deletes_InventoryLevel()
        {
            var currentInvLevel = (Fixture.Service.List(new InventoryLevelFilter { InventoryItemIds = new[] { Fixture.InventoryItemId } })).First();
            //When switching from the default location to a Fulfillment location, the default InventoryLevel is deleted
            var created = Fixture.Create();
            //Set inventory back to original location because a location is required
            Fixture.Service.Set(currentInvLevel, true);
            bool threw = false;
            try
            {
                Fixture.Service.Delete(created.InventoryItemId.Value, created.LocationId.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Deletes_InventoryLevel)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
            //Delete will not throw an error but still will not delete if there isn't another location available for the product.
            Assert.Equal(0, (Fixture.Service.List(new InventoryLevelFilter { InventoryItemIds = new[] { created.InventoryItemId.Value }, LocationIds = new[] { created.LocationId.Value } })).Count());
        }
    }

    public class InventoryLevel_Tests_Fixture : IAsyncLifetime
    {
        public InventoryLevelService Service { get; private set; } = new InventoryLevelService(Utils.MyShopifyUrl, Utils.AccessToken);

        public Product_Tests_Fixture ProductTest { get; private set; } = new Product_Tests_Fixture();

        public ProductVariant_Tests_Fixture VariantTest { get; private set; } = new ProductVariant_Tests_Fixture();

        public FulfillmentService_Tests_Fixture FulfillmentServiceServTest { get; private set; } = new FulfillmentService_Tests_Fixture();

        public long InventoryItemId { get; set; }

        public void Initialize()
        {
            // Get a product id to use with these tests.
            var prod = ProductTest.Create();
            VariantTest.ProductId = prod.Id.Value;
            var variant = prod.Variants.First();
            InventoryItemId = variant.InventoryItemId.Value;
            variant.SKU = "TestSKU";//To change fulfillment, SKU is required
            variant.InventoryManagement = "Shopify";//To set inventory, InventoryManagement must be Shopify
            VariantTest.Service.Update(variant.Id.Value, variant);
        }

        public void Dispose()
        {
            VariantTest.Dispose();
            ProductTest.Dispose();
            FulfillmentServiceServTest.Dispose();
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<InventoryLevel> Create(bool skipAddToCreateList = false)
        {
            var locId = (FulfillmentServiceServTest.Create(skipAddToCreateList)).LocationId.Value;
            return Service.Connect(InventoryItemId, locId, true);
        }
    }
}