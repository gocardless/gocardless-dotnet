using Newtonsoft.Json;
using System.Net.Http;

namespace GoCardless.Resources
{
    public abstract class ApiResponse
    {
        [JsonIgnore]
        public HttpResponseMessage ResponseMessage { get; internal set; }
    }
}