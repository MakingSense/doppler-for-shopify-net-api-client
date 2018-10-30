using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Product")]
    public class Product_Tests : IClassFixture<Product_Tests_Fixture>
    {
        private Product_Tests_Fixture Fixture { get; set; }

        public Product_Tests(Product_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Products()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Products()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_Products()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_Products failed. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_Products()
        {
            var obj = Fixture.Service.Get(Fixture.Created.First().Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.Title, obj.Title);
            Assert.Equal(Fixture.BodyHtml, obj.BodyHtml);
            Assert.Equal(Fixture.ProductType, obj.ProductType);
            Assert.Equal(Fixture.Vendor, obj.Vendor);
        }

        [Fact]
        public void Creates_Products()
        {
            var obj = Fixture.Create();

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.Title, obj.Title);
            Assert.Equal(Fixture.BodyHtml, obj.BodyHtml);
            Assert.Equal(Fixture.ProductType, obj.ProductType);
            Assert.Equal(Fixture.Vendor, obj.Vendor);
        }

        [Fact]
        public void Creates_Unpublished_Products()
        {
            var created = Fixture.Create(options: new ProductCreateOptions()
            {
                Published = false
            });

            Assert.False(created.PublishedAt.HasValue);
        }

        [Fact]
        public void Updates_Products()
        {
            string title = "Doppler.Shopify.ApiClient Updated Test Product";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Title = title;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(title, updated.Title);
        }

        [Fact]
        public void Publishes_Products()
        {
            var created = Fixture.Create(options: new ProductCreateOptions()
            {
                Published = false
            });
            var published = Fixture.Service.Publish(created.Id.Value);

            Assert.True(published.PublishedAt.HasValue);
        }

        [Fact]
        public void Unpublishes_Products()
        {
            var created = Fixture.Create(options: new ProductCreateOptions()
            {
                Published = true
            });
            var unpublished = Fixture.Service.Unpublish(created.Id.Value);

            Assert.False(unpublished.PublishedAt.HasValue);
        }
    }

    public class Product_Tests_Fixture : IDisposable
    {
        public ProductService Service { get { return _service; } private set { _service = value; } }
        public List<Product> Created { get { return _created; } private set { _created = value; } }
        public string Title
        {
            get
            {
                return "Doppler.Shopify.ApiClient Test Product";
            }
        }

        private string vendor = "Auntie Dot";
        private ProductService _service = new ProductService(Utils.MyShopifyUrl, Utils.AccessToken);
        private List<Product> _created = new List<Product>();

        public string BodyHtml
        {
            get
            {
                return "<strong>This product was created while testing Doppler.Shopify.ApiClient!</strong>";
            }
        }

        public string ProductType
        {
            get
            {
                return "Foobars";
            }
        }

        public string Vendor { get { return vendor; } set { vendor = value; } }

        public Product_Tests_Fixture()
        {
            Initialize();
        }

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
                        Console.WriteLine(string.Format("Failed to delete created Product with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public Product Create(bool skipAddToCreateList = false, ProductCreateOptions options = null)
        {
            var obj = Service.Create(new Product()
            {
                Title = Title,
                Vendor = Vendor,
                BodyHtml = BodyHtml,
                ProductType = ProductType,
                Handle = Guid.NewGuid().ToString(),
                Images = new List<ProductImage>
                {
                    new ProductImage
                    {
                        Attachment = "R0lGODlhAQABAIAAAAAAAAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw=="
                    }
                },
            }, options);

            if (!skipAddToCreateList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
