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
    /// Service class for working with logo resources.
    ///
    /// Logos are image uploads that, when associated with a creditor, are shown
    /// on the [billing request flow](#billing-requests-billing-request-flows)
    /// payment pages.
    /// </summary>
    public class LogoService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public LogoService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Creates a new logo associated with a creditor. If a creditor already
        /// has a logo, this will update the existing logo linked to the
        /// creditor.
        ///
        /// We support JPG and PNG formats. Your logo will be scaled to a
        /// maximum of 300px by 40px. For more guidance on how to upload logos
        /// that will look
        /// great across your customer payment page and notification emails see
        /// [here](https://developer.gocardless.com/gc-embed/setting-up-branding#tips_for_uploading_your_logo).
        /// </summary>
        /// <param name="request">An optional `LogoCreateForCreditorRequest` representing the body for this create_for_creditor request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single logo resource</returns>
        public Task<LogoResponse> CreateForCreditorAsync(
            LogoCreateForCreditorRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new LogoCreateForCreditorRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<LogoResponse>(
                "POST",
                "/branding/logos",
                urlParams,
                request,
                null,
                "logos",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// Creates a new logo associated with a creditor. If a creditor already has
    /// a logo, this will update the existing logo linked to the creditor.
    ///
    /// We support JPG and PNG formats. Your logo will be scaled to a maximum of
    /// 300px by 40px. For more guidance on how to upload logos that will look
    /// great across your customer payment page and notification emails see
    /// [here](https://developer.gocardless.com/gc-embed/setting-up-branding#tips_for_uploading_your_logo).
    /// </summary>
    public class LogoCreateForCreditorRequest
    {
        /// <summary>
        /// Base64 encoded string.
        /// </summary>
        [JsonProperty("image")]
        public string Image { get; set; }

        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public LogoLinks Links { get; set; }

        /// <summary>
        /// Linked resources for a Logo.
        /// </summary>
        public class LogoLinks
        {
            /// <summary>
            /// ID of the creditor the logo belongs to
            /// </summary>
            [JsonProperty("creditor")]
            public string Creditor { get; set; }
        }
    }

    /// <summary>
    /// An API response for a request returning a single logo.
    /// </summary>
    public class LogoResponse : ApiResponse
    {
        /// <summary>
        /// The logo from the response.
        /// </summary>
        [JsonProperty("logos")]
        public Logo Logo { get; private set; }
    }
}
