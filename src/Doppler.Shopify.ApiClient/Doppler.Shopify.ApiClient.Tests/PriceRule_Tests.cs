using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "PriceRule")]
    public class PriceRule_Tests : IClassFixture<PriceRule_Tests_Fixture>
    {
        private PriceRule_Tests_Fixture Fixture { get; private set; }

        public PriceRule_Tests(PriceRule_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Lists_PriceRules()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_PriceRules()
        {
            string suffix = Guid.NewGuid().ToString();
            var created = Fixture.Create(suffix, true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Deletes_PriceRules)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_PriceRules()
        {
            string suffix = Guid.NewGuid().ToString();
            var created = Fixture.Create(suffix);
            var rule = Fixture.Service.Get(created.Id.Value);

            Assert.NotNull(rule);
            Assert.Equal(Fixture.ValueType, rule.ValueType);
            Assert.Equal(Fixture.TargetType, rule.TargetType);
            Assert.Equal(Fixture.TargetSelection, rule.TargetSelection);
            Assert.Equal(Fixture.AllocationMethod, rule.AllocationMethod);
            Assert.Equal(Fixture.Value, rule.Value);
        }

        [Fact]
        public void Creates_PriceRules()
        {
            string suffix = Guid.NewGuid().ToString();
            var created = Fixture.Create(suffix);

            Assert.NotNull(created);
            Assert.StartsWith(Fixture.TitlePrefix, created.Title);
            Assert.Equal(Fixture.ValueType, created.ValueType);
            Assert.Equal(Fixture.TargetType, created.TargetType);
            Assert.Equal(Fixture.TargetSelection, created.TargetSelection);
            Assert.Equal(Fixture.AllocationMethod, created.AllocationMethod);
            Assert.Equal(Fixture.Value, created.Value);
        }

        [Fact]
        public void Updates_PriceRules()
        {
            string suffix = Guid.NewGuid().ToString();
            var created = Fixture.Create(suffix);
            long id = created.Id.Value;

            created.Value = -5.0m;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(created.Value, updated.Value);
        }
    }

    public class PriceRule_Tests_Fixture : IAsyncLifetime
    {
        public PriceRuleService Service { get; private set; } = new PriceRuleService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<PriceRule> Created { get; private set; } = new List<PriceRule>();

        public string TitlePrefix => "ShopifySharp PriceRule ";

        public string ValueType => "percentage";

        public string TargetType => "line_item";

        public string TargetSelection => "all";

        public string AllocationMethod => "across";

        public decimal Value => -10.0m;

        public string CustomerSelection => "all";

        public bool OncePerCustomer => false;


        public void Initialize()
        {
            // Create one for count, list, get, etc. orders.
            Create(Guid.NewGuid().ToString());
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
                        Console.WriteLine(string.Format("Failed to delete created Page with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<PriceRule> Create(string titleGuid, bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(new ShopifySharp.PriceRule()
            {
                Title = this.TitlePrefix + titleGuid,
                ValueType = this.ValueType,
                TargetType = this.TargetType,
                TargetSelection = this.TargetSelection,
                AllocationMethod = this.AllocationMethod,
                Value = this.Value,
                CustomerSelection = this.CustomerSelection,
                OncePerCustomer = this.OncePerCustomer,
                PrerequisiteSubtotalRange = new PrerequisiteValueRange()
                {
                    GreaterThanOrEqualTo = 40
                },
                StartsAt = new DateTimeOffset(DateTime.Now)
            });

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
