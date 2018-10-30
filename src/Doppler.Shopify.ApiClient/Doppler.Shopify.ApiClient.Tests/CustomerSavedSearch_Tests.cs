//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using Xunit;
//using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

//namespace Doppler.Shopify.ApiClient.Tests
//{
//    [Trait("Category", "CustomerSavedSearchavedSearch")]
//    private class CustomerSavedSearchavedSearch_Tests : IClassFixture<CustomerSavedSearchavedSearch_Tests_Fixture>
//    {
//        private CustomerSavedSearchavedSearch_Tests_Fixture Fixture { get; set; }

//        public CustomerSavedSearchavedSearch_Tests(CustomerSavedSearchavedSearch_Tests_Fixture fixture)
//        {
//            this.Fixture = fixture;
//        }

//        [Fact]
//        public void Counts_CustomerSavedSearch()
//        {
//            var count = Fixture.Service.Count();

//            Assert.True(count > 0);
//        }

//        [Fact]
//        public void Lists_CustomerSavedSearch()
//        {
//            var list = Fixture.Service.List();

//            Assert.True(list.Any());
//        }

//        [Fact]
//        public void Deletes_CustomerSavedSearch()
//        {
//            var created = Fixture.Create();
//            bool threw = false;

//            try
//            {
//                Fixture.Service.Delete(created.Id.Value);
//            }
//            catch (ShopifyException ex)
//            {
//                Console.WriteLine(string.Format("Deletes_CustomerSavedSearch failed. {0}", ex.Message));

//                threw = true;
//            }

//            Assert.False(threw);
//        }

//        [Fact]
//        public void Gets_CustomerSavedSearch()
//        {
//            var customerSavedSearch = Fixture.Service.Get(Fixture.Created.First().Id.Value);

//            Assert.NotNull(customerSavedSearch);
//            //Assert.Equal(Fixture.Name, customerSavedSearch.Name);
//            //Assert.Equal(Fixture.Query, customerSavedSearch.Query);
//        }

//        [Fact]
//        public void Creates_CustomerSavedSearch()
//        {
//            var customerSavedSearch = Fixture.Create();
            
//            Assert.NotNull(customerSavedSearch.Name);
//            //Assert.Equal(Fixture.Name, customerSavedSearch.Name);
//            //Assert.Equal(Fixture.Query, customerSavedSearch.Query);
//        }

//        [Fact]
//        public void Updates_CustomerSavedSearch()
//        {
//            string name = "newName";
//            var created = Fixture.Create();
//            long id = created.Id.Value;

//            created.Name = name;
//            created.Id = null;

//            var updated = Fixture.Service.Update(id, created);

//            // Reset the id so the Fixture can properly delete this object.
//            created.Id = id;

//            Assert.Equal(name, updated.Name);
//        }

//        [Fact]
//        public void Searches_For_CustomerSavedSearch()
//        {
//            // It takes anywhere between 3 seconds to 30 seconds for Shopify to index new CustomerSavedSearch for searches.
//            // Rather than putting a 20 second Thread.Sleep in the test, we'll just assume it's successful if the
//            // test doesn't throw an exception.
//            bool threw = false;

//            try
//            {
//                var search = Fixture.Service.Search("-notes");
//            }
//            catch (ShopifyException ex)
//            {
//                Console.WriteLine(string.Format("Searches_For_CustomerSavedSearch failed. {0}", ex.Message));

//                threw = true;
//            }

//            Assert.False(threw);
//        }

//        [Fact]
//        public void Can_Be_Partially_Updated()
//        {
//            string newQuery = "notes=yes";
//            var created = Fixture.Create();
//            var updated = Fixture.Service.Update(created.Id.Value, new CustomerSavedSearch()
//            {
//                Query = newQuery
//            });

//            Assert.Equal(created.Id, updated.Id);
//            Assert.Equal(newQuery, updated.Query);

//            // In previous versions of Doppler.Shopify.ApiClient, the updated JSON would have sent 'email=null' or 'note=null', clearing out the email address.
//            Assert.Equal(created.Name, updated.Name);
//        }

//        [Fact]
//        public void Retrieves_Customers_In_A_Saved_Search()
//        {
//            using (var customerFixture = new Customer_Tests_Fixture())
//            {
//                var expectedCustomer = customerFixture.Create();
//                var savedSearch = Fixture.Create();

//                var customersInSearch = Fixture.Service.GetCustomersFromSavedSearch(savedSearch.Id.Value);
//                var actualCustomer = customersInSearch.Single();

//                Assert.Equal(expectedCustomer.Id, actualCustomer.Id);
//                Assert.Equal(customerFixture.FirstName, actualCustomer.FirstName);
//                Assert.Equal(customerFixture.LastName, actualCustomer.LastName);
//                Assert.Equal(customerFixture.Note, actualCustomer.Note);
//            }
//        }
//    }

//    public class CustomerSavedSearchavedSearch_Tests_Fixture : IDisposable
//    {
//        public CustomerSavedSearchService Service { get; private set; }

//        public List<CustomerSavedSearch> Created { get; private set; }

//        public string Name { get { return "My Test"; } }
//        public string Query { get { return "-notes"; } }

//        public CustomerSavedSearchavedSearch_Tests_Fixture()
//        {
//            Service =  new CustomerSavedSearchService(Utils.MyShopifyUrl, Utils.AccessToken);
//            Created =  new List<CustomerSavedSearch>();
//            Initialize();
//        }

//        public void Initialize()
//        {
//            // Create one customer for use with count, list, get, etc. tests.
//            Create();
//        }

//        public void Dispose()
//        {
//            foreach (var obj in Created)
//            {
//                try
//                {
//                    Service.Delete(obj.Id.Value);
//                }
//                catch (ShopifyException ex)
//                {
//                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
//                    {
//                        Console.WriteLine(string.Format("Failed to delete created CustomerSavedSearchavedSearch with id {0}. {1}", obj.Id.Value, ex.Message));
//                    }
//                }
//            }
//        }

//        public CustomerSavedSearch Create(bool skipAddToCreatedList = false)
//        {
//            var obj = Service.Create(new CustomerSavedSearch()
//            {
//                Name = Guid.NewGuid().ToString().Replace("-", ""),
//                Query = "-notes"
//            });

//            if (!skipAddToCreatedList)
//            {
//                Created.Add(obj);
//            }

//            return obj;
//        }
//    }
//}
