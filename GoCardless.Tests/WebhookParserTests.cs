using System;
using System.Linq;
using GoCardless.Exceptions;
using NUnit.Framework;

namespace GoCardless.Tests
{
    public class WebhookParserTests
    {
        private string body = @"{""events"":[{""id"":""EV00BD05S5VM2T"",""created_at"":""2018-07-05T09:13:51.404Z"",""resource_type"":""subscriptions"",""action"":""created"",""links"":{""subscription"":""SB0003JJQ2MR06""},""details"":{""origin"":""api"",""cause"":""subscription_created"",""description"":""Subscription created via the API."", ""not_retried_reason"":""other""},""metadata"":{}},{""id"":""EV00BD05TB8K63"",""created_at"":""2018-07-05T09:13:56.893Z"",""resource_type"":""mandates"",""action"":""created"",""links"":{""mandate"":""MD000AMA19XGEC""},""details"":{""origin"":""api"",""cause"":""mandate_created"",""description"":""Mandate created via the API.""},""metadata"":{}}]}";
        private string signature = "2693754819d3e32d7e8fcb13c729631f316c6de8dc1cf634d6527f1c07276e7e";
        private string key = "ED7D658C-D8EB-4941-948B-3973214F2D49";

        [Test]
        public void ShouldCorrectlyBuildEvents()
        {
            var result = WebhookParser.Parse(body, key, signature);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void ShouldThrowWithInvalidSignature()
        {
            Assert.Throws<InvalidSignatureException>(
                    () => WebhookParser.Parse(body, key, "notatallcorrect"));
        }
    }
}
