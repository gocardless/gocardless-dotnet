

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
    /// Service class for working with outbound payment resources.
    ///
    /// Outbound Payments represent payments sent from
    /// [creditors](#core-endpoints-creditors).
    /// 
    /// GoCardless will notify you via a [webhook](#appendix-webhooks) when the
    /// status of the outbound payment [changes](#event-types-outbound-payment).
    /// 
    /// <p class="restricted-notice"><strong>Restricted</strong>: Outbound
    /// Payments are currently in Early Access and available only to a limited
    /// list of organisations. If you are interested in using this feature,
    /// please stay tuned for our public launch announcement. We are actively
    /// testing and refining our API to ensure it meets your needs and provides
    /// the best experience.</p>
    /// </summary>

    public class OutboundPaymentService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public OutboundPaymentService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">An optional `OutboundPaymentCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single outbound payment resource</returns>
        public Task<OutboundPaymentResponse> CreateAsync(OutboundPaymentCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new OutboundPaymentCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<OutboundPaymentResponse>("POST", "/outbound_payments", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "outbound_payments", customiseRequestMessage);
        }

        /// <summary>
        /// Creates an outbound payment to your verified business bank account
        /// as the recipient.
        /// </summary>
        /// <param name="request">An optional `OutboundPaymentWithdrawRequest` representing the body for this withdraw request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single outbound payment resource</returns>
        public Task<OutboundPaymentResponse> WithdrawAsync(OutboundPaymentWithdrawRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new OutboundPaymentWithdrawRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<OutboundPaymentResponse>("POST", "/outbound_payments/withdrawal", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// Cancels an outbound payment. Only outbound payments with either
        /// `verifying`, `pending_approval`, or `scheduled` status can be
        /// cancelled.
        /// Once an outbound payment is `executing`, the money moving process
        /// has begun and cannot be reversed.
        /// </summary>  
        /// <param name="identity">Unique identifier of the outbound payment.</param> 
        /// <param name="request">An optional `OutboundPaymentCancelRequest` representing the body for this cancel request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single outbound payment resource</returns>
        public Task<OutboundPaymentResponse> CancelAsync(string identity, OutboundPaymentCancelRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new OutboundPaymentCancelRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<OutboundPaymentResponse>("POST", "/outbound_payments/:identity/actions/cancel", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// Approves an outbound payment. Only outbound payments with the
        /// “pending_approval” status can be approved.
        /// </summary>  
        /// <param name="identity">Unique identifier of the outbound payment.</param> 
        /// <param name="request">An optional `OutboundPaymentApproveRequest` representing the body for this approve request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single outbound payment resource</returns>
        public Task<OutboundPaymentResponse> ApproveAsync(string identity, OutboundPaymentApproveRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new OutboundPaymentApproveRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<OutboundPaymentResponse>("POST", "/outbound_payments/:identity/actions/approve", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// Fetches an outbound_payment by ID
        /// </summary>  
        /// <param name="identity">Unique identifier of the outbound payment.</param> 
        /// <param name="request">An optional `OutboundPaymentGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single outbound payment resource</returns>
        public Task<OutboundPaymentResponse> GetAsync(string identity, OutboundPaymentGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new OutboundPaymentGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<OutboundPaymentResponse>("GET", "/outbound_payments/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// outbound payments.
        /// </summary>
        /// <param name="request">An optional `OutboundPaymentListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of outbound payment resources</returns>
        public Task<OutboundPaymentListResponse> ListAsync(OutboundPaymentListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new OutboundPaymentListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<OutboundPaymentListResponse>("GET", "/outbound_payments", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of outbound payments.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<OutboundPayment> All(OutboundPaymentListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new OutboundPaymentListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.OutboundPayments)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of outbound payments.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<OutboundPayment>>> AllAsync(OutboundPaymentListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new OutboundPaymentListRequest();

            return new TaskEnumerable<IReadOnlyList<OutboundPayment>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.OutboundPayments, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Updates an outbound payment object. This accepts only the metadata
        /// parameter.
        /// </summary>  
        /// <param name="identity">Unique identifier of the outbound payment.</param> 
        /// <param name="request">An optional `OutboundPaymentUpdateRequest` representing the body for this update request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single outbound payment resource</returns>
        public Task<OutboundPaymentResponse> UpdateAsync(string identity, OutboundPaymentUpdateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new OutboundPaymentUpdateRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<OutboundPaymentResponse>("PUT", "/outbound_payments/:identity", urlParams, request, null, "outbound_payments", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// 
    /// </summary>
    public class OutboundPaymentCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// Amount, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// A human-readable description of the outbound payment
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// A future date on which the outbound payment should be sent.
        /// If not specified, the payment will be sent as soon as possible.
        /// </summary>
        [JsonProperty("execution_date")]
        public string ExecutionDate { get; set; }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public OutboundPaymentLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a OutboundPayment.
        /// </summary>
        public class OutboundPaymentLinks
        {
                
                /// <summary>
                            /// ID of the creditor who sends the outbound payment.
                /// </summary>
                [JsonProperty("creditor")]
                public string Creditor { get; set; }
                
                /// <summary>
                            /// ID of the customer bank account which receives the outbound
            /// payment.
                /// </summary>
                [JsonProperty("recipient_bank_account")]
                public string RecipientBankAccount { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with
        /// key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

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
        /// Bank payment scheme to process the outbound payment. Currently only
        /// "faster_payments" (GBP) is supported.
        /// </summary>
        [JsonProperty("scheme")]
        public OutboundPaymentScheme? Scheme { get; set; }
            
        /// <summary>
        /// Bank payment scheme to process the outbound payment. Currently only
        /// "faster_payments" (GBP) is supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum OutboundPaymentScheme
        {
    
            /// <summary>`scheme` with a value of "faster_payments"</summary>
            [EnumMember(Value = "faster_payments")]
            FasterPayments,
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
    /// Creates an outbound payment to your verified business bank account as
    /// the recipient.
    /// </summary>
    public class OutboundPaymentWithdrawRequest
    {

        /// <summary>
        /// Amount, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// A human-readable description of the outbound payment
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// A future date on which the outbound payment should be sent.
        /// If not specified, the payment will be sent as soon as possible.
        /// </summary>
        [JsonProperty("execution_date")]
        public string ExecutionDate { get; set; }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public OutboundPaymentLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a OutboundPayment.
        /// </summary>
        public class OutboundPaymentLinks
        {
                
                /// <summary>
                            /// ID of the creditor who sends the outbound payment.
                /// </summary>
                [JsonProperty("creditor")]
                public string Creditor { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with
        /// key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

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
        /// Bank payment scheme to process the outbound payment. Currently only
        /// "faster_payments" (GBP) is supported.
        /// </summary>
        [JsonProperty("scheme")]
        public OutboundPaymentScheme? Scheme { get; set; }
            
        /// <summary>
        /// Bank payment scheme to process the outbound payment. Currently only
        /// "faster_payments" (GBP) is supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum OutboundPaymentScheme
        {
    
            /// <summary>`scheme` with a value of "faster_payments"</summary>
            [EnumMember(Value = "faster_payments")]
            FasterPayments,
        }
    }

        
    /// <summary>
    /// Cancels an outbound payment. Only outbound payments with either
    /// `verifying`, `pending_approval`, or `scheduled` status can be cancelled.
    /// Once an outbound payment is `executing`, the money moving process has
    /// begun and cannot be reversed.
    /// </summary>
    public class OutboundPaymentCancelRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with
        /// key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

        
    /// <summary>
    /// Approves an outbound payment. Only outbound payments with the
    /// “pending_approval” status can be approved.
    /// </summary>
    public class OutboundPaymentApproveRequest
    {
    }

        
    /// <summary>
    /// Fetches an outbound_payment by ID
    /// </summary>
    public class OutboundPaymentGetRequest
    {
    }

        
    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
    /// outbound payments.
    /// </summary>
    public class OutboundPaymentListRequest
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
        /// The beginning of query period
        /// </summary>
        [JsonProperty("created_from")]
        public string CreatedFrom { get; set; }

        /// <summary>
        /// The end of query period
        /// </summary>
        [JsonProperty("created_to")]
        public string CreatedTo { get; set; }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`verifying`: The payment has been
        /// [created](#outbound-payments-create-an-outbound-payment) and the
        /// verification process has begun.</li>
        /// <li>`pending_approval`: The payment is awaiting
        /// [approval](#outbound-payments-approve-an-outbound-payment).</li>
        /// <li>`scheduled`: The payment has passed verification &
        /// [approval](#outbound-payments-approve-an-outbound-payment), but
        /// processing has not yet begun.</li>
        /// <li>`executing`: The execution date has arrived and the payment has
        /// been placed in queue for processing.</li>
        /// <li>`executed`: The payment has been accepted by the scheme and is
        /// now on its way to the recipient.</li>
        /// <li>`cancelled`: The payment has been
        /// [cancelled](#outbound-payments-cancel-an-outbound-payment) or was
        /// not [approved](#outbound-payments-approve-an-outbound-payment) on
        /// time.</li>
        /// <li>`failed`: The payment was not sent, usually due to an error
        /// while or after executing.</li>
        /// </ul>
        /// </summary>
        [JsonProperty("status")]
        public OutboundPaymentStatus? Status { get; set; }
            
        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`verifying`: The payment has been
        /// [created](#outbound-payments-create-an-outbound-payment) and the
        /// verification process has begun.</li>
        /// <li>`pending_approval`: The payment is awaiting
        /// [approval](#outbound-payments-approve-an-outbound-payment).</li>
        /// <li>`scheduled`: The payment has passed verification &
        /// [approval](#outbound-payments-approve-an-outbound-payment), but
        /// processing has not yet begun.</li>
        /// <li>`executing`: The execution date has arrived and the payment has
        /// been placed in queue for processing.</li>
        /// <li>`executed`: The payment has been accepted by the scheme and is
        /// now on its way to the recipient.</li>
        /// <li>`cancelled`: The payment has been
        /// [cancelled](#outbound-payments-cancel-an-outbound-payment) or was
        /// not [approved](#outbound-payments-approve-an-outbound-payment) on
        /// time.</li>
        /// <li>`failed`: The payment was not sent, usually due to an error
        /// while or after executing.</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum OutboundPaymentStatus
        {
    
            /// <summary>`status` with a value of "verifying"</summary>
            [EnumMember(Value = "verifying")]
            Verifying,
            /// <summary>`status` with a value of "pending_approval"</summary>
            [EnumMember(Value = "pending_approval")]
            PendingApproval,
            /// <summary>`status` with a value of "scheduled"</summary>
            [EnumMember(Value = "scheduled")]
            Scheduled,
            /// <summary>`status` with a value of "executing"</summary>
            [EnumMember(Value = "executing")]
            Executing,
            /// <summary>`status` with a value of "executed"</summary>
            [EnumMember(Value = "executed")]
            Executed,
            /// <summary>`status` with a value of "cancelled"</summary>
            [EnumMember(Value = "cancelled")]
            Cancelled,
            /// <summary>`status` with a value of "failed"</summary>
            [EnumMember(Value = "failed")]
            Failed,
        }
    }

        
    /// <summary>
    /// Updates an outbound payment object. This accepts only the metadata
    /// parameter.
    /// </summary>
    public class OutboundPaymentUpdateRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with
        /// key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single outbound payment.
    /// </summary>
    public class OutboundPaymentResponse : ApiResponse
    {
        /// <summary>
        /// The outbound payment from the response.
        /// </summary>
        [JsonProperty("outbound_payments")]
        public OutboundPayment OutboundPayment { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of outbound payments.
    /// </summary>
    public class OutboundPaymentListResponse : ApiResponse
    {
        /// <summary>
        /// The list of outbound payments from the response.
        /// </summary>
        [JsonProperty("outbound_payments")]
        public IReadOnlyList<OutboundPayment> OutboundPayments { get; private set; }
        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }}
}
