using System;
using Microsoft.Azure.Search;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BookFast.Search.Adapter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("provision", StringComparison.OrdinalIgnoreCase))
            {
                Provision();
            }
            else
            {
                Console.WriteLine("Usage: dotnet run provision");
            }
        }

        private static void Provision()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddUserSecrets<Program>();

            var configuration = builder.Build();
            var searchServiceClient = new SearchServiceClient(configuration["Search:ServiceName"], new SearchCredentials(configuration["Search:AdminKey"]));

            var index = new BookFastIndex(searchServiceClient, configuration);
            index.ProvisionAsync().Wait();
        }
    }
}
