using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "Customer")]
    public class Customer_Tests : IClassFixture<Customer_Tests_Fixture>
    {
        private Customer_Tests_Fixture Fixture { get; set; }

        public Customer_Tests(Customer_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Counts_Customers()
        {
            var count = Fixture.Service.Count();

            Assert.True(count > 0);
        }

        [Fact]
        public void Lists_Customers()
        {
            var list = Fixture.Service.List();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Deletes_Customers()
        {
            var created = Fixture.Create();
            bool threw = false;

            try
            {
                Fixture.Service.Delete(created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_Customers failed. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_Customers()
        {
            var customer = Fixture.Service.Get(Fixture.Created.First().Id.Value);

            Assert.NotNull(customer);
            Assert.Equal(Fixture.FirstName, customer.FirstName);
            Assert.Equal(Fixture.LastName, customer.LastName);
            Assert.Equal(Fixture.Note, customer.Note);
            Assert.NotNull(customer.Addresses);
            Assert.NotNull(customer.DefaultAddress);
        }

        [Fact]
        public void Gets_Customers_With_Options()
        {
            var customer = Fixture.Service.Get(Fixture.Created.First().Id.Value, "first_name,last_name");

            Assert.NotNull(customer);
            Assert.Equal(Fixture.FirstName, customer.FirstName);
            Assert.Equal(Fixture.LastName, customer.LastName);
            EmptyAssert.NullOrEmpty(customer.Note);
            EmptyAssert.NullOrEmpty(customer.Addresses);
            Assert.Null(customer.DefaultAddress);

        }

        [Fact]
        public void Creates_Customers()
        {
            var customer = Fixture.Create();

            Assert.NotNull(customer);
            Assert.Equal(Fixture.FirstName, customer.FirstName);
            Assert.Equal(Fixture.LastName, customer.LastName);
            Assert.Equal(Fixture.Note, customer.Note);
            Assert.NotNull(customer.Addresses);
        }

        [Fact]
        public void Creates_Customers_With_Options()
        {
            var customer = Fixture.Create(options: new CustomerCreateOptions()
            {
                Password = "loktarogar",
                PasswordConfirmation = "loktarogar",
                SendEmailInvite = false,
                SendWelcomeEmail = false,
            });

            Assert.NotNull(customer);
            Assert.Equal(Fixture.FirstName, customer.FirstName);
            Assert.Equal(Fixture.LastName, customer.LastName);
            Assert.Equal(Fixture.Note, customer.Note);
            Assert.NotNull(customer.Addresses);
        }

        [Fact]
        public void Updates_Customers()
        {
            string firstName = "Jane";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.FirstName = firstName;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(firstName, updated.FirstName);
        }

        [Fact]
        public void Updates_Customers_With_Options()
        {
            string firstName = "Jane";
            var created = Fixture.Create();
            long id = created.Id.Value;

            created.FirstName = firstName;
            created.Id = null;

            var updated = Fixture.Service.Update(id, created, new CustomerUpdateOptions()
            {
                Password = "loktarogar",
                PasswordConfirmation = "loktarogar"
            });

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(firstName, updated.FirstName);
        }

        [Fact]
        public void Searches_For_Customers()
        {
            // It takes anywhere between 3 seconds to 30 seconds for Shopify to index new customers for searches.
            // Rather than putting a 20 second Thread.Sleep in the test, we'll just assume it's successful if the
            // test doesn't throw an exception.
            bool threw = false;

            try
            {
                var search = Fixture.Service.Search("John");
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Searches_For_Customers failed. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Can_Be_Partially_Updated()
        {
            string newFirstName = "Sheev";
            string newLastName = "Palpatine";
            var created = Fixture.Create();
            var updated = Fixture.Service.Update(created.Id.Value, new Customer()
            {
                FirstName = newFirstName,
                LastName = newLastName
            });

            Assert.Equal(created.Id, updated.Id);
            Assert.Equal(newFirstName, updated.FirstName);
            Assert.Equal(newLastName, updated.LastName);

            // In previous versions of Doppler.Shopify.ApiClient, the updated JSON would have sent 'email=null' or 'note=null', clearing out the email address.
            Assert.Equal(created.Email, updated.Email);
            Assert.Equal(created.Note, updated.Note);
        }

        [Fact]
        public void SendInvite_Customers_Default()
        {
            var created = Fixture.Create();
            long id = created.Id.Value;

            var invite = Fixture.Service.SendInvite(created.Id.Value);

            Assert.NotNull(invite);

        }

        [Fact]
        public void SendInvite_Customers_Custom()
        {
            var created = Fixture.Create();
            long id = created.Id.Value;

            var options = new CustomerInvite()
            {
                Subject = "Custom Subject courtesy of Doppler.Shopify.ApiClient",
                CustomMessage = "Custom Message courtesy of Doppler.Shopify.ApiClient"
            };

            var invite = Fixture.Service.SendInvite(created.Id.Value, options);

            Assert.NotNull(invite);
            Assert.Equal(options.Subject, invite.Subject);
            Assert.Equal(options.CustomMessage, invite.CustomMessage);

        }

        [Fact]
        public void GetAccountActivationUrl_Customers()
        {
            var created = Fixture.Create();
            long id = created.Id.Value;

            var url = Fixture.Service.GetAccountActivationUrl(created.Id.Value);

            Assert.NotEmpty(url);
            Assert.Contains("account/activate", url);

        }
    }

    public class Customer_Tests_Fixture : IDisposable
    {
        public CustomerService Service { get; private set; }

        public List<Customer> Created { get; private set; }

        public string FirstName { get { return "John"; } }

        public string LastName { get { return "Doe"; } }

        public string Note { get { return "Test note about this customer."; } }

        public Customer_Tests_Fixture()
        {
            ShopifyService.SetGlobalExecutionPolicy(new RetryExecutionPolicy());
            Service = new CustomerService(Utils.MyShopifyUrl, Utils.AccessToken);
            Created = new List<Customer>();
            Initialize();
        }

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
                        Console.WriteLine(string.Format("Failed to delete created Customer with id {0}. {1}", obj.Id.Value, ex.Message));
                    }
                }
            }
        }

        public Customer Create(bool skipAddToCreatedList = false, CustomerCreateOptions options = null)
        {
            var obj = Service.Create(new Customer()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Guid.NewGuid().ToString() + "@example.com",
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        Address1 = "123 4th Street",
                        City = "Minneapolis",
                        Province = "Minnesota",
                        ProvinceCode = "MN",
                        Zip = "55401",
                        Phone = "555-555-5555",
                        FirstName = "John",
                        LastName = "Doe",
                        Company = "Tomorrow Corporation",
                        Country = "United States",
                        CountryCode = "US",
                        Default = true,
                    }
                },
                VerifiedEmail = true,
                Note = Note,
                State = "enabled"
            }, options);

            if (!skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
