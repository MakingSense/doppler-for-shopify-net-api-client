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
        private DiscountCodes_Tests_Fixture Fixture { get; set; }

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

    public class DiscountCodes_Tests_Fixture : IDisposable
    {        
        public DiscountCodeService DiscountCodeService { get; private set; }
        public PriceRuleService PriceRuleService { get; private set; }

        public List<PriceRuleDiscountCode> CreatedDiscountCodes { get; private set; }
        public List<PriceRule> CreatedPriceRules { get; private set; }        

        public DiscountCodes_Tests_Fixture()
        {
            DiscountCodeService = new DiscountCodeService(Utils.MyShopifyUrl, Utils.AccessToken);
            PriceRuleService = new PriceRuleService(Utils.MyShopifyUrl, Utils.AccessToken);
            CreatedDiscountCodes = new List<PriceRuleDiscountCode>();
            CreatedPriceRules= new List<PriceRule>(); 
            Initialize();
        }

        public void Initialize()
        {
            // Create one for count, list, get, etc. orders.
            Create(Code);
        }
        private string _code = "UNITTEST";
        public string Code 
        { 
            get 
            { 
                return _code; 
            }    
            set 
            { 
                _code = value; 
            } 
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish. 
        /// A PriceRule is created then a PriceRuleDiscountCode is assigned to it
        /// </summary>
        public PriceRuleDiscountCode Create(string code, bool skipAddToCreatedList = false)
        {
            var ruleObj = CreatePriceRule(skipAddToCreatedList);

            var discountCode = new PriceRuleDiscountCode()
            {
                Code = code,
                PriceRuleId = ruleObj.Id.Value,
            };

            return CreateDiscountCode(ruleObj, discountCode, skipAddToCreatedList);
        }

        private PriceRule CreatePriceRule(bool skipAddToCreatedList)
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

        private PriceRule CreatePriceRule(PriceRule rule, bool skipAddToCreatedList)
        {
            var obj = PriceRuleService.Create(rule);

            if (!skipAddToCreatedList)
            {
                CreatedPriceRules.Add(obj);
            }

            return obj;
        }

        private PriceRuleDiscountCode CreateDiscountCode(PriceRule rule, PriceRuleDiscountCode discountCode, bool skipAddToCreatedList)
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

        private void DeleteDiscountCodes()
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
                        Console.WriteLine(string.Format("Failed to delete created PriceRuleDiscountCode with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        private void DeletePriceRules()
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
                        Console.WriteLine(string.Format("Failed to delete created PriceRule with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }
    }
}


