using System;

namespace BookFast.Files.Contracts.Models
{
    [Flags]
    public enum AccessPermission
    {
        Read = 1,
        Write = 2,
        Delete = 4
    }
}
