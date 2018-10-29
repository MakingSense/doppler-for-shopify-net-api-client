using System.Linq;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Policy")]
    public class Policy_Tests
    {
        private PolicyService _Service { get; private set; } = new PolicyService(Utils.MyShopifyUrl, Utils.AccessToken);              

        [Fact]
        public void Lists_Orders()
        {
            var list = _Service.List();

            Assert.NotNull(list);
            
            foreach(var policy in list)
            {
                EmptyAssert.NotNullOrEmpty(policy.Title);
                Assert.NotNull(policy.CreatedAt);
                Assert.NotNull(policy.UpdatedAt);
            }
        }
    }
}
