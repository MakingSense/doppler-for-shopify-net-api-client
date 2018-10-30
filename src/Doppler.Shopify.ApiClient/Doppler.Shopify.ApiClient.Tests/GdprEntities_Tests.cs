using System;
using System.Linq;
using Newtonsoft.Json;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    /// <remarks>
    /// I've added tests for these entitites because there's no easy way to test the actual webhooks, and Shopify's documentation is unclear about whether it's sending number ids or string ids through these webhooks.
    /// </remarks>
    [Trait("Category", "GDPR")]
    public class GdprEntities_Tests
    {
        [Fact]
        public void  Deserializes_Shop_Redacted_With_String_Id()
        {
            Int64 shopId = 654321;
            var shopDomain = "example.myshopify.com";
            string json = string.Format("{{'shop_id':'{0}','shop_domain':'{1}'}}", shopId, shopDomain);

            var deserialized = JsonConvert.DeserializeObject<ShopRedactedWebhook>(json);

            Assert.Equal(shopDomain, deserialized.ShopDomain);
            Assert.Equal(shopId, deserialized.ShopId);
        }

        [Fact]
        public void  Deserializes_Shop_Redacted_With_Int64_Id()
        {
            Int64 shopId = 654321;
            var shopDomain = "example.myshopify.com";
            string json = string.Format("{{'shop_id':'{0}','shop_domain':'{1}'}}", shopId, shopDomain);

            var deserialized = JsonConvert.DeserializeObject<ShopRedactedWebhook>(json);

            Assert.Equal(shopDomain, deserialized.ShopDomain);
            Assert.Equal(shopId, deserialized.ShopId);
        }

        [Fact]
        public void  Deserializes_Customer_Redacted_With_String_Id()
        {
            Int64 customerId = 123456;
            var email = "hello@example.com";
            var phone = "555-555-5555";

            var json = string.Format(@"{{
                'shop_id': 654321,
                'shop_domain': 'example.myshopify.com',
                'customer': {{
                    'id': '{0}',
                    'email': '{1}',
                    'phone': '{2}'
                }},
                'orders_to_redact': ['987654321']}}", customerId, email, phone);
            var deserialized = JsonConvert.DeserializeObject<CustomerRedactedWebhook>(json);

            Assert.NotNull(deserialized.Customer);
            Assert.True(deserialized.Customer.Id.HasValue);
            Assert.Equal(customerId, deserialized.Customer.Id);
            Assert.Equal(email, deserialized.Customer.Email);
            Assert.Equal(phone, deserialized.Customer.Phone);
        }

        [Fact]
        public void  Deserializes_Customer_Redacted_With_Int64_Id()
        {
            Int64 customerId = 123456;
            var email = "hello@example.com";
            var phone = "555-555-5555";

            var json = string.Format(@"{{
                'shop_id': 654321,
                'shop_domain': 'example.myshopify.com',
                'customer': {{
                    'id': {0},
                    'email': '{1}',
                    'phone': '{2}'
                }},
                'orders_to_redact': ['987654321']}}", customerId, email, phone);
            var deserialized = JsonConvert.DeserializeObject<CustomerRedactedWebhook>(json);

            Assert.NotNull(deserialized.Customer);
            Assert.True(deserialized.Customer.Id.HasValue);
            Assert.Equal(customerId, deserialized.Customer.Id);
            Assert.Equal(email, deserialized.Customer.Email);
            Assert.Equal(phone, deserialized.Customer.Phone);
        }

        [Fact]
        public void  Deserializes_Customer_Redacted_With_No_Id()
        {
            var email = "hello@example.com";

            var json = string.Format(@"{{
                'shop_id': 654321,
                'shop_domain': 'example.myshopify.com',
                'customer': {{
                    'email': '{0}',
                }},
                'orders_to_redact': ['987654321']}}", email);
            var deserialized = JsonConvert.DeserializeObject<CustomerRedactedWebhook>(json);

            Assert.NotNull(deserialized.Customer);
            Assert.False(deserialized.Customer.Id.HasValue);
            Assert.Equal(email, deserialized.Customer.Email);
        }

        [Fact]
        public void  Deserializes_Customer_Redacted_With_String_Order_Ids()
        {
            Int64 orderId = 987654321;

            var json = string.Format(@"{{
                'shop_id': 654321,
                'shop_domain': 'example.myshopify.com',
                'customer': {{
                    'id': 123456,
                    'email': 'hello@example.com',
                    'phone': '555-555-5555'
                }},
                'orders_to_redact': ['{0}']}}", orderId);
            var deserialized = JsonConvert.DeserializeObject<CustomerRedactedWebhook>(json);

            Assert.NotNull(deserialized.OrdersToRedact);
            Assert.True(deserialized.OrdersToRedact.Any());
            Assert.Equal(1, deserialized.OrdersToRedact.Count());
            Assert.All(deserialized.OrdersToRedact, o =>
            {
                Assert.Equal(o, orderId);
            });
        }

        [Fact]
        public void  Deserializes_Customer_Redacted_With_Int64_Order_Ids()
        {
            Int64 orderId = 987654321;

            var json = string.Format(@"{{
                'shop_id': 654321,
                'shop_domain': 'example.myshopify.com',
                'customer': {{
                    'id': 123456,
                    'email': 'hello@example.com',
                    'phone': '555-555-5555'
                }},
                'orders_to_redact': [{0}]}}", orderId);
            var deserialized = JsonConvert.DeserializeObject<CustomerRedactedWebhook>(json);

            Assert.NotNull(deserialized.OrdersToRedact);
            Assert.True(deserialized.OrdersToRedact.Any());
            Assert.Equal(1, deserialized.OrdersToRedact.Count());
            Assert.All(deserialized.OrdersToRedact, o =>
            {
                Assert.Equal(o, orderId);
            });
        }

        [Fact]
        public void  Deserializes_Customer_Data_Request_With_String_Id()
        {
            Int64 customerId = 123456;
            var email = "hello@example.com";
            var phone = "555-555-5555";

            var json = string.Format(@"{{
                'shop_id': 654321,
                'shop_domain': 'example.myshopify.com',
                'customer': {{
                    'id': '{0}',
                    'email': '{1}',
                    'phone': '{2}'
                }},
                'orders_requested': ['987654321']}}", customerId, email, phone);
            var deserialized = JsonConvert.DeserializeObject<CustomerDataRequestWebhook>(json);

            Assert.NotNull(deserialized.Customer);
            Assert.True(deserialized.Customer.Id.HasValue);
            Assert.Equal(customerId, deserialized.Customer.Id);
            Assert.Equal(email, deserialized.Customer.Email);
            Assert.Equal(phone, deserialized.Customer.Phone);
        }

        [Fact]
        public void  Deserializes_Customer_Data_Request_With_Int64_Id()
        {
            Int64 customerId = 123456;
            var email = "hello@example.com";
            var phone = "555-555-5555";

            var json = string.Format(@"{{
                'shop_id': 654321,
                'shop_domain': 'example.myshopify.com',
                'customer': {{
                    'id': {0},
                    'email': '{1}',
                    'phone': '{2}'
                }},
                'orders_requested': ['987654321']}}", customerId, email, phone);
            var deserialized = JsonConvert.DeserializeObject<CustomerDataRequestWebhook>(json);

            Assert.NotNull(deserialized.Customer);
            Assert.True(deserialized.Customer.Id.HasValue);
            Assert.Equal(customerId, deserialized.Customer.Id);
            Assert.Equal(email, deserialized.Customer.Email);
            Assert.Equal(phone, deserialized.Customer.Phone);
        }

        [Fact]
        public void  Deserializes_Customer_Data_Request_With_No_Id()
        {
            var email = "hello@example.com";

            var json = string.Format(@"{{
                'shop_id': 654321,
                'shop_domain': 'example.myshopify.com',
                'customer': {{
                    'email': '{0}',
                }},
                'orders_requested': ['987654321']}}", email);
            var deserialized = JsonConvert.DeserializeObject<CustomerDataRequestWebhook>(json);

            Assert.NotNull(deserialized.Customer);
            Assert.False(deserialized.Customer.Id.HasValue);
            Assert.Equal(email, deserialized.Customer.Email);
        }

        [Fact]
        public void  Deserializes_Customer_Data_Request_With_String_Order_Ids()
        {
            Int64 orderId = 987654321;

            var json = string.Format(@"{{
                'shop_id': 654321,
                'shop_domain': 'example.myshopify.com',
                'customer': {{
                    'id': 123456,
                    'email': 'hello@example.com',
                    'phone': '555-555-5555'
                }},
                'orders_requested': ['{0}']}}", orderId);
            var deserialized = JsonConvert.DeserializeObject<CustomerDataRequestWebhook>(json);

            Assert.NotNull(deserialized.OrdersRequested);
            Assert.True(deserialized.OrdersRequested.Any());
            Assert.Equal(1, deserialized.OrdersRequested.Count());
            Assert.All(deserialized.OrdersRequested, o =>
            {
                Assert.Equal(o, orderId);
            });
        }

        [Fact]
        public void  Deserializes_Customer_Data_Request_With_Int64_Order_Ids()
        {
            Int64 orderId = 987654321;

            var json = string.Format(@"{{
                'shop_id': 654321,
                'shop_domain': 'example.myshopify.com',
                'customer': {{
                    'id': 123456,
                    'email': 'hello@example.com',
                    'phone': '555-555-5555'
                }},
                'orders_requested': [{0}]}}", orderId);
            var deserialized = JsonConvert.DeserializeObject<CustomerDataRequestWebhook>(json);

            Assert.NotNull(deserialized.OrdersRequested);
            Assert.True(deserialized.OrdersRequested.Any());
            Assert.Equal(1, deserialized.OrdersRequested.Count());
            Assert.All(deserialized.OrdersRequested, o =>
            {
                Assert.Equal(o, orderId);
            });
        }
    }
}