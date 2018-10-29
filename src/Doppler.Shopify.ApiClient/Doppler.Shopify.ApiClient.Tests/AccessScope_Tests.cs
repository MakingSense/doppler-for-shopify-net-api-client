using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Access scope")]
    public class AccessScope_Tests : IClassFixture<AccessScope_Tests_Fixture>
    {
        private AccessScope_Tests_Fixture Fixture { get; set; }

        public AccessScope_Tests(AccessScope_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void List()
        {
            var scopes = Fixture.Service.List();
            Assert.True(scopes.Count() > 0);
        }
    }

    public class AccessScope_Tests_Fixture
    {
        public AccessScopeService Service { get; private set; }
        
        public AccessScope_Tests_Fixture()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            Service = new AccessScopeService(Utils.MyShopifyUrl, Utils.AccessToken);
        }
    }
}
