using System.Linq;
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
    }

    public class Product_Tests_Fixture
    {
        public ProductService Service { get; private set; } = new ProductService(Utils.MyShopifyUrl, Utils.AccessToken);
    }
}
