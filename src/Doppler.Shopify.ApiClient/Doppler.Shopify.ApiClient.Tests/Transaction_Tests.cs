using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Transaction")]
    public class Transaction_Tests : IClassFixture<Transaction_Tests_Fixture>
    {
        private Transaction_Tests_Fixture Fixture { get; set; }

        public Transaction_Tests(Transaction_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        public void Counts_Transactions()
        {
            var count = Fixture.Service.Count(Fixture.Created.First().OrderId.Value);

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Transactions()
        {
            var list = Fixture.Service.List(Fixture.Created.First().OrderId.Value);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Gets_Transactions()
        {
            var order = Fixture.CreateOrder();
            var created = Fixture.Create(order.Id.Value);
            var obj = Fixture.Service.Get(created.OrderId.Value, created.Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Null(obj.ErrorCode);
            Assert.Equal(Fixture.Amount, obj.Amount);
            Assert.Equal(Fixture.Currency, obj.Currency);
            Assert.Equal(Fixture.Status, obj.Status);
        }

        [Fact]
        public void Creates_Transactions()
        {
            var order = Fixture.CreateOrder();
            var obj = Fixture.Create(order.Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Null(obj.ErrorCode);
            Assert.Equal(Fixture.Amount, obj.Amount);
            Assert.Equal(Fixture.Currency, obj.Currency);
            Assert.Equal(Fixture.Status, obj.Status);
        }

        [Fact]
        public void Creates_Capture_Transactions()
        {
            string kind = "capture";
            var order = Fixture.CreateOrder();
            var obj = Fixture.Create(order.Id.Value, kind);

            Assert.Equal("success", obj.Status);
            Assert.Equal(kind, obj.Kind);
            Assert.Null(obj.ErrorCode);
        }

        [Fact(Skip = "This test returns the error 'Order cannot be refunded'. Orders that were created via API, not using a Shopify transaction gateway, cannot be refunded. Therefore, refunds are untestable.")]
        public void Creates_Refund_Transactions()
        {
            string kind = "refund";
            var order = Fixture.CreateOrder();
            var obj = Fixture.Create(order.Id.Value, kind);

            Assert.Equal("success", obj.Status);
            Assert.Equal(kind, obj.Kind);
            Assert.Null(obj.ErrorCode);
        }

        [Fact(Skip = "Transactions that aren't on store-credit or cash gateways require a parent_id.")]
        public void Creates_A_Void_Transaction()
        {
            string kind = "void";
            var order = Fixture.CreateOrder();
            var obj = Fixture.Create(order.Id.Value, kind);

            Assert.Equal("success", obj.Status);
            Assert.Equal(kind, obj.Kind);
            Assert.Null(obj.ErrorCode);
        }
    }

    public class Transaction_Tests_Fixture : IDisposable
    {
        private TransactionService _service = new TransactionService(Utils.MyShopifyUrl, Utils.AccessToken);
        private OrderService _orderService = new OrderService(Utils.MyShopifyUrl, Utils.AccessToken);
        private List<Transaction> _created = new List<Transaction>();
        private List<Order> _createdOrders = new List<Order>();

        public TransactionService Service { get { return _service; } private set { _service = value; } }
        public OrderService OrderService { get { return _orderService; } private set { _orderService = value; } }
        public List<Transaction> Created { get { return _created; } private set { _created = value; } }
        public List<Order> CreatedOrders { get { return _createdOrders; } private set { _createdOrders = value; } }
        public decimal Amount
        {
            get
            {
                return 10.00m;
            }
        }

        public string Currency
        {
            get
            {
                return "ARS";// TODO: It's hardcoded
            }
        }

        public string Gateway
        {
            get
            {
                return "bogus";
            }
        }

        public string Status
        {
            get
            {
                return "success";
            }
        }

        public long OrderId { get; set; }

        public Transaction_Tests_Fixture()
        {
            Initialize();
        }

        public void Initialize()
        {
            // Create one collection for use with count, list, get, etc. tests.
            var order = CreateOrder();
            Create(order.Id.Value);
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
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete created Order with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
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
                        Price = 50
                    },
                    new LineItem()
                    {
                        Name = "Test Line Item 2",
                        Title = "Test Line Item Title 2",
                        Quantity = 2,
                        Price = 50
                    }
                },
                FinancialStatus = "paid",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = 20.00m,
                        Status = "success",
                        Kind = "authorization",
                        Test = true,
                    }
                },
                Email = Guid.NewGuid().ToString() + "@example.com",
                Note = "Test note about the customer.",
            }, new OrderCreateOptions()
            {
                SendFulfillmentReceipt = false,
                SendReceipt = false
            });

            CreatedOrders.Add(obj);

            return obj;
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public Transaction Create(long orderId, string kind = "capture", bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(orderId, new Transaction()
            {
                Amount = Amount,
                Currency = Currency,
                Gateway = Gateway,
                Status = Status,
                Test = true,
                Kind = kind
            });

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}