using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopifySharp.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "FulfillmentEvent")]
    public class FulfillmentEvents_Tests : IClassFixture<FulfillmentEvents_Tests_Fixture>
    {
        private FulfillmentEvents_Tests_Fixture Fixture { get; private set; }

        public FulfillmentEvents_Tests(FulfillmentEvents_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Lists_FulfillmentEvents()
        {
            long orderId = Fixture.CreatedFulfillments.First().OrderId.Value;
            long fulfillmentId = Fixture.CreatedFulfillments.First().Id.Value;
            var list = Fixture.FulfillmentEventService.List(orderId, fulfillmentId);
            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void CreatesAndDeletes_FulfillmentEvents()
        {
            long orderId = Fixture.CreatedFulfillments.First().OrderId.Value;
            long fulfillmentId = Fixture.CreatedFulfillments.First().Id.Value;
            var @event = Fixture.CreateFulfillmentEvent(orderId, fulfillmentId);

            Assert.NotNull(@event);
            Assert.True(@event.Id.HasValue);
            Assert.Equal("confirmed", @event.Status);

            Fixture.FulfillmentEventService.Delete(orderId, fulfillmentId, @event.Id.Value);
        }
    }

    public class FulfillmentEvents_Tests_Fixture : IAsyncLifetime
    {
        public FulfillmentEventService FulfillmentEventService { get; private set; } = new FulfillmentEventService(Utils.MyShopifyUrl, Utils.AccessToken);

        public FulfillmentService FulfillmentService { get; private set; } = new FulfillmentService(Utils.MyShopifyUrl, Utils.AccessToken);

        public OrderService OrderService { get; private set; } = new OrderService(Utils.MyShopifyUrl, Utils.AccessToken);

        /// <summary>
        /// Fulfillments must be part of an order and cannot be deleted.
        /// </summary>
        public List<Order> CreatedOrders { get; private set; } = new List<Order>();

        public List<Fulfillment> CreatedFulfillments { get; private set; } = new List<Fulfillment>();

        public List<FulfillmentEvent> CreatedFulfillmentEvents { get; private set; } = new List<FulfillmentEvent>();

        public void Initialize()
        {
            // Fulfillment API has a stricter rate limit when on a non-paid store.
            FulfillmentService.SetExecutionPolicy(new SmartRetryExecutionPolicy());

            // Create an order and fulfillment for count, list, get, etc. tests.
            var order = CreateOrder();
            var fulfillment = CreateFulfillment(order.Id.Value);
            CreateFulfillmentEvent(order.Id.Value, fulfillment.Id.Value);
        }

        public void Dispose()
        {
            foreach (var obj in CreatedOrders)
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

        public async Task<Fulfillment> CreateFulfillment(long orderId, bool multipleTrackingNumbers = false, IEnumerable<LineItem> items = null)
        {
            Fulfillment fulfillment = new Fulfillment()
            {
                TrackingCompany = "Jack Black's Pack, Stack and Track",
                TrackingUrl = "https://example.com/123456789",
                TrackingNumber = "123456789",
                LineItems = CreatedOrders.First().LineItems
            };

            fulfillment = FulfillmentService.Create(orderId, fulfillment, false);

            CreatedFulfillments.Add(fulfillment);

            return fulfillment;
        }

        public async Task<FulfillmentEvent> CreateFulfillmentEvent(long orderId, long fulfillmentId)
        {
            var @event = new FulfillmentEvent()
            {
                OrderId = orderId,
                FulfillmentId = fulfillmentId,
                Status = "confirmed"
            };

            @event = FulfillmentEventService.Create(orderId, fulfillmentId, @event);

            CreatedFulfillmentEvents.Add(@event);

            return @event;
        }
    }
}
