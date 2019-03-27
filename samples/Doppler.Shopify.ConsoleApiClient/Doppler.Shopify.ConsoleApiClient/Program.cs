using CommandLine;
using System;
using Newtonsoft.Json;

namespace Doppler.Shopify.ConsoleApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<ShopsOptions, CheckoutsOptions, ProductsOptions, SynchronizeCustomersOptions>(args)
                .WithParsed<IOptions>(opts => PrintResultAsJson(ActionFactory.CreateAction(opts).Execute()));
        }

        static void PrintResultAsJson(object o) =>  Console.Write(JsonConvert.SerializeObject(o, Formatting.Indented));
    }
}
