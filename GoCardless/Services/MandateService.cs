

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
    /// Service class for working with mandate resources.
    ///
    /// Mandates represent the Direct Debit mandate with a
    /// [customer](#core-endpoints-customers).
    /// 
    /// GoCardless will
    /// notify you via a [webhook](#appendix-webhooks) whenever the status of a
    /// mandate changes.
    /// </summary>

    public class MandateService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public MandateService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new mandate object.
        /// </summary>
        /// <returns>A single mandate resource</returns>
        public Task<MandateResponse> CreateAsync(MandateCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new MandateCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<MandateResponse>("POST", "/mandates", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "mandates", customiseRequestMessage);
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your mandates.
        /// </summary>
        /// <returns>A set of mandate resources</returns>
        public Task<MandateListResponse> ListAsync(MandateListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new MandateListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<MandateListResponse>("GET", "/mandates", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of mandates.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Mandate> All(MandateListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new MandateListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Mandates)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of mandates.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Mandate>>> AllAsync(MandateListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new MandateListRequest();

            return new TaskEnumerable<IReadOnlyList<Mandate>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Mandates, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of an existing mandate.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "MD".</param>
        /// <returns>A single mandate resource</returns>
        public Task<MandateResponse> GetAsync(string identity, MandateGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new MandateGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<MandateResponse>("GET", "/mandates/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Updates a mandate object. This accepts only the metadata parameter.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "MD".</param>
        /// <returns>A single mandate resource</returns>
        public Task<MandateResponse> UpdateAsync(string identity, MandateUpdateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new MandateUpdateRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<MandateResponse>("PUT", "/mandates/:identity", urlParams, request, null, "mandates", customiseRequestMessage);
        }

        /// <summary>
        /// Immediately cancels a mandate and all associated cancellable
        /// payments. Any metadata supplied to this endpoint will be stored on
        /// the mandate cancellation event it causes.
        /// 
        ///
        /// This will fail with a `cancellation_failed` error if the mandate is
        /// already cancelled.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "MD".</param>
        /// <returns>A single mandate resource</returns>
        public Task<MandateResponse> CancelAsync(string identity, MandateCancelRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new MandateCancelRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<MandateResponse>("POST", "/mandates/:identity/actions/cancel", urlParams, request, null, "data", customiseRequestMessage);
        }

        /// <summary>
        /// <a name="mandate_not_inactive"></a>Reinstates a cancelled or expired
        /// mandate to the banks. You will receive a `resubmission_requested`
        /// webhook, but after that reinstating the mandate follows the same
        /// process as its initial creation, so you will receive a `submitted`
        /// webhook, followed by a `reinstated` or `failed` webhook up to two
        /// working days later. Any metadata supplied to this endpoint will be
        /// stored on the `resubmission_requested` event it causes.
        ///
        /// 
        /// This will fail with a `mandate_not_inactive` error if
        /// the mandate is already being submitted, or is active.
        /// 
 
        ///       /// Mandates can be resubmitted up to 3 times.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "MD".</param>
        /// <returns>A single mandate resource</returns>
        public Task<MandateResponse> ReinstateAsync(string identity, MandateReinstateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new MandateReinstateRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<MandateResponse>("POST", "/mandates/:identity/actions/reinstate", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    public class MandateCreateRequest : IHasIdempotencyKey
    {

        [JsonProperty("links")]
        public MandateLinks Links { get; set; }
        public class MandateLinks
        {

            /// <summary>
            /// ID of the associated [creditor](#core-endpoints-creditors). Only
            /// required if your account manages multiple creditors.
            /// </summary>
            [JsonProperty("creditor")]
            public string Creditor { get; set; }

            /// <summary>
            /// ID of the associated [customer bank
            /// account](#core-endpoints-customer-bank-accounts) which the
            /// mandate is created and submits payments against.
            /// </summary>
            [JsonProperty("customer_bank_account")]
            public string CustomerBankAccount { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// Unique reference. Different schemes have different length and
        /// [character set](#appendix-character-sets) requirements. GoCardless
        /// will generate a unique reference satisfying the different scheme
        /// requirements if this field is left blank.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// <a name="mandates_scheme"></a>Direct Debit scheme to which this
        /// mandate and associated payments are submitted. Can be supplied or
        /// automatically detected from the customer's bank account. Currently
        /// only "autogiro", "bacs", and "sepa_core" are supported.
        /// </summary>
        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

        
    public class MandateListRequest
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

        [JsonProperty("created_at")]
        public CreatedAtParam CreatedAt { get; set; }

        public class CreatedAtParam
        {
            /// <summary>
            /// Limit to records created within certain times
            /// </summary>
            [JsonProperty("gt")]
            public DateTimeOffset? GreaterThan { get; set; }

            [JsonProperty("gte")]
            public DateTimeOffset? GreaterThanOrEqual { get; set; }

            [JsonProperty("lt")]
            public DateTimeOffset? LessThan { get; set; }

            [JsonProperty("lte")]
            public DateTimeOffset? LessThanOrEqual { get; set; }
        }

        /// <summary>
        /// ID of a [creditor](#core-endpoints-creditors). If specified, this
        /// endpoint will return all mandates for the given creditor. Cannot be
        /// used in conjunction with `customer` or `customer_bank_account`
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// ID of a [customer](#core-endpoints-customers). If specified, this
        /// endpoint will return all mandates for the given customer. Cannot be
        /// used in conjunction with `customer_bank_account` or `creditor`
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// ID of a [customer bank
        /// account](#core-endpoints-customer-bank-accounts). If specified, this
        /// endpoint will return all mandates for the given bank account. Cannot
        /// be used in conjunction with `customer` or `creditor`
        /// </summary>
        [JsonProperty("customer_bank_account")]
        public string CustomerBankAccount { get; set; }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Unique reference. Different schemes have different length and
        /// [character set](#appendix-character-sets) requirements. GoCardless
        /// will generate a unique reference satisfying the different scheme
        /// requirements if this field is left blank.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// At most three valid status values
        /// </summary>
        [JsonProperty("status")]
        public MandateStatus[] Status { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public enum MandateStatus
        {
            /// <summary>
            /// One of:
            /// <ul>
            ///
            /// <li>`pending_customer_approval`: the mandate has not yet been
            /// signed by the second customer</li>
            ///
            /// <li>`pending_submission`: the mandate has not yet been submitted
            /// to the customer's bank</li>
            /// <li>`submitted`: the
            /// mandate has been submitted to the customer's bank but has not
            /// been processed yet</li>
            /// <li>`active`: the
            /// mandate has been successfully set up by the customer's
            /// bank</li>
            /// <li>`failed`: the mandate could not be
            /// created</li>
            /// <li>`cancelled`: the mandate has
            /// been cancelled</li>
            /// <li>`expired`: the mandate
            /// has expired due to dormancy</li>
            /// </ul>
            /// </summary>
    
            [EnumMember(Value = "pending_customer_approval")]
            PendingCustomerApproval,
            [EnumMember(Value = "pending_submission")]
            PendingSubmission,
            [EnumMember(Value = "submitted")]
            Submitted,
            [EnumMember(Value = "active")]
            Active,
            [EnumMember(Value = "failed")]
            Failed,
            [EnumMember(Value = "cancelled")]
            Cancelled,
            [EnumMember(Value = "expired")]
            Expired,
        }
    }

        
    public class MandateGetRequest
    {
    }

        
    public class MandateUpdateRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

        
    public class MandateCancelRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

        
    public class MandateReinstateRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

    public class MandateResponse : ApiResponse
    {
        [JsonProperty("mandates")]
        public Mandate Mandate { get; private set; }
    }

    public class MandateListResponse : ApiResponse
    {
        public IReadOnlyList<Mandate> Mandates { get; private set; }
        public Meta Meta { get; private set; }
    }
}
