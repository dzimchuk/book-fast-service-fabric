using System;

namespace BookFast.Web.Contracts.Files
{
    [Flags]
    public enum AccessPermission
    {
        Read = 1,
        Write = 2,
        Delete = 4
    }
}
