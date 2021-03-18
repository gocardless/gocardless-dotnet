

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
    /// Service class for working with event resources.
    ///
    /// Events are stored for all webhooks. An event refers to a resource which
    /// has been updated, for example a payment which has been collected, or a
    /// mandate which has been transferred. See [here](#event-actions) for a
    /// complete list of event types.
    /// </summary>

    public class EventService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public EventService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// your events.
        /// </summary> 
        /// <param name="request">An optional `EventListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of event resources</returns>
        public Task<EventListResponse> ListAsync(EventListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new EventListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<EventListResponse>("GET", "/events", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of events.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Event> All(EventListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new EventListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.Events)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of events.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<Event>>> AllAsync(EventListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new EventListRequest();

            return new TaskEnumerable<IReadOnlyList<Event>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.Events, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of a single event.
        /// </summary>   
        /// <param name="identity">Unique identifier, beginning with
        /// "EV".</param> 
        /// <param name="request">An optional `EventGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single event resource</returns>
        public Task<EventResponse> GetAsync(string identity, EventGetRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new EventGetRequest();
            if (identity == null) throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<EventResponse>("GET", "/events/:identity", urlParams, request, null, null, customiseRequestMessage);
        }
    }

        
    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of your
    /// events.
    /// </summary>
    public class EventListRequest
    {

        /// <summary>
        /// Limit to events with a given `action`.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

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
        /// Includes linked resources in the response. Must be used with the
        /// `resource_type` parameter specified. The include should be one of:
        /// <ul>
        /// <li>`payment`</li>
        /// <li>`mandate`</li>
        /// <li>`payer_authorisation`</li>
        /// <li>`payout`</li>
        /// <li>`refund`</li>
        /// <li>`subscription`</li>
        /// <li>`instalment_schedule`</li>
        /// <li>`creditor`</li>
        /// </ul>
        /// </summary>
        [JsonProperty("include")]
        public EventInclude? Include { get; set; }
            
        /// <summary>
        /// Includes linked resources in the response. Must be used with the
        /// `resource_type` parameter specified. The include should be one of:
        /// <ul>
        /// <li>`payment`</li>
        /// <li>`mandate`</li>
        /// <li>`payer_authorisation`</li>
        /// <li>`payout`</li>
        /// <li>`refund`</li>
        /// <li>`subscription`</li>
        /// <li>`instalment_schedule`</li>
        /// <li>`creditor`</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum EventInclude
        {
    
            /// <summary>`include` with a value of "payment"</summary>
            [EnumMember(Value = "payment")]
            Payment,
            /// <summary>`include` with a value of "mandate"</summary>
            [EnumMember(Value = "mandate")]
            Mandate,
            /// <summary>`include` with a value of "payout"</summary>
            [EnumMember(Value = "payout")]
            Payout,
            /// <summary>`include` with a value of "refund"</summary>
            [EnumMember(Value = "refund")]
            Refund,
            /// <summary>`include` with a value of "subscription"</summary>
            [EnumMember(Value = "subscription")]
            Subscription,
            /// <summary>`include` with a value of "instalment_schedule"</summary>
            [EnumMember(Value = "instalment_schedule")]
            InstalmentSchedule,
            /// <summary>`include` with a value of "creditor"</summary>
            [EnumMember(Value = "creditor")]
            Creditor,
            /// <summary>`include` with a value of "payer_authorisation"</summary>
            [EnumMember(Value = "payer_authorisation")]
            PayerAuthorisation,
        }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// ID of a [mandate](#core-endpoints-mandates). If specified, this
        /// endpoint will return all events for the given mandate.
        /// </summary>
        [JsonProperty("mandate")]
        public string Mandate { get; set; }

        /// <summary>
        /// ID of an event. If specified, this endpoint will return all events
        /// whose parent_event is the given event ID.
        /// </summary>
        [JsonProperty("parent_event")]
        public string ParentEvent { get; set; }

        /// <summary>
        /// ID of a [payer authorisation](#core-endpoints-payer-authorisations).
        /// </summary>
        [JsonProperty("payer_authorisation")]
        public string PayerAuthorisation { get; set; }

        /// <summary>
        /// ID of a [payment](#core-endpoints-payments). If specified, this
        /// endpoint will return all events for the given payment.
        /// </summary>
        [JsonProperty("payment")]
        public string Payment { get; set; }

        /// <summary>
        /// ID of a [payout](#core-endpoints-payouts). If specified, this
        /// endpoint will return all events for the given payout.
        /// </summary>
        [JsonProperty("payout")]
        public string Payout { get; set; }

        /// <summary>
        /// ID of a [refund](#core-endpoints-refunds). If specified, this
        /// endpoint will return all events for the given refund.
        /// </summary>
        [JsonProperty("refund")]
        public string Refund { get; set; }

        /// <summary>
        /// Type of resource that you'd like to get all events for. Cannot be
        /// used together with the `payment`,    `payer_authorisation`,
        /// `mandate`, `subscription`, `instalment_schedule`, `creditor`,
        /// `refund` or `payout` parameter. The type can be one of:
        /// <ul>
        /// <li>`payments`</li>
        /// <li>`mandates`</li>
        /// <li>`payer_authorisations`</li>
        /// <li>`payouts`</li>
        /// <li>`subscriptions`</li>
        /// <li>`instalment_schedules`</li>
        /// <li>`creditors`</li>
        /// <li>`refunds`</li>
        /// </ul>
        /// </summary>
        [JsonProperty("resource_type")]
        public EventResourceType? ResourceType { get; set; }
            
        /// <summary>
        /// Type of resource that you'd like to get all events for. Cannot be
        /// used together with the `payment`,    `payer_authorisation`,
        /// `mandate`, `subscription`, `instalment_schedule`, `creditor`,
        /// `refund` or `payout` parameter. The type can be one of:
        /// <ul>
        /// <li>`payments`</li>
        /// <li>`mandates`</li>
        /// <li>`payer_authorisations`</li>
        /// <li>`payouts`</li>
        /// <li>`subscriptions`</li>
        /// <li>`instalment_schedules`</li>
        /// <li>`creditors`</li>
        /// <li>`refunds`</li>
        /// </ul>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum EventResourceType
        {
    
            /// <summary>`resource_type` with a value of "creditors"</summary>
            [EnumMember(Value = "creditors")]
            Creditors,
            /// <summary>`resource_type` with a value of "instalment_schedules"</summary>
            [EnumMember(Value = "instalment_schedules")]
            InstalmentSchedules,
            /// <summary>`resource_type` with a value of "mandates"</summary>
            [EnumMember(Value = "mandates")]
            Mandates,
            /// <summary>`resource_type` with a value of "payer_authorisations"</summary>
            [EnumMember(Value = "payer_authorisations")]
            PayerAuthorisations,
            /// <summary>`resource_type` with a value of "payments"</summary>
            [EnumMember(Value = "payments")]
            Payments,
            /// <summary>`resource_type` with a value of "payouts"</summary>
            [EnumMember(Value = "payouts")]
            Payouts,
            /// <summary>`resource_type` with a value of "refunds"</summary>
            [EnumMember(Value = "refunds")]
            Refunds,
            /// <summary>`resource_type` with a value of "subscriptions"</summary>
            [EnumMember(Value = "subscriptions")]
            Subscriptions,
            /// <summary>`resource_type` with a value of "organisations"</summary>
            [EnumMember(Value = "organisations")]
            Organisations,
        }

        /// <summary>
        /// ID of a [subscription](#core-endpoints-subscriptions). If specified,
        /// this endpoint will return all events for the given subscription.
        /// </summary>
        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }

        
    /// <summary>
    /// Retrieves the details of a single event.
    /// </summary>
    public class EventGetRequest
    {
    }

    /// <summary>
    /// An API response for a request returning a single event.
    /// </summary>
    public class EventResponse : ApiResponse
    {
        /// <summary>
        /// The event from the response.
        /// </summary>
        [JsonProperty("events")]
        public Event Event { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of events.
    /// </summary>
    public class EventListResponse : ApiResponse
    {
        /// <summary>
        /// The list of events from the response.
        /// </summary>
        [JsonProperty("events")]
        public IReadOnlyList<Event> Events { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
