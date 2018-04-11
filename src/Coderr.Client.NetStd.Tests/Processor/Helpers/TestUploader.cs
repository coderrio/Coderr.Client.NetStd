using System;
using Coderr.Client.NetStd.Contracts;
using Coderr.Client.NetStd.Uploaders;

namespace Coderr.Client.NetStd.Tests.Processor.Helpers
{
    public class TestUploader : IReportUploader
    {
        public FeedbackDTO Feedback { get; set; }

        public ErrorReportDTO Report { get; set; }

        public event EventHandler<UploadReportFailedEventArgs> UploadFailed;

        public void UploadFeedback(FeedbackDTO feedback)
        {
            Feedback = feedback;
        }

        public void UploadReport(ErrorReportDTO report)
        {
            Report = report;
        }
    }
}