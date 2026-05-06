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
    /// Service class for working with outbound payment import resources.
    ///
    /// Outbound Payment Imports allow you to create multiple payments via a
    /// single API call.
    ///
    /// The Workflow:
    /// 1. Create the outbound payment import.
    /// 2. Retrieve an authorisation link from the response.
    /// 3. Redirect the user to the link to authorise the import.
    /// 4. Once the user authorises the import, the individual outbound payments
    /// are automatically submitted.
    ///
    /// Import entries are not processed as actual payments until they are
    /// reviewed and authorised in GoCardless Dashboard.
    /// Upon approval, a unique outbound payment is generated for every entry in
    /// the import.
    ///
    /// <p class="notice">Outbound Payment Imports are capped at 1000 entries.
    /// If you expect to exceed this limit, please create multiple smaller
    /// imports.</p>
    /// </summary>
    public class OutboundPaymentImportService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public OutboundPaymentImportService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request">An optional `OutboundPaymentImportCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single outbound payment import resource</returns>
        public Task<OutboundPaymentImportResponse> CreateAsync(
            OutboundPaymentImportCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new OutboundPaymentImportCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<OutboundPaymentImportResponse>(
                "POST",
                "/outbound_payment_imports",
                urlParams,
                request,
                id => GetAsync(id, null, customiseRequestMessage),
                "outbound_payment_imports",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Returns a single outbound payment import.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "IM".</param>
        /// <param name="request">An optional `OutboundPaymentImportGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single outbound payment import resource</returns>
        public Task<OutboundPaymentImportResponse> GetAsync(
            string identity,
            OutboundPaymentImportGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new OutboundPaymentImportGetRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<OutboundPaymentImportResponse>(
                "GET",
                "/outbound_payment_imports/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your outbound payment imports.
        /// </summary>
        /// <param name="request">An optional `OutboundPaymentImportListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of outbound payment import resources</returns>
        public Task<OutboundPaymentImportListResponse> ListAsync(
            OutboundPaymentImportListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new OutboundPaymentImportListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<OutboundPaymentImportListResponse>(
                "GET",
                "/outbound_payment_imports",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of outbound payment imports.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<OutboundPaymentImport> All(
            OutboundPaymentImportListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new OutboundPaymentImportListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.OutboundPaymentImports)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of outbound payment imports.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<OutboundPaymentImport>>> AllAsync(
            OutboundPaymentImportListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new OutboundPaymentImportListRequest();

            return new TaskEnumerable<IReadOnlyList<OutboundPaymentImport>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.OutboundPaymentImports, list.Meta?.Cursors?.After);
            });
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OutboundPaymentImportCreateRequest : IHasIdempotencyKey
    {
        [JsonProperty("entry_items")]
        public OutboundPaymentImportEntryItems[] EntryItems { get; set; }

        /// <summary>
        /// The entry items contain attributes required to [Create an outbound
        /// payment](#outbound-payments-create-an-outbound-payment).
        /// Please refer to that documentation for more information on the
        /// requirements for these fields.
        ///
        /// <b>Required</b>:
        /// <ul>
        /// <li>`amount`</li>
        /// <li>`scheme`</li>
        /// <li>`recipient_bank_account_id`</li>
        /// </ul>
        ///
        /// Additional supported fields:
        /// <ul>
        /// <li>`reference`</li>
        /// <li>`metadata`</li>
        /// </ul>
        ///
        /// </summary>
        public class OutboundPaymentImportEntryItems
        {
            /// <summary>
            /// Amount, in the lowest denomination for the currency (e.g. pence
            /// in GBP, cents in EUR).
            /// </summary>
            [JsonProperty("amount")]
            public int? Amount { get; set; }

            /// <summary>
            /// Key-value store of custom data. Up to 3 keys are permitted, with
            /// key names up to 50 characters and values up to 500 characters.
            /// </summary>
            [JsonProperty("metadata")]
            public IDictionary<string, string> Metadata { get; set; }

            /// <summary>
            /// ID of the customer bank account which receives the outbound
            /// payment.
            /// </summary>
            [JsonProperty("recipient_bank_account_id")]
            public string RecipientBankAccountId { get; set; }

            /// <summary>
            /// An optional reference that will appear on your customer's bank
            /// statement.
            /// The character limit for this reference is dependent on the
            /// scheme.<br />
            /// <strong>Faster Payments</strong> - 18 characters, including:
            /// "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789
            /// &-./"<br />
            /// </summary>
            [JsonProperty("reference")]
            public string Reference { get; set; }

            /// <summary>
            /// Bank payment scheme to process the outbound payment. Currently
            /// only "faster_payments" (GBP) is supported.
            /// </summary>
            [JsonProperty("scheme")]
            public OutboundPaymentImportScheme? Scheme { get; set; }

            /// <summary>
            /// Bank payment scheme to process the outbound payment. Currently only
            /// "faster_payments" (GBP) is supported.
            /// </summary>
            [JsonConverter(typeof(StringEnumConverter))]
            public enum OutboundPaymentImportScheme
            {
                /// <summary>`scheme` with a value of "faster_payments"</summary>
                [EnumMember(Value = "faster_payments")]
                FasterPayments,
            }
        }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public OutboundPaymentImportLinks Links { get; set; }

        /// <summary>
        /// Linked resources for a OutboundPaymentImport.
        /// </summary>
        public class OutboundPaymentImportLinks
        {
            /// <summary>
            /// ID of the creditor who sends the outbound payments from the
            /// import.
            /// </summary>
            [JsonProperty("creditor")]
            public string Creditor { get; set; }
        }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

    /// <summary>
    /// Returns a single outbound payment import.
    /// </summary>
    public class OutboundPaymentImportGetRequest { }

    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// outbound payment imports.
    /// </summary>
    public class OutboundPaymentImportListRequest
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
        /// Limit to records created within certain times.
        /// </summary>
        [JsonProperty("created_at")]
        public CreatedAtParam CreatedAt { get; set; }

        /// <summary>
        /// Specify filters to limit records by creation time.
        /// </summary>
        public class CreatedAtParam
        {
            /// <summary>
            /// Limit to records created after the specified date-time.
            /// </summary>
            [JsonProperty("gt")]
            public DateTimeOffset? GreaterThan { get; set; }

            /// <summary>
            /// Limit to records created on or after the specified date-time.
            /// </summary>
            [JsonProperty("gte")]
            public DateTimeOffset? GreaterThanOrEqual { get; set; }

            /// <summary>
            /// Limit to records created before the specified date-time.
            /// </summary>
            [JsonProperty("lt")]
            public DateTimeOffset? LessThan { get; set; }

            /// <summary>
            /// Limit to records created on or before the specified date-time.
            /// </summary>
            [JsonProperty("lte")]
            public DateTimeOffset? LessThanOrEqual { get; set; }
        }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// The status of the outbound payment import.
        /// <ul>
        /// <li>`created`: The initial state of a new import.</li>
        /// <li>`validating`: Import validation in progress.</li>
        /// <li>`invalid`: Import validation failed.</li>
        /// <li>`valid`: Import validation succeeded.</li>
        /// <li>`processing`: Authorisation received; payments are being
        /// generated.</li>
        /// <li>`processed`: All entries have been successfully converted into
        /// outbound payments.</li>
        /// <li>`cancelled`: The import was cancelled by a user or automatically
        /// expired by the system.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public OutboundPaymentImportStatus? Status { get; set; }

        /// <summary>
        /// The status of the outbound payment import.
        /// <ul>
        /// <li>`created`: The initial state of a new import.</li>
        /// <li>`validating`: Import validation in progress.</li>
        /// <li>`invalid`: Import validation failed.</li>
        /// <li>`valid`: Import validation succeeded.</li>
        /// <li>`processing`: Authorisation received; payments are being
        /// generated.</li>
        /// <li>`processed`: All entries have been successfully converted into
        /// outbound payments.</li>
        /// <li>`cancelled`: The import was cancelled by a user or automatically
        /// expired by the system.</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum OutboundPaymentImportStatus
        {
            /// <summary>`status` with a value of "created"</summary>
            [EnumMember(Value = "created")]
            Created,

            /// <summary>`status` with a value of "validating"</summary>
            [EnumMember(Value = "validating")]
            Validating,

            /// <summary>`status` with a value of "valid"</summary>
            [EnumMember(Value = "valid")]
            Valid,

            /// <summary>`status` with a value of "invalid"</summary>
            [EnumMember(Value = "invalid")]
            Invalid,

            /// <summary>`status` with a value of "processing"</summary>
            [EnumMember(Value = "processing")]
            Processing,

            /// <summary>`status` with a value of "processed"</summary>
            [EnumMember(Value = "processed")]
            Processed,

            /// <summary>`status` with a value of "cancelled"</summary>
            [EnumMember(Value = "cancelled")]
            Cancelled,
        }
    }

    /// <summary>
    /// An API response for a request returning a single outbound payment import.
    /// </summary>
    public class OutboundPaymentImportResponse : ApiResponse
    {
        /// <summary>
        /// The outbound payment import from the response.
        /// </summary>
        [JsonProperty("outbound_payment_imports")]
        public OutboundPaymentImport OutboundPaymentImport { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of outbound payment imports.
    /// </summary>
    public class OutboundPaymentImportListResponse : ApiResponse
    {
        /// <summary>
        /// The list of outbound payment imports from the response.
        /// </summary>
        [JsonProperty("outbound_payment_imports")]
        public IReadOnlyList<OutboundPaymentImport> OutboundPaymentImports { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
