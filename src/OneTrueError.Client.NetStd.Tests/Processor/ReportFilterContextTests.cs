using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using OneTrueError.Client.Contracts;
using OneTrueError.Client.Processor;
using Xunit;
using Xunit.Sdk;

namespace OneTrueError.Client.NetStd.Tests.Processor
{
    public class ReportFilterContextTests
    {
        [Fact]
        public void should_allow_reports_per_default()
        {
            var report = new ErrorReportDTO("aa", new ExceptionDTO(new Exception()), new ContextCollectionDTO[0]);

            var sut = new ReportFilterContext(report);

            sut.CanSubmitReport.Should().BeTrue();
        }

        [Fact]
        public void the_report_should_be_mandatory_so_that_we_can_filter_on_it()
        {

            Action actual = () => new ReportFilterContext(null);

            actual.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void the_report_should_be_assign_to_the_property_so_that_we_can_filter_on_it()
        {
            var report = new ErrorReportDTO("aa", new ExceptionDTO(new Exception()), new ContextCollectionDTO[0]);

            var sut = new ReportFilterContext(report);

            sut.Report.Should().BeSameAs(report);
        }
    }
}
