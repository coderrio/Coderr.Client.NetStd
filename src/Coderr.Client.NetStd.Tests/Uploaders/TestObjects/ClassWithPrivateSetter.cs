using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Sdk;

namespace codeRR.Client.NetStd.Tests.Uploaders.TestObjects
{
    public class ClassWithPrivateSetter
    {
        public ClassWithPrivateSetter(string prop)
        {
            Prop = prop;
        }

        protected ClassWithPrivateSetter()
        {
            
        }

        public string Prop { get; private set; }
    }
}
