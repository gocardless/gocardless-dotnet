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
    /// Service class for working with currency exchange rate resources.
    ///
    /// Currency exchange rates from our foreign exchange provider.
    /// </summary>
    public class CurrencyExchangeRateService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public CurrencyExchangeRateService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// all exchange rates.
        /// </summary>
        /// <param name="request">An optional `CurrencyExchangeRateListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of currency exchange rate resources</returns>
        public Task<CurrencyExchangeRateListResponse> ListAsync(
            CurrencyExchangeRateListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new CurrencyExchangeRateListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<CurrencyExchangeRateListResponse>(
                "GET",
                "/currency_exchange_rates",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of currency exchange rates.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<CurrencyExchangeRate> All(
            CurrencyExchangeRateListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new CurrencyExchangeRateListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.CurrencyExchangeRates)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of currency exchange rates.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<CurrencyExchangeRate>>> AllAsync(
            CurrencyExchangeRateListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new CurrencyExchangeRateListRequest();

            return new TaskEnumerable<IReadOnlyList<CurrencyExchangeRate>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.CurrencyExchangeRates, list.Meta?.Cursors?.After);
            });
        }
    }

    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of all
    /// exchange rates.
    /// </summary>
    public class CurrencyExchangeRateListRequest
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
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Source currency
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// Target currency
        /// </summary>
        [JsonProperty("target")]
        public string Target { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single currency exchange rate.
    /// </summary>
    public class CurrencyExchangeRateResponse : ApiResponse
    {
        /// <summary>
        /// The currency exchange rate from the response.
        /// </summary>
        [JsonProperty("currency_exchange_rates")]
        public CurrencyExchangeRate CurrencyExchangeRate { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of currency exchange rates.
    /// </summary>
    public class CurrencyExchangeRateListResponse : ApiResponse
    {
        /// <summary>
        /// The list of currency exchange rates from the response.
        /// </summary>
        [JsonProperty("currency_exchange_rates")]
        public IReadOnlyList<CurrencyExchangeRate> CurrencyExchangeRates { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
