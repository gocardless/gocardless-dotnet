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
    /// Service class for working with payment account resources.
    ///
    /// Access the details of bank accounts provided for you by GoCardless that
    /// are used to fund [Outbound Payments](#core-endpoints-outbound-payments).
    /// </summary>
    public class PaymentAccountService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public PaymentAccountService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Retrieves the details of an existing payment account.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "BA".</param>
        /// <param name="request">An optional `PaymentAccountGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single payment account resource</returns>
        public Task<PaymentAccountResponse> GetAsync(
            string identity,
            PaymentAccountGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentAccountGetRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PaymentAccountResponse>(
                "GET",
                "/payment_accounts/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your payment accounts.
        /// </summary>
        /// <param name="request">An optional `PaymentAccountListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of payment account resources</returns>
        public Task<PaymentAccountListResponse> ListAsync(
            PaymentAccountListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentAccountListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<PaymentAccountListResponse>(
                "GET",
                "/payment_accounts",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of payment accounts.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<PaymentAccount> All(
            PaymentAccountListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentAccountListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.PaymentAccounts)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of payment accounts.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<PaymentAccount>>> AllAsync(
            PaymentAccountListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentAccountListRequest();

            return new TaskEnumerable<IReadOnlyList<PaymentAccount>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.PaymentAccounts, list.Meta?.Cursors?.After);
            });
        }
    }

    /// <summary>
    /// Retrieves the details of an existing payment account.
    /// </summary>
    public class PaymentAccountGetRequest { }

    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// payment accounts.
    /// </summary>
    public class PaymentAccountListRequest
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
    /// An API response for a request returning a single payment account.
    /// </summary>
    public class PaymentAccountResponse : ApiResponse
    {
        /// <summary>
        /// The payment account from the response.
        /// </summary>
        [JsonProperty("payment_accounts")]
        public PaymentAccount PaymentAccount { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of payment accounts.
    /// </summary>
    public class PaymentAccountListResponse : ApiResponse
    {
        /// <summary>
        /// The list of payment accounts from the response.
        /// </summary>
        [JsonProperty("payment_accounts")]
        public IReadOnlyList<PaymentAccount> PaymentAccounts { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
