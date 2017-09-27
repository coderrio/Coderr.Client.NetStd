﻿using System;
using System.Collections.Generic;
using codeRR.Client.Config;
using codeRR.Client.Contracts;

namespace codeRR.Client.Uploaders
{
    /// <summary>
    ///     Invokes all uploaders for every report.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Every time a complete error report have been collected it need to be uploaded. But since the internet
    ///         connection might be down, there might delivery failures etc we use
    ///         specific classes for uploaded error reports. Those classes is reponsible of handling all that. This class takes
    ///         care of invoking all uploaders.
    ///     </para>
    ///     <para>
    ///         This class uses the <see cref="OneTrueConfiguration.QueueReports" /> to determine if uploads should be done in
    ///         the background (i.e. don't fail on errors, attempt again late).
    ///     </para>
    /// </remarks>
    /// <seealso cref="UploadQueue{T}" />
    public class UploadDispatcher : IUploadDispatcher
    {
        private readonly OneTrueConfiguration _configuration;
        private readonly List<IReportUploader> _uploaders = new List<IReportUploader>();


        /// <summary>
        ///     Creates a new instance of <see cref="UploadDispatcher" />.
        /// </summary>
        /// <param name="configuration">Used to check at runtime of queuing is enabled or not.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public UploadDispatcher(OneTrueConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        

        ///// <summary>
        /////     Max number of items that may wait in queue to get uploaded.
        ///// </summary>
        //public int MaxQueueSize
        //{
        //    get => _reportQueue.MaxQueueSize;
        //    set => _reportQueue.MaxQueueSize = value;
        //}
        
        /// <summary>
        ///     Register an uploader.
        /// </summary>
        /// <param name="uploader">uploader</param>
        /// <exception cref="ArgumentNullException">uploader</exception>
        public void Register(IReportUploader uploader)
        {
            if (uploader == null) throw new ArgumentNullException("uploader");

            //uploader.UploadFailed += OnUploadFailed;
            _uploaders.Add(uploader);
        }

        /// <summary>
        ///     Invoke callbacks
        /// </summary>
        /// <param name="dto">Report to be uploaded.</param>
        /// <returns><c>false</c> if any of the callbacks return <c>false</c>; otherwise <c>true</c></returns>
        /// <remarks>
        ///     <para>
        ///         All callbacks will be invoked, even if one of them returns <c>false</c>.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">dto</exception>
        public void Upload(ErrorReportDTO dto)
        {
            if (dto == null) throw new ArgumentNullException("dto");

            if (dto == null) throw new ArgumentNullException(nameof(dto));

            foreach (var uploader in _uploaders)
                uploader.UploadReport(dto);
        }

        /// <summary>
        ///     Upload feedback.
        /// </summary>
        /// <param name="dto">Feedback provided  by the user.</param>
        /// <exception cref="ArgumentNullException">dto</exception>
        public void Upload(FeedbackDTO dto)
        {
            if (dto == null) throw new ArgumentNullException("dto");

            foreach (var uploader in _uploaders)
                uploader.UploadFeedback(dto);
        }
        
    }
}