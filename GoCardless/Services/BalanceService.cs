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
    /// Service class for working with balance resources.
    ///
    /// Returns the balances for a creditor. These balances are the same as
    /// what’s shown in the dashboard with one exception (mentioned below under
    /// balance_type).
    ///
    /// These balances will typically be 3-5 minutes old. The balance amounts
    /// likely won’t match what’s shown in the dashboard as the dashboard
    /// balances are updated much less frequently (once per day).
    /// </summary>
    public class BalanceService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public BalanceService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// balances for a given creditor. This endpoint is rate limited to 60
        /// requests per minute.
        /// </summary>
        /// <param name="request">An optional `BalanceListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of balance resources</returns>
        public Task<BalanceListResponse> ListAsync(
            BalanceListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BalanceListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<BalanceListResponse>(
                "GET",
                "/balances",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of balances.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Balance> All(
            BalanceListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BalanceListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Balances)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of balances.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Balance>>> AllAsync(
            BalanceListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BalanceListRequest();

            return new TaskEnumerable<IReadOnlyList<Balance>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Balances, list.Meta?.Cursors?.After);
            });
        }
    }

    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
    /// balances for a given creditor. This endpoint is rate limited to 60
    /// requests per minute.
    /// </summary>
    public class BalanceListRequest
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
        /// ID of a [creditor](#core-endpoints-creditors).
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single balance.
    /// </summary>
    public class BalanceResponse : ApiResponse
    {
        /// <summary>
        /// The balance from the response.
        /// </summary>
        [JsonProperty("balances")]
        public Balance Balance { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of balances.
    /// </summary>
    public class BalanceListResponse : ApiResponse
    {
        /// <summary>
        /// The list of balances from the response.
        /// </summary>
        [JsonProperty("balances")]
        public IReadOnlyList<Balance> Balances { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
