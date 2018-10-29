using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ShopifySharp.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "ProductImage")]
    public class ProductImage_Tests : IClassFixture<ProductImage_Tests_Fixture>
    {
        private ProductImage_Tests_Fixture Fixture { get; private set; }

        public ProductImage_Tests(ProductImage_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_ProductImages()
        {
            var count = Fixture.Service.Count(Fixture.ProductId);

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_ProductImages()
        {
            var list = Fixture.Service.List(Fixture.ProductId);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_ProductImages()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(Fixture.ProductId, created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Deletes_ProductImages)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_ProductImages()
        {
            var image = Fixture.Service.Get(Fixture.ProductId, Fixture.Created.First().Id.Value);

            Assert.NotNull(image);
            Assert.True(image.Id.HasValue);
            Assert.Equal(Fixture.ProductId, image.ProductId);
        }

        [Fact]
        public void Creates_ProductImages()
        {
            var created = Fixture.Create();

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal(Fixture.ProductId, created.ProductId);
        }

        [Fact]
        public void Updates_ProductImages()
        {
            var created = Fixture.Create();
            var newAlt = string.Format("ShopifySharp test {Guid.NewGuid()}";
            long id = created.Id.Value;
            
            created.Alt = newAlt;       
            created.Id = null;

            var updated = Fixture.Service.Update(created.ProductId.Value, id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(newAlt, updated.Alt);
        }
    }

    public class ProductImage_Tests_Fixture : IAsyncLifetime
    {
        public string ImageFileName => "image-filename.jpg";

        public ProductService ProductService { get; private set; } = new ProductService(Utils.MyShopifyUrl, Utils.AccessToken);

        public ProductImageService Service { get; private set; } = new ProductImageService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<ProductImage> Created { get; private set; } = new List<ProductImage>();

        public long ProductId { get; set; }

        public void Initialize()
        {
            // Get a product to use as the parent for all images.
            ProductId = (ProductService.List(new ProductFilter()
            {
                Limit = 1
            })).First().Id.Value;

            // Create one for count, list, get, etc. orders.
            Create();
        }

        public void Dispose()
        {
            foreach (var obj in Created)
            {
                try
                {
                    Service.Delete(ProductId, obj.Id.Value);
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
        public async Task<ProductImage> Create(bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(ProductId, new ProductImage()
            {
                Filename = ImageFileName,
                Attachment = "R0lGODlhAQABAIAAAAAAAAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==\n"
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
