

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
    /// Service class for working with scheme identifier resources.
    ///
    /// This represents a scheme identifier (e.g. a SUN in Bacs or a CID in
    /// SEPA). Scheme identifiers are used to specify the beneficiary name that
    /// appears on customers' bank statements.
    /// 
    /// </summary>

    public class SchemeIdentifierService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public SchemeIdentifierService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new scheme identifier. The scheme identifier status will
        /// be `pending` while GoCardless is
        /// processing the request. Once the scheme identifier is ready to be
        /// used the status will be updated to `active`.
        /// At this point, GoCardless will emit a scheme identifier activated
        /// event via webhook to notify you of this change.
        /// In Bacs, it will take up to five working days for a scheme
        /// identifier to become active. On other schemes, including SEPA,
        /// this happens instantly.
        /// 
        /// #### Scheme identifier name validations
        /// 
        /// The `name` field of a scheme identifier can contain alphanumeric
        /// characters, spaces and
        /// special characters.
        /// 
        /// Its maximum length and the special characters it supports depend on
        /// the scheme:
        /// 
        /// | __scheme__        | __maximum length__ | __special characters
        /// allowed__                      |
        /// | :---------------- | :----------------- |
        /// :-------------------------------------------------- |
        /// | `bacs`            | 18 characters      | `/` `.` `&` `-`          
        ///                           |
        /// | `sepa`            | 70 characters      | `/` `?` `:` `(` `)` `.`
        /// `,` `+` `&` `<` `>` `'` `"` |
        /// | `ach`             | 16 characters      | `/` `?` `:` `(` `)` `.`
        /// `,` `'` `+` `-`             |
        /// | `faster_payments` | 18 characters      | `/` `?` `:` `(` `)` `.`
        /// `,` `'` `+` `-`             |
        /// 
        /// The validation error that gets returned for an invalid name will
        /// contain a suggested name
        /// in the metadata that is guaranteed to pass name validations.
        /// 
        /// You should ensure that the name you set matches the legal name or
        /// the trading name of
        /// the creditor, otherwise, there is an increased risk of chargeback.
        /// 
        /// </summary>
        /// <param name="request">An optional `SchemeIdentifierCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single scheme identifier resource</returns>
        public Task<SchemeIdentifierResponse> CreateAsync(SchemeIdentifierCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new SchemeIdentifierCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<SchemeIdentifierResponse>("POST", "/scheme_identifiers", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "scheme_identifiers", customiseRequestMessage);
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your scheme identifiers.
        /// </summary>
        /// <param name="request">An optional `SchemeIdentifierListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of scheme identifier resources</returns>
        public Task<SchemeIdentifierListResponse> ListAsync(SchemeIdentifierListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new SchemeIdentifierListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<SchemeIdentifierListResponse>("GET", "/scheme_identifiers", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of scheme identifiers.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<SchemeIdentifier> All(SchemeIdentifierListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new SchemeIdentifierListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.SchemeIdentifiers)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of scheme identifiers.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<SchemeIdentifier>>> AllAsync(SchemeIdentifierListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new SchemeIdentifierListRequest();

            return new TaskEnumerable<IReadOnlyList<SchemeIdentifier>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.SchemeIdentifiers, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of an existing scheme identifier.
        /// </summary>  
        /// <param name="identity">Unique identifier, usually beginning with "SU".</param> 
        /// <param name="request">An optional `SchemeIdentifierGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single scheme identifier resource</returns>
        public Task<SchemeIdentifierResponse> GetAsync(string identity, SchemeIdentifierGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new SchemeIdentifierGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<SchemeIdentifierResponse>("GET", "/scheme_identifiers/:identity", urlParams, request, null, null, customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Creates a new scheme identifier. The scheme identifier status will be
    /// `pending` while GoCardless is
    /// processing the request. Once the scheme identifier is ready to be used
    /// the status will be updated to `active`.
    /// At this point, GoCardless will emit a scheme identifier activated event
    /// via webhook to notify you of this change.
    /// In Bacs, it will take up to five working days for a scheme identifier to
    /// become active. On other schemes, including SEPA,
    /// this happens instantly.
    /// 
    /// #### Scheme identifier name validations
    /// 
    /// The `name` field of a scheme identifier can contain alphanumeric
    /// characters, spaces and
    /// special characters.
    /// 
    /// Its maximum length and the special characters it supports depend on the
    /// scheme:
    /// 
    /// | __scheme__        | __maximum length__ | __special characters
    /// allowed__                      |
    /// | :---------------- | :----------------- |
    /// :-------------------------------------------------- |
    /// | `bacs`            | 18 characters      | `/` `.` `&` `-`              
    ///                       |
    /// | `sepa`            | 70 characters      | `/` `?` `:` `(` `)` `.` `,`
    /// `+` `&` `<` `>` `'` `"` |
    /// | `ach`             | 16 characters      | `/` `?` `:` `(` `)` `.` `,`
    /// `'` `+` `-`             |
    /// | `faster_payments` | 18 characters      | `/` `?` `:` `(` `)` `.` `,`
    /// `'` `+` `-`             |
    /// 
    /// The validation error that gets returned for an invalid name will contain
    /// a suggested name
    /// in the metadata that is guaranteed to pass name validations.
    /// 
    /// You should ensure that the name you set matches the legal name or the
    /// trading name of
    /// the creditor, otherwise, there is an increased risk of chargeback.
    /// 
    /// </summary>
    public class SchemeIdentifierCreateRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public SchemeIdentifierLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a SchemeIdentifier.
        /// </summary>
        public class SchemeIdentifierLinks
        {
                
                /// <summary>
                            /// <em>required</em> ID of the associated
            /// [creditor](#core-endpoints-creditors).
            /// 
                /// </summary>
                [JsonProperty("creditor")]
                public string Creditor { get; set; }
        }

        /// <summary>
        /// The name which appears on customers' bank statements. This should
        /// usually be the merchant's trading name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The scheme which this scheme identifier applies to.
        /// </summary>
        [JsonProperty("scheme")]
        public SchemeIdentifierScheme? Scheme { get; set; }
            
        /// <summary>
        /// The scheme which this scheme identifier applies to.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum SchemeIdentifierScheme
        {
    
            /// <summary>`scheme` with a value of "ach"</summary>
            [EnumMember(Value = "ach")]
            Ach,
            /// <summary>`scheme` with a value of "autogiro"</summary>
            [EnumMember(Value = "autogiro")]
            Autogiro,
            /// <summary>`scheme` with a value of "bacs"</summary>
            [EnumMember(Value = "bacs")]
            Bacs,
            /// <summary>`scheme` with a value of "becs"</summary>
            [EnumMember(Value = "becs")]
            Becs,
            /// <summary>`scheme` with a value of "becs_nz"</summary>
            [EnumMember(Value = "becs_nz")]
            BecsNz,
            /// <summary>`scheme` with a value of "betalingsservice"</summary>
            [EnumMember(Value = "betalingsservice")]
            Betalingsservice,
            /// <summary>`scheme` with a value of "faster_payments"</summary>
            [EnumMember(Value = "faster_payments")]
            FasterPayments,
            /// <summary>`scheme` with a value of "pad"</summary>
            [EnumMember(Value = "pad")]
            Pad,
            /// <summary>`scheme` with a value of "pay_to"</summary>
            [EnumMember(Value = "pay_to")]
            PayTo,
            /// <summary>`scheme` with a value of "sepa"</summary>
            [EnumMember(Value = "sepa")]
            Sepa,
            /// <summary>`scheme` with a value of "sepa_credit_transfer"</summary>
            [EnumMember(Value = "sepa_credit_transfer")]
            SepaCreditTransfer,
            /// <summary>`scheme` with a value of "sepa_instant_credit_transfer"</summary>
            [EnumMember(Value = "sepa_instant_credit_transfer")]
            SepaInstantCreditTransfer,
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
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// scheme identifiers.
    /// </summary>
    public class SchemeIdentifierListRequest
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
        /// Unique identifier, beginning with "CR".
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
    /// Retrieves the details of an existing scheme identifier.
    /// </summary>
    public class SchemeIdentifierGetRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single scheme identifier.
    /// </summary>
    public class SchemeIdentifierResponse : ApiResponse
    {
        /// <summary>
        /// The scheme identifier from the response.
        /// </summary>
        [JsonProperty("scheme_identifiers")]
        public SchemeIdentifier SchemeIdentifier { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of scheme identifiers.
    /// </summary>
    public class SchemeIdentifierListResponse : ApiResponse
    {
        /// <summary>
        /// The list of scheme identifiers from the response.
        /// </summary>
        [JsonProperty("scheme_identifiers")]
        public IReadOnlyList<SchemeIdentifier> SchemeIdentifiers { get; private set; }
        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }}
}
