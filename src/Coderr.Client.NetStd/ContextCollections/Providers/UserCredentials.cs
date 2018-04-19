using System;
using System.Collections.Concurrent;
using System.Security.Principal;
using Coderr.Client.Contracts;

namespace Coderr.Client.ContextCollections.Providers
{
    /// <summary>
    ///     Carries the user credentials to the server.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Creates a context collection with the name <c>UserCredentials</c> with the properties <c>UserName</c> and if
    ///         available <c>DomainName</c>.
    ///     </para>
    /// </remarks>
    public sealed class UserCredentials : ContextCollectionDTO
    {
        /// <summary>
        ///     Creates a new instance of <see cref="UserCredentials" />.
        /// </summary>
        /// <param name="identity">
        ///     Identity, typically from <c>Thread.CurrentPrincipal.Identity</c> or <c>Request.User.Identity</c>
        ///     .
        /// </param>
        public UserCredentials(IIdentity identity) : base("UserCredentials")
        {
            if (identity == null) throw new ArgumentNullException("identity");
            Properties = new ConcurrentDictionary<string, string>();

            if (string.IsNullOrEmpty(identity.Name))
                Properties.Add("UserName", "[Anonynmous]");
            else
                SplitAccountName(identity.Name);
        }

        /// <summary>
        ///     Creates a new instance of <see cref="UserCredentials" />.
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="domainName">Domain name</param>
        public UserCredentials(string domainName, string userName) : base("UserCredentials")
        {
            DomainName = userName ?? throw new ArgumentNullException(nameof(userName));
            DomainName = domainName ?? throw new ArgumentNullException(nameof(domainName));
        }

        /// <summary>
        ///     Creates a new instance of <see cref="UserCredentials" />.
        /// </summary>
        /// <param name="userName">User name without domain (i.e. should not include "domainName\")</param>
        public UserCredentials(string userName) : base("UserCredentials")
        {
            SplitAccountName(userName);
        }

        /// <summary>
        /// Domain name (if any)
        /// </summary>
        public string DomainName
        {
            get => Properties["DomainName"];
            set => Properties["DomainName"] = value;
        }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName
        {
            get => Properties["UserName"];
            set => Properties["UserName"] = value;
        }


        private void SplitAccountName(string accountName)
        {
            if (accountName == null) throw new ArgumentNullException(nameof(accountName));

            var pos = accountName.IndexOf("\\", StringComparison.Ordinal);
            if (pos != -1)
            {
                DomainName = accountName.Substring(0, pos);
                UserName = accountName.Substring(pos + 1);
            }
            else
            {
                UserName = accountName;
            }
        }
    }
}