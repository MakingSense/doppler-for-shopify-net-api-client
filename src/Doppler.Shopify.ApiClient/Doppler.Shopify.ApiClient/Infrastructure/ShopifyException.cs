﻿using System;
using System.Collections.Generic;
using System.Net;

namespace Doppler.Shopify.ApiClient
{
    public class ShopifyException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// The XRequestId header returned by Shopify. Can be used when working with the Shopify support team to identify the failed request.
        /// </summary>
        public string RequestId { get; set; }

        /// <remarks>
        /// Dictionary is always initialized to ensure null reference errors won't be thrown when trying to check error messages.
        /// </remarks>
        public Dictionary<string, IEnumerable<string>> Errors { get; set; }

        /// <summary>
        /// The raw JSON string returned by Shopify.
        /// </summary>
        public string RawBody { get; set; }

        public ShopifyException() 
        {
            Errors = new Dictionary<string, IEnumerable<string>>();
        }

        public ShopifyException(string message) : base(message) 
        { 
            Errors = new Dictionary<string, IEnumerable<string>>();
        }

        public ShopifyException(HttpStatusCode httpStatusCode, Dictionary<string, IEnumerable<string>> errors, string message, string rawBody, string requestId) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            Errors = errors;
            RawBody = rawBody;
            RequestId = requestId;
            Errors = new Dictionary<string, IEnumerable<string>>();
        }
    }
}
