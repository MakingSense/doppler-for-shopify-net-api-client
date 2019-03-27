using CommandLine;
using System;

namespace Doppler.Shopify.ConsoleApiClient
{
    interface IOptions
    {}

    abstract class ShopifyApiOptions : IOptions
    {
        [Option(
         't',
          "accessToken",
          Required = true,
          HelpText = "The Shopify API access token for a shop")]
        public string AccessToken { get; set; }

        [Option(
         's',
          "shop",
          Required = true,
          HelpText = "The given shop")]
        public string Shop { get; set; }
    }

    [Verb("shops", HelpText = "Returns all the shops associated to a Doppler account")]
    class ShopsOptions : IOptions
    {
        [Option(
          'k',
          "dopplerApiKey",
          Required = true,
          HelpText = "The Doppler API Key to get the associated shops")]
        public string DopplerApiKey { get; set; }

        [Option(
          Required = false,
          Default = "https://sfy.fromdoppler.com:4433", // TODO: use production
          HelpText = "The Url to use when retrieving shops")]
        public string DopplerForShopifyUrl { get; set; }
    }

    [Verb("sync", HelpText = "Run the  customer <-> subscriber synchronization process")]
    class SynchronizeCustomersOptions : IOptions
    {
        [Option(
          's',
          "shop",
          Required = true,
          HelpText = "The shop domain")]
        public string Shop { get; set; }

        [Option(
          'k',
          "dopplerApiKey",
          Required = true,
          HelpText = "The Doppler API Key to get the associated shops")]
        public string DopplerApiKey { get; set; }

        [Option(
          Required = false,
          Default = "https://sfy.fromdoppler.com:4433", // TODO: use production
          HelpText = "The Url to use when retrieving shops")]
        public string DopplerForShopifyUrl { get; set; }
    }

    [Verb("checkouts", HelpText = "Returns all the abandoned checkouts for a given shop")]
    class CheckoutsOptions : ShopifyApiOptions
    {
        [Option(
          "since",
          Required = false,
          HelpText = "Filter abandoned checkouts with a creation date greater than this value (format: yyyy-MM-dd)")]
        public string Since { get; set; }

        [Option(
          "until",
          Required = false,
          HelpText = "Filter abandoned checkouts with a creation date less than this value (format: yyyy-MM-dd)")]
        public string Until { get; set; }
    }

    [Verb("products", HelpText = "Returns all the visited products for a given shop")]
    class ProductsOptions : ShopifyApiOptions
    {
        [Option(
          "since",
          Required = false,
          HelpText = "Filter visited products with a creation date greater than this value (format: yyyy-MM-dd)")]
        public string Since { get; set; }

        [Option(
          "until",
          Required = false,
          HelpText = "Filter visited products with a creation date less than this value (format: yyyy-MM-dd)")]
        public string Until { get; set; }

        [Option(
          "published-only",
          Required = false,
          Default = false,
          HelpText = "Filter only published products")]
        public bool PublishedOnly { get; set; }
    }
    
    // Maybe we can unify all in the "products" verb
    //[Verb("published-products", HelpText = "Returns all the published products for a given shop")]
    //class PublishedProductsOptions : ShopifyApiOptions
    //{

    //}
}
