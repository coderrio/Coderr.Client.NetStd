using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Coderr.Client.ContextCollections;
using FluentAssertions;
using Xunit;

namespace Coderr.Client.NetStd.Tests.ContextCollections
{
    public class CollectionBuilderTests
    {
        [Fact]
        public void CreateForCredentials_should_support_domain_names()
        {
            var identity = new GenericIdentity("myjob\\someone");

            var actual = CollectionBuilder.CreateForCredentials(identity);

            actual.Properties["UserName"].Should().Be("someone");
            actual.Properties["DomainName"].Should().Be("myjob");
        }

        [Fact]
        public void CreateForCredentials_should_work_without_domain_names()
        {
            var identity = new GenericIdentity("someone");

            var actual = CollectionBuilder.CreateForCredentials(identity);

            actual.Properties["UserName"].Should().Be("someone");
            actual.Properties.Should().NotContainKey("DomainName");
        }

        [Fact]
        public void CreateTokenForCredentials_should_support_domain_names()
        {
            var identity = new GenericIdentity("myjob\\someone");
            var expected = CreateMd5Hash("someone");

            var actual = CollectionBuilder.CreateTokenForCredentials(identity);

            actual.Properties["UserToken"].Should().Be(expected);
            actual.Properties["DomainName"].Should().Be("myjob");
        }

        [Fact]
        public void CreateTokenForCredentials_should_work_without_domain_names()
        {
            var identity = new GenericIdentity("someone");
            var expected = CreateMd5Hash("someone");

            var actual = CollectionBuilder.CreateTokenForCredentials(identity);

            actual.Properties["UserToken"].Should().Be(expected);
            actual.Properties.Should().NotContainKey("DomainName");
        }

        private string CreateMd5Hash(string value)
        {
            var buffer = Encoding.UTF8.GetBytes(value);
            var hash = MD5.Create().ComputeHash(buffer);
            return Convert.ToBase64String(hash).TrimEnd('=');
        }
    }
}
