using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShopifySharp.Infrastructure
{
    public class RequestUri
    {
        public RequestUri(Uri uri)
        {
            Url = uri;
            QueryParams = new Dictionary<string, object>();
        }

        private Uri Url;

        public Dictionary<string, object> QueryParams { get; private set; }

        public Uri ToUri()
        {
            // Combine the url and the query param dictionary into a uri
            var query = QueryParams.Select(kvp =>
            {
                return string.Format("{0}={1}",kvp.Key, Uri.EscapeDataString(kvp.Value.ToString()));
            });
            var ub = new UriBuilder(Url)
            {
                Query = string.Join("&", query)
            };

            return ub.Uri;
        }

        public override string ToString()
        {
            return ToUri().ToString();
        }
    }
}