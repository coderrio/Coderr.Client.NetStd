using System;
using FluentAssertions;
using NSubstitute;
using OneTrueError.Client.Config;
using OneTrueError.Client.Uploaders;
using Xunit;

namespace OneTrueError.Client.NetStd.Tests.Config
{
    public class OneTrueConfigurationTests
    {
        [Fact]
        public void should_initialize_contextProviders_collection_so_that_providers_can_Be_found_And_Registered()
        {

            var sut = new OneTrueConfiguration();

            sut.ContextProviders.Should().NotBeNull();
        }

        [Fact]
        public void should_initialize_FilterCollection_so_that_errorReports_can_be_filtered_out_or_sampled()
        {

            var sut = new OneTrueConfiguration();

            sut.FilterCollection.Should().NotBeNull();
        }

        [Fact]
        public void Credentials_should_add_the_default_uploader()
        {
            var dispatcher = Substitute.For<IUploadDispatcher>();

            var sut = new OneTrueConfiguration(dispatcher);
            sut.Credentials(new Uri("http://localhost"), "ada", "buffen");

            dispatcher.Received().Register(Arg.Any<UploadToOneTrueError>());
        }
    }
}
