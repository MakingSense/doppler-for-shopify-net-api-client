using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EmptyAssert = Doppler.Shopify.ApiClient.Tests.Extensions.EmptyExtensions;

namespace Doppler.Shopify.ApiClient.Tests
{
    [Trait("Category", "CustomerAddress")]
    public class CustomerAddress_Tests : IClassFixture<CustomerAddress_Tests_Fixture>
    {
        private CustomerAddress_Tests_Fixture Fixture { get; set; }

        public CustomerAddress_Tests(CustomerAddress_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Lists_Addresses()
        {
            var Addresss = Fixture.Service.List(Fixture.CustomerId.Value);

            Assert.True(Addresss.Count() > 0);
        }

        [Fact]
        public void Deletes_Addresses()
        {
            var created = Fixture.Create("123 4th St", true);
            bool threw = false;

            try
            {
                Fixture.Service.Delete(Fixture.CustomerId.Value, created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine(string.Format("Deletes_Addresses threw exception. {0}", ex.Message));

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public void Gets_Addresses()
        {
            var created = Fixture.Create("234 5th St");
            var address = Fixture.Service.Get(Fixture.CustomerId.Value, created.Id.Value);

            Assert.NotNull(address);
            Assert.Equal(Fixture.FirstName, address.FirstName);
            Assert.Equal(Fixture.LastName, address.LastName);
        }

        [Fact]
        public void Creates_Addresses()
        {
            var address = Fixture.Create("345 6th St");

            Assert.NotNull(address);
            Assert.Equal(Fixture.FirstName, address.FirstName);
            Assert.Equal(Fixture.LastName, address.LastName);

            Fixture.Service.Delete(Fixture.CustomerId.Value, address.Id.Value);
        }

        [Fact]
        public void Updates_Addresss()
        {
            string firstName = "Jane";
            string lastName = "Doe";
            string fullName = "Jane Doe";
            var created = Fixture.Create("456 7th St");
            long id = created.Id.Value;

            created.FirstName = firstName;
            created.LastName = lastName;
            created.Name = fullName;
            created.Id = null;

            var updated = Fixture.Service.Update(Fixture.CustomerId.Value, id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(firstName, updated.FirstName);
        }
    }

    public class CustomerAddress_Tests_Fixture : IDisposable
    {
        public CustomerAddressService Service { get; private set; }

        public string FirstName { get { return "John"; } }

        public string LastName { get { return "Doe"; } }

        public long? CustomerId { get; set; }

        public List<Address> Created { get; private set; }

        public CustomerAddress_Tests_Fixture()
        {
            Service = new CustomerAddressService(Utils.MyShopifyUrl, Utils.AccessToken);
            Created = new List<Address>();
            Initialize();
        }

        public void Initialize()
        {
            var customerService = new CustomerService(Utils.MyShopifyUrl, Utils.AccessToken);
            var customers = customerService.List();

            CustomerId = customers.First().Id;

            // Create at least one Address for list, count, etc commands.
            //Create();
        }

        public void Dispose()
        {
            foreach (var Address in Created)
            {
                try
                {
                    Service.Delete(CustomerId.Value, Address.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(string.Format("Failed to delete CustomerAddress with id {0}. {1}", Address.Id.Value, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Gets an object from the list of already created objects, or creates the object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public Address Create(string streetAddress, bool skipAddToDeleteList = false)
        {
            var obj = Service.Create(CustomerId.Value, new Address()
            {
                Address1 = streetAddress,
                City = "Minneapolis",
                Province = "Minnesota",
                ProvinceCode = "MN",
                Zip = "55401",
                Phone = "555-555-5555",
                FirstName = FirstName,
                LastName = LastName,
                Company = "Tomorrow Corporation",
                Country = "United States",
                CountryCode = "US",
            });

            if (!skipAddToDeleteList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
