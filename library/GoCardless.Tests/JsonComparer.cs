using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoCardless.Tests
{
    class JsonComparer
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
            for(var i=0;i<left.Count;i++)
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

            var missingFromRight = ld.Keys.Except(rd.Keys).ToArray();
            if (missingFromRight.Any()) yield return $"right is missing keys {string.Join(",", missingFromRight)}";

            var missingFromLeft = rd.Keys.Except(ld.Keys).ToArray();
            if (missingFromLeft.Any()) yield return $"left is missing keys {string.Join(",", missingFromLeft)}";

            var inBoth = ld.Keys.Intersect(rd.Keys);
            foreach(var key in inBoth)
            {
                foreach(var error in GetDifferences(left[key], right[key]))
                {
                    yield return $".{key} " + error;
                }
            }
        }

        public static IEnumerable<string> GetDifferences(JValue left, JValue right)
        {
            if (!left.Equals(right)) yield return $"{left} is not equal to ${right}";
        }
    }
}
