using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "ScriptTag")]
    public class ScriptTag_Tests : IClassFixture<ScriptTag_Tests_Fixture>
    {
        private ScriptTag_Tests_Fixture Fixture { get; set; }

        public ScriptTag_Tests(ScriptTag_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_ScriptTags()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_ScriptTags()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_ScriptTags()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_ScriptTags failed. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_ScriptTags()
        {
            var obj = Fixture.Service.Get(Fixture.Created.First().Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.Src, obj.Src);
            Assert.Equal(Fixture.Event, obj.Event);
            Assert.Equal(Fixture.Scope, obj.DisplayScope);
        }

        [Fact]
        public void Creates_ScriptTags()
        {
            var obj = Fixture.Create();

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Equal(Fixture.Src, obj.Src);
            Assert.Equal(Fixture.Event, obj.Event);
            Assert.Equal(Fixture.Scope, obj.DisplayScope);
        }

        [Fact]
        public void Updates_ScriptTags()
        {
            string newValue = "all";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.DisplayScope = newValue;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(newValue, updated.DisplayScope);   
        }
    }

    public class ScriptTag_Tests_Fixture : IDisposable
    {
        private ScriptTagService _service = new ScriptTagService(Utils.MyShopifyUrl, Utils.AccessToken);
        private List<ScriptTag> _created = new List<ScriptTag>();

        public ScriptTagService Service { get { return _service; } private set { _service = value; } }
        public List<ScriptTag> Created { get { return _created; } private set { _created = value; } }
        public string Event
        {
            get
            {
                return "onload";
            }
        }

        public string Src
        {
            get
            {
                return "https://unpkg.com/davenport@2.1.0/bin/browser.js";
            }
        }

        public string Scope
        {
            get
            {
                return "online_store";
            }
        }

        public ScriptTag_Tests_Fixture()
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
                        Console.WriteLine(string.Format("Failed to delete created ScriptTag with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public ScriptTag Create(bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(new ScriptTag()
            {
                Event = Event,
                Src = Src,
                DisplayScope = Scope,
            });

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}