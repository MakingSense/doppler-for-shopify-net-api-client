using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "FulfillmentService")]
    public class FulfillmentService_Tests : IClassFixture<FulfillmentService_Tests_Fixture>
    {
        private FulfillmentService_Tests_Fixture Fixture { get; set; }

        public FulfillmentService_Tests(FulfillmentService_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }


        [Fact]
        public void Lists_FulfillmentServices()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Lists_All_FulfillmentServices()
        {
            var list = Fixture.Service.List("all");

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Gets_FulfillmentServices()
        {
            // Find an id 
            var created = Fixture.Created.First();
            var fulfillmentServiceEntity = Fixture.Service.Get(created.Id.Value);

            Assert.NotNull(fulfillmentServiceEntity);
        }

        [Fact]
        public void Creates_FulfillmentServices()
        {
            var created = Fixture.Create();

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
        }


        [Fact]
        public void Updates_FulfillmentServices()
        {
            string name = "Auntie Dot's Fulfillment Company";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Name = name;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(name, updated.Name);
        }

    }

    public class FulfillmentService_Tests_Fixture : IDisposable
    {
        private FulfillmentServiceService _service = new FulfillmentServiceService(Utils.MyShopifyUrl, Utils.AccessToken);
        private List<FulfillmentServiceEntity> _created = new List<FulfillmentServiceEntity>();

        public FulfillmentServiceService Service { get { return _service; } private set { _service = value; } }
        public List<FulfillmentServiceEntity> Created { get { return _created; } private set { _created = value; } }

        public FulfillmentService_Tests_Fixture()
        {
            Initialize();
        }

        public void Initialize()
        {
            // Fulfillment API has a stricter rate limit when on a non-paid store.
            Service.SetExecutionPolicy(new SmartRetryExecutionPolicy());

            // Create a fulfillment service for count, list, get, etc. tests.
            var fulfillmentServiceEntity = Create();
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
                    Console.WriteLine(string.Format("Failed to delete fulfillment service with id {0}. {1}", obj.Id.Value, ex.Message));
                }
            }
        }

        public FulfillmentServiceEntity Create(bool skipAddToCreateList = false)
        {
            FulfillmentServiceEntity fulfillmentServiceEntity = Service.Create(new FulfillmentServiceEntity()
            {
                Name = string.Format("MarsFulfillment{0}",DateTime.Now.Ticks),
                CallbackUrl = "http://google.com",
                InventoryManagement = false,
                TrackingSupport = false,
                RequiresShippingMethod = false,
                Format = "json",
            });

            if (!skipAddToCreateList) Created.Add(fulfillmentServiceEntity);

            return fulfillmentServiceEntity;
        }
    }
}
