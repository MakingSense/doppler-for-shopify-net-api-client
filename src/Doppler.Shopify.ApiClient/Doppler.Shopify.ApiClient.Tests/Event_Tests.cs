using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopifySharp.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Event")]
    public class Event_Tests : IClassFixture<Event_Tests_Fixture>
    {
        private Event_Tests_Fixture Fixture { get; private set; }

        public Event_Tests(Event_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Events()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Events()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Lists_Events_For_Subjects()
        {
            // Get an order id
            long orderId = (new OrderService(Utils.MyShopifyUrl, Utils.AccessToken).List(new OrderFilter()
            {
                Limit = 1
            })).First().Id.Value;
            string subject = "Order";
            var list = Fixture.Service.List(orderId, subject);

            Assert.NotNull(list);
            Assert.All(list, e => Assert.Equal(subject, e.SubjectType));
        }

        [Fact]
        public void Gets_Events()
        {
            var list = Fixture.Service.List(options: new EventListFilter()
            {
                Limit = 1
            });
            var evt = Fixture.Service.Get(list.First().Id.Value);

            Assert.NotNull(evt);
            Assert.NotNull(evt.Author);
            Assert.True(evt.CreatedAt.HasValue);
            Assert.NotNull(evt.Message);
            Assert.True(evt.SubjectId > 0);
            Assert.NotNull(evt.SubjectType);
            Assert.NotNull(evt.Verb);
        }
    }

    public class Event_Tests_Fixture
    {
        public EventService Service { get; private set; } = new EventService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<Event> Created { get; private set; } = new List<Event>();
    }
}
