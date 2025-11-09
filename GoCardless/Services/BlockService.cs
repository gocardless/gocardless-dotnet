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
    /// Service class for working with block resources.
    ///
    /// Blocks are created to prevent certain customer details from being used
    /// when creating
    /// mandates.
    ///
    /// The details used to create blocks can be exact matches, like a bank
    /// account or an email,
    /// or a more generic match such as an email domain or bank name. Please be
    /// careful when creating
    /// blocks for more generic matches as this may block legitimate payers from
    /// using your service.
    ///
    /// New block types may be added over time.
    ///
    /// A block is in essence a simple rule that is used to match against
    /// details in a newly
    /// created mandate. If there is a successful match then the mandate is
    /// transitioned to a
    /// "blocked" state.
    ///
    /// Please note:
    ///
    ///   - Payments and subscriptions cannot be created against a mandate in
    /// blocked state.
    ///   - A mandate can never be transitioned out of the blocked state.
    ///
    /// The one exception to this is when blocking a 'bank_name'. This block
    /// will prevent bank
    /// accounts from being created for banks that match the given name. To
    /// ensure we match
    /// bank names correctly an existing bank account must be used when creating
    /// this block. Please
    /// be aware that we cannot always match a bank account to a given bank
    /// name.
    ///
    /// <p class="notice">
    ///   This API is currently only available for GoCardless Protect+
    /// integrators - please <a href="mailto:help@gocardless.com">get in
    /// touch</a> if you would like to use this API.
    /// </p>
    /// </summary>
    public class BlockService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public BlockService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new Block of a given type. By default it will be active.
        /// </summary>
        /// <param name="request">An optional `BlockCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single block resource</returns>
        public Task<BlockResponse> CreateAsync(
            BlockCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BlockCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<BlockResponse>(
                "POST",
                "/blocks",
                urlParams,
                request,
                id => GetAsync(id, null, customiseRequestMessage),
                "blocks",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Retrieves the details of an existing block.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "BLC".</param>
        /// <param name="request">An optional `BlockGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single block resource</returns>
        public Task<BlockResponse> GetAsync(
            string identity,
            BlockGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BlockGetRequest();
            if (identity == null)
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BlockResponse>(
                "GET",
                "/blocks/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your blocks.
        /// </summary>
        /// <param name="request">An optional `BlockListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of block resources</returns>
        public Task<BlockListResponse> ListAsync(
            BlockListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BlockListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<BlockListResponse>(
                "GET",
                "/blocks",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of blocks.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Block> All(
            BlockListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BlockListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Blocks)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of blocks.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Block>>> AllAsync(
            BlockListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BlockListRequest();

            return new TaskEnumerable<IReadOnlyList<Block>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Blocks, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Disables a block so that it no longer will prevent mandate creation.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "BLC".</param>
        /// <param name="request">An optional `BlockDisableRequest` representing the body for this disable request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single block resource</returns>
        public Task<BlockResponse> DisableAsync(
            string identity,
            BlockDisableRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BlockDisableRequest();
            if (identity == null)
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BlockResponse>(
                "POST",
                "/blocks/:identity/actions/disable",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Enables a previously disabled block so that it will prevent mandate
        /// creation
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "BLC".</param>
        /// <param name="request">An optional `BlockEnableRequest` representing the body for this enable request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single block resource</returns>
        public Task<BlockResponse> EnableAsync(
            string identity,
            BlockEnableRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BlockEnableRequest();
            if (identity == null)
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BlockResponse>(
                "POST",
                "/blocks/:identity/actions/enable",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Creates new blocks for a given reference. By default blocks will be
        /// active.
        /// Returns 201 if at least one block was created. Returns 200 if there
        /// were no new
        /// blocks created.
        /// </summary>
        /// <param name="request">An optional `BlockBlockByRefRequest` representing the body for this block_by_ref request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of block resources</returns>
        public Task<BlockListResponse> BlockByRefAsync(
            BlockBlockByRefRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BlockBlockByRefRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<BlockListResponse>(
                "POST",
                "/blocks/block_by_ref",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// Creates a new Block of a given type. By default it will be active.
    /// </summary>
    public class BlockCreateRequest : IHasIdempotencyKey
    {
        /// <summary>
        /// Shows if the block is active or disabled. Only active blocks will be
        /// used when deciding
        /// if a mandate should be blocked.
        /// </summary>
        [JsonProperty("active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Type of entity we will seek to match against when blocking the
        /// mandate. This
        /// can currently be one of 'email', 'email_domain', 'bank_account', or
        /// 'bank_name'.
        /// </summary>
        [JsonProperty("block_type")]
        public string BlockType { get; set; }

        /// <summary>
        /// Type of entity we will seek to match against when blocking the
        /// mandate. This
        /// can currently be one of 'email', 'email_domain', 'bank_account', or
        /// 'bank_name'.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BlockBlockType
        {
            /// <summary>`block_type` with a value of "email"</summary>
            [EnumMember(Value = "email")]
            Email,

            /// <summary>`block_type` with a value of "email_domain"</summary>
            [EnumMember(Value = "email_domain")]
            EmailDomain,

            /// <summary>`block_type` with a value of "bank_account"</summary>
            [EnumMember(Value = "bank_account")]
            BankAccount,

            /// <summary>`block_type` with a value of "bank_name"</summary>
            [EnumMember(Value = "bank_name")]
            BankName,
        }

        /// <summary>
        /// This field is required if the reason_type is other. It should be a
        /// description of
        /// the reason for why you wish to block this payer and why it does not
        /// align with the
        /// given reason_types. This is intended to help us improve our
        /// knowledge of types of
        /// fraud.
        /// </summary>
        [JsonProperty("reason_description")]
        public string ReasonDescription { get; set; }

        /// <summary>
        /// The reason you wish to block this payer, can currently be one of
        /// 'identity_fraud',
        /// 'no_intent_to_pay', 'unfair_chargeback'. If the reason isn't
        /// captured by one of the
        /// above then 'other' can be selected but you must provide a reason
        /// description.
        /// </summary>
        [JsonProperty("reason_type")]
        public string ReasonType { get; set; }

        /// <summary>
        /// The reason you wish to block this payer, can currently be one of
        /// 'identity_fraud',
        /// 'no_intent_to_pay', 'unfair_chargeback'. If the reason isn't
        /// captured by one of the
        /// above then 'other' can be selected but you must provide a reason
        /// description.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BlockReasonType
        {
            /// <summary>`reason_type` with a value of "identity_fraud"</summary>
            [EnumMember(Value = "identity_fraud")]
            IdentityFraud,

            /// <summary>`reason_type` with a value of "no_intent_to_pay"</summary>
            [EnumMember(Value = "no_intent_to_pay")]
            NoIntentToPay,

            /// <summary>`reason_type` with a value of "unfair_chargeback"</summary>
            [EnumMember(Value = "unfair_chargeback")]
            UnfairChargeback,

            /// <summary>`reason_type` with a value of "other"</summary>
            [EnumMember(Value = "other")]
            Other,
        }

        /// <summary>
        /// This field is a reference to the value you wish to block. This may
        /// be the raw value
        /// (in the case of emails or email domains) or the ID of the resource
        /// (in the case of
        /// bank accounts and bank names). This means in order to block a
        /// specific bank account
        /// (even if you wish to block generically by name) it must already have
        /// been created as
        /// a resource.
        /// </summary>
        [JsonProperty("resource_reference")]
        public string ResourceReference { get; set; }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

    /// <summary>
    /// Retrieves the details of an existing block.
    /// </summary>
    public class BlockGetRequest { }

    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// blocks.
    /// </summary>
    public class BlockListRequest
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
        /// ID of a [Block](#core-endpoints-blocks).
        /// </summary>
        [JsonProperty("block")]
        public string Block { get; set; }

        /// <summary>
        /// Type of entity we will seek to match against when blocking the
        /// mandate. This
        /// can currently be one of 'email', 'email_domain', 'bank_account', or
        /// 'bank_name'.
        /// </summary>
        [JsonProperty("block_type")]
        public string BlockType { get; set; }

        /// <summary>
        /// Type of entity we will seek to match against when blocking the
        /// mandate. This
        /// can currently be one of 'email', 'email_domain', 'bank_account', or
        /// 'bank_name'.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BlockBlockType
        {
            /// <summary>`block_type` with a value of "email"</summary>
            [EnumMember(Value = "email")]
            Email,

            /// <summary>`block_type` with a value of "email_domain"</summary>
            [EnumMember(Value = "email_domain")]
            EmailDomain,

            /// <summary>`block_type` with a value of "bank_account"</summary>
            [EnumMember(Value = "bank_account")]
            BankAccount,

            /// <summary>`block_type` with a value of "bank_name"</summary>
            [EnumMember(Value = "bank_name")]
            BankName,
        }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was created.
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
            ///Limit to records created on or before the specified date-time.
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
        /// The reason you wish to block this payer, can currently be one of
        /// 'identity_fraud',
        /// 'no_intent_to_pay', 'unfair_chargeback'. If the reason isn't
        /// captured by one of the
        /// above then 'other' can be selected but you must provide a reason
        /// description.
        /// </summary>
        [JsonProperty("reason_type")]
        public string ReasonType { get; set; }

        /// <summary>
        /// The reason you wish to block this payer, can currently be one of
        /// 'identity_fraud',
        /// 'no_intent_to_pay', 'unfair_chargeback'. If the reason isn't
        /// captured by one of the
        /// above then 'other' can be selected but you must provide a reason
        /// description.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BlockReasonType
        {
            /// <summary>`reason_type` with a value of "identity_fraud"</summary>
            [EnumMember(Value = "identity_fraud")]
            IdentityFraud,

            /// <summary>`reason_type` with a value of "no_intent_to_pay"</summary>
            [EnumMember(Value = "no_intent_to_pay")]
            NoIntentToPay,

            /// <summary>`reason_type` with a value of "unfair_chargeback"</summary>
            [EnumMember(Value = "unfair_chargeback")]
            UnfairChargeback,

            /// <summary>`reason_type` with a value of "other"</summary>
            [EnumMember(Value = "other")]
            Other,
        }

        /// <summary>
        /// Fixed [timestamp](#api-usage-dates-and-times), recording when this
        /// resource was updated.
        /// </summary>
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Disables a block so that it no longer will prevent mandate creation.
    /// </summary>
    public class BlockDisableRequest { }

    /// <summary>
    /// Enables a previously disabled block so that it will prevent mandate
    /// creation
    /// </summary>
    public class BlockEnableRequest { }

    /// <summary>
    /// Creates new blocks for a given reference. By default blocks will be
    /// active.
    /// Returns 201 if at least one block was created. Returns 200 if there were
    /// no new
    /// blocks created.
    /// </summary>
    public class BlockBlockByRefRequest
    {
        /// <summary>
        /// Shows if the block is active or disabled. Only active blocks will be
        /// used when deciding
        /// if a mandate should be blocked.
        /// </summary>
        [JsonProperty("active")]
        public bool? Active { get; set; }

        /// <summary>
        /// This field is required if the reason_type is other. It should be a
        /// description of
        /// the reason for why you wish to block this payer and why it does not
        /// align with the
        /// given reason_types. This is intended to help us improve our
        /// knowledge of types of
        /// fraud.
        /// </summary>
        [JsonProperty("reason_description")]
        public string ReasonDescription { get; set; }

        /// <summary>
        /// The reason you wish to block this payer, can currently be one of
        /// 'identity_fraud',
        /// 'no_intent_to_pay', 'unfair_chargeback'. If the reason isn't
        /// captured by one of the
        /// above then 'other' can be selected but you must provide a reason
        /// description.
        /// </summary>
        [JsonProperty("reason_type")]
        public string ReasonType { get; set; }

        /// <summary>
        /// The reason you wish to block this payer, can currently be one of
        /// 'identity_fraud',
        /// 'no_intent_to_pay', 'unfair_chargeback'. If the reason isn't
        /// captured by one of the
        /// above then 'other' can be selected but you must provide a reason
        /// description.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BlockReasonType
        {
            /// <summary>`reason_type` with a value of "identity_fraud"</summary>
            [EnumMember(Value = "identity_fraud")]
            IdentityFraud,

            /// <summary>`reason_type` with a value of "no_intent_to_pay"</summary>
            [EnumMember(Value = "no_intent_to_pay")]
            NoIntentToPay,

            /// <summary>`reason_type` with a value of "unfair_chargeback"</summary>
            [EnumMember(Value = "unfair_chargeback")]
            UnfairChargeback,

            /// <summary>`reason_type` with a value of "other"</summary>
            [EnumMember(Value = "other")]
            Other,
        }

        /// <summary>
        /// Type of entity we will seek to get the associated emails and bank
        /// accounts to
        /// create blocks from. This can currently be one of 'customer' or
        /// 'mandate'.
        /// </summary>
        [JsonProperty("reference_type")]
        public string ReferenceType { get; set; }

        /// <summary>
        /// Type of entity we will seek to get the associated emails and bank
        /// accounts to
        /// create blocks from. This can currently be one of 'customer' or
        /// 'mandate'.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BlockReferenceType
        {
            /// <summary>`reference_type` with a value of "customer"</summary>
            [EnumMember(Value = "customer")]
            Customer,

            /// <summary>`reference_type` with a value of "mandate"</summary>
            [EnumMember(Value = "mandate")]
            Mandate,
        }

        /// <summary>
        /// This field is a reference to the entity you wish to block based on
        /// its emails
        /// and bank accounts. This may be the ID of a customer or a mandate.
        /// This means in
        /// order to block by reference the entity must have already been
        /// created as a
        /// resource.
        /// </summary>
        [JsonProperty("reference_value")]
        public string ReferenceValue { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single block.
    /// </summary>
    public class BlockResponse : ApiResponse
    {
        /// <summary>
        /// The block from the response.
        /// </summary>
        [JsonProperty("blocks")]
        public Block Block { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of blocks.
    /// </summary>
    public class BlockListResponse : ApiResponse
    {
        /// <summary>
        /// The list of blocks from the response.
        /// </summary>
        [JsonProperty("blocks")]
        public IReadOnlyList<Block> Blocks { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
