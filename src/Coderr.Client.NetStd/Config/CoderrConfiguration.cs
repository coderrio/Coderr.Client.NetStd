using System;
using codeRR.Client.ContextCollections;
using codeRR.Client.Processor;
using codeRR.Client.Uploaders;

namespace codeRR.Client.Config
{
    /// <summary>
    ///     Configuration root object.
    /// </summary>
    public class CoderrConfiguration : IDisposable
    {
        /// <summary>
        ///     Configure how the reporting UI will interact with the user.
        /// </summary>
        private UserInteractionConfiguration _userInteraction = new UserInteractionConfiguration();

        /// <summary>
        ///     Creates a new instance of <see cref="CoderrConfiguration" />.
        /// </summary>
        public CoderrConfiguration()
        {
            Uploaders = new UploadDispatcher(this);
            _userInteraction.AskUserForDetails = true;
            ThrowExceptions = true;
            MaxNumberOfPropertiesPerCollection = 100;
        }


        /// <summary>
        ///     Creates a new instance of <see cref="CoderrConfiguration" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">uploadDispatcher</exception>
        public CoderrConfiguration(IUploadDispatcher uploadDispatcher)
        {
            Uploaders = uploadDispatcher ?? throw new ArgumentNullException(nameof(uploadDispatcher));
            _userInteraction.AskUserForDetails = true;
            ThrowExceptions = true;
            MaxNumberOfPropertiesPerCollection = 100;
        }


        /// <summary>
        ///     Used to add custom context info providers.
        /// </summary>
        public ContextProvidersRegistrar ContextProviders { get; } = new ContextProvidersRegistrar();

        /// <summary>
        ///     Used to decide which reports can be uploaded.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Use it to filter out reports from certain areas in the system or to use sampling in high load systems.
        ///     </para>
        ///     <para>Collection is empty unless you add filters to it.</para>
        /// </remarks>
        public ReportFilterDispatcher FilterCollection { get; } = new ReportFilterDispatcher();

        /// <summary>
        ///     Limit the amount of properties that can be collected per context collection.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Some collections can contain thousands of properties when building collections by reflecting objects. Those
        ///         will take time to process and analyze by the server
        ///         when a lot of reports are uploaded for the same incident. To limit that you can specify a property limit wich
        ///         will make the <see cref="ObjectToContextCollectionConverter" />
        ///         stop after a certain amount of properties (when invoked from within the library).
        ///     </para>
        /// </remarks>
        /// <value>
        ///     Default is 100.
        /// </value>
        public int MaxNumberOfPropertiesPerCollection { get; set; }

        /// <summary>
        ///     Queue reports and upload them in the background.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This option is great if you do not want to freeze the UI while reports are being uploaded. They are queued in
        ///         an internal
        ///         queue until being uploaded in orderly fashion.
        ///     </para>
        /// </remarks>
        public bool QueueReports { get; set; }

        /// <summary>
        ///     The library may throw exceptions if the server cannot be contacted / accept the upload.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Default is <c>true</c>, turn of before going to production.
        ///     </para>
        ///     <para>
        ///         This option is not used when <seealso cref="QueueReports" /> is <c>true</c>.
        ///     </para>
        /// </remarks>
        public bool ThrowExceptions { get; set; }

        /// <summary>
        ///     The objects used to upload reports to the codeRR service.
        /// </summary>
        public IUploadDispatcher Uploaders { get; private set; }

        /// <summary>
        ///     Configure how the reporting UI will interact with the user.
        /// </summary>
        public UserInteractionConfiguration UserInteraction
        {
            get => _userInteraction;
            set => _userInteraction = value;
        }


        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        ///     Configure uploads
        /// </summary>
        /// <param name="oneTrueHost">Host. Host and absolute path to the coderr directory</param>
        /// <param name="appKey">Appkey from the web site</param>
        /// <param name="sharedSecret">Shared secret from the web site</param>
        public void Credentials(Uri oneTrueHost, string appKey, string sharedSecret)
        {
            if (oneTrueHost == null) throw new ArgumentNullException("oneTrueHost");
            if (appKey == null) throw new ArgumentNullException("appKey");
            if (sharedSecret == null) throw new ArgumentNullException("sharedSecret");
            Uploaders.Register(new UploadTocodeRR(oneTrueHost, appKey, sharedSecret));
        }

        /// <summary>
        ///     Dispose pattern.
        /// </summary>
        /// <param name="isDisposing">Invoked from the dispose method.</param>
        protected virtual void Dispose(bool isDisposing)
        {
        }
    }
}