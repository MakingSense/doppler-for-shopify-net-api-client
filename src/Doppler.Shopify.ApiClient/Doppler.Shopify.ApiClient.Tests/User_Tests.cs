using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "User")]
    public class User_Tests : IClassFixture<User_Tests_Fixture>
    {
        private User_Tests_Fixture Fixture { get; set; }

        public User_Tests(User_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }


        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Lists_Users()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Any());
        }


        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Gets_Users()
        {
            // Find an id 
            var list = Fixture.Service.List();
            var user = Fixture.Service.Get(list.First().Id.Value);

            Assert.NotNull(user);
            Assert.Equal(user.Id, list.First().Id);
        }
    }

    public class User_Tests_Fixture
    {
        public UserService Service
        {
            get
            {
                return new UserService(Utils.MyShopifyUrl, Utils.AccessToken);
            }
        }
    }
}
