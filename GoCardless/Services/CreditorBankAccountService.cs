

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
    /// Service class for working with creditor bank account resources.
    ///
    /// Creditor Bank Accounts hold the bank details of a
    /// [creditor](#core-endpoints-creditors). These are the bank accounts which
    /// your [payouts](#core-endpoints-payouts) will be sent to.
    /// 
    /// Note that creditor bank accounts must be unique, and so you will
    /// encounter a `bank_account_exists` error if you try to create a duplicate
    /// bank account. You may wish to handle this by updating the existing
    /// record instead, the ID of which will be provided as
    /// `links[creditor_bank_account]` in the error response.
    /// 
    /// <p class="restricted-notice"><strong>Restricted</strong>: This API is
    /// not available for partner integrations.</p>
    /// </summary>

    public class CreditorBankAccountService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public CreditorBankAccountService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new creditor bank account object.
        /// </summary>
        /// <param name="request">An optional `CreditorBankAccountCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single creditor bank account resource</returns>
        public Task<CreditorBankAccountResponse> CreateAsync(CreditorBankAccountCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorBankAccountCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<CreditorBankAccountResponse>("POST", "/creditor_bank_accounts", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "creditor_bank_accounts", customiseRequestMessage);
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your creditor bank accounts.
        /// </summary>
        /// <param name="request">An optional `CreditorBankAccountListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of creditor bank account resources</returns>
        public Task<CreditorBankAccountListResponse> ListAsync(CreditorBankAccountListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorBankAccountListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<CreditorBankAccountListResponse>("GET", "/creditor_bank_accounts", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of creditor bank accounts.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<CreditorBankAccount> All(CreditorBankAccountListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorBankAccountListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.CreditorBankAccounts)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of creditor bank accounts.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<CreditorBankAccount>>> AllAsync(CreditorBankAccountListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorBankAccountListRequest();

            return new TaskEnumerable<IReadOnlyList<CreditorBankAccount>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.CreditorBankAccounts, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of an existing creditor bank account.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BA".</param> 
        /// <param name="request">An optional `CreditorBankAccountGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single creditor bank account resource</returns>
        public Task<CreditorBankAccountResponse> GetAsync(string identity, CreditorBankAccountGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorBankAccountGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CreditorBankAccountResponse>("GET", "/creditor_bank_accounts/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Immediately disables the bank account, no money can be paid out to a
        /// disabled account.
        /// 
        /// This will return a `disable_failed` error if the bank account has
        /// already been disabled.
        /// 
        /// A disabled bank account can be re-enabled by creating a new bank
        /// account resource with the same details.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "BA".</param> 
        /// <param name="request">An optional `CreditorBankAccountDisableRequest` representing the body for this disable request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single creditor bank account resource</returns>
        public Task<CreditorBankAccountResponse> DisableAsync(string identity, CreditorBankAccountDisableRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorBankAccountDisableRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CreditorBankAccountResponse>("POST", "/creditor_bank_accounts/:identity/actions/disable", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Creates a new creditor bank account object.
    /// </summary>
    public class CreditorBankAccountCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// Name of the account holder, as known by the bank. Usually this is
        /// the same as the name stored with the linked
        /// [creditor](#core-endpoints-creditors). This field will be
        /// transliterated, upcased and truncated to 18 characters.
        /// </summary>
        [JsonProperty("account_holder_name")]
        public string AccountHolderName { get; set; }

        /// <summary>
        /// Bank account number - see [local
        /// details](#appendix-local-bank-details) for more information.
        /// Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See [local
        /// details](#local-bank-details-united-states) for more information.
        /// </summary>
        [JsonProperty("account_type")]
        public CreditorBankAccountAccountType? AccountType { get; set; }
            
        /// <summary>
        /// Bank account type. Required for USD-denominated bank accounts. Must
        /// not be provided for bank accounts in other currencies. See [local
        /// details](#local-bank-details-united-states) for more information.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CreditorBankAccountAccountType
        {
    
            /// <summary>`account_type` with a value of "savings"</summary>
            [EnumMember(Value = "savings")]
            Savings,
            /// <summary>`account_type` with a value of "checking"</summary>
            [EnumMember(Value = "checking")]
            Checking,
        }

        /// <summary>
        /// Bank code - see [local details](#appendix-local-bank-details) for
        /// more information. Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// Branch code - see [local details](#appendix-local-bank-details) for
        /// more information. Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }

        /// <summary>
        /// [ISO 3166-1 alpha-2
        /// code](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements).
        /// Defaults to the country code of the `iban` if supplied, otherwise is
        /// required.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// International Bank Account Number. Alternatively you can provide
        /// [local details](#appendix-local-bank-details). IBANs are not
        /// accepted for Swedish bank accounts denominated in SEK - you must
        /// supply [local details](#local-bank-details-sweden).
        /// </summary>
        [JsonProperty("iban")]
        public string Iban { get; set; }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public CreditorBankAccountLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a CreditorBankAccount.
        /// </summary>
        public class CreditorBankAccountLinks
        {

            /// <summary>
            /// ID of the [creditor](#core-endpoints-creditors) that owns this
            /// bank account.
            /// </summary>
            [JsonProperty("creditor")]
            public string Creditor { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// Defaults to `false`. When this is set to `true`, it will cause this
        /// bank account to be set as the account that GoCardless will pay out
        /// to.
        /// </summary>
        [JsonProperty("set_as_default_payout_account")]
        public bool? SetAsDefaultPayoutAccount { get; set; }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

        
    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// creditor bank accounts.
    /// </summary>
    public class CreditorBankAccountListRequest
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
            ///Limit to records created on or before the specified date-time.
            /// </summary>
            [JsonProperty("lte")]
            public DateTimeOffset? LessThanOrEqual { get; set; }
        }

        /// <summary>
        /// Unique identifier, beginning with "CR".
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }

        /// <summary>
        /// Boolean value showing whether the bank account is enabled or
        /// disabled
        /// </summary>
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
            
        /// <summary>
        /// Boolean value showing whether the bank account is enabled or
        /// disabled
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CreditorBankAccountEnabled
        {
    
            /// <summary>`enabled` with a value of "true"</summary>
            [EnumMember(Value = "true")]
            True,
            /// <summary>`enabled` with a value of "false"</summary>
            [EnumMember(Value = "false")]
            False,
        }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

        
    /// <summary>
    /// Retrieves the details of an existing creditor bank account.
    /// </summary>
    public class CreditorBankAccountGetRequest
    {
    }

        
    /// <summary>
    /// Immediately disables the bank account, no money can be paid out to a
    /// disabled account.
    /// 
    /// This will return a `disable_failed` error if the bank account has
    /// already been disabled.
    /// 
    /// A disabled bank account can be re-enabled by creating a new bank account
    /// resource with the same details.
    /// </summary>
    public class CreditorBankAccountDisableRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single creditor bank account.
    /// </summary>
    public class CreditorBankAccountResponse : ApiResponse
    {
        /// <summary>
        /// The creditor bank account from the response.
        /// </summary>
        [JsonProperty("creditor_bank_accounts")]
        public CreditorBankAccount CreditorBankAccount { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of creditor bank accounts.
    /// </summary>
    public class CreditorBankAccountListResponse : ApiResponse
    {
        /// <summary>
        /// The list of creditor bank accounts from the response.
        /// </summary>
        [JsonProperty("creditor_bank_accounts")]
        public IReadOnlyList<CreditorBankAccount> CreditorBankAccounts { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
