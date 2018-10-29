using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ShopifySharp.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Order")]
    public class Order_Tests : IClassFixture<Order_Tests_Fixture>
    {
        private Order_Tests_Fixture Fixture { get; private set; }

        public Order_Tests(Order_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Orders()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Orders()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Lists_Orders_With_Filter()
        {
            var created = Task.WhenAll(Enumerable.Range(0, 2).Select(i => Fixture.Create()));
            var ids = created.Select(o => o.Id.Value);
            var list = Fixture.Service.List(new OrderFilter()
            {
                Ids = ids
            });

            Assert.All(list, o => Assert.Contains(o.Id.Value, ids));
        }

        [Fact]
        public void Deletes_Orders()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Deletes_Orders)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_Orders()
        {
            var order = Fixture.Service.Get(Fixture.Created.First().Id.Value);

            Assert.NotNull(order);
            Assert.Equal(Fixture.Note, order.Note);
            Assert.True(order.Id.HasValue);
        }

        [Fact]
        public void Creates_Orders()
        {
            var created = Fixture.Create();

            Assert.NotNull(created);
            Assert.Equal(Fixture.Note, created.Note);
            Assert.True(created.Id.HasValue);
        }

        [Fact]
        public void Updates_Orders()
        {
            string note = "This note was updated while testing ShopifySharp!";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Note = note;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(note, updated.Note);
        }

        [Fact]
        public void Opens_Orders()
        {
            // Close an order before opening it.
            var closed = Fixture.Service.Close(Fixture.Created.First().Id.Value);
            var opened = Fixture.Service.Open(closed.Id.Value);

            Assert.False(opened.ClosedAt.HasValue);
        }

        [Fact]
        public void Closes_Orders()
        {
            var closed = Fixture.Service.Close(Fixture.Created.Last().Id.Value);

            Assert.True(closed.ClosedAt.HasValue);
        }

        [Fact]
        public void Cancels_Orders()
        {
            var order = Fixture.Create();
            bool threw = false;

            try
            {
                Fixture.Service.Cancel(order.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Cancels_Orders)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Cancels_Orders_With_Options()
        {
            var order = Fixture.Create();
            bool threw = false;

            try
            {
                Fixture.Service.Cancel(order.Id.Value, new OrderCancelOptions()
                {
                    Reason = "customer"
                });
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Cancels_Orders_With_Options)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Can_Be_Partially_Updated()
        {
            string newNote = "These notes were part of a partial update to this order.";
            var created = Fixture.Create();
            var updated = Fixture.Service.Update(created.Id.Value, new Order()
            {
                Note = newNote
            });

            Assert.Equal(created.Id, updated.Id);
            Assert.Equal(newNote, updated.Note);

            // In previous versions of ShopifySharp, the updated JSON would have sent 'email=null', clearing out the email address.
            Assert.Equal(created.Email, updated.Email);
        }
    }

    public class Order_Tests_Fixture : IAsyncLifetime
    {
        public OrderService Service { get; private set; } = new OrderService(Utils.MyShopifyUrl, Utils.AccessToken);

        public string Note => "This order was created while testing ShopifySharp!";

        public List<Order> Created { get; private set; } = new List<Order>();

        public void Initialize()
        {
            // Create an order for count, list, get, etc. orders.
            Create();
        }

        public void Dispose()
        {
            foreach (var obj in Created)
            {
                try
                {
                    Service.Delete(obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete created Order with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<Order> Create(bool skipAddToCreateList = false)
        {
            var obj = Service.Create(new Order()
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
                Note = Note,
            });

            if (!skipAddToCreateList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
