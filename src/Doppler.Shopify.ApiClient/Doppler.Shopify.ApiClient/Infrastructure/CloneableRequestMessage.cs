using System;
using System.Net;
using System.Net.Http;

namespace Doppler.Shopify.ApiClient.Infrastructure
{
    public class CloneableRequestMessage : HttpRequestMessage
    {
        private SecurityProtocolType _restoreSecurityProtocol;
            
        public CloneableRequestMessage(Uri url, HttpMethod method, HttpContent content = null) : base(method, url)
        {
            _restoreSecurityProtocol = ServicePointManager.SecurityProtocol;

            if (content != null)
            {
                this.Content = content;
            }

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        }

        public CloneableRequestMessage Clone()
        {
            HttpContent newContent = Content;

            if (newContent != null && newContent is JsonContent)
            {
                newContent = (newContent as JsonContent).Clone();
            }

            var cloned = new CloneableRequestMessage(RequestUri, Method, newContent);

            // Copy over the request's headers which includes the access token if set
            foreach (var header in Headers)
            {
                cloned.Headers.Add(header.Key, header.Value);
            }

            return cloned;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ServicePointManager.SecurityProtocol = _restoreSecurityProtocol;
        }
    }
}