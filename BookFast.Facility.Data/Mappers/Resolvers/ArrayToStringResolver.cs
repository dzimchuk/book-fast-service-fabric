using AutoMapper;
using Newtonsoft.Json;

namespace BookFast.Facility.Data.Mappers.Resolvers
{
    internal class ArrayToStringResolver : ValueResolver<string[], string>
    {
        protected override string ResolveCore(string[] source) => source == null ? null : JsonConvert.SerializeObject(source);
    }
}