using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Location")]
    public class Location_Tests
    {
        private LocationService _Service { get; private set; } = new LocationService(Utils.MyShopifyUrl, Utils.AccessToken);

        [Fact]
        public void Lists_Locations()
        {
            var list = _Service.List();

            Assert.NotNull(list);
        }

        [Fact]
        public void Gets_Locations()
        {
            var list = _Service.List();

            // Not all shops have a location.
            if (list.Count() > 0)
            {
                long id = list.First().Id.Value;
                var location = _Service.Get(id);

                Assert.NotNull(location.Address1);
                Assert.True(location.Id.HasValue);
                Assert.NotNull(location.City);
                Assert.NotNull(location.Province);
                Assert.NotNull(location.Zip);
                Assert.NotNull(location.Country);
            }
        }
    }
}
