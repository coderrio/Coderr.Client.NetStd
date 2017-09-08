using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneTrueError.Client.Contracts;

namespace OneTrueError.Client.Uploaders
{
    internal class UploadToOneTrueError : IReportUploader
    {
        private readonly string _apiKey;
        private readonly HttpClient _client = new HttpClient();
        private readonly IUploadQueue<FeedbackDTO> _feedbackQueue;
        private readonly Func<bool> _queueReportsAccessor;
        private readonly IUploadQueue<ErrorReportDTO> _reportQueue;
        private readonly Uri _reportUri, _feedbackUri;
        private readonly string _sharedSecret;
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _uploadFunc;
        private readonly Func<bool> _throwExceptionsAccessor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UploadToOneTrueError" /> class.
        /// </summary>
        /// <param name="oneTrueHost">
        ///     Uri to the root of the OneTrueError web. Example.
        ///     <code>http://yourWebServer/OneTrueError/</code>
        /// </param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="sharedSecret">The shared secret.</param>
        /// <exception cref="System.ArgumentNullException">apiKey</exception>
        public UploadToOneTrueError(Uri oneTrueHost, string apiKey, string sharedSecret)
        {
            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException("apiKey");
            if (string.IsNullOrEmpty(sharedSecret)) throw new ArgumentNullException("sharedSecret");

            if (!oneTrueHost.AbsolutePath.EndsWith("/"))
                oneTrueHost = new Uri(oneTrueHost + "/");

            if (oneTrueHost.AbsolutePath.Contains("/receiver/"))
                throw new ArgumentException(
                    "The OneTrueError URI should not contain the reporting area '/receiver/', but should point at the site root.");

            _reportUri = new Uri(oneTrueHost, "receiver/report/" + apiKey + "/");
            _feedbackUri = new Uri(oneTrueHost, "receiver/report/" + apiKey + "/feedback/");
            _apiKey = apiKey;
            _sharedSecret = sharedSecret;

            _feedbackQueue = new UploadQueue<FeedbackDTO>(UploadFeedbackNow);
            _feedbackQueue.UploadFailed += OnUploadFailed;

            _reportQueue = new UploadQueue<ErrorReportDTO>(UploadReportNow);
            _reportQueue.UploadFailed += OnUploadFailed;

            _uploadFunc = message => _client.SendAsync(message);
            _queueReportsAccessor = () => OneTrue.Configuration.QueueReports;
            _throwExceptionsAccessor = () => OneTrue.Configuration.ThrowExceptions;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UploadToOneTrueError" /> class.
        /// </summary>
        /// <param name="oneTrueHost">
        ///     Uri to the root of the OneTrueError web. Example.
        ///     <code>http://yourWebServer/OneTrueError/</code>
        /// </param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="sharedSecret">The shared secret.</param>
        /// <param name="config">parameters for tests</param>
        /// <exception cref="System.ArgumentNullException">apiKey</exception>
        internal UploadToOneTrueError(Uri oneTrueHost, string apiKey, string sharedSecret, TestConfig config)
            : this(oneTrueHost, apiKey, sharedSecret)
        {
            _queueReportsAccessor = config.QueueReportsAccessor;
            _throwExceptionsAccessor = config.ThrowExceptionsAccessor;
            _uploadFunc = config.UploadFunc;
            _feedbackQueue = config.FeedbackQueue;
            _reportQueue = config.ReportQueue;
        }

        public event EventHandler<UploadReportFailedEventArgs> UploadFailed;

        public void UploadFeedback(FeedbackDTO feedback)
        {
            if (_queueReportsAccessor())
                _feedbackQueue.Enqueue(feedback);
            else
                UploadFeedbackNow(feedback);
        }

        public void UploadReport(ErrorReportDTO report)
        {
            if (_queueReportsAccessor())
                _reportQueue.Enqueue(report);
            else
                UploadReportNow(report);
        }

        internal HttpRequestMessage CreateRequest(string uri, object dto)
        {
            var reportJson = JsonConvert.SerializeObject(dto, Formatting.None,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None,
                    ContractResolver =
                        new IncludeNonPublicMembersContractResolver()
                });
            var jsonBytes = Encoding.UTF8.GetBytes(reportJson);

            var ms = new MemoryStream();
            using (var gzip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                gzip.Write(jsonBytes, 0, jsonBytes.Length);
            }
            ms.Position = 0;

            byte[] hash;
            var hashAlgo = new HMACSHA256(Encoding.UTF8.GetBytes(_sharedSecret));
            if (ms.TryGetBuffer(out ArraySegment<byte> buffer))
                hash = hashAlgo.ComputeHash(buffer.Array, buffer.Offset, buffer.Count);
            else
            {
                byte[] buf2 = new byte[ms.Length];
                ms.Read(buf2, 0, buf2.Length);
                hash = hashAlgo.ComputeHash(buffer.Array, buffer.Offset, buffer.Count);
            }

            var signature = Convert.ToBase64String(hash);

            var content = new StreamContent(ms);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // this is version 2. Need to push that on the server side first
            // and we also need to calc the signature on the JSON and not the gzipped content
            // content.Headers.ContentEncoding.Add("gzip");

            uri = uri + "?sig=" + signature + "&v=1";
            return new HttpRequestMessage(HttpMethod.Post, uri) {Content = content};
        }

        private void OnUploadFailed(object sender, UploadReportFailedEventArgs e)
        {
            if (UploadFailed != null)
                UploadFailed(this, e);
        }

        private async Task PostData(string uri, object dto)
        {
            var msg = CreateRequest(uri, dto);
            try
            {
                var response = await _uploadFunc(msg);
                if (!response.IsSuccessStatusCode)
                    ProcessResponseError(response);
            }
            catch (Exception ex)
            {
                OnUploadFailed(this, new UploadReportFailedEventArgs(ex, dto));
                if (_throwExceptionsAccessor() || _queueReportsAccessor())
                    throw;
            }
        }

        private void ProcessResponseError(HttpResponseMessage response)
        {
            var title = response.ReasonPhrase;
            var description = response.Content.ReadAsStringAsync().Result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException($"{response.StatusCode}: {title}\r\n{description}");
                case HttpStatusCode.NotFound:
                    //legacy handling of 404. Misconfigured web servers will report 404,
                    //so remove the usage to avoid ambiguity
                    if (title.IndexOf("key", StringComparison.OrdinalIgnoreCase) != -1)
                        throw new InvalidApplicationKeyException($"{response.StatusCode}: {title}\r\n{description}");
                    throw new InvalidOperationException($"{response.StatusCode}: {title}\r\n{description}");
                default:
                    if (response.StatusCode == HttpStatusCode.BadRequest && title.Contains("APP_KEY"))
                        throw new InvalidApplicationKeyException($"{response.StatusCode}: {title}\r\n{description}");
                    throw new InvalidOperationException($"{response.StatusCode}: {title}\r\n{description}");
            }
        }

        internal void UploadFeedbackNow(FeedbackDTO dto)
        {
            PostData(_feedbackUri.ToString(), dto)
                .GetAwaiter()
                .GetResult();
        }

        private void UploadReportNow(ErrorReportDTO dto)
        {
            PostData(_reportUri.ToString(), dto)
                .GetAwaiter()
                .GetResult();
        }
    }
}