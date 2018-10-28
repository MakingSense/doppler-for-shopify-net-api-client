using System.Net.Http;
using System.Threading.Tasks;
using Doppler.Shopify.ApiClient.Infrastructure;

namespace Doppler.Shopify.ApiClient
{
    public class DefaultRequestExecutionPolicy : IRequestExecutionPolicy
    {
        public T Run<T>(CloneableRequestMessage request, ExecuteRequest<T> executeRequest)
        {
            var fullResult = executeRequest(request);

            return fullResult.Result;
        }
    }
}
