

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
    /// Look up the name and reachability of a bank.
    /// </summary>

    public class BankDetailsLookupService
    {
        private readonly GoCardlessClient _goCardlessClient;

        /// <summary>
        /// Constructor. Users of this library should not call this. An instance of this
        /// class can be accessed through an initialised GoCardlessClient.
        /// </summary>
        public BankDetailsLookupService(GoCardlessClient goCardlessClient)
        {
            _goCardlessClient = goCardlessClient;
        }

        /// <summary>
        /// Performs a bank details lookup.
        /// 
        /// As part of
        /// the lookup a modulus check and reachability check are performed.
   
        ///     /// 
        /// Bank account details may be supplied using
        /// [local details](#appendix-local-bank-details) or an IBAN.
       
        /// /// 
        /// _Note:_ Usage of this endpoint is monitored. If
        /// your organisation relies on GoCardless for
        /// modulus or
        /// reachability checking but not for payment collection, please get in
        /// touch.
        /// </summary>
        /// <returns>A single bank details lookup resource</returns>
        public Task<BankDetailsLookupResponse> CreateAsync(BankDetailsLookupCreateRequest request = null, RequestSettings customiseRequestMessage = null)
        {
            request = request ?? new BankDetailsLookupCreateRequest();

            var urlParams = new List<KeyValuePair<string, object>>
            {};

            return _goCardlessClient.ExecuteAsync<BankDetailsLookupResponse>("POST", "/bank_details_lookups", urlParams, request, null, "bank_details_lookups", customiseRequestMessage);
        }
    }

        
    public class BankDetailsLookupCreateRequest
    {

        /// <summary>
        /// Bank account number - see [local
        /// details](#appendix-local-bank-details) for more information.
        /// Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Bank code - see [local details](#appendix-local-bank-details) for
        /// more information. Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// Branch code - see [local details](#appendix-local-bank-details) for
        /// more information. Alternatively you can provide an `iban`.
        /// </summary>
        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }

        /// <summary>
        /// [ISO
        /// 3166-1](http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Officially_assigned_code_elements)
        /// alpha-2 code. Must be provided if specifying local details.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// International Bank Account Number. Alternatively you can provide
        /// [local details](#appendix-local-bank-details).
        /// </summary>
        [JsonProperty("iban")]
        public string Iban { get; set; }
    }

    public class BankDetailsLookupResponse : ApiResponse
    {
        [JsonProperty("bank_details_lookups")]
        public BankDetailsLookup BankDetailsLookup { get; private set; }
    }
}
