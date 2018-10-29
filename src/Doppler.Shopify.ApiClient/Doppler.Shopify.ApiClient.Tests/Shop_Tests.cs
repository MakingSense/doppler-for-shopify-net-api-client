using System;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Shop")]
    public class Shop_Tests
    {
        private ShopService _Service { get; private set; } = new ShopService(Utils.MyShopifyUrl, Utils.AccessToken);

        [Fact]
        public void Gets_Shops()
        {
            var shop = _Service.Get();

            Assert.NotNull(shop);
            EmptyAssert.NotNullOrEmpty(shop.Name);
            EmptyAssert.NotNullOrEmpty(shop.PlanDisplayName);
            EmptyAssert.NotNullOrEmpty(shop.MyShopifyDomain);
        }

        [Fact(Skip = "Private applications cannot be uninstalled.")]
        public void Uninstalls_Apps()
        {
            bool threw = false;

            try
            {
                _Service.UninstallApp();
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Uninstalls_Apps)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }
    }
}
