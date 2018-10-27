using System.Net.Http;

namespace ShopifySharp
{
    public class RequestResult<T>
    {
        public HttpResponseMessage Response { get; private set;  }

        public T Result { get; private set; }

        public string RawResult { get; private set; }

        public RequestResult(HttpResponseMessage response, T result, string rawResult)
        {
            this.Response = response;
            this.Result = result;
            this.RawResult = rawResult;
        }
    }
}
