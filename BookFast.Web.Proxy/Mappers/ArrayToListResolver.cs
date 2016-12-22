using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace BookFast.Web.Proxy.Mappers
{
    internal class ArrayToListResolver : ValueResolver<string[], List<string>>
    {
        protected override List<string> ResolveCore(string[] source) => source != null ? source.ToList() : null;
    }
}
