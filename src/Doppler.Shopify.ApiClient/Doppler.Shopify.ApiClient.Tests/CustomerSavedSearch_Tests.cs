using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "CustomerSavedSearchavedSearch")]
    public class CustomerSavedSearchavedSearch_Tests : IClassFixture<Customer_Tests_Fixture>
    {
        private CustomerSavedSearchavedSearch_Tests_Fixture Fixture { get; private set; }

        public CustomerSavedSearchavedSearch_Tests(CustomerSavedSearchavedSearch_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_CustomerSavedSearch()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_CustomerSavedSearch()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Any());
        }

        [Fact]
        public void Deletes_CustomerSavedSearch()
        {
            var created = Fixture.Create();
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Deletes_CustomerSavedSearch)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_CustomerSavedSearch()
        {
            var customerSavedSearch = Fixture.Service.Get(Fixture.Created.First().Id.Value);

            Assert.NotNull(customerSavedSearch);
            Assert.Equal(Fixture.Name, customerSavedSearch.Name);
            Assert.Equal(Fixture.Query, customerSavedSearch.Query);
        }

        [Fact]
        public void Creates_CustomerSavedSearch()
        {
            var customerSavedSearch = Fixture.Create();
            
            Assert.NotNull(customerSavedSearch.Name);
            Assert.Equal(Fixture.Name, customerSavedSearch.Name);
            Assert.Equal(Fixture.Query, customerSavedSearch.Query);
        }

        [Fact]
        public void Updates_CustomerSavedSearch()
        {
            string name = "newName";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.Name = name;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(name, updated.Name);
        }

        [Fact]
        public void Searches_For_CustomerSavedSearch()
        {
            // It takes anywhere between 3 seconds to 30 seconds for Shopify to index new CustomerSavedSearch for searches.
            // Rather than putting a 20 second Thread.Sleep in the test, we'll just assume it's successful if the
            // test doesn't throw an exception.
            bool threw = false;

            try
            {
                var search = Fixture.Service.Search("-notes");
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("{nameof(Searches_For_CustomerSavedSearch)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Can_Be_Partially_Updated()
        {
            string newQuery = "notes=yes";
            var created = Fixture.Create();
            var updated = Fixture.Service.Update(created.Id.Value, new CustomerSavedSearch()
            {
                Query = newQuery
            });

            Assert.Equal(created.Id, updated.Id);
            Assert.Equal(newQuery, updated.Query);

            // In previous versions of ShopifySharp, the updated JSON would have sent 'email=null' or 'note=null', clearing out the email address.
            Assert.Equal(created.Name, updated.Name);
        }

        [Fact]
        public void Retrieves_Customers_In_A_Saved_Search()
        {
            var customerFixture = new Customer_Tests_Fixture();
            var expectedCustomer = customerFixture.Create();

            var savedSearch = Fixture.Create();

            var customersInSearch = Fixture.Service.GetCustomersFromSavedSearch(savedSearch.Id.Value);
            var actualCustomer = customersInSearch.Single();
            
            Assert.Equal(expectedCustomer.Id, actualCustomer.Id);
            Assert.Equal(customerFixture.FirstName, actualCustomer.FirstName);
            Assert.Equal(customerFixture.LastName, actualCustomer.LastName);
            Assert.Equal(customerFixture.Note, actualCustomer.Note);
        }
    }

    public class CustomerSavedSearchavedSearch_Tests_Fixture : IAsyncLifetime
    {
        public CustomerSavedSearchService Service { get; private set; } = new CustomerSavedSearchService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<CustomerSavedSearch> Created { get; private set; } = new List<CustomerSavedSearch>();

        public string Name => "My Test";
        public string Query => "-notes";

        public void Initialize()
        {
            // Create one customer for use with count, list, get, etc. tests.
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
                        Console.WriteLine(string.Format("Failed to delete created CustomerSavedSearchavedSearch with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        public async Task<CustomerSavedSearch> Create(bool skipAddToCreatedList = false)
        {
            var obj = Service.Create(new CustomerSavedSearch()
            {
                Name = "My Test",
                Query = "-notes"
            });

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
