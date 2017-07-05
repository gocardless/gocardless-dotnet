

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
    /// Service class for working with customer bank account resources.
    ///
    /// Customer Bank Accounts hold the bank details of a
    /// [customer](#core-endpoints-customers). They always belong to a
    /// [customer](#core-endpoints-customers), and may be linked to several
    /// Direct Debit [mandates](#core-endpoints-mandates).
    /// 
    /// Note
    /// that customer bank accounts must be unique, and so you will encounter a
    /// `bank_account_exists` error if you try to create a duplicate bank
    /// account. You may wish to handle this by updating the existing record
    /// instead, the ID of which will be provided as
    /// `links[customer_bank_account]` in the error response.
    /// </summary>

    public class CustomerBankAccountService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public CustomerBankAccountService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new customer bank account object.
        /// 
        ///
        /// There are three different ways to supply bank account details:
     
        ///   /// 
        /// - [Local details](#appendix-local-bank-details)
 
        ///       /// 
        /// - IBAN
        /// 
        /// - [Customer
        /// Bank Account
        /// Tokens](#javascript-flow-create-a-customer-bank-account-token)
     
        ///   /// 
        /// For more information on the different fields
        /// required in each country, see [local bank
        /// details](#appendix-local-bank-details).
        /// </summary>
        /// <returns>A single customer bank account resource</returns>
        public Task<CustomerBankAccountResponse> CreateAsync(CustomerBankAccountCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CustomerBankAccountCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<CustomerBankAccountResponse>("POST", "/customer_bank_accounts", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "customer_bank_accounts", customiseRequestMessage);
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your bank accounts.
        /// </summary>
        /// <returns>A set of customer bank account resources</returns>
        public Task<CustomerBankAccountListResponse> ListAsync(CustomerBankAccountListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CustomerBankAccountListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<CustomerBankAccountListResponse>("GET", "/customer_bank_accounts", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of customer bank accounts.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<CustomerBankAccount> All(CustomerBankAccountListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CustomerBankAccountListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.CustomerBankAccounts)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of customer bank accounts.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<CustomerBankAccount>>> AllAsync(CustomerBankAccountListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CustomerBankAccountListRequest();

            return new TaskEnumerable<IReadOnlyList<CustomerBankAccount>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.CustomerBankAccounts, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of an existing bank account.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "BA".</param>
        /// <returns>A single customer bank account resource</returns>
        public Task<CustomerBankAccountResponse> GetAsync(string identity, CustomerBankAccountGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CustomerBankAccountGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CustomerBankAccountResponse>("GET", "/customer_bank_accounts/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Updates a customer bank account object. Only the metadata parameter
        /// is allowed.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "BA".</param>
        /// <returns>A single customer bank account resource</returns>
        public Task<CustomerBankAccountResponse> UpdateAsync(string identity, CustomerBankAccountUpdateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CustomerBankAccountUpdateRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CustomerBankAccountResponse>("PUT", "/customer_bank_accounts/:identity", urlParams, request, null, "customer_bank_accounts", customiseRequestMessage);
        }

        /// <summary>
        /// Immediately cancels all associated mandates and cancellable
        /// payments.
        /// 
        /// This will return a
        /// `disable_failed` error if the bank account has already been
        /// disabled.
        /// 
        /// A disabled bank account can be
        /// re-enabled by creating a new bank account resource with the same
        /// details.
        /// </summary>
        /// <param name="identity">Unique identifier, beginning with "BA".</param>
        /// <returns>A single customer bank account resource</returns>
        public Task<CustomerBankAccountResponse> DisableAsync(string identity, CustomerBankAccountDisableRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CustomerBankAccountDisableRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CustomerBankAccountResponse>("POST", "/customer_bank_accounts/:identity/actions/disable", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    public class CustomerBankAccountCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// Name of the account holder, as known by the bank. Usually this
        /// matches the name of the linked
        /// [customer](#core-endpoints-customers). This field will be
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
        /// Bank code - see [local details](#appendix-local-bank-details) for
        /// more information.  Alternatively you can provide an `iban`.
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
        /// [ISO
        /// 3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// alpha-2 code. Defaults to the country code of the `iban` if
        /// supplied, otherwise is required.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code, defaults to national currency of `country_code`.
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

        [JsonProperty("links")]
        public CustomerBankAccountLinks Links { get; set; }
        public class CustomerBankAccountLinks
        {

            /// <summary>
            /// ID of the [customer](#core-endpoints-customers) that owns this
            /// bank account.
            /// </summary>
            [JsonProperty("customer")]
            public string Customer { get; set; }

            /// <summary>
            /// ID of a [customer bank account
            /// token](#javascript-flow-customer-bank-account-tokens) to use in
            /// place of bank account parameters.
            /// </summary>
            [JsonProperty("customer_bank_account_token")]
            public string CustomerBankAccountToken { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

        
    public class CustomerBankAccountListRequest
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
        /// Unique identifier, beginning with "CU".
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// Get enabled or disabled customer bank accounts.
        /// </summary>
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
            
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CustomerBankAccountEnabled
        {
            /// <summary>
            /// Get enabled or disabled customer bank accounts.
            /// </summary>
    
            [EnumMember(Value = "true")]
            True,
            [EnumMember(Value = "false")]
            False,
        }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

        
    public class CustomerBankAccountGetRequest
    {
    }

        
    public class CustomerBankAccountUpdateRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

        
    public class CustomerBankAccountDisableRequest
    {
    }

    public class CustomerBankAccountResponse : ApiResponse
    {
        [JsonProperty("customer_bank_accounts")]
        public CustomerBankAccount CustomerBankAccount { get; private set; }
    }

    public class CustomerBankAccountListResponse : ApiResponse
    {
        public IReadOnlyList<CustomerBankAccount> CustomerBankAccounts { get; private set; }
        public Meta Meta { get; private set; }
    }
}
