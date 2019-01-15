using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoCardless.Internals
{
    public class JsonSerializerSettings : Newtonsoft.Json.JsonSerializerSettings
    {
        public JsonSerializerSettings()
        {
            ContractResolver = new GoCardlessDefaultContractResolver();
            NullValueHandling = NullValueHandling.Ignore;
            DateFormatHandling = DateFormatHandling.IsoDateFormat;
            DateParseHandling = DateParseHandling.DateTimeOffset;
            DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }

        public class GoCardlessDefaultContractResolver : DefaultContractResolver
        {
            public GoCardlessDefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy();
            }

            protected override JsonProperty CreateProperty(
                MemberInfo member,
                MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);

                if (!prop.Writable)
                {
                    if (member is PropertyInfo property)
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
