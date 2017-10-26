using System;

namespace codeRR.Client.NetStd.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = new Uri("http://localhost:50473/");
            Err.Configuration.Credentials(url,
                "13c1e4e3a01a44c39f64176746c3f49d",
                "6680fca365684d6ba978c5b0f649543e");

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