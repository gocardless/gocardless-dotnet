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
    /// Service class for working with customer resources.
    ///
    ///  Customer objects hold the contact details for a customer. A customer
    ///  can have several [customer bank
    ///  accounts](#core-endpoints-customer-bank-accounts), which in turn can
    ///  have several Direct Debit [mandates](#core-endpoints-mandates).
    /// </summary>
    public class CustomerService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public CustomerService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        ///  Creates a new customer object.
        /// </summary>
        /// <param name="request">An optional `CustomerCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single customer resource</returns>
        public Task<CustomerResponse> CreateAsync(
            CustomerCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new CustomerCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<CustomerResponse>(
                "POST",
                "/customers",
                urlParams,
                request,
                id => GetAsync(id, null, customiseRequestMessage),
                "customers",
                customiseRequestMessage
            );
        }

        /// <summary>
        ///  Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        ///  your customers.
        /// </summary>
        /// <param name="request">An optional `CustomerListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of customer resources</returns>
        public Task<CustomerListResponse> ListAsync(
            CustomerListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new CustomerListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<CustomerListResponse>(
                "GET",
                "/customers",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of customers.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Customer> All(
            CustomerListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new CustomerListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Customers)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of customers.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Customer>>> AllAsync(
            CustomerListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new CustomerListRequest();

            return new TaskEnumerable<IReadOnlyList<Customer>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Customers, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        ///  Retrieves the details of an existing customer.
        /// </summary>
        ///  <param name="identity">Unique identifier, beginning with "CU".</param>
        /// <param name="request">An optional `CustomerGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single customer resource</returns>
        public Task<CustomerResponse> GetAsync(
            string identity,
            CustomerGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new CustomerGetRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CustomerResponse>(
                "GET",
                "/customers/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        ///  Updates a customer object. Supports all of the fields supported
        ///  when creating a customer.
        /// </summary>
        ///  <param name="identity">Unique identifier, beginning with "CU".</param>
        /// <param name="request">An optional `CustomerUpdateRequest` representing the body for this update request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single customer resource</returns>
        public Task<CustomerResponse> UpdateAsync(
            string identity,
            CustomerUpdateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new CustomerUpdateRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CustomerResponse>(
                "PUT",
                "/customers/:identity",
                urlParams,
                request,
                null,
                "customers",
                customiseRequestMessage
            );
        }

        /// <summary>
        ///  Removed customers will not appear in search results or lists of
        ///  customers (in our API
        ///  or exports), and it will not be possible to load an individually
        ///  removed customer by
        ///  ID.
        ///
        ///  <p class="restricted-notice"><strong>The action of removing a
        ///  customer cannot be reversed, so please use with care.</strong></p>
        /// </summary>
        ///  <param name="identity">Unique identifier, beginning with "CU".</param>
        /// <param name="request">An optional `CustomerRemoveRequest` representing the body for this remove request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single customer resource</returns>
        public Task<CustomerResponse> RemoveAsync(
            string identity,
            CustomerRemoveRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new CustomerRemoveRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<CustomerResponse>(
                "DELETE",
                "/customers/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    ///  Creates a new customer object.
    /// </summary>
    public class CustomerCreateRequest : IHasIdempotencyKey
    {
        /// <summary>
        ///  The first line of the customer's address.
        /// </summary>
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        ///  The second line of the customer's address.
        /// </summary>
        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        ///  The third line of the customer's address.
        /// </summary>
        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }

        /// <summary>
        ///  The city of the customer's address.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        ///  Customer's company name. Required unless a `given_name` and
        ///  `family_name` are provided. For Canadian customers, the use of a
        ///  `company_name` value will mean that any mandate created from this
        ///  customer will be considered to be a "Business PAD" (otherwise, any
        ///  mandate will be considered to be a "Personal PAD").
        /// </summary>
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        ///  [ISO 3166-1 alpha-2
        ///  code.](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        ///  For Danish customers only. The civic/company number (CPR or CVR) of
        ///  the customer. Must be supplied if the customer's bank account is
        ///  denominated in Danish krone (DKK).
        /// </summary>
        [JsonProperty("danish_identity_number")]
        public string DanishIdentityNumber { get; set; }

        /// <summary>
        ///  Customer's email address. Required in most cases, as this allows
        ///  GoCardless to send notifications to this customer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        ///  Customer's surname. Required unless a `company_name` is provided.
        /// </summary>
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        /// <summary>
        ///  Customer's first name. Required unless a `company_name` is
        ///  provided.
        /// </summary>
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        /// <summary>
        ///  [ISO 639-1](http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)
        ///  code. Used as the language for notification emails sent by
        ///  GoCardless if your organisation does not send its own (see
        ///  [compliance requirements](#appendix-compliance-requirements)).
        ///  Currently only "en", "fr", "de", "pt", "es", "it", "nl", "da",
        ///  "nb", "sl", "sv" are supported. If this is not provided, the
        ///  language will be chosen based on the `country_code` (if supplied)
        ///  or default to "en".
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        ///  Key-value store of custom data. Up to 3 keys are permitted, with
        ///  key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        ///  [ITU E.123](https://en.wikipedia.org/wiki/E.123) formatted phone
        ///  number, including country code.
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///  The customer's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        ///  The customer's address region, county or department. For US
        ///  customers a 2 letter
        ///  [ISO3166-2:US](https://en.wikipedia.org/wiki/ISO_3166-2:US) state
        ///  code is required (e.g. `CA` for California).
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        ///  For Swedish customers only. The civic/company number (personnummer,
        ///  samordningsnummer, or organisationsnummer) of the customer. Must be
        ///  supplied if the customer's bank account is denominated in Swedish
        ///  krona (SEK). This field cannot be changed once it has been set.
        /// </summary>
        [JsonProperty("swedish_identity_number")]
        public string SwedishIdentityNumber { get; set; }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

    /// <summary>
    ///  Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    ///  customers.
    /// </summary>
    public class CustomerListRequest
    {
        /// <summary>
        ///  Boolean indicating whether the customer has any actions required.
        /// </summary>
        [JsonProperty("action_required")]
        public CustomerActionRequired? ActionRequired { get; set; }

        /// <summary>
        ///  Boolean indicating whether the customer has any actions required.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CustomerActionRequired
        {
            /// <summary>`action_required` with a value of "true"</summary>
            [EnumMember(Value = "true")]
            True,

            /// <summary>`action_required` with a value of "false"</summary>
            [EnumMember(Value = "false")]
            False,
        }

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
        ///  Limit to records created within certain times.
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
        ///  [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        ///  currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        ///  "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public CustomerCurrency? Currency { get; set; }

        /// <summary>
        ///  [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        ///  currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        ///  "SEK" and "USD" are supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CustomerCurrency
        {
            /// <summary>`currency` with a value of "AUD"</summary>
            [EnumMember(Value = "AUD")]
            AUD,

            /// <summary>`currency` with a value of "CAD"</summary>
            [EnumMember(Value = "CAD")]
            CAD,

            /// <summary>`currency` with a value of "DKK"</summary>
            [EnumMember(Value = "DKK")]
            DKK,

            /// <summary>`currency` with a value of "EUR"</summary>
            [EnumMember(Value = "EUR")]
            EUR,

            /// <summary>`currency` with a value of "GBP"</summary>
            [EnumMember(Value = "GBP")]
            GBP,

            /// <summary>`currency` with a value of "NZD"</summary>
            [EnumMember(Value = "NZD")]
            NZD,

            /// <summary>`currency` with a value of "SEK"</summary>
            [EnumMember(Value = "SEK")]
            SEK,

            /// <summary>`currency` with a value of "USD"</summary>
            [EnumMember(Value = "USD")]
            USD,
        }

        /// <summary>
        ///  Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        ///  The direction to sort in.
        ///  One of:
        ///  <ul>
        ///  <li>`asc`</li>
        ///  <li>`desc`</li>
        ///  </ul>
        /// </summary>
        [JsonProperty("sort_direction")]
        public CustomerSortDirection? SortDirection { get; set; }

        /// <summary>
        ///  The direction to sort in.
        ///  One of:
        ///  <ul>
        ///  <li>`asc`</li>
        ///  <li>`desc`</li>
        ///  </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CustomerSortDirection
        {
            /// <summary>`sort_direction` with a value of "asc"</summary>
            [EnumMember(Value = "asc")]
            Asc,

            /// <summary>`sort_direction` with a value of "desc"</summary>
            [EnumMember(Value = "desc")]
            Desc,
        }

        /// <summary>
        ///  Field by which to sort records.
        ///  One of:
        ///  <ul>
        ///  <li>`name`</li>
        ///  <li>`company_name`</li>
        ///  <li>`created_at`</li>
        ///  </ul>
        /// </summary>
        [JsonProperty("sort_field")]
        public CustomerSortField? SortField { get; set; }

        /// <summary>
        ///  Field by which to sort records.
        ///  One of:
        ///  <ul>
        ///  <li>`name`</li>
        ///  <li>`company_name`</li>
        ///  <li>`created_at`</li>
        ///  </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CustomerSortField
        {
            /// <summary>`sort_field` with a value of "name"</summary>
            [EnumMember(Value = "name")]
            Name,

            /// <summary>`sort_field` with a value of "company_name"</summary>
            [EnumMember(Value = "company_name")]
            CompanyName,

            /// <summary>`sort_field` with a value of "created_at"</summary>
            [EnumMember(Value = "created_at")]
            CreatedAt,
        }
    }

    /// <summary>
    ///  Retrieves the details of an existing customer.
    /// </summary>
    public class CustomerGetRequest { }

    /// <summary>
    ///  Updates a customer object. Supports all of the fields supported when
    ///  creating a customer.
    /// </summary>
    public class CustomerUpdateRequest
    {
        /// <summary>
        ///  The first line of the customer's address.
        /// </summary>
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        ///  The second line of the customer's address.
        /// </summary>
        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        ///  The third line of the customer's address.
        /// </summary>
        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }

        /// <summary>
        ///  The city of the customer's address.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        ///  Customer's company name. Required unless a `given_name` and
        ///  `family_name` are provided. For Canadian customers, the use of a
        ///  `company_name` value will mean that any mandate created from this
        ///  customer will be considered to be a "Business PAD" (otherwise, any
        ///  mandate will be considered to be a "Personal PAD").
        /// </summary>
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        ///  [ISO 3166-1 alpha-2
        ///  code.](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        ///  For Danish customers only. The civic/company number (CPR or CVR) of
        ///  the customer. Must be supplied if the customer's bank account is
        ///  denominated in Danish krone (DKK).
        /// </summary>
        [JsonProperty("danish_identity_number")]
        public string DanishIdentityNumber { get; set; }

        /// <summary>
        ///  Customer's email address. Required in most cases, as this allows
        ///  GoCardless to send notifications to this customer.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        ///  Customer's surname. Required unless a `company_name` is provided.
        /// </summary>
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        /// <summary>
        ///  Customer's first name. Required unless a `company_name` is
        ///  provided.
        /// </summary>
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        /// <summary>
        ///  [ISO 639-1](http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)
        ///  code. Used as the language for notification emails sent by
        ///  GoCardless if your organisation does not send its own (see
        ///  [compliance requirements](#appendix-compliance-requirements)).
        ///  Currently only "en", "fr", "de", "pt", "es", "it", "nl", "da",
        ///  "nb", "sl", "sv" are supported. If this is not provided, the
        ///  language will be chosen based on the `country_code` (if supplied)
        ///  or default to "en".
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        ///  Key-value store of custom data. Up to 3 keys are permitted, with
        ///  key names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        ///  [ITU E.123](https://en.wikipedia.org/wiki/E.123) formatted phone
        ///  number, including country code.
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///  The customer's postal code.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        ///  The customer's address region, county or department. For US
        ///  customers a 2 letter
        ///  [ISO3166-2:US](https://en.wikipedia.org/wiki/ISO_3166-2:US) state
        ///  code is required (e.g. `CA` for California).
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        ///  For Swedish customers only. The civic/company number (personnummer,
        ///  samordningsnummer, or organisationsnummer) of the customer. Must be
        ///  supplied if the customer's bank account is denominated in Swedish
        ///  krona (SEK). This field cannot be changed once it has been set.
        /// </summary>
        [JsonProperty("swedish_identity_number")]
        public string SwedishIdentityNumber { get; set; }
    }

    /// <summary>
    ///  Removed customers will not appear in search results or lists of
    ///  customers (in our API
    ///  or exports), and it will not be possible to load an individually
    ///  removed customer by
    ///  ID.
    ///
    ///  <p class="restricted-notice"><strong>The action of removing a customer
    ///  cannot be reversed, so please use with care.</strong></p>
    /// </summary>
    public class CustomerRemoveRequest { }

    /// <summary>
    /// An API response for a request returning a single customer.
    /// </summary>
    public class CustomerResponse : ApiResponse
    {
        /// <summary>
        /// The customer from the response.
        /// </summary>
        [JsonProperty("customers")]
        public Customer Customer { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of customers.
    /// </summary>
    public class CustomerListResponse : ApiResponse
    {
        /// <summary>
        /// The list of customers from the response.
        /// </summary>
        [JsonProperty("customers")]
        public IReadOnlyList<Customer> Customers { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
