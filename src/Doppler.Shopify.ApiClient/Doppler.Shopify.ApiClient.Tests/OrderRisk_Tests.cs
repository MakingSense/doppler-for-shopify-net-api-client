using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "OrderRisk")]
    public class OrderRisk_Tests : IClassFixture<OrderRisk_Tests_Fixture>
    {
        private OrderRisk_Tests_Fixture Fixture { get; set; }

        public OrderRisk_Tests(OrderRisk_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Lists_Risks()
        {
            var list = Fixture.Service.List(Fixture.OrderId);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_Risks()
        {
            var created = Fixture.Create(Fixture.OrderId, true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(Fixture.OrderId, created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_Risks failed. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_Risks()
        {
            var created = Fixture.Create(Fixture.OrderId);
            var risk = Fixture.Service.Get(created.OrderId.Value, created.Id.Value);

            Assert.NotNull(risk);
            Assert.Equal(Fixture.OrderId, risk.OrderId);
            Assert.Equal(Fixture.Message, risk.Message);
            Assert.Equal(Fixture.Score, risk.Score);
            Assert.Equal(Fixture.Recommendation, risk.Recommendation);
            Assert.Equal(Fixture.Source, risk.Source);
            Assert.Equal(Fixture.CauseCancel, risk.CauseCancel);
            Assert.Equal(Fixture.Display, risk.Display);
        }

        [Fact]
        public void Creates_Risks()
        {
            var created = Fixture.Create(Fixture.OrderId);

            Assert.NotNull(created);
            Assert.Equal(Fixture.OrderId, created.OrderId);
            Assert.Equal(Fixture.Message, created.Message);
            Assert.Equal(Fixture.Score, created.Score);
            Assert.Equal(Fixture.Recommendation, created.Recommendation);
            Assert.Equal(Fixture.Source, created.Source);
            Assert.Equal(Fixture.CauseCancel, created.CauseCancel);
            Assert.Equal(Fixture.Display, created.Display);
        }

        [Fact]
        public void Updates_Risks()
        {
            string message = "An updated risk message.";
            var created = Fixture.Create(Fixture.OrderId);
            long id = created.Id.Value;

            created.Message = message;
            created.Id = null;

            var updated = Fixture.Service.Update(Fixture.OrderId, id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(message, updated.Message);
        }
    }

    public class OrderRisk_Tests_Fixture : IDisposable
    {
        private OrderRiskService _service = new OrderRiskService(Utils.MyShopifyUrl, Utils.AccessToken);
        private List<OrderRisk> _created = new List<OrderRisk>();

        public OrderRiskService Service { get { return _service; } private set { _service = value; } }
        public List<OrderRisk> Created { get { return _created; } private set { _created = value; } }
        public string Message
        {
            get
            {
                return "This looks risky!";
            }
        }

        public decimal Score
        {
            get
            {
                return (decimal)0.85;
            }
        }

        public string Recommendation
        {
            get
            {
                return "cancel";
            }
        }

        public string Source
        {
            get
            {
                return "External";
            }
        }

        public bool CauseCancel
        {
            get
            {
                return false;
            }
        }

        public bool Display
        {
            get
            {
                return true;
            }
        }

        public long OrderId { get; set; }

        public OrderRisk_Tests_Fixture()
        {
            Initialize();
        }

        public void Initialize()
        {
            OrderId = (new OrderService(Utils.MyShopifyUrl, Utils.AccessToken).List(new OrderFilter()
            {
                Limit = 1
            })).First().Id.Value;

            // Create a risk for count, list, get, etc. tests.
            Create(OrderId);
        }

        public void Dispose()
        {
            foreach (var obj in Created)
            {
                try
                {
                    Service.Delete(OrderId, obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete created OrderRisk with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public OrderRisk Create(long orderId, bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(orderId, new OrderRisk()
            {
                Message = Message,
                Score = Score,
                Recommendation = Recommendation,
                Source = Source,
                CauseCancel = CauseCancel,
                Display = Display,
            });

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
