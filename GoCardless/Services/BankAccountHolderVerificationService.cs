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
    /// Service class for working with bank account holder verification resources.
    ///
    /// Create a bank account holder verification for a bank account.
    /// </summary>
    public class BankAccountHolderVerificationService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public BankAccountHolderVerificationService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Verify the account holder of the bank account. A complete
        /// verification can be attached when creating an outbound payment. This
        /// endpoint allows partner merchants to create Confirmation of Payee
        /// checks on customer bank accounts before sending outbound payments.
        /// </summary>
        /// <param name="request">An optional `BankAccountHolderVerificationCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single bank account holder verification resource</returns>
        public Task<BankAccountHolderVerificationResponse> CreateAsync(
            BankAccountHolderVerificationCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BankAccountHolderVerificationCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<BankAccountHolderVerificationResponse>(
                "POST",
                "/bank_account_holder_verifications",
                urlParams,
                request,
                id => GetAsync(id, null, customiseRequestMessage),
                "bank_account_holder_verifications",
                customiseRequestMessage
            );
        }

        /// <summary>
        /// Fetches a bank account holder verification by ID.
        /// </summary>
        /// <param name="identity">The unique identifier for the bank account holder verification
        /// resource, e.g. "BAHV123".</param>
        /// <param name="request">An optional `BankAccountHolderVerificationGetRequest` representing the query parameters for this get request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single bank account holder verification resource</returns>
        public Task<BankAccountHolderVerificationResponse> GetAsync(
            string identity,
            BankAccountHolderVerificationGetRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BankAccountHolderVerificationGetRequest();
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException(nameof(identity));

            var urlParams = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("identity", identity),
            };

            return _goCardlessClient.ExecuteAsync<BankAccountHolderVerificationResponse>(
                "GET",
                "/bank_account_holder_verifications/:identity",
                urlParams,
                request,
                null,
                null,
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// Verify the account holder of the bank account. A complete verification
    /// can be attached when creating an outbound payment. This endpoint allows
    /// partner merchants to create Confirmation of Payee checks on customer
    /// bank accounts before sending outbound payments.
    /// </summary>
    public class BankAccountHolderVerificationCreateRequest : IHasIdempotencyKey
    {
        /// <summary>
        /// Linked resources.
        /// </summary>
        [JsonProperty("links")]
        public BankAccountHolderVerificationLinks Links { get; set; }

        /// <summary>
        /// Linked resources for a BankAccountHolderVerification.
        /// </summary>
        public class BankAccountHolderVerificationLinks
        {
            /// <summary>
            /// The ID of the bank account to verify, e.g. "BA123".
            /// </summary>
            [JsonProperty("bank_account")]
            public string BankAccount { get; set; }
        }

        /// <summary>
        /// Type of the verification that has been performed
        /// eg. [Confirmation of
        /// Payee](https://www.wearepay.uk/what-we-do/overlay-services/confirmation-of-payee/)
        /// </summary>
        [JsonProperty("type")]
        public BankAccountHolderVerificationType? Type { get; set; }

        /// <summary>
        /// Type of the verification that has been performed
        /// eg. [Confirmation of
        /// Payee](https://www.wearepay.uk/what-we-do/overlay-services/confirmation-of-payee/)
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BankAccountHolderVerificationType
        {
            /// <summary>`type` with a value of "confirmation_of_payee"</summary>
            [EnumMember(Value = "confirmation_of_payee")]
            ConfirmationOfPayee,
        }

        /// <summary>
        /// A unique key to ensure that this request only succeeds once, allowing you to safely retry request errors such as network failures.
        /// Any requests, where supported, to create a resource with a key that has previously been used will not succeed.
        /// See: https://developer.gocardless.com/api-reference/#making-requests-idempotency-keys
        /// </summary>
        [JsonIgnore]
        public string IdempotencyKey { get; set; }
    }

    /// <summary>
    /// Fetches a bank account holder verification by ID.
    /// </summary>
    public class BankAccountHolderVerificationGetRequest { }

    /// <summary>
    /// An API response for a request returning a single bank account holder verification.
    /// </summary>
    public class BankAccountHolderVerificationResponse : ApiResponse
    {
        /// <summary>
        /// The bank account holder verification from the response.
        /// </summary>
        [JsonProperty("bank_account_holder_verifications")]
        public BankAccountHolderVerification BankAccountHolderVerification { get; private set; }
    }
}
