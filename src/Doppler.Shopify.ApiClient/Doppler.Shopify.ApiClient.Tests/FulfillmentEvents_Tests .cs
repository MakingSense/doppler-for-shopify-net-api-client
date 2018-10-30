using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "FulfillmentEvent")]
    public class FulfillmentEvents_Tests : IClassFixture<FulfillmentEvents_Tests_Fixture>
    {
        private FulfillmentEvents_Tests_Fixture Fixture { get; set; }

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

    public class FulfillmentEvents_Tests_Fixture : IDisposable
    {
        private FulfillmentEventService _fulfillmentEventService = new FulfillmentEventService(Utils.MyShopifyUrl, Utils.AccessToken);
        private FulfillmentService _fulfillmentService = new FulfillmentService(Utils.MyShopifyUrl, Utils.AccessToken);
        private OrderService _orderService = new OrderService(Utils.MyShopifyUrl, Utils.AccessToken);
        private List<Order> _createdOrders = new List<Order>();
        private List<Fulfillment> _createdFulfillments = new List<Fulfillment>();
        private List<FulfillmentEvent> _createdFulfillmentEvents = new List<FulfillmentEvent>();

        public FulfillmentEventService FulfillmentEventService { get { return _fulfillmentEventService; } private set { _fulfillmentEventService = value; } }
        public FulfillmentService FulfillmentService { get { return _fulfillmentService; } private set { _fulfillmentService = value; } }
        public OrderService OrderService { get { return _orderService; } private set { _orderService = value; } }
        /// <summary>
        /// Fulfillments must be part of an order and cannot be deleted.
        /// </summary>
        public List<Order> CreatedOrders { get { return _createdOrders; } private set { _createdOrders = value; } }
        public List<Fulfillment> CreatedFulfillments { get { return _createdFulfillments; } private set { _createdFulfillments = value; } }
        public List<FulfillmentEvent> CreatedFulfillmentEvents { get { return _createdFulfillmentEvents; } private set { _createdFulfillmentEvents = value; } }

        public FulfillmentEvents_Tests_Fixture()
        {
            Initialize();
        }

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
                    Console.WriteLine(string.Format("Failed to delete order with id {0}. {1}", obj.Id.Value, ex.Message));
                }
            }
        }

        public Order CreateOrder()
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

        public Fulfillment CreateFulfillment(long orderId, bool multipleTrackingNumbers = false, IEnumerable<LineItem> items = null)
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

        public FulfillmentEvent CreateFulfillmentEvent(long orderId, long fulfillmentId)
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
