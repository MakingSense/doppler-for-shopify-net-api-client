using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Blog")]
    public class Blog_Tests : IClassFixture<Blog_Tests_Fixture>
    {
        private Blog_Tests_Fixture Fixture { get; set; }

        public Blog_Tests(Blog_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Blogs()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Blogs()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Gets_Blogs()
        {
            var id = Fixture.Created.First().Id.Value;
            var blog = Fixture.Service.Get(id);

            Assert.True(blog.Id.HasValue);
            Assert.StartsWith(Fixture.Title, blog.Title);
            Assert.Equal(blog.Commentable, Fixture.Commentable);
        }

        [Fact]
        public void Deletes_Blogs()
        {
            var created = Fixture.Create(true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_Blogs threw exception. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Creates_Blogs()
        {
            var created = Fixture.Create();

            Assert.NotNull(created);
            Assert.StartsWith(Fixture.Title, created.Title);
            Assert.Equal(Fixture.Commentable, created.Commentable);
        }

        [Fact]
        public void Updates_Blogs()
        {
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Commentable = "yes";
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal("yes", updated.Commentable);
        }
    }

    public class Blog_Tests_Fixture : IDisposable
    {
        public BlogService Service { get; private set; }

        public List<Blog> Created { get; private set; }

        public string Title { get { return "Doppler.Shopify.ApiClient Test Blog"; } }

        public string Commentable { get { return "moderate"; } }

        public Blog_Tests_Fixture()
        {
            Service = new BlogService(Utils.MyShopifyUrl, Utils.AccessToken);
            Created = new List<Blog>();
            Initialize();
        }

        public void Initialize()
        {
            // Create one blog for methods like count, get, list, etc.
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
                        Console.WriteLine(string.Format("Failed to delete created Blog with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public Blog Create(bool skipAddToCreatedList = false)
        {
            var blog = Service.Create(new Blog()
            {
                Title = string.Format("{0} #{1}", Title, Guid.NewGuid()),
                Commentable = Commentable,
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(blog);
            }

            return blog;
        }
    }
}
