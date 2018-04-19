using System;

namespace Coderr.Client.NetStd.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //var url = new Uri("http://localhost:50473/");
            //Err.Configuration.Credentials(url, 
            //    "2d6a5205227b4535b28ce274fe7b39da", 
            //    "97432021e36b4e04a76a12c81f07ec63");
            //var url = new Uri("http://jonas-hempc/coderr.oss/");
            //Err.Configuration.Credentials(url,
            //    "ae0428b701054c5d9481024f81ad8b05", 
            //    "988cedd2bf4641d1aa228766450fab97");
            var url = new Uri("http://localhost:50473/");
            Err.Configuration.Credentials(url, 
                "ae0428b701054c5d9481024f81ad8b05", 
                "988cedd2bf4641d1aa228766450fab97");
            try
            {
                throw new NotSupportedException("Not invented here");
                //throw new ArgumentException("Bajs");
            }
            catch (Exception ex)
            {
                Err.Report(ex, new { myData = "hello", ErrTags = "important" });
            }
            Console.WriteLine("Hello World!");
        }
    }
}