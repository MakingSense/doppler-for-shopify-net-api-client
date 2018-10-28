using Newtonsoft.Json.Linq;
using System.Net.Http;
using Doppler.Shopify.ApiClient.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;
using System;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// A service for manipulating Carriers
    /// </summary>
    public class CarrierService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="CarrierService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public CarrierService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }        
        
        /// <summary>
        /// Retrieve a list of all carrier services that are associated with the store.
        /// </summary>
        /// <returns>The list of <see cref="Carrier" that are associated with the store.</returns>
        public virtual List<Carrier> List()
        {
            var req = PrepareRequest("carrier_services.json");

            return ExecuteRequest<List<Carrier>>(req, HttpMethod.Get, rootElement: "carrier_services");
        }

        /// <summary>
        /// Creates a new <see cref="Carrier"/> Carrier
        /// </summary>
        /// <param name="carrier">A new <see cref="Carrier"/>. Id should be set to null.</param>
        /// <returns>The new <see cref="Carrier"/>.</returns>
        public virtual Carrier Create(Carrier carrier)
        {
            var req = PrepareRequest("carrier_services.json");
            var content = new JsonContent(new
            {
                carrier_service = carrier
            });

            return ExecuteRequest<Carrier>(req, HttpMethod.Post, content, "carrier_service");
        }

        /// <summary>
        /// Retrieves the <see cref="Carrier"/> with the given id.
        /// </summary>
        /// <param name="carrierId">The id of the Carrier to retrieve.</param>
        /// <returns>The <see cref="Carrier"/>.</returns>
        public virtual Carrier Get(long carrierId)
        {            
            var req = PrepareRequest(string.Format("carrier_services/{0}.json", carrierId));

            return ExecuteRequest<Carrier>(req, HttpMethod.Get, rootElement: "carrier_service");           
        }

        /// <summary>
        /// Deletes a Carruer with the given Id.
        /// </summary>
        /// <param name="carrierId">The Carrier's Id.</param>
        public virtual void Delete(long carrierId)
        {
            var req = PrepareRequest(string.Format("carrier_services/{0}.json", carrierId));

            ExecuteRequest(req, HttpMethod.Delete);
        }

        /// <summary>
        /// Updates the given <see cref="Carrier"/>.
        /// </summary>
        /// <param name="carrierId">Id of the Carrier being updated.</param>
        /// <param name="carrier">The <see cref="Carrier"/> to update.</param>
        /// <returns>The updated <see cref="Carrier"/>.</returns>
        public virtual Carrier Update(long carrierId, Carrier carrier)
        {
            var req = PrepareRequest(string.Format("carrier_services/{0}.json", carrierId));
            var content = new JsonContent(new
            {
                carrier_service = carrier
            });

            return ExecuteRequest<Carrier>(req, HttpMethod.Put, content, "carrier_service");
        }
    }
}
