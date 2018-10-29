using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "DiscountCodes")]
    public class DiscountCode_Tests : IClassFixture<DiscountCodes_Tests_Fixture>
    {
        private DiscountCodes_Tests_Fixture Fixture { get; private set; }

        public DiscountCode_Tests(DiscountCodes_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Creates_DiscountCode()
        {
            var code = "UNITTEST2";
            var created = Fixture.Create(code);

            Assert.NotNull(created);
            Assert.Equal(code, created.Code);
            Assert.NotNull(created.UsageCount);
        }

        [Fact]
        public void Lists_DiscountCodes()
        {
            var list = Fixture.DiscountCodeService.List(Fixture.CreatedPriceRules.First().Id.Value);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Gets_DiscountCode()
        {
            var obj = Fixture.DiscountCodeService.Get(Fixture.CreatedPriceRules.First().Id.Value, Fixture.CreatedDiscountCodes.First().Id.Value);

            Assert.NotNull(obj);
            Assert.Equal(Fixture.Code, obj.Code);            
        }

        [Fact]
        public void Updates_DiscountCode()
        {
            var newCode = "UNITTEST-AFTER-UPDATE";
            var created = Fixture.Create("UNITTEST-BEFORE-UPDATE");
            created.Code = newCode;

            var updated = Fixture.DiscountCodeService.Update(created.PriceRuleId.Value, created);

            Assert.Equal(newCode, updated.Code);
        }
    }

    public class DiscountCodes_Tests_Fixture : IAsyncLifetime
    {        
        public DiscountCodeService DiscountCodeService { get; private set; } = new DiscountCodeService(Utils.MyShopifyUrl, Utils.AccessToken);
        public PriceRuleService PriceRuleService { get; private set; } = new PriceRuleService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<PriceRuleDiscountCode> CreatedDiscountCodes { get; private set; } = new List<PriceRuleDiscountCode>();
        public List<PriceRule> CreatedPriceRules { get; private set; } = new List<PriceRule>();        

        public void Initialize()
        {
            // Create one for count, list, get, etc. orders.
            Create(Code);
        }

        public string Code { get; private set; } = "UNITTEST";

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish. 
        /// A PriceRule is created then a PriceRuleDiscountCode is assigned to it
        /// </summary>
        public async Task<PriceRuleDiscountCode> Create(string code, bool skipAddToCreatedList = false)
        {
            var ruleObj = CreatePriceRule(skipAddToCreatedList);

            var discountCode = new PriceRuleDiscountCode()
            {
                Code = code,
                PriceRuleId = ruleObj.Id.Value,
            };

            return CreateDiscountCode(ruleObj, discountCode, skipAddToCreatedList);
        }

        private async Task<PriceRule> CreatePriceRule(bool skipAddToCreatedList)
        {
            var priceRule = new PriceRule()
            {
                Title = "UNIT TEST",
                ValueType = "percentage",
                Value = -30,
                TargetType = "line_item",
                TargetSelection = "all",
                AllocationMethod = "across",
                StartsAt = DateTime.Now,
                CustomerSelection = "all"
            };

            return CreatePriceRule(priceRule, skipAddToCreatedList);
        }

        private async Task<PriceRule> CreatePriceRule(PriceRule rule, bool skipAddToCreatedList)
        {
            var obj = PriceRuleService.Create(rule);

            if (!skipAddToCreatedList)
            {
                CreatedPriceRules.Add(obj);
            }

            return obj;
        }

        private async Task<PriceRuleDiscountCode> CreateDiscountCode(PriceRule rule, PriceRuleDiscountCode discountCode, bool skipAddToCreatedList)
        {
            var obj = DiscountCodeService.Create(rule.Id.Value, discountCode);

            if (!skipAddToCreatedList)
            {
                CreatedDiscountCodes.Add(obj);
            }

            return obj;
        }

        public void Dispose()
        {
            DeleteDiscountCodes();
            DeletePriceRules();
        }

        private async Task DeleteDiscountCodes()
        {
            foreach (var obj in CreatedDiscountCodes)
            {
                try
                {
                    DiscountCodeService.Delete(obj.PriceRuleId.Value, obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete created PriceRuleDiscountCode with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        private async Task DeletePriceRules()
        {
            foreach (var obj in CreatedPriceRules)
            {
                try
                {
                    PriceRuleService.Delete(obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete created PriceRule with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }
    }
}


