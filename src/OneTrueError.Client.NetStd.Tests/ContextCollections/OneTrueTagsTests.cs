using System;
using FluentAssertions;
using OneTrueError.Client.ContextCollections;
using Xunit;

namespace OneTrueError.Client.NetStd.Tests.ContextCollections
{
    public class OneTrueTagsTests
    {
        [Fact]
        public void should_include_tags_as_a_semicolon_separated_list()
        {
            
            var actual = CollectionBuilder.CreateTags("csharp", "ado.net");

            actual.Properties["Tags"].Should().Be("csharp;ado.net");
        }

        [Fact]
        public void should_not_be_able_to_Set_an_Empty_array_as_that_indicates_an_logical_Error_somewhere_in_the_code()
        {

            Action actual = () => CollectionBuilder.CreateTags();

            actual.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void should_not_be_able_to_Set_null_as_that_indicates_an_logical_Error_somewhere_in_the_code()
        {

            Action actual = () => CollectionBuilder.CreateTags(null);

            actual.ShouldThrow<ArgumentNullException>();
        }
    }
}
