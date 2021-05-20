

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
    /// Service class for working with instalment schedule resources.
    ///
    /// Instalment schedules are objects which represent a collection of related
    /// payments, with the
    /// intention to collect the `total_amount` specified. The API supports both
    /// schedule-based
    /// creation (similar to subscriptions) as well as explicit selection of
    /// differing payment
    /// amounts and charge dates.
    /// 
    /// Unlike subscriptions, the payments are created immediately, so the
    /// instalment schedule
    /// cannot be modified once submitted and instead can only be cancelled
    /// (which will cancel
    /// any of the payments which have not yet been submitted).
    /// 
    /// Customers will receive a single notification about the complete schedule
    /// of collection.
    /// 
    /// </summary>

    public class InstalmentScheduleService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public InstalmentScheduleService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new instalment schedule object, along with the associated
        /// payments. This
        /// API is recommended if you know the specific dates you wish to
        /// charge. Otherwise,
        /// please check out the [scheduling
        /// version](#instalment-schedules-create-with-schedule).
        /// 
        /// The `instalments` property is an array of payment properties
        /// (`amount` and
        /// `charge_date`).
        /// 
        /// It can take quite a while to create the associated payments, so the
        /// API will return
        /// the status as `pending` initially. When processing has completed, a
        /// subsequent GET
        /// request for the instalment schedule will either have the status
        /// `success` and link
        /// to the created payments, or the status `error` and detailed
        /// information about the
        /// failures.
        /// </summary>
        /// <param name="request">An optional `InstalmentScheduleCreateWithDatesRequest` representing the body for this create_with_dates request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single instalment schedule resource</returns>
        public Task<InstalmentScheduleResponse> CreateWithDatesAsync(InstalmentScheduleCreateWithDatesRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new InstalmentScheduleCreateWithDatesRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<InstalmentScheduleResponse>("POST", "/instalment_schedules", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "instalment_schedules", customiseRequestMessage);
        }

        /// <summary>
        /// Creates a new instalment schedule object, along with the associated
        /// payments. This
        /// API is recommended if you wish to use the GoCardless scheduling
        /// logic. For finer
        /// control over the individual dates, please check out the [alternative
        /// version](#instalment-schedules-create-with-dates).
        /// 
        /// It can take quite a while to create the associated payments, so the
        /// API will return
        /// the status as `pending` initially. When processing has completed, a
        /// subsequent
        /// GET request for the instalment schedule will either have the status
        /// `success` and link to
        /// the created payments, or the status `error` and detailed information
        /// about the
        /// failures.
        /// </summary>
        /// <param name="request">An optional `InstalmentScheduleCreateWithScheduleRequest` representing the body for this create_with_schedule request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single instalment schedule resource</returns>
        public Task<InstalmentScheduleResponse> CreateWithScheduleAsync(InstalmentScheduleCreateWithScheduleRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new InstalmentScheduleCreateWithScheduleRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<InstalmentScheduleResponse>("POST", "/instalment_schedules", urlParams, request, id => GetAsync(id, null, customiseRequestMessage), "instalment_schedules", customiseRequestMessage);
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your instalment schedules.
        /// </summary>
        /// <param name="request">An optional `InstalmentScheduleListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of instalment schedule resources</returns>
        public Task<InstalmentScheduleListResponse> ListAsync(InstalmentScheduleListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new InstalmentScheduleListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<InstalmentScheduleListResponse>("GET", "/instalment_schedules", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of instalment schedules.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<InstalmentSchedule> All(InstalmentScheduleListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new InstalmentScheduleListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.InstalmentSchedules)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of instalment schedules.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<InstalmentSchedule>>> AllAsync(InstalmentScheduleListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new InstalmentScheduleListRequest();

            return new TaskEnumerable<IReadOnlyList<InstalmentSchedule>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.InstalmentSchedules, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of an existing instalment schedule.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "IS".</param> 
        /// <param name="request">An optional `InstalmentScheduleGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single instalment schedule resource</returns>
        public Task<InstalmentScheduleResponse> GetAsync(string identity, InstalmentScheduleGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new InstalmentScheduleGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<InstalmentScheduleResponse>("GET", "/instalment_schedules/:identity", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Updates an instalment schedule. This accepts only the metadata
        /// parameter.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "IS".</param> 
        /// <param name="request">An optional `InstalmentScheduleUpdateRequest` representing the body for this update request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single instalment schedule resource</returns>
        public Task<InstalmentScheduleResponse> UpdateAsync(string identity, InstalmentScheduleUpdateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new InstalmentScheduleUpdateRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<InstalmentScheduleResponse>("PUT", "/instalment_schedules/:identity", urlParams, request, null, "instalment_schedules", customiseRequestMessage);
        }

        /// <summary>
        /// Immediately cancels an instalment schedule; no further payments will
        /// be collected for it.
        /// 
        /// This will fail with a `cancellation_failed` error if the instalment
        /// schedule is already cancelled or has completed.
        /// </summary>  
        /// <param name="identity">Unique identifier, beginning with "IS".</param> 
        /// <param name="request">An optional `InstalmentScheduleCancelRequest` representing the body for this cancel request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single instalment schedule resource</returns>
        public Task<InstalmentScheduleResponse> CancelAsync(string identity, InstalmentScheduleCancelRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new InstalmentScheduleCancelRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<InstalmentScheduleResponse>("POST", "/instalment_schedules/:identity/actions/cancel", urlParams, request, null, "data", customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Creates a new instalment schedule object, along with the associated
    /// payments. This
    /// API is recommended if you know the specific dates you wish to charge.
    /// Otherwise,
    /// please check out the [scheduling
    /// version](#instalment-schedules-create-with-schedule).
    /// 
    /// The `instalments` property is an array of payment properties (`amount`
    /// and
    /// `charge_date`).
    /// 
    /// It can take quite a while to create the associated payments, so the API
    /// will return
    /// the status as `pending` initially. When processing has completed, a
    /// subsequent GET
    /// request for the instalment schedule will either have the status
    /// `success` and link
    /// to the created payments, or the status `error` and detailed information
    /// about the
    /// failures.
    /// </summary>
    public class InstalmentScheduleCreateWithDatesRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// The amount to be deducted from each payment as an app fee, to be
        /// paid to the partner integration which created the subscription, in
        /// the lowest denomination for the currency (e.g. pence in GBP, cents
        /// in EUR).
        /// </summary>
        [JsonProperty("app_fee")]
        public int? AppFee { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public InstalmentScheduleCurrency? Currency { get; set; }
            
        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum InstalmentScheduleCurrency
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
        /// An explicit array of instalment payments, each specifying at least
        /// an `amount` and `charge_date`.
        /// </summary>
        [JsonProperty("instalments")]
        public InstalmentScheduleInstalments[] Instalments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public class InstalmentScheduleInstalments
        {

            /// <summary>
            /// Amount, in the lowest denomination for the currency (e.g. pence
            /// in GBP, cents in EUR).
            /// </summary>
            [JsonProperty("amount")]
            public int? Amount { get; set; }

            /// <summary>
            /// A future date on which the payment should be collected. If the
            /// date
            /// is before the next_possible_charge_date on the
            /// [mandate](#core-endpoints-mandates), it will be automatically
            /// rolled
            /// forwards to that date.
            /// </summary>
            [JsonProperty("charge_date")]
            public string ChargeDate { get; set; }

            /// <summary>
            /// A human-readable description of the payment. This will be
            /// included in the notification email GoCardless sends to your
            /// customer if your organisation does not send its own
            /// notifications (see [compliance
            /// requirements](#appendix-compliance-requirements)).
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }
        }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public InstalmentScheduleLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a InstalmentSchedule.
        /// </summary>
        public class InstalmentScheduleLinks
        {

            /// <summary>
            /// ID of the associated [mandate](#core-endpoints-mandates) which
            /// the instalment schedule will create payments against.
            /// </summary>
            [JsonProperty("mandate")]
            public string Mandate { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// Name of the instalment schedule, up to 100 chars. This name will
        /// also be
        /// copied to the payments of the instalment schedule if you use
        /// schedule-based creation.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// An optional reference that will appear on your customer's bank
        /// statement. The character limit for this reference is dependent on
        /// the scheme.<br /> <strong>ACH</strong> - 10 characters<br />
        /// <strong>Autogiro</strong> - 11 characters<br />
        /// <strong>Bacs</strong> - 10 characters<br /> <strong>BECS</strong> -
        /// 30 characters<br /> <strong>BECS NZ</strong> - 12 characters<br />
        /// <strong>Betalingsservice</strong> - 30 characters<br />
        /// <strong>PAD</strong> - 12 characters<br /> <strong>SEPA</strong> -
        /// 140 characters<br /> Note that this reference must be unique (for
        /// each merchant) for the BECS scheme as it is a scheme requirement. <p
        /// class='restricted-notice'><strong>Restricted</strong>: You can only
        /// specify a payment reference for Bacs payments (that is, when
        /// collecting from the UK) if you're on the <a
        /// href='https://gocardless.com/pricing'>GoCardless Plus, Pro or
        /// Enterprise packages</a>.</p>
        /// </summary>
        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        /// <summary>
        /// On failure, automatically retry payments using [intelligent
        /// retries](#success-intelligent-retries). Default is `false`.
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }

        /// <summary>
        /// The total amount of the instalment schedule, defined as the sum of
        /// all individual
        /// payments, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in
        /// EUR). If the requested payment amounts do not sum up correctly, a
        /// validation error
        /// will be returned.
        /// </summary>
        [JsonProperty("total_amount")]
        public int? TotalAmount { get; set; }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

        
    /// <summary>
    /// Creates a new instalment schedule object, along with the associated
    /// payments. This
    /// API is recommended if you wish to use the GoCardless scheduling logic.
    /// For finer
    /// control over the individual dates, please check out the [alternative
    /// version](#instalment-schedules-create-with-dates).
    /// 
    /// It can take quite a while to create the associated payments, so the API
    /// will return
    /// the status as `pending` initially. When processing has completed, a
    /// subsequent
    /// GET request for the instalment schedule will either have the status
    /// `success` and link to
    /// the created payments, or the status `error` and detailed information
    /// about the
    /// failures.
    /// </summary>
    public class InstalmentScheduleCreateWithScheduleRequest : IHasIdempotencyKey
    {

        /// <summary>
        /// The amount to be deducted from each payment as an app fee, to be
        /// paid to the partner integration which created the subscription, in
        /// the lowest denomination for the currency (e.g. pence in GBP, cents
        /// in EUR).
        /// </summary>
        [JsonProperty("app_fee")]
        public int? AppFee { get; set; }

        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public InstalmentScheduleCurrency? Currency { get; set; }
            
        /// <summary>
        /// [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        /// currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        /// "SEK" and "USD" are supported.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum InstalmentScheduleCurrency
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
        /// Frequency of the payments you want to create, together with an array
        /// of payment
        /// amounts to be collected, with a specified start date for the first
        /// payment.
        /// 
        /// </summary>
        [JsonProperty("instalments")]
        public InstalmentScheduleInstalments Instalments { get; set; }
        /// <summary>
        /// Frequency of the payments you want to create, together with an array
        /// of payment
        /// amounts to be collected, with a specified start date for the first
        /// payment.
        /// 
        /// </summary>
        public class InstalmentScheduleInstalments
        {

            /// <summary>
            /// List of amounts of each instalment, in the lowest denomination
            /// for the
            /// currency (e.g. pence in GBP, cents in EUR).
            /// 
            /// </summary>
            [JsonProperty("amounts")]
            public int?[] Amounts { get; set; }

            /// <summary>
            /// Number of `interval_units` between charge dates. Must be greater
            /// than or
            /// equal to `1`.
            /// 
            /// </summary>
            [JsonProperty("interval")]
            public int? Interval { get; set; }

            /// <summary>
            /// The unit of time between customer charge dates. One of `weekly`,
            /// `monthly` or `yearly`.
            /// </summary>
            [JsonProperty("interval_unit")]
            public InstalmentScheduleIntervalUnit? IntervalUnit { get; set; }
        /// <summary>
        /// The unit of time between customer charge dates. One of `weekly`,
        /// `monthly` or `yearly`.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum InstalmentScheduleIntervalUnit
        {
    
            /// <summary>`interval_unit` with a value of "weekly"</summary>
            [EnumMember(Value = "weekly")]
            Weekly,
            /// <summary>`interval_unit` with a value of "monthly"</summary>
            [EnumMember(Value = "monthly")]
            Monthly,
            /// <summary>`interval_unit` with a value of "yearly"</summary>
            [EnumMember(Value = "yearly")]
            Yearly,
        }

            /// <summary>
            /// The date on which the first payment should be charged. Must be
            /// on or after the [mandate](#core-endpoints-mandates)'s
            /// `next_possible_charge_date`. When left blank and `month` or
            /// `day_of_month` are provided, this will be set to the date of the
            /// first payment. If created without `month` or `day_of_month` this
            /// will be set as the mandate's `next_possible_charge_date`
            /// </summary>
            [JsonProperty("start_date")]
            public string StartDate { get; set; }
        }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public InstalmentScheduleLinks Links { get; set; }
        /// <summary>
        /// Linked resources for a InstalmentSchedule.
        /// </summary>
        public class InstalmentScheduleLinks
        {

            /// <summary>
            /// ID of the associated [mandate](#core-endpoints-mandates) which
            /// the instalment schedule will create payments against.
            /// </summary>
            [JsonProperty("mandate")]
            public string Mandate { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }

        /// <summary>
        /// Name of the instalment schedule, up to 100 chars. This name will
        /// also be
        /// copied to the payments of the instalment schedule if you use
        /// schedule-based creation.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// An optional reference that will appear on your customer's bank
        /// statement. The character limit for this reference is dependent on
        /// the scheme.<br /> <strong>ACH</strong> - 10 characters<br />
        /// <strong>Autogiro</strong> - 11 characters<br />
        /// <strong>Bacs</strong> - 10 characters<br /> <strong>BECS</strong> -
        /// 30 characters<br /> <strong>BECS NZ</strong> - 12 characters<br />
        /// <strong>Betalingsservice</strong> - 30 characters<br />
        /// <strong>PAD</strong> - 12 characters<br /> <strong>SEPA</strong> -
        /// 140 characters<br /> Note that this reference must be unique (for
        /// each merchant) for the BECS scheme as it is a scheme requirement. <p
        /// class='restricted-notice'><strong>Restricted</strong>: You can only
        /// specify a payment reference for Bacs payments (that is, when
        /// collecting from the UK) if you're on the <a
        /// href='https://gocardless.com/pricing'>GoCardless Plus, Pro or
        /// Enterprise packages</a>.</p>
        /// </summary>
        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        /// <summary>
        /// On failure, automatically retry payments using [intelligent
        /// retries](#success-intelligent-retries). Default is `false`.
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }

        /// <summary>
        /// The total amount of the instalment schedule, defined as the sum of
        /// all individual
        /// payments, in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in
        /// EUR). If the requested payment amounts do not sum up correctly, a
        /// validation error
        /// will be returned.
        /// </summary>
        [JsonProperty("total_amount")]
        public int? TotalAmount { get; set; }

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
    /// instalment schedules.
    /// </summary>
    public class InstalmentScheduleListRequest
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
        /// ID of the associated [customer](#core-endpoints-customers).
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// ID of the associated [mandate](#core-endpoints-mandates) which the
        /// instalment schedule will create payments against.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// At most five valid status values
        /// </summary>
        [JsonProperty("status")]
        public InstalmentScheduleStatus[] Status { get; set; }
        /// <summary>
        /// One of:
        /// <ul>
        /// <li>`pending`: we're waiting for GC to create the payments</li>
        /// <li>`active`: the payments have been created, and the schedule is
        /// active</li>
        /// <li>`creation_failed`: payment creation failed</li>
        /// <li>`completed`: we have passed the date of the final payment and
        /// all payments have been collected</li>
        /// <li>`cancelled`: the schedule has been cancelled</li>
        /// <li>`errored`: one or more payments have failed</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum InstalmentScheduleStatus
        {
    
            /// <summary>`status` with a value of "pending"</summary>
            [EnumMember(Value = "pending")]
            Pending,
            /// <summary>`status` with a value of "active"</summary>
            [EnumMember(Value = "active")]
            Active,
            /// <summary>`status` with a value of "creation_failed"</summary>
            [EnumMember(Value = "creation_failed")]
            CreationFailed,
            /// <summary>`status` with a value of "completed"</summary>
            [EnumMember(Value = "completed")]
            Completed,
            /// <summary>`status` with a value of "cancelled"</summary>
            [EnumMember(Value = "cancelled")]
            Cancelled,
            /// <summary>`status` with a value of "errored"</summary>
            [EnumMember(Value = "errored")]
            Errored,
        }
    }

        
    /// <summary>
    /// Retrieves the details of an existing instalment schedule.
    /// </summary>
    public class InstalmentScheduleGetRequest
    {
    }

        
    /// <summary>
    /// Updates an instalment schedule. This accepts only the metadata
    /// parameter.
    /// </summary>
    public class InstalmentScheduleUpdateRequest
    {

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<String, String> Metadata { get; set; }
    }

        
    /// <summary>
    /// Immediately cancels an instalment schedule; no further payments will be
    /// collected for it.
    /// 
    /// This will fail with a `cancellation_failed` error if the instalment
    /// schedule is already cancelled or has completed.
    /// </summary>
    public class InstalmentScheduleCancelRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single instalment schedule.
    /// </summary>
    public class InstalmentScheduleResponse : ApiResponse
    {
        /// <summary>
        /// The instalment schedule from the response.
        /// </summary>
        [JsonProperty("instalment_schedules")]
        public InstalmentSchedule InstalmentSchedule { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of instalment schedules.
    /// </summary>
    public class InstalmentScheduleListResponse : ApiResponse
    {
        /// <summary>
        /// The list of instalment schedules from the response.
        /// </summary>
        [JsonProperty("instalment_schedules")]
        public IReadOnlyList<InstalmentSchedule> InstalmentSchedules { get; private set; }
        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
