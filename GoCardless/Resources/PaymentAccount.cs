using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    ///  Represents a payment account resource.
    ///
    ///  Access the details of bank accounts provided for you by GoCardless that
    ///  are used to fund [Outbound
    ///  Payments](#core-endpoints-outbound-payments).
    /// </summary>
    public class PaymentAccount
    {
        /// <summary>
        ///  Current balance on a payment account in the lowest denomination for
        ///  the currency (e.g. pence in GBP, cents in EUR).
        ///  It is time-sensitive as it is ever changing.
        /// </summary>
        [JsonProperty("account_balance")]
        public int? AccountBalance { get; set; }

        /// <summary>
        ///  Name of the account holder, as known by the bank. Usually this is
        ///  the same as the name stored with the linked
        ///  [creditor](#core-endpoints-creditors). This field will be
        ///  transliterated, upcased and truncated to 18 characters.
        /// </summary>
        [JsonProperty("account_holder_name")]
        public string AccountHolderName { get; set; }

        /// <summary>
        ///  The last few digits of the account number. Currently 4 digits for
        ///  NZD bank accounts and 2 digits for other currencies.
        /// </summary>
        [JsonProperty("account_number_ending")]
        public string AccountNumberEnding { get; set; }

        /// <summary>
        ///  Name of bank, taken from the bank details.
        /// </summary>
        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        ///  [ISO 4217](http://en.wikipedia.org/wiki/ISO_4217#Active_codes)
        ///  currency code. Currently "AUD", "CAD", "DKK", "EUR", "GBP", "NZD",
        ///  "SEK" and "USD" are supported.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        ///  Unique identifier, beginning with "BA".
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///  Resources linked to this PaymentAccount.
        /// </summary>
        [JsonProperty("links")]
        public PaymentAccountLinks Links { get; set; }
    }

    /// <summary>
    ///  Resources linked to this PaymentAccount
    /// </summary>
    public class PaymentAccountLinks
    {
        /// <summary>
        ///  ID of the [creditor](#core-endpoints-creditors) that owns this bank
        ///  account.
        /// </summary>
        [JsonProperty("creditor")]
        public string Creditor { get; set; }
    }
}
