using System;

namespace Coderr.Client.NetStd
{
    /// <summary>
    ///     Exception thrown in the codeRR library
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         We should not try to upload these.
    ///     </para>
    /// </remarks>
    public class CoderrClientException : Exception
    {
        public CoderrClientException(string msg) : base(msg)
        {
        }

        public CoderrClientException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}