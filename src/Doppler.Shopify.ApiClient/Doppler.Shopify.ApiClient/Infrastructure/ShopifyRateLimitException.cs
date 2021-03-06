﻿using System.Collections.Generic;
using System.Net;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// An exception thrown when an API call has reached Shopify's rate limit.
    /// </summary>
    public class ShopifyRateLimitException : ShopifyException
    {
        public ShopifyRateLimitException() : base() { }

        public ShopifyRateLimitException(string message): base(message) { }

        public ShopifyRateLimitException(HttpStatusCode httpStatusCode, Dictionary<string, IEnumerable<string>> errors, string message, string jsonError, string requestId) : base(httpStatusCode, errors, message, jsonError, requestId) { }
    }
}
