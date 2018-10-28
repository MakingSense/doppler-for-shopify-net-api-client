using System.Net.Http;
using ShopifySharp.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopifySharp.Infrastructure;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating a Shopify inventory items.
    /// </summary>
    public class InventoryItemService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="InventoryItemService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public InventoryItemService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a list of inventory items.
        /// </summary>
        /// <param name="filterOptions">Options for filtering the result. Ids must be populated.</param>
        public virtual List<InventoryItem> List(ListFilter filterOptions)
        {
            var req = PrepareRequest("inventory_items.json");

            if (filterOptions != null)
            {
                req.QueryParams.AddRange(filterOptions.ToParameters());
            }

            return ExecuteRequest<List<InventoryItem>>(req, HttpMethod.Get, rootElement: "inventory_items");
        }

        /// <summary>
        /// Retrieves the <see cref="InventoryItem"/> with the given id.
        /// </summary>
        /// <param name="inventoryItemId">The id of the inventory item to retrieve.</param>
        public virtual InventoryItem Get(long inventoryItemId)
        {
            var req = PrepareRequest(string.Format("inventory_items/{0}.json", inventoryItemId));

            return ExecuteRequest<InventoryItem>(req, HttpMethod.Get, rootElement: "inventory_item");
        }


        /// <summary>
        /// Updates an existing <see cref="InventoryItem"/>.
        /// </summary>
        /// <param name="inventoryItemId">The id of the inventory item to retrieve.</param>
        public virtual InventoryItem Update( long inventoryItemId, InventoryItem inventoryItem )
        {
            var req = PrepareRequest(string.Format("inventory_items/{0}.json", inventoryItemId));
            var content = new JsonContent( new
            {
                inventory_item = inventoryItem
            } );

            return ExecuteRequest<InventoryItem>( req, HttpMethod.Put, content, rootElement: "inventory_item" );
        }

    }
}
