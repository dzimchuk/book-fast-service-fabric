using BookFast.Files.Contracts.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookFast.Files.Models
{
    public class FileAccessTokenRepresentation
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public AccessPermission AccessPermission { get; set; }
        public string Url { get; set; }
    }
}
