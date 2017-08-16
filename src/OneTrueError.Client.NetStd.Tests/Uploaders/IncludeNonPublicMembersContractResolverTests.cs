using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using OneTrueError.Client.NetStd.Tests.Uploaders.TestObjects;
using OneTrueError.Client.Uploaders;
using Xunit;

namespace OneTrueError.Client.NetStd.Tests.Uploaders
{
    public class IncludeNonPublicMembersContractResolverTests
    {
        [Fact]
        public void should_serialize_private_property()
        {
            var settings =new JsonSerializerSettings {ContractResolver = new IncludeNonPublicMembersContractResolver()};
            var item = new ClassWithPrivateSetter("Hello world");

            var json = JsonConvert.SerializeObject(item, settings);
            var actual = JsonConvert.DeserializeObject<ClassWithPrivateSetter>(json, settings);


            actual.Prop.Should().Be(item.Prop);
        }
    }
}
