using AutoMapper;
using Newtonsoft.Json;
using System;

namespace BookFast.Facility.Data.Mappers.Resolvers
{
    internal class StringToArrayResolver : ValueResolver<string, string[]>
    {
        protected override string[] ResolveCore(string source) => string.IsNullOrWhiteSpace(source) ? null : JsonConvert.DeserializeObject<string[]>(source);
    }
}
