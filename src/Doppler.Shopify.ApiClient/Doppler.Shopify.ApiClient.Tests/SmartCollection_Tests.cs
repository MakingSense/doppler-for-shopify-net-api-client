using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "SmartCollection")]
    public class SmartCollection_Tests : IClassFixture<SmartCollection_Tests_Fixture>
    {
        private SmartCollection_Tests_Fixture Fixture { get; set; }

        public SmartCollection_Tests(SmartCollection_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_SmartCollections()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_SmartCollections()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_SmartCollections()
        {
            var created = Fixture.Create(true, true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_SmartCollections failed. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_SmartCollections()
        {
            var created = Fixture.Create();
            var obj = Fixture.Service.Get(created.Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.BodyHtml, obj.BodyHtml);
            Assert.Equal(Fixture.Title, obj.Title);
            Assert.StartsWith(Fixture.Handle, obj.Handle, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Creates_SmartCollections()
        {
            var obj = Fixture.Create();

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.BodyHtml, obj.BodyHtml);
            Assert.Equal(Fixture.Title, obj.Title);
            Assert.StartsWith(Fixture.Handle, obj.Handle, StringComparison. OrdinalIgnoreCase);
            Assert.NotNull(obj.PublishedAt);
            Assert.NotNull(obj.PublishedScope);
        }

        [Fact]
        public void Creates_Unpublished_SmartCollections()
        {
            var obj = Fixture.Create(false);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.BodyHtml, obj.BodyHtml);
            Assert.Equal(Fixture.Title, obj.Title);
            Assert.StartsWith(Fixture.Handle, obj.Handle, StringComparison.OrdinalIgnoreCase);
            Assert.Null(obj.PublishedAt);
        }

        [Fact]
        public void Updates_SmartCollections()
        {
            string newValue = "New Title";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Title = newValue;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(newValue, updated.Title);
        }

        [Fact]
        public void Publishes_SmartCollections()
        {
            var created = Fixture.Create(false);

            Assert.Null(created.PublishedAt);

            var updated = Fixture.Service.Publish(created.Id.Value);

            Assert.NotNull(updated.PublishedAt);
        }

        [Fact]
        public void Unpublishes_SmartCollections()
        {
            var created = Fixture.Create(true);

            Assert.NotNull(created.PublishedAt);

            var updated = Fixture.Service.Unpublish(created.Id.Value);

            Assert.Null(updated.PublishedAt);
        }

        [Fact(Skip = "This test has a bit of a time delay that Doppler.Shopify.ApiClient isn't equipped to handle yet (Retry-After header).")]
        public void Updates_SmartCollection_Products_Order()
        {
            //generate a unique tag
            var tag = Guid.NewGuid().ToString();

            //create collection
            var collection = Fixture.Service.Create(new SmartCollection()
            {
                BodyHtml = Fixture.BodyHtml,
                Handle = Fixture.Handle,
                Title = Fixture.Title,
                Rules = new List<SmartCollectionRules>
                {
                    new SmartCollectionRules
                    {
                        Column = "tag",
                        Condition = tag,
                        Relation = "equals"
                    }
                }
            });

            //create 4 products with unique tag
            var productService = new ProductService(Utils.MyShopifyUrl, Utils.AccessToken);
            var products = new List<Product>();
            for (var i = 0; i < 4; i++)
            {
                var product = productService.Create(new Product()
                {
                    Title = Guid.NewGuid().ToString(),
                    Tags = tag
                });
                products.Add(product);
            }

            //reorder items
            products.Reverse();
            var productIds = products.Select(p => p.Id.Value).ToArray();
            Fixture.Service.UpdateProductOrder(collection.Id.Value, "manual", productIds);


            //get collection
            collection = Fixture.Service.Get(collection.Id.Value);

            //get products  - use collect service to get products so they are returned in order
            var collectService = new CollectService(Utils.MyShopifyUrl, Utils.AccessToken);
            var collects = (collectService.List(new CollectFilter() { CollectionId = collection.Id })).ToList();

            //check
            Assert.Equal("manual", collection.SortOrder);
            collects.ForEach(c => Assert.True(productIds.Contains(c.ProductId.Value)));

            //delete the objects
            Fixture.Service.Delete(collection.Id.Value);
            products.ForEach(x => productService.Delete(x.Id.Value));

        }
    }

    public class SmartCollection_Tests_Fixture : IDisposable
    {
        private SmartCollectionService _service = new SmartCollectionService(Utils.MyShopifyUrl, Utils.AccessToken);
        private List<SmartCollection> _created = new List<SmartCollection>();

        public SmartCollectionService Service { get { return _service; } private set { _service = value; } }
        public List<SmartCollection> Created { get { return _created; } private set { _created = value; } }
        public string BodyHtml
        {
            get
            {
                return "<h1>Hello world!</h1>";
            }
        }

        public string Handle
        {
            get
            {
                return "Doppler-Shopify-ApiClient-Handle";
            }
        }

        public string Title
        {
            get
            {
                return "Doppler-Shopify-ApiClient Test Smart Collection";
            }
        }

        public SmartCollection_Tests_Fixture()
        {
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
                        Console.WriteLine(string.Format("Failed to delete created SmartCollection with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public SmartCollection Create(bool published = true, bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(new SmartCollection()
            {
                BodyHtml = BodyHtml,
                Handle = Handle,
                Title = Title,
                Rules = new List<SmartCollectionRules>
                {
                    new SmartCollectionRules
                    {
                        Column = "variant_price",
                        Condition = "20",
                        Relation = "less_than"
                    }
                }
            }, published);

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}