using Newtonsoft.Json;

namespace GoCardless.Errors
{
    public class Error : Error.IError, Error.IValidationError
    {
       

        [JsonProperty("reason")]
        public string Reason { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("links")]
        public ErrorLinks Links { get; set; }
        
        
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("request_pointer")]
        public string RequestPointer { get; set; }

        public interface IError
        {
            string Reason { get; }
            string Message { get; }
            ErrorLinks Links { get; }
        }

        public interface IValidationError
        {
            string Field { get; }
            string Message { get; }
            string RequestPointer { get; }
        }
    }
}