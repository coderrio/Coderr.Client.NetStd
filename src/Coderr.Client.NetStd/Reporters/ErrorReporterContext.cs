using System;
using System.Collections.Generic;
using codeRR.Client.Contracts;

namespace codeRR.Client.Reporters
{
    /// <summary>
    ///     Context supplied by error reports
    /// </summary>
    /// <remarks>
    ///     Used to be able to provide app specific context information (for instance HTTP apps can provide the HTTP
    ///     context)
    /// </remarks>
    public class ErrorReporterContext : IErrorReporterContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ErrorReporterContext" /> class.
        /// </summary>
        /// <param name="reporter">The reporter.</param>
        /// <param name="exception">The exception.</param>
        /// <exception cref="ArgumentNullException">exception</exception>
        public ErrorReporterContext(object reporter, Exception exception)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
            Reporter = reporter;
            ContextCollections = new List<ContextCollectionDTO>();

            MoveCollectionsInException(exception, ContextCollections);
        }

        /// <inheritdoc />
        public IList<ContextCollectionDTO> ContextCollections { get; }

        /// <summary>
        ///     Gets class which is sending the report
        /// </summary>
        /// <remarks>
        /// <para>
        /// 
        /// </para>
        /// </remarks>
        public object Reporter { get; }


        /// <summary>
        ///     Gets caught exception
        /// </summary>
        public Exception Exception { get; }


        /// <summary>
        ///     Can be used to copy collections from an exception to a collection collection ;)
        /// </summary>
        /// <param name="exception">Exception that might contain collections</param>
        /// <param name="destination">target</param>
        public static void MoveCollectionsInException(Exception exception, IList<ContextCollectionDTO> destination)
        {
            var keysToRemove = new List<object>();
            foreach (var key in exception.Data.Keys)
            {
                var keyStr = key?.ToString();
                if (key == null || !keyStr.StartsWith("Err."))
                    continue;

                keysToRemove.Add(key);
                var collection = (ContextCollectionDTO)exception.Data[key];
                destination.Add(collection);
            }

            foreach (var key in keysToRemove)
                exception.Data.Remove(key);
        }
    }
}