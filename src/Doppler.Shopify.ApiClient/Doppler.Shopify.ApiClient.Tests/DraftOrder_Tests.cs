using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "DraftOrder")]
    public class DraftOrder_Tests : IClassFixture<DraftOrder_Tests_Fixture>
    {
        private DraftOrder_Tests_Fixture Fixture { get; private set; }

        public DraftOrder_Tests(DraftOrder_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_DraftOrders()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_DraftOrders()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_DraftOrders()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Deletes_DraftOrders)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_DraftOrders()
        {
            var created = Fixture.Create();
            created = Fixture.Service.Get(created.Id.Value);

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal(created.Note, Fixture.Note);
            Assert.False(string.IsNullOrEmpty(created.InvoiceUrl), "InvoiceUrl should not be null or empty.");

            foreach (var item in created.LineItems)
            {
                Assert.Equal(Fixture.LineItemTitle, item.Title);
                Assert.Equal(Fixture.LineItemQuantity, item.Quantity);
                Assert.Equal(Fixture.LineItemQuantity, item.Quantity);
            }
        }

        [Fact]
        public void Creates_DraftOrders()
        {
            var created = Fixture.Create();

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal(created.Note, Fixture.Note);
            Assert.False(string.IsNullOrEmpty(created.InvoiceUrl), "InvoiceUrl should not be null or empty.");

            foreach (var item in created.LineItems)
            {
                Assert.Equal(Fixture.LineItemTitle, item.Title);
                Assert.Equal(Fixture.LineItemQuantity, item.Quantity);
                Assert.Equal(Fixture.LineItemQuantity, item.Quantity);
            }
        }

        [Fact]
        public void Updates_DraftOrders()
        {
            string newNote = string.Format("New note value {Guid.NewGuid()}";
            var created = Fixture.Create();
            long id = created.Id.Value;
            
            created.Note = newNote;
            created.Id = null;
            
            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(newNote, updated.Note);   
        }

        [Fact(Skip = "Cannot test with my dev store since the trial has expired.")]
        public void Sends_Invoice()
        {
            var created = Fixture.Create();
            string to = "joshua@example.com";
            string subject = "Your draft order is ready";
            string message = "Pay pls";
            var result = Fixture.Service.SendInvoice(created.Id.Value, new DraftOrderInvoice()
            {
                To = to, 
                Subject = subject,
                CustomMessage = message,
            });

            Assert.False(String.IsNullOrEmpty(result.From), "`From` should not be null or empty");
            Assert.Equal(to, result.To);
            Assert.Equal(subject, result.Subject);
            Assert.Equal(message, result.CustomMessage);
        }

        [Fact]
        public void Completes_DraftOrder()
        {
            var created = Fixture.Create();
            created = Fixture.Service.Complete(created.Id.Value);

            Assert.NotNull(created.CompletedAt);
            Assert.Equal("completed", created.Status);
        }

        [Fact]
        public void Completes_DraftOrder_With_Pending_Payment()
        {
            var created = Fixture.Create();
            created = Fixture.Service.Complete(created.Id.Value, true);

            Assert.NotNull(created.CompletedAt);
            Assert.Equal("completed", created.Status);            
        }
    }

    public class DraftOrder_Tests_Fixture: IAsyncLifetime
    {
        public DraftOrderService Service => new DraftOrderService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<DraftOrder> Created { get; private set; } = new List<DraftOrder>();

        public string LineItemTitle = "Custom Draft Line Item";

        public decimal LineItemPrice = 15.00m;

        public int LineItemQuantity = 2;

        public string Note = "A note for the draft order.";

        public void Initialize()
        {
            // Create one for count, list, get, etc. tests.
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
                        Console.WriteLine(string.Format("Failed to delete created DraftOrder with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<DraftOrder> Create(bool skipAddToCreateList = false)
        {
            var obj = Service.Create(new DraftOrder()
            {
                LineItems = new List<DraftLineItem>()
                {
                    new DraftLineItem()
                    {
                        Title = LineItemTitle,
                        Price = LineItemPrice,
                        Quantity = LineItemQuantity,
                    }
                },
                Note = Note
            });

            if (! skipAddToCreateList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}