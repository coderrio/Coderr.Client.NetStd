using Coderr.Client.NetStd.Reporters;

namespace Coderr.Client.NetStd.Config
{
    /// <summary>
    /// Used to be able to process exceptions before they are converted into DTOs
    /// </summary>
    /// <param name="context">context info</param>
    /// <seealso cref="CoderrConfiguration.ExceptionPreProcessor"/>
    public delegate void ExceptionPreProcessorHandler(IErrorReporterContext context);
}