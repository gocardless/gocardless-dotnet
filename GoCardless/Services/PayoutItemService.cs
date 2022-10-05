

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
    /// Service class for working with payout item resources.
    ///
    /// When we collect a payment on your behalf, we add the money you've
    /// collected to your
    /// GoCardless balance, minus any fees paid. Periodically (usually every
    /// working day),
    /// we take any positive balance in your GoCardless account, and pay it out
    /// to your
    /// nominated bank account.
    /// 
    /// Other actions in your GoCardless account can also affect your balance.
    /// For example,
    /// if a customer charges back a payment, we'll deduct the payment's amount
    /// from your
    /// balance, but add any fees you paid for that payment back to your
    /// balance.
    /// 
    /// The Payout Items API allows you to view, on a per-payout basis, the
    /// credit and debit
    /// items that make up that payout's amount.
    /// 
    /// </summary>

    public class PayoutItemService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public PayoutItemService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// items in the payout.
        /// 
        /// </summary>
        /// <param name="request">An optional `PayoutItemListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of payout item resources</returns>
        public Task<PayoutItemListResponse> ListAsync(PayoutItemListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PayoutItemListRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<PayoutItemListResponse>("GET", "/payout_items", urlParams, request, null, null, customiseRequestMessage);
        }

        /// <summary>
        /// Get a lazily enumerated list of payout items.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<PayoutItem> All(PayoutItemListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PayoutItemListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.PayoutItems)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of payout items.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<PayoutItem>>> AllAsync(PayoutItemListRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new PayoutItemListRequest();

            return new TaskEnumerable<IReadOnlyList<PayoutItem>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.PayoutItems, list.Meta?.Cursors?.After);
            });
        }
    }

        
    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of items
    /// in the payout.
    /// 
    /// </summary>
    public class PayoutItemListRequest
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
        /// Boolean value indicating whether the API should return tax data for
        /// the cutover period of April to August 2020. Defaults to false.
        /// </summary>
        [JsonProperty("include_2020_tax_cutover")]
        public PayoutItemInclude2020TaxCutover? Include2020TaxCutover { get; set; }
            
        /// <summary>
        /// Boolean value indicating whether the API should return tax data for
        /// the cutover period of April to August 2020. Defaults to false.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PayoutItemInclude2020TaxCutover
        {
    
            /// <summary>`include2020_tax_cutover` with a value of "true"</summary>
            [EnumMember(Value = "true")]
            True,
            /// <summary>`include2020_tax_cutover` with a value of "false"</summary>
            [EnumMember(Value = "false")]
            False,
        }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "PO".
        /// </summary>
        [JsonProperty("payout")]
        public string Payout { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single payout item.
    /// </summary>
    public class PayoutItemResponse : ApiResponse
    {
        /// <summary>
        /// The payout item from the response.
        /// </summary>
        [JsonProperty("payout_items")]
        public PayoutItem PayoutItem { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of payout items.
    /// </summary>
    public class PayoutItemListResponse : ApiResponse
    {
        /// <summary>
        /// The list of payout items from the response.
        /// </summary>
        [JsonProperty("payout_items")]
        public IReadOnlyList<PayoutItem> PayoutItems { get; private set; }
        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
