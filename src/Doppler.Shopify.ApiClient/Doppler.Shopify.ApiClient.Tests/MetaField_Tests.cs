using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "MetaField")]
    public class MetaField_Tests : IClassFixture<MetaField_Tests_Fixture>
    {
        private MetaField_Tests_Fixture Fixture { get; set; }

        public MetaField_Tests(MetaField_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Metafields()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Counts_Metafields_On_Resources()
        {
            var count = Fixture.Service.Count(Fixture.ResourceId, Fixture.ResourceType);
            
            Assert.True(count > 0);
        }

        [Fact]
        public void Counts_Metafields_On_Resources_And_Parent()
        {
            var count = Fixture.Service.Count(Fixture.ChildResourceId, Fixture.ChildResourceType, Fixture.ResourceId, Fixture.ResourceType);
            
            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Metafields()
        {
            var list = Fixture.Service.List().Where(i=>i.Namespace == Fixture.Namespace && i.Description == Fixture.Description);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Lists_Metafields_On_Resources()
        {
            var list = Fixture.Service.List(Fixture.ResourceId, Fixture.ResourceType)
                .Where(i=>i.Namespace == Fixture.Namespace && i.Description == Fixture.Description);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Lists_Metafields_On_Resources_And_Parent()
        {
            var list = Fixture.Service.List(Fixture.ChildResourceId, Fixture.ChildResourceType, Fixture.ResourceId, Fixture.ResourceType)
                .Where(i=>i.Namespace == Fixture.Namespace && i.Description == Fixture.Description);
            
            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_Metafields()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_Metafields failed. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Creates_Metafields()
        {
            var created = Fixture.Create();
            
            Assert.NotNull(created);
            Assert.Equal(Fixture.Namespace, created.Namespace);
            Assert.Equal(Fixture.Description, created.Description);
            EmptyAssert.NotNullOrEmpty(created.Key);
            Assert.NotNull(created.Value);
        }

        [Fact]
        public void Creates_Metafields_On_Resources()
        {
            var created = Fixture.Create(Fixture.ResourceId, Fixture.ResourceType);

            Assert.NotNull(created);
            Assert.Equal(Fixture.Namespace, created.Namespace);
            Assert.Equal(Fixture.Description, created.Description);
            EmptyAssert.NotNullOrEmpty(created.Key);
            Assert.NotNull(created.Value);
            Assert.True(new string[] { Fixture.ResourceType, Fixture.ResourceType.Substring(0, Fixture.ResourceType.Length - 1) }.Contains(created.OwnerResource));
            Assert.Equal(Fixture.ResourceId, created.OwnerId);
        }

        [Fact]
        public void Creates_Metafields_On_Resources_And_Parent()
        {
            var created = Fixture.Create(Fixture.ChildResourceId,Fixture.ChildResourceType,Fixture.ResourceId, Fixture.ResourceType);

            Assert.NotNull(created);
            Assert.Equal(Fixture.Namespace, created.Namespace);
            Assert.Equal(Fixture.Description, created.Description);
            EmptyAssert.NotNullOrEmpty(created.Key);
            Assert.NotNull(created.Value);
            Assert.True(new string[] { Fixture.ChildResourceType, Fixture.ChildResourceType.Substring(0, Fixture.ChildResourceType.Length - 1) }.Contains(created.OwnerResource));
            Assert.Equal(Fixture.ChildResourceId, created.OwnerId);
        }

        [Fact]
        public void Updates_Metafields()
        {
            string value = "10";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Value = value;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(value, updated.Value.ToString());
        }

        [Fact]
        public void Updates_Metafields_On_Resources()
        {
            string value = "10";
            var created = Fixture.Create(Fixture.ResourceId, Fixture.ResourceType);
            long id = created.Id.Value;

            created.Value = value;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(value, updated.Value.ToString());
        }

        [Fact]
        public void Updates_Metafields_On_Child_Resources()
        {
            string value = "10";
            var created = Fixture.Create(Fixture.ChildResourceId, Fixture.ChildResourceType);
            long id = created.Id.Value;

            created.Value = value;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(value, updated.Value.ToString());
        }

        [Fact]
        public void Updates_Metafields_On_Resources_And_Parent()
        {
            string value = "10";
            var created = Fixture.Create(Fixture.ChildResourceId,Fixture.ChildResourceType,Fixture.ResourceId, Fixture.ResourceType);
            long id = created.Id.Value;

            created.Value = value;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(value, updated.Value.ToString());
        }
    }

    public class MetaField_Tests_Fixture : IDisposable
    {
        private MetaFieldService _service = new MetaFieldService(Utils.MyShopifyUrl, Utils.AccessToken);
        private List<MetaField> _created = new List<Doppler.Shopify.ApiClient.MetaField>();

        public MetaFieldService Service { get { return _service; } private set { _service = value; } }
        public List<Doppler.Shopify.ApiClient.MetaField> Created { get { return _created; } private set { _created = value; } }
        public string Namespace
        {
            get
            {
                return "testing";
            }
        }

        public string Description
        {
            get
            {
                return "This is a test meta field. It is an integer value.";
            }
        }

        public string ResourceType
        {
            get
            {
                return "products";
            }
        }

        public string ChildResourceType
        {
            get
            {
                return "variants";
            }
        }

        public long ResourceId { get; set; }
        public long ChildResourceId { get; set; }

        public MetaField_Tests_Fixture()
        {
            Initialize();
        }

        public void Initialize()
        {
            // Get a product to add metafields to.
            var products = new ProductService(Utils.MyShopifyUrl, Utils.AccessToken).List();
            ResourceId = products.First().Id.Value;
            ChildResourceId = products.First().Variants.First().Id.Value;

            // Create a metafield for use in count, list, get, etc. tests.
            Create();
            Create(ResourceId, ResourceType);
            Create(ChildResourceId, ChildResourceType, ResourceId, ResourceType);
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
                        Console.WriteLine(string.Format("Failed to delete created MetaField with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public MetaField Create(bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(new MetaField()
            {
                Namespace = Namespace,
                Key = Guid.NewGuid().ToString().Substring(0, 25),
                Value = "5",
                ValueType = "integer",
                Description = Description,
            });

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public MetaField Create(long targetId, string resourceType, bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(new MetaField()
            {
                Namespace = Namespace,
                Key = Guid.NewGuid().ToString().Substring(0, 25),
                Value = "5",
                ValueType = "integer",
                Description = Description,
            }, targetId, resourceType);

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public MetaField Create(long targetId, string resourceType, long parentTargetId, string parentResourceType, bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(new MetaField()
            {
                Namespace = Namespace,
                Key = Guid.NewGuid().ToString().Substring(0, 25),
                Value = "5",
                ValueType = "integer",
                Description = Description,
            }, targetId, resourceType, parentTargetId, parentResourceType);

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
