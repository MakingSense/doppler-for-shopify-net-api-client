using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "ApplicationCredit")]
    public class ApplicationCredit_Tests
    {
        private ApplicationCreditService _Service { get; set; }

        public ApplicationCredit_Tests()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            _Service = new ApplicationCreditService(Utils.MyShopifyUrl, Utils.AccessToken);
        }


        [Fact(Skip = "Application Credits cannot be tested because they're unusable in a private application.")]
        public void Creates_An_Application_Credit()
        {
            var credit = _Service.Create(new ApplicationCredit()
            {
                Description = "Refund for Foo",
                Amount = 10.00m,
                Test = true,
            });

            Assert.NotNull(credit);
            Assert.True(credit.Id.HasValue);
            EmptyAssert.NotNullOrEmpty(credit.Description);
            Assert.True(credit.Test.Value);
            Assert.True(credit.Amount > 0);
        }
    }
}
