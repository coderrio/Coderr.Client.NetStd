using System;

namespace OneTrueError.Client.NetStd.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:64707/");
            OneTrue.Configuration.Credentials(uri,
                "2acaa7e303e0445295e03d5f35182b33",
                "6892bb2907b7498dbe6c5c540335765a");

            try
            {
                throw new NotSupportedException("Not invented here");
            }
            catch (Exception ex)
            {
                OneTrue.Report(ex);
            }
            Console.WriteLine("Hello World!");
        }
    }
}