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
    /// Service class for working with payer theme resources.
    ///
    ///  Custom colour themes for payment pages and customer notifications.
    /// </summary>
    public class PayerThemeService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public PayerThemeService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        ///  Creates a new payer theme associated with a creditor. If a creditor
        ///  already has payer themes, this will update the existing payer theme
        ///  linked to the creditor.
        /// </summary>
        /// <param name="request">An optional `PayerThemeCreateForCreditorRequest` representing the body for this create_for_creditor request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single payer theme resource</returns>
        public Task<PayerThemeResponse> CreateForCreditorAsync(
            PayerThemeCreateForCreditorRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new PayerThemeCreateForCreditorRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<PayerThemeResponse>(
                "POST",
                "/branding/payer_themes",
                urlParams,
                request,
                null,
                "payer_themes",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    ///  Creates a new payer theme associated with a creditor. If a creditor
    ///  already has payer themes, this will update the existing payer theme
    ///  linked to the creditor.
    /// </summary>
    public class PayerThemeCreateForCreditorRequest
    {
        /// <summary>
        ///  Colour for buttons background (hexcode)
        /// </summary>
        [JsonProperty("button_background_colour")]
        public string ButtonBackgroundColour { get; set; }

        /// <summary>
        ///  Colour for content box border (hexcode)
        /// </summary>
        [JsonProperty("content_box_border_colour")]
        public string ContentBoxBorderColour { get; set; }

        /// <summary>
        ///  Colour for header background (hexcode)
        /// </summary>
        [JsonProperty("header_background_colour")]
        public string HeaderBackgroundColour { get; set; }

        /// <summary>
        ///  Colour for text links (hexcode)
        /// </summary>
        [JsonProperty("link_text_colour")]
        public string LinkTextColour { get; set; }

        /// <summary>
        ///  Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public PayerThemeLinks Links { get; set; }

        /// <summary>
        ///  Linked resources for a PayerTheme.
        /// </summary>
        public class PayerThemeLinks
        {
            /// <summary>
            ///  ID of the creditor the payer theme belongs to
            /// </summary>
            [JsonProperty("creditor")]
            public string Creditor { get; set; }
        }
    }

    /// <summary>
    /// An API response for a request returning a single payer theme.
    /// </summary>
    public class PayerThemeResponse : ApiResponse
    {
        /// <summary>
        /// The payer theme from the response.
        /// </summary>
        [JsonProperty("payer_themes")]
        public PayerTheme PayerTheme { get; private set; }
    }
}
