using System;
using System.Collections.Generic;

namespace Doppler.Shopify.ApiClient.Tests
{
    /// <summary>
    /// A utility class for running tests.
    /// </summary>
    public static class Utils
    {
        static Utils()
        {
            // Console.WriteLine("DIRECTORY: " + System.IO.Directory.GetCurrentDirectory());
            // dotEnvFile = DotEnvFile.DotEnvFile.LoadFile("env.yml");
        }

        /// <summary>
        /// Attempts to get an environment variable first by the key, then by 'SHOPIFYSHARP_{KEY}'. All keys must be uppercased!
        /// </summary>
        private static string Get(string key)
        {
            key = key.ToUpper();

            string prefix = "DOPPLER_FOR_SHOPIFY_";
            string value = Environment.GetEnvironmentVariable(key) ?? Environment.GetEnvironmentVariable(prefix + key);

            if (string.IsNullOrEmpty(value))
            {
                throw new Exception(string.Format("{key {0} was not found in environment variables. Add the key or {1} to your environment variables and try again.", key, prefix + key));
            }

            return value;
        }

        public static string ApiKey { get { return Get("API_KEY"); } }

        public static string SecretKey { get { return Get("SECRET_KEY"); } }

        public static string AccessToken { get { return Get("ACCESS_TOKEN"); } }

        public static string MyShopifyUrl { get { return Get("MY_SHOPIFY_URL"); } }
    }
}
