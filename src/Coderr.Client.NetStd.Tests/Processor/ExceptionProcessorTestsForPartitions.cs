using System;
using Coderr.Client.NetStd.Config;
using Coderr.Client.NetStd.Processor;
using Coderr.Client.NetStd.Reporters;
using Coderr.Client.NetStd.Tests.Processor.Helpers;
using FluentAssertions;
using Xunit;

namespace Coderr.Client.NetStd.Tests.Processor
{
    public class ExceptionProcessorTestsForPartitions
    {
        [Fact]
        public void Should_include_partitions_in_reports()
        {
            var upl = new TestUploader();
            var config = new CoderrConfiguration();
            var ex = new Exception("hello");
            var ctx = new ErrorReporterContext(this, ex);
            config.Uploaders.Register(upl);
            config.AddPartition(x =>
            {
                x.AddPartition("Id", "42");
            });

            var processor = new ExceptionProcessor(config);
            processor.Process(ctx);

            upl.Report.GetCollectionProperty("Coderr", "ErrPartition.Id").Should().Be("42");
        }
    }
}
