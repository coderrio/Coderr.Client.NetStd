using System;
using System.Net.Http;
using System.Threading.Tasks;
using OneTrueError.Client.Contracts;

namespace OneTrueError.Client.Uploaders
{
    internal class TestConfig
    {
        public Func<bool> QueueReportsAccessor;
        public Func<bool> ThrowExceptionsAccessor;
        public Func<HttpRequestMessage, Task<HttpResponseMessage>> UploadFunc;
        public IUploadQueue<ErrorReportDTO> ReportQueue;
        public IUploadQueue<FeedbackDTO> FeedbackQueue;

    }
}