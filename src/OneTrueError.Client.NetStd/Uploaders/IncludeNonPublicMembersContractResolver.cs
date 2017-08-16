using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OneTrueError.Client.Uploaders
{
    /// <summary>
    ///     JSON.NET class which also includes all private fields.
    /// </summary>
    internal class IncludeNonPublicMembersContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            //TODO: Maybe cache
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasPrivateSetter = property.SetMethod != null;
                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }
    }
}