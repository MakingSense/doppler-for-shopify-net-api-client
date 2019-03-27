using Doppler.Shopify.ConsoleApiClient.Actions;
using Doppler.Shopify.ConsoleApiClient.Helpers;

namespace Doppler.Shopify.ConsoleApiClient
{
    static class ActionFactory
    {
        public static IAction CreateAction(IOptions options)
        {
            if (options is SynchronizeCustomersOptions synchronizeCustomersOptions)
            {
                return new SynchronizeCustomersAction(synchronizeCustomersOptions.DopplerApiKey, synchronizeCustomersOptions.DopplerForShopifyUrl, synchronizeCustomersOptions.Shop);
            }

            if (options is ShopsOptions shopOptions)
            {
                return new ShopsAction(shopOptions.DopplerApiKey, shopOptions.DopplerForShopifyUrl);
            }

            if (options is CheckoutsOptions checkoutsOptions)
            {
                return new CheckoutsAction(checkoutsOptions.Shop, 
                    checkoutsOptions.AccessToken,
                    DateTimeOffsetHelper.ParseCustom(checkoutsOptions.Since),
                    DateTimeOffsetHelper.ParseCustom(checkoutsOptions.Until));
            }

            if (options is ProductsOptions productsOptions)
            {
                return new ProductsAction(productsOptions.Shop,
                    productsOptions.AccessToken,
                    DateTimeOffsetHelper.ParseCustom(productsOptions.Since),
                    DateTimeOffsetHelper.ParseCustom(productsOptions.Until),
                    productsOptions.PublishedOnly);
            }

            return null;
        }
    }
}
