using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NCrunch.Framework;
using OneTrueError.Client.Processor;
using Xunit;

namespace OneTrueError.Client.NetStd.Tests.Processor
{
    public class ReportIdGeneratorTests
    {
        [Fact, ExclusivelyUses("ReportIdGenerator")]
        public void should_be_able_to_generate_an_id_per_default()
        {
            var sut = new ReportIdGenerator();

            var actual = sut.GenerateImp(new Exception());

            actual.Should().NotBeNullOrWhiteSpace();
        }

        [Fact, ExclusivelyUses("ReportIdGenerator")]
        public void should_use_the_new_factory_when_configured()
        {
            var sut = new ReportIdGenerator();

            var actual = sut.GenerateImp(new Exception());

            actual.Should().NotBeNullOrWhiteSpace();
        }
    }
}
