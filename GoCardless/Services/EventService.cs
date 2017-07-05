

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
    /// mandate which has been transferred.
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
        /// <param name="identity">Unique identifier, beginning with "EV".</param>
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
        /// Includes linked resources in the response. Must be used with the
        /// `resource_type` parameter specified. The include should be one of:
 
        ///       /// <ul>
        /// <li>`payment`</li>
        ///
        /// <li>`mandate`</li>
        /// <li>`payout`</li>
        ///
        /// <li>`refund`</li>
        /// <li>`subscription`</li>
        ///
        /// </ul>
        /// </summary>
        [JsonProperty("include")]
        public EventInclude? Include { get; set; }
            
        [JsonConverter(typeof(StringEnumConverter))]
        public enum EventInclude
        {
            /// <summary>
            /// Includes linked resources in the response. Must be used with the
            /// `resource_type` parameter specified. The include should be one
            /// of:
            /// <ul>
            /// <li>`payment`</li>
    
            ///        /// <li>`mandate`</li>
            /// <li>`payout`</li>

            ///            /// <li>`refund`</li>
            ///
            /// <li>`subscription`</li>
            /// </ul>
            /// </summary>
    
            [EnumMember(Value = "payment")]
            Payment,
            [EnumMember(Value = "mandate")]
            Mandate,
            [EnumMember(Value = "payout")]
            Payout,
            [EnumMember(Value = "refund")]
            Refund,
            [EnumMember(Value = "subscription")]
            Subscription,
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
        /// ID of an event. If specified, this endpint will return all events
        /// whose parent_event is the given event ID.
        /// </summary>
        [JsonProperty("parent_event")]
        public string ParentEvent { get; set; }

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
        /// used together with the `payment`, `mandate`, `subscription`,
        /// `refund` or `payout` parameter. The type can be one of:
        ///
        /// <ul>
        /// <li>`payments`</li>
        ///
        /// <li>`mandates`</li>
        /// <li>`payouts`</li>
        ///
        /// <li>`subscriptions`</li>
        /// <li>`refunds`</li>
        ///
        /// </ul>
        /// </summary>
        [JsonProperty("resource_type")]
        public EventResourceType? ResourceType { get; set; }
            
        [JsonConverter(typeof(StringEnumConverter))]
        public enum EventResourceType
        {
            /// <summary>
            /// Type of resource that you'd like to get all events for. Cannot
            /// be used together with the `payment`, `mandate`, `subscription`,
            /// `refund` or `payout` parameter. The type can be one of:
        
            ///    /// <ul>
            /// <li>`payments`</li>
            ///
            /// <li>`mandates`</li>
            /// <li>`payouts`</li>
         
            ///   /// <li>`subscriptions`</li>
            ///
            /// <li>`refunds`</li>
            /// </ul>
            /// </summary>
    
            [EnumMember(Value = "payments")]
            Payments,
            [EnumMember(Value = "mandates")]
            Mandates,
            [EnumMember(Value = "payouts")]
            Payouts,
            [EnumMember(Value = "refunds")]
            Refunds,
            [EnumMember(Value = "subscriptions")]
            Subscriptions,
        }

        /// <summary>
        /// ID of a [subscription](#core-endpoints-subscriptions). If specified,
        /// this endpoint will return all events for the given subscription.
        /// </summary>
        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }

        
    public class EventGetRequest
    {
    }

    public class EventResponse : ApiResponse
    {
        [JsonProperty("events")]
        public Event Event { get; private set; }
    }

    public class EventListResponse : ApiResponse
    {
        public IReadOnlyList<Event> Events { get; private set; }
        public Meta Meta { get; private set; }
    }
}
