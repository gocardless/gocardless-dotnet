using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using GoCardless.Resources;
using GoCardless.Exceptions;
using GoCardless.Services;
using Newtonsoft.Json;

namespace GoCardless
{
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

        public static IReadOnlyList<Event> Parse(string body, string webhookSecret, string signatureHeader)
        {
            var parser = new WebhookParser(body, webhookSecret, signatureHeader);

            return parser.Parse();
        }

        public IReadOnlyList<Event> Parse()
        {
            var response = JsonConvert
                .DeserializeObject<EventListResponse>(_body, new JsonSerializerSettings());

            return response.Events;
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
