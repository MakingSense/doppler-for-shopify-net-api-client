﻿using System.Linq;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Checkout")]
    public class Checkout_Tests
    {
        private CheckoutService _Service { get; set; }

        public Checkout_Tests()
        {
            _Service = new CheckoutService(Utils.MyShopifyUrl, Utils.AccessToken);
        }

        [Fact]
        public void Counts_Checkouts()
        {
            var count = _Service.Count();

            Assert.True(count >= 0);
        }

        [Fact]
        public void Lists_Checkouts()
        {
            var list = _Service.List();

            Assert.True(list.Count() >= 0);
            if (list.Count() > 0)
            { 
                foreach(Checkout ckout in list)
                {
                    Assert.NotNull(ckout.Token);
                    Assert.NotNull(ckout.CartToken);
                    Assert.NotNull(ckout.Email);
                    Assert.True(ckout.LineItems.Count() > 0);
                    foreach(CheckoutLineItem ln in ckout.LineItems)
                    {
                        Assert.NotNull(ln.SKU);
                        Assert.NotNull(ln.ProductId);
                        Assert.NotNull(ln.Price);
                    }
                    Assert.NotNull(ckout.Currency);
                    Assert.NotNull(ckout.Name);
                }
            }
        }
    }
}
