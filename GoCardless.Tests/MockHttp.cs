using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GoCardless.Tests
{
    public class MockHttp : HttpMessageHandler
    {
        private readonly Queue<
            Tuple<HttpResponseMessage, Action<HttpResponseMessage>>
        > _queuedMessages = new Queue<Tuple<HttpResponseMessage, Action<HttpResponseMessage>>>();
        private readonly Queue<Tuple<HttpRequestMessage, string>> _requests =
            new Queue<Tuple<HttpRequestMessage, string>>();

        public void EnqueueResponse(
            int statusCode,
            string pathToBodyDocument,
            Action<HttpResponseMessage> transform = null
        )
        {
            var httpResponseMessage = new HttpResponseMessage((HttpStatusCode)statusCode)
            {
                Content = new StringContent(
                    File.ReadAllText(pathToBodyDocument),
                    Encoding.UTF8,
                    "application/json"
                ),
            };
            _queuedMessages.Enqueue(Tuple.Create(httpResponseMessage, transform));
        }

        public void AssertRequestMade(
            string httpMethod,
            string url,
            string pathToBodyDocument = null,
            Action<Tuple<HttpRequestMessage, string>> handle = null
        )
        {
            var req = _requests.Dequeue();
            ClassicAssert.AreEqual(httpMethod, req.Item1.Method.ToString());
            ClassicAssert.AreEqual(url, req.Item1.RequestUri.PathAndQuery);
            if (pathToBodyDocument != null)
            {
                var errors = TestHelpers.GetDifferences(
                    JToken.Parse(req.Item2),
                    JToken.Parse(File.ReadAllText(pathToBodyDocument))
                );
                if (errors.Any())
                {
                    Assert.Fail(string.Join(", ", errors));
                }
            }
            handle?.Invoke(req);
        }

        private bool HaveEquivalentJson(string left, string right)
        {
            var leftCleaned = JToken.Parse(left).ToString(Formatting.None);
            var rightCleaned = JToken.Parse(right).ToString(Formatting.None);
            return leftCleaned.Equals(rightCleaned);
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            string content =
                request.Content is StringContent
                    ? (await request.Content.ReadAsStringAsync())
                    : null;
            _requests.Enqueue(Tuple.Create(request, content));
            var tuple = _queuedMessages.Dequeue();
            var httpResponseMessage = tuple.Item1;
            var transform = tuple.Item2;
            transform?.Invoke(httpResponseMessage);
            return httpResponseMessage;
        }
    }
}
