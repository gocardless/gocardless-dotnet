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
    /// Service class for working with payment account transaction resources.
    ///
    ///  Payment account transactions represent movements of funds on a given
    ///  payment account. The payment account is provisioned by GoCardless and
    ///  is used to fund [outbound payments](#core-endpoints-outbound-payments).
    /// </summary>
    public class PaymentAccountTransactionService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public PaymentAccountTransactionService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        ///  List transactions for a given payment account.
        /// </summary>
        ///  <param name="identity">The unique ID of the [bank
        ///  account](#core-endpoints-creditor-bank-accounts) which happens to be the payment
        ///  account.</param>
        /// <param name="request">An optional `PaymentAccountTransactionListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of payment account transaction resources</returns>
        public Task<PaymentAccountTransactionListResponse> ListAsync(
            string identity,
            PaymentAccountTransactionListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentAccountTransactionListRequest();
            if (identity == null)
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<PaymentAccountTransactionListResponse>(
                "GET",
                "/payment_accounts/:identity/transactions",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of payment account transactions.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        ///  <param name="identity">The unique ID of the [bank
        ///  account](#core-endpoints-creditor-bank-accounts) which happens to be the payment
        ///  account.</param>
        public IEnumerable<PaymentAccountTransaction> All(
            string identity,
            PaymentAccountTransactionListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentAccountTransactionListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() =>
                    ListAsync(identity, request, customiseRequestMessage)
                ).Result;
                foreach (var item in result.PaymentAccountTransactions)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of payment account transactions.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        ///  <param name="identity">The unique ID of the [bank
        ///  account](#core-endpoints-creditor-bank-accounts) which happens to be the payment
        ///  account.</param>
        public IEnumerable<Task<IReadOnlyList<PaymentAccountTransaction>>> AllAsync(
            string identity,
            PaymentAccountTransactionListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PaymentAccountTransactionListRequest();

            return new TaskEnumerable<IReadOnlyList<PaymentAccountTransaction>, string>(
                async after =>
                {
                    request.After = after;
                    var list = await this.ListAsync(identity, request, customiseRequestMessage);
                    return Tuple.Create(list.PaymentAccountTransactions, list.Meta?.Cursors?.After);
                }
            );
        }
    }

    /// <summary>
    ///  List transactions for a given payment account.
    /// </summary>
    public class PaymentAccountTransactionListRequest
    {
        /// <summary>
        ///  Cursor pointing to the start of the desired set.
        /// </summary>
        [JsonProperty("after")]
        public string After { get; set; }

        /// <summary>
        ///  Cursor pointing to the end of the desired set.
        /// </summary>
        [JsonProperty("before")]
        public string Before { get; set; }

        /// <summary>
        ///  The direction of the transaction. Debits mean money leaving the
        ///  account (e.g. outbound payment), while credits signify money coming
        ///  in (e.g. manual top-up).
        /// </summary>
        [JsonProperty("direction")]
        public PaymentAccountTransactionDirection? Direction { get; set; }

        /// <summary>
        ///  The direction of the transaction. Debits mean money leaving the
        ///  account (e.g. outbound payment), while credits signify money coming
        ///  in (e.g. manual top-up).
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PaymentAccountTransactionDirection
        {
            /// <summary>`direction` with a value of "credit"</summary>
            [EnumMember(Value = "credit")]
            Credit,

            /// <summary>`direction` with a value of "debit"</summary>
            [EnumMember(Value = "debit")]
            Debit,
        }

        /// <summary>
        ///  Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        ///  The beginning of query period
        /// </summary>
        [JsonProperty("value_date_from")]
        public string ValueDateFrom { get; set; }

        /// <summary>
        ///  The end of query period
        /// </summary>
        [JsonProperty("value_date_to")]
        public string ValueDateTo { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single payment account transaction.
    /// </summary>
    public class PaymentAccountTransactionResponse : ApiResponse
    {
        /// <summary>
        /// The payment account transaction from the response.
        /// </summary>
        [JsonProperty("payment_account_transactions")]
        public PaymentAccountTransaction PaymentAccountTransaction { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of payment account transactions.
    /// </summary>
    public class PaymentAccountTransactionListResponse : ApiResponse
    {
        /// <summary>
        /// The list of payment account transactions from the response.
        /// </summary>
        [JsonProperty("payment_account_transactions")]
        public IReadOnlyList<PaymentAccountTransaction> PaymentAccountTransactions
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
