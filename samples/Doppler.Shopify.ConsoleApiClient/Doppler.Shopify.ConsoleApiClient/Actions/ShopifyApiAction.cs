namespace Doppler.Shopify.ConsoleApiClient.Actions
{
    abstract class ShopifyApiAction : IAction
    {
        protected readonly string _shop;
        protected string _accessToken { get; }

        public ShopifyApiAction(string shop, string accessToken)
        {
            _shop = shop;
            _accessToken = accessToken;
        }

        public abstract object Execute();
    }
}
