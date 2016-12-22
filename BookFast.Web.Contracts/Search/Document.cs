using System.Collections.Generic;

namespace BookFast.Web.Contracts.Search
{
    public class Document : Dictionary<string, object>
    {
        public Document(IDictionary<string, object> dictionary) : base(dictionary)
        {
        }
    }
}