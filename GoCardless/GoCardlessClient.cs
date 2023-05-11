using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GoCardless.Errors;
using GoCardless.Exceptions;
using GoCardless.Internals;
using GoCardless.Resources;
using GoCardless.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializerSettings = GoCardless.Internals.JsonSerializerSettings;

namespace GoCardless
{

    /// <summary>
    ///Entry point into the client.
    /// </summary>
    public partial class GoCardlessClient
    {
        /// <summary>
        /// This is the singleton HttpClient used if none is passed in via the .Create factory methods.
        /// See https://msdn.microsoft.com/en-us/library/system.net.http.httpclient(v=vs.110).aspx#Anchor_5
        /// </summary>
        public static HttpClient DefaultHttpClient { get; set; } = new HttpClient();

        public static ushort DefaultNumberOfRetriesOnTimeout { get; set; } = 2;
        public static TimeSpan DefaultWaitBetweenRetries { get; set; } = TimeSpan.FromSeconds(0.5d);

        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly Uri _baseUrl;
        private readonly bool _errorOnIdempotencyConflict;

        private GoCardlessClient(string accessToken, string baseUrl, HttpClient httpClient, bool errorOnIdempotencyConflict)
        {
            this._httpClient = httpClient ?? DefaultHttpClient;
            // Disable ExpectContinue when using the Default Http Client
            if (httpClient == null) {
                this._httpClient.DefaultRequestHeaders.ExpectContinue = false;
            }
            _accessToken = accessToken;
            _baseUrl = new Uri(baseUrl, UriKind.Absolute);
            _errorOnIdempotencyConflict = errorOnIdempotencyConflict;
        }


        /// <summary>
        ///Available environments for this client.
        /// </summary>
        public enum Environment
        {
            /// <summary>
            ///Live environment (base URL https://api.gocardless.com).
            /// </summary>
            LIVE,

            /// <summary>
            ///Sandbox environment (base URL https://api-sandbox.gocardless.com).
            /// </summary>
            SANDBOX

        }

        /// <summary>
        ///Creates an instance of the client in the live environment.
        ///
        ///@param accessToken the access token
        /// </summary>
        public static GoCardlessClient Create(string accessToken)
        {
            return Create(accessToken, Environment.LIVE);
        }

        /// <summary>
        ///Creates an instance of the client in a specified environment.
        ///
        ///@param accessToken the access token
        ///@param environment the environment
        /// </summary>
        public static GoCardlessClient Create(string accessToken, Environment environment, HttpClient httpClient = null)
        {
            return Create(accessToken, GetBaseUrl(environment), httpClient, false);
        }

        /// <summary>
        ///Creates an instance of the client running against a custom URL.
        ///
        ///@param accessToken the access token
        ///@param baseUrl the base URL of the API
        /// </summary>
        public static GoCardlessClient Create(string accessToken, string baseUrl, HttpClient client = null)
        {
            return Create(accessToken, baseUrl, client, false);
        }

        /// <summary>
        ///Creates an instance of the client.
        ///
        ///@param accessToken the access token
        ///@param baseUrl the base URL of the API
        ///@param errorOnIdempotencyConflict the behaviour for Idemptency Key conflicts
        /// </summary>
        public static GoCardlessClient Create(string accessToken, string baseUrl, HttpClient client = null, bool errorOnIdempotencyConflict = false)
        {
            return new GoCardlessClient(accessToken, baseUrl, client, errorOnIdempotencyConflict);
        }

        private static string GetBaseUrl(Environment env)
        {
            switch (env)
            {
                case Environment.LIVE:
                    return "https://api.gocardless.com";
                case Environment.SANDBOX:
                    return "https://api-sandbox.gocardless.com";
            }
            throw new ArgumentException("Unknown environment:" + env);
        }

        internal async Task<T> ExecuteAsync<T>(string method, string path, List<KeyValuePair<string, object>> urlParams,
            object requestParams, Func<string, Task<T>> fetchById, string payloadKey,
            RequestSettings requestSettings)
            where T : ApiResponse
        {
            var numberOfRetries = requestSettings?.NumberOfRetriesOnTimeout ?? DefaultNumberOfRetriesOnTimeout;
            var waitBetweenRetries = requestSettings?.WaitBetweenRetries ?? DefaultWaitBetweenRetries;

            var cancellationTokenSource = new CancellationTokenSource();

            Func<Task<T>> execute = async () =>
            {
                try
                {
                    return await ExecuteAsyncInner<T>(method, path, urlParams, requestParams, payloadKey,
                            requestSettings, cancellationTokenSource)
                        .ConfigureAwait(false);
                }
                catch (InvalidStateException ex)
                    when (ex.Errors.FirstOrDefault()?.Reason == "idempotent_creation_conflict" &&
                          ex.Errors.First().Links?.ContainsKey("conflicting_resource_id") == true)
                {
                    if (_errorOnIdempotencyConflict)
                    {
                        throw;
                    }

                    var conflictingResourceId = ex.Errors.First().Links.FirstOrDefault().Value;
                    return await fetchById(conflictingResourceId)
                        .ConfigureAwait(false);
                }
            };
            for (var i = 0; i < numberOfRetries; i++)
            {
                try
                {
                    return await execute().ConfigureAwait(false);
                }
                catch (TaskCanceledException exception)
                    when (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    // This case represents a timeout, which we want to retry - see
                    // https://stackoverflow.com/questions/10547895/how-can-i-tell-when-httpclient-has-timed-out/32230327
                    await Task.Delay(waitBetweenRetries);
                }
                catch (HttpRequestException exception)
                {
                    // An HttpRequestException is raised when "[t]he request failed due to
                    // an underlying issue such as network connectivity, DNS failure,
                    // server certificate validation or timeout"
                    await Task.Delay(waitBetweenRetries);
                }
            }
            return await execute().ConfigureAwait(false);

        }

        private async Task<T> ExecuteAsyncInner<T>(string method, string path, List<KeyValuePair<string, object>> urlParams, object requestParams,
            string payloadKey, RequestSettings requestSettings, CancellationTokenSource cancellationTokenSource) where T : ApiResponse
        {
            var requestMessage = BuildHttpRequestMessage<T>(method, path, urlParams, requestParams, payloadKey, requestSettings);


            var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationTokenSource.Token).ConfigureAwait(false);
            try
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    var json = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings());

                    if (result == null)
                    {
                        result = (T)Activator.CreateInstance(typeof(T));
                    }

                    result.ResponseMessage = responseMessage;
                    return result;
                }
                else
                {
                    var json = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<ApiErrorResponse>(json, new JsonSerializerSettings());
                    result.ResponseMessage = responseMessage;

                    switch (result.Error.Code)
                    { 
                            case 401:
                                result.Error.Type = ApiErrorType.AUTHENTICATION_FAILED;
                                break;
                            case 403:
                                result.Error.Type = ApiErrorType.INSUFFICIENT_PERMISSIONS;
                                break;
                            case 429:
                                result.Error.Type = ApiErrorType.RATE_LIMIT_REACHED;
                                break;
                    }
                    throw result.ToException();
                }
            }
            catch (JsonException)
            {
                throw new ApiException(new ApiErrorResponse()
                {
                    ResponseMessage = responseMessage,
                    Error = new ApiError()
                    {
                        Code = (int) responseMessage.StatusCode,
                        Type = ApiErrorType.GOCARDLESS,
                        Message = "Something went wrong with this request. Please check the ResponseMessage property."
                    }
                });
            }
        }

        private HttpRequestMessage BuildHttpRequestMessage<T>(string method, string path, List<KeyValuePair<string, object>> urlParams, object requestParams, string payloadKey, RequestSettings requestSettings) where T : ApiResponse
        {
            //insert url arguments into template
            foreach (var arg in urlParams)
            {
                path = path.Replace(":" + arg.Key, Helpers.Stringify(arg.Value));
            }

            //add querystring for GET requests
            if (method == "GET")
            {
                var requestArguments = Helpers.ExtractQueryStringValuesFromObject(requestParams);
                if (requestArguments.Count > 0)
                {
                    var queryString = String.Join("&", requestArguments.Select(Helpers.QueryStringArgument));
                    path += "?" + queryString;
                }
            }

            var httpMethod = new HttpMethod(method);

            var requestMessage = new HttpRequestMessage(httpMethod, new Uri(_baseUrl, path));
            var OSRunningOn = "";
            var runtimeFrameworkInformation = "";

#if NETSTANDARD
            OSRunningOn = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            runtimeFrameworkInformation = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
#endif
#if NET46
            OSRunningOn = System.Environment.OSVersion.VersionString;
            runtimeFrameworkInformation = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
#endif

            var userAgentInformation = $" gocardless-dotnet/5.16.0 {runtimeFrameworkInformation} {Helpers.CleanupOSDescriptionString(OSRunningOn)}";

            requestMessage.Headers.Add("User-Agent", userAgentInformation);
            requestMessage.Headers.Add("GoCardless-Version", "2015-07-06");
            requestMessage.Headers.Add("GoCardless-Client-Version", "5.16.0");
            requestMessage.Headers.Add("GoCardless-Client-Library", "gocardless-dotnet");
            requestMessage.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            //add request body for non-GETs
            if (method != "GET" && requestParams != null && !String.IsNullOrWhiteSpace(payloadKey))
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };
                var serializer = JsonSerializer.Create(settings);

                StringBuilder sb = new StringBuilder();
                using (var sw = new StringWriter(sb))
                {
                    var jo = new JObject();
                    using (var jsonTextWriter = new LinuxLineEndingJsonTextWriter(sw))
                    {
                        serializer.Serialize(jsonTextWriter, requestParams);
                        jo[payloadKey] = JToken.Parse(sb.ToString());
                    }
                    requestMessage.Content = new StringContent(jo.ToString(Formatting.Indented), Encoding.UTF8,
                        "application/json");
                }
            }

            var hasIdempotencyKey = requestParams as IHasIdempotencyKey;

            if (hasIdempotencyKey != null)
            {
                hasIdempotencyKey.IdempotencyKey = hasIdempotencyKey.IdempotencyKey ?? Guid.NewGuid().ToString();
                requestMessage.Headers.TryAddWithoutValidation("Idempotency-Key", hasIdempotencyKey.IdempotencyKey);
            }

            if (requestSettings != null) {
                foreach (var header in requestSettings.Headers)
                {
                    if (requestMessage.Headers.Contains(header.Key)) {
                        requestMessage.Headers.Remove(header.Key);
                    }

                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            requestSettings?.CustomiseRequestMessage?.Invoke(requestMessage);
            return requestMessage;
        }

        class Helpers
        {
            internal static string Stringify(object value)
            {
                if (value is bool)
                    return value.ToString().ToLower();
                if (value is DateTimeOffset)
                    return WebUtility.UrlEncode(((DateTimeOffset?) value).Value.ToString("o"));
                var typeInfo = value.GetType().GetTypeInfo();
                if (typeInfo.IsArray)
                {
                    return String.Join(WebUtility.UrlEncode(","), ((IEnumerable) value).Cast<object>().Select(Stringify));
                }
                if (typeInfo.IsEnum)
                {
                    var memInfo = typeInfo.GetMember(value.ToString());
                    var attr = memInfo[0].GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
                    return attr?.Value;
                }

                return WebUtility.UrlEncode(value.ToString());
            }

            internal static List<KeyValuePair<string, object>> ExtractQueryStringValuesFromObject(object obj)
            {
                if (obj == null) return Enumerable.Empty<KeyValuePair<string, object>>().ToList();

                var args = new List<KeyValuePair<string, object>>();
                var propertyInfos = obj.GetType().GetTypeInfo().GetProperties();

                foreach (var propertyInfo in propertyInfos)
                {
                    var value = propertyInfo.GetValue(obj);
                    if (value != null)
                    {
                        var att = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
                        if (att != null)
                        {
                            var ti = propertyInfo.PropertyType.GetTypeInfo();
                            var isParameterObject = !ti.IsArray && !ti.IsEnum && ti.Namespace.StartsWith("GoCardless");
                            if (isParameterObject)
                            {
                                foreach (var inner in ExtractQueryStringValuesFromObject(value))
                                {
                                    args.Add(new KeyValuePair<string, object>($"{att.PropertyName}[{inner.Key}]",
                                        inner.Value));
                                }
                            }
                            else
                            {
                                args.Add(new KeyValuePair<string, object>(att.PropertyName, value));
                            }
                        }
                    }
                }
                return args;
            }

            internal static string QueryStringArgument(KeyValuePair<string, object> argument)
            {
                var urlEncodedValue = Helpers.Stringify(argument.Value);
                var urlEncodedKey = WebUtility.UrlEncode(argument.Key);
                return $"{urlEncodedKey}={urlEncodedValue}";
            }

            // Necessary due to a bug in User Agent header parser when multiple params passed
            // fixed in later versions of .NET but required until a later date when we upgrade
            internal static string CleanupOSDescriptionString(string input)
            {
                Regex pattern = new Regex("[;:#()~]");
                return pattern.Replace(input, "-");
            }
        }
    }
}
