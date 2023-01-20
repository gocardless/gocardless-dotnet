

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GoCardless.Internals;
using GoCardless.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Services
{
    /// <summary>
    /// Service class for working with scheme identifier resources.
    ///
    /// Scheme identifiers
    /// </summary>

    public class SchemeIdentifierService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public SchemeIdentifierService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your scheme identifiers.
        /// </summary>
        /// <param name="request">An optional `SchemeIdentifierListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of scheme identifier resources</returns>
        public Task<SchemeIdentifierListResponse> ListAsync(SchemeIdentifierListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new SchemeIdentifierListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<SchemeIdentifierListResponse>("GET", "/scheme_identifiers", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of scheme identifiers.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<SchemeIdentifier> All(SchemeIdentifierListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new SchemeIdentifierListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.SchemeIdentifiers)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of scheme identifiers.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<SchemeIdentifier>>> AllAsync(SchemeIdentifierListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new SchemeIdentifierListRequest();

            return new TaskEnumerable<IReadOnlyList<SchemeIdentifier>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.SchemeIdentifiers, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of an existing scheme identifier.
        /// </summary>  
        /// <param name="identity">Unique identifier, usually beginning with "SU".</param> 
        /// <param name="request">An optional `SchemeIdentifierGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single scheme identifier resource</returns>
        public Task<SchemeIdentifierResponse> GetAsync(string identity, SchemeIdentifierGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new SchemeIdentifierGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<SchemeIdentifierResponse>("GET", "/scheme_identifiers/:identity", urlParams, request, null, null, customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// scheme identifiers.
    /// </summary>
    public class SchemeIdentifierListRequest
    {

        /// <summary>
        /// Cursor pointing to the start of the desired set.
        /// </summary>
        [JsonProperty("after")]
        public string After { get; set; }

        /// <summary>
        /// Cursor pointing to the end of the desired set.
        /// </summary>
        [JsonProperty("before")]
        public string Before { get; set; }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

        
    /// <summary>
    /// Retrieves the details of an existing scheme identifier.
    /// </summary>
    public class SchemeIdentifierGetRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single scheme identifier.
    /// </summary>
    public class SchemeIdentifierResponse : ApiResponse
    {
        /// <summary>
        /// The scheme identifier from the response.
        /// </summary>
        [JsonProperty("scheme_identifiers")]
        public SchemeIdentifier SchemeIdentifier { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of scheme identifiers.
    /// </summary>
    public class SchemeIdentifierListResponse : ApiResponse
    {
        /// <summary>
        /// The list of scheme identifiers from the response.
        /// </summary>
        [JsonProperty("scheme_identifiers")]
        public IReadOnlyList<SchemeIdentifier> SchemeIdentifiers { get; private set; }
        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
