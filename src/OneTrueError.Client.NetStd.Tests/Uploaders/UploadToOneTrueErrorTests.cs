using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Exceptions;
using OneTrueError.Client.Config;
using OneTrueError.Client.Contracts;
using OneTrueError.Client.Uploaders;
using Xunit;

namespace OneTrueError.Client.NetStd.Tests.Uploaders
{
    public class UploadToOneTrueErrorTests
    {
        private TestConfig _config;
        
        public UploadToOneTrueErrorTests()
        {
            _config = new TestConfig
            {
                QueueReportsAccessor = () => false,
                UploadFunc = message => Task.FromResult(new HttpResponseMessage()),
                FeedbackQueue = Substitute.For<IUploadQueue<FeedbackDTO>>(),
                ReportQueue = Substitute.For<IUploadQueue<ErrorReportDTO>>(),
                ThrowExceptionsAccessor = () => false
            };
        }
        [Fact]
        public void should_make_sure_that_only_the_root_is_specified_in_the_uri_so_that_we_may_change_the_specific_uri_in_future_packages()
        {
            var uri = new Uri("http://localhost/receiver/");

            Action actual = () => new UploadToOneTrueError(uri, "ada", "cesar", _config);

            actual.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void should_make_sure_that_the_uri_ends_with_a_slash()
        {
            var report = Substitute.For<ErrorReportDTO>();
            HttpRequestMessage msg = null;
            var uri = new Uri("http://localhost");
            _config.UploadFunc = x =>
            {
                msg = x;
                return Task.FromResult(new HttpResponseMessage());
            };
            

            var sut = new UploadToOneTrueError(uri, "ada", "cesar", _config);
            sut.UploadReport(report);

            msg.RequestUri.ToString().EndsWith("/");
        }

        [Fact]
        public void should_queue_reports_when_specified()
        {
            var report = Substitute.For<ErrorReportDTO>();
            var uri = new Uri("http://localhost");
            _config.QueueReportsAccessor = () => true;

            var sut = new UploadToOneTrueError(uri, "ada", "cesar", _config);
            sut.UploadReport(report);

            _config.ReportQueue.Received().Enqueue(Arg.Any<ErrorReportDTO>());
        }

        [Fact]
        public void should_queue_feedback_when_specified()
        {
            var dto = Substitute.For<FeedbackDTO>();
            var uri = new Uri("http://localhost");
            _config.QueueReportsAccessor = () => true;

            var sut = new UploadToOneTrueError(uri, "ada", "cesar", _config);
            sut.UploadFeedback(dto);

            _config.FeedbackQueue.Received().Enqueue(Arg.Any<FeedbackDTO>());
        }

        [Fact]
        public void should_throw_exceptions_when_upload_fails_when_configured()
        {
            _config.ThrowExceptionsAccessor = () => true;
            var dto = Substitute.For<FeedbackDTO>();
            var uri = new Uri("http://localhost");
            _config.UploadFunc = message => throw new InvalidOperationException("err");

            var sut = new UploadToOneTrueError(uri, "ada", "cesar", _config);
            Action actual = ()=> sut.UploadFeedback(dto);


            actual.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void should_always_throw_when_queing_is_configured_to_allow_retries()
        {
            _config.QueueReportsAccessor = () => true;
            var dto = Substitute.For<FeedbackDTO>();
            var uri = new Uri("http://localhost");
            _config.UploadFunc = message =>
            {
                throw new InvalidOperationException("err");
            };

            var sut = new UploadToOneTrueError(uri, "ada", "cesar", _config);
            Action actual = () => sut.UploadFeedbackNow(dto);


            actual.ShouldThrow<InvalidOperationException>();
        }
    }
}

