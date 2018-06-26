using Newtonsoft.Json;

namespace BookFast.Facility.Data
{
    internal static class Extensions
    {
        public static string ToJson(this string[] array)
        {
            return array != null ? JsonConvert.SerializeObject(array) : null;
        }

        public static string[] ToStringArray(this string json)
        {
            return !string.IsNullOrWhiteSpace(json) ? JsonConvert.DeserializeObject<string[]>(json) : null;
        }
    }
}
