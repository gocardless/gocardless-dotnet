using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoCardless.Internals
{
    public class JsonSerializerSettings : Newtonsoft.Json.JsonSerializerSettings
    {
        public JsonSerializerSettings()
        {
            this.ContractResolver = new GoCardlessDefaultContractResolver();
            NullValueHandling = NullValueHandling.Ignore;
            DateFormatHandling = DateFormatHandling.IsoDateFormat;
            DateParseHandling = DateParseHandling.DateTimeOffset;
            DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }

        public class GoCardlessDefaultContractResolver : DefaultContractResolver
        {
            public GoCardlessDefaultContractResolver()
            {
                this.NamingStrategy = new SnakeCaseNamingStrategy();
            }

            protected override JsonProperty CreateProperty(
                MemberInfo member,
                MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);

                if (!prop.Writable)
                {
                    var property = member as PropertyInfo;
                    if (property != null)
                    {
                        var hasPrivateSetter = property.GetSetMethod(true) != null;
                        prop.Writable = hasPrivateSetter;
                    }
                }

                return prop;
            }
        }
    }
}
