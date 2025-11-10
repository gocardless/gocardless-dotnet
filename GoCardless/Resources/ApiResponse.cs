using System.Net.Http;
using Newtonsoft.Json;

namespace GoCardless.Resources
{
    public abstract class ApiResponse
    {
        [JsonIgnore]
        public HttpResponseMessage ResponseMessage { get; internal set; }
    }
}
