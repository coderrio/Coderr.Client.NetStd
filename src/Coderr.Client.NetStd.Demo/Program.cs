using System;

namespace codeRR.Client.NetStd.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("http://ote-buildagent1.westeurope.cloudapp.azure.com/coderr/");
            Err.Configuration.Credentials(uri,
                "52cbfd2f2aae49d4aa68c6cce1f484a6",
                "2922ba4a0db24b12b97055a83f84ffd3");
            //var uri = new Uri("http://localhost:50473/");
            //Err.Configuration.Credentials(uri,
            //    "5f219f356daa40b3b31dfc67514df6d6",
            //    "22612e4444f347d1bb3d841d64c9750a");


            try
            {
                throw new NotSupportedException("Not invented here");
            }
            catch (Exception ex)
            {
                Err.Report(ex);
            }
            Console.WriteLine("Hello World!");
        }
    }
}