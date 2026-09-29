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
    /// Service class for working with subscription resources.
    ///
    /// Subscriptions create <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-payments">payments</a>
    /// according to a schedule.
    ///
    /// <h3>Recurrence Rules</h3>
    /// The following rules apply when specifying recurrence:
    ///
    /// <ul>
    /// <li>If <c>day_of_month</c> and <c>start_date</c> are not provided
    /// <c>start_date</c> will be the <a
    /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>'s
    /// <c>next_possible_charge_date</c> and the subscription will then recur
    /// based on the <c>interval</c> &amp; <c>interval_unit</c></li>
    /// <li>If <c>month</c> or <c>day_of_month</c> are present the following
    /// validations apply:</li>
    /// </ul>
    /// | interval_unit | month                                      |
    /// day_of_month                           |
    /// | :---------------- | :--------------------------------------------- |
    /// :----------------------------------------- |
    /// | yearly            | optional (required if <c>day_of_month</c>
    /// provided) | optional (invalid if <c>month</c> not provided) |
    /// | monthly           | invalid                                        |
    /// optional                                   |
    /// | weekly            | invalid                                        |
    /// invalid                                    |
    ///
    /// Examples:
    ///
    /// | interval_unit | interval | month | day_of_month | valid?
    ///                               |
    /// | :---------------- | :----------- | :-------- | :--------------- |
    /// :------------------------------------------------- |
    /// | yearly            | 1            | january   | -1               |
    /// valid                                              |
    /// | monthly           | 6            |           |                  |
    /// valid                                              |
    /// | monthly           | 6            |           | 12               |
    /// valid                                              |
    /// | weekly            | 2            |           |                  |
    /// valid                                              |
    /// | yearly            | 1            | march     |                  |
    /// invalid - missing <c>day_of_month</c>                   |
    /// | yearly            | 1            |           | 2                |
    /// invalid - missing <c>month</c>                          |
    /// | monthly           | 6            | august    | 12               |
    /// invalid - <c>month</c> must be blank                    |
    /// | weekly            | 2            | october   | 10               |
    /// invalid - <c>month</c> and <c>day_of_month</c> must be blank |
    ///
    /// <h3>Rolling dates</h3>
    /// When a charge date falls on a non-business day, one of two things will
    /// happen:
    ///
    /// <ul>
    /// <li>if the recurrence rule specified <c>-1</c> as the
    /// <c>day_of_month</c>, the charge date will be rolled backwards to the
    /// previous business day (i.e., the last working day of the month).</li>
    /// <li>otherwise the charge date will be rolled forwards to the next
    /// business day.</li>
    /// </ul>
    /// </summary>
    public class SubscriptionService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public SubscriptionService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new subscription object
        /// </summary>
        /// <param name="request">An optional `SubscriptionCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single subscription resource</returns>
        public Task<SubscriptionResponse> CreateAsync(
            SubscriptionCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new SubscriptionCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<SubscriptionResponse>(
                "POST",
                "/subscriptions",
                urlParams,
                request,
                id => GetAsync(id, null, customiseRequestMessage),
                "subscriptions",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Returns a <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-cursor-pagination">cursor-paginated</a>
        /// list of your subscriptions. Please note if the subscriptions are
        /// related to customers who have been removed, they will not be shown
        /// in the response.
        /// </summary>
        /// <param name="request">An optional `SubscriptionListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of subscription resources</returns>
        public Task<SubscriptionListResponse> ListAsync(
            SubscriptionListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new SubscriptionListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<SubscriptionListResponse>(
                "GET",
                "/subscriptions",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of subscriptions.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Subscription> All(
            SubscriptionListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new SubscriptionListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Subscriptions)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of subscriptions.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Subscription>>> AllAsync(
            SubscriptionListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new SubscriptionListRequest();

            return new TaskEnumerable<IReadOnlyList<Subscription>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Subscriptions, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of a single subscription.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "SB".
        /// <param name="request">An optional `SubscriptionGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single subscription resource</returns>
        public Task<SubscriptionResponse> GetAsync(
            string identity,
            SubscriptionGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new SubscriptionGetRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<SubscriptionResponse>(
                "GET",
                "/subscriptions/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Updates a subscription object.
        ///
        /// This fails with:
        ///
        /// <ul>
        /// <li>
        /// <c>validation_failed</c> if invalid data is provided when attempting
        /// to update a subscription.</li>
        /// <li>
        /// <c>subscription_not_active</c> if the subscription is no longer
        /// active.</li>
        /// <li>
        /// <c>subscription_already_ended</c> if the subscription has taken all
        /// payments.</li>
        /// <li>
        /// <c>mandate_payments_require_approval</c> if the amount is being
        /// changed and the mandate requires approval.</li>
        /// <li>
        /// <c>number_of_subscription_amendments_exceeded</c> error if the
        /// subscription amount has already been changed 10 times.</li>
        /// <li>
        /// <c>forbidden</c> if the amount is being changed, and the
        /// subscription was created by an app and you are not authenticated as
        /// that app, or if the subscription was not created by an app and you
        /// are authenticated as an app</li>
        /// <li>
        /// <c>resource_created_by_another_app</c> if the app fee is being
        /// changed, and the subscription was created by an app other than the
        /// app you are authenticated as</li>
        /// </ul>
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "SB".
        /// <param name="request">An optional `SubscriptionUpdateRequest` representing the body for this update request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single subscription resource</returns>
        public Task<SubscriptionResponse> UpdateAsync(
            string identity,
            SubscriptionUpdateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new SubscriptionUpdateRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<SubscriptionResponse>(
                "PUT",
                "/subscriptions/:identity",
                urlParams,
                request,
                null,
                "subscriptions",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Pause a subscription object.
        /// No payments will be created until it is resumed.
        ///
        /// This can only be used when a subscription is collecting a fixed
        /// number of payments (created using <c>count</c>),
        /// when they continue forever (created without <c>count</c> or
        /// <c>end_date</c>) or
        /// the subscription is already paused for a number of cycles.
        ///
        /// When <c>pause_cycles</c> is omitted the subscription is paused until
        /// the <a
        /// href="https://developer.gocardless.com/api-reference/#subscriptions-resume-a-subscription">resume
        /// endpoint</a> is called.
        /// If the subscription is collecting a fixed number of payments,
        /// <c>end_date</c> will be set to <c>null</c>.
        /// When paused indefinitely, <c>upcoming_payments</c> will be empty.
        ///
        /// When <c>pause_cycles</c> is provided the subscription will be paused
        /// for the number of cycles requested.
        /// If the subscription is collecting a fixed number of payments,
        /// <c>end_date</c> will be set to a new value.
        /// When paused for a number of cycles, <c>upcoming_payments</c> will
        /// still contain the upcoming charge dates.
        ///
        /// This fails with:
        ///
        /// <ul>
        /// <li>
        /// <c>forbidden</c> if the subscription was created by an app and you
        /// are not authenticated as that app, or if the subscription was not
        /// created by an app and you are authenticated as an app</li>
        /// <li>
        /// <c>validation_failed</c> if invalid data is provided when attempting
        /// to pause a subscription.</li>
        /// <li>
        /// <c>subscription_paused_cannot_update_cycles</c> if the subscription
        /// is already paused for a number of cycles and the request provides a
        /// value for <c>pause_cycle</c>.</li>
        /// <li>
        /// <c>subscription_cannot_be_paused</c> if the subscription cannot be
        /// paused.</li>
        /// <li>
        /// <c>subscription_already_ended</c> if the subscription has taken all
        /// payments.</li>
        /// <li>
        /// <c>pause_cycles_must_be_greater_than_or_equal_to</c> if the provided
        /// value for <c>pause_cycles</c> cannot be satisfied.</li>
        /// </ul>
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "SB".
        /// <param name="request">An optional `SubscriptionPauseRequest` representing the body for this pause request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single subscription resource</returns>
        public Task<SubscriptionResponse> PauseAsync(
            string identity,
            SubscriptionPauseRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new SubscriptionPauseRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<SubscriptionResponse>(
                "POST",
                "/subscriptions/:identity/actions/pause",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Resume a subscription object.
        /// Payments will start to be created again based on the subscriptions
        /// recurrence rules.
        /// The <c>charge_date</c> on the next payment will be the same as the
        /// subscriptions <c>earliest_charge_date_after_resume</c>
        ///
        /// This fails with:
        ///
        /// <ul>
        /// <li>
        /// <c>forbidden</c> if the subscription was created by an app and you
        /// are not authenticated as that app, or if the subscription was not
        /// created by an app and you are authenticated as an app</li>
        /// <li>
        /// <c>validation_failed</c> if invalid data is provided when attempting
        /// to resume a subscription.</li>
        /// <li>
        /// <c>subscription_not_paused</c> if the subscription is not
        /// paused.</li>
        /// </ul>
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "SB".
        /// <param name="request">An optional `SubscriptionResumeRequest` representing the body for this resume request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single subscription resource</returns>
        public Task<SubscriptionResponse> ResumeAsync(
            string identity,
            SubscriptionResumeRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new SubscriptionResumeRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<SubscriptionResponse>(
                "POST",
                "/subscriptions/:identity/actions/resume",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Immediately cancels a subscription; no more payments will be created
        /// under it. Any metadata supplied to this endpoint will be stored on
        /// the payment cancellation event it causes.
        ///
        /// This will fail with a cancellation_failed error if the subscription
        /// is already cancelled or finished.
        /// </summary>
        /// <param name="identity"></param>Unique identifier, beginning with "SB".
        /// <param name="request">An optional `SubscriptionCancelRequest` representing the body for this cancel request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single subscription resource</returns>
        public Task<SubscriptionResponse> CancelAsync(
            string identity,
            SubscriptionCancelRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new SubscriptionCancelRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<SubscriptionResponse>(
                "POST",
                "/subscriptions/:identity/actions/cancel",
                urlParams,
                request,
                null,
                "data",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// Creates a new subscription object
    /// </summary>
    public class SubscriptionCreateRequest : IHasIdempotencyKey
    {
        /// <summary>
        /// Amount in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// The amount to be deducted from each payment as an app fee, to be
        /// paid to the partner integration which created the subscription, in
        /// the lowest denomination for the currency (e.g. pence in GBP, cents
        /// in EUR).
        /// </summary>
        [JsonProperty("app_fee")]
        public int? AppFee { get; set; }

        /// <summary>
        /// The total number of payments that should be taken by this
        /// subscription.
        /// </summary>
        [JsonProperty("count")]
        public int? Count { get; set; }

        /// <summary>
        /// <a href="https://en.wikipedia.org/wiki/ISO_4217#Active_codes">ISO
        /// 4217</a> currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP",
        /// "NZD", "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// As per RFC 2445. The day of the month to charge customers on.
        /// <c>1</c><ul>
        /// <li></li>
        /// </ul><c>28</c> or <c>-1</c> to indicate the last day of the month.
        /// </summary>
        [JsonProperty("day_of_month")]
        public int? DayOfMonth { get; set; }

        /// <summary>
        /// Date on or after which no further payments should be created.
        /// <br></br>
        /// If this field is blank and <c>count</c> is not specified, the
        /// subscription will continue forever.
        /// <br></br>
        ///
        /// <p class="deprecated-notice">Deprecated: This field will be removed
        /// in a future API version. Use <code>count</code> to specify a number
        /// of payments instead.</p>
        /// </summary>
        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        /// <summary>
        /// Number of <c>interval_units</c> between customer charge dates. Must
        /// be greater than or equal to <c>1</c>. Must result in at least one
        /// charge date per year. Defaults to <c>1</c>.
        /// </summary>
        [JsonProperty("interval")]
        public int? Interval { get; set; }

        /// <summary>
        /// The unit of time between customer charge dates. One of
        /// <c>weekly</c>, <c>monthly</c> or <c>yearly</c>.
        /// </summary>
        [JsonProperty("interval_unit")]
        public SubscriptionIntervalUnit? IntervalUnit { get; set; }

        /// <summary>
        /// The unit of time between customer charge dates. One of
        /// <c>weekly</c>, <c>monthly</c> or <c>yearly</c>.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum SubscriptionIntervalUnit
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
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public SubscriptionLinks Links { get; set; }

        /// <summary>
        /// Linked resources for a Subscription.
        /// </summary>
        public class SubscriptionLinks
        {
            /// <summary>
            /// ID of the associated <a
            /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>
            /// which the subscription will create payments against.
            /// </summary>
            [JsonProperty("mandate")]
            public string Mandate { get; set; }
        }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Name of the month on which to charge a customer. Must be lowercase.
        /// Only applies
        /// when the interval_unit is <c>yearly</c>.
        /// </summary>
        [JsonProperty("month")]
        public SubscriptionMonth? Month { get; set; }

        /// <summary>
        /// Name of the month on which to charge a customer. Must be lowercase.
        /// Only applies
        /// when the interval_unit is <c>yearly</c>.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum SubscriptionMonth
        {
            /// <summary>`month` with a value of "january"</summary>
            [EnumMember(Value = "january")]
            January,

            /// <summary>`month` with a value of "february"</summary>
            [EnumMember(Value = "february")]
            February,

            /// <summary>`month` with a value of "march"</summary>
            [EnumMember(Value = "march")]
            March,

            /// <summary>`month` with a value of "april"</summary>
            [EnumMember(Value = "april")]
            April,

            /// <summary>`month` with a value of "may"</summary>
            [EnumMember(Value = "may")]
            May,

            /// <summary>`month` with a value of "june"</summary>
            [EnumMember(Value = "june")]
            June,

            /// <summary>`month` with a value of "july"</summary>
            [EnumMember(Value = "july")]
            July,

            /// <summary>`month` with a value of "august"</summary>
            [EnumMember(Value = "august")]
            August,

            /// <summary>`month` with a value of "september"</summary>
            [EnumMember(Value = "september")]
            September,

            /// <summary>`month` with a value of "october"</summary>
            [EnumMember(Value = "october")]
            October,

            /// <summary>`month` with a value of "november"</summary>
            [EnumMember(Value = "november")]
            November,

            /// <summary>`month` with a value of "december"</summary>
            [EnumMember(Value = "december")]
            December,
        }

        /// <summary>
        /// Optional name for the subscription. This will be set as the
        /// description on each payment created. Must not exceed 255 characters.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// An optional payment reference. This will be set as the reference on
        /// each payment
        /// created and will appear on your customer's bank statement. See the
        /// documentation for
        /// the <a
        /// href="https://developer.gocardless.com/api-reference/#payments-create-a-payment">create
        /// payment endpoint</a> for more details.
        /// <br></br>
        ///
        /// <p class="restricted-notice">Restricted: You need your own Service
        /// User Number to specify a payment reference for Bacs payments.</p>
        /// </summary>
        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        /// <summary>
        /// On failure, automatically retry payments using <a
        /// href="https://developer.gocardless.com/success-plus/overview">intelligent
        /// retries</a>. Default is <c>false</c>. <p class="notice">Important:
        /// To be able to use intelligent retries, Success+ needs to be enabled
        /// in <a href="https://manage.gocardless.com/success-plus">GoCardless
        /// dashboard</a>. </p>
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }

        /// <summary>
        /// The date on which the first payment should be charged. Must be on or
        /// after the <a
        /// href="https://developer.gocardless.com/api-reference/#core-endpoints-mandates">mandate</a>'s
        /// <c>next_possible_charge_date</c>. When left blank and <c>month</c>
        /// or <c>day_of_month</c> are provided, this will be set to the date of
        /// the first payment. If created without <c>month</c> or
        /// <c>day_of_month</c> this will be set as the mandate's
        /// <c>next_possible_charge_date</c>
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

    /// <summary>
    /// Returns a <a
    /// href="https://developer.gocardless.com/api-reference/#api-usage-cursor-pagination">cursor-paginated</a>
    /// list of your subscriptions. Please note if the subscriptions are related
    /// to customers who have been removed, they will not be shown in the
    /// response.
    /// </summary>
    public class SubscriptionListRequest
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
        /// Unique identifier, beginning with "CU".
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "MD". Note that this prefix may
        /// not apply to mandates created before 2016.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// Up to 5 of:
        ///
        /// <ul>
        /// <li><c>pending_customer_approval</c></li>
        /// <li><c>customer_approval_denied</c></li>
        /// <li><c>active</c></li>
        /// <li><c>finished</c></li>
        /// <li><c>cancelled</c></li>
        /// <li><c>paused</c></li>
        /// </ul>
        /// Omit entirely to include subscriptions in all states.
        /// </summary>
        [JsonProperty("status")]
        public string[] Status { get; set; }

        /// <summary>
        /// One of:
        ///
        /// <ul>
        /// <li><c>pending_customer_approval</c>: the subscription is waiting
        /// for customer approval before becoming active</li>
        /// <li><c>customer_approval_denied</c>: the customer did not approve
        /// the subscription</li>
        /// <li><c>active</c>: the subscription is currently active and will
        /// continue to create payments</li>
        /// <li><c>finished</c>: all of the payments scheduled for creation
        /// under this subscription have been created</li>
        /// <li><c>cancelled</c>: the subscription has been cancelled and will
        /// no longer create payments</li>
        /// <li><c>paused</c>: the subscription has been paused and will not
        /// create payments</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum SubscriptionStatus
        {
            /// <summary>`status` with a value of "pending_customer_approval"</summary>
            [EnumMember(Value = "pending_customer_approval")]
            PendingCustomerApproval,

            /// <summary>`status` with a value of "customer_approval_denied"</summary>
            [EnumMember(Value = "customer_approval_denied")]
            CustomerApprovalDenied,

            /// <summary>`status` with a value of "active"</summary>
            [EnumMember(Value = "active")]
            Active,

            /// <summary>`status` with a value of "finished"</summary>
            [EnumMember(Value = "finished")]
            Finished,

            /// <summary>`status` with a value of "cancelled"</summary>
            [EnumMember(Value = "cancelled")]
            Cancelled,

            /// <summary>`status` with a value of "paused"</summary>
            [EnumMember(Value = "paused")]
            Paused,
        }
    }

    /// <summary>
    /// Retrieves the details of a single subscription.
    /// </summary>
    public class SubscriptionGetRequest { }

    /// <summary>
    /// Updates a subscription object.
    ///
    /// This fails with:
    ///
    /// <ul>
    /// <li>
    /// <c>validation_failed</c> if invalid data is provided when attempting to
    /// update a subscription.</li>
    /// <li>
    /// <c>subscription_not_active</c> if the subscription is no longer
    /// active.</li>
    /// <li>
    /// <c>subscription_already_ended</c> if the subscription has taken all
    /// payments.</li>
    /// <li>
    /// <c>mandate_payments_require_approval</c> if the amount is being changed
    /// and the mandate requires approval.</li>
    /// <li>
    /// <c>number_of_subscription_amendments_exceeded</c> error if the
    /// subscription amount has already been changed 10 times.</li>
    /// <li>
    /// <c>forbidden</c> if the amount is being changed, and the subscription
    /// was created by an app and you are not authenticated as that app, or if
    /// the subscription was not created by an app and you are authenticated as
    /// an app</li>
    /// <li>
    /// <c>resource_created_by_another_app</c> if the app fee is being changed,
    /// and the subscription was created by an app other than the app you are
    /// authenticated as</li>
    /// </ul>
    /// </summary>
    public class SubscriptionUpdateRequest
    {
        /// <summary>
        /// Amount in the lowest denomination for the currency (e.g. pence in
        /// GBP, cents in EUR).
        /// </summary>
        [JsonProperty("amount")]
        public int? Amount { get; set; }

        /// <summary>
        /// The amount to be deducted from each payment as an app fee, to be
        /// paid to the partner integration which created the subscription, in
        /// the lowest denomination for the currency (e.g. pence in GBP, cents
        /// in EUR).
        /// </summary>
        [JsonProperty("app_fee")]
        public int? AppFee { get; set; }

        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Optional name for the subscription. This will be set as the
        /// description on each payment created. Must not exceed 255 characters.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// An optional payment reference. This will be set as the reference on
        /// each payment
        /// created and will appear on your customer's bank statement. See the
        /// documentation for
        /// the <a
        /// href="https://developer.gocardless.com/api-reference/#payments-create-a-payment">create
        /// payment endpoint</a> for more details.
        /// <br></br>
        ///
        /// <p class="restricted-notice">Restricted: You need your own Service
        /// User Number to specify a payment reference for Bacs payments.</p>
        /// </summary>
        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        /// <summary>
        /// On failure, automatically retry payments using <a
        /// href="https://developer.gocardless.com/success-plus/overview">intelligent
        /// retries</a>. Default is <c>false</c>. <p class="notice">Important:
        /// To be able to use intelligent retries, Success+ needs to be enabled
        /// in <a href="https://manage.gocardless.com/success-plus">GoCardless
        /// dashboard</a>. </p>
        /// </summary>
        [JsonProperty("retry_if_possible")]
        public bool? RetryIfPossible { get; set; }
    }

    /// <summary>
    /// Pause a subscription object.
    /// No payments will be created until it is resumed.
    ///
    /// This can only be used when a subscription is collecting a fixed number
    /// of payments (created using <c>count</c>),
    /// when they continue forever (created without <c>count</c> or
    /// <c>end_date</c>) or
    /// the subscription is already paused for a number of cycles.
    ///
    /// When <c>pause_cycles</c> is omitted the subscription is paused until the
    /// <a
    /// href="https://developer.gocardless.com/api-reference/#subscriptions-resume-a-subscription">resume
    /// endpoint</a> is called.
    /// If the subscription is collecting a fixed number of payments,
    /// <c>end_date</c> will be set to <c>null</c>.
    /// When paused indefinitely, <c>upcoming_payments</c> will be empty.
    ///
    /// When <c>pause_cycles</c> is provided the subscription will be paused for
    /// the number of cycles requested.
    /// If the subscription is collecting a fixed number of payments,
    /// <c>end_date</c> will be set to a new value.
    /// When paused for a number of cycles, <c>upcoming_payments</c> will still
    /// contain the upcoming charge dates.
    ///
    /// This fails with:
    ///
    /// <ul>
    /// <li>
    /// <c>forbidden</c> if the subscription was created by an app and you are
    /// not authenticated as that app, or if the subscription was not created by
    /// an app and you are authenticated as an app</li>
    /// <li>
    /// <c>validation_failed</c> if invalid data is provided when attempting to
    /// pause a subscription.</li>
    /// <li>
    /// <c>subscription_paused_cannot_update_cycles</c> if the subscription is
    /// already paused for a number of cycles and the request provides a value
    /// for <c>pause_cycle</c>.</li>
    /// <li>
    /// <c>subscription_cannot_be_paused</c> if the subscription cannot be
    /// paused.</li>
    /// <li>
    /// <c>subscription_already_ended</c> if the subscription has taken all
    /// payments.</li>
    /// <li>
    /// <c>pause_cycles_must_be_greater_than_or_equal_to</c> if the provided
    /// value for <c>pause_cycles</c> cannot be satisfied.</li>
    /// </ul>
    /// </summary>
    public class SubscriptionPauseRequest
    {
        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// The number of cycles to pause a subscription for. A cycle is one
        /// duration of <c>interval</c> and <c>interval_unit</c>. This should be
        /// a non zero positive value.
        /// For AUD subscriptions with <c>interval_unit: weekly</c> the minimum
        /// value varies between <c>3</c> &amp; <c>4</c> because of the <a
        /// href="https://developer.gocardless.com/api-reference/#subscriptions-resume-a-subscription">mandatory
        /// minimum waiting period</a>.
        /// For NZD subscriptions with <c>interval_unit: weekly</c> the minimum
        /// value is <c>2</c> because of the <a
        /// href="https://developer.gocardless.com/api-reference/#subscriptions-resume-a-subscription">mandatory
        /// minimum waiting period</a>.
        /// </summary>
        [JsonProperty("pause_cycles")]
        public int? PauseCycles { get; set; }
    }

    /// <summary>
    /// Resume a subscription object.
    /// Payments will start to be created again based on the subscriptions
    /// recurrence rules.
    /// The <c>charge_date</c> on the next payment will be the same as the
    /// subscriptions <c>earliest_charge_date_after_resume</c>
    ///
    /// This fails with:
    ///
    /// <ul>
    /// <li>
    /// <c>forbidden</c> if the subscription was created by an app and you are
    /// not authenticated as that app, or if the subscription was not created by
    /// an app and you are authenticated as an app</li>
    /// <li>
    /// <c>validation_failed</c> if invalid data is provided when attempting to
    /// resume a subscription.</li>
    /// <li>
    /// <c>subscription_not_paused</c> if the subscription is not paused.</li>
    /// </ul>
    /// </summary>
    public class SubscriptionResumeRequest
    {
        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }

    /// <summary>
    /// Immediately cancels a subscription; no more payments will be created
    /// under it. Any metadata supplied to this endpoint will be stored on the
    /// payment cancellation event it causes.
    ///
    /// This will fail with a cancellation_failed error if the subscription is
    /// already cancelled or finished.
    /// </summary>
    public class SubscriptionCancelRequest
    {
        /// <summary>
        /// Key-value store of custom data. Up to 3 keys are permitted, with key
        /// names up to 50 characters and values up to 500 characters.
        /// </summary>
        [JsonProperty("metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single subscription.
    /// </summary>
    public class SubscriptionResponse : ApiResponse
    {
        /// <summary>
        /// The subscription from the response.
        /// </summary>
        [JsonProperty("subscriptions")]
        public Subscription Subscription { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of subscriptions.
    /// </summary>
    public class SubscriptionListResponse : ApiResponse
    {
        /// <summary>
        /// The list of subscriptions from the response.
        /// </summary>
        [JsonProperty("subscriptions")]
        public IReadOnlyList<Subscription> Subscriptions { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
