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
    /// Service class for working with bank details lookup resources.
    ///
    /// Look up the name and reachability of a bank account.
    /// </summary>
    public class BankDetailsLookupService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this.
        /// An instance of this class can be accessed through an initialised
        /// GoCardlessClient.
        /// </summary>
        public BankDetailsLookupService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Performs a bank details lookup. As part of the lookup, a modulus
        /// check and
        /// reachability check are performed.
        ///
        /// For UK-based bank accounts, where an account holder name is provided
        /// (and an account number, a sort code or an iban
        /// are already present), we verify that the account holder name and
        /// bank account number match the details held by
        /// the relevant bank.
        ///
        /// If your request returns an <a
        /// href="https://developer.gocardless.com/api-reference/#api-usage-errors">error</a>
        /// or the <c>available_debit_schemes</c>
        /// attribute is an empty array, you will not be able to collect
        /// payments from the
        /// specified bank account. GoCardless may be able to collect payments
        /// from an account
        /// even if no <c>bic</c> is returned.
        ///
        /// Bank account details may be supplied using <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> or an IBAN.
        ///
        /// ACH scheme For compliance reasons, an extra validation step is done
        /// using
        /// a third-party provider to make sure the customer's bank account can
        /// accept
        /// Direct Debit. If a bank account is discovered to be closed or
        /// invalid, the
        /// customer is requested to adjust the account number/routing number
        /// and
        /// succeed in this check to continue with the flow.
        ///
        /// Note: Usage of this endpoint is monitored. If your organisation
        /// relies on GoCardless for
        /// modulus or reachability checking but not for payment collection,
        /// please get in touch.
        /// </summary>
        /// <param name="request">An optional `BankDetailsLookupCreateRequest` representing the body for this create request.</param>
        /// <param name="customiseRequestMessage">An optional `RequestSettings` allowing you to configure the request</param>
        /// <returns>A single bank details lookup resource</returns>
        public Task<BankDetailsLookupResponse> CreateAsync(
            BankDetailsLookupCreateRequest request = null,
            RequestSettings customiseRequestMessage = null
        )
        {
            request = request ?? new BankDetailsLookupCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>> { };

            return _goCardlessClient.ExecuteAsync<BankDetailsLookupResponse>(
                "POST",
                "/bank_details_lookups",
                urlParams,
                request,
                null,
                "bank_details_lookups",
                customiseRequestMessage
            );
        }
    }

    /// <summary>
    /// Performs a bank details lookup. As part of the lookup, a modulus check
    /// and
    /// reachability check are performed.
    ///
    /// For UK-based bank accounts, where an account holder name is provided
    /// (and an account number, a sort code or an iban
    /// are already present), we verify that the account holder name and bank
    /// account number match the details held by
    /// the relevant bank.
    ///
    /// If your request returns an <a
    /// href="https://developer.gocardless.com/api-reference/#api-usage-errors">error</a>
    /// or the <c>available_debit_schemes</c>
    /// attribute is an empty array, you will not be able to collect payments
    /// from the
    /// specified bank account. GoCardless may be able to collect payments from
    /// an account
    /// even if no <c>bic</c> is returned.
    ///
    /// Bank account details may be supplied using <a
    /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
    /// details</a> or an IBAN.
    ///
    /// ACH scheme For compliance reasons, an extra validation step is done
    /// using
    /// a third-party provider to make sure the customer's bank account can
    /// accept
    /// Direct Debit. If a bank account is discovered to be closed or invalid,
    /// the
    /// customer is requested to adjust the account number/routing number and
    /// succeed in this check to continue with the flow.
    ///
    /// Note: Usage of this endpoint is monitored. If your organisation relies
    /// on GoCardless for
    /// modulus or reachability checking but not for payment collection, please
    /// get in touch.
    /// </summary>
    public class BankDetailsLookupCreateRequest
    {
        /// <summary>
        /// The account holder name associated with the account number (if
        /// available). If provided and the country code is GB, a payer name
        /// verification will be performed.
        /// </summary>
        [JsonProperty("account_holder_name")]
        public string AccountHolderName { get; set; }

        /// <summary>
        /// Bank account number - see <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> for more information. Alternatively you can provide an
        /// <c>iban</c>.
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Bank code - see <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> for more information. Alternatively you can provide an
        /// <c>iban</c>.
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// Branch code - see <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a> for more information. Alternatively you can provide an
        /// <c>iban</c>.
        /// </summary>
        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }

        /// <summary>
        /// <a
        /// href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements">ISO
        /// 3166-1</a> alpha-2 code. Must be provided if specifying local
        /// details.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// International Bank Account Number. Alternatively you can provide <a
        /// href="https://developer.gocardless.com/api-reference/#appendix-local-bank-details">local
        /// details</a>.
        /// </summary>
        [JsonProperty("iban")]
        public string Iban { get; set; }
    }

    /// <summary>
    /// An API response for a request returning a single bank details lookup.
    /// </summary>
    public class BankDetailsLookupResponse : ApiResponse
    {
        /// <summary>
        /// The bank details lookup from the response.
        /// </summary>
        [JsonProperty("bank_details_lookups")]
        public BankDetailsLookup BankDetailsLookup { get; private set; }
    }
}
