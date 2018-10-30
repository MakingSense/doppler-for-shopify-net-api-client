using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "ShippingZone")]
    public class ShippingZone_Tests
    {
        private ShippingZoneService _service = new ShippingZoneService(Utils.MyShopifyUrl, Utils.AccessToken);

        private ShippingZoneService _Service { get { return _service; } set { _service = value; } }

        [Fact]
        public void Lists_ShippingZones()
        {
            var shippingZones = _Service.List();

            Assert.NotNull(shippingZones);

            foreach (var shippingZone in shippingZones)
            {
                Assert.NotNull(shippingZone.Name);
            }
        }
    }
}