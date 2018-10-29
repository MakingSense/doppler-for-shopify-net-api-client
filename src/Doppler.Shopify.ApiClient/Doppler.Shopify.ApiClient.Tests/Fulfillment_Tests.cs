using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopifySharp.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Fulfillment")]
    public class Fulfillment_Tests : IClassFixture<Fulfillment_Tests_Fixture>
    {
        private Fulfillment_Tests_Fixture Fixture { get; private set; }

        public Fulfillment_Tests(Fulfillment_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Fulfillments()
        {
            long orderId = Fixture.Created.First().OrderId.Value;
            var count = Fixture.Service.Count(orderId);

            Assert.True(count > 0);
        }

        [Fact]
        public void Counts_Fulfillments_With_A_Filter()
        {
            long orderId = Fixture.Created.First().OrderId.Value;
            var fromDate = DateTime.UtcNow.AddDays(-2);
            var count = Fixture.Service.Count(orderId, new CountFilter()
            {
                CreatedAtMin = fromDate
            });

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Fulfillments()
        {
            long orderId = Fixture.Created.First().OrderId.Value;
            var list = Fixture.Service.List(orderId);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Lists_Fulfillments_With_A_Filter()
        {
            long orderId = Fixture.Created.First().OrderId.Value;
            var fromDate = DateTime.UtcNow.AddDays(-2);
            var list = Fixture.Service.List(orderId, new ListFilter()
            {
                CreatedAtMin = fromDate
            });

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Gets_Fulfillments()
        {
            // Find an id 
            var created = Fixture.Created.First();
            var fulfillment = Fixture.Service.Get(created.OrderId.Value, created.Id.Value);

            Assert.NotNull(fulfillment);
        }

        [Fact]
        public void Creates_Fulfillments()
        {
            var order = Fixture.CreateOrder();
            var created = Fixture.Create(order.Id.Value);

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal("success", created.Status);
        }

        [Fact]
        public void Creates_Fulfillments_With_Tracking_Numbers()
        {
            var order = Fixture.CreateOrder();
            var created = Fixture.Create(order.Id.Value, true);

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal("success", created.Status);
            Assert.True(created.TrackingNumbers.Count() > 1);
        }

        [Fact]
        public void Creates_Partial_Fulfillments()
        {
            var order = Fixture.CreateOrder();
            var lineItem = order.LineItems.First();

            // A partial fulfillment does not fulfill the entire line item quantity
            lineItem.Quantity -= 1;

            var created = Fixture.Create(order.Id.Value, false, new LineItem[] { lineItem });

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal("success", created.Status);
        }

        [Fact]
        public void Updates_Fulfillments()
        {
            string company = "Auntie Dot's Shipping Company";
            var order = Fixture.CreateOrder();
            var created = Fixture.Create(order.Id.Value, false);
            long id = created.Id.Value;

            created.TrackingCompany = company;
            created.Id = null;

            var updated = Fixture.Service.Update(created.OrderId.Value, id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(company, updated.TrackingCompany);
        }

        [Fact(Skip = "Can't complete/cancel/open a fulfillment whose status is not 'pending'. It's not clear how to create a fulfillment that's pending.")]
        public void Opens_Fulfillments()
        {
            var order = Fixture.CreateOrder();
            var created = Fixture.Create(order.Id.Value);
            var opened = Fixture.Service.Open(order.Id.Value, created.Id.Value);

            Assert.Equal("open", opened.Status);
        }

        [Fact(Skip = "Can't complete/cancel/open a fulfillment whose status is not 'pending'. It's not clear how to create a fulfillment that's pending.")]
        public void Cancels_Fulfillments()
        {
            var order = Fixture.CreateOrder();
            var created = Fixture.Create(order.Id.Value);
            var cancelled = Fixture.Service.Cancel(order.Id.Value, created.Id.Value);

            Assert.Equal("cancelled", cancelled.Status);
        }

        [Fact(Skip = "Can't complete/cancel/open a fulfillment whose status is not 'pending'. It's not clear how to create a fulfillment that's pending.")]
        public void Completes_Fulfillments()
        {
            var order = Fixture.CreateOrder();
            var created = Fixture.Create(order.Id.Value);
            var cancelled = Fixture.Service.Cancel(order.Id.Value, created.Id.Value);

            Assert.Equal("success", cancelled.Status);
        }
    }

    public class Fulfillment_Tests_Fixture : IAsyncLifetime
    {
        public FulfillmentService Service { get; private set; } = new FulfillmentService(Utils.MyShopifyUrl, Utils.AccessToken);

        public OrderService OrderService { get; private set; } = new OrderService(Utils.MyShopifyUrl, Utils.AccessToken);

        /// <summary>
        /// Fulfillments must be part of an order and cannot be deleted.
        /// </summary>
        public List<Order> CreatedOrders { get; private set; } = new List<Order>();

        public List<Fulfillment> Created { get; private set; } = new List<Fulfillment>();

        public void Initialize()
        {
            // Fulfillment API has a stricter rate limit when on a non-paid store.
            Service.SetExecutionPolicy(new SmartRetryExecutionPolicy());

            // Create an order and fulfillment for count, list, get, etc. tests.
            var order = CreateOrder();
            var fulfillment = Create(order.Id.Value);
        }

        public void Dispose()
        {
            foreach (var obj in Created)
            {
                try
                {
                    OrderService.Delete(obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    Console.WriteLine(string.Format("Failed to delete order with id {obj.Id.Value}. {ex.Message}");
                }
            }
        }

        public async Task<Order> CreateOrder()
        {
            var obj = OrderService.Create(new Order()
            {
                CreatedAt = DateTime.UtcNow,
                BillingAddress = new Address()
                {
                    Address1 = "123 4th Street",
                    City = "Minneapolis",
                    Province = "Minnesota",
                    ProvinceCode = "MN",
                    Zip = "55401",
                    Phone = "555-555-5555",
                    FirstName = "John",
                    LastName = "Doe",
                    Company = "Tomorrow Corporation",
                    Country = "United States",
                    CountryCode = "US",
                    Default = true,
                },
                LineItems = new List<LineItem>()
                {
                    new LineItem()
                    {
                        Name = "Test Line Item",
                        Title = "Test Line Item Title",
                        Quantity = 2,
                        Price = 5
                    },
                    new LineItem()
                    {
                        Name = "Test Line Item 2",
                        Title = "Test Line Item Title 2",
                        Quantity = 2,
                        Price = 5
                    }
                },
                FinancialStatus = "paid",
                TotalPrice = 5.00m,
                Email = Guid.NewGuid().ToString() + "@example.com",
                Note = "Test note about the customer.",
            }, new OrderCreateOptions()
            {
                SendReceipt = false,
                SendFulfillmentReceipt = false
            });

            CreatedOrders.Add(obj);

            return obj;
        }

        public async Task<Fulfillment> Create(long orderId, bool multipleTrackingNumbers = false, IEnumerable<LineItem> items = null)
        {
            Fulfillment fulfillment;

            if (multipleTrackingNumbers)
            {
                fulfillment = new Fulfillment()
                {
                    TrackingCompany = "Jack Black's Pack, Stack and Track",
                    TrackingUrls = new string[]
                    {
                        "https://example.com/da10038ee679f9afc93a785cafdd8d52",
                        "https://example.com/6349a40313ae3c7544331ff9fb44f28c",
                        "https://example.com/ca0b2d7bcccec4b58a94a24fa04101d3"
                    },
                    TrackingNumbers = new string[]
                    {
                        "da10038ee679f9afc93a785cafdd8d52",
                        "6349a40313ae3c7544331ff9fb44f28c",
                        "ca0b2d7bcccec4b58a94a24fa04101d3"
                    }
                };
            }
            else
            {
                fulfillment = new Fulfillment()
                {
                    TrackingCompany = "Jack Black's Pack, Stack and Track",
                    TrackingUrl = "https://example.com/123456789",
                    TrackingNumber = "123456789",
                };
            }

            if (items != null)
            {
                fulfillment.LineItems = items;
            }

            fulfillment = Service.Create(orderId, fulfillment, false);

            Created.Add(fulfillment);

            return fulfillment;
        }
    }
}
