using BookFast.Files.Contracts.Models;
using System;

namespace BookFast.Files.Business.Data
{
    public interface ISASTokenProvider
    {
        string GetUrlWithAccessToken(string path, AccessPermission permission, DateTimeOffset expirationTime);
    }
}
