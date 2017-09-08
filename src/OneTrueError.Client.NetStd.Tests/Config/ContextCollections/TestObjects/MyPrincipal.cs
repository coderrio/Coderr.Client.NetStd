using System.Security.Principal;

namespace OneTrueError.Client.NetStd.Tests.ContextCollections
{
    public class MyPrincipal : IPrincipal
    {
        public MyPrincipal(string identityNAme)
        {
            Identity=new MyIdentity(identityNAme);
        }

        public MyPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public IIdentity Identity { get; set; }
    }
}