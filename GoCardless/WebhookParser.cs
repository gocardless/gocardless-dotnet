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

            if (result != _signatureHeader)
            {
                throw new InvalidSignatureException();
            }
        }
    }
}
