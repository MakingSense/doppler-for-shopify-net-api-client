# Doppler-Shopify .net APIs Client

Doppler-Shopify .net APIs Client is a .NET 4.0 library intended to be used from [Doppler](https://github.com/MakingSense/Doppler) as a Nuget package in order to provide an abstraction layer when consuming the enpoints for [doppler-for-shopify](https://github.com/MakingSense/doppler-for-shopify) and the [Shopify REST Admin API](https://help.shopify.com/en/api/reference).

**Note**: This code is based in the [ShopifySharp](https://github.com/nozzlegear/ShopifySharp). I would recommend to take a look at how it works. This is a reduced (and ported) version of that library.

# Installation

Add the makingsense-aspnet package source to your Nuget.config file:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
	<packageSources>
		<clear />
		<add key="api.nuget.org" value="https://api.nuget.org/v3/index.json" />
		<add key="makingsense-aspnet" value="https://ci.appveyor.com/nuget/makingsense-aspnet" />
	</packageSources>
</configuration>
```

# Features

## List shops associated to a Doppler account

```csharp
var dopplerIntegrationService = new DopplerIntegrationService(_dopplerForShopifyUrl);
return dopplerIntegrationService.GetShops(_dopplerApiKey);
```

Where `_dopplerForShopifyUrl` is the URL where [doppler-for-shopify](https://github.com/MakingSense/doppler-for-shopify) is hosted (by default https://sfy.fromdoppler.com) and `_dopplerApiKey` well, you know...
You will get a response like this:
```json
[
  {
    "ShopName": "doppler-dev01.myshopify.com",
    "ShopifyAccessToken": "ecf068aaed17e66eec9c6ce794b1209a",
    "ConnectedOn": "2019-01-25T20:25:09.068Z",
    "SyncProcessDopplerImportSubscribersTaskId": "import-100080535",
    "DopplerListId": 16725114,
    "SyncProcessInProgress": false,
    "ImportedCustomersCount": 170,
    "SyncProcessLastRunDate": "2019-01-25T20:33:41.269Z"
  }
]
```
The most important field here is **ShopifyAccessToken** that will be used in the other services provided by this library like getting the abandoned carts or products for a given shop.

## Synchronize customers
```csharp
var dopplerIntegrationService = new DopplerIntegrationService(_dopplerForShopifyUrl);
dopplerIntegrationService.SynchronizeCustomers(_dopplerApiKey, _shop);
```

## List the abandoned carts

```csharp
 var checkoutsService = new CheckoutService(_shop, _accessToken);

var filter = new CheckoutFilter
{
    CreatedAtMin = _since,
    CreatedAtMax = _until
};
return checkoutsService.List(filter);
```
Where `_shop` is the shop domain (e.g.: "doppler-dev01.myshopify.com"), and `_accessToken` is the token for the [Shopify REST Admin API](https://help.shopify.com/en/api/reference) (provided by `DopplerIntegrationService`)

> Explore some other options for the `CheckoutFilter`

## List the shop's products
```csharp
var prodcutService = new ProductService(_shop, _accessToken);

var filter = new ProductFilter
{
    PublishedStatus= _publishedOnly ? "published" : "any"
};

return prodcutService.List(filter);
```

> Explore some other options for the `ProductFilter`

# Deploying to Nuget

1. Set the environment variable `MAKING_SENSE_APP_VEYOR_NUGET_API_KEY`
2. run the script `publish.ps1` under the `.nuget` directory.







