﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Doppler.Shopify.ApiClient
{
    public class DopplerIntegrationService
    {
        private static JsonSerializer _serializer = new JsonSerializer { DateParseHandling = DateParseHandling.DateTime };
        private readonly string _dopplerForShopifyBaseUrl = "https://sfy.fromdoppler.com";

        public DopplerIntegrationService()
        {
        }

        public DopplerIntegrationService(string dopplerForShopifyBaseUrl)
        {
            _dopplerForShopifyBaseUrl = dopplerForShopifyBaseUrl;
        }

        public List<DopplerIntegrationShopResult> GetShops(string dopplerApiKey)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(_dopplerForShopifyBaseUrl + "/me/shops");
                client.DefaultRequestHeaders.Add("Authorization", string.Format("token {0}", dopplerApiKey));

                using (var msg = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    var response = client.SendAsync(msg).Result;
                    var rawResult = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new ApplicationException(rawResult);
                    }

                    var reader = new JsonTextReader(new StringReader(rawResult));

                    return _serializer.Deserialize<List<DopplerIntegrationShopResult>>(reader);
                }
            }
        }
    }
}
