using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BookFast.Facility.Data
{
    internal static class ConfigurationHelper
    {
        public static string GetConnectionString(string targetEnv)
        {
            var configPath = $"../BookFast/ApplicationParameters/{targetEnv}.xml";
            if (!File.Exists(configPath))
            {
                throw new ArgumentException($"No configuraton exists for target environment '{targetEnv}'. Expected path: {configPath}");
            }

            var doc = XDocument.Parse(File.ReadAllText(configPath));
            var connectionString = (from param in doc.Descendants(XName.Get("Parameter", "http://schemas.microsoft.com/2011/01/fabric"))
                                    let name = param.Attribute("Name")
                                    where name.Value == "Data:DefaultConnection:ConnectionString"
                                    select param.Attribute("Value").Value).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception($"No connection string found for target environment '{targetEnv}'.");
            }

            return connectionString;
        }
    }
}
