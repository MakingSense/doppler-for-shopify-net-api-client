using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Doppler.Shopify.ApiClient.Enums;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Authorization")]
    public class Authorization_Tests
    {
        //[Fact]
        //public void Validates_Proxy_Requests()
        //{
        //    //Configure querystring
        //    var qs = new Dictionary<string, StringValues>()
        //    {
        //        {"shop", "stages-test-shop-2.myshopify.com"},
        //        {"path_prefix", "/apps/stages-order-tracker"},
        //        {"timestamp", "1459781841"},
        //        {"signature", "239813a42e1164a9f52e85b2119b752774fafb26d0f730359c86572e1791854a"},
        //    };

        //    bool isValid = AuthorizationService.IsAuthenticProxyRequest(qs, Utils.SecretKey);

        //    Assert.True(isValid);
        //}

        //[Fact]
        //public void Validates_Proxy_Requests_With_Dictionary_QueryString()
        //{
        //    //Configure querystring
        //    var qs = new Dictionary<string, string>()
        //    {
        //        {"shop", "stages-test-shop-2.myshopify.com"},
        //        {"path_prefix", "/apps/stages-order-tracker"},
        //        {"timestamp", "1459781841"},
        //        {"signature", "239813a42e1164a9f52e85b2119b752774fafb26d0f730359c86572e1791854a"},
        //    };

        //    bool isValid = AuthorizationService.IsAuthenticProxyRequest(qs, Utils.SecretKey);

        //    Assert.True(isValid);
        //}

        //[Fact]
        //public void  Validates_Proxy_Requests_With_Raw_QueryString()
        //{
        //    //Configure querystring
        //    //var qs = "shop=stages-test-shop-2.myshopify.com&path_prefix=/apps/stages-order-tracker&timestamp=1459781841&signature=239813a42e1164a9f52e85b2119b752774fafb26d0f730359c86572e1791854a";
        //    var qs = "code=2ecee34d7bc831b2bff3805699b361a5&hmac=0fa29e9c6ec36985c6f0173cf3b66b89e12591d77e3f8657e69009f95ee0d34b&shop=doppler-dev01.myshopify.com&timestamp=1540769697";

        //    bool isValid = AuthorizationService.IsAuthenticProxyRequest(qs, Utils.SecretKey);

        //    Assert.True(isValid);
        //}

        //[Fact]
        //public void  Validates_Web_Requests()
        //{
        //    var qs = new Dictionary<string, StringValues>()
        //    {
        //        {"hmac", "134298b94779fc1be04851ed8f972c827d9a3b4fdf6725fe97369ef422cc5746"},
        //        {"shop", "stages-test-shop-2.myshopify.com"},
        //        {"signature", "f477a85f3ed6027735589159f9da74da"},
        //        {"timestamp", "1459779785"},
        //    };

        //    bool isValid = AuthorizationService.IsAuthenticRequest(qs, Utils.SecretKey);

        //    Assert.True(isValid);
        //}

        //[Fact]
        //public void Validates_Web_Requests_With_Dictionary_Querystring()
        //{
        //    // Note that this method differes from Validates_Web_Requests() in that the aforementioned's dictionary is Dictionary<string, stringvalues> and this is Dictionary<string, string>.
        //    var qs = new Dictionary<string, string>()
        //    {
        //        {"hmac", "134298b94779fc1be04851ed8f972c827d9a3b4fdf6725fe97369ef422cc5746"},
        //        {"shop", "stages-test-shop-2.myshopify.com"},
        //        {"signature", "f477a85f3ed6027735589159f9da74da"},
        //        {"timestamp", "1459779785"},
        //    };

        //    bool isValid = AuthorizationService.IsAuthenticRequest(qs, Utils.SecretKey);

        //    Assert.True(isValid);
        //}

        [Fact]
        public void  Validates_Web_Requests_With_Raw_Querystring()
        {
            var qs = "code=cda84da98ee07b1231068a6ba51f7101&hmac=1f2b427c24b6a0e2a9004a71ea16d3d5a213db5058218e57e9d5fb44d601b903&shop=doppler-dev01.myshopify.com&timestamp=1540772285";

            bool isValid = AuthorizationService.IsAuthenticRequest(qs, "3467219a2f96ab7d2e95b8b9b3cd0514");

            Assert.True(isValid);
        }

        [Fact]
        public void Validates_Shop_Urls()
        {
            string validUrl = Utils.MyShopifyUrl;
            string invalidUrl = "https://google.com";

            Assert.True(AuthorizationService.IsValidShopDomain(validUrl));
            Assert.False(AuthorizationService.IsValidShopDomain(invalidUrl));
        }

        [Fact]
        public void Validates_Shop_Malfunctioned_Urls()
        {
            string invalidUrl = "foo";
            
            Assert.False(AuthorizationService.IsValidShopDomain(invalidUrl));
        }

        [Fact]
        public void  Builds_Authorization_Urls_With_Enums()
        {
            var scopes = new List<AuthorizationScope>()
            {
                AuthorizationScope.ReadCustomers,
                AuthorizationScope.WriteCustomers
            };
            string redirectUrl = "http://example.com";
            string result = AuthorizationService.BuildAuthorizationUrl(scopes, Utils.MyShopifyUrl, Utils.ApiKey, redirectUrl).ToString();

            Assert.Contains("/admin/oauth/authorize?", result);
            Assert.Contains(string.Format("client_id={0}", Utils.ApiKey), result);
            Assert.Contains("scope=read_customers,write_customers", result);
            Assert.Contains(string.Format("redirect_uri={0}", redirectUrl), result);
        }

        [Fact]
        public void  Builds_Authorization_Urls_With_Strings()
        {
            string[] scopes = { "read_customers", "write_customers" };
            string redirectUrl = "http://example.com";
            string result = AuthorizationService.BuildAuthorizationUrl(scopes, Utils.MyShopifyUrl, Utils.ApiKey, redirectUrl).ToString();

            Assert.Contains("/admin/oauth/authorize?", result);
            Assert.Contains(string.Format("client_id={0}", Utils.ApiKey), result);
            Assert.Contains("scope=read_customers,write_customers", result);
            Assert.Contains(string.Format("redirect_uri={0}", redirectUrl), result);
        }

        [Fact]
        public void  Builds_Authorization_Urls_With_Grants_And_State()
        {
            string[] scopes = { "read_customers", "write_customers" };
            string redirectUrl = "http://example.com";
            string[] grants = { "per-user" };
            string state = Guid.NewGuid().ToString();
            string result = AuthorizationService.BuildAuthorizationUrl(scopes, Utils.MyShopifyUrl, Utils.ApiKey, redirectUrl, state, grants).ToString();

            Assert.Contains("/admin/oauth/authorize?", result);
            Assert.Contains(string.Format("client_id={0}", Utils.ApiKey), result);
            Assert.Contains("scope=read_customers,write_customers", result);
            Assert.Contains(string.Format("redirect_uri={0}", redirectUrl), result);
            Assert.Contains(string.Format("state={0}", state), result);
            Assert.Contains("grant_options[]=per-user", result);
        }
    }
}