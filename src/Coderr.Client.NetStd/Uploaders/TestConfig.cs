using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coderr.Client.NetStd.Contracts;

namespace Coderr.Client.NetStd.Uploaders
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