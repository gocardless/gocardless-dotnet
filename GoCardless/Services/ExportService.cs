

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
    /// Service class for working with export resources.
    ///
    /// File-based exports of data
    /// </summary>

    public class ExportService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public ExportService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a single export.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "EX".</param> 
        /// <param name="request">An optional `ExportGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single export resource</returns>
        public Task<ExportResponse> GetAsync(string identity, ExportGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new ExportGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<ExportResponse>("GET", "/exports/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Returns a list of exports which are available for download.
        /// </summary>
        /// <param name="request">An optional `ExportListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of export resources</returns>
        public Task<ExportListResponse> ListAsync(ExportListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new ExportListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<ExportListResponse>("GET", "/exports", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of exports.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Export> All(ExportListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new ExportListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Exports)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of exports.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Export>>> AllAsync(ExportListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new ExportListRequest();

            return new TaskEnumerable<IReadOnlyList<Export>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Exports, list.Meta?.Cursors?.After);
            });
        }
    }

        
    /// <summary>
    /// Returns a single export.
    /// </summary>
    public class ExportGetRequest
    {
    }

        
    /// <summary>
    /// Returns a list of exports which are available for download.
    /// </summary>
    public class ExportListRequest
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
    /// An API response for a request returning a single export.
    /// </summary>
    public class ExportResponse : ApiResponse
    {
        /// <summary>
        /// The export from the response.
        /// </summary>
        [JsonProperty("exports")]
        public Export Export { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of exports.
    /// </summary>
    public class ExportListResponse : ApiResponse
    {
        /// <summary>
        /// The list of exports from the response.
        /// </summary>
        [JsonProperty("exports")]
        public IReadOnlyList<Export> Exports { get; private set; }
        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
