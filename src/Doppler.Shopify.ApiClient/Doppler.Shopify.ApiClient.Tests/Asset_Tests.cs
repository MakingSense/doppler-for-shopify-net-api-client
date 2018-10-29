using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Asset")]
    public class Asset_Tests : IClassFixture<Asset_Tests_Fixture>
    {
        private Asset_Tests_Fixture Fixture { get; set; }

        public Asset_Tests(Asset_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Creates_Assets()
        {
            string key = "templates/test.liquid";
            var created = Fixture.Create(key);

            Assert.NotNull(created);
            Assert.Equal(key, created.Key);
            Assert.Equal(Fixture.ThemeId, created.ThemeId);

            // Value is not returned when creating or updating. Must get the asset to check it.
            var asset = Fixture.Service.Get(Fixture.ThemeId, key);

            Assert.Equal(Fixture.AssetValue, asset.Value);
        }

        [Fact]
        public void Updates_Assets()
        {
            string key = "templates/update-test.liquid";
            string newValue = "<h1>Hello, world! I've been updated!</h1>";
            var created = Fixture.Create(key);
            created.Value = newValue;

            Fixture.Service.CreateOrUpdate(Fixture.ThemeId, created);

            // Value is not returned when creating or updating. Must get the asset to check it.
            var updated = Fixture.Service.Get(Fixture.ThemeId, key);

            Assert.Equal(newValue, updated.Value);
        }

        [Fact]
        public void Gets_Assets()
        {
            Fixture.Create("templates/test.liquid");
            string key = Fixture.Created.First().Key;
            var asset = Fixture.Service.Get(Fixture.ThemeId, key);

            Assert.NotNull(asset);
            Assert.Equal(asset.Key, key);
            Assert.Equal(asset.ThemeId, Fixture.ThemeId);
        }

        [Fact]
        public void Copies_Assets()
        {
            string key = "templates/copy-test.liquid";
            var original = Fixture.Create("templates/copy-original-test.liquid");
            var asset = Fixture.Service.CreateOrUpdate(Fixture.ThemeId, new Asset()
            {
                Key = key,
                SourceKey = original.Key,
            });

            Assert.NotNull(asset);
            Assert.Equal(asset.Key, key);
            Assert.Equal(asset.Value, original.Value);
            Assert.Equal(asset.ContentType, original.ContentType);
            Assert.Equal(asset.ThemeId, Fixture.ThemeId);
        }

        [Fact]
        public void Lists_Assets()
        {
            var list = Fixture.Service.List(Fixture.ThemeId);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_Assets()
        {
            bool threw = false;
            string key = "templates/delete-test.liquid";
            var created = Fixture.Create(key, true);

            try
            {
                Fixture.Service.Delete(Fixture.ThemeId, key);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_Assets threw exception. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }
    }

    public class Asset_Tests_Fixture : IDisposable
    {
        public AssetService Service { get; private set; }

        public List<Asset> Created { get; private set; }

        public string AssetValue { get { return "<h1>Hello world!</h1>"; } }

        public long ThemeId { get; set; }

        public Asset_Tests_Fixture()
        {
             Service = new AssetService(Utils.MyShopifyUrl, Utils.AccessToken);
             Created = new List<Asset>();
             Initialize();
        }

        public void Initialize()
        {
            var themeService = new ThemeService(Utils.MyShopifyUrl, Utils.AccessToken);  
            var themes = themeService.List();  

            ThemeId = themes.First().Id.Value;
        }

        public void Dispose()
        {
            foreach (var asset in Created)
            {
                try
                {
                    Service.Delete(ThemeId, asset.Key);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete created Asset with key {0}. {1}", asset.Key, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates the object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public Asset Create(string key, bool skipAddToCreatedList = false)
        {
            var asset = Service.CreateOrUpdate(ThemeId, new Asset()
            {
                ContentType = "text/x-liquid",
                Value = AssetValue,
                Key = key
            });

            if (!skipAddToCreatedList)
            {
                Created.Add(asset);
            }

            return asset;
        }
    }    
}
