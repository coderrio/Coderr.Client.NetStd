using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NSubstitute;
using OneTrueError.Client.Contracts;
using OneTrueError.Client.NetStd.Tests.Processor.Items;
using OneTrueError.Client.Processor;
using Xunit;

namespace OneTrueError.Client.NetStd.Tests.Processor
{
    public class ReportFilterDispatcherTests
    {
        [Fact]
        public void should_block_if_one_of_the_filters_says_no()
        {
            var sut = new ReportFilterDispatcher();
            var report = Substitute.For<ErrorReportDTO>();
            sut.Add(new Filter{Answer = true});
            sut.Add(new Filter { Answer = false });
            sut.Add(new Filter { Answer = true });

            var actual = sut.CanUploadReport(report);

            actual.Should().BeFalse();
        }

        [Fact]
        public void should_allow_upload_if_all_of_the_filters_says_yes()
        {
            var sut = new ReportFilterDispatcher();
            var report = Substitute.For<ErrorReportDTO>();
            sut.Add(new Filter { Answer = true });
            sut.Add(new Filter { Answer = true });
            sut.Add(new Filter { Answer = true });

            var actual = sut.CanUploadReport(report);

            actual.Should().BeTrue();
        }

        [Fact]
        public void answer_should_be_yes_if_there_are_no_filters()
        {
            var sut = new ReportFilterDispatcher();
            var report = Substitute.For<ErrorReportDTO>();

            var actual = sut.CanUploadReport(report);

            actual.Should().BeTrue();
        }
    }
}
