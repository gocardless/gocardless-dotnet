using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GoCardless.Resources;
using GoCardless.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace GoCardless.Tests
{
    static class TestHelpers
    {
        public static IEnumerable<string> GetDifferences(JToken left, JToken right)
        {
            if (left.GetType() != right.GetType())
            {
                yield return $"left is {left.GetType()}, right is {right.GetType()}";
                yield break;
            }
            if (left is JObject)
                foreach (var error in GetDifferences((JObject)left, (JObject)right))
                {
                    yield return error;
                }
            else if (left is JValue)
                foreach (var error in GetDifferences((JValue)left, (JValue)right))
                {
                    yield return error;
                }
            else if (left is JArray)
                foreach (var error in GetDifferences((JArray)left, (JArray)right))
                {
                    yield return error;
                }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static IEnumerable<string> GetDifferences(JArray left, JArray right)
        {
            if (left.Count != right.Count)
            {
                yield return $"different array lengths {left.Count} vs {right.Count}";
                yield break;
            }
            for (var i = 0; i < left.Count; i++)
            {
                foreach (var error in GetDifferences(left[i], right[i]))
                {
                    yield return $"[{i}] " + error;
                }
            }
        }

        public static IEnumerable<string> GetDifferences(JObject left, JObject right)
        {
            var ld = left.ToObject<Dictionary<string, object>>();
            var rd = right.ToObject<Dictionary<string, object>>();

            var missingFromRight = ld.Keys.Except(rd.Keys).Where(key => ld[key] != null).ToArray();
            if (missingFromRight.Any())
                yield return $"right is missing keys {string.Join(",", missingFromRight)}";

            var missingFromLeft = rd.Keys.Except(ld.Keys).Where(key => rd[key] != null).ToArray();
            if (missingFromLeft.Any())
                yield return $"left is missing keys {string.Join(",", missingFromLeft)}";

            var inBoth = ld.Keys.Intersect(rd.Keys);
            foreach (var key in inBoth)
            {
                foreach (var error in GetDifferences(left[key], right[key]))
                {
                    yield return $".{key} " + error;
                }
            }
        }

        public static IEnumerable<string> GetDifferences(JValue left, JValue right)
        {
            if (left.Type == JTokenType.Date)
            {
                var leftDate = left.Value<DateTime>();
                var rightDate = right.Value<DateTime>();
                if (leftDate != rightDate)
                    yield return $"{leftDate.ToString("o")} is not equal to {right.ToString("o")}";
            }
            else
            {
                if (!left.Equals(right))
                    yield return $"{left} is not equal to {right}";
            }
        }

        public static void AssertResponseCanSerializeBackToFixture(
            object responseObject,
            string responseFixture
        )
        {
            var json = JsonConvert.SerializeObject(
                responseObject,
                new Internals.JsonSerializerSettings()
            );
            var response = JToken.Parse(json);
            var fixture = LoadJToken(File.ReadAllText(responseFixture));
            var errors = TestHelpers.GetDifferences(response, fixture).ToArray();
            if (errors.Any())
            {
                Assert.Fail(string.Join(", ", errors));
            }
        }

        private static JToken LoadJToken(string json)
        {
            using (var sr = new StringReader(json))
            using (var jr = new JsonTextReader(sr) { DateParseHandling = DateParseHandling.None })
            {
                var j = JToken.ReadFrom(jr);
                return j;
            }
        }

        internal static MandateCreateRequest CreateMandateCreateRequest()
        {
            return new MandateCreateRequest()
            {
                Scheme = "bacs",
                Metadata = new Metadata
                {
                    { "internal_account_ref", "customer_xyz" },
                    { "salesforceId", "000123" },
                    { "12", "34" },
                },
                Links = new MandateCreateRequest.MandateLinks { CustomerBankAccount = "BA000123" },
                IdempotencyKey = "12345",
            };
        }
    }
}
