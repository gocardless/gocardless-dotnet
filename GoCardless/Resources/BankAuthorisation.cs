using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a bank authorisation resource.
    ///
    /// Bank Authorisations can be used to authorise Billing Requests.
    /// Authorisations
    /// are created against a specific bank, usually the bank that provides the
    /// payer's
    /// account.
    ///
    /// Creation of Bank Authorisations is only permitted from GoCardless hosted
    /// UIs
    /// (see Billing Request Flows) to ensure we meet regulatory requirements
    /// for
    /// checkout flows.
    /// </summary>
    public class BankAuthorisation
    {
        /// <summary>
        /// Type of authorisation, can be either 'mandate' or 'payment'.
        /// </summary>
        [JsonProperty("authorisation_type")]
        public string AuthorisationType { get; set; }

        /// <summary>
        /// Fixed <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-dates-and-times">timestamp</a>,
        /// recording when the user has been authorised.
        /// </summary>
        [JsonProperty("authorised_at")]
        public string AuthorisedAt { get; set; }

        /// <summary>
        /// Timestamp when the flow was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Timestamp when the url will expire. Each authorisation url currently
        /// lasts for 15 minutes, but this can vary by bank.
        /// </summary>
        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }

        /// <summary>
        /// Unique identifier, beginning with "BAU".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Fixed <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-dates-and-times">timestamp</a>,
        /// recording when the authorisation URL has been visited.
        /// </summary>
        [JsonProperty("last_visited_at")]
        public string LastVisitedAt { get; set; }

        /// <summary>
        /// Resources linked to this BankAuthorisation.
        /// </summary>
        [JsonProperty("links")]
        public BankAuthorisationLinks Links { get; set; }

        /// <summary>
        /// URL to a QR code PNG image of the bank authorisation url.
        /// This QR code can be used as an alternative to providing the
        /// <c>url</c> to the payer to allow them to authorise with their mobile
        /// devices.
        /// </summary>
        [JsonProperty("qr_code_url")]
        public string QrCodeUrl { get; set; }

        /// <summary>
        /// URL that the payer can be redirected to after authorising the
        /// payment.
        ///
        /// On completion of bank authorisation, the query parameter of either
        /// <c>outcome=success</c> or <c>outcome=failure</c> will be
        /// appended to the <c>redirect_uri</c> to indicate the result of the
        /// bank authorisation. If the bank authorisation is
        /// expired, the query parameter <c>outcome=timeout</c> will be appended
        /// to the <c>redirect_uri</c>, in which case you should
        /// prompt the user to try the bank authorisation step again.
        ///
        /// Please note: bank authorisations can still fail despite an
        /// <c>outcome=success</c> on the <c>redirect_uri</c>. It is therefore
        /// recommended to wait for the relevant bank authorisation event, such
        /// as <a
        /// href="https://developer.gocardless.com/api-reference/#billing-request-bankauthorisationauthorised"><c>BANK_AUTHORISATION_AUTHORISED</c></a>,
        /// <a
        /// href="https://developer.gocardless.com/api-reference/#billing-request-bankauthorisationdenied"><c>BANK_AUTHORISATION_DENIED</c></a>,
        /// or <a
        /// href="https://developer.gocardless.com/api-reference/#billing-request-bankauthorisationfailed"><c>BANK_AUTHORISATION_FAILED</c></a>
        /// in order to show the correct outcome to the user.
        ///
        /// The BillingRequestFlow ID will also be appended to the
        /// <c>redirect_uri</c> as query parameter <c>id=BRF123</c>.
        ///
        /// Defaults to
        /// <c>https://pay.gocardless.com/billing/static/thankyou</c>.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// URL for an oauth flow that will allow the user to authorise the
        /// payment
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    /// <summary>
    /// Type of authorisation, can be either 'mandate' or 'payment'.
    /// </summary>
    [JsonConverter(typeof(GcStringEnumConverter), (int)Unknown)]
    public enum BankAuthorisationAuthorisationType
    {
        /// <summary>Unknown status</summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        /// <summary>`authorisation_type` with a value of "mandate"</summary>
        [EnumMember(Value = "mandate")]
        Mandate,

        /// <summary>`authorisation_type` with a value of "payment"</summary>
        [EnumMember(Value = "payment")]
        Payment,
    }

    /// <summary>
    /// Resources linked to this BankAuthorisation
    /// </summary>
    public class BankAuthorisationLinks
    {
        /// <summary>
        /// ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#billing-requests-billing-requests">billing
        /// request</a> against which this authorisation was created.
        /// </summary>
        [JsonProperty("billing_request")]
        public string BillingRequest { get; set; }

        /// <summary>
        /// ID of the <a
        /// href="https://developer.gocardless.com/api-reference/#billing-requests-institutions">institution</a>
        /// against which this authorisation was created.
        /// </summary>
        [JsonProperty("institution")]
        public string Institution { get; set; }
    }
}
