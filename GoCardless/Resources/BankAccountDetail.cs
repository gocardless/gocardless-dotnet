using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GoCardless.Internals;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GoCardless.Resources
{
    /// <summary>
    /// Represents a bank account detail resource.
    ///
    /// Retrieve bank account details in JWE encrypted format
    /// </summary>
    public class BankAccountDetail
    {
        /// <summary>
        /// Base64 URL encoded encrypted payload, in this case bank details.
        /// </summary>
        [JsonProperty("ciphertext")]
        public string Ciphertext { get; set; }

        /// <summary>
        /// Base64 URL encoded symmetric content encryption key, encrypted with
        /// the asymmetric key from your JWKS.
        /// </summary>
        [JsonProperty("encrypted_key")]
        public string EncryptedKey { get; set; }

        /// <summary>
        /// Base64 URL encoded initialization vector, used during content
        /// encryption.
        /// </summary>
        [JsonProperty("iv")]
        public string Iv { get; set; }

        /// <summary>
        /// Base64 URL encoded JWE header values, containing the following keys:
        ///
        ///   - `alg`: the asymmetric encryption type used to encrypt symmetric
        /// key, e.g: `RSA-OAEP`.
        ///   - `enc`: the content encryption type, e.g: `A256GCM`.
        ///   - `kid`: the ID of an RSA-2048 public key, from your JWKS, used to
        /// encrypt the AES key.
        /// </summary>
        [JsonProperty("protected")]
        public string Protected { get; set; }

        /// <summary>
        /// Base64 URL encoded authentication tag, used to verify payload
        /// integrity during decryption.
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }
    }
}
