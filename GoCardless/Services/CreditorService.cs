

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
    /// Service class for working with creditor resources.
    ///
    /// Each [payment](#core-endpoints-payments) taken through the API is linked
    /// to a "creditor", to whom the payment is then paid out. In most cases
    /// your organisation will have a single "creditor", but the API also
    /// supports collecting payments on behalf of others.
    /// 
    /// Currently, for Anti Money Laundering reasons, any creditors you add must
    /// be directly related to your organisation.
    /// </summary>

    public class CreditorService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public CreditorService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new creditor.
        /// </summary>
        /// <param name="request">An optional `CreditorCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single creditor resource</returns>
        public Task<CreditorResponse> CreateAsync(CreditorCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<CreditorResponse>("POST", "/creditors", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "creditors", customiseRequestMessage);
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your creditors.
        /// </summary>
        /// <param name="request">An optional `CreditorListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of creditor resources</returns>
        public Task<CreditorListResponse> ListAsync(CreditorListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<CreditorListResponse>("GET", "/creditors", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of creditors.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Creditor> All(CreditorListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Creditors)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of creditors.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Creditor>>> AllAsync(CreditorListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorListRequest();

            return new TaskEnumerable<IReadOnlyList<Creditor>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Creditors, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of an existing creditor.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "CR".</param> 
        /// <param name="request">An optional `CreditorGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single creditor resource</returns>
        public Task<CreditorResponse> GetAsync(string identity, CreditorGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CreditorResponse>("GET", "/creditors/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Updates a creditor object. Supports all of the fields supported when
        /// creating a creditor.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "CR".</param> 
        /// <param name="request">An optional `CreditorUpdateRequest` representing the body for this update request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single creditor resource</returns>
        public Task<CreditorResponse> UpdateAsync(string identity, CreditorUpdateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new CreditorUpdateRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CreditorResponse>("PUT", "/creditors/:identity", urlParams, request, null, "creditors", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Creates a new creditor.
    /// </summary>
    public class CreditorCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// [ISO 3166-1 alpha-2
        /// code.](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// The type of business of the creditor. Currently, `individual`,
        /// `company`, `charity`, `partnership`, and `trust` are supported.
        /// </summary>
        [JsonProperty("creditor_type")]
        public CreditorCreditorType? CreditorType { get; set; }
            
        /// <summary>
        /// The type of business of the creditor. Currently, `individual`,
        /// `company`, `charity`, `partnership`, and `trust` are supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CreditorCreditorType
        {
    
            /// <summary>`creditor_type` with a value of "company"</summary>
            [EnumMember(Value = "company")]
            Company,
            /// <summary>`creditor_type` with a value of "individual"</summary>
            [EnumMember(Value = "individual")]
            Individual,
            /// <summary>`creditor_type` with a value of "charity"</summary>
            [EnumMember(Value = "charity")]
            Charity,
            /// <summary>`creditor_type` with a value of "partnership"</summary>
            [EnumMember(Value = "partnership")]
            Partnership,
            /// <summary>`creditor_type` with a value of "trust"</summary>
            [EnumMember(Value = "trust")]
            Trust,
        }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public IDictionary<String, String> Links { get; set; }

        /// <summary>
        /// The creditor's name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

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
    /// creditors.
    /// </summary>
    public class CreditorListRequest
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
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

        
    /// <summary>
    /// Retrieves the details of an existing creditor.
    /// </summary>
    public class CreditorGetRequest
    {
    }

        
    /// <summary>
    /// Updates a creditor object. Supports all of the fields supported when
    /// creating a creditor.
    /// </summary>
    public class CreditorUpdateRequest
    {

        /// <summary>
        /// The first line of the creditor's address.
        /// </summary>
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// The second line of the creditor's address.
        /// </summary>
        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The third line of the creditor's address.
        /// </summary>
        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }

        /// <summary>
        /// The city of the creditor's address.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// [ISO 3166-1 alpha-2
        /// code.](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public CreditorLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a Creditor.
        /// </summary>
        public class CreditorLinks
        {
                
                /// <summary>
                            /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
            /// which is set up to receive payouts in AUD.
                /// </summary>
                [JsonProperty("default_aud_payout_account")]
                public string DefaultAudPayoutAccount { get; set; }
                
                /// <summary>
                            /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
            /// which is set up to receive payouts in CAD.
                /// </summary>
                [JsonProperty("default_cad_payout_account")]
                public string DefaultCadPayoutAccount { get; set; }
                
                /// <summary>
                            /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
            /// which is set up to receive payouts in DKK.
                /// </summary>
                [JsonProperty("default_dkk_payout_account")]
                public string DefaultDkkPayoutAccount { get; set; }
                
                /// <summary>
                            /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
            /// which is set up to receive payouts in EUR.
                /// </summary>
                [JsonProperty("default_eur_payout_account")]
                public string DefaultEurPayoutAccount { get; set; }
                
                /// <summary>
                            /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
            /// which is set up to receive payouts in GBP.
                /// </summary>
                [JsonProperty("default_gbp_payout_account")]
                public string DefaultGbpPayoutAccount { get; set; }
                
                /// <summary>
                            /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
            /// which is set up to receive payouts in NZD.
                /// </summary>
                [JsonProperty("default_nzd_payout_account")]
                public string DefaultNzdPayoutAccount { get; set; }
                
                /// <summary>
                            /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
            /// which is set up to receive payouts in SEK.
                /// </summary>
                [JsonProperty("default_sek_payout_account")]
                public string DefaultSekPayoutAccount { get; set; }
                
                /// <summary>
                            /// ID of the [bank account](#core-endpoints-creditor-bank-accounts)
            /// which is set up to receive payouts in USD.
                /// </summary>
                [JsonProperty("default_usd_payout_account")]
                public string DefaultUsdPayoutAccount { get; set; }
        }

        /// <summary>
        /// The creditor's name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The creditor's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The creditor's address region, county or department.
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single creditor.
    /// </summary>
    public class CreditorResponse : ApiResponse
    {
        /// <summary>
        /// The creditor from the response.
        /// </summary>
        [JsonProperty("creditors")]
        public Creditor Creditor { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of creditors.
    /// </summary>
    public class CreditorListResponse : ApiResponse
    {
        /// <summary>
        /// The list of creditors from the response.
        /// </summary>
        [JsonProperty("creditors")]
        public IReadOnlyList<Creditor> Creditors { get; private set; }
        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
