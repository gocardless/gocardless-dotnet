using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using GoCardless.Exceptions;
using GoCardless.Resources;
using GoCardless.Services;
using Newtonsoft.Json;

namespace GoCardless
{
    public class WebhookParseResult
    {
        public IReadOnlyList<Event> Events { get; }
        public string WebhookId { get; }

        internal WebhookParseResult(IReadOnlyList<Event> events, string webhookId)
        {
            Events = events;
            WebhookId = webhookId;
        }
    }

    public class WebhookParser
    {
        private readonly string _body;
        private readonly string _webhookSecret;
        private readonly string _signatureHeader;

        private WebhookParser(string body, string webhookSecret, string signatureHeader)
        {
            _body = body;
            _webhookSecret = webhookSecret;
            _signatureHeader = signatureHeader;

            verifySignature();
        }

        public static IReadOnlyList<Event> Parse(
            string body,
            string webhookSecret,
            string signatureHeader
        )
        {
            var parser = new WebhookParser(body, webhookSecret, signatureHeader);

            return parser.Parse();
        }

        public static WebhookParseResult ParseWithMeta(
            string body,
            string webhookSecret,
            string signatureHeader
        )
        {
            var parser = new WebhookParser(body, webhookSecret, signatureHeader);

            return parser.ParseWithMetaInternal();
        }

        public IReadOnlyList<Event> Parse()
        {
            var response = JsonConvert.DeserializeObject<EventListResponse>(
                _body,
                new JsonSerializerSettings()
            );

            return response.Events;
        }

        private WebhookParseResult ParseWithMetaInternal()
        {
            var response = JsonConvert.DeserializeObject<WebhookResponse>(
                _body,
                new JsonSerializerSettings()
            );

            return new WebhookParseResult(response.Events, response.Meta?.WebhookId);
        }

        private class WebhookResponse
        {
            [JsonProperty("events")]
            public IReadOnlyList<Event> Events { get; set; }

            [JsonProperty("meta")]
            public WebhookMeta Meta { get; set; }
        }

        private class WebhookMeta
        {
            [JsonProperty("webhook_id")]
            public string WebhookId { get; set; }
        }

        private void verifySignature()
        {
            var hmac256 = new HMACSHA256(Encoding.UTF8.GetBytes(_webhookSecret));
            var computedSignature = hmac256.ComputeHash(Encoding.UTF8.GetBytes(_body));
            var result = BitConverter.ToString(computedSignature).Replace("-", "").ToLower();

            if (!ConstantTimeEquals(result, _signatureHeader))
            {
                throw new InvalidSignatureException();
            }
        }

        // Constant-time comparison: a plain string comparison returns at the first
        // differing character, turning signature verification into a timing oracle.
        private static bool ConstantTimeEquals(string computed, string provided)
        {
            if (computed == null || provided == null)
            {
                return false;
            }

            var computedBytes = Encoding.UTF8.GetBytes(computed);
            var providedBytes = Encoding.UTF8.GetBytes(provided);

#if NETSTANDARD2_1 || NET5_0_OR_GREATER
            // CryptographicOperations.FixedTimeEquals is unavailable on netstandard2.0
            // and net461, hence the manual fallback below.
            return CryptographicOperations.FixedTimeEquals(computedBytes, providedBytes);
#else
            if (computedBytes.Length != providedBytes.Length)
            {
                return false;
            }

            var difference = 0;
            for (var i = 0; i < computedBytes.Length; i++)
            {
                difference |= computedBytes[i] ^ providedBytes[i];
            }

            return difference == 0;
#endif
        }
    }
}
