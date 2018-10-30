using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Theme")]
    public class Theme_Tests : IClassFixture<Theme_Tests_Fixture>
    {
        private Theme_Tests_Fixture Fixture { get; set; }

        public Theme_Tests(Theme_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Lists_Themes()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact(Skip = "Often fails during CI tests because stores can only have 20 themes.")]
        public void Deletes_Themes()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_Themes failed. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact(Skip = "Often fails during CI tests because stores can only have 20 themes.")]
        public void Gets_Themes()
        {
            var created = Fixture.Create();
            var obj = Fixture.Service.Get(created.Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.StartsWith(Fixture.NamePrefix, obj.Name);
            Assert.Equal(Fixture.Role, obj.Role);
        }

        [Fact(Skip = "Often fails during CI tests because stores can only have 20 themes.")]
        public void Creates_Themes()
        {
            var obj = Fixture.Create();

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.StartsWith(Fixture.NamePrefix, obj.Name);
            Assert.Equal(Fixture.Role, obj.Role);
        }

        [Fact(Skip = "Often fails during CI tests because stores can only have 20 themes.")]
        public void Creates_Themes_From_Zip_Files()
        {
            var obj = Fixture.Create(fromZipUrl: true);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.StartsWith(Fixture.NamePrefix, obj.Name);
            Assert.Equal(Fixture.Role, obj.Role);
        }

        [Fact(Skip = "Often fails during CI tests because stores can only have 20 themes.")]
        public void Updates_Themes()
        {
            string newValue = ("Doppler.Shopify.ApiClient Updated Theme " + Guid.NewGuid()).Substring(0, 50);
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Name = newValue;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(newValue, updated.Name);
        }
    }

    public class Theme_Tests_Fixture : IDisposable
    {
        private ThemeService _service = new ThemeService(Utils.MyShopifyUrl, Utils.AccessToken);
        private List<Theme> _created = new List<Theme>();

        public ThemeService Service { get { return _service; } private set { _service = value; } }
        public List<Theme> Created { get { return _created; } private set { _created = value; } }
        /// <summary>
        /// A URL pointing to a zipped up Shopify theme.
        /// </summary>
        public string ZipUrl
        {
            get
            {
                return "https://ironstorage.blob.core.windows.net/public-downloads/Doppler.Shopify.ApiClient/shopify_test_theme_for_Doppler.Shopify.ApiClient.zip";
            }
        }

        public string NamePrefix
        {
            get
            {
                return "Doppler.Shopify.ApiClient Test Theme ";
            }
        }

        public string Role
        {
            get
            {
                return "unpublished";
            }
        }

        public Theme_Tests_Fixture()
        {
            Initialize();
        }

        public void Initialize()
        {
            // Create one collection for use with count, list, get, etc. tests.
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
                        Console.WriteLine(string.Format("Failed to delete created Theme with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public Theme Create(bool skipAddToCreatedList = false, bool fromZipUrl = false)
        {
            var theme = new Theme()
            {
                Name = (NamePrefix + Guid.NewGuid()).Substring(0, 50),
                Role = Role,
            };
            var obj = fromZipUrl ? Service.Create(theme, ZipUrl) : Service.Create(theme);

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}