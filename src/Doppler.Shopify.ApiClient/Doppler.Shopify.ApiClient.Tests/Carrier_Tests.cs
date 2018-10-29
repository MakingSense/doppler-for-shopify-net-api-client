using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Carrier")]
    public class Carrier_Tests : IClassFixture<Carrier_Tests_Fixture>
    {
        private Carrier_Tests_Fixture Fixture { get; set; }

        public Carrier_Tests(Carrier_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact(Skip = "Shopify won't let us create more than one random carrier.")]
        public void Lists_Carriers()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() >= 0);
        }

        [Fact(Skip = "Shopify won't let us create more than one random carrier.")]
        public void Gets_Carriers()
        {
            var created = Fixture.Create();
            var carrier = Fixture.Service.Get(created.Id.Value);
            Fixture.Service.Delete(created.Id.Value);

            Assert.NotNull(carrier);
            Assert.True(carrier.Id.HasValue);
            Assert.Contains(Fixture.CallbackUrl, carrier.CallbackUrl);
        }

        [Fact(Skip = "Shopify won't let us create more than one random carrier.")]
        public void Deletes_Carriers()
        {
            var created = Fixture.Create();
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_Carriers failed. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }


        [Fact(Skip = "Shopify won't let us create more than one random carrier.")]
        public void Creates_Carriers()
        {
            var carrier = Fixture.Create();
            Fixture.Service.Delete(carrier.Id.Value);

            Assert.NotNull(carrier);
            Assert.True(carrier.Id.HasValue);
            Assert.Contains(Fixture.CallbackUrl, carrier.CallbackUrl);
        }

        [Fact(Skip = "Shopify won't let us create more than one random carrier.")]
        public void Updates_Carriers()
        {
            string newCallbackUrl = "http://fakecallback2.com/";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.CallbackUrl = newCallbackUrl;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);
            Fixture.Service.Delete(updated.Id.Value);

            Assert.Equal(newCallbackUrl, updated.CallbackUrl);
        }
    }

    public class Carrier_Tests_Fixture : IDisposable
    {
        public CarrierService Service { get; private set; }
        public List<Carrier> Created { get; private set; }

        public string CallbackUrl { get { return "http://fakecallback.com/"; } }

        public Carrier_Tests_Fixture()
        {
            Service = new CarrierService(Utils.MyShopifyUrl, Utils.AccessToken);
            Created  = new List<Carrier>();
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
                        Console.WriteLine(string.Format("Failed to delete Carrier with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public Carrier Create()
        {
            string uid = Guid.NewGuid().ToString();
            string name = string.Format("DERP DERP {0}", uid);
            string cb = string.Format("{CallbackUrl}{0}", uid);

            var obj = Service.Create(new Carrier()
            {
                Name = name,
                Active = false,
                CallbackUrl = cb,
                CarrierServiceType = "api",
                ServiceDiscovery = true,
                Format = "json"
            });

            return obj;
        }
    }
}
