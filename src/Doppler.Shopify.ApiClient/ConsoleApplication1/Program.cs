using Doppler.Shopify.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Note that this method differes from Validates_Web_Requests() in that the aforementioned's dictionary is Dictionary<string, stringvalues> and this is Dictionary<string, string>.
            var qs = new Dictionary<string, string>()
            {
                {"hmac", "1f2b427c24b6a0e2a9004a71ea16d3d5a213db5058218e57e9d5fb44d601b903"},
                {"shop", "doppler-dev01.myshopify.com"},
                {"code", "cda84da98ee07b1231068a6ba51f7101"},
                {"timestamp", "1540772285"},
            };

            bool isValid = AuthorizationService.IsAuthenticRequest(qs, "3467219a2f96ab7d2e95b8b9b3cd0514");
        }
    }
}
