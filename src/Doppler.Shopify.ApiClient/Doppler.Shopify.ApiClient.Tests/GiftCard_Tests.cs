using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopifySharp.Filters;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "GiftCard")]
    public class GiftCard_Tests : IClassFixture<GiftCard_Tests_Fixture>
    {
        public static decimal GiftCardValue = 100;
        private GiftCard_Tests_Fixture Fixture { get; private set; }

        public GiftCard_Tests(GiftCard_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Counts_GiftCards()
        {
            var count = Fixture.Service.Count();
            Assert.True(count > 0);
        }

        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Counts_GiftCards_With_A_Filter()
        {
            var enabledCount = Fixture.Service.Count("enabled");
            Assert.True(enabledCount > 0);
        }

        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Lists_GiftCards()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Any());
        }

        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Lists_GiftCards_With_A_Filter()
        {
            var list = Fixture.Service.List(new GiftCardFilter()
            {
                Status = "enabled"
            });

            Assert.True(list.Any());
        }

        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Gets_GiftCards()
        {
            // Find an id 
            var created = Fixture.Created.First();
            var giftCard = Fixture.Service.Get(created.Id.Value);

            Assert.NotNull(giftCard);
            Assert.Equal(GiftCardValue, giftCard.InitialValue);
        }

        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Creates_GiftCards()
        {
            var created = Fixture.Create(GiftCardValue);

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
        }

        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Creates_GiftCards_With_Code()
        {
            var customCode = Guid.NewGuid().ToString();
            var lastFour = customCode.Substring(customCode.Length - 4);
            var created = Fixture.Create(GiftCardValue, customCode);

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal(lastFour, created.LastCharacters);
        }


        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Updates_GiftCards()
        {
            string note = "Updates_GiftCards note";
            var created = Fixture.Create(GiftCardValue);
            long id = created.Id.Value;

            created.Note = note;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(note, updated.Note);
        }


        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Disable_GiftCards()
        {
            var created = Fixture.Create(GiftCardValue);
            var disabled = Fixture.Service.Disable(created.Id.Value);

            Assert.True(disabled.DisabledAt.HasValue);
        }

        [Fact(Skip = "Cannot run without a Shopify Plus account.")]
        public void Searches_For_GiftCards()
        {

            var customCode = Guid.NewGuid().ToString();
            customCode = customCode.Substring(customCode.Length - 20);
            Fixture.Create(GiftCardValue, customCode);
            var query = "code:" + customCode;
            var search = Fixture.Service.Search(query);

            Assert.True(search.Any());
        }
    }

    public class GiftCard_Tests_Fixture : IAsyncLifetime
    {
        public GiftCardService Service => new GiftCardService(Utils.MyShopifyUrl, Utils.AccessToken);


        public List<GiftCard> Created { get; private set; } = new List<GiftCard>();

        public void Initialize()
        {
            // Create an giftCard.
            var giftCard = Create(GiftCard_Tests.GiftCardValue);
        }

        public void Dispose()
        {
            foreach (var obj in Created)
            {
                try
                {
                    Service.Disable(obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    Console.WriteLine(string.Format("Failed to delete gift card with id {obj.Id.Value}. {ex.Message}");
                }
            }
        }
        public async Task<GiftCard> Create(decimal value, string code = null)
        {
            var giftCardRequest = new GiftCard() { InitialValue = value };
            if (!string.IsNullOrEmpty(code))
            {
                giftCardRequest.Code = code;
            }
            if (giftCardRequest.Code != null && giftCardRequest.Code.Length > 20)
            {
                giftCardRequest.Code = giftCardRequest.Code.Substring(giftCardRequest.Code.Length - 20);
            }
            var giftCard = Service.Create(giftCardRequest);

            Created.Add(giftCard);

            return giftCard;
        }
    }
}
