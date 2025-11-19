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
    /// Service class for working with tax rate resources.
    ///
    /// Tax rates from tax authority.
    ///
    /// We also maintain a [static list of the tax rates for each
    /// jurisdiction](#appendix-tax-rates).
    /// </summary>
    public class TaxRateService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public TaxRateService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of
        /// all tax rates.
        /// </summary>
        /// <param name="request">An optional `TaxRateListRequest` representing the query parameters for this list request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A set of tax rate resources</returns>
        public Task<TaxRateListResponse> ListAsync(
            TaxRateListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new TaxRateListRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<TaxRateListResponse>(
                "GET",
                "/tax_rates",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Get a lazily enumerated list of tax rates.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<TaxRate> All(
            TaxRateListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new TaxRateListRequest();

            string cursor = null;
            do
            {
                request.After = cursor;

                var result = Task.Run(() => ListAsync(request, customiseRequestMessage)).Result;
                foreach (var item in result.TaxRates)
                {
                    yield return item;
                }
                cursor = result.Meta?.Cursors?.After;
            } while (cursor != null);
        }

        /// <summary>
        /// Get a lazily enumerated list of tax rates.
        /// This acts like the #list method, but paginates for you automatically.
        /// </summary>
        public IEnumerable<Task<IReadOnlyList<TaxRate>>> AllAsync(
            TaxRateListRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new TaxRateListRequest();

            return new TaskEnumerable<IReadOnlyList<TaxRate>, string>(async after =>
            {
                request.After = after;
                var list = await this.ListAsync(request, customiseRequestMessage);
                return Tuple.Create(list.TaxRates, list.Meta?.Cursors?.After);
            });
        }

        /// <summary>
        /// Retrieves the details of a tax rate.
        /// </summary>
        /// <param name="identity">The unique identifier created by the jurisdiction, tax type and
        /// version</param>
        /// <param name="request">An optional `TaxRateGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single tax rate resource</returns>
        public Task<TaxRateResponse> GetAsync(
            string identity,
            TaxRateGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new TaxRateGetRequest();
            if (identity == null)
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<TaxRateResponse>(
                "GET",
                "/tax_rates/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// Returns a [cursor-paginated](#api-usage-cursor-pagination) list of all
    /// tax rates.
    /// </summary>
    public class TaxRateListRequest
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
        /// The jurisdiction this tax rate applies to
        /// </summary>
        [JsonProperty("jurisdiction")]
        public string Jurisdiction { get; set; }

        /// <summary>
        /// Number of records to return.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }

    /// <summary>
    /// Retrieves the details of a tax rate.
    /// </summary>
    public class TaxRateGetRequest { }

    /// <summary>
    /// An API response for a request returning a single tax rate.
    /// </summary>
    public class TaxRateResponse : ApiResponse
    {
        /// <summary>
        /// The tax rate from the response.
        /// </summary>
        [JsonProperty("tax_rates")]
        public TaxRate TaxRate { get; private set; }
    }

    /// <summary>
    /// An API response for a request returning a list of tax rates.
    /// </summary>
    public class TaxRateListResponse : ApiResponse
    {
        /// <summary>
        /// The list of tax rates from the response.
        /// </summary>
        [JsonProperty("tax_rates")]
        public IReadOnlyList<TaxRate> TaxRates { get; private set; }

        /// <summary>
        /// Response metadata (e.g. pagination cursors)
        /// </summary>
        public Meta Meta { get; private set; }
    }
}
