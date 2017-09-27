using System;

namespace codeRR.Client.ContextCollections.Providers
{
    /// <summary>
    ///     Use to indicate which providers are added into the collection process per default
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultProviderAttribute : Attribute
    {
    }
}