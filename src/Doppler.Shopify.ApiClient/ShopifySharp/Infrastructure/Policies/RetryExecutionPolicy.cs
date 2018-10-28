using System;
using System.Net.Http;
using System.Threading.Tasks;
using ShopifySharp.Infrastructure;
using System.Threading;

namespace ShopifySharp
{
    /// <summary>
    /// See https://help.shopify.com/api/guides/api-call-limit
    /// </summary>
    public class RetryExecutionPolicy : IRequestExecutionPolicy
    {
        private static readonly TimeSpan RETRY_DELAY = TimeSpan.FromMilliseconds(500);

        public T Run<T>(CloneableRequestMessage baseRequest, ExecuteRequest<T> executeRequest)
        {
            while (true)
            {
                var request = baseRequest.Clone();

                try
                {
                    var fullResult = executeRequest(request);

                    return fullResult.Result;
                }
                catch (ShopifyRateLimitException)
                {
                    Thread.Sleep(RETRY_DELAY);
                }
            }
        }
    }
}
