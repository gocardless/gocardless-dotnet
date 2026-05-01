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
    /// Service class for working with outbound payment import entry resources.
    ///
    /// Import Entries are the individual rows of an outbound payment import,
    /// representing each payment to be created.
    /// </summary>
    public class OutboundPaymentImportEntryService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public OutboundPaymentImportEntryService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// the entries for a given outbound payment import.
        /// </summary>
        /// <param name="request">An optional `OutboundPaymentImportEntryListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of outbound payment import entry resources</returns>
        public Task<OutboundPaymentImportEntryListResponse> ListAsync(
            OutboundPaymentImportEntryListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new OutboundPaymentImportEntryListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<OutboundPaymentImportEntryListResponse>(
                "GET",
                "/outbound_payment_import_entries",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of outbound payment import entries.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<OutboundPaymentImportEntry> All(
            OutboundPaymentImportEntryListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new OutboundPaymentImportEntryListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.OutboundPaymentImportEntries)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of outbound payment import entries.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<OutboundPaymentImportEntry>>> AllAsync(
            OutboundPaymentImportEntryListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new OutboundPaymentImportEntryListRequest();

            return new TaskEnumerable<IReadOnlyList<OutboundPaymentImportEntry>, string>(
                async after =>
                {
                    request.After = after;
                    var list = await this.ListAsync(request, customiseRequestMessage);
                    return Tuple.Create(
                        list.OutboundPaymentImportEntries,
                        list.Meta?.Cursors?.After
                    );
                }
            );
        }
    }

    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of the
    /// entries for a given outbound payment import.
    /// </summary>
    public class OutboundPaymentImportEntryListRequest
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

        /// <summary>
        /// Unique identifier, beginning with "IM".
        /// </summary>
        [JsonProperty("outbound_payment_import")]
        public string OutboundPaymentImport { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single outbound payment import entry.
    /// </summary>
    public class OutboundPaymentImportEntryResponse : ApiResponse
    {
        /// <summary>
        /// The outbound payment import entry from the response.
        /// </summary>
        [JsonProperty("outbound_payment_import_entries")]
        public OutboundPaymentImportEntry OutboundPaymentImportEntry { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of outbound payment import entries.
    /// </summary>
    public class OutboundPaymentImportEntryListResponse : ApiResponse
    {
        /// <summary>
        /// The list of outbound payment import entries from the response.
        /// </summary>
        [JsonProperty("outbound_payment_import_entries")]
        public IReadOnlyList<OutboundPaymentImportEntry> OutboundPaymentImportEntries
        {
            get;
            private set;
        }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
