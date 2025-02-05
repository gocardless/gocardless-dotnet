using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GoCardless.Internals;

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
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when the
        /// user has been authorised.
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
        /// Fixed [timestamp](#api-usage-time-zones--dates), recording when the
        /// authorisation URL has been visited.
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
        /// This QR code can be used as an alternative to providing the `url` to
        /// the payer to allow them to authorise with their mobile devices.
        /// </summary>
        [JsonProperty("qr_code_url")]
        public string QrCodeUrl { get; set; }

        /// <summary>
        /// URL that the payer can be redirected to after authorising the
        /// payment.
        /// 
        /// On completion of bank authorisation, the query parameter of either
        /// `outcome=success` or `outcome=failure` will be
        /// appended to the `redirect_uri` to indicate the result of the bank
        /// authorisation. If the bank authorisation is
        /// expired, the query parameter `outcome=timeout` will be appended to
        /// the `redirect_uri`, in which case you should
        /// prompt the user to try the bank authorisation step again.
        /// 
        /// Please note: bank authorisations can still fail despite an
        /// `outcome=success` on the `redirect_uri`. It is therefore recommended
        /// to wait for the relevant bank authorisation event, such as
        /// [`BANK_AUTHORISATION_AUTHORISED`](#billing-request-bankauthorisationauthorised),
        /// [`BANK_AUTHORISATION_DENIED`](#billing-request-bankauthorisationdenied),
        /// or
        /// [`BANK_AUTHORISATION_FAILED`](#billing-request-bankauthorisationfailed)
        /// in order to show the correct outcome to the user.
        /// 
        /// The BillingRequestFlow ID will also be appended to the
        /// `redirect_uri` as query parameter `id=BRF123`.
        /// 
        /// Defaults to `https://pay.gocardless.com/billing/static/thankyou`.
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
    public enum BankAuthorisationAuthorisationType {
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
        /// ID of the [billing request](#billing-requests-billing-requests)
        /// against which this authorisation was created.
        /// </summary>
        [JsonProperty("billing_request")]
        public string BillingRequest { get; set; }

        /// <summary>
        /// ID of the [institution](#billing-requests-institutions) against
        /// which this authorisation was created.
        /// </summary>
        [JsonProperty("institution")]
        public string Institution { get; set; }
    }
    
}
