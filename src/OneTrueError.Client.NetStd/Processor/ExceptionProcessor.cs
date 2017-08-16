using System;
using System.Collections.Generic;
using System.Linq;
using OneTrueError.Client.Config;
using OneTrueError.Client.Contracts;
using OneTrueError.Client.Reporters;

namespace OneTrueError.Client.Processor
{
    /// <summary>
    ///     Will proccess the exception to generate context info and then upload it to the server.
    /// </summary>
    public class ExceptionProcessor
    {
        private readonly OneTrueConfiguration _configuration;

        /// <summary>
        ///     Creates a new instance of <see cref="ExceptionProcessor" />.
        /// </summary>
        /// <param name="configuration">Current configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public ExceptionProcessor(OneTrueConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
        }

        /// <summary>
        ///     Build an report, but do not upload it
        /// </summary>
        /// <param name="exception">caught exception</param>
        /// <remarks>
        ///     <para>
        ///         Will collect context info and generate a report.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">exception</exception>
        public ErrorReportDTO Build(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            var context = new ErrorReporterContext(null, exception);
            return Build(context);
        }

        /// <summary>
        ///     Build an report, but do not upload it
        /// </summary>
        /// <param name="exception">caught exception</param>
        /// <param name="contextData">context data</param>
        /// <remarks>
        ///     <para>
        ///         Will collect context info and generate a report.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">exception;contextData</exception>
        public ErrorReportDTO Build(Exception exception, object contextData)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            if (contextData == null) throw new ArgumentNullException(nameof(contextData));

            var context = new ErrorReporterContext(null, exception);
            AppendCustomContextData(contextData, context.ContextCollections);
            return Build(context);
        }

        /// <summary>
        ///     Build an report, but do not upload it
        /// </summary>
        /// <param name="context">
        ///     context passed to all context providers when collecting information. This context is typically
        ///     implemented by one of the integration libraries to provide more context that can be used to process the
        ///     environment.
        /// </param>
        /// <remarks>
        ///     <para>
        ///         Will collect context info and generate a report.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">exception;contextData</exception>
        public ErrorReportDTO Build(IErrorReporterContext context)
        {
            _configuration.ContextProviders.Collect(context);
            var reportId = ReportIdGenerator.Generate(context.Exception);
            var report = new ErrorReportDTO(reportId, new ExceptionDTO(context.Exception),
                context.ContextCollections.ToArray());
            return report;
        }

        /// <summary>
        ///     Process exception.
        /// </summary>
        /// <param name="exception">caught exception</param>
        /// <remarks>
        ///     <para>
        ///         Will collect context info, generate a report, go through filters and finally upload it.
        ///     </para>
        /// </remarks>
        public void Process(Exception exception)
        {
            var report = Build(exception);
            UploadReportIfAllowed(report);
        }

        /// <summary>
        ///     Process exception.
        /// </summary>
        /// <param name="context">
        ///     Used to reports (like for ASP.NET) can attach information which can be used during the context
        ///     collection pipeline.
        /// </param>
        /// <remarks>
        ///     <para>
        ///         Will collect context info, generate a report, go through filters and finally upload it.
        ///     </para>
        /// </remarks>
        /// <seealso cref="IReportFilter" />
        public void Process(IErrorReporterContext context)
        {
            var report = Build(context);
            UploadReportIfAllowed(report);
        }


        /// <summary>
        ///     Process exception and upload the generated error report (along with context data)
        /// </summary>
        /// <param name="exception">caught exception</param>
        /// <param name="contextData">Context data</param>
        /// <remarks>
        ///     <para>
        ///         Will collect context info, generate a report, go through filters and finally upload it.
        ///     </para>
        ///     <para>
        ///         Do note that reports can be discared if a filter in <c>OneTrue.Configuration.FilterCollection</c> says so.
        ///     </para>
        /// </remarks>
        public void Process(Exception exception, object contextData)
        {
            var report = Build(exception, contextData);
            UploadReportIfAllowed(report);
        }


        private static void AppendCustomContextData(object contextData, IList<ContextCollectionDTO> contextInfo)
        {
            var dtos = contextData as IEnumerable<ContextCollectionDTO>;
            if (dtos != null)
            {
                var arr = dtos;
                foreach (var dto in arr)
                    contextInfo.Add(dto);
            }
            else
            {
                var col = contextData.ToContextCollection();
                contextInfo.Add(col);
            }
        }

        private void UploadReportIfAllowed(ErrorReportDTO report)
        {
            var canUpload = _configuration.FilterCollection.CanUploadReport(report);
            if (!canUpload)
                return;
            _configuration.Uploaders.Upload(report);
        }
    }
}